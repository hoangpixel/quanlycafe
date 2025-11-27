using DAO.CONFIG; 
using DTO;
using MySql.Data.MySqlClient; 
using System;
using System.ComponentModel;

namespace DAO
{
    public class nhaCungCapDAO
    {
        // 1. Lấy danh sách
        public BindingList<nhaCungCapDTO> LayDanhSach()
        {
            BindingList<nhaCungCapDTO> list = new BindingList<nhaCungCapDTO>();
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = "SELECT * FROM nhacungcap";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    nhaCungCapDTO ncc = new nhaCungCapDTO();
                    ncc.MaNCC = Convert.ToInt32(reader["MANCC"]);
                    ncc.TenNCC = reader["TENNCC"].ToString();
                    ncc.SoDienThoai = reader["SODIENTHOAI"].ToString();
                    ncc.Email = reader["EMAIL"].ToString();
                    ncc.DiaChi = reader["DIACHI"].ToString();
                    ncc.ConHoatDong = Convert.ToInt32(reader["CONHOATDONG"]);
                    list.Add(ncc);
                }
            }
            catch (Exception ex) { throw new Exception("Lỗi: " + ex.Message); }
            finally { DBConnect.CloseConnection(conn); }
            return list;
        }

        // 2. Thêm
        public bool Them(nhaCungCapDTO ncc)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = "INSERT INTO nhacungcap (MANCC, TENNCC, SODIENTHOAI, EMAIL, DIACHI, CONHOATDONG) VALUES (@Ma, @Ten, @Sdt, @Email, @DiaChi, @TrangThai)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ma", ncc.MaNCC);
                cmd.Parameters.AddWithValue("@Ten", ncc.TenNCC);
                cmd.Parameters.AddWithValue("@Sdt", ncc.SoDienThoai);
                cmd.Parameters.AddWithValue("@Email", ncc.Email);
                cmd.Parameters.AddWithValue("@DiaChi", ncc.DiaChi);
                cmd.Parameters.AddWithValue("@TrangThai", ncc.ConHoatDong);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }

        // 3. Sửa
        public bool Sua(nhaCungCapDTO ncc)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = "UPDATE nhacungcap SET TENNCC=@Ten, SODIENTHOAI=@Sdt, EMAIL=@Email, DIACHI=@DiaChi, CONHOATDONG=@TrangThai WHERE MANCC=@Ma";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ten", ncc.TenNCC);
                cmd.Parameters.AddWithValue("@Sdt", ncc.SoDienThoai);
                cmd.Parameters.AddWithValue("@Email", ncc.Email);
                cmd.Parameters.AddWithValue("@DiaChi", ncc.DiaChi);
                cmd.Parameters.AddWithValue("@TrangThai", ncc.ConHoatDong);
                cmd.Parameters.AddWithValue("@Ma", ncc.MaNCC);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }

        // 4. Xóa
        public bool Xoa(int ma)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = "DELETE FROM nhacungcap WHERE MANCC=@Ma";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ma", ma);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }

        // 5. Kiểm tra trùng mã
        public bool KiemTraTrungMa(int ma)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = "SELECT COUNT(*) FROM nhacungcap WHERE MANCC=@Ma";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ma", ma);
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }
    }
}