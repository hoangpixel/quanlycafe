using MySql.Data.MySqlClient;
using DAO.CONFIG;
using DTO;
using System;
using System.Collections.Generic;

namespace DAO
{
    public class thongKeDAO
    {
        private static thongKeDAO instance;
        public static thongKeDAO Instance
        {
            get { if (instance == null) instance = new thongKeDAO(); return instance; }
        }
        private thongKeDAO() { }

        // ====================================================================
        // PHẦN 1: DOANH THU
        // ====================================================================

        // 1.1 Chi tiết theo ngày (Dùng cho: Theo Ngày, Theo Quý)
        public List<thongKeDTO> GetDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            List<thongKeDTO> ds = new List<thongKeDTO>();
            MySqlConnection conn = null;
            string qry = "SELECT DATE(THOIGIANTAO) as Ngay, SUM(TONGTIEN) as TongTien " +
                         "FROM hoadon WHERE TRANGTHAIXOA = 1 " +
                         "AND THOIGIANTAO >= @tuNgay AND THOIGIANTAO <= @denNgay " +
                         "GROUP BY DATE(THOIGIANTAO) ORDER BY DATE(THOIGIANTAO) ASC";
            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@denNgay", denNgay);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ds.Add(new thongKeDTO
                    {
                        Nhan = reader.GetDateTime("Ngay").ToString("dd/MM"),
                        GiaTri = reader.IsDBNull(reader.GetOrdinal("TongTien")) ? 0 : reader.GetDecimal("TongTien")
                    });
                }
                reader.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { DBConnect.CloseConnection(conn); }
            return ds;
        }

        // 1.2 Theo Năm (Trả về cố định 12 tháng)
        public List<thongKeDTO> GetDoanhThuTheoThang(int nam)
        {
            List<thongKeDTO> ds = new List<thongKeDTO>();
            for (int i = 1; i <= 12; i++) ds.Add(new thongKeDTO { Nhan = "Tháng " + i, GiaTri = 0 });

            MySqlConnection conn = null;
            string qry = "SELECT MONTH(THOIGIANTAO) as Thang, SUM(TONGTIEN) as TongTien " +
                         "FROM hoadon WHERE TRANGTHAIXOA = 1 AND YEAR(THOIGIANTAO) = @nam " +
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
                    if (thang >= 1 && thang <= 12) ds[thang - 1].GiaTri = reader.GetDecimal("TongTien");
                }
                reader.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { DBConnect.CloseConnection(conn); }
            return ds;
        }

        // 1.3 Theo Khoảng nhưng Gom Nhóm Tháng (Dùng cho: Theo Tháng - xem nhiều tháng)
        public List<thongKeDTO> GetDoanhThuTheoKhoang_GroupThang(DateTime tuNgay, DateTime denNgay)
        {
            List<thongKeDTO> ds = new List<thongKeDTO>();
            MySqlConnection conn = null;
            string qry = "SELECT CONCAT(MONTH(THOIGIANTAO), '/', YEAR(THOIGIANTAO)) as Nhan, SUM(TONGTIEN) as TongTien " +
                         "FROM hoadon WHERE TRANGTHAIXOA = 1 " +
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
                    ds.Add(new thongKeDTO
                    {
                        Nhan = reader.GetString("Nhan"),
                        GiaTri = reader.GetDecimal("TongTien")
                    });
                }
                reader.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { DBConnect.CloseConnection(conn); }
            return ds;
        }

        // ====================================================================
        // PHẦN 2: CHI TIÊU (Logic tương tự Doanh thu, chỉ đổi bảng phieunhap)
        // ====================================================================

        public List<thongKeDTO> GetChiTieu(DateTime tuNgay, DateTime denNgay)
        {
            List<thongKeDTO> ds = new List<thongKeDTO>();
            MySqlConnection conn = null;
            string qry = "SELECT DATE(THOIGIAN) as Ngay, SUM(TONGTIEN) as TongTien " +
                         "FROM phieunhap WHERE TRANGTHAIXOA = 1 " +
                         "AND THOIGIAN >= @tuNgay AND THOIGIAN <= @denNgay " +
                         "GROUP BY DATE(THOIGIAN) ORDER BY DATE(THOIGIAN) ASC";
            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@denNgay", denNgay);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ds.Add(new thongKeDTO
                    {
                        Nhan = reader.GetDateTime("Ngay").ToString("dd/MM"),
                        GiaTri = reader.IsDBNull(reader.GetOrdinal("TongTien")) ? 0 : reader.GetDecimal("TongTien")
                    });
                }
                reader.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { DBConnect.CloseConnection(conn); }
            return ds;
        }

        public List<thongKeDTO> GetChiTieuTheoThang(int nam)
        {
            List<thongKeDTO> ds = new List<thongKeDTO>();
            for (int i = 1; i <= 12; i++) ds.Add(new thongKeDTO { Nhan = "Tháng " + i, GiaTri = 0 });

            MySqlConnection conn = null;
            string qry = "SELECT MONTH(THOIGIAN) as Thang, SUM(TONGTIEN) as TongTien " +
                         "FROM phieunhap WHERE TRANGTHAIXOA = 1 AND YEAR(THOIGIAN) = @nam " +
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
                    if (thang >= 1 && thang <= 12) ds[thang - 1].GiaTri = reader.GetDecimal("TongTien");
                }
                reader.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { DBConnect.CloseConnection(conn); }
            return ds;
        }

        public List<thongKeDTO> GetChiTieuTheoKhoang_GroupThang(DateTime tuNgay, DateTime denNgay)
        {
            List<thongKeDTO> ds = new List<thongKeDTO>();
            MySqlConnection conn = null;
            string qry = "SELECT CONCAT(MONTH(THOIGIAN), '/', YEAR(THOIGIAN)) as Nhan, SUM(TONGTIEN) as TongTien " +
                         "FROM phieunhap WHERE TRANGTHAIXOA = 1 " +
                         "AND THOIGIAN >= @tuNgay AND THOIGIAN <= @denNgay " +
                         "GROUP BY YEAR(THOIGIAN), MONTH(THOIGIAN) " +
                         "ORDER BY YEAR(THOIGIAN) ASC, MONTH(THOIGIAN) ASC";
            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@denNgay", denNgay);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ds.Add(new thongKeDTO
                    {
                        Nhan = reader.GetString("Nhan"),
                        GiaTri = reader.GetDecimal("TongTien")
                    });
                }
                reader.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { DBConnect.CloseConnection(conn); }
            return ds;
        }

        // ====================================================================
        // PHẦN 3: LƯƠNG
        // ====================================================================
        public decimal GetTongLuong()
        {
            decimal tongLuong = 0;
            MySqlConnection conn = null;
            string qry = "SELECT SUM(LUONG) FROM nhanvien WHERE TRANGTHAI = 1";
            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value) tongLuong = Convert.ToDecimal(result);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { DBConnect.CloseConnection(conn); }
            return tongLuong;
        }

        public List<hoaDonDTO> GetDanhSachHoaDon(DateTime tuNgay, DateTime denNgay)
        {
            List<hoaDonDTO> ds = new List<hoaDonDTO>();
            MySqlConnection conn = null;

            string qry = "SELECT * FROM hoadon WHERE TRANGTHAIXOA = 1 " +
                         "AND THOIGIANTAO >= @tuNgay AND THOIGIANTAO <= @denNgay " +
                         "ORDER BY THOIGIANTAO DESC";
            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@denNgay", denNgay);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    hoaDonDTO item = new hoaDonDTO();
                    item.MaHD = reader.GetInt32("MAHOADON");
                    item.MaNhanVien = reader.GetInt32("MANHANVIEN");
                    if (!reader.IsDBNull(reader.GetOrdinal("MAKHACHHANG")))
                    {
                        item.MaKhachHang = reader.GetInt32("MAKHACHHANG");
                    }
                    else
                    {
                        item.MaKhachHang = 0;
                    }
                    item.ThoiGianTao = reader.GetDateTime("THOIGIANTAO");
                    item.TongTien = reader.GetDecimal("TONGTIEN");

                    ds.Add(item);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi lấy danh sách hóa đơn: " + ex.Message);
            }
            finally { DBConnect.CloseConnection(conn); }

            return ds;
        }

        public List<phieuNhapDTO> GetDanhSachPhieuNhap(DateTime tuNgay, DateTime denNgay)
        {
            List<phieuNhapDTO> ds = new List<phieuNhapDTO>();
            MySqlConnection conn = null;
            string qry = "SELECT * FROM phieunhap WHERE TRANGTHAIXOA = 1 " +
                         "AND THOIGIAN >= @tuNgay AND THOIGIAN <= @denNgay " +
                         "ORDER BY THOIGIAN DESC";
            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@denNgay", denNgay);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    phieuNhapDTO item = new phieuNhapDTO();
                    item.MaPN = reader.GetInt32("MAPN");
                    item.MaNCC = reader.GetInt32("MANCC");
                    item.MaNhanVien = reader.GetInt32("MANHANVIEN");
                    item.ThoiGian = reader.GetDateTime("THOIGIAN");
                    item.TongTien = reader.GetDecimal("TONGTIEN");

                    ds.Add(item);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi lấy danh sách phiếu nhập: " + ex.Message);
            }
            finally { DBConnect.CloseConnection(conn); }

            return ds;
        }

        // Thêm vào thongKeDAO.cs
        // Trong thongKeDAO.cs

        public List<topSanPhamDTO> GetTopSanPhamBanChay(DateTime tuNgay, DateTime denNgay)
        {
            List<topSanPhamDTO> ds = new List<topSanPhamDTO>();
            MySqlConnection conn = null;

            // --- CẬP NHẬT SQL THEO BẢNG CTHD ---
            // 1. Dùng bảng 'cthd' thay vì 'chitiethoadon'
            // 2. Lấy cột THANHTIEN trực tiếp
            string qry = "SELECT sp.MASANPHAM, sp.TENSANPHAM, " +
                         "SUM(ct.SOLUONG) as TongSoLuong, " +
                         "SUM(ct.THANHTIEN) as TongDoanhThu " +
                         "FROM cthd ct " +
                         "JOIN hoadon hd ON ct.MAHOADON = hd.MAHOADON " +
                         "JOIN sanpham sp ON ct.MASANPHAM = sp.MASANPHAM " +
                         "WHERE hd.TRANGTHAIXOA = 1 " +
                         "AND hd.THOIGIANTAO >= @tuNgay AND hd.THOIGIANTAO <= @denNgay " +
                         "GROUP BY sp.MASANPHAM, sp.TENSANPHAM " +
                         "ORDER BY TongSoLuong DESC";

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@denNgay", denNgay);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    topSanPhamDTO item = new topSanPhamDTO();

                    // Map dữ liệu
                    item.MaSP = reader.GetInt32("MASANPHAM");
                    item.TenSP = reader.GetString("TENSANPHAM");

                    // Lấy tổng số lượng
                    if (!reader.IsDBNull(reader.GetOrdinal("TongSoLuong")))
                        item.SoLuongBan = Convert.ToInt32(reader.GetDecimal("TongSoLuong"));

                    // Lấy tổng doanh thu
                    if (!reader.IsDBNull(reader.GetOrdinal("TongDoanhThu")))
                        item.DoanhThu = reader.GetDecimal("TongDoanhThu");

                    ds.Add(item);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi lấy Top SP: " + ex.Message);
            }
            finally { DBConnect.CloseConnection(conn); }
            return ds;
        }
    }
}