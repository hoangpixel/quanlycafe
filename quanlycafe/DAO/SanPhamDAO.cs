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
    internal class SanPhamDAO
    {
        public List<sanPhamDTO> DocDanhSachSanPham()
        {
            List<sanPhamDTO> ds = new List<sanPhamDTO>();
            string qry = "SELECT * FROM sanpham";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sanPhamDTO sp = new sanPhamDTO
                    {
                        MaSP = reader.GetInt32("MASANPHAM"),
                        MaLoai = reader.GetInt32("MALOAI"),
                        TenSP = reader.GetString("TENSANPHAM"),
                        TrangThai = reader.GetInt32("TRANGTHAI"),
                        TrangThaiCT = reader.GetInt32("TRANGTHAICT"),
                        Gia = Convert.ToSingle(reader["GIA"]),
                        Hinh = reader.IsDBNull(reader.GetOrdinal("HINH")) ? null : reader.GetString("HINH")
                    };

                    ds.Add(sp);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách sản phẩm: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }

        public bool Them(sanPhamDTO sp)
        {
            try
            {
                string qry = "INSERT INTO sanpham (MALOAI, TENSANPHAM, GIA, HINH) VALUES (";
                qry += $"{sp.MaLoai}, ";
                qry += $"'{sp.TenSP}', ";
                qry += $"{sp.Gia}, ";
                qry += $"'{sp.Hinh}'";
                qry += ")";

                MySqlConnection conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Thêm sản phẩm thành công!");
                DBConnect.CloseConnection(conn);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi thêm sản phẩm: " + e.Message);
                return false;
            }
        }

        public bool Sua(sanPhamDTO sp)
        {
            try
            {
                string qry = "UPDATE sanpham SET ";
                qry += $"MALOAI = {sp.MaLoai}, ";
                qry += $"TENSANPHAM = '{sp.TenSP}', ";
                qry += $"GIA = {sp.Gia}, ";
                qry += $"HINH = '{sp.Hinh}' ";
                qry += $"WHERE MASANPHAM = {sp.MaSP}";

                MySqlConnection conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Sửa sản phẩm thành công!");
                DBConnect.CloseConnection(conn);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi sửa sản phẩm: " + e.Message);
                return false;
            }
        }

        public bool Xoa(int maSP)
        {
            try
            {
                string qry = $"UPDATE sanpham SET TRANGTHAI = 0 WHERE MASANPHAM = {maSP}";

                MySqlConnection conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Xóa (ẩn) sản phẩm thành công!");
                DBConnect.CloseConnection(conn);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi xóa sản phẩm: " + e.Message);
                return false;
            }
        }

    }
}
