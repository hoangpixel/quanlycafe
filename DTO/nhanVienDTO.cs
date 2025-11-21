using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class nhanVienDTO
    {
        // Các thuộc tính đúng tên cột trong DB
        public int MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public decimal Luong { get; set; } = 1; // 1 = hoạt động, 0 = khóa
        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Thuộc tính tiện dụng để hiển thị (không cần thiết nhưng hay dùng)
        public string HienThi
        {
            get
            {
                return $"{HoTen} ({SoDienThoai})";
            }
        }

        // Constructor mặc định
        public nhanVienDTO() { }

        // Constructor có tham số (dùng khi thêm mới)
        public nhanVienDTO(string ten, string sdt, string email = "")
        {
            HoTen = ten;
            SoDienThoai = sdt;
            Email = email;
        }

        // ToString để hiển thị trong ComboBox, ListBox...
        public override string ToString()
        {
            return $"{HoTen} - {SoDienThoai}";
        }
    }
}
