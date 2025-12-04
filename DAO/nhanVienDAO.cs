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
            string qry = "SELECT * FROM nhanvien WHERE TRANGTHAI = 1";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
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
                            Luong = r.GetFloat("LUONG"),
                            NgayTao = r.GetDateTime("NGAYTAO"),
                            TrangThai = r.GetInt32("TRANGTHAI")
                        });
                    }
                }
            }
            return ds;
        }

        public bool ThemNhanVien(nhanVienDTO nv)
        {
            string qry = @"INSERT INTO nhanvien (HOTEN, SODIENTHOAI, EMAIL, LUONG) 
                          VALUES (@hoten, @sdt, @email, @luong)";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@hoten", nv.HoTen);
                cmd.Parameters.AddWithValue("@sdt", string.IsNullOrEmpty(nv.SoDienThoai) ? (object)DBNull.Value : nv.SoDienThoai);
                cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(nv.Email) ? (object)DBNull.Value : nv.Email);
                cmd.Parameters.AddWithValue("@luong", nv.Luong);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }


        public bool CapNhatNhanVien(nhanVienDTO nv)
        {
            string qry = @"UPDATE nhanvien 
                          SET HOTEN = @hoten, 
                              SODIENTHOAI = @sdt, 
                              EMAIL = @email, 
                              LUONG = @luong 
                          WHERE MANHANVIEN = @ma";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@ma", nv.MaNhanVien);
                cmd.Parameters.AddWithValue("@hoten", nv.HoTen);
                cmd.Parameters.AddWithValue("@sdt", string.IsNullOrEmpty(nv.SoDienThoai) ? (object)DBNull.Value : nv.SoDienThoai);
                cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(nv.Email) ? (object)DBNull.Value : nv.Email);
                cmd.Parameters.AddWithValue("@luong", nv.Luong);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool XoaNhanVien(int maNV)
        {
            string qry = "UPDATE nhanvien SET TRANGTHAI = 0 WHERE MANHANVIEN = @ma";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@ma", maNV);
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public int layMa()
        {
            MySqlConnection conn = DBConnect.GetConnection();
            int maNV = 0;
            try
            {
                string qry = "SELECT IFNULL(MAX(MANHANVIEN), 0) FROM nhanvien";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                maNV = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy mã NV : " + ex);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return maNV + 1;
        }
    }
}