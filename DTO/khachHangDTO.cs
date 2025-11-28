using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class khachHangDTO
    {
        // Các thuộc tính đúng tên cột trong DB
        public int MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public int TrangThai { get; set; } = 1;
        public DateTime NgayTao { get; set; } = DateTime.Now;

        public khachHangDTO() { }

        public khachHangDTO(string ten, string sdt, string email = "")
        {
            TenKhachHang = ten;
            SoDienThoai = sdt;
            Email = email;
        }
    }
}
