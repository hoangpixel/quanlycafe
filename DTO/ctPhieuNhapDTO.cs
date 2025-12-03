namespace DTO
{
    public class ctPhieuNhapDTO
    {
        // Lưu ý: Không còn MaCTPN nữa
        public int MaPN { get; set; }
        public int MaNguyenLieu { get; set; }
        public int MaDonVi { get; set; }
        public decimal HeSo { get; set; }         // Để tính toán quy đổi
        public decimal SoLuong { get; set; }      // Số lượng nhập vào (ví dụ: 2 thùng)
        public decimal SoLuongCoSo { get; set; }  // Số lượng quy đổi (ví dụ: 48 lon)
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }
}