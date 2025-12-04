using DAO.CONFIG;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

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
                    hd.TONGTIEN,
                    hd.KHOASO,
                    hd.MATT
                FROM hoadon hd
                WHERE hd.TRANGTHAIXOA = 1
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
                        MaTT=reader.GetInt32("MATT"),
                        TongTien = reader.GetDecimal("TONGTIEN"),
                        KhoaSo = reader.GetInt32("KHOASO")
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

        public int Them(hoaDonDTO hd, BindingList<cthoaDonDTO> dsCT)
        {
            if (hd == null)
                throw new Exception("hoaDonDTO (hd) đang NULL!");

            if (dsCT == null || dsCT.Count == 0)
                throw new Exception("Danh sách chi tiết hóa đơn (dsCT) NULL hoặc không có sản phẩm!");

            MySqlConnection conn = DBConnect.GetConnection();
            MySqlTransaction tran = null;

            try
            {
                // ĐẢM BẢO CHẮC CHẮN KẾT NỐI ĐƯỢC MỞ
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                // PHẢI CÓ NGOẶC {}, NẾU KHÔNG tran SẼ LUÔN NULL
                tran = conn.BeginTransaction();

                //-------------------------------------
                // TÍNH TỔNG TIỀN
                //-------------------------------------
                /*decimal tongTien = 0;
                foreach (var ct in dsCT)
                {
                    tongTien += ct.SoLuong * ct.DonGia;
                }
                hd.TongTien = tongTien;*/

                //-------------------------------------
                // INSERT HÓA ĐƠN
                //-------------------------------------

                string sqlHD = @"
            INSERT INTO hoadon 
                (MABAN, MATT, THOIGIANTAO, TRANGTHAI, TONGTIEN, MAKHACHHANG, MANHANVIEN)
            VALUES
                (@MaBan, @MaTT, @ThoiGianTao, 1, @TongTien, @MaKH, @MaNV);
            SELECT LAST_INSERT_ID();
        ";

                var cmdHD = new MySqlCommand(sqlHD, conn, tran);
                cmdHD.Parameters.AddWithValue("@MaBan", hd.MaBan);
                cmdHD.Parameters.AddWithValue("@MaTT", hd.MaTT);
                cmdHD.Parameters.AddWithValue("@MaKH", hd.MaKhachHang);
                cmdHD.Parameters.AddWithValue("@MaNV", hd.MaNhanVien);
                cmdHD.Parameters.AddWithValue("@TongTien", hd.TongTien);

                var thoiGian = hd.ThoiGianTao == default(DateTime)
                    ? DateTime.Now
                    : hd.ThoiGianTao;
                cmdHD.Parameters.AddWithValue("@ThoiGianTao", thoiGian);

                int maHD = Convert.ToInt32(cmdHD.ExecuteScalar());
                hd.MaHD = maHD; // Gán mã hóa đơn cho DTO

                //-------------------------------------
                // INSERT CHI TIẾT HÓA ĐƠN
                //-------------------------------------

                string sqlCT = @"
            INSERT INTO cthd 
                (MAHOADON, MASANPHAM, SOLUONG, DONGIA, THANHTIEN)
            VALUES 
                (@MaHD, @MaSP, @SoLuong, @DonGia, @ThanhTien);
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

                //-------------------------------------
                // COMMIT TRANSACTION
                //-------------------------------------
                tran.Commit();
                return maHD;
            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();

                Console.WriteLine("Lỗi thêm hóa đơn: " + ex.Message);
                return -1;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public int SuaHD(hoaDonDTO hd, BindingList<cthoaDonDTO> dsCT)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            MySqlTransaction tran = null;

            try
            {
                tran = conn.BeginTransaction();

                // 1. Tính tổng tiền
                decimal tongTien = 0;
                foreach (var ct in dsCT)
                {
                    tongTien += ct.SoLuong * ct.DonGia;
                }
                hd.TongTien = tongTien;

                // 2. Insert Hóa Đơn
                // Lưu ý: Đã sửa số 0 thành @TongTien
                string sqlHD = @"
        INSERT INTO hoadon 
            (MABAN, MATT, THOIGIANTAO, TRANGTHAI, TONGTIEN, MAKHACHHANG, MANHANVIEN)
        VALUES
            (@MaBan, @MaTT, @ThoiGianTao, 1, @TongTien, @MaKH, @MaNV);
        SELECT LAST_INSERT_ID();
        ";

                var cmdHD = new MySqlCommand(sqlHD, conn, tran);
                cmdHD.Parameters.AddWithValue("@MaBan", hd.MaBan);
                cmdHD.Parameters.AddWithValue("@MaTT", hd.MaTT);

                // --- SỬA LỖI QUAN TRỌNG Ở ĐÂY ---
                // Nếu Mã KH <= 0 (Khách lẻ) thì truyền NULL vào database
                if (hd.MaKhachHang <= 0)
                {
                    cmdHD.Parameters.AddWithValue("@MaKH", DBNull.Value);
                }
                else
                {
                    cmdHD.Parameters.AddWithValue("@MaKH", hd.MaKhachHang);
                }
                // --------------------------------

                cmdHD.Parameters.AddWithValue("@MaNV", hd.MaNhanVien);
                cmdHD.Parameters.AddWithValue("@TongTien", hd.TongTien); // Truyền tổng tiền vào

                var thoiGian = hd.ThoiGianTao == default(DateTime)
                    ? DateTime.Now
                    : hd.ThoiGianTao;
                cmdHD.Parameters.AddWithValue("@ThoiGianTao", thoiGian);

                // Thực thi insert hóa đơn
                int maHD = Convert.ToInt32(cmdHD.ExecuteScalar());
                hd.MaHD = maHD;

                // 3. Insert Chi Tiết Hóa Đơn
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
                return maHD;
            }
            catch (MySqlException ex)
            {
                if (tran != null)
                    tran.Rollback();
                Console.WriteLine("Lỗi thêm hóa đơn chi tiết: " + ex.Message);
                return -1;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }
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
        public bool Xoa(int maHD)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            MySqlTransaction tran = null;

            try
            {

                tran = conn.BeginTransaction();

                string qryCT = "DELETE FROM cthd WHERE MAHOADON = @MaHD";
                MySqlCommand cmdCT = new MySqlCommand(qryCT, conn, tran);
                cmdCT.Parameters.AddWithValue("@MaHD", maHD);
                cmdCT.ExecuteNonQuery();

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
                            TenKhachHang = reader.GetString("TenKhach"),
                            HoTen = reader.GetString("TenNV")
                        };
                    }
                }
            }
            return null;
        }
        public bool KhoaHoaDon(int maHD)
        {
            using (var conn = DBConnect.GetConnection())
            {
                string qry = "UPDATE hoadon SET TrangThai = 1 WHERE MaHD = @maHD";

                using (var cmd = new MySqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@maHD", maHD);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool UpdateTrangThai(int maHD)
        {
            string sql = "UPDATE hoadon SET TRANGTHAIXOA = 0 WHERE MAHOADON = @id";

            using (var conn = DBConnect.GetConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", maHD);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public int layMa()
        {
            MySqlConnection conn = DBConnect.GetConnection();
            int maHD = 0;
            try
            {
                string qry = "SELECT IFNULL(MAX(MAHOADON), 0) FROM hoadon";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                maHD = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy mã HD: " + ex);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return maHD + 1;
        }

        public bool UpdateKhoaSo(int maBan)
        {
            string sql = "UPDATE hoadon SET KHOASO = 1 WHERE MABAN = @id";

            using (var conn = DBConnect.GetConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", maBan);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool doiTrangThaiBanSauKhiXoaHD(int maBan)
        {
            string sql = "UPDATE ban SET DANGSUDUNG = 1 WHERE MABAN = @id";

            using (var conn = DBConnect.GetConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", maBan);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool CapNhatHoaDon(hoaDonDTO hd)
        {
            string sql = @"
        UPDATE hoadon SET 
            MaKhachHang = @maKH, 
            MaNhanVien = @maNV, 
            MaTT = @maTT,
            TongTien = @tongTien
        WHERE MaHoaDon = @maHD";

            using (var conn = DBConnect.GetConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@maHD", hd.MaHD);
                cmd.Parameters.AddWithValue("@maKH", hd.MaKhachHang);
                cmd.Parameters.AddWithValue("@maNV", hd.MaNhanVien);
                cmd.Parameters.AddWithValue("@maTT", hd.MaTT);
                cmd.Parameters.AddWithValue("@tongTien", hd.TongTien);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}