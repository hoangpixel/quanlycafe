using MySql.Data.MySqlClient;
using DAO.CONFIG;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
        public vaitroDTO LayTheoMa(int maVaiTro)
        {
            vaitroDTO vt = null;
            string qry = "SELECT MAVAITRO, TENVAITRO FROM vaitro WHERE MAVAITRO = @mvt AND TRANGTHAI = 1";
            MySqlConnection conn = DBConnect.GetConnection();

            try
            {
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@mvt", maVaiTro);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    vt = new vaitroDTO
                    {
                        MaVaiTro = reader.GetInt32("MAVAITRO"),
                        TenVaiTro = reader.GetString("TENVAITRO"),
                        TrangThai = 1
                    };
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy vai trò theo mã: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return vt;
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
    }
}