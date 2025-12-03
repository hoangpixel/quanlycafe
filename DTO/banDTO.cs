using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class banDTO
    {
        public int MaBan { get; set; }
        public string TenBan { get; set; }
        public byte DangSuDung { get; set; } 
        public int MaKhuVuc { get; set; }
        public byte TrangThaiXoa { get; set; } 
        public int? MaDonHienTai { get; set; }
    }
}
