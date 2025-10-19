using MySql.Data.MySqlClient;
using quanlycafe.config;
using quanlycafe.DTO;
using System;
using System.Collections.Generic;

namespace quanlycafe.DAO
{
    internal class heSoDAO
    {
        public List<heSoDTO> DocDanhSachHeSo()
        {
            List<heSoDTO> ds = new List<heSoDTO>();
            string qry = "SELECT * FROM hesodonvi";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    heSoDTO hs = new heSoDTO
                    {
                        MaNguyenLieu = reader.GetInt32("MANGUYENLIEU"),
                        MaDonVi = reader.GetInt32("MADONVI"),
                        HeSo = reader.IsDBNull(reader.GetOrdinal("HESO"))
                            ? 0
                            : reader.GetFloat("HESO")
                    };
                    ds.Add(hs);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách hệ số: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }

        public bool Them(heSoDTO hs)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = @"INSERT INTO hesodonvi (MANGUYENLIEU, MADONVI, HESO)
                               VALUES (@maNL, @maDV, @heSo);";

                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maNL", hs.MaNguyenLieu);
                cmd.Parameters.AddWithValue("@maDV", hs.MaDonVi);
                cmd.Parameters.AddWithValue("@heSo", hs.HeSo);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi thêm hệ số: " + e.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool CapNhat(heSoDTO hs)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = @"UPDATE hesodonvi 
                               SET HESO = @heSo 
                               WHERE MANGUYENLIEU = @maNL AND MADONVI = @maDV;";

                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maNL", hs.MaNguyenLieu);
                cmd.Parameters.AddWithValue("@maDV", hs.MaDonVi);
                cmd.Parameters.AddWithValue("@heSo", hs.HeSo);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi cập nhật hệ số: " + e.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool Xoa(int maNguyenLieu, int maDonVi)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = @"DELETE FROM hesodonvi 
                               WHERE MANGUYENLIEU = @maNL AND MADONVI = @maDV;";

                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maNL", maNguyenLieu);
                cmd.Parameters.AddWithValue("@maDV", maDonVi);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi xóa hệ số: " + e.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }
    }
}
