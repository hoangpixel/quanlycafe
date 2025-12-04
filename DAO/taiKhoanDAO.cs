using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DAO
{
    public class taikhoanDAO
    {
        private string connectionString = "Server=localhost;Database=quan_cafe;Uid=root;Pwd=;CharSet=utf8mb4;";

        public List<taikhoanDTO> LayDanhSach()
        {
            List<taikhoanDTO> list = new List<taikhoanDTO>();
            string query = @"
                SELECT 
                    tk.MATAIKHOAN,
                    tk.MANHANVIEN,
                    tk.TENDANGNHAP,
                    tk.MATKHAU,
                    tk.TRANGTHAI,
                    tk.NGAYTAO,
                    tk.MAVAITRO,
                    nv.HOTEN AS TENNHANVIEN,
                    vt.TENVAITRO
                FROM TAIKHOAN tk
                INNER JOIN NHANVIEN nv ON tk.MANHANVIEN = nv.MANHANVIEN
                INNER JOIN VAITRO vt ON tk.MAVAITRO = vt.MAVAITRO
                ORDER BY tk.NGAYTAO DESC";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new taikhoanDTO
                            {
                                MAtaikHOAN = reader.GetInt32(0),
                                MANHANVIEN = reader.GetInt32(1),
                                TENDANGNHAP = reader.GetString(2),
                                MATKHAU = reader.GetString(3),
                                TRANGTHAI = reader.GetBoolean(4),
                                NGAYTAO = reader.GetDateTime(5),
                                MAVAITRO = reader.GetInt32(6),
                                TENNHANVIEN = reader.GetString(7),
                                TENVAITRO = reader.GetString(8)
                            });
                        }
                    }
                }
            }
            return list;
        }

        public bool Them(taikhoanDTO tk)
        {
            string query = @"
                INSERT INTO TAIKHOAN (MANHANVIEN, TENDANGNHAP, MATKHAU, TRANGTHAI, MAVAITRO)
                VALUES (@MANHANVIEN, @TENDANGNHAP, @MATKHAU, @TRANGTHAI, @MAVAITRO)";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MANHANVIEN", tk.MANHANVIEN);
                    cmd.Parameters.AddWithValue("@TENDANGNHAP", tk.TENDANGNHAP);
                    cmd.Parameters.AddWithValue("@MATKHAU", tk.MATKHAU);
                    cmd.Parameters.AddWithValue("@TRANGTHAI", tk.TRANGTHAI);
                    cmd.Parameters.AddWithValue("@MAVAITRO", tk.MAVAITRO);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Sua(taikhoanDTO tk)
        {
            string query = @"
                UPDATE TAIKHOAN 
                SET MANHANVIEN = @MANHANVIEN,
                    TENDANGNHAP = @TENDANGNHAP,
                    MATKHAU = @MATKHAU,
                    TRANGTHAI = @TRANGTHAI,
                    MAVAITRO = @MAVAITRO
                WHERE MATAIKHOAN = @MATAIKHOAN";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MATAIKHOAN", tk.MAtaikHOAN);
                    cmd.Parameters.AddWithValue("@MANHANVIEN", tk.MANHANVIEN);
                    cmd.Parameters.AddWithValue("@TENDANGNHAP", tk.TENDANGNHAP);
                    cmd.Parameters.AddWithValue("@MATKHAU", tk.MATKHAU);
                    cmd.Parameters.AddWithValue("@TRANGTHAI", tk.TRANGTHAI);
                    cmd.Parameters.AddWithValue("@MAVAITRO", tk.MAVAITRO);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Xoa(int mataikhoan)
        {
            string query = "DELETE FROM TAIKHOAN WHERE MATAIKHOAN = @MATAIKHOAN";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MATAIKHOAN", mataikhoan);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public taikhoanDTO DangNhap(string tenDangNhap, string matKhau)
        {
            string query = @"
                SELECT 
                    tk.MATAIKHOAN,
                    tk.MANHANVIEN,
                    tk.TENDANGNHAP,
                    tk.MAVAITRO,
                    nv.HOTEN AS TENNHANVIEN,
                    vt.TENVAITRO
                FROM TAIKHOAN tk
                INNER JOIN NHANVIEN nv ON tk.MANHANVIEN = nv.MANHANVIEN
                INNER JOIN VAITRO vt ON tk.MAVAITRO = vt.MAVAITRO
                WHERE tk.TENDANGNHAP = @TENDANGNHAP 
                  AND tk.MATKHAU = @MATKHAU
                  AND tk.TRANGTHAI = 1";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TENDANGNHAP", tenDangNhap);
                    cmd.Parameters.AddWithValue("@MATKHAU", matKhau);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new taikhoanDTO
                            {
                                MAtaikHOAN = reader.GetInt32(0),
                                MANHANVIEN = reader.GetInt32(1),
                                TENDANGNHAP = reader.GetString(2),
                                MAVAITRO = reader.GetInt32(3),
                                TENNHANVIEN = reader.GetString(4),
                                TENVAITRO = reader.GetString(5)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<KeyValuePair<int, string>> LayDanhSachVaiTro()
        {
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            string query = "SELECT MAVAITRO, TENVAITRO FROM VAITRO WHERE TRANGTHAI = 1 ORDER BY MAVAITRO";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new KeyValuePair<int, string>(
                                reader.GetInt32(0),
                                reader.GetString(1)
                            ));
                        }
                    }
                }
            }
            return list;
        }

        // ✅ ĐÚNG VỚI DATABASE CỦA BẠN
        public List<KeyValuePair<int, string>> LayDanhSachNhanVienChuaCoTK()
        {
            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
            string query = @"
                SELECT nv.MANHANVIEN, nv.HOTEN 
                FROM NHANVIEN nv
                WHERE NOT EXISTS (
                    SELECT 1 FROM TAIKHOAN tk 
                    WHERE tk.MANHANVIEN = nv.MANHANVIEN
                )
                ORDER BY nv.HOTEN";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new KeyValuePair<int, string>(
                                reader.GetInt32(0),
                                reader.GetString(1)
                            ));
                        }
                    }
                }
            }
            return list;
        }
    }
}