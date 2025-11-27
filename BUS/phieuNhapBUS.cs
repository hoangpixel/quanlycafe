using DAO;
using DTO;
using DAO.CONFIG;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace BUS
{
    public class phieuNhapBUS
    {
        private phieuNhapDAO pnDAO = new phieuNhapDAO();
        private ctPhieuNhapDAO ctDAO = new ctPhieuNhapDAO();


        public bool KiemTraTonTai(int mapn)
        {
            return pnDAO.KiemTraTonTai(mapn);
        }

        public List<ctPhieuNhapDTO> LayChiTiet(int mapn)
        {
            return ctDAO.LayDanhSachChiTietTheoMaPN(mapn);
        }

        public bool CapNhatThongTinPhieu(int maPN, int maNCC, int MANHANVIEN)
        {
            return pnDAO.CapNhatThongTin(maPN, maNCC, MANHANVIEN);
        }

        public BindingList<phieuNhapDTO> LayDanhSach()
        {
            return pnDAO.LayDanhSach();
        }

        public bool XoaPhieu(int mapn)
        {
            return pnDAO.Delete(mapn);
        }


        public int ThemPhieuNhap(phieuNhapDTO header, List<ctPhieuNhapDTO> details)
        {
            if (details == null || details.Count == 0) return 0;

            MySqlConnection conn = DBConnect.GetConnection();
            MySqlTransaction tran = null;
            try
            {
                tran = conn.BeginTransaction();

                decimal tongTien = details.Sum(ct => ct.SoLuong * ct.DonGia);


                int ketQua = pnDAO.Insert(header, conn, tran);

                if (ketQua == 0) throw new Exception("Lỗi insert header.");


                foreach (var ct in details)
                {
                    ct.MaPN = ketQua;

                    bool ok = ctDAO.Insert(ct, conn, tran);
                    if (!ok) throw new Exception("Lỗi insert chi tiết.");

                    var cmdUpd = new MySqlCommand("UPDATE nguyenlieu SET TONKHO = TONKHO + @soluong WHERE MANGUYENLIEU = @mangl", conn, tran);
                    cmdUpd.Parameters.AddWithValue("@soluong", ct.SoLuongCoSo);
                    cmdUpd.Parameters.AddWithValue("@mangl", ct.MaNguyenLieu);
                    cmdUpd.ExecuteNonQuery();
                }


                if (!pnDAO.UpdateTongTien(ketQua, tongTien, conn, tran))
                    throw new Exception("Lỗi update tổng tiền.");

                tran.Commit();

                return ketQua;
            }
            catch (Exception ex)
            {
                try { tran?.Rollback(); } catch { }
                throw new Exception(ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }

        public bool XoaPhieuPN(int mapn)
        {
            return pnDAO.DeletePN(mapn);
        }
    

public bool ThemChiTietVaoPhieuCu(int maPN, List<ctPhieuNhapDTO> details)
        {
            MySqlConnection conn = DBConnect.GetConnection();
            MySqlTransaction tran = null;
            try
            {
                tran = conn.BeginTransaction();

              
                foreach (var ct in details)
                {
                    ct.MaPN = maPN; 
                    bool ok = ctDAO.Insert(ct, conn, tran);
                    if (!ok) throw new Exception("Lỗi thêm chi tiết: " + ct.TenNguyenLieu);

                    var cmdUpd = new MySqlCommand("UPDATE nguyenlieu SET TONKHO = TONKHO + @soluong WHERE MANGUYENLIEU = @mangl", conn, tran);
                    cmdUpd.Parameters.AddWithValue("@soluong", ct.SoLuongCoSo);
                    cmdUpd.Parameters.AddWithValue("@mangl", ct.MaNguyenLieu);
                    cmdUpd.ExecuteNonQuery();
                }

                
                string sqlSum = "SELECT SUM(THANHTIEN) FROM ctphieunhap WHERE MAPN = @mapn";
                var cmdSum = new MySqlCommand(sqlSum, conn, tran);
                cmdSum.Parameters.AddWithValue("@mapn", maPN);
                object result = cmdSum.ExecuteScalar();
                decimal tongTienMoi = (result != DBNull.Value) ? Convert.ToDecimal(result) : 0;

               
                if (!pnDAO.UpdateTongTien(maPN, tongTienMoi, conn, tran))
                    throw new Exception("Lỗi cập nhật tổng tiền phiếu.");

                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                try { tran?.Rollback(); } catch { }
                throw new Exception(ex.Message);
            }
            finally
            {
                DBConnect.CloseConnection(conn);
            }
        }
    } }