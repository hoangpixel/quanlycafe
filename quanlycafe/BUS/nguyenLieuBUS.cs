using System;
using System.Collections.Generic;
using quanlycafe.DAO;
using quanlycafe.DTO;

namespace quanlycafe.BUS
{
    internal class nguyenLieuBUS
    {
        public static List<nguyenLieuDTO> ds = new List<nguyenLieuDTO>();

        public List<nguyenLieuDTO> docDSNguyenLieu()
        {
            nguyenLieuDAO data = new nguyenLieuDAO();
            return data.docDanhSachNguyenLieu();
        }

        public void napDSNguyenLieu()
        {
            nguyenLieuDAO data = new nguyenLieuDAO();
            ds = data.docDanhSachNguyenLieu();
        }

        public bool themNguyenLieu(nguyenLieuDTO nl)
        {
            nguyenLieuDAO data = new nguyenLieuDAO();
            bool kq = data.Them(nl);
            if (kq)
            {
                ds.Add(nl);
                Console.WriteLine($"BUS: Đã thêm nguyên liệu '{nl.TenNguyenLieu}' thành công!");
            }
            else
            {
                Console.WriteLine("BUS: Lỗi khi thêm nguyên liệu!");
            }
            return kq;
        }

        public bool suaNguyenLieu(nguyenLieuDTO nl)
        {
            nguyenLieuDAO data = new nguyenLieuDAO();
            bool result = data.Sua(nl);

            if (result)
            {
                var existing = ds.Find(x => x.MaNguyenLieu == nl.MaNguyenLieu);
                if (existing != null)
                {
                    existing.TenNguyenLieu = nl.TenNguyenLieu;
                    existing.DonViCoSo = nl.DonViCoSo;
                    existing.TonKho = nl.TonKho;
                    existing.TrangThai = nl.TrangThai;
                }
                Console.WriteLine("BUS: Sửa nguyên liệu thành công!");
            }
            else
            {
                Console.WriteLine("BUS: Lỗi khi sửa nguyên liệu!");
            }

            return result;
        }

        public bool xoaNguyenLieu(int maNguyenLieu)
        {
            nguyenLieuDAO data = new nguyenLieuDAO();
            bool result = data.Xoa(maNguyenLieu);

            if (result)
            {
                ds.RemoveAll(x => x.MaNguyenLieu == maNguyenLieu);
                Console.WriteLine("BUS: Đã ẩn nguyên liệu thành công!");
            }
            else
            {
                Console.WriteLine("BUS: Lỗi khi ẩn nguyên liệu!");
            }

            return result;
        }
    }
}
