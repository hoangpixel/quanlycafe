using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using quanlycafe.DAO;
using quanlycafe.DTO;

namespace quanlycafe.BUS
{
    internal class SanPhamBUS
    {
        public static List<sanPhamDTO> ds;

        public void docDSSanPham()
        {
            SanPhamDAO data = new SanPhamDAO();
            ds = data.DocDanhSachSanPham();
        }

        public bool them(sanPhamDTO ct)
        {
            SanPhamDAO data = new SanPhamDAO();
            bool kq = data.Them(ct);
            if(kq)
            {
                ds.Add(ct);
            }
            return kq;
        }

        public bool Sua(sanPhamDTO sp)
        {
            SanPhamDAO data = new SanPhamDAO();
            bool result = data.Sua(sp);

            if (result)
            {
                var existing = ds.Find(x => x.MaSP == sp.MaSP);
                if (existing != null)
                {
                    existing.MaLoai = sp.MaLoai;
                    existing.TenSP = sp.TenSP;
                    existing.Hinh = sp.Hinh;
                    existing.TrangThai = sp.TrangThai;
                    existing.TrangThaiCT = sp.TrangThaiCT;
                    existing.Gia = sp.Gia;
                }
                Console.WriteLine("BUS: Sửa sản phẩm thành công!");
            }
            else
            {
                Console.WriteLine("BUS: Lỗi khi sửa sản phẩm!");
            }

            return result;
        }

        public bool Xoa(int maSP)
        {
            SanPhamDAO data = new SanPhamDAO();
            bool result = data.Xoa(maSP);

            if (result)
            {
                ds.RemoveAll(x => x.MaSP == maSP);
                Console.WriteLine("BUS: Xóa sản phẩm thành công!");
            }
            else
            {
                Console.WriteLine("BUS: Lỗi khi xóa sản phẩm!");
            }

            return result;
        }
    }
}
