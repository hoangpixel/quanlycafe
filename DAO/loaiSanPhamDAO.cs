using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using MySql.Data.MySqlClient;
using DAO.CONFIG;

namespace DAO
{
    public class loaiSanPhamDAO
    {
        public List<loaiDTO> docDanhSachLoai()
        {
            List<loaiDTO> ds = new List<loaiDTO>();
            string qry = "SELECT * FROM loai WHERE TRANGTHAI = 1";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    loaiDTO l = new loaiDTO
                    {
                        MaLoai = reader.GetInt32("MALOAI"),
                        TenLoai = reader.GetString("TENLOAI")
                    };

                    ds.Add(l);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách loại sản phẩm: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }


        public bool Them(loaiDTO ct)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = "INSERT INTO loai (TENLOAI) VALUES (@TenLoai); SELECT LAST_INSERT_ID();";

                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@TenLoai", ct.TenLoai);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    ct.MaLoai = Convert.ToInt32(result);
                    Console.WriteLine($"Thêm loại sản phẩm thành công! Mã loại mới = {ct.MaLoai}");
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi thêm loại sản phẩm: " + e.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }


        public bool Sua(loaiDTO ct)
        {
            try
            {
                string qry = "UPDATE loai SET ";
                qry = $"UPDATE loai SET TENLOAI = '{ct.TenLoai}' WHERE MALOAI = {ct.MaLoai}";


                MySqlConnection conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Sửa loại sản phẩm thành công!");
                DBConnect.CloseConnection(conn);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi sửa loại sản phẩm: " + e.Message);
                return false;
            }
        }

        public bool Xoa(int maLoai)
        {
            try
            {
                string qry = $"UPDATE loai SET TRANGTHAI = 0 WHERE MALOAI = {maLoai}";

                MySqlConnection conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Xóa (ẩn) loại sản phẩm thành công!");
                DBConnect.CloseConnection(conn);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi xóa loại sản phẩm: " + e.Message);
                return false;
            }
        }
    }
}
