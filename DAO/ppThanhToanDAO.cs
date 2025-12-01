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
    public class ppThanhToanDAO
    {
        public BindingList<ppThanhToanDTO> LayDanhSach()
        {
            BindingList<ppThanhToanDTO> ds = new BindingList<ppThanhToanDTO>();
            string qry = "SELECT * FROM thanhtoan";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ppThanhToanDTO ct = new ppThanhToanDTO();
                    ct.MaTT = reader.GetInt32("MATT");
                    ct.HinhThuc = reader.GetString("HINHTHUC");
                    ds.Add(ct);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách nguyên liệu: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }
    }
}
