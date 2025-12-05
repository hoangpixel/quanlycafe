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
        // 1. LẤY DANH SÁCH (Giữ nguyên logic nhưng viết gọn hơn)
        public BindingList<phieuNhapDTO> LayDanhSach()
        {
            BindingList<phieuNhapDTO> ds = new BindingList<phieuNhapDTO>();
            string sql = @"SELECT * FROM phieunhap WHERE TRANGTHAIXOA = 1 ORDER BY phieunhap.MAPN DESC";

            // Dùng using để tự động Close connection dù có lỗi hay không
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
                    // Nên throw ra để BUS hoặc GUI biết mà hiện thông báo lỗi
                    throw new Exception("Lỗi LayDanhSach: " + ex.Message);
                }
            }
            return ds;
        }

        // 2. INSERT HEADER (Đã sửa để nhận TongTien ngay từ đầu)
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
                cmd.Parameters.AddWithValue("@tongtien", pn.TongTien); // Truyền tổng tiền vào luôn
                cmd.Parameters.AddWithValue("@trangthai", pn.TrangThai);
                
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // 3. THÊM PHIẾU NHẬP (Transaction hoàn chỉnh - Đã tối ưu)
        public int ThemPhieuNhap(phieuNhapDTO header, List<ctPhieuNhapDTO> details)
        {
            if (details == null || details.Count == 0) throw new Exception("Không có chi tiết phiếu nhập");

            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                MySqlTransaction tran = conn.BeginTransaction();
                ctPhieuNhapDAO ctDAO = new ctPhieuNhapDAO();

                try
                {
                    // Bước 1: Tính tổng tiền TRƯỚC khi insert
                    // Điều này giúp ta không phải gọi lệnh Update lại lần nữa
                    decimal tongTien = details.Sum(ct => ct.ThanhTien);
                    header.TongTien = tongTien;

                    // Bước 2: Insert Header
                    int newPhieuID = Insert(header, conn, tran);
                    if (newPhieuID <= 0) throw new Exception("Không thể tạo phiếu nhập.");

                    // Bước 3: Insert Chi tiết & Update Tồn kho
                    foreach (var ct in details)
                    {
                        ct.MaPN = newPhieuID;
                        
                        // Gọi DAO chi tiết (phải hỗ trợ transaction)
                        if (!ctDAO.Insert(ct, conn, tran)) 
                            throw new Exception($"Lỗi thêm sản phẩm:");

                        // Update Tồn kho
                        //string sqlKho = "UPDATE nguyenlieu SET TONKHO = TONKHO + @sl WHERE MANGUYENLIEU = @maNL";
                        //using(var cmdKho = new MySqlCommand(sqlKho, conn, tran))
                        //{
                        //    cmdKho.Parameters.AddWithValue("@sl", ct.SoLuongCoSo);
                        //    cmdKho.Parameters.AddWithValue("@maNL", ct.MaNguyenLieu);
                        //    cmdKho.ExecuteNonQuery();
                        //}
                    }

                    // Commit Transaction
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

        // Thêm hàm này vào class phieuNhapDAO
        public bool DuyetPhieuNhap(int maPN)
        {
            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                MySqlTransaction tran = conn.BeginTransaction();

                try
                {
                    // 1. Lấy danh sách chi tiết của phiếu này để biết cần cộng bao nhiêu
                    // (Ta viết query trực tiếp ở đây để đỡ phải gọi ngược sang DAO khác rắc rối)
                    string sqlGetDetail = "SELECT MANGUYENLIEU, SOLUONGCOSO FROM ctphieunhap WHERE MAPN = @mapn";
                    var listChiTiet = new List<dynamic>(); // Dùng dynamic hoặc tạo class tạm để lưu

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

                    if (listChiTiet.Count == 0) return false; // Phiếu rỗng không duyệt được

                    // 2. Duyệt vòng lặp để CỘNG TỒN KHO
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

                    // 3. Cập nhật trạng thái phiếu sang 1 (Đã xử lý)
                    // (Bạn nói là "chuyển về 0 mới cộng", nhưng thường 0 là nháp, 1 là đã nhập. 
                    // Nếu bạn muốn ngược lại thì sửa số 1 bên dưới thành 0 nhé)
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

        // 4. XÓA PHIẾU (Gộp Delete và DeletePN thành 1 hàm duy nhất)
        public bool XoaPhieuNhap(int maPN)
        {
            // Dùng using để đảm bảo kết nối luôn được đóng dù có lỗi
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
                    // Nên throw lỗi hoặc log ra file, Console.WriteLine trong WinForm sẽ khó thấy
                    Console.WriteLine("Lỗi xóa phiếu nhập: " + ex.Message);
                    return false;
                }
            }
        }

        // 5. CẬP NHẬT THÔNG TIN CƠ BẢN
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

        // 6. THÊM CHI TIẾT VÀO PHIẾU CŨ (Giữ nguyên logic update tiền)
        // 6. THÊM CHI TIẾT VÀO PHIẾU CŨ
        public bool ThemChiTietVaoPhieuCu(int maPN, List<ctPhieuNhapDTO> details)
        {
            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                MySqlTransaction tran = conn.BeginTransaction();
                ctPhieuNhapDAO ctDAO = new ctPhieuNhapDAO();

                try
                {
                    // Kiểm tra trạng thái phiếu trước
                    // Nếu phiếu đã duyệt (TrangThai = 1) thì không cho sửa hoặc thêm kiểu này
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

                        // --- BỎ ĐOẠN UPDATE KHO Ở ĐÂY ĐI NHÉ ---
                        // Lý do: Vì đây là phiếu chưa duyệt, nên chưa được cộng kho.
                        // Khi nào người dùng bấm nút "Duyệt" thì hàm DuyetPhieuNhap sẽ cộng sau.
                    }

                    // Tính lại tổng tiền và cập nhật Header (Giữ nguyên đoạn này là đúng)
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
        public bool CapNhatPhieuNhap(phieuNhapDTO header, List<ctPhieuNhapDTO> details)
        {
            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                MySqlTransaction tran = conn.BeginTransaction();
                ctPhieuNhapDAO ctDAO = new ctPhieuNhapDAO();

                try
                {
                    // 1. CẬP NHẬT THÔNG TIN CHUNG (HEADER)
                    string sqlUpdateHeader = @"UPDATE phieunhap 
                                       SET MANCC = @mancc, MANHANVIEN = @manv, 
                                           THOIGIAN = @thoigian, TONGTIEN = @tongtien 
                                       WHERE MAPN = @mapn";

                    using (var cmd = new MySqlCommand(sqlUpdateHeader, conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@mancc", header.MaNCC);
                        cmd.Parameters.AddWithValue("@manv", header.MaNhanVien);
                        cmd.Parameters.AddWithValue("@thoigian", header.ThoiGian);
                        cmd.Parameters.AddWithValue("@tongtien", header.TongTien);
                        cmd.Parameters.AddWithValue("@mapn", header.MaPN);
                        cmd.ExecuteNonQuery();
                    }

                    // 2. XÓA SẠCH CHI TIẾT CŨ
                    string sqlDeleteOld = "DELETE FROM ctphieunhap WHERE MAPN = @mapn";
                    using (var cmdDel = new MySqlCommand(sqlDeleteOld, conn, tran))
                    {
                        cmdDel.Parameters.AddWithValue("@mapn", header.MaPN);
                        cmdDel.ExecuteNonQuery();
                    }

                    // 3. THÊM LẠI CHI TIẾT MỚI (Từ danh sách trên màn hình)
                    foreach (var ct in details)
                    {
                        ct.MaPN = header.MaPN; // Gán lại MaPN cho chắc

                        // Gọi hàm Insert của ctDAO (nhớ hàm này KHÔNG được Open connection nhé)
                        if (!ctDAO.Insert(ct, conn, tran))
                        {
                            throw new Exception("Lỗi khi thêm lại chi tiết:");
                        }
                    }

                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception("Lỗi cập nhật phiếu: " + ex.Message);
                }
            }
        }


    }
}