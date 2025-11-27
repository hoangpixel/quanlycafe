using DAO.CONFIG;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic; // Cần thiết cho List<>

namespace DAO
{
    public class ctPhieuNhapDAO
    {
        // 1. Insert chi tiết (sử dụng conn/tran truyền vào để đảm bảo tính toàn vẹn dữ liệu)
        public bool Insert(ctPhieuNhapDTO ct, MySqlConnection conn, MySqlTransaction tran)
        {
            string sql = @"INSERT INTO ctphieunhap (MAPN, MANGUYENLIEU, MADONVI, SOLUONG, SOLUONGCOSO, DONGIA, THANHTIEN)
                           VALUES (@mapn, @mangl, @madv, @soluong, @soluongcoso, @dongia, @thanhtien)";

            // Lưu ý: Không dùng 'using' ở đây vì conn và tran được quản lý ở bên ngoài (PhieuNhapDAO)
            MySqlCommand cmd = new MySqlCommand(sql, conn, tran);
            cmd.Parameters.AddWithValue("@mapn", ct.MaPN);
            cmd.Parameters.AddWithValue("@mangl", ct.MaNguyenLieu);
            cmd.Parameters.AddWithValue("@madv", ct.MaDonVi);
            cmd.Parameters.AddWithValue("@soluong", ct.SoLuong);
            cmd.Parameters.AddWithValue("@soluongcoso", ct.SoLuongCoSo);
            cmd.Parameters.AddWithValue("@dongia", ct.DonGia);
            cmd.Parameters.AddWithValue("@thanhtien", ct.ThanhTien);

            int rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        // 2. Xóa chi tiết theo Mã Phiếu Nhập
        public bool DeleteByMapn(int mapn)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                var cmd = new MySqlCommand("DELETE FROM ctphieunhap WHERE MAPN = @mapn", conn);
                cmd.Parameters.AddWithValue("@mapn", mapn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ctPhieuNhapDAO.DeleteByMapn: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        // 3. Lấy danh sách chi tiết để hiển thị
        public List<ctPhieuNhapDTO> LayDanhSachChiTietTheoMaPN(int maPN)
        {
            List<ctPhieuNhapDTO> list = new List<ctPhieuNhapDTO>();

            // Join bảng để lấy tên nguyên liệu và đơn vị hiển thị cho đẹp
            string sql = @"SELECT c.*, n.TENNGUYENLIEU, d.TENDONVI 
                           FROM ctphieunhap c
                           JOIN nguyenlieu n ON c.MANGUYENLIEU = n.MANGUYENLIEU
                           JOIN donvi d ON c.MADONVI = d.MADONVI
                           WHERE c.MAPN = @mapn";

            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mapn", maPN);

                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        ctPhieuNhapDTO ct = new ctPhieuNhapDTO
                        {
                            MaCTPN = rdr.GetInt32("MACTPN"),
                            MaPN = rdr.GetInt32("MAPN"),
                            MaNguyenLieu = rdr.GetInt32("MANGUYENLIEU"),
                            MaDonVi = rdr.GetInt32("MADONVI"), // Đã bổ sung
                            SoLuong = rdr.GetDecimal("SOLUONG"),
                            SoLuongCoSo = rdr.GetDecimal("SOLUONGCOSO"), 
                            DonGia = rdr.GetDecimal("DONGIA"),
                            ThanhTien = rdr.GetDecimal("THANHTIEN"),

                         
                            TenNguyenLieu = rdr["TENNGUYENLIEU"].ToString(),
                            TenDonVi = rdr["TENDONVI"].ToString()
                        };
                        list.Add(ct);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi lấy chi tiết PN: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return list;
        }
   
        public bool DeleteOne(int maPN, int maNL)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string sql = "DELETE FROM ctphieunhap WHERE MAPN = @mapn AND MANGUYENLIEU = @manl";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mapn", maPN);
                cmd.Parameters.AddWithValue("@manl", maNL);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }
        public decimal GetGiaNhapGanNhat(int maNguyenLieu)
        {
            decimal gia = 0;
            string sql = @"SELECT DONGIA FROM ctphieunhap 
                   WHERE MANGUYENLIEU = @manl 
                   ORDER BY MACTPN DESC LIMIT 1"; 

            MySqlConnection conn = DBConnect.GetConnection();
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
            finally { DBConnect.CloseConnection(conn); }
            return gia;
        }
        public bool DeleteByMaCTPN(int maCTPN)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string sql = "DELETE FROM ctphieunhap WHERE MACTPN = @mactpn";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mactpn", maCTPN);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xóa chi tiết: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }
    }
}