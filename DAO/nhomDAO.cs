using DAO.CONFIG;
using DTO;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class nhomDAO
    {
        public List<nhomDTO> layDanhSachNhom()
        {
            List<nhomDTO> ds = new List<nhomDTO>();
            string qry = "SELECT * FROM nhom WHERE TRANGTHAI = 1";
            MySqlConnection conn = null;
            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader rs = cmd.ExecuteReader();
                while(rs.Read())
                {
                    nhomDTO ct = new nhomDTO();
                    ct.MaNhom = rs.GetInt32("MANHOM");
                    ct.TenNhom = rs.GetString("TENNHOM");
                    ct.TrangThai = rs.GetInt32("TRANGTHAI");
                    ds.Add(ct);
                }
                rs.Close();
                cmd.Dispose();
            }catch(MySqlException ex)
            {
                Console.WriteLine("Lỗi không lấy được danh sách nhóm : " + ex);
            }
            return ds;
        }

        public bool them(nhomDTO ct)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = @"INSERT INTO nhom (TENNHOM, TRANGTHAI) VALUES (@tenNhom,@trangThai)";
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tenNhom", ct.TenNhom);
                cmd.Parameters.AddWithValue("@trangThai", ct.TrangThai);

                object rs = cmd.ExecuteNonQuery();
                if(rs != null)
                {
                    return true;
                }
                return false;
            }catch(MySqlException ex)
            {
                Console.WriteLine("Lỗi không thêm được nhóm : " + ex);
                return false;
            }
            finally { DBConnect.CloseConnection(conn); }
        }

        public bool sua(nhomDTO ct)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = @"UPDATE nhom SET TENNHOM = @tenNhom WHERE MANHOM = @maNhom";
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@tenNhom", ct.TenNhom);
                cmd.Parameters.AddWithValue("@maNhom", ct.MaNhom);

                int row = cmd.ExecuteNonQuery();
                if(row > 0)
                {
                    return true;
                }
                return false;
            }catch(MySqlException ex)
            {
                Console.WriteLine("Lỗi không update đc nhóm : " + ex);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool xoa(int maNhom)
        {
            MySqlConnection conn = null;
            try
            {
                string qry = @"UPDATE nhom SET TRANGTHAI = 0 WHERE MANHOM = @maNhom";
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@maNhom", maNhom);

                int row = cmd.ExecuteNonQuery();
                if(row > 0)
                {
                    return true;
                }
                return false;
            }catch(MySqlException ex)
            {
                Console.WriteLine("Lỗi không xóa đc nhóm : " + ex);
                return false;
            }
            finally { DBConnect.CloseConnection(conn); }
        }
    }
}
