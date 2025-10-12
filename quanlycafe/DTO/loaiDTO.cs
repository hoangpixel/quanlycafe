using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quanlycafe
{
    internal class loaiDTO
    {
        private int maLoai;
        private String tenLoai;

        private loaiDTO() { }

        private loaiDTO(int maLoai, String tenLoai)
        {
            this.maLoai = maLoai;
            this.tenLoai = tenLoai;
        }

        public int getMaLoai() { return maLoai; }
        public String getTenLoai() { return tenLoai; }

        public void setMaLoai(int maLoai) { this.maLoai = maLoai; }
        public void setTenLoai(String tenLoai) { this.tenLoai = tenLoai; }
    }
}
