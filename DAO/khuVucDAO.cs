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
    }
}
