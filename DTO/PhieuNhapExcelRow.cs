using System;

namespace DTO
{
    // Class này dùng để hứng dữ liệu phẳng từ Excel
    public class PhieuNhapExcelRow
    {
        // --- Phần Header (Thông tin phiếu) ---
        public int MaPN_Excel { get; set; } // Mã tạm trên Excel để gom nhóm
        public int MaNCC { get; set; }
        public int MaNV { get; set; }
        public DateTime ThoiGian { get; set; }

        // --- Phần Detail (Chi tiết hàng hóa) ---
        public int MaNguyenLieu { get; set; }
        public string TenDonVi { get; set; } // Người dùng nhập chữ: "Thùng", "Cái"...
        public decimal SoLuong { get; set; } // Số lượng nhập vào
        public decimal DonGia { get; set; }
    }
}