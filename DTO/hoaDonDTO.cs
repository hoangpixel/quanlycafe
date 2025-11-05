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
        public string MaBan { get; set; }  // VARCHAR
        public DateTime ThoiGianTao { get; set; }
        public string TrangThai { get; set; }
        public decimal TongTien { get; set; }
    }
}
