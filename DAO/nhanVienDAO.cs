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
        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        public BindingList<nhanVienDTO> LayDanhSach()
        {
            var ds = new BindingList<nhanVienDTO>();
            string qry = "SELECT * FROM nhanvien ORDER BY MANHANVIEN DESC";

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
                            Luong = r.GetDecimal("LUONG"),
                            NgayTao = r.GetDateTime("NGAYTAO")
                        });
                    }
                }
            }
            return ds;
        }

        /// <summary>
        /// Thêm nhân viên mới
        /// </summary>
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

        /// <summary>
        /// Kiểm tra email đã tồn tại chưa
        /// </summary>
        public bool KiemTraEmailTonTai(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            string qry = "SELECT COUNT(*) FROM nhanvien WHERE EMAIL = @email";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@email", email);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        /// <summary>
        /// Cập nhật nhân viên
        /// </summary>
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

        /// <summary>
        /// Xóa nhân viên
        /// </summary>
        public bool XoaNhanVien(int maNV)
        {
            string qry = "DELETE FROM nhanvien WHERE MANHANVIEN = @ma";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@ma", maNV);
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        /// <summary>
        /// Tìm kiếm nhân viên
        /// </summary>
        public BindingList<nhanVienDTO> TimKiemNhanVien(string tuKhoa)
        {
            var ds = new BindingList<nhanVienDTO>();
            string qry = @"SELECT * FROM nhanvien 
                          WHERE HOTEN LIKE @tukhoa 
                             OR EMAIL LIKE @tukhoa 
                             OR SODIENTHOAI LIKE @tukhoa
                          ORDER BY MANHANVIEN DESC";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@tukhoa", "%" + tuKhoa + "%");

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

        /// <summary>
        /// Lấy thông tin 1 nhân viên theo mã
        /// </summary>
        public nhanVienDTO LayTheoMa(int maNV)
        {
            string qry = "SELECT * FROM nhanvien WHERE MANHANVIEN = @ma";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@ma", maNV);

                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        return new nhanVienDTO
                        {
                            MaNhanVien = r.GetInt32("MANHANVIEN"),
                            HoTen = r.GetString("HOTEN"),
                            SoDienThoai = r.IsDBNull(r.GetOrdinal("SODIENTHOAI")) ? "" : r.GetString("SODIENTHOAI"),
                            Email = r.IsDBNull(r.GetOrdinal("EMAIL")) ? "" : r.GetString("EMAIL"),
                            Luong = r.GetDecimal("LUONG"),
                            NgayTao = r.GetDateTime("NGAYTAO")
                        };
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Kiểm tra số điện thoại đã tồn tại chưa
        /// </summary>
        public bool KiemTraSDTTonTai(string sdt)
        {
            if (string.IsNullOrEmpty(sdt))
                return false;

            string qry = "SELECT COUNT(*) FROM nhanvien WHERE SODIENTHOAI = @sdt";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@sdt", sdt);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }
}