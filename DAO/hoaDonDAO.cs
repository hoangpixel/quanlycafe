using MySql.Data.MySqlClient;
using DAO.CONFIG;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;

namespace DAO
{
    public class hoaDonDAO
    {
        // 1. LẤY DANH SÁCH HÓA ĐƠN
        public BindingList<hoaDonDTO> LayDanhSach()
        {
            BindingList<hoaDonDTO> ds = new BindingList<hoaDonDTO>();
            string qry = @"
                SELECT 
                    hd.MAHOADON, 
                    hd.MABAN, 
                    hd.THOIGIANTAO, 
                    hd.TRANGTHAI, 
                    hd.TONGTIEN
                FROM hoadon hd
                ORDER BY hd.MAHOADON DESC";

            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    hoaDonDTO hd = new hoaDonDTO
                    {
                        MaHD = reader.GetInt32("MAHOADON"),
                        MaBan = reader.GetString("MABAN"), // VARCHAR
                        ThoiGianTao = reader.GetDateTime("THOIGIANTAO"),
                        TrangThai = reader.GetString("TRANGTHAI"),
                        TongTien = reader.GetDecimal("TONGTIEN")
                    };
                    ds.Add(hd);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách hóa đơn: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }

        // 2. THÊM HÓA ĐƠN + CHI TIẾT (CÓ TRANSACTION)
        public int Them(hoaDonDTO hd, List<gioHangItemDTO> gioHang)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            MySqlTransaction tran = null;
            int maHD = 0;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                // Bước 1: Thêm hóa đơn
                string qryHD = @"
                    INSERT INTO hoadon (MABAN, MANV, TRANGTHAI, TONGTIEN) 
                    VALUES (@MaBan, @MaNV, 'Chưa tính tiền', @TongTien);
                    SELECT LAST_INSERT_ID();";

                MySqlCommand cmdHD = new MySqlCommand(qryHD, conn, tran);
                cmdHD.Parameters.AddWithValue("@MaBan", hd.MaBan);
                cmdHD.Parameters.AddWithValue("@MaNV", 1); // TODO: Lấy từ đăng nhập
                cmdHD.Parameters.AddWithValue("@TongTien", hd.TongTien);

                maHD = Convert.ToInt32(cmdHD.ExecuteScalar());

                // Bước 2: Thêm chi tiết hóa đơn
                string qryCT = @"
                    INSERT INTO cthd (MAHOADON, MASANPHAM, SOLUONG, DONGIA) 
                    VALUES (@MaHD, @MaSP, @SoLuong, @DonGia)";

                foreach (var item in gioHang)
                {
                    MySqlCommand cmdCT = new MySqlCommand(qryCT, conn, tran);
                    cmdCT.Parameters.AddWithValue("@MaHD", maHD);
                    cmdCT.Parameters.AddWithValue("@MaSP", item.SanPham.MaSP);
                    cmdCT.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                    cmdCT.Parameters.AddWithValue("@DonGia", item.SanPham.Gia);
                    cmdCT.ExecuteNonQuery();
                }

                tran.Commit();
                Console.WriteLine($"Thêm hóa đơn {maHD} thành công!");
            }
            catch (MySqlException ex)
            {
                if (tran != null) tran.Rollback();
                Console.WriteLine("Lỗi thêm hóa đơn: " + ex.Message);
                maHD = 0;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return maHD;
        }

        // 3. CẬP NHẬT TRẠNG THÁI HÓA ĐƠN
        public bool CapNhatTrangThai(int maHD, string trangThai)
        {
            string qry = "UPDATE hoadon SET TRANGTHAI = @TrangThai WHERE MAHOADON = @MaHD";
            MySqlConnection conn = DBConnect.GetConnection();

            try
            {
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                cmd.Parameters.AddWithValue("@MaHD", maHD);
                int rs = cmd.ExecuteNonQuery();

                Console.WriteLine($"Cập nhật trạng thái hóa đơn {maHD} → {trangThai}");
                return rs > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi cập nhật trạng thái: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        // 4. XÓA HÓA ĐƠN (ẨN HOẶC XÓA THẬT – TÙY YÊU CẦU)
        public bool Xoa(int maHD)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            MySqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                // Xóa chi tiết trước
                string qryCT = "DELETE FROM cthd WHERE MAHOADON = @MaHD";
                MySqlCommand cmdCT = new MySqlCommand(qryCT, conn, tran);
                cmdCT.Parameters.AddWithValue("@MaHD", maHD);
                cmdCT.ExecuteNonQuery();

                // Xóa hóa đơn
                string qryHD = "DELETE FROM hoadon WHERE MAHOADON = @MaHD";
                MySqlCommand cmdHD = new MySqlCommand(qryHD, conn, tran);
                cmdHD.Parameters.AddWithValue("@MaHD", maHD);
                cmdHD.ExecuteNonQuery();

                tran.Commit();
                Console.WriteLine($"Xóa hóa đơn {maHD} thành công!");
                return true;
            }
            catch (MySqlException ex)
            {
                if (tran != null) tran.Rollback();
                Console.WriteLine("Lỗi xóa hóa đơn: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        // 5. TÌM HÓA ĐƠN THEO MÃ
        public hoaDonDTO TimTheoMa(int maHD)
        {
            hoaDonDTO hd = null;
            string qry = "SELECT * FROM hoadon WHERE MAHOADON = @MaHD";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@MaHD", maHD);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        hd = new hoaDonDTO
                        {
                            MaHD = reader.GetInt32("MAHOADON"),
                            MaBan = reader.GetString("MABAN"),
                            ThoiGianTao = reader.GetDateTime("THOIGIANTAO"),
                            TrangThai = reader.GetString("TRANGTHAI"),
                            TongTien = reader.GetDecimal("TONGTIEN")
                        };
                    }
                }
            }

            return hd;
        }
    }
}