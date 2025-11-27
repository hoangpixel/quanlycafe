using DAO.CONFIG;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
namespace DAO
{
    public class khachHangDAO
    {
        public BindingList<khachHangDTO> LayDanhSach()
        {
            var ds = new BindingList<khachHangDTO>();
            string qry = "SELECT * FROM khachhang ORDER BY MAKHACHHANG DESC";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                //conn.Open();
                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        ds.Add(new khachHangDTO
                        {
                            MaKhachHang = r.GetInt32("MAKHACHHANG"),
                            TenKhachHang = r.GetString("TENKHACHHANG"),
                            SoDienThoai = r.IsDBNull(r.GetOrdinal("SODIENTHOAI")) ? "" : r.GetString("SODIENTHOAI"),
                            Email = r.IsDBNull(r.GetOrdinal("EMAIL")) ? "" : r.GetString("EMAIL"),
                            NgayTao = r.GetDateTime("NGAYTAO")
                        });
                    }
                }
            }
            return ds;
        }
    }
}
