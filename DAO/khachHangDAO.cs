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
                string query = "SELECT * FROM khachhang WHERE TRANGTHAI = 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    khachHangDTO kh = new khachHangDTO();
                    kh.MaKhachHang = Convert.ToInt32(reader["MAKHACHHANG"]);
                    kh.TenKhachHang = reader["TENKHACHHANG"].ToString();
                    kh.SoDienThoai = reader["SODIENTHOAI"].ToString();
                    kh.Email = reader["EMAIL"].ToString();
                    kh.TrangThai = Convert.ToInt32(reader["TRANGTHAI"]);

                    kh.NgayTao = Convert.ToDateTime(reader["NGAYTAO"]);
                    list.Add(kh);
                }
            }
            catch (Exception ex) { throw new Exception("Lỗi: " + ex.Message); }
            finally { DBConnect.CloseConnection(conn); }
            return list;
        }

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

        public int layMa()
        {
            MySqlConnection conn = DBConnect.GetConnection();
            int maKH = 0;
            try
            {
                string qry = "SELECT IFNULL(MAX(MAKHACHHANG), 0) FROM khachhang";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                maKH = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy mã KH : " + ex);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return maKH + 1;
        }

        public bool Xoa(int ma)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = "UPDATE khachhang SET TRANGTHAI = 0 WHERE MAKHACHHANG = @Ma";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ma", ma);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }
    }
}