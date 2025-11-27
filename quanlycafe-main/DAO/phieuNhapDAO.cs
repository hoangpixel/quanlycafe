

using DAO.CONFIG;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;


namespace DAO
{
    public class phieuNhapDAO
    {

        public BindingList<phieuNhapDTO> LayDanhSach()
        {
            BindingList<phieuNhapDTO> ds = new BindingList<phieuNhapDTO>();

           
            string sql = @"SELECT pn.MAPN, pn.MANCC, ncc.TENNCC, 
                          pn.MANHANVIEN, nv.HOTEN, 
                          pn.THOIGIAN, pn.TONGTIEN, pn.TRANGTHAI 
                   FROM phieunhap pn
                   JOIN nhacungcap ncc ON pn.MANCC = ncc.MANCC
                   JOIN nhanvien nv ON pn.MANHANVIEN = nv.MANHANVIEN
                   ORDER BY pn.THOIGIAN DESC";

            MySqlConnection conn = null;
            try
            {
                conn = DBConnect.GetConnection();
                var cmd = new MySqlCommand(sql, conn);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var p = new phieuNhapDTO
                    {
                        MaPN = rdr.GetInt32("MAPN"),
                        MaNCC = rdr.GetInt32("MANCC"),
                        MANHANVIEN = rdr.GetInt32("MANHANVIEN"), 
                        ThoiGian = rdr.GetDateTime("THOIGIAN"),
                        TrangThai = rdr.GetInt32("TRANGTHAI"),

                        TongTien = rdr.GetDecimal("TONGTIEN"),
                        TenNCC = rdr.GetString("TENNCC"),
                        TenNV = rdr.GetString("HOTEN")
                    };
                    ds.Add(p);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi LayDanhSach: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return ds;
        }


        public int Insert(phieuNhapDTO pn, MySqlConnection conn, MySqlTransaction tran)
        {

            string sql = @"INSERT INTO phieunhap (MANCC, MANHANVIEN, THOIGIAN, TONGTIEN, TRANGTHAI) 
                   VALUES (@mancc, @manv, @thoigian, 0, @trangthai); 
                   SELECT LAST_INSERT_ID();";

            using (var cmd = new MySqlCommand(sql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@mancc", pn.MaNCC);
                cmd.Parameters.AddWithValue("@manv", pn.MANHANVIEN); 
                cmd.Parameters.AddWithValue("@thoigian", pn.ThoiGian);
                cmd.Parameters.AddWithValue("@trangthai", pn.TrangThai);
                object res = cmd.ExecuteScalar();
                return Convert.ToInt32(res);
            }
        
        }

        public bool KiemTraTonTai(int maPN)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string sql = "SELECT COUNT(*) FROM phieunhap WHERE MAPN = @mapn";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mapn", maPN);

                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi KiemTraTonTai: " + ex.Message);
                throw ex;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }
     

        public bool Delete(int mapn)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            MySqlTransaction tran = null;
            try
            {
                tran = conn.BeginTransaction();

           
                var cmd1 = new MySqlCommand("DELETE FROM ctphieunhap WHERE MAPN = @mapn", conn, tran);
                cmd1.Parameters.AddWithValue("@mapn", mapn);
                cmd1.ExecuteNonQuery();

               
                var cmd2 = new MySqlCommand("DELETE FROM phieunhap WHERE MAPN = @mapn", conn, tran);
                cmd2.Parameters.AddWithValue("@mapn", mapn);
                cmd2.ExecuteNonQuery();

                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                try { tran?.Rollback(); } catch { }
                Console.WriteLine("phieuNhapDAO.Delete: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool CapNhatThongTin(int maPN, int maNCC, int MANHANVIEN)
        {
            // SỬA LẠI TÊN CỘT CHO ĐÚNG VỚI CSDL: MANHANVIEN
            string sql = "UPDATE phieunhap SET MANCC = @mancc, MANHANVIEN = @manv WHERE MAPN = @mapn";
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mancc", maNCC);
                cmd.Parameters.AddWithValue("@manv", MANHANVIEN);
                cmd.Parameters.AddWithValue("@mapn", maPN);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi cập nhật PN: " + ex.Message);
                return false;
            }
            finally { DBConnect.CloseConnection(conn); }
        }
        public bool DeletePN(int mapn)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            MySqlTransaction tran = null;
            try
            {
                tran = conn.BeginTransaction();

                
                var cmd1 = new MySqlCommand("DELETE FROM ctphieunhap WHERE MAPN = @mapn", conn, tran);
                cmd1.Parameters.AddWithValue("@mapn", mapn);
                cmd1.ExecuteNonQuery();
                var cmd2 = new MySqlCommand("DELETE FROM phieunhap WHERE MAPN = @mapn", conn, tran);
                cmd2.Parameters.AddWithValue("@mapn", mapn);
                int rows = cmd2.ExecuteNonQuery();

                tran.Commit();
                return rows > 0;
            }
            catch (Exception ex)
            {
                try { tran?.Rollback(); } catch { }
                Console.WriteLine("Lỗi xóa phiếu: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }
     
        public bool CapNhatTongTienDonGian(int maPN, decimal tongTien)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string sql = "UPDATE phieunhap SET TONGTIEN = @tt WHERE MAPN = @mapn";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tt", tongTien);
                cmd.Parameters.AddWithValue("@mapn", maPN);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }
        

        public bool UpdateTongTien(int maPN, decimal tongTien, MySqlConnection conn, MySqlTransaction tran)
        {
            // Cập nhật cả TONGTIEN và THOIGIAN (NOW)
            string sql = "UPDATE phieunhap SET TONGTIEN = @tongTien, THOIGIAN = NOW() WHERE MAPN = @maPN";
            using (var cmd = new MySqlCommand(sql, conn, tran))
            {
                cmd.Parameters.AddWithValue("@tongTien", tongTien);
                cmd.Parameters.AddWithValue("@maPN", maPN);
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}