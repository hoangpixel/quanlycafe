using MySql.Data.MySqlClient;
using DAO.CONFIG;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DAO
{
    public class quyenDAO
    {
        public BindingList<quyenDTO> LayDanhSachQuyen()
        {
            BindingList<quyenDTO> ds = new BindingList<quyenDTO>();
            string qry = "SELECT MAQUYEN, TENQUYEN, TRANGTHAI FROM quyen WHERE TRANGTHAI = 1";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    quyenDTO q = new quyenDTO
                    {
                        MaQuyen = reader.GetInt32("MAQUYEN"),
                        TenQuyen = reader.GetString("TENQUYEN"),
                        TrangThai = reader.GetInt32("TRANGTHAI")
                    };
                    ds.Add(q);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách quyền: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return ds;
        }

        public int layMa()
        {
            MySqlConnection conn = DBConnect.GetConnection();
            int maQuyen = 0;
            try
            {
                string qry = "SELECT IFNULL(MAX(MAQUYEN), 0) FROM quyen";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                maQuyen = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy mã quyen : " + ex);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return maQuyen + 1;
        }

        public bool ThemQuyen(quyenDTO vt)
        {
            string qry = "INSERT INTO quyen (TENQUYEN) VALUES (@tenQuyen)";
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tenQuyen", vt.TenQuyen);
                int rs = cmd.ExecuteNonQuery();
                return rs > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi thêm quyen: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool SuaQuyen(quyenDTO vt)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = @"UPDATE quyen SET TENQUYEN = @tenQuyen WHERE MAQUYEN = @maQuyen";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maQuyen", vt.MaQuyen);
                cmd.Parameters.AddWithValue("@tenQuyen", vt.TenQuyen);
                int rs = cmd.ExecuteNonQuery();
                if (rs > 0)
                {
                    return true;
                }
                return false;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Kh sua dc quyen : " + ex);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool XoaQuyen(int maQuyen)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = @"UPDATE quyen SET TRANGTHAI = 0 WHERE MAQUYEN = @maQuyen";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maQuyen", maQuyen);
                int rs = cmd.ExecuteNonQuery();
                if (rs > 0)
                {
                    return true;
                }
                return false;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Kh xoa dc quyen : " + ex);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }
    }
}