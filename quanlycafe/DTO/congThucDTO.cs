using System;

namespace quanlycafe.DTO
{
    public class congThucDTO
    {
        public int MaSanPham { get; set; }
        public int MaNguyenLieu { get; set; }
        public float SoLuongCoSo { get; set; }
        public int TrangThai { get; set; } = 1;
        public string TenNguyenLieu { get; set; }
        public string TenSanPham { get; set; }
        public congThucDTO() { }

        public congThucDTO(int maSP, int maNL, float soLuong, int trangThai = 1)
        {
            MaSanPham = maSP;
            MaNguyenLieu = maNL;
            SoLuongCoSo = soLuong;
            TrangThai = trangThai;
        }
    }
}
