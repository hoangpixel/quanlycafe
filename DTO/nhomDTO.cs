using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class nhomDTO
    {
        public int MaNhom { get; set; }
        public string TenNhom { get; set; }
        public int TrangThai { get; set; } = 1;
        
        public nhomDTO() { }
        public nhomDTO(int MaNhom,string TenNhom,int TrangThai)
        {
            this.MaNhom = MaNhom;
            this.TenNhom = TenNhom;
            this.TrangThai = TrangThai;
        }
    }
}
