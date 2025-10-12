using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quanlycafe
{
    internal class nguyenLieuDTO
    {
        private int maNguyenLieu, trangThai;
        private String tenNguyenLieu, donViCoSo;
        private float tonKho;

        private nguyenLieuDTO() { }

        private nguyenLieuDTO(int maNguyenLieu, String tenNguyenLieu, String donViCoSo, int trangThai, float tonKho)
        {
            this.maNguyenLieu = maNguyenLieu;
            this.tenNguyenLieu = tenNguyenLieu;
            this.donViCoSo = donViCoSo;
            this.trangThai = trangThai;
            this.tonKho = tonKho;
        }

        public int getMaNguyenLieu() { return maNguyenLieu; }
        public String getTenNguyenLieu() { return tenNguyenLieu; }
        public String getDonViCoSo() { return donViCoSo; }
        public int getTrangThai() { return trangThai; }
        public float getTonKho() { return tonKho; }

        public void setMaNguyenLieu(int maNguyenLieu) { this.maNguyenLieu = maNguyenLieu; }
        public void setTenNguyenLieu(String tenNguyenLieu) { this.tenNguyenLieu = tenNguyenLieu; }
        public void setDonViCoSo(String donViCoSo) { this.donViCoSo = donViCoSo; }
        public void setTrangThai(int trangThai) { this.trangThai = trangThai; }
        public void setTonKho(float tonKho) { this.tonKho = tonKho; }
    }
}
