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
        public byte TrangThai { get; set; } = 1; // 1 = hoạt động, 0 = khóa
        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Thuộc tính tiện dụng để hiển thị (không cần thiết nhưng hay dùng)
        public string HienThi
        {
            get
            {
                return $"{TenKhachHang} ({SoDienThoai})";
            }
        }

        // Constructor mặc định
        public khachHangDTO() { }

        // Constructor có tham số (dùng khi thêm mới)
        public khachHangDTO(string ten, string sdt, string email = "")
        {
            TenKhachHang = ten;
            SoDienThoai = sdt;
            Email = email;
        }

        // ToString để hiển thị trong ComboBox, ListBox...
        public override string ToString()
        {
            return $"{TenKhachHang} - {SoDienThoai}";
        }
    }
}
