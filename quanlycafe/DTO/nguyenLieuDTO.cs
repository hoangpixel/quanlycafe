//using System;

//namespace quanlycafe.DTO
//{
//    public class nguyenLieuDTO
//    {
//        public int MaNguyenLieu { get; set; }

//        public string TenNguyenLieu { get; set; }

//        public string DonViCoSo { get; set; }

//        public int TrangThai { get; set; }

//        public decimal TonKho { get; set; }


//        public nguyenLieuDTO() { }

//        public nguyenLieuDTO(int ma, string ten, string donVi, int trangThai, decimal tonKho)
//        {
//            MaNguyenLieu = ma;
//            TenNguyenLieu = ten;
//            DonViCoSo = donVi;
//            TrangThai = trangThai;
//            TonKho = tonKho;
//        }

//        public override string ToString()
//        {
//            return $"{TenNguyenLieu} ({DonViCoSo})";
//        }
//    }
//}

using System;

namespace quanlycafe.DTO
{
    public class nguyenLieuDTO
    {
        public int MaNguyenLieu { get; set; }

        public string TenNguyenLieu { get; set; }

        public int MaDonViCoSo { get; set; }  

        public int TrangThai { get; set; } = 1;

        public int TrangThaiDV { get; set; }

        public decimal TonKho { get; set; }

        public string TenDonViCoSo { get; set; }

        public nguyenLieuDTO() { }

        public nguyenLieuDTO(int ma, string ten, int maDonVi, int trangThai, decimal tonKho)
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

