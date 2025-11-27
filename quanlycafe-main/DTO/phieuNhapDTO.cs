// TÊN FILE: phieuNhapDTO.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class phieuNhapDTO
    {
        public int MaPN { get; set; }
        public int MaNCC { get; set; }
        public int MANHANVIEN { get; set; }      
        public decimal TongTien { get; set; } 
        public DateTime ThoiGian { get; set; } = DateTime.Now;
        public int TrangThai { get; set; } = 1;

        
        public List<ctPhieuNhapDTO> ChiTiet { get; set; } = new List<ctPhieuNhapDTO>();
        public string TenNCC { get; set; } 
        public string TenNV { get; set; }  
        public phieuNhapDTO() { }

        public phieuNhapDTO(int maNcc)
        {
            MaNCC = maNcc;
            ThoiGian = DateTime.Now;
            TrangThai = 1;
        }
    }
}