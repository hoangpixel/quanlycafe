using DAO.CONFIG;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;

namespace DAO
{
    public class taikhoanDAO
    {
        public BindingList<taikhoanDTO> LayDanhSach()
        {
            BindingList<taikhoanDTO> list = new BindingList<taikhoanDTO>();
            MySqlConnection conn = DBConnect.GetConnection();

            try
            {
                string query = @"SELECT * FROM taikhoan WHERE TRANGTHAI = 1";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    taikhoanDTO tk = new taikhoanDTO();
                    tk.MATAIKHOAN = Convert.ToInt32(reader["MATAIKHOAN"]);
                    tk.MANHANVIEN = Convert.ToInt32(reader["MANHANVIEN"]);
                    tk.TENDANGNHAP = reader["TENDANGNHAP"].ToString();
                    tk.MATKHAU = reader["MATKHAU"].ToString();
                    tk.TRANGTHAI = Convert.ToInt32(reader["TRANGTHAI"]);
                    tk.NGAYTAO = Convert.ToDateTime(reader["NGAYTAO"]);
                    tk.MAVAITRO = Convert.ToInt32(reader["MAVAITRO"]);

                    list.Add(tk);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi lấy danh sách tài khoản: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return list;
        }

        public int layMa()
        {
            MySqlConnection conn = DBConnect.GetConnection();
            int maTK = 0;
            try
            {
                string qry = "SELECT IFNULL(MAX(MATAIKHOAN), 0) FROM taikhoan";
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                maTK = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy mã maTK : " + ex);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return maTK + 1;
        }
        public taikhoanDTO DangNhap(string tenDangNhap, string matKhau)
        {
            taikhoanDTO tk = null;
            MySqlConnection conn = DBConnect.GetConnection();

            try
            {
                string query = "SELECT * FROM TAIKHOAN WHERE TENDANGNHAP = @User AND MATKHAU = @Pass AND TRANGTHAI = 1";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@User", tenDangNhap);
                cmd.Parameters.AddWithValue("@Pass", matKhau);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    tk = new taikhoanDTO();
                    tk.MATAIKHOAN = Convert.ToInt32(reader["MATAIKHOAN"]);
                    tk.MANHANVIEN = Convert.ToInt32(reader["MANHANVIEN"]);
                    tk.TENDANGNHAP = reader["TENDANGNHAP"].ToString();
                    tk.MATKHAU = reader["MATKHAU"].ToString();
                    tk.TRANGTHAI = Convert.ToInt32(reader["TRANGTHAI"]);
                    tk.MAVAITRO = Convert.ToInt32(reader["MAVAITRO"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi đăng nhập: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return tk;
        }

        public bool Them(taikhoanDTO tk)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                // 1. Thêm cột MATAIKHOAN vào danh sách cột
                string query = @"INSERT INTO TAIKHOAN (MANHANVIEN, TENDANGNHAP, MATKHAU, MAVAITRO) 
                        VALUES (@MaNV, @TenDN, @MatKhau, @MaVaiTro)";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@MaNV", tk.MANHANVIEN);
                cmd.Parameters.AddWithValue("@TenDN", tk.TENDANGNHAP);
                cmd.Parameters.AddWithValue("@MatKhau", tk.MATKHAU);
                cmd.Parameters.AddWithValue("@MaVaiTro", tk.MAVAITRO);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                // Ném lỗi ra để Form bắt được và hiện MessageBox
                throw new Exception("Lỗi SQL: " + ex.Message);
            }
            finally { DBConnect.CloseConnection(conn); }
        }

        public bool Sua(taikhoanDTO tk)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = @"UPDATE TAIKHOAN 
                                SET MANHANVIEN = @MaNV, TENDANGNHAP = @TenDN, MATKHAU = @MatKhau, 
                                    TRANGTHAI = @TrangThai, MAVAITRO = @MaVaiTro 
                                WHERE MATAIKHOAN = @MaTK";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaTK", tk.MATAIKHOAN);
                cmd.Parameters.AddWithValue("@MaNV", tk.MANHANVIEN);
                cmd.Parameters.AddWithValue("@TenDN", tk.TENDANGNHAP);
                cmd.Parameters.AddWithValue("@MatKhau", tk.MATKHAU);
                cmd.Parameters.AddWithValue("@TrangThai", tk.TRANGTHAI);
                cmd.Parameters.AddWithValue("@MaVaiTro", tk.MAVAITRO);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }

        public bool Xoa(int maTK)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                string query = "DELETE FROM taikhoan WHERE MATAIKHOAN = @MaTK";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaTK", maTK);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
            finally { DBConnect.CloseConnection(conn); }
        }
    }
}