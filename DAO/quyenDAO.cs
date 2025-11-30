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
    }
}