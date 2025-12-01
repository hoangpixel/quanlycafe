using DAO.CONFIG; 
using DTO;
using MySql.Data.MySqlClient; 
using System;
using System.ComponentModel;

namespace DAO
{
    public class nhaCungCapDAO
    {
        public BindingList<nhaCungCapDTO> LayDanhSach()
        {
            BindingList<nhaCungCapDTO> list = new BindingList<nhaCungCapDTO>();
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = "SELECT * FROM nhacungcap WHERE CONHOATDONG = 1";
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

        public int layMa()
        {
            MySqlConnection conn = DBConnect.GetConnection();
            int maNCC = 0;
            try
            {
                string qry = "SELECT IFNULL(MAX(MANCC), 0) FROM nhacungcap";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                maNCC = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy mã NCC : " + ex);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return maNCC + 1;
        }

        // 2. Thêm
        public bool Them(nhaCungCapDTO ncc)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = "INSERT INTO nhacungcap (MANCC, TENNCC, SODIENTHOAI, EMAIL, DIACHI) VALUES (@Ma, @Ten, @Sdt, @Email, @DiaChi)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ma", ncc.MaNCC);
                cmd.Parameters.AddWithValue("@Ten", ncc.TenNCC);
                cmd.Parameters.AddWithValue("@Sdt", ncc.SoDienThoai);
                cmd.Parameters.AddWithValue("@Email", ncc.Email);
                cmd.Parameters.AddWithValue("@DiaChi", ncc.DiaChi);
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
                string query = "UPDATE nhacungcap SET TENNCC=@Ten, SODIENTHOAI=@Sdt, EMAIL=@Email, DIACHI=@DiaChi WHERE MANCC=@Ma";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ten", ncc.TenNCC);
                cmd.Parameters.AddWithValue("@Sdt", ncc.SoDienThoai);
                cmd.Parameters.AddWithValue("@Email", ncc.Email);
                cmd.Parameters.AddWithValue("@DiaChi", ncc.DiaChi);
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
                string query = "UPDATE nhacungcap SET CONHOATDONG = 0 WHERE MANCC=@Ma";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ma", ma);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }
    }
}