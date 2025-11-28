using DAO;
using DTO;
using DAO.CONFIG;
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

        public bool XoaPhieuPN(int mapn)
        {
            return pnDAO.DeletePN(mapn);
        }
        public int ThemPhieuNhap(phieuNhapDTO header, List<ctPhieuNhapDTO> details)
        {
            if (header.MaNCC <= 0)
            {
                throw new Exception("Vui lòng chọn Nhà cung cấp.");
            }
            if (details == null || details.Count == 0)
            {
                throw new Exception("Danh sách hàng nhập đang trống.");
            }
            return pnDAO.ThemPhieuNhap(header, details);
        }
        public bool ThemChiTietVaoPhieuCu(int mapn, List<ctPhieuNhapDTO> details)
        {
            if (mapn <= 0) throw new Exception("Mã phiếu nhập không hợp lệ.");
            if (details == null || details.Count == 0) throw new Exception("Chưa chọn nguyên liệu để thêm.");

            return pnDAO.ThemChiTietVaoPhieuCu(mapn, details);
        }
    }  
}