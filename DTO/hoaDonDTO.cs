using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class hoaDonDTO
    {
        public int MaHD { get; set; }
        public int MaBan { get; set; }  // VARCHAR
        public DateTime ThoiGianTao { get; set; }
        public bool TrangThai { get; set; }
        public decimal TongTien { get; set; }
        public int MaTT { get; set; } = 1;
        public int MaKhachHang { get; set; }
        public int MaNhanVien { get; set; }
        public string TenKhachHang { get; set; } = "Khách lẻ";
        public string HoTen { get; set; } = "Nhân viên";
        public int KhoaSo { get; set; }
    }
}
