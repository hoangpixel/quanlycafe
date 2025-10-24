using MySql.Data.MySqlClient;
using DAO.CONFIG;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class sanPhamDAO
    {
        public List<sanPhamDTO> DocDanhSachSanPham()
        {
            List<sanPhamDTO> ds = new List<sanPhamDTO>();
            string qry = "SELECT * FROM sanpham WHERE TRANGTHAI = 1";
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

        public bool CapNhatTrangThaiCT(int maSP, int trangThaiCT)
        {
            try
            {
                string qry = "UPDATE sanpham SET TRANGTHAICT = @trangthai WHERE MASANPHAM = @masp";

                using (MySqlConnection conn = DBConnect.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@masp", maSP);
                        cmd.Parameters.AddWithValue("@trangthai", trangThaiCT);
                        cmd.ExecuteNonQuery();
                    }
                    DBConnect.CloseConnection(conn);
                }

                Console.WriteLine($"DAO: Cập nhật TRANGTHAICT = {trangThaiCT} cho SP {maSP}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi caaph nhật trạng thái");
                return false;
            }
        }

        public void xoaTatCa()
        {
            try
            {
                string query = "DELETE FROM sanpham";
                MySqlConnection conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Đã xóa toàn bộ dữ liệu trong bảng sanpham!");
                DBConnect.CloseConnection(conn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xóa toàn bộ sản phẩm: " + ex.Message);
            }
        }


        public sanPhamDTO TimTheoMa(int maSP)
        {
            sanPhamDTO sp = null;
            string qry = "SELECT * FROM sanpham WHERE MASANPHAM = @masp";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@masp", maSP);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        sp = new sanPhamDTO
                        {
                            MaSP = reader.GetInt32("MASANPHAM"),
                            MaLoai = reader.GetInt32("MALOAI"),
                            TenSP = reader.GetString("TENSANPHAM"),
                            TrangThai = reader.GetInt32("TRANGTHAI"),
                            TrangThaiCT = reader.GetInt32("TRANGTHAICT"),
                            Gia = Convert.ToSingle(reader["GIA"]),
                            Hinh = reader.IsDBNull(reader.GetOrdinal("HINH")) ? null : reader.GetString("HINH")
                        };
                    }
                }
            }

            return sp;
        }



    }
}
