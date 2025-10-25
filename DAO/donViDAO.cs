using MySql.Data.MySqlClient;
using DAO.CONFIG;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class donViDAO
    {
        public List<donViDTO> docDangSachDonVi()
        {
            List<donViDTO> ds = new List<donViDTO>();
            string qry = "SELECT * FROM donvi WHERE TRANGTHAI = 1";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    donViDTO nl = new donViDTO
                    {
                        MaDonVi = reader.GetInt32("MADONVI"),
                        TenDonVi = reader.GetString("TENDONVI"),
                        TrangThai = reader.GetInt32("TRANGTHAI")
                    };

                    ds.Add(nl);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách đơn vị: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }

        public bool Them(donViDTO ct)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = @"INSERT INTO donvi (TENDONVI, TRANGTHAI)
                               VALUES (@Ten, @TrangThai);
                               SELECT LAST_INSERT_ID();";

                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@Ten", ct.TenDonVi);
                cmd.Parameters.AddWithValue("@TrangThai", ct.TrangThai);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    ct.MaDonVi = Convert.ToInt32(result);
                    Console.WriteLine($"Thêm nguyên liệu thành công! Mã mới: {ct.MaDonVi}");
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi thêm đơn vị: " + e.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool Sua(donViDTO ct)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = @"UPDATE donvi 
                               SET TENDONVI = @Ten, TRANGTHAI = @TrangThai
                               WHERE MADONVI = @Ma";

                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@Ten", ct.TenDonVi);
                cmd.Parameters.AddWithValue("@TrangThai", ct.TrangThai);
                cmd.Parameters.AddWithValue("@Ma", ct.MaDonVi);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi sửa đơn vị: " + e.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool Xoa(int maDonVi)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = "UPDATE donvi SET TRANGTHAI = 0 WHERE MADONVI = @Ma";
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@Ma", maDonVi);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    Console.WriteLine($"Ẩn nguyên liệu có mã {maDonVi} thành công!");
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi xóa đơn vị: " + e.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public List<donViDTO> layDanhSachDonViTheoNguyenLieu(int maNguyenLieu)
        {
            List<donViDTO> ds = new List<donViDTO>();
            try
            {
                string qry = @"
            SELECT d.MADONVI, d.TENDONVI
            FROM hesodonvi h
            JOIN donvi d ON h.MADONVI = d.MADONVI
            WHERE h.MANGUYENLIEU = @maNL;";

                using (MySqlConnection conn = DBConnect.GetConnection())
                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@maNL", maNguyenLieu);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            donViDTO dv = new donViDTO
                            {
                                MaDonVi = dr.GetInt32("MADONVI"),
                                TenDonVi = dr.GetString("TENDONVI")
                            };
                            ds.Add(dv);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi lấy danh sách đơn vị theo nguyên liệu: " + ex.Message);
            }

            return ds;
        }

    }
}
