using System;

namespace DTO
{
    public class phieuNhapDTO
    {
        public int MaPN { get; set; }
        public int MaNCC { get; set; }
        public int MaNhanVien { get; set; }
        public DateTime ThoiGian { get; set; }
        public decimal TongTien { get; set; }
        public int TrangThai { get; set; } // 0: Chưa xử lý, 1: Đã nhập kho
        public int TrangThaiXoa { get; set; } // 1: Chưa xóa
    }
}