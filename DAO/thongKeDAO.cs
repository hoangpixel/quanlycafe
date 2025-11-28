using MySql.Data.MySqlClient;
using DAO.CONFIG;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAO
{
    public class thongKeDAO
    {
        private static thongKeDAO instance;

        public static thongKeDAO Instance
        {
            get
            {
                if (instance == null) instance = new thongKeDAO();
                return instance;
            }
        }

        private thongKeDAO() { }

        // ==========================================================
        // 1. DOANH THU: THEO NGÀY (Dùng cho xem Tùy chỉnh/Tháng/Quý)
        // ==========================================================
        public List<thongKeDTO> GetDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            List<thongKeDTO> ds = new List<thongKeDTO>();
            MySqlConnection conn = null;
            string qry = "SELECT DATE(THOIGIANTAO) as Ngay, SUM(TONGTIEN) as TongTien " +
                         "FROM hoadon " +
                         "WHERE TRANGTHAIXOA = 1 " +
                         "AND THOIGIANTAO >= @tuNgay AND THOIGIANTAO <= @denNgay " +
                         "GROUP BY DATE(THOIGIANTAO) " +
                         "ORDER BY DATE(THOIGIANTAO) ASC";

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@denNgay", denNgay);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    thongKeDTO item = new thongKeDTO();
                    DateTime date = reader.GetDateTime("Ngay");
                    item.Nhan = date.ToString("dd/MM"); // Nhãn là ngày/tháng
                    item.GiaTri = reader.IsDBNull(reader.GetOrdinal("TongTien")) ? 0 : reader.GetDecimal("TongTien");
                    ds.Add(item);
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy doanh thu theo ngày: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return ds;
        }

        // ==========================================================
        // 2. DOANH THU: THEO THÁNG (Dùng cho xem Năm - 12 cột)
        // ==========================================================
        public List<thongKeDTO> GetDoanhThuTheoThang(int nam)
        {
            List<thongKeDTO> ds = new List<thongKeDTO>();

            // Khởi tạo sẵn 12 tháng bằng 0 để biểu đồ đẹp
            for (int i = 1; i <= 12; i++)
            {
                ds.Add(new thongKeDTO { Nhan = "Tháng " + i, GiaTri = 0 });
            }

            MySqlConnection conn = null;
            string qry = "SELECT MONTH(THOIGIANTAO) as Thang, SUM(TONGTIEN) as TongTien " +
                         "FROM hoadon " +
                         "WHERE TRANGTHAIXOA = 1 AND YEAR(THOIGIANTAO) = @nam " +
                         "GROUP BY MONTH(THOIGIANTAO)";

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@nam", nam);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int thang = reader.GetInt32("Thang");
                    decimal tien = reader.GetDecimal("TongTien");

                    // Gán tiền vào đúng tháng trong list (Index = Tháng - 1)
                    if (thang >= 1 && thang <= 12)
                    {
                        ds[thang - 1].GiaTri = tien;
                    }
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy doanh thu theo tháng: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return ds;
        }

        // Trong class thongKeDAO

        // Hàm mới: Lấy doanh thu theo khoảng thời gian nhưng GOM NHÓM THEO THÁNG
        public List<thongKeDTO> GetDoanhThuTheoKhoang_GroupThang(DateTime tuNgay, DateTime denNgay)
        {
            List<thongKeDTO> ds = new List<thongKeDTO>();
            MySqlConnection conn = null;

            // SQL: Group by Tháng và Năm
            // CONCAT(MONTH, '/', YEAR) để ra nhãn dạng "8/2025", "9/2025"
            string qry = "SELECT CONCAT(MONTH(THOIGIANTAO), '/', YEAR(THOIGIANTAO)) as Nhan, " +
                         "SUM(TONGTIEN) as TongTien " +
                         "FROM hoadon " +
                         "WHERE TRANGTHAIXOA = 1 " +
                         "AND THOIGIANTAO >= @tuNgay AND THOIGIANTAO <= @denNgay " +
                         "GROUP BY YEAR(THOIGIANTAO), MONTH(THOIGIANTAO) " +
                         "ORDER BY YEAR(THOIGIANTAO) ASC, MONTH(THOIGIANTAO) ASC";

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@denNgay", denNgay);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    thongKeDTO item = new thongKeDTO();
                    item.Nhan = reader.GetString("Nhan"); // "8/2025"
                    item.GiaTri = reader.GetDecimal("TongTien");
                    ds.Add(item);
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy doanh thu group tháng: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return ds;
        }

        // ==========================================================
        // 3. CHI TIÊU: THEO NGÀY (Dùng cho xem Tùy chỉnh/Tháng/Quý)
        // ==========================================================
        public List<thongKeDTO> GetChiTieu(DateTime tuNgay, DateTime denNgay)
        {
            List<thongKeDTO> ds = new List<thongKeDTO>();
            MySqlConnection conn = null;
            string qry = "SELECT DATE(THOIGIAN) as Ngay, SUM(TONGTIEN) as TongTien " +
                         "FROM phieunhap " +
                         "WHERE TRANGTHAIXOA = 1 " +
                         "AND THOIGIAN >= @tuNgay AND THOIGIAN <= @denNgay " +
                         "GROUP BY DATE(THOIGIAN) " +
                         "ORDER BY DATE(THOIGIAN) ASC";

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@denNgay", denNgay);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    thongKeDTO item = new thongKeDTO();
                    DateTime date = reader.GetDateTime("Ngay");
                    item.Nhan = date.ToString("dd/MM");
                    item.GiaTri = reader.IsDBNull(reader.GetOrdinal("TongTien")) ? 0 : reader.GetDecimal("TongTien");
                    ds.Add(item);
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy chi tiêu: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return ds;
        }

        // ==========================================================
        // [MỚI] 3.5. CHI TIÊU: THEO THÁNG (Dùng cho xem Năm - 12 cột)
        // ==========================================================
        public List<thongKeDTO> GetChiTieuTheoThang(int nam)
        {
            List<thongKeDTO> ds = new List<thongKeDTO>();

            // Tạo sẵn 12 tháng
            for (int i = 1; i <= 12; i++)
            {
                ds.Add(new thongKeDTO { Nhan = "Tháng " + i, GiaTri = 0 });
            }

            MySqlConnection conn = null;
            string qry = "SELECT MONTH(THOIGIAN) as Thang, SUM(TONGTIEN) as TongTien " +
                         "FROM phieunhap " +
                         "WHERE TRANGTHAIXOA = 1 AND YEAR(THOIGIAN) = @nam " +
                         "GROUP BY MONTH(THOIGIAN)";

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@nam", nam);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int thang = reader.GetInt32("Thang");
                    decimal tien = reader.GetDecimal("TongTien");

                    if (thang >= 1 && thang <= 12)
                    {
                        ds[thang - 1].GiaTri = tien;
                    }
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy chi tiêu theo tháng: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return ds;
        }

        // ==========================================================
        // 4. LẤY TỔNG LƯƠNG NHÂN VIÊN
        // ==========================================================
        public decimal GetTongLuong()
        {
            decimal tongLuong = 0;
            MySqlConnection conn = null;
            string qry = "SELECT SUM(LUONG) FROM nhanvien";

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);

                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    tongLuong = Convert.ToDecimal(result);
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy tổng lương: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return tongLuong;
        }
    }
}