using System;

namespace DTO
{
    public class nguyenLieuDTO
    {
        public int MaNguyenLieu { get; set; }

        public string TenNguyenLieu { get; set; }

        public int MaDonViCoSo { get; set; }

        public int TrangThai { get; set; } = 1;

        public int TrangThaiDV { get; set; }

        public float TonKho { get; set; }

        public string TenDonViCoSo { get; set; }

        public nguyenLieuDTO() { }

        public nguyenLieuDTO(int ma, string ten, int maDonVi, int trangThai, float tonKho)
        {
            MaNguyenLieu = ma;
            TenNguyenLieu = ten;
            MaDonViCoSo = maDonVi;
            TrangThai = trangThai;
            TonKho = tonKho;
        }

        public override string ToString()
        {
            return TenDonViCoSo != null
                ? $"{TenNguyenLieu} ({TenDonViCoSo})"
                : $"{TenNguyenLieu}";
        }
    }
}
