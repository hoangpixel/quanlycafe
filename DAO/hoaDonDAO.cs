using MySql.Data.MySqlClient;
using DAO.CONFIG;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data;

namespace DAO
{
    public class hoaDonDAO
    {
        // 1. LẤY DANH SÁCH HÓA ĐƠN
        public BindingList<hoaDonDTO> LayDanhSach()
        {
            BindingList<hoaDonDTO> ds = new BindingList<hoaDonDTO>();
            string qry = @"
                SELECT 
                    hd.MAHOADON, 
                    hd.MABAN, 
                    hd.THOIGIANTAO, 
                    hd.TRANGTHAI,
                    hd.TONGTIEN
                FROM hoadon hd
                ORDER BY hd.MAHOADON DESC
                ";

            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    hoaDonDTO hd = new hoaDonDTO
                    {
                        MaHD = reader.GetInt32("MAHOADON"),
                        MaBan = Convert.ToInt32(reader["MABAN"]), // VARCHAR
                        ThoiGianTao = reader.GetDateTime("THOIGIANTAO"),
                        TrangThai = reader.GetBoolean("TRANGTHAI"),
                        TongTien = reader.GetDecimal("TONGTIEN")
                    };
                    ds.Add(hd);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách hóa đơn: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }

        // 2. THÊM HÓA ĐƠN + CHI TIẾT (CÓ TRANSACTION)
        public int Them(hoaDonDTO hd, BindingList<cthoaDonDTO> dsCT)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            MySqlTransaction tran = null;

            try
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                tran = conn.BeginTransaction();

                decimal tongTien = 0;
                foreach (var ct in dsCT)
                {
                    tongTien += ct.SoLuong * ct.DonGia;
                }
                hd.TongTien = tongTien;

                string sqlHD = @"
                INSERT INTO hoadon 
                    (MABAN, MATT, THOIGIANTAO, TRANGTHAI, TONGTIEN, MAKHACHHANG, MANHANVIEN)
                VALUES
                    (@MaBan, @MaTT, @ThoiGianTao, @TrangThai, 0, @MaKH, @MaNV);
                SELECT LAST_INSERT_ID();
                ";


                var cmdHD = new MySqlCommand(sqlHD, conn, tran);
                cmdHD.Parameters.AddWithValue("@MaBan", hd.MaBan);
                cmdHD.Parameters.AddWithValue("@MaTT", hd.MaTT);
                cmdHD.Parameters.AddWithValue("@MaKH", hd.MaKhachHang);
                cmdHD.Parameters.AddWithValue("@MaNV", hd.MaNhanVien);

                var thoiGian = hd.ThoiGianTao == default(DateTime)
                    ? DateTime.Now
                    : hd.ThoiGianTao;
                cmdHD.Parameters.AddWithValue("@ThoiGianTao", thoiGian);

                byte trangThaiValue = hd.TrangThai ? (byte)1 : (byte)0;

                cmdHD.Parameters.AddWithValue("@TrangThai", trangThaiValue);

                int maHD = Convert.ToInt32(cmdHD.ExecuteScalar());
                hd.MaHD = maHD;

                string sqlCT = @"
            INSERT INTO cthd (MAHOADON, MASANPHAM, SOLUONG, DONGIA, THANHTIEN)
            VALUES (@MaHD, @MaSP, @SoLuong, @DonGia, @ThanhTien);
        ";

                foreach (var ct in dsCT)
                {
                    decimal thanhTien = ct.SoLuong * ct.DonGia;
                    ct.maHD = maHD;
                    ct.ThanhTien = thanhTien;

                    var cmdCT = new MySqlCommand(sqlCT, conn, tran);
                    cmdCT.Parameters.AddWithValue("@MaHD", maHD);
                    cmdCT.Parameters.AddWithValue("@MaSP", ct.MaSP);
                    cmdCT.Parameters.AddWithValue("@SoLuong", ct.SoLuong);
                    cmdCT.Parameters.AddWithValue("@DonGia", ct.DonGia);
                    cmdCT.Parameters.AddWithValue("@ThanhTien", thanhTien);

                    cmdCT.ExecuteNonQuery();
                }

                tran.Commit();
                return maHD;     // 🔥 trả về mã hóa đơn
            }
            catch (MySqlException ex)
            {
                if (tran != null)
                    tran.Rollback();

                Console.WriteLine("Lỗi thêm hóa đơn: " + ex.Message);
                return -1;       // báo lỗi
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }



