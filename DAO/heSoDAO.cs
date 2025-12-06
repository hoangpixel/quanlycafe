using DAO.CONFIG;
using DTO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DAO
{
    public class heSoDAO
    {
        public BindingList<heSoDTO> DocDanhSachHeSo()
        {
            BindingList<heSoDTO> ds = new BindingList<heSoDTO>();
            string qry = "SELECT * FROM hesodonvi";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    heSoDTO hs = new heSoDTO
                    {
                        MaNguyenLieu = reader.GetInt32("MANGUYENLIEU"),
                        MaDonVi = reader.GetInt32("MADONVI"),
                        HeSo = reader.IsDBNull(reader.GetOrdinal("HESO"))
                            ? 0
                            : reader.GetDecimal("HESO")
                    };
                    ds.Add(hs);
                }

                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách hệ số: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }

        public bool Them(heSoDTO hs)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = @"INSERT INTO hesodonvi (MANGUYENLIEU, MADONVI, HESO)
                               VALUES (@maNL, @maDV, @heSo);";

                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maNL", hs.MaNguyenLieu);
                cmd.Parameters.AddWithValue("@maDV", hs.MaDonVi);
                cmd.Parameters.AddWithValue("@heSo", hs.HeSo);

                int rows = cmd.ExecuteNonQuery();
                if(rows > 0)
                {
                    string updateTT = @"UPDATE nguyenlieu SET TRANGTHAIDV = 1 WHERE MANGUYENLIEU = @maNL";
                    MySqlCommand updatecmd = new MySqlCommand(updateTT, conn);
                    updatecmd.Parameters.AddWithValue("maNL", hs.MaNguyenLieu);
                    updatecmd.ExecuteNonQuery();
                }
                return rows > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi thêm hệ số: " + e.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }



        public bool CapNhat(heSoDTO hs)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = @"UPDATE hesodonvi 
                               SET HESO = @heSo 
                               WHERE MANGUYENLIEU = @maNL AND MADONVI = @maDV;";

                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maNL", hs.MaNguyenLieu);
                cmd.Parameters.AddWithValue("@maDV", hs.MaDonVi);
                cmd.Parameters.AddWithValue("@heSo", hs.HeSo);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi cập nhật hệ số: " + e.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool Xoa(int maNguyenLieu, int maDonVi)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = @"DELETE FROM hesodonvi 
                               WHERE MANGUYENLIEU = @maNL AND MADONVI = @maDV;";

                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maNL", maNguyenLieu);
                cmd.Parameters.AddWithValue("@maDV", maDonVi);

                int rows = cmd.ExecuteNonQuery();
                if(rows > 0)
                {
                    string demNL = @"SELECT COUNT(*) FROM hesodonvi WHERE MANGUYENLIEU = @maNL";
                    MySqlCommand cmdDem = new MySqlCommand(demNL, conn);
                    cmdDem.Parameters.AddWithValue("@maNL", maNguyenLieu);
                    int d = Convert.ToInt32(cmdDem.ExecuteScalar());
                    if(d == 0)
                    {
                        string qryUpdate = @"UPDATE nguyenlieu SET TRANGTHAIDV = 0 WHERE MANGUYENLIEU = @maNL";
                        MySqlCommand cmdUpdate = new MySqlCommand(qryUpdate, conn);
                        cmdUpdate.Parameters.AddWithValue("@maNL", maNguyenLieu);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }
                return rows > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi xóa hệ số: " + e.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public List<heSoDTO> layDanhSachHeSoTheoNL(int maNL)
        {
            List<heSoDTO> dskq = new List<heSoDTO>();
            string qry = @"SELECT * FROM hesodonvi WHERE MANGUYENLIEU = @manl";
            MySqlConnection conn = null;
            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@manl", maNL);
                MySqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    heSoDTO ct = new heSoDTO();
                    ct.MaNguyenLieu = reader.GetInt32("MANGUYENLIEU");
                    ct.MaDonVi = reader.GetInt32("MADONVI");
                    ct.HeSo = reader.IsDBNull(reader.GetOrdinal("HESO"))
                            ? 0
                            : reader.GetDecimal("HESO");
                    dskq.Add(ct);
                }
                reader.Close();
                cmd.Dispose();
            }catch(MySqlException e)
            {
                Console.WriteLine("Lỗi đọc danh sách hệ số theo nguyên liệu: " + e.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return dskq;
        }

        public decimal LayHeSo(int maNL, int maDV)
        {
            decimal heso = 0; 
            string qry = "SELECT HESO FROM hesodonvi WHERE MANGUYENLIEU = @maNL AND MADONVI = @maDV";

            using (MySqlConnection conn = DBConnect.GetConnection())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(qry, conn);
                    cmd.Parameters.AddWithValue("@maNL", maNL);
                    cmd.Parameters.AddWithValue("@maDV", maDV);

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        heso = Convert.ToDecimal(result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi lấy hệ số: " + ex.Message);
                }
            }
            return heso;
        }
    }
}
