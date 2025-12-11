using DAO.CONFIG;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DAO
{
    public class banDAO
    {
        public BindingList<banDTO> LayDanhSachBan()
        {
            BindingList<banDTO> ds = new BindingList<banDTO>();
            string qry = "SELECT MABAN, TENBAN, DANGSUDUNG, MAKHUVUC, TRANGTHAIXOA FROM ban";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    banDTO ban = new banDTO();
                    ban.MaBan = reader.GetInt32("MABAN");
                    ban.TenBan = reader.GetString("TENBAN");
                    ban.DangSuDung = reader.GetByte("DANGSUDUNG");
                    ban.MaKhuVuc = reader.GetInt32("MAKHUVUC");
                    ban.TrangThaiXoa = reader.GetByte("TRANGTHAIXOA");
                    ds.Add(ban);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách bàn: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }

        public BindingList<banDTO> LayDanhSachTheoKhuVuc(int maKhuVuc)
        {
            BindingList<banDTO> ds = new BindingList<banDTO>();
            string qry = "SELECT MABAN, TENBAN, DANGSUDUNG, MAKHUVUC, TRANGTHAIXOA FROM ban WHERE MAKHUVUC = @maKhuVuc AND TRANGTHAIXOA = 1";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maKhuVuc", maKhuVuc);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    banDTO ban = new banDTO();
                    ban.MaBan = reader.GetInt32("MABAN");
                    ban.TenBan = reader.GetString("TENBAN");
                    ban.DangSuDung = reader.GetByte("DANGSUDUNG");
                    ban.MaKhuVuc = reader.GetInt32("MAKHUVUC");
                    ban.TrangThaiXoa = reader.GetByte("TRANGTHAIXOA");

                    ds.Add(ban);
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Lỗi đọc danh sách bàn theo khu vực {maKhuVuc}: {ex.Message}");
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }
        public bool DoiTrangThai(int maBan)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = $"UPDATE ban SET DANGSUDUNG = 0 WHERE MABAN = {maBan}";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi xóa sản phẩm: " + e.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public int LayMa()
        {
            MySqlConnection conn = DBConnect.GetConnection();
            int maBan = 0;
            try
            {
                string qry = "SELECT IFNULL(MAX(MABAN), 0) FROM ban";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                maBan = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy mã bàn: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return maBan + 1;
        }

        // 5. Thêm bàn mới
        public bool ThemBan(banDTO ban)
        {
            // Thêm bàn mới: Mặc định DANGSUDUNG = 1 (Trống/Sẵn sàng), TRANGTHAIXOA = 1 (Chưa xóa)
            string qry = "INSERT INTO ban (TENBAN, MAKHUVUC, DANGSUDUNG, TRANGTHAIXOA) VALUES (@tenBan, @maKhuVuc, 1, 1)";
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tenBan", ban.TenBan);
                cmd.Parameters.AddWithValue("@maKhuVuc", ban.MaKhuVuc);

                int rs = cmd.ExecuteNonQuery();
                return rs > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi thêm bàn: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        // 6. Sửa thông tin bàn
        public bool SuaBan(banDTO ban)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = @"UPDATE ban SET TENBAN = @tenBan, MAKHUVUC = @maKhuVuc WHERE MABAN = @maBan";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maBan", ban.MaBan);
                cmd.Parameters.AddWithValue("@tenBan", ban.TenBan);
                cmd.Parameters.AddWithValue("@maKhuVuc", ban.MaKhuVuc);

                int rs = cmd.ExecuteNonQuery();
                return rs > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi sửa bàn: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        // 7. Xóa bàn (Xóa mềm - Chuyển TRANGTHAIXOA về 0)
        public bool XoaBan(int maBan)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = @"UPDATE ban SET TRANGTHAIXOA = 0 WHERE MABAN = @maBan";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maBan", maBan);

                int rs = cmd.ExecuteNonQuery();
                return rs > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi xóa bàn: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }
        }
}
