using DAO.CONFIG;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class banDAO
    {
        public BindingList<banDTO> LayDanhSachBan()
        {
            BindingList<banDTO> ds = new BindingList<banDTO>();
            string qry = "SELECT MABAN, TENBAN, DANGSUDUNG, MAKHUVUC, TRANGTHAIXOA, MADONHIENTAI FROM ban";
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

                    if (!reader.IsDBNull(reader.GetOrdinal("MADONHIENTAI")))
                    {
                        ban.MaDonHienTai = reader.GetInt32("MADONHIENTAI");
                    }
                    else
                    {
                        ban.MaDonHienTai = null;
                    }

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
            string qry = "SELECT MABAN, TENBAN, DANGSUDUNG, MAKHUVUC, TRANGTHAIXOA, MADONHIENTAI FROM ban WHERE MAKHUVUC = @maKhuVuc AND TRANGTHAIXOA = 1";
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

                    if (!reader.IsDBNull(reader.GetOrdinal("MADONHIENTAI")))
                    {
                        ban.MaDonHienTai = reader.GetInt32("MADONHIENTAI");
                    }
                    else
                    {
                        ban.MaDonHienTai = null;
                    }

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
        public bool DoiTrangThai(int maBan,int maHD)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string qry = $"UPDATE ban SET DANGSUDUNG = 0, MADONHIENTAI = {maHD} WHERE MABAN = {maBan}";
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
    }
}
