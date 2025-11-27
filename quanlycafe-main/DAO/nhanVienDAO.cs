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
    public class nhanVienDAO
    {
        public BindingList<nhanVienDTO> LayDanhSach()
        {
            var ds = new BindingList<nhanVienDTO>();
            string qry = "SELECT * FROM nhanvien ORDER BY MANHANVIEN DESC";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                //conn.Open();
                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        ds.Add(new nhanVienDTO
                        {
                            MaNhanVien = r.GetInt32("MANHANVIEN"),
                            HoTen = r.GetString("HOTEN"),
                            SoDienThoai = r.IsDBNull(r.GetOrdinal("SODIENTHOAI")) ? "" : r.GetString("SODIENTHOAI"),
                            Email = r.IsDBNull(r.GetOrdinal("EMAIL")) ? "" : r.GetString("EMAIL"),
                            Luong = r.GetDecimal("LUONG"),
                            NgayTao = r.GetDateTime("NGAYTAO")
                        });
                    }
                }
            }
            return ds;
        }
    }
}
