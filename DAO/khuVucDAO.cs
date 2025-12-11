using DAO.CONFIG;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DAO
{
    public class khuVucDAO
    {
        public BindingList<khuVucDTO> LayDanhSach()
        {
            BindingList<khuVucDTO> ds = new BindingList<khuVucDTO>();
            string qry = "SELECT * FROM khuvuc";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    khuVucDTO kv = new khuVucDTO();
                    kv.MaKhuVuc = reader.GetInt32("MAKHUVUC");
                    kv.TenKhuVuc = reader.GetString("TENKHUVUC");
                    ds.Add(kv);
                }

                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách vai trò: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }

        public int LayMa()
        {
            MySqlConnection conn = DBConnect.GetConnection();
            int maKV = 0;
            try
            {
                string qry = "SELECT IFNULL(MAX(MAKHUVUC), 0) FROM khuvuc";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                maKV = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy mã khu vực: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return maKV + 1;
        }

        // 3. Thêm khu vực
        public bool ThemKhuVuc(khuVucDTO kv)
        {
            string qry = "INSERT INTO khuvuc (TENKHUVUC) VALUES (@tenKV)";
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tenKV", kv.TenKhuVuc);

                int rs = cmd.ExecuteNonQuery();
                return rs > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi thêm khu vực: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        // 4. Sửa khu vực
        public bool SuaKhuVuc(khuVucDTO kv)
        {
            string qry = "UPDATE khuvuc SET TENKHUVUC = @tenKV WHERE MAKHUVUC = @maKV";
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maKV", kv.MaKhuVuc);
                cmd.Parameters.AddWithValue("@tenKV", kv.TenKhuVuc);

                int rs = cmd.ExecuteNonQuery();
                return rs > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi sửa khu vực: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        // 5. Xóa khu vực
        public bool XoaKhuVuc(int maKV)
        {
            string qry = "DELETE FROM khuvuc WHERE MAKHUVUC = @maKV";
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maKV", maKV);

                int rs = cmd.ExecuteNonQuery();
                return rs > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi xóa khu vực: " + ex.Message);
                // Lưu ý: Nếu khu vực đang có bàn, SQL sẽ báo lỗi ràng buộc khóa ngoại tại đây
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }
        public int ThemKV(khuVucDTO kv)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = "INSERT INTO khuvuc (TENKHUVUC) VALUES (@Ten); SELECT LAST_INSERT_ID();";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@Ten", kv.TenKhuVuc);
                int newId = Convert.ToInt32(cmd.ExecuteScalar());
                kv.MaKhuVuc = newId; // GÁN LẠI VÀO ĐỐI TƯỢNG
                return newId;
                //return cmd.ExecuteNonQuery() > 0;

            }
            catch { return 0; }
            finally { DBConnect.CloseConnection(conn); }
        }
        public bool SuaKV(khuVucDTO kv)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = "UPDATE khuvuc SET TENKHUVUC = @Ten WHERE MAKHUVUC = @Ma";
                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@Ten", kv.TenKhuVuc);
                    cmd.Parameters.AddWithValue("@Ma", kv.MaKhuVuc);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa: " + ex.Message); // chỉ để debug
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool XoaKV(int maKV)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = "DELETE FROM khuvuc WHERE MAKHUVUC = @Ma";
                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", maKV);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (MySqlException ex)
            {
                // Nếu có khóa ngoại thì báo rõ hơn
                if (ex.Number == 1451)
                    MessageBox.Show("Không thể xóa vì khu vực này đang được sử dụng!");
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }
    }
}
