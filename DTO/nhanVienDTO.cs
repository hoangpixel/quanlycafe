using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class nhanVienDTO
    {
        public int MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public float Luong { get; set; }
        public int TrangThai { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;

    }
}
