using System;

namespace DTO
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
        public int SoLuongToiDa { get; set; }

        public sanPhamDTO() { }
    }
}