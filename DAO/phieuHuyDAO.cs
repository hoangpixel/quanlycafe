using DAO.CONFIG;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace DAO
{
    public class phieuHuyDAO
    {
        public BindingList<phieuHuyDTO> DocDanhSachPhieuHuy(int maHD)
        {
            BindingList<phieuHuyDTO> ds = new BindingList<phieuHuyDTO>();
            string qry = @"SELECT * FROM phieuhuy WHERE MAHOADON = @maHD";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("maHD", maHD);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    phieuHuyDTO ph = new phieuHuyDTO
                    {
                        MaPhieuHuy = reader.GetInt32("MAPHIEUHUY"),
                        MaNhanVien = reader.GetInt32("MANHANVIEN"),
                        MaHoaDon = reader.GetInt32("MAHOADON"),
                        MaNguyenLieu = reader.GetInt32("MANGUYENLIEU"),
                        SoLuong = reader.GetDecimal("SOLUONG"),
                        LyDo = reader.IsDBNull(reader.GetOrdinal("LYDO")) ? "" : reader.GetString("LYDO"),
                        NgayTao = reader.GetDateTime("NGAYTAO")
                    };
                    ds.Add(ph);
                }

                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách phiếu hủy: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }
        public int LayMa()
        {
            MySqlConnection conn = DBConnect.GetConnection();
            int maPhieu = 0;
            try
            {
                string qry = "SELECT IFNULL(MAX(MAPHIEUHUY), 0) FROM phieuhuy";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                maPhieu = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy mã phiếu hủy: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return maPhieu + 1;
        }
        private double LayHeSoQuyDoi(int maNguyenLieu, int maDonViNhap, MySqlConnection conn, MySqlTransaction trans)
        {
            int maDonViCoSo = -1;
            string qryCheckGoc = "SELECT MADONVICOSO FROM nguyenlieu WHERE MANGUYENLIEU = @maNL";

            using (MySqlCommand cmd = new MySqlCommand(qryCheckGoc, conn, trans))
            {
                cmd.Parameters.AddWithValue("@maNL", maNguyenLieu);
                object result = cmd.ExecuteScalar();
                if (result != null) maDonViCoSo = Convert.ToInt32(result);
            }
            if (maDonViNhap == maDonViCoSo)
            {
                return 1.0;
            }
            string qryHeSo = "SELECT HESO FROM hesodonvi WHERE MANGUYENLIEU = @maNL AND MADONVI = @maDV";
            using (MySqlCommand cmd = new MySqlCommand(qryHeSo, conn, trans))
            {
                cmd.Parameters.AddWithValue("@maNL", maNguyenLieu);
                cmd.Parameters.AddWithValue("@maDV", maDonViNhap);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    return Convert.ToDouble(result);
                }
            }
            throw new Exception("Chưa cấu hình quy đổi đơn vị cho nguyên liệu này!");
        }

        // Trong class phieuHuyDAO
        public double LayHeSoQuyDoiDonGian(int maNL, int maDonViNhap)
        {
            double heSo = 1.0;
            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                // 1. Lấy đơn vị gốc
                string qryGoc = "SELECT MADONVICOSO FROM nguyenlieu WHERE MANGUYENLIEU = @maNL";
                int maDonViGoc = -1;
                using (MySqlCommand cmd = new MySqlCommand(qryGoc, conn))
                {
                    cmd.Parameters.AddWithValue("@maNL", maNL);
                    object rs = cmd.ExecuteScalar();
                    if (rs != null) maDonViGoc = Convert.ToInt32(rs);
                }

                // Nếu đơn vị nhập == đơn vị gốc thì hệ số là 1
                if (maDonViNhap == maDonViGoc) return 1.0;

                // 2. Nếu khác, tìm trong bảng hệ số
                string qryHeSo = "SELECT HESO FROM hesodonvi WHERE MANGUYENLIEU = @maNL AND MADONVI = @maDV";
                using (MySqlCommand cmd = new MySqlCommand(qryHeSo, conn))
                {
                    cmd.Parameters.AddWithValue("@maNL", maNL);
                    cmd.Parameters.AddWithValue("@maDV", maDonViNhap);
                    object rs = cmd.ExecuteScalar();
                    if (rs != null) heSo = Convert.ToDouble(rs);
                }
            }
            return heSo;
        }

        public bool ThemPhieuHuyVaTruKho(phieuHuyDTO ph, int maDonViNhap, decimal soLuongNhap)
        {
            MySqlConnection conn = DBConnect.GetConnection();

            using (MySqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    double heSo = LayHeSoQuyDoi(ph.MaNguyenLieu, maDonViNhap, conn, trans);
                    decimal soLuongCanTru = soLuongNhap * (decimal)heSo;
                    ph.SoLuong = soLuongCanTru;
                    string qryUpdateKho = "UPDATE nguyenlieu SET TONKHO = TONKHO - @sl WHERE MANGUYENLIEU = @maNL";
                    using (MySqlCommand cmdUpdate = new MySqlCommand(qryUpdateKho, conn, trans))
                    {
                        cmdUpdate.Parameters.AddWithValue("@sl", soLuongCanTru); // Trừ số đã quy đổi
                        cmdUpdate.Parameters.AddWithValue("@maNL", ph.MaNguyenLieu);

                        int row = cmdUpdate.ExecuteNonQuery();
                        if (row == 0) throw new Exception("Nguyên liệu không tồn tại hoặc lỗi cập nhật.");
                    }
                    string qryInsert = @"INSERT INTO phieuhuy (MANHANVIEN, MAHOADON, MANGUYENLIEU, SOLUONG, LYDO, NGAYTAO) 
                                         VALUES (@nv, @hd, @nl, @sl, @lydo, NOW())";

                    using (MySqlCommand cmdInsert = new MySqlCommand(qryInsert, conn, trans))
                    {
                        cmdInsert.Parameters.AddWithValue("@nv", ph.MaNhanVien);
                        cmdInsert.Parameters.AddWithValue("@hd", ph.MaHoaDon);
                        cmdInsert.Parameters.AddWithValue("@nl", ph.MaNguyenLieu);
                        cmdInsert.Parameters.AddWithValue("@sl", ph.SoLuong);
                        cmdInsert.Parameters.AddWithValue("@lydo", ph.LyDo);
                        cmdInsert.ExecuteNonQuery();
                    }

                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    Console.WriteLine("Lỗi xử lý phiếu hủy: " + ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}