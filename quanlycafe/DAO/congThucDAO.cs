using MySql.Data.MySqlClient;
using quanlycafe.config;
using quanlycafe.DTO;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace quanlycafe.DAO
{
    internal class congThucDAO
    {
        public List<congThucDTO> docDanhSachCongThucTheoSP(int maSP)
        {
            var ds = new List<congThucDTO>();
            string qry = @"
        SELECT c.MASANPHAM, c.MANGUYENLIEU, c.SOLUONGCOSO, c.MADONVICOSO, c.TRANGTHAI,
               n.TENNGUYENLIEU, d.TENDONVI
        FROM congthuc c
        JOIN nguyenlieu n ON c.MANGUYENLIEU = n.MANGUYENLIEU
        JOIN donvi d ON c.MADONVICOSO = d.MADONVI
        WHERE c.MASANPHAM = @masp AND c.TRANGTHAI = 1";

            using (var conn = DBConnect.GetConnection())
            using (var cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@masp", maSP);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ds.Add(new congThucDTO
                        {
                            MaSanPham = reader.GetInt32("MASANPHAM"),
                            MaNguyenLieu = reader.GetInt32("MANGUYENLIEU"),
                            SoLuongCoSo = reader.GetFloat("SOLUONGCOSO"),
                            MaDonViCoSo = reader.GetInt32("MADONVICOSO"),
                            TrangThai = reader.GetInt32("TRANGTHAI"),
                            TenNguyenLieu = reader.GetString("TENNGUYENLIEU"),
                            TenDonViCoSo = reader.GetString("TENDONVI")
                        });
                    }
                }
            }
            return ds;
        }


        public List<congThucDTO> docTatCaCongThuc()
        {
            var ds = new List<congThucDTO>();
            string qry = @"
        SELECT c.MASANPHAM, s.TENSANPHAM,
               c.MANGUYENLIEU, n.TENNGUYENLIEU,
               c.SOLUONGCOSO, c.MADONVICOSO,
               d.TENDONVI
        FROM congthuc c
        JOIN sanpham s   ON c.MASANPHAM   = s.MASANPHAM
        JOIN nguyenlieu n ON c.MANGUYENLIEU = n.MANGUYENLIEU
        JOIN donvi d     ON c.MADONVICOSO = d.MADONVI
        WHERE c.TRANGTHAI = 1";

            try
            {
                using (var conn = DBConnect.GetConnection())
                using (var cmd = new MySqlCommand(qry, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ds.Add(new congThucDTO
                        {
                            MaSanPham = reader.GetInt32("MASANPHAM"),
                            TenSanPham = reader.GetString("TENSANPHAM"),
                            MaNguyenLieu = reader.GetInt32("MANGUYENLIEU"),
                            TenNguyenLieu = reader.GetString("TENNGUYENLIEU"),
                            SoLuongCoSo = reader.GetFloat("SOLUONGCOSO"),
                            MaDonViCoSo = reader.GetInt32("MADONVICOSO"),
                            TenDonViCoSo = reader.GetString("TENDONVI")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc danh sách công thức: " + ex.Message,
                                "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return ds;
        }



        // 🟢 Thêm công thức mới
        public bool Them(congThucDTO ct)
        {
            bool result = false;
            try
            {
                string qry = @"INSERT INTO congthuc 
                       (MASANPHAM, MANGUYENLIEU, SOLUONGCOSO, MADONVICOSO,TRANGTHAI)
                       VALUES (@masp, @manl, @soluong,@madonvi ,@trangthai)";

                using (MySqlConnection conn = DBConnect.GetConnection())
                {
                    // ❌ Đừng mở lại kết nối ở đây!
                    using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@masp", ct.MaSanPham);
                        cmd.Parameters.AddWithValue("@manl", ct.MaNguyenLieu);
                        cmd.Parameters.AddWithValue("@soluong", ct.SoLuongCoSo);
                        cmd.Parameters.AddWithValue("@madonvi", ct.MaDonViCoSo);
                        cmd.Parameters.AddWithValue("@trangthai", ct.TrangThai);

                        int rows = cmd.ExecuteNonQuery();
                        result = rows > 0;
                        Console.WriteLine($"✅ Thêm công thức: SP={ct.MaSanPham}, NL={ct.MaNguyenLieu}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm công thức: " + ex.Message,
                                "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }





        // 🟡 Cập nhật định lượng
        public bool Sua(congThucDTO ct)
        {
            string qry = "UPDATE congthuc SET SOLUONGCOSO = @soluong, MADONVICOSO = @madonvi ,TRANGTHAI = @trangthai " +
                         "WHERE MASANPHAM = @masp AND MANGUYENLIEU = @manl";

            try
            {
                using (MySqlConnection conn = DBConnect.GetConnection())
                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@masp", ct.MaSanPham);
                    cmd.Parameters.AddWithValue("@manl", ct.MaNguyenLieu);
                    cmd.Parameters.AddWithValue("@soluong", ct.SoLuongCoSo);
                    cmd.Parameters.AddWithValue("@madonvi", ct.MaDonViCoSo);
                    cmd.Parameters.AddWithValue("@trangthai", ct.TrangThai);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("❌ Lỗi sửa công thức: " + e.Message);
                return false;
            }
        }


        // 🔴 Ẩn công thức (set trạng thái = 0)
        public bool Xoa(int maSP, int maNL)
        {
            try
            {
                string qry = "DELETE FROM congthuc WHERE MASANPHAM = @maSP AND MANGUYENLIEU = @maNL";
                using (MySqlConnection conn = DBConnect.GetConnection())
                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@maSP", maSP);
                    cmd.Parameters.AddWithValue("@maNL", maNL);
                    cmd.ExecuteNonQuery();
                }

                string checkQry = "SELECT COUNT(*) FROM congthuc WHERE MASANPHAM = @maSP";
                using (MySqlConnection conn = DBConnect.GetConnection())
                using (MySqlCommand cmd = new MySqlCommand(checkQry, conn))
                {
                    cmd.Parameters.AddWithValue("@maSP", maSP);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        string updateQry = "UPDATE sanpham SET TRANGTHAICT = 0 WHERE MASANPHAM = @maSP";
                        using (MySqlCommand updateCmd = new MySqlCommand(updateQry, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@maSP", maSP);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xóa công thức: " + ex.Message);
                return false;
            }
        }


        // 🔵 Xóa toàn bộ công thức của 1 sản phẩm (ẩn đi)
        public bool XoaTheoSanPham(int maSP)
        {
            try
            {
                string qry = "UPDATE congthuc SET TRANGTHAI = 0 WHERE MASANPHAM = @masp";
                MySqlConnection conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@masp", maSP);

                cmd.ExecuteNonQuery();
                DBConnect.CloseConnection(conn);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi xóa toàn bộ công thức theo SP: " + e.Message);
                return false;
            }
        }

        public bool ThemHoacCapNhat(congThucDTO ct)
        {
            try
            {
                string qry = @"
            INSERT INTO congthuc (MASANPHAM, MANGUYENLIEU, SOLUONGCOSO, MADONVICOSO,TRANGTHAI)
            VALUES (@masp, @manl, @soluong, @madonvi,@trangthai)
            ON DUPLICATE KEY UPDATE 
                SOLUONGCOSO = SOLUONGCOSO + @soluong,
                TRANGTHAI = VALUES(TRANGTHAI);";

                using (MySqlConnection conn = DBConnect.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@masp", ct.MaSanPham);
                        cmd.Parameters.AddWithValue("@manl", ct.MaNguyenLieu);
                        cmd.Parameters.AddWithValue("@soluong", ct.SoLuongCoSo);
                        cmd.Parameters.AddWithValue("@madonvi", ct.MaDonViCoSo);
                        cmd.Parameters.AddWithValue("@trangthai", ct.TrangThai);

                        cmd.ExecuteNonQuery();
                    }
                    DBConnect.CloseConnection(conn);
                }

                Console.WriteLine($"DAO: Đã thêm hoặc cập nhật CT (SP={ct.MaSanPham}, NL={ct.MaNguyenLieu})");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Lỗi thêm công thức: " + e.Message, "Lỗi SQL",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public List<sanPhamDTO> docDanhSachSanPhamTheoNguyenLieu(int maNguyenLieu)
        {
            List<sanPhamDTO> ds = new List<sanPhamDTO>();
            string qry = @"
        SELECT sp.MASANPHAM, sp.TENSANPHAM, sp.HINH, sp.GIA, c.SOLUONGCOSO, c.MADONVICOSO
        FROM congthuc c
        JOIN sanpham sp ON c.MASANPHAM = sp.MASANPHAM
        WHERE c.MANGUYENLIEU = @maNguyenLieu AND c.TRANGTHAI = 1;
    ";

            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maNguyenLieu", maNguyenLieu);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sanPhamDTO sp = new sanPhamDTO
                        {
                            MaSP = reader.GetInt32("MASANPHAM"),
                            TenSP = reader.GetString("TENSANPHAM"),
                            Hinh = reader.IsDBNull(reader.GetOrdinal("HINH")) ? "" : reader.GetString("HINH"),
                            Gia = reader.IsDBNull(reader.GetOrdinal("GIA")) ? 0 : Convert.ToSingle(reader["GIA"]),
                            SoLuongCoSo = reader.IsDBNull(reader.GetOrdinal("SOLUONGCOSO")) ? 0 : reader.GetFloat("SOLUONGCOSO"),
                            MaDonViCoSo = reader.GetInt32("MADONVICOSO")
                        };
                        ds.Add(sp);
                    }
                }
            }

            return ds;
        }



    }
}
