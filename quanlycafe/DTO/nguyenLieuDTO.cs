using System;

namespace quanlycafe.DTO
{
    public class nguyenLieuDTO
    {
        public int MaNguyenLieu { get; set; }

        public string TenNguyenLieu { get; set; }

        public string DonViCoSo { get; set; }

        public int TrangThai { get; set; }

        public float TonKho { get; set; }

        public nguyenLieuDTO() { }

        public nguyenLieuDTO(int ma, string ten, string donVi, int trangThai, float tonKho)
        {
            MaNguyenLieu = ma;
            TenNguyenLieu = ten;
            DonViCoSo = donVi;
            TrangThai = trangThai;
            TonKho = tonKho;
        }

        public override string ToString()
        {
            return $"{TenNguyenLieu} ({DonViCoSo})";
        }
    }
}
