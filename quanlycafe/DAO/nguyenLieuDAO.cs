using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using quanlycafe.config;
using quanlycafe.DTO;

namespace quanlycafe.DAO
{
    internal class nguyenLieuDAO
    {
        public List<nguyenLieuDTO> docDanhSachNguyenLieu()
        {
            List<nguyenLieuDTO> ds = new List<nguyenLieuDTO>();
            string qry = "SELECT * FROM nguyenlieu WHERE TRANGTHAI = 1";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    nguyenLieuDTO nl = new nguyenLieuDTO
                    {
                        MaNguyenLieu = reader.GetInt32("MANGUYENLIEU"),
                        TenNguyenLieu = reader.GetString("TENNGUYENLIEU"),
                        DonViCoSo = reader.GetString("DONVICOSO"),
                        TrangThai = reader.GetInt32("TRANGTHAI"),
                        TonKho = reader.GetDecimal("TONKHO")
                    };

                    ds.Add(nl);
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

        public bool Them(nguyenLieuDTO nl)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = @"INSERT INTO nguyenlieu (TENNGUYENLIEU, DONVICOSO, TRANGTHAI, TONKHO)
                               VALUES (@Ten, @DonVi, @TrangThai, @TonKho);
                               SELECT LAST_INSERT_ID();";

                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@Ten", nl.TenNguyenLieu);
                cmd.Parameters.AddWithValue("@DonVi", nl.DonViCoSo);
                cmd.Parameters.AddWithValue("@TrangThai", nl.TrangThai);
                cmd.Parameters.AddWithValue("@TonKho", nl.TonKho);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    nl.MaNguyenLieu = Convert.ToInt32(result);
                    Console.WriteLine($"Thêm nguyên liệu thành công! Mã mới: {nl.MaNguyenLieu}");
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi thêm nguyên liệu: " + e.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool Sua(nguyenLieuDTO nl)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = @"UPDATE nguyenlieu 
                               SET TENNGUYENLIEU = @Ten, DONVICOSO = @DonVi, TONKHO = @TonKho, TRANGTHAI = @TrangThai
                               WHERE MANGUYENLIEU = @Ma";

                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@Ten", nl.TenNguyenLieu);
                cmd.Parameters.AddWithValue("@DonVi", nl.DonViCoSo);
                cmd.Parameters.AddWithValue("@TonKho", nl.TonKho);
                cmd.Parameters.AddWithValue("@TrangThai", nl.TrangThai);
                cmd.Parameters.AddWithValue("@Ma", nl.MaNguyenLieu);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi sửa nguyên liệu: " + e.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool Xoa(int maNguyenLieu)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = "UPDATE nguyenlieu SET TRANGTHAI = 0 WHERE MANGUYENLIEU = @Ma";
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@Ma", maNguyenLieu);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    Console.WriteLine($"Ẩn nguyên liệu có mã {maNguyenLieu} thành công!");
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi xóa nguyên liệu: " + e.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }



        public nguyenLieuDTO TimTheoMa(int ma)
        {
            nguyenLieuDTO nl = null;
            string qry = "SELECT * FROM nguyenlieu WHERE MANGUYENLIEU = @ma";
            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@ma", ma);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        nl = new nguyenLieuDTO
                        {
                            MaNguyenLieu = reader.GetInt32("MANGUYENLIEU"),
                            TenNguyenLieu = reader.GetString("TENNGUYENLIEU"),
                            DonViCoSo = reader.GetString("DONVICOSO"),
                            TonKho = reader.GetDecimal("TONKHO")
                        };
                    }
                }
            }
            return nl;
        }
    }
}
