using MySql.Data.MySqlClient;
using DAO.CONFIG;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace DAO
{
    public class phanquyenDAO
    {
        public BindingList<phanquyenDTO> LayDanhSach()
        {
            BindingList<phanquyenDTO> ds = new BindingList<phanquyenDTO>();
            string qry = "SELECT * FROM phanquyen";
            MySqlConnection conn = null;

            try
            {
                conn = DBConnect.GetConnection();
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    phanquyenDTO sp = new phanquyenDTO
                    {
                        MaVaiTro = reader.GetInt32("MAVAITRO"),
                        MaQuyen = reader.GetInt32("MAQUYEN"),
                        CAN_READ = reader.GetInt32("CAN_READ"),
                        CAN_CREATE = reader.GetInt32("CAN_CREATE"),
                        CAN_UPDATE = reader.GetInt32("CAN_UPDATE"),
                        CAN_DELETE = reader.GetInt32("CAN_DELETE")
                    };
                    ds.Add(sp);
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi đọc danh sách sản phẩm: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }

            return ds;
        }
        public BindingList<phanquyenDTO> LayChiTietQuyenTheoVaiTro(int maVaiTro)
        {
            BindingList<phanquyenDTO> ds = new BindingList<phanquyenDTO>();

            string qry = @"
                SELECT
                    Q.MAQUYEN,
                    Q.TENQUYEN,
                    IFNULL(PQ.CAN_READ, 0) AS CAN_READ,
                    IFNULL(PQ.CAN_CREATE, 0) AS CAN_CREATE,
                    IFNULL(PQ.CAN_UPDATE, 0) AS CAN_UPDATE,
                    IFNULL(PQ.CAN_DELETE, 0) AS CAN_DELETE
                FROM
                    quyen Q
                LEFT JOIN
                    phanquyen PQ ON Q.MAQUYEN = PQ.MAQUYEN AND PQ.MAVAITRO = @mvt
                WHERE Q.TRANGTHAI = 1";

            MySqlConnection conn = DBConnect.GetConnection();

            try
            {
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@mvt", maVaiTro);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    phanquyenDTO pq = new phanquyenDTO();

                    pq.MaVaiTro = maVaiTro;
                    pq.MaQuyen = reader.GetInt32("MAQUYEN");

                    pq.TenQuyen = reader["TENQUYEN"] == DBNull.Value ? "" : reader.GetString("TENQUYEN");

                    pq.CAN_READ = Convert.ToInt32(reader["CAN_READ"]);
                    pq.CAN_CREATE = Convert.ToInt32(reader["CAN_CREATE"]);
                    pq.CAN_UPDATE = Convert.ToInt32(reader["CAN_UPDATE"]);
                    pq.CAN_DELETE = Convert.ToInt32(reader["CAN_DELETE"]);

                    ds.Add(pq);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi lấy chi tiết quyền theo vai trò: " + ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
            return ds;
        }

        public bool XoaToanBoQuyenCuaVaiTro(int maVaiTro)
        {
            string qry = "DELETE FROM phanquyen WHERE MAVAITRO = @mvt";
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@mvt", maVaiTro);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi xóa: " + ex.Message);
                return false;
            }
            finally { DBConnect.CloseConnection(conn); }
        }

        public bool ThemPhanQuyen(phanquyenDTO pq)
        {
            string qry = @"INSERT INTO phanquyen (MAVAITRO, MAQUYEN, CAN_READ, CAN_CREATE, CAN_UPDATE, CAN_DELETE) 
                           VALUES (@mvt, @mq, @cr, @cw, @cu, @cd)";
            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(qry, conn);
                cmd.Parameters.AddWithValue("@mvt", pq.MaVaiTro);
                cmd.Parameters.AddWithValue("@mq", pq.MaQuyen);
                cmd.Parameters.AddWithValue("@cr", pq.CAN_READ);
                cmd.Parameters.AddWithValue("@cw", pq.CAN_CREATE);
                cmd.Parameters.AddWithValue("@cu", pq.CAN_UPDATE);
                cmd.Parameters.AddWithValue("@cd", pq.CAN_DELETE);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi thêm: " + ex.Message);
                return false;
            }
            finally { DBConnect.CloseConnection(conn); }
        }

        // Hàm cập nhật quyền dựa trên khóa chính (Mã Vai Trò + Mã Quyền)
        public bool CapNhatQuyen(int maVaiTro, int maQuyen, int read, int create, int update, int delete)
        {
            // Update lại 4 quyền cơ bản
            string qry = @"UPDATE phanquyen 
                           SET CAN_READ = @cr, 
                               CAN_CREATE = @cc, 
                               CAN_UPDATE = @cu, 
                               CAN_DELETE = @cd
                           WHERE MAVAITRO = @mvt AND MAQUYEN = @mq";

            MySqlConnection conn = DBConnect.GetConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(qry, conn);

                // Gán tham số
                cmd.Parameters.AddWithValue("@mvt", maVaiTro);
                cmd.Parameters.AddWithValue("@mq", maQuyen);
                cmd.Parameters.AddWithValue("@cr", read);
                cmd.Parameters.AddWithValue("@cc", create);
                cmd.Parameters.AddWithValue("@cu", update);
                cmd.Parameters.AddWithValue("@cd", delete);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi cập nhật quyền: " + ex.Message);
                return false;
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }
    }
}