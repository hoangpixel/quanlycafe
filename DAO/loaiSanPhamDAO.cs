using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using MySql.Data.MySqlClient;
using DAO.CONFIG;
using System.ComponentModel;

namespace DAO
{
    public class loaiSanPhamDAO
    {
        public BindingList<loaiDTO> docDanhSachLoai()
        {
            BindingList<loaiDTO> ds = new BindingList<loaiDTO>();
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
                        TenLoai = reader.GetString("TENLOAI"),
                        MaNhom = reader.GetInt32("MANHOM")
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
                string qry = "INSERT INTO loai (TENLOAI,MANHOM) VALUES (@TenLoai, @MaNhom); SELECT LAST_INSERT_ID();";

                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@TenLoai", ct.TenLoai);
                cmd.Parameters.AddWithValue("@MaNhom", ct.MaNhom);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
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
                string qry = @"UPDATE loai SET TENLOAI = @TenLoai, MANHOM = @MaNhom WHERE MALOAI = @MaLoai";
                MySqlConnection conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@TenLoai", ct.TenLoai);
                cmd.Parameters.AddWithValue("@MaNhom", ct.MaNhom);
                cmd.Parameters.AddWithValue("@Maloai", ct.MaLoai);

                int row = cmd.ExecuteNonQuery();
                if(row > 0)
                {
                    return true;
                }
                return false;
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
