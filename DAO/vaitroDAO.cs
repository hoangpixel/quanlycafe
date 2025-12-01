using MySql.Data.MySqlClient;
using DAO.CONFIG;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ZstdSharp.Unsafe;

namespace DAO
{
    public class vaitroDAO
    {
        public BindingList<vaitroDTO> LayDanhSachVaiTro()
        {
            BindingList<vaitroDTO> ds = new BindingList<vaitroDTO>();
            string qry = "SELECT MAVAITRO, TENVAITRO, TRANGTHAI FROM vaitro WHERE TRANGTHAI = 1";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    vaitroDTO vt = new vaitroDTO
                    {
                        MaVaiTro = reader.GetInt32("MAVAITRO"),
                        TenVaiTro = reader.GetString("TENVAITRO"),
                        TrangThai = reader.GetInt32("TRANGTHAI")
                    };
                    ds.Add(vt);
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

        public int layMa()
        {
            MySqlConnection conn = DBConnect.GetConnection();
            int maVT = 0;
            try
            {
                string qry = "SELECT IFNULL(MAX(MAVAITRO), 0) FROM vaitro";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                maVT = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy mã VT : " + ex);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return maVT + 1;
        }

        public bool ThemVaiTro(vaitroDTO vt)
        {
            string qry = "INSERT INTO vaitro (TENVAITRO, TRANGTHAI) VALUES (@tenvt, 1)";
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tenvt", vt.TenVaiTro);
                int rs = cmd.ExecuteNonQuery();
                return rs > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi thêm vai trò: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool SuaVaiTro(vaitroDTO vt)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = @"UPDATE vaitro SET TENVAITRO = @tenVT WHERE MAVAITRO = @maVT";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maVT", vt.MaVaiTro);
                cmd.Parameters.AddWithValue("@tenVT",vt.TenVaiTro);
                int rs = cmd.ExecuteNonQuery();
                if(rs > 0)
                {
                    return true;
                }
                return false;
            }catch(MySqlException ex)
            {
                Console.WriteLine("Kh sua dc vai tro : " + ex);
                return false;
            }finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool XoaVaiTro(int maVT)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = @"UPDATE vaitro SET TRANGTHAI = 0 WHERE MAVAITRO = @maVT";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maVT", maVT);
                int rs = cmd.ExecuteNonQuery();
                if(rs > 0)
                {
                    return true;
                }
                return false;
            }catch(MySqlException ex)
            {
                Console.WriteLine("Kh xoa dc vai tro : " + ex);
                return false;
            }finally
            {
                DBConnect.CloseConnection(conn);
            }
        }
    }
}