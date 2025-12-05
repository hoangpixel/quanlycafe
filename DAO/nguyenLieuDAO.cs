using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DAO.CONFIG;
using DTO;
using System.ComponentModel;
using System.Windows;

namespace DAO
{
    public class nguyenLieuDAO
    {
        public BindingList<nguyenLieuDTO> docDanhSachNguyenLieu()
        {
            BindingList<nguyenLieuDTO> ds = new BindingList<nguyenLieuDTO>();
            string qry = "SELECT * FROM nguyenlieu WHERE TRANGTHAI = 1 ORDER BY nguyenlieu.MANGUYENLIEU DESC";
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
                        MaDonViCoSo = reader.GetInt32("MADONVICOSO"),
                        TrangThai = reader.GetInt32("TRANGTHAI"),
                        TonKho = reader.GetDecimal("TONKHO"),
                        TrangThaiDV = reader.GetInt32("TRANGTHAIDV")
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

        public int layMa()
        {
            MySqlConnection conn = DBConnect.GetConnection();
            int maNguyenLieu = 0;
            try
            {
                string qry = "SELECT IFNULL(MAX(MANGUYENLIEU), 0) FROM nguyenlieu";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                maNguyenLieu = Convert.ToInt32(cmd.ExecuteScalar());
            }catch(MySqlException ex)
            {
                Console.WriteLine("Lỗi kh lấy đc mã nl : " + ex);
            }finally
            {
                DBConnect.CloseConnection(conn);
            }
            return maNguyenLieu + 1;
        }
        public bool Them(nguyenLieuDTO nl)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = @"INSERT INTO nguyenlieu (MANGUYENLIEU ,TENNGUYENLIEU, MADONVICOSO, TRANGTHAI, TONKHO)
                               VALUES (@MaNguyenlieu,@Ten, @DonVi, @TrangThai, @TonKho)";

                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@MaNguyenlieu", nl.MaNguyenLieu);
                cmd.Parameters.AddWithValue("@Ten", nl.TenNguyenLieu);
                cmd.Parameters.AddWithValue("@DonVi", nl.MaDonViCoSo);
                cmd.Parameters.AddWithValue("@TrangThai", nl.TrangThai);
                cmd.Parameters.AddWithValue("@TonKho", nl.TonKho);

                int rs = cmd.ExecuteNonQuery();
                if (rs > 0)
                {
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
                               SET TENNGUYENLIEU = @Ten, MADONVICOSO = @DonVi, TONKHO = @TonKho, TRANGTHAI = @TrangThai
                               WHERE MANGUYENLIEU = @Ma";

                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@Ten", nl.TenNguyenLieu);
                cmd.Parameters.AddWithValue("@DonVi", nl.MaDonViCoSo);
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
                            MaDonViCoSo = reader.GetInt32("MADONVICOSO"),
                            TonKho = reader.GetDecimal("TONKHO")
                        };
                    }
                }
            }
            return nl;
        }

        public bool TruTonKhoKhiBanHang(Dictionary<int, int> gioHang)
        {
            MySqlConnection conn = DBConnect.GetConnection();

            // Đảm bảo kết nối mở
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }

            using (MySqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    foreach (var item in gioHang)
                    {
                        int maSP = item.Key;
                        int soLuongBan = item.Value;

                        string queryInfo = @"
                            SELECT 
                                CT.MANGUYENLIEU,
                                CT.SOLUONGCOSO AS DinhLuong,
                                CASE 
                                    WHEN CT.MADONVICOSO = NL.MADONVICOSO THEN 1
                                    ELSE IFNULL(HS.HESO, 0) 
                                END AS HeSoQuyDoi
                            FROM congthuc CT
                            JOIN nguyenlieu NL ON CT.MANGUYENLIEU = NL.MANGUYENLIEU
                            LEFT JOIN hesodonvi HS ON CT.MANGUYENLIEU = HS.MANGUYENLIEU 
                                                   AND CT.MADONVICOSO = HS.MADONVI
                            WHERE CT.MASANPHAM = @maSP AND CT.TRANGTHAI = 1";

                        var listUpdate = new List<(int MaNL, double LuongTru)>();

                        using (MySqlCommand cmdInfo = new MySqlCommand(queryInfo, conn, trans))
                        {
                            cmdInfo.Parameters.AddWithValue("@maSP", maSP);
                            using (MySqlDataReader reader = cmdInfo.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int maNL = reader.GetInt32("MANGUYENLIEU");
                                    double dinhLuong = reader.GetDouble("DinhLuong");
                                    double heSo = reader.GetDouble("HeSoQuyDoi");

                                    // Tính toán số lượng cần trừ
                                    double tongTru = soLuongBan * dinhLuong * heSo;

                                    if (tongTru > 0)
                                    {
                                        listUpdate.Add((maNL, tongTru));
                                    }
                                }
                            }
                        }

                        // Cập nhật Database
                        foreach (var update in listUpdate)
                        {
                            string queryUpdate = "UPDATE nguyenlieu SET TONKHO = TONKHO - @luongTru WHERE MANGUYENLIEU = @maNL";
                            using (MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, conn, trans))
                            {
                                cmdUpdate.Parameters.AddWithValue("@luongTru", update.LuongTru);
                                cmdUpdate.Parameters.AddWithValue("@maNL", update.MaNL);
                                cmdUpdate.ExecuteNonQuery();
                            }
                        }
                    }

                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    Console.WriteLine("Lỗi trừ kho: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
