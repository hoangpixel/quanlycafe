using System;
using System.Collections.Generic;
using System.Linq;
using quanlycafe.DAO;
using quanlycafe.DTO;

namespace quanlycafe.BUS
{
    internal class congThucBUS
    {
        public static List<congThucDTO> ds = new List<congThucDTO>();

        public List<congThucDTO> docDSCongThucTheoSP(int maSP)
        {
            congThucDAO data = new congThucDAO();
            ds = data.docDanhSachCongThucTheoSP(maSP);
            return ds;
        }

        public List<sanPhamDTO> docDSSanPhamTheoNguyenLieu(int maNL)
        {
            congThucDAO data = new congThucDAO();
            return data.docDanhSachSanPhamTheoNguyenLieu(maNL);
        }


        public List<congThucDTO> docTatCaCongThuc()
        {
            congThucDAO data = new congThucDAO();
            return data.docTatCaCongThuc();
        }

        public bool themCongThuc(congThucDTO ct)
        {
            ct.TrangThai = 1;
            congThucDAO data = new congThucDAO();
            bool kq = data.ThemHoacCapNhat(ct);

            if (kq)
            {
                ds.Add(ct);
                Console.WriteLine($"BUS: Đã thêm công thức cho SP {ct.MaSanPham}, NL {ct.MaNguyenLieu}");

                // ✅ Cập nhật trạng thái CT cho sản phẩm
                SanPhamDAO spDAO = new SanPhamDAO();
                spDAO.CapNhatTrangThaiCT(ct.MaSanPham, 1);
            }

            return kq;
        }


        public bool suaCongThuc(congThucDTO ct)
        {
            congThucDAO data = new congThucDAO();
            bool result = data.Sua(ct);
            if (result)
            {
                var existing = ds.FirstOrDefault(x => x.MaSanPham == ct.MaSanPham && x.MaNguyenLieu == ct.MaNguyenLieu);
                if (existing != null)
                {
                    existing.SoLuongCoSo = ct.SoLuongCoSo;
                    existing.TrangThai = ct.TrangThai;
                }
            }
            return result;
        }

        public bool xoaCongThuc(int maSP, int maNL)
        {
            congThucDAO data = new congThucDAO();
            bool result = data.Xoa(maSP, maNL);
            if (result)
            {
                ds.RemoveAll(x => x.MaSanPham == maSP && x.MaNguyenLieu == maNL);
            }
            return result;
        }

    }
}
