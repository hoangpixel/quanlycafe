using System;

namespace DTO
{
    public class heSoDTO
    {
        public int MaNguyenLieu { get; set; }
        public int MaDonVi { get; set; }
        public float HeSo { get; set; }

        public heSoDTO() { }

        public heSoDTO(int maNguyenLieu, int maDonVi, float heSo)
        {
            MaNguyenLieu = maNguyenLieu;
            MaDonVi = maDonVi;
            HeSo = heSo;
        }
    }
}
