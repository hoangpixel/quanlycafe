using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DTO
{
    public class ctPhieuNhapDTO
    {
        public int MaCTPN { get; set; }
        public int MaPN { get; set; }
        public int MaNguyenLieu { get; set; }
        public int MaDonVi { get; set; }
        public decimal SoLuong { get; set; }
        public decimal SoLuongCoSo { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string TenNguyenLieu { get; set; }
        public string TenDonVi { get; set; }
        public ctPhieuNhapDTO() { }

        public ctPhieuNhapDTO(int maNguyenLieu, int maDonVi, decimal soLuong, decimal donGia)
        {
            MaNguyenLieu = maNguyenLieu;
            MaDonVi = maDonVi;
            SoLuong = soLuong;
            SoLuongCoSo = soLuong;
            DonGia = donGia;
            ThanhTien = soLuong * donGia;
        }
    }
}