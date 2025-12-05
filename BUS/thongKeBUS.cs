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

        // Add this method to the thongKeBUS class to fix CS1061
        public List<thongKeDTO> GetChiTieuTheoKhoang_GroupThang(DateTime tuNgay, DateTime denNgay)
        {
            // Implement logic similar to GetDoanhThuTheoKhoang_GroupThang, but for Chi Tieu
            // This is a stub implementation. Replace with actual data retrieval logic.
            List<thongKeDTO> result = new List<thongKeDTO>();
            DateTime current = new DateTime(tuNgay.Year, tuNgay.Month, 1);
            while (current <= denNgay)
            {
                // Example: Add dummy data for each month in the range
                result.Add(new thongKeDTO
                {
                    Nhan = current.ToString("MM/yyyy"),
                    GiaTri = 0 // Replace with actual value
                });
                current = current.AddMonths(1);
            }
            return result;
        }
    }
}