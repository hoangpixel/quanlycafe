using DAO;
using DTO;
using System;
using System.Collections.Generic;

namespace BUS
{
    public class thongKeBUS
    {
        // =================================================================
        // 1. XỬ LÝ DOANH THU
        // =================================================================

        // Dùng cho xem: Tùy chỉnh / Theo Tháng / Theo Quý
        public List<thongKeDTO> GetDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            return thongKeDAO.Instance.GetDoanhThuTheoNgay(tuNgay, denNgay);
        }

        // Dùng cho xem: Theo Năm (Hiện 12 cột tháng)
        public List<thongKeDTO> GetDoanhThuTheoThang(int nam)
        {
            return thongKeDAO.Instance.GetDoanhThuTheoThang(nam);
        }

        // =================================================================
        // 2. XỬ LÝ CHI TIÊU
        // =================================================================

        // Dùng cho xem: Tùy chỉnh / Theo Tháng / Theo Quý
        public List<thongKeDTO> GetChiTieuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            return thongKeDAO.Instance.GetChiTieu(tuNgay, denNgay);
        }

        // Dùng cho xem: Theo Năm (Hiện 12 cột tháng)
        public List<thongKeDTO> GetChiTieuTheoThang(int nam)
        {
            return thongKeDAO.Instance.GetChiTieuTheoThang(nam);
        }

        // =================================================================
        // 3. XỬ LÝ LƯƠNG
        // =================================================================
        public decimal GetTongLuong()
        {
            return thongKeDAO.Instance.GetTongLuong();
        }

        // Trong class thongKeBUS
        public List<thongKeDTO> GetDoanhThuTheoKhoang_GroupThang(DateTime tuNgay, DateTime denNgay)
        {
            return thongKeDAO.Instance.GetDoanhThuTheoKhoang_GroupThang(tuNgay, denNgay);
        }
    }
}