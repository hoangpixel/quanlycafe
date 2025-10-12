using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quanlycafe
{
    internal class productDTO
    {
        private int maSP, maLoai, trangThai, trangThaiCT;
        private String tenSP, hinh;
        private float gia;
        private productDTO() { }
        private productDTO(int maSP, int maLoai, String hinh, String tenSP, int trangThai, int trangThaiCT, float gia)
        {
            this.maSP = maSP;
            this.maLoai = maLoai;
            this.hinh = hinh;
            this.tenSP = tenSP;
            this.trangThai = trangThai;
            this.trangThaiCT = trangThaiCT;
            this.gia = gia;
        }
        public int getMaSP() { return maSP; }
        public int getMaLoai() { return maLoai; }
        public String getHinh() { return hinh; }
        public String getTenSP() { return tenSP; }
        public int getTrangThai() { return trangThai; }
        public int getTrangThaiCT() { return trangThaiCT; }
        public float getGia() { return gia; }

        public void setMaSP(int maSP) { this.maSP = maSP; }
        public void setMaLoai(int maLoai) { this.maLoai = maLoai; }
        public void setHinh(String hinh) { this.hinh = hinh; }
        public void setTenSP(String tenSP) { this.tenSP = tenSP; }
        public void setTrangThai(int trangThai) { this.trangThai = trangThai; }
        public void setTrangThaiCT(int trangThaiCT) { this.trangThaiCT = trangThaiCT; }
        public void setGia(float gia) { this.gia = gia; }
    }
}
