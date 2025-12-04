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

        public decimal TonKho { get; set; }

        public nguyenLieuDTO() { }
    }
}
