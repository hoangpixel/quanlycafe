using MySql.Data.MySqlClient;
using DAO.CONFIG;
using DTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DAO
{
    public class sanPhamDAO
    {
        public BindingList<sanPhamDTO> DocDanhSachSanPham()
        {
            BindingList<sanPhamDTO> ds = new BindingList<sanPhamDTO>();
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

        public BindingList<sanPhamDTO> DocDanhSachSanPhamCoCongThuc()
        {
            BindingList<sanPhamDTO> ds = new BindingList<sanPhamDTO>();
            string qry = "SELECT * FROM sanpham WHERE TRANGTHAI = 1 AND TRANGTHAICT = 1";
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
        public int layMa()
        {
            MySqlConnection conn = DBConnect.GetConnection();
            int maSP = 0;
            try
            {
                string qry = "SELECT IFNULL(MAX(MASANPHAM), 0) FROM sanpham";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                maSP = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch(MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy mã SP : " + ex);
            }finally
            {
                DBConnect.CloseConnection(conn);
            }
            return maSP + 1;
        }

        public bool Them(sanPhamDTO ct)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = @"INSERT INTO sanpham (MASANPHAM,MALOAI, TENSANPHAM, GIA, HINH) VALUES (@masp,@maloai,@tensp,@gia,@hinh)";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@masp", ct.MaSP);
                cmd.Parameters.AddWithValue("@maloai", ct.MaLoai);
                cmd.Parameters.AddWithValue("@tensp", ct.TenSP);
                cmd.Parameters.AddWithValue("@gia", ct.Gia);
                cmd.Parameters.AddWithValue("@hinh", ct.Hinh);
                int rs = cmd.ExecuteNonQuery();
                if(rs > 0)
                {
                    return true;
                }
                return false;
            }catch(MySqlException ex)
            {
                Console.WriteLine("Lỗi thêm sản phẩm : " + ex);
                return false;
            }finally
            {
                DBConnect.CloseConnection(conn);   
            }
        }

        public bool Sua(sanPhamDTO sp)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = @"UPDATE sanpham SET MALOAI = @maLoai, TENSANPHAM = @tenSP, GIA = @gia, HINH = @hinh WHERE MASANPHAM = @maSP";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maLoai", sp.MaLoai);
                cmd.Parameters.AddWithValue("@tenSP", sp.TenSP);
                cmd.Parameters.AddWithValue("@gia", sp.Gia);
                cmd.Parameters.AddWithValue("@hinh", sp.Hinh);
                cmd.Parameters.AddWithValue("@masp", sp.MaSP);
                int rs = cmd.ExecuteNonQuery();
                if(rs > 0)
                {
                    return true;
                }
                return false;
            }catch(MySqlException ex)
            {
                Console.WriteLine("Lỗi kh cập nhật đc sp : " + ex);
                return false;
            }finally
            {
                DBConnect.CloseConnection(conn);
            }
        }
        public bool Xoa(int maSP)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = $"UPDATE sanpham SET TRANGTHAI = 0 WHERE MASANPHAM = {maSP}";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Xóa (ẩn) sản phẩm thành công!");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi xóa sản phẩm: " + e.Message);
                return false;
            }finally
            {
                DBConnect.CloseConnection(conn);
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
