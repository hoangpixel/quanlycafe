using System;

namespace quanlycafe.DTO
{
    public class sanPhamDTO
    {
        public int MaSP { get; set; }
        public int MaLoai { get; set; }
        public string Hinh { get; set; }
        public string TenSP { get; set; }
        public int TrangThai { get; set; }
        public int TrangThaiCT { get; set; }
        public float Gia { get; set; }
        public string TenLoai { get; set; }
        public float SoLuongCoSo { get; set; }

        public sanPhamDTO() { }

        public sanPhamDTO(int maSP, int maLoai, string hinh, string tenSP, int trangThai, int trangThaiCT, float gia)
        {
            MaSP = maSP;
            MaLoai = maLoai;
            Hinh = hinh;
            TenSP = tenSP;
            TrangThai = trangThai;
            TrangThaiCT = trangThaiCT;
            Gia = gia;
        }
    }
}