        // 3. CẬP NHẬT TRẠNG THÁI HÓA ĐƠN
        public bool CapNhatTrangThai(int maHD, string trangThai)
        {
            string qry = "UPDATE hoadon SET TRANGTHAI = @TrangThai WHERE MAHOADON = @MaHD";
            MySqlConnection conn = DBConnect.GetConnection();

            try
            {
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@TrangThai", trangThai);
                cmd.Parameters.AddWithValue("@MaHD", maHD);
                int rs = cmd.ExecuteNonQuery();

                Console.WriteLine($"Cập nhật trạng thái hóa đơn {maHD} → {trangThai}");
                return rs > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi cập nhật trạng thái: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        // 4. XÓA HÓA ĐƠN (ẨN HOẶC XÓA THẬT – TÙY YÊU CẦU)
        public bool Xoa(int maHD)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            MySqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                // Xóa chi tiết trước
                string qryCT = "DELETE FROM cthd WHERE MAHOADON = @MaHD";
                MySqlCommand cmdCT = new MySqlCommand(qryCT, conn, tran);
                cmdCT.Parameters.AddWithValue("@MaHD", maHD);
                cmdCT.ExecuteNonQuery();

                // Xóa hóa đơn
                string qryHD = "DELETE FROM hoadon WHERE MAHOADON = @MaHD";
                MySqlCommand cmdHD = new MySqlCommand(qryHD, conn, tran);
                cmdHD.Parameters.AddWithValue("@MaHD", maHD);
                cmdHD.ExecuteNonQuery();

                tran.Commit();
                Console.WriteLine($"Xóa hóa đơn {maHD} thành công!");
                return true;
            }
            catch (MySqlException ex)
            {
                if (tran != null) tran.Rollback();
                Console.WriteLine("Lỗi xóa hóa đơn: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        // 5. TÌM HÓA ĐƠN THEO MÃ
        public hoaDonDTO TimTheoMa(int maHD)
        {
            hoaDonDTO hd = null;
            string qry = "SELECT * FROM hoadon WHERE MAHOADON = @MaHD";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@MaHD", maHD);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        hd = new hoaDonDTO
                        {
                            MaHD = reader.GetInt32("MAHOADON"),
                            MaBan = Convert.ToInt32(reader["MABAN"]),
                            ThoiGianTao = reader.GetDateTime("THOIGIANTAO"),
                            TrangThai = reader.GetBoolean("TRANGTHAI"),
                            TongTien = reader.GetDecimal("TONGTIEN")
                        };
                    }
                }
            }

            return hd;
        }
        // Kiểm tra bàn đang có hóa đơn chưa thanh toán không
        public bool BanDangCoHoaDonChuaThanhToan(int maBan)
        {
            string qry = @"
        SELECT COUNT(*) 
        FROM hoadon 
        WHERE MABAN = @MaBan 
          AND TRANGTHAI = 0";  // 0 = chưa thanh toán

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@MaBan", maBan);
                //conn.Open();
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        // LẤY CHI TIẾT HÓA ĐƠN THEO MÃ HÓA ĐƠN
        public BindingList<cthoaDonDTO> LayChiTietHoaDon(int maHD)
        {
            BindingList<cthoaDonDTO> dsCT = new BindingList<cthoaDonDTO>();
            string qry = @"
        SELECT 
            cthd.MASANPHAM,
            sp.TENSANPHAM AS TENSP,
            cthd.SOLUONG,
            cthd.DONGIA,
            cthd.THANHTIEN
        FROM cthd 
        INNER JOIN sanpham sp ON cthd.MASANPHAM = sp.MASANPHAM
        WHERE cthd.MAHOADON = @MaHD";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@MaHD", maHD);
                //conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dsCT.Add(new cthoaDonDTO
                        {
                            MaSP = reader.GetInt32("MASANPHAM"),
                            TenSP = reader.GetString("TENSP"),
                            SoLuong = reader.GetInt32("SOLUONG"),
                            DonGia = reader.GetDecimal("DONGIA"),
                            ThanhTien = reader.GetDecimal("THANHTIEN")
                        });
                    }
                }
            }
            return dsCT;
        }

        // LẤY THÔNG TIN CHUNG CỦA HÓA ĐƠN (Bàn, Khách, Nhân viên, Thời gian, Tổng tiền)
        public hoaDonDTO LayThongTinHoaDon(int maHD)
        {
            string qry = @"
        SELECT 
        hd.MAHOADON,
        hd.MABAN,
        hd.THOIGIANTAO,
        hd.TONGTIEN,
        COALESCE(kh.TENKHACHHANG, 'Khách lẻ') AS TenKhach,
        COALESCE(nv.HOTEN, 'Nhân Viên') AS TenNV

        FROM hoadon hd
        LEFT JOIN khachhang kh ON hd.MAKHACHHANG = kh.MAKHACHHANG
        LEFT JOIN nhanvien nv ON hd.MANHANVIEN = nv.MANHANVIEN
        WHERE hd.MAHOADON = @MaHD";

            using (MySqlConnection conn = DBConnect.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(qry, conn))
            {
                cmd.Parameters.AddWithValue("@MaHD", maHD);
                //conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new hoaDonDTO
                        {
                            MaHD = reader.GetInt32("MAHOADON"),
                            MaBan = Convert.ToInt32(reader["MABAN"]),
                            ThoiGianTao = reader.GetDateTime("THOIGIANTAO"),
                            TongTien = reader.GetDecimal("TONGTIEN"),
                            // Các thông tin bổ sung
                            //TenKhuVuc = reader.IsDBNull(reader.GetOrdinal("TENKV")) ? "" : reader.GetString("TENKV"),
                            TenKhachHang = reader.GetString("TenKhach"),
                            HoTen = reader.GetString("TenNV")
                        };
                    }
                }
            }
            return null;
        }
        // XÓA HÓA ĐƠN + CHI TIẾT (AN TOÀN VỚI TRANSACTION)
        public bool KhoaHoaDon(int maHD)
        {
            using (var conn = DBConnect.GetConnection())
            {
                conn.Open();

                string qry = "UPDATE hoadon SET TrangThai = 1 WHERE MaHD = @maHD";

                using (var cmd = new MySqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@maHD", maHD);

                    // rows > 0 nghĩa là update thành công
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool UpdateTrangThai(int maHD, bool trangThai)
        {
            string sql = "UPDATE hoadon SET TRANGTHAI = @tt WHERE MAHOADON = @id";

            using (var conn = DBConnect.GetConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@tt", trangThai ? 1 : 0);
                cmd.Parameters.AddWithValue("@id", maHD);

                return cmd.ExecuteNonQuery() > 0;
            }
        }



    }
}