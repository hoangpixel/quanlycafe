using DAO.CONFIG;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DAO
{
    public class phieuNhapDAO
    {
        public BindingList<phieuNhapDTO> LayDanhSach()
        {
            BindingList<phieuNhapDTO> ds = new BindingList<phieuNhapDTO>();
            string sql = @"SELECT * FROM phieunhap WHERE TRANGTHAIXOA = 1 ORDER BY phieunhap.MAPN DESC";
            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    var cmd = new MySqlCommand(sql, conn);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ds.Add(new phieuNhapDTO
                            {
                                MaPN = rdr.GetInt32("MAPN"),
                                MaNCC = rdr.GetInt32("MANCC"),
                                MaNhanVien = rdr.GetInt32("MANHANVIEN"),
                                ThoiGian = rdr.GetDateTime("THOIGIAN"),
                                TrangThai = rdr.GetInt32("TRANGTHAI"),
                                TongTien = rdr.GetDecimal("TONGTIEN"),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi LayDanhSach: " + ex.Message);
                }
            }
            return ds;
        }

        public int Insert(phieuNhapDTO pn, MySqlConnection conn, MySqlTransaction tran)
        {
            string sql = @"INSERT INTO phieunhap (MANCC, MANHANVIEN, THOIGIAN, TONGTIEN, TRANGTHAI, TRANGTHAIXOA) 
                           VALUES (@mancc, @manv, @thoigian, @tongtien, @trangthai, 1); 
                           SELECT LAST_INSERT_ID();";

            using (var cmd = new MySqlCommand(sql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@mancc", pn.MaNCC);
                cmd.Parameters.AddWithValue("@manv", pn.MaNhanVien);
                cmd.Parameters.AddWithValue("@thoigian", pn.ThoiGian);
                cmd.Parameters.AddWithValue("@tongtien", pn.TongTien);
                cmd.Parameters.AddWithValue("@trangthai", pn.TrangThai);
                
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public int ThemPhieuNhap(phieuNhapDTO header, List<ctPhieuNhapDTO> details)
        {
            if (details == null || details.Count == 0) throw new Exception("Không có chi tiết phiếu nhập");

            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                MySqlTransaction tran = conn.BeginTransaction();
                ctPhieuNhapDAO ctDAO = new ctPhieuNhapDAO();

                try
                {
                    decimal tongTien = details.Sum(ct => ct.ThanhTien);
                    header.TongTien = tongTien;

                    int newPhieuID = Insert(header, conn, tran);
                    if (newPhieuID <= 0) throw new Exception("Không thể tạo phiếu nhập.");

                    foreach (var ct in details)
                    {
                        ct.MaPN = newPhieuID;
                        
                        // Gọi DAO chi tiết (phải hỗ trợ transaction)
                        if (!ctDAO.Insert(ct, conn, tran)) 
                            throw new Exception($"Lỗi thêm sản phẩm:");

                        //string sqlKho = "UPDATE nguyenlieu SET TONKHO = TONKHO + @sl WHERE MANGUYENLIEU = @maNL";
                        //using(var cmdKho = new MySqlCommand(sqlKho, conn, tran))
                        //{
                        //    cmdKho.Parameters.AddWithValue("@sl", ct.SoLuongCoSo);
                        //    cmdKho.Parameters.AddWithValue("@maNL", ct.MaNguyenLieu);
                        //    cmdKho.ExecuteNonQuery();
                        //}
                    }

                    tran.Commit();
                    return newPhieuID;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception("Lỗi ThemPhieuNhap: " + ex.Message);
                }
            }
        }

        public bool DuyetPhieuNhap(int maPN)
        {
            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                MySqlTransaction tran = conn.BeginTransaction();

                try
                {
                    string sqlGetDetail = "SELECT MANGUYENLIEU, SOLUONGCOSO FROM ctphieunhap WHERE MAPN = @mapn";
                    var listChiTiet = new List<dynamic>();

                    using (var cmdGet = new MySqlCommand(sqlGetDetail, conn, tran))
                    {
                        cmdGet.Parameters.AddWithValue("@mapn", maPN);
                        using (var rdr = cmdGet.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                listChiTiet.Add(new
                                {
                                    MaNL = rdr.GetInt32("MANGUYENLIEU"),
                                    SlCoSo = rdr.GetDecimal("SOLUONGCOSO")
                                });
                            }
                        }
                    }

                    if (listChiTiet.Count == 0) return false;
                    string sqlUpdateKho = "UPDATE nguyenlieu SET TONKHO = TONKHO + @sl WHERE MANGUYENLIEU = @manl";
                    foreach (var item in listChiTiet)
                    {
                        using (var cmdKho = new MySqlCommand(sqlUpdateKho, conn, tran))
                        {
                            cmdKho.Parameters.AddWithValue("@sl", item.SlCoSo);
                            cmdKho.Parameters.AddWithValue("@manl", item.MaNL);
                            cmdKho.ExecuteNonQuery();
                        }
                    }
                    string sqlUpdateStatus = "UPDATE phieunhap SET TRANGTHAI = 1 WHERE MAPN = @mapn";
                    using (var cmdStatus = new MySqlCommand(sqlUpdateStatus, conn, tran))
                    {
                        cmdStatus.Parameters.AddWithValue("@mapn", maPN);
                        int rows = cmdStatus.ExecuteNonQuery();
                        if (rows == 0) throw new Exception("Không tìm thấy phiếu để duyệt.");
                    }

                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception("Lỗi duyệt phiếu: " + ex.Message);
                }
            }
        }
        public bool XoaPhieuNhap(int maPN)
        {
            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    string qry = "UPDATE phieunhap SET TRANGTHAIXOA = 0 WHERE MAPN = @maPN";

                    MySqlCommand cmd = new MySqlCommand(qry, conn);
                    cmd.Parameters.AddWithValue("@maPN", maPN);

                    int rs = cmd.ExecuteNonQuery();
                    return rs > 0;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Lỗi xóa phiếu nhập: " + ex.Message);
                    return false;
                }
            }
        }

        public bool CapNhatThongTin(int maPN, int maNCC, int MANHANVIEN)
        {
            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    string sql = "UPDATE phieunhap SET MANCC = @mancc, MANHANVIEN = @manv WHERE MAPN = @mapn";
                    var cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@mancc", maNCC);
                    cmd.Parameters.AddWithValue("@manv", MANHANVIEN);
                    cmd.Parameters.AddWithValue("@mapn", maPN);
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch { return false; }
            }
        }
        public bool ThemChiTietVaoPhieuCu(int maPN, List<ctPhieuNhapDTO> details)
        {
            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                MySqlTransaction tran = conn.BeginTransaction();
                ctPhieuNhapDAO ctDAO = new ctPhieuNhapDAO();

                try
                {
                    string sqlCheck = "SELECT TRANGTHAI FROM phieunhap WHERE MAPN = @mapn";
                    var cmdCheck = new MySqlCommand(sqlCheck, conn, tran);
                    cmdCheck.Parameters.AddWithValue("@mapn", maPN);
                    int trangThai = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    if (trangThai == 1)
                    {
                        throw new Exception("Phiếu đã duyệt, không thể thêm chi tiết!");
                    }

                    foreach (var ct in details)
                    {
                        ct.MaPN = maPN;
                        if (!ctDAO.Insert(ct, conn, tran)) throw new Exception("Lỗi thêm chi tiết.");
                    }

                    string sqlSum = "SELECT SUM(THANHTIEN) FROM ctphieunhap WHERE MAPN = @mapn";
                    var cmdSum = new MySqlCommand(sqlSum, conn, tran);
                    cmdSum.Parameters.AddWithValue("@mapn", maPN);
                    object result = cmdSum.ExecuteScalar();
                    decimal tongTienMoi = (result != DBNull.Value) ? Convert.ToDecimal(result) : 0;

                    var cmdHeader = new MySqlCommand("UPDATE phieunhap SET TONGTIEN = @tt, THOIGIAN = NOW() WHERE MAPN = @mapn", conn, tran);
                    cmdHeader.Parameters.AddWithValue("@tt", tongTienMoi);
                    cmdHeader.Parameters.AddWithValue("@mapn", maPN);
                    cmdHeader.ExecuteNonQuery();

                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception("Lỗi ThemChiTietVaoPhieuCu: " + ex.Message);
                }
            }
        }

        // Trong class phieuNhapDAO
        //public bool CapNhatPhieuNhap(phieuNhapDTO header, List<ctPhieuNhapDTO> details)
        //{
        //    using (MySqlConnection conn = DBConnect.GetConnection())
        //    {
        //        MySqlTransaction tran = conn.BeginTransaction();
        //        ctPhieuNhapDAO ctDAO = new ctPhieuNhapDAO();

        //        try
        //        {
        //            // 1. CẬP NHẬT THÔNG TIN CHUNG (HEADER)
        //            string sqlUpdateHeader = @"UPDATE phieunhap 
        //                               SET MANCC = @mancc, MANHANVIEN = @manv, 
        //                                   THOIGIAN = @thoigian, TONGTIEN = @tongtien 
        //                               WHERE MAPN = @mapn";

        //            using (var cmd = new MySqlCommand(sqlUpdateHeader, conn, tran))
        //            {
        //                cmd.Parameters.AddWithValue("@mancc", header.MaNCC);
        //                cmd.Parameters.AddWithValue("@manv", header.MaNhanVien);
        //                cmd.Parameters.AddWithValue("@thoigian", header.ThoiGian);
        //                cmd.Parameters.AddWithValue("@tongtien", header.TongTien);
        //                cmd.Parameters.AddWithValue("@mapn", header.MaPN);
        //                cmd.ExecuteNonQuery();
        //            }

        //            // 2. XÓA SẠCH CHI TIẾT CŨ
        //            string sqlDeleteOld = "DELETE FROM ctphieunhap WHERE MAPN = @mapn";
        //            using (var cmdDel = new MySqlCommand(sqlDeleteOld, conn, tran))
        //            {
        //                cmdDel.Parameters.AddWithValue("@mapn", header.MaPN);
        //                cmdDel.ExecuteNonQuery();
        //            }

        //            // 3. THÊM LẠI CHI TIẾT MỚI (Từ danh sách trên màn hình)
        //            foreach (var ct in details)
        //            {
        //                ct.MaPN = header.MaPN; // Gán lại MaPN cho chắc

        //                // Gọi hàm Insert của ctDAO (nhớ hàm này KHÔNG được Open connection nhé)
        //                if (!ctDAO.Insert(ct, conn, tran))
        //                {
        //                    throw new Exception("Lỗi khi thêm lại chi tiết:");
        //                }
        //            }

        //            tran.Commit();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            tran.Rollback();
        //            throw new Exception("Lỗi cập nhật phiếu: " + ex.Message);
        //        }
        //    }
        //}
        public bool CapNhatPhieuNhap(phieuNhapDTO header, List<ctPhieuNhapDTO> details)
        {
            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                MySqlTransaction tran = conn.BeginTransaction();
                ctPhieuNhapDAO ctDAO = new ctPhieuNhapDAO();

                try
                {
                    string sqlCheck = "SELECT COUNT(*) FROM phieunhap WHERE MAPN = @mapn";
                    var cmdCheck = new MySqlCommand(sqlCheck, conn, tran);
                    cmdCheck.Parameters.AddWithValue("@mapn", header.MaPN);
                    if (Convert.ToInt32(cmdCheck.ExecuteScalar()) == 0)
                    {
                        throw new Exception("Phiếu nhập không tồn tại để cập nhật!");
                    }

                    string sqlUpdateHeader = @"UPDATE phieunhap 
                                       SET MANCC = @mancc, MANHANVIEN = @manv, 
                                           THOIGIAN = @thoigian, TONGTIEN = @tongtien,
                                           TRANGTHAI = @trangthai
                                       WHERE MAPN = @mapn";

                    using (var cmd = new MySqlCommand(sqlUpdateHeader, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@mancc", header.MaNCC);
                        cmd.Parameters.AddWithValue("@manv", header.MaNhanVien);
                        cmd.Parameters.AddWithValue("@thoigian", header.ThoiGian);
                        cmd.Parameters.AddWithValue("@tongtien", header.TongTien);
                        cmd.Parameters.AddWithValue("@trangthai", header.TrangThai);
                        cmd.Parameters.AddWithValue("@mapn", header.MaPN);
                        cmd.ExecuteNonQuery();
                    }

                    string sqlDeleteOld = "DELETE FROM ctphieunhap WHERE MAPN = @mapn";
                    using (var cmdDel = new MySqlCommand(sqlDeleteOld, conn, tran))
                    {
                        cmdDel.Parameters.AddWithValue("@mapn", header.MaPN);
                        cmdDel.ExecuteNonQuery();
                    }

                    foreach (var ct in details)
                    {
                        ct.MaPN = header.MaPN;
                        if (!ctDAO.Insert(ct, conn, tran))
                        {
                            throw new Exception("Lỗi khi thêm lại chi tiết.");
                        }
                    }

                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public int ThemPhieuNhapTuExcel(phieuNhapDTO header, List<ctPhieuNhapDTO> details)
        {
            if (details == null || details.Count == 0) throw new Exception("Excel không có chi tiết sản phẩm.");

            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                MySqlTransaction tran = conn.BeginTransaction();
                ctPhieuNhapDAO ctDAO = new ctPhieuNhapDAO();

                try
                {
                    header.TongTien = details.Sum(ct => ct.ThanhTien);
                    int newPhieuID = Insert(header, conn, tran);
                    if (newPhieuID <= 0) throw new Exception("Không thể tạo phiếu nhập từ Excel.");

                    string sqlCongKho = "UPDATE nguyenlieu SET TONKHO = TONKHO + @sl WHERE MANGUYENLIEU = @maNL";

                    foreach (var ct in details)
                    {
                        ct.MaPN = newPhieuID;

                        if (!ctDAO.Insert(ct, conn, tran))
                            throw new Exception($"Lỗi thêm chi tiết sản phẩm {ct.MaNguyenLieu}");
                        if (header.TrangThai == 1)
                        {
                            using (var cmdKho = new MySqlCommand(sqlCongKho, conn, tran))
                            {
                                cmdKho.Parameters.AddWithValue("@sl", ct.SoLuongCoSo);
                                cmdKho.Parameters.AddWithValue("@maNL", ct.MaNguyenLieu);
                                cmdKho.ExecuteNonQuery();
                            }
                        }
                    }

                    tran.Commit();
                    return newPhieuID;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception("Lỗi nhập Excel: " + ex.Message);
                }
            }
        }

    }
}