using System;

namespace DTO
{
    public class phieuHuyDTO
    {
        public int MaPhieuHuy { get; set; }
        public int MaNhanVien { get; set; }
        public int MaHoaDon { get; set; }
        public int MaNguyenLieu { get; set; }
        public decimal SoLuong { get; set; }
        public string LyDo { get; set; }
        public DateTime NgayTao { get; set; }
        public int MaDonVi { get; set; }
    }
}