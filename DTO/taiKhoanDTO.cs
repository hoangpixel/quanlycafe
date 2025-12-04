using System;

namespace DTO
{
    public class taikhoanDTO
    {
        public int MAtaikHOAN { get; set; }
        public int MANHANVIEN { get; set; }
        public string TENDANGNHAP { get; set; }
        public string MATKHAU { get; set; }
        public bool TRANGTHAI { get; set; }
        public DateTime NGAYTAO { get; set; }
        public int MAVAITRO { get; set; }

        // JOIN từ bảng khác
        public string TENNHANVIEN { get; set; } // Từ NHANVIEN
        public string TENVAITRO { get; set; } // Từ VAITRO

        // Hiển thị
        public string TrangThaiText => TRANGTHAI ? "Hoạt động" : "Bị khóa";
    }
}