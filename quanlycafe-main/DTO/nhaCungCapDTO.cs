using System;

namespace DTO
{
    public class nhaCungCapDTO
    {
        public int MaNCC { get; set; }
        public string TenNCC { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public int ConHoatDong { get; set; } = 1;

        public nhaCungCapDTO() { }
    }
}