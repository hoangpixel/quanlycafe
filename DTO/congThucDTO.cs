using System;

namespace DTO
{
    public class congThucDTO
    {
        public int MaSanPham { get; set; }
        public int MaNguyenLieu { get; set; }
        public decimal SoLuongCoSo { get; set; }
        public int MaDonViCoSo { get; set; }
        public string TenDonViCoSo { get; set; }

        public int TrangThai { get; set; } = 1;
        public string TenNguyenLieu { get; set; }

        public congThucDTO() { }

        public override string ToString()
        {
            return $"{TenNguyenLieu} - {SoLuongCoSo} {TenDonViCoSo}";
        }
    }
}