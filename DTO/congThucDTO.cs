using System;

namespace DTO
{
    public class congThucDTO
    {
        public int MaSanPham { get; set; }
        public int MaNguyenLieu { get; set; }
        public float SoLuongCoSo { get; set; }
        public int MaDonViCoSo { get; set; }
        public string TenDonViCoSo { get; set; }

        public int TrangThai { get; set; } = 1;
        public string TenNguyenLieu { get; set; }
        public string TenSanPham { get; set; }

        public congThucDTO() { }

        public congThucDTO(
            int maSP,
            int maNL,
            float soLuong,
            int maDonViCoSo,
            string tenDonViCoSo,
            int trangThai = 1)
        {
            MaSanPham = maSP;
            MaNguyenLieu = maNL;
            SoLuongCoSo = soLuong;
            MaDonViCoSo = maDonViCoSo;
            TenDonViCoSo = tenDonViCoSo;
            TrangThai = trangThai;
        }

        public override string ToString()
        {
            return $"{TenNguyenLieu} - {SoLuongCoSo} {TenDonViCoSo}";
        }
    }
}
