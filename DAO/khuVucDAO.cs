using DAO.CONFIG;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
