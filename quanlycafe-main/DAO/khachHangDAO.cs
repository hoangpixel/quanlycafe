using DAO.CONFIG;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;

namespace DAO
{
    public class khachHangDAO
    {
        public BindingList<khachHangDTO> LayDanhSach()
        {
            BindingList<khachHangDTO> list = new BindingList<khachHangDTO>();
            MySqlConnection conn = DBConnect.GetConnection();

            try
            {
                string query = "SELECT * FROM khachhang";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    khachHangDTO kh = new khachHangDTO();
                    kh.MaKhachHang = Convert.ToInt32(reader["MAKHACHHANG"]);
                    kh.TenKhachHang = reader["TENKHACHHANG"].ToString();
                    kh.SoDienThoai = reader["SODIENTHOAI"].ToString();
                    kh.Email = reader["EMAIL"].ToString();

                    // --- QUAN TRỌNG: Đổi thành ToByte ---
                    kh.TrangThai = Convert.ToByte(reader["TRANGTHAI"]);

                    kh.NgayTao = Convert.ToDateTime(reader["NGAYTAO"]);
                    list.Add(kh);
                }
            }
            catch (Exception ex) { throw new Exception("Lỗi: " + ex.Message); }
            finally { DBConnect.CloseConnection(conn); }
            return list;
        }

        // Các hàm Thêm, Sửa, Xóa giữ nguyên logic, 
        // MySQL Connector tự động hiểu byte khi truyền vào parameter
        public bool Them(khachHangDTO kh)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = "INSERT INTO khachhang (TENKHACHHANG, SODIENTHOAI, EMAIL, TRANGTHAI) VALUES (@Ten, @Sdt, @Email, @TrangThai)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ten", kh.TenKhachHang);
                cmd.Parameters.AddWithValue("@Sdt", kh.SoDienThoai);
                if (string.IsNullOrEmpty(kh.Email)) cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                else cmd.Parameters.AddWithValue("@Email", kh.Email);
                cmd.Parameters.AddWithValue("@TrangThai", kh.TrangThai);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }

        public bool Sua(khachHangDTO kh)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = "UPDATE khachhang SET TENKHACHHANG = @Ten, SODIENTHOAI = @Sdt, EMAIL = @Email, TRANGTHAI = @TrangThai WHERE MAKHACHHANG = @Ma";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ten", kh.TenKhachHang);
                cmd.Parameters.AddWithValue("@Sdt", kh.SoDienThoai);
                if (string.IsNullOrEmpty(kh.Email)) cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                else cmd.Parameters.AddWithValue("@Email", kh.Email);
                cmd.Parameters.AddWithValue("@TrangThai", kh.TrangThai);
                cmd.Parameters.AddWithValue("@Ma", kh.MaKhachHang);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }

        // (Giữ nguyên các hàm Xóa, KiemTraTrung... như cũ)
        public bool Xoa(int ma)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = "DELETE FROM khachhang WHERE MAKHACHHANG = @Ma";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ma", ma);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }

        public bool KiemTraTrungSDT(string sdt, int maLoaiTru = -1)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = "SELECT COUNT(*) FROM khachhang WHERE SODIENTHOAI = @Sdt AND MAKHACHHANG != @Ma";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Sdt", sdt);
                cmd.Parameters.AddWithValue("@Ma", maLoaiTru);
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }

        public bool KiemTraTrungEmail(string email, int maLoaiTru = -1)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = "SELECT COUNT(*) FROM khachhang WHERE EMAIL = @Email AND MAKHACHHANG != @Ma";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Ma", maLoaiTru);
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }
    }
}