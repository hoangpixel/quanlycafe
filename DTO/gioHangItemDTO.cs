using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class gioHangItemDTO
    {
        public sanPhamDTO SanPham { get; set; }
        public int SoLuong { get; set; }

        public decimal ThanhTien => (decimal)SanPham.Gia * SoLuong;
    }
}

