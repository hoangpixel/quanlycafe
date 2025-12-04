using MySql.Data.MySqlClient;
using DAO.CONFIG;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DAO
{
    public class congThucDAO
    {
        public BindingList<congThucDTO> docDanhSachCongThucTheoSP(int maSP)
        {
            BindingList<congThucDTO> ds = new BindingList<congThucDTO>();
            string qry = @"
        SELECT c.MASANPHAM, c.MANGUYENLIEU, c.SOLUONGCOSO, c.MADONVICOSO, c.TRANGTHAI,
               n.TENNGUYENLIEU, d.TENDONVI
        FROM congthuc c
        JOIN nguyenlieu n ON c.MANGUYENLIEU = n.MANGUYENLIEU
        JOIN donvi d ON c.MADONVICOSO = d.MADONVI
        WHERE c.MASANPHAM = @masp AND c.TRANGTHAI = 1";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@masp", maSP);
                using (MySqlDataReader reader = cmd.ExecuteReader())
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

        public BindingList<congThucDTO> layDanhSach()
        {
            BindingList<congThucDTO> ds = new BindingList<congThucDTO>();
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = "SELECT * FROM congthuc";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader rs = cmd.ExecuteReader();
                while(rs.Read())
                {
                    congThucDTO ct = new congThucDTO();
                    ct.MaSanPham = rs.GetInt32("MASANPHAM");
                    ct.MaNguyenLieu = rs.GetInt32("MANGUYENLIEU");
                    ct.SoLuongCoSo = rs.GetFloat("SOLUONGCOSO");
                    ct.MaDonViCoSo = rs.GetInt32("MADONVICOSO");
                    ds.Add(ct);
                }
            }catch(MySqlException ex)
            {
                Console.WriteLine("Lỗi kh lấy đc danh sách công thức : " + ex);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return ds;
        }


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
                Console.WriteLine("Lỗi thêm công thức: " + ex.Message);
            }
            return result;
        }


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
                Console.WriteLine("Lỗi sửa công thức: " + e.Message);
                return false;
            }
        }

        public bool Xoa(int maSP, int maNL)
        {
            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    string qryDelete = "DELETE FROM congthuc WHERE MASANPHAM = @maSP AND MANGUYENLIEU = @maNL";
                    using (MySqlCommand cmd = new MySqlCommand(qryDelete, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@maSP", maSP);
                        cmd.Parameters.AddWithValue("@maNL", maNL);
                        cmd.ExecuteNonQuery();
                    }

                    string checkQry = "SELECT COUNT(*) FROM congthuc WHERE MASANPHAM = @maSP";
                    int count = 0;
                    using (MySqlCommand cmd = new MySqlCommand(checkQry, conn, trans))
                    {
                        cmd.Parameters.AddWithValue("@maSP", maSP);
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (count == 0)
                    {
                        string updateQry = "UPDATE sanpham SET TRANGTHAICT = 0 WHERE MASANPHAM = @maSP";
                        using (MySqlCommand cmd = new MySqlCommand(updateQry, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@maSP", maSP);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    Console.WriteLine("Lỗi xóa công thức: " + ex.Message);
                    return false;
                }
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
                Console.WriteLine("Lỗi thêm hoặc cập nhật CT: " + e.Message);
                return false;
            }
        }
    }
}