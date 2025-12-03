using DAO.CONFIG;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DAO
{
    public class ctPhieuNhapDAO
    {
        // 1. Insert chi tiết (Giữ nguyên, chuẩn rồi)
        public bool Insert(ctPhieuNhapDTO ct, MySqlConnection conn, MySqlTransaction tran)
        {
            // Bỏ MACTPN khỏi câu Insert
            string sql = @"INSERT INTO ctphieunhap (MAPN, MANGUYENLIEU, MADONVI, SOLUONG, SOLUONGCOSO, DONGIA, THANHTIEN) 
                           VALUES (@mapn, @mangl, @madv, @soluong, @soluongcoso, @dongia, @thanhtien)";

            MySqlCommand cmd = new MySqlCommand(sql, conn, tran);
            cmd.Parameters.AddWithValue("@mapn", ct.MaPN);
            cmd.Parameters.AddWithValue("@mangl", ct.MaNguyenLieu);
            cmd.Parameters.AddWithValue("@madv", ct.MaDonVi);
            cmd.Parameters.AddWithValue("@soluong", ct.SoLuong);
            cmd.Parameters.AddWithValue("@soluongcoso", ct.SoLuongCoSo);
            cmd.Parameters.AddWithValue("@dongia", ct.DonGia);
            cmd.Parameters.AddWithValue("@thanhtien", ct.ThanhTien);

            return cmd.ExecuteNonQuery() > 0;
        }

        // 2. Xóa chi tiết theo Mã Phiếu Nhập (Khi xóa phiếu thì xóa hết chi tiết)
        public bool DeleteByMapn(int mapn)
        {
            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    var cmd = new MySqlCommand("DELETE FROM ctphieunhap WHERE MAPN = @mapn", conn);
                    cmd.Parameters.AddWithValue("@mapn", mapn);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi DeleteByMapn: " + ex.Message);
                    return false;
                }
            }
        }

        // 3. Lấy danh sách chi tiết (Đã xóa MACTPN)
        public List<ctPhieuNhapDTO> LayDanhSachChiTietTheoMaPN(int maPN)
        {
            List<ctPhieuNhapDTO> list = new List<ctPhieuNhapDTO>();

            string qry = "SELECT * FROM ctphieunhap WHERE MAPN = @maPN";

            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(qry, conn);
                    cmd.Parameters.AddWithValue("@mapn", maPN);

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new ctPhieuNhapDTO
                            {
                                MaPN = rdr.GetInt32("MAPN"),
                                MaNguyenLieu = rdr.GetInt32("MANGUYENLIEU"),
                                MaDonVi = rdr.GetInt32("MADONVI"),
                                SoLuong = rdr.GetDecimal("SOLUONG"),
                                SoLuongCoSo = rdr.GetDecimal("SOLUONGCOSO"),
                                DonGia = rdr.GetDecimal("DONGIA"),
                                ThanhTien = rdr.GetDecimal("THANHTIEN"),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi LayDanhSachChiTiet: " + ex.Message);
                }
            }
            return list;
        }

        // 4. Xóa 1 dòng chi tiết (Dùng khóa phức hợp: MaPN + MaNL + MaDV)
        // Lưu ý: Nếu 1 phiếu nhập có thể nhập cùng 1 nguyên liệu nhưng khác đơn vị, 
        // thì cần thêm tham số MaDonVi vào đây để xóa chính xác.
        public bool DeleteOne(int maPN, int maNL, int maDV)
        {
            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    string sql = "DELETE FROM ctphieunhap WHERE MAPN = @mapn AND MANGUYENLIEU = @manl AND MADONVI = @madv";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@mapn", maPN);
                    cmd.Parameters.AddWithValue("@manl", maNL);
                    cmd.Parameters.AddWithValue("@madv", maDV); // Thêm điều kiện đơn vị cho chắc
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch { return false; }
            }
        }

        // 5. Lấy giá nhập gần nhất (Đã sửa logic JOIN)
        public decimal GetGiaNhapGanNhat(int maNguyenLieu)
        {
            decimal gia = 0;
            // Vì bỏ MACTPN, ta phải JOIN với bảng PHIEUNHAP để sắp xếp theo THOIGIAN
            string sql = @"SELECT ct.DONGIA 
                           FROM ctphieunhap ct
                           JOIN phieunhap pn ON ct.MAPN = pn.MAPN
                           WHERE ct.MANGUYENLIEU = @manl 
                           ORDER BY pn.THOIGIAN DESC 
                           LIMIT 1";

            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@manl", maNguyenLieu);
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        gia = Convert.ToDecimal(result);
                    }
                }
                catch { }
            }
            return gia;
        }
    }
}