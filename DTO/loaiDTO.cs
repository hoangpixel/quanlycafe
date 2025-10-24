using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class loaiDTO
    {
        public int MaLoai { get; set; }
        public string TenLoai { get; set; }

        public loaiDTO() { }

        public loaiDTO(int maLoai, string tenLoai)
        {
            MaLoai = maLoai;
            TenLoai = tenLoai;
        }
    }
}
