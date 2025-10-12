using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quanlycafe
{
    internal class congThucDTO
    {
        private int maSanPham;
        private int maNguyenLieu;
        private float soLuongCoSo;

        private congThucDTO() { }

        private congThucDTO(int maSanPham, int maNguyenLieu, float soLuongCoSo)
        {
            this.maSanPham = maSanPham;
            this.maNguyenLieu = maNguyenLieu;
            this.soLuongCoSo = soLuongCoSo;
        }

        public int getMaSanPham() { return maSanPham; }
        public int getMaNguyenLieu() { return maNguyenLieu; }
        public float getSoLuongCoSo() { return soLuongCoSo; }

        public void setMaSanPham(int maSanPham) { this.maSanPham = maSanPham; }
        public void setMaNguyenLieu(int maNguyenLieu) { this.maNguyenLieu = maNguyenLieu; }
        public void setSoLuongCoSo(float soLuongCoSo) { this.soLuongCoSo = soLuongCoSo; }
    }
}
