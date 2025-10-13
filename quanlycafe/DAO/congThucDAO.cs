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
        // 🟢 Lấy danh sách công thức đang hoạt động
        public List<congThucDTO> docDanhSachCongThucTheoSP(int maSP)
        {
            List<congThucDTO> ds = new List<congThucDTO>();
            string qry = $"SELECT * FROM congthuc WHERE MASANPHAM = {maSP} AND TRANGTHAI = 1";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    congThucDTO ct = new congThucDTO
                    {
                        MaSanPham = reader.GetInt32("MASANPHAM"),
                        MaNguyenLieu = reader.GetInt32("MANGUYENLIEU"),
                        SoLuongCoSo = reader.GetFloat("SOLUONGCOSO"),
                        TrangThai = reader.GetInt32("TRANGTHAI")
                    };
                    ds.Add(ct);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách công thức: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }

        public List<congThucDTO> docTatCaCongThuc()
        {
            List<congThucDTO> ds = new List<congThucDTO>();
            try
            {
                string qry = @"
            SELECT c.MASANPHAM, s.TENSANPHAM, 
                   c.MANGUYENLIEU, n.TENNGUYENLIEU, 
                   c.SOLUONGCOSO
            FROM congthuc c
            JOIN sanpham s ON c.MASANPHAM = s.MASANPHAM
            JOIN nguyenlieu n ON c.MANGUYENLIEU = n.MANGUYENLIEU
            WHERE c.TRANGTHAI = 1";

                using (MySqlConnection conn = DBConnect.GetConnection())
                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        congThucDTO ct = new congThucDTO
                        {
                            MaSanPham = reader.GetInt32("MASANPHAM"),
                            TenSanPham = reader.GetString("TENSANPHAM"),
                            MaNguyenLieu = reader.GetInt32("MANGUYENLIEU"),
                            TenNguyenLieu = reader.GetString("TENNGUYENLIEU"),
                            SoLuongCoSo = reader.GetFloat("SOLUONGCOSO")
                        };
                        ds.Add(ct);
                    }
                }

                DBConnect.CloseConnection(null);
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
                       (MASANPHAM, MANGUYENLIEU, SOLUONGCOSO, TRANGTHAI)
                       VALUES (@masp, @manl, @soluong, @trangthai)";

                using (MySqlConnection conn = DBConnect.GetConnection())
                {
                    // ❌ Đừng mở lại kết nối ở đây!
                    using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@masp", ct.MaSanPham);
                        cmd.Parameters.AddWithValue("@manl", ct.MaNguyenLieu);
                        cmd.Parameters.AddWithValue("@soluong", ct.SoLuongCoSo);
                        cmd.Parameters.AddWithValue("@trangthai", ct.TrangThai);

                        int rows = cmd.ExecuteNonQuery();
                        result = rows > 0;
                        Console.WriteLine($"✅ Thêm công thức: SP={ct.MaSanPham}, NL={ct.MaNguyenLieu}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi thêm công thức: " + ex.Message,
                                "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }





        // 🟡 Cập nhật định lượng
        public bool Sua(congThucDTO ct)
        {
            try
            {
                string qry = "UPDATE congthuc SET SOLUONGCOSO = @soluong, TRANGTHAI = @trangthai " +
                             "WHERE MASANPHAM = @masp AND MANGUYENLIEU = @manl";

                MySqlConnection conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@masp", ct.MaSanPham);
                cmd.Parameters.AddWithValue("@manl", ct.MaNguyenLieu);
                cmd.Parameters.AddWithValue("@soluong", ct.SoLuongCoSo);
                cmd.Parameters.AddWithValue("@trangthai", ct.TrangThai);

                cmd.ExecuteNonQuery();
                DBConnect.CloseConnection(conn);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi sửa công thức: " + e.Message);
                return false;
            }
        }

        // 🔴 Ẩn công thức (set trạng thái = 0)
        public bool Xoa(int maSP, int maNL)
        {
            try
            {
                string qry = "UPDATE congthuc SET TRANGTHAI = 0 WHERE MASANPHAM = @masp AND MANGUYENLIEU = @manl";

                MySqlConnection conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@masp", maSP);
                cmd.Parameters.AddWithValue("@manl", maNL);

                cmd.ExecuteNonQuery();
                DBConnect.CloseConnection(conn);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi xóa công thức: " + e.Message);
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
            INSERT INTO congthuc (MASANPHAM, MANGUYENLIEU, SOLUONGCOSO, TRANGTHAI)
            VALUES (@masp, @manl, @soluong, @trangthai)
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

    }
}
