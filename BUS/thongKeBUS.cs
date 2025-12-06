using DAO;
using DTO;
using System;
using System.Collections.Generic;

namespace BUS
{
    public class thongKeBUS
    {
        // ===================== DOANH THU =====================

        // GUI gọi cái này khi chọn: Theo ngày, Theo quý
        public List<thongKeDTO> GetDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            return thongKeDAO.Instance.GetDoanhThuTheoNgay(tuNgay, denNgay);
        }

        // GUI gọi cái này khi chọn: Theo năm
        public List<thongKeDTO> GetDoanhThuTheoThang(int nam)
        {
            return thongKeDAO.Instance.GetDoanhThuTheoThang(nam);
        }

        // GUI gọi cái này khi chọn: Theo tháng (Khoảng tháng)
        public List<thongKeDTO> GetDoanhThuTheoKhoang_GroupThang(DateTime tuNgay, DateTime denNgay)
        {
            return thongKeDAO.Instance.GetDoanhThuTheoKhoang_GroupThang(tuNgay, denNgay);
        }

        // ===================== CHI TIÊU =====================

        // GUI gọi cái này khi chọn: Theo ngày, Theo quý
        public List<thongKeDTO> GetChiTieuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            return thongKeDAO.Instance.GetChiTieu(tuNgay, denNgay);
        }

        // GUI gọi cái này khi chọn: Theo năm
        public List<thongKeDTO> GetChiTieuTheoThang(int nam)
        {
            return thongKeDAO.Instance.GetChiTieuTheoThang(nam);
        }

        // GUI gọi cái này khi chọn: Theo tháng (Khoảng tháng)
        public List<thongKeDTO> GetChiTieuTheoKhoang_GroupThang(DateTime tuNgay, DateTime denNgay)
        {
            return thongKeDAO.Instance.GetChiTieuTheoKhoang_GroupThang(tuNgay, denNgay);
        }

        // ===================== LƯƠNG =====================
        public decimal GetTongLuong()
        {
            return thongKeDAO.Instance.GetTongLuong();
        }

        // Thêm vào class thongKeBUS

        // Lấy danh sách chi tiết Hóa Đơn cho DataGridView
        public List<hoaDonDTO> GetListHoaDon(DateTime tuNgay, DateTime denNgay)
        {
            return thongKeDAO.Instance.GetDanhSachHoaDon(tuNgay, denNgay);
        }

        // Lấy danh sách chi tiết Phiếu Nhập cho DataGridView
        public List<phieuNhapDTO> GetListPhieuNhap(DateTime tuNgay, DateTime denNgay)
        {
            return thongKeDAO.Instance.GetDanhSachPhieuNhap(tuNgay, denNgay);
        }

        // Thêm vào thongKeBUS.cs
        public List<topSanPhamDTO> GetTopSanPham(DateTime tuNgay, DateTime denNgay)
        {
            return thongKeDAO.Instance.GetTopSanPhamBanChay(tuNgay, denNgay);
        }
    }
}