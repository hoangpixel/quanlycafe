using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using quanlycafe.DTO;
using MySql.Data.MySqlClient;
using quanlycafe.config;

namespace quanlycafe.DAO
{
    internal class loaiSanPhamDAO
    {
        public List<loaiDTO> docDanhSachLoai()
        {
            List<loaiDTO> ds = new List<loaiDTO>();
            string qry = "SELECT * FROM loai";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    loaiDTO l = new loaiDTO
                    {
                        MaLoai = reader.GetInt32("MALOAI"),
                        TenLoai = reader.GetString("TENLOAI")
                    };

                    ds.Add(l);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách loại sản phẩm: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }
    }
}
