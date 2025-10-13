using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using quanlycafe.DAO;
using quanlycafe.DTO;

namespace quanlycafe.BUS
{
    internal class loaiSanPhamBUS
    {
        public static List<loaiDTO> ds = new List<loaiDTO>();

        public List<loaiDTO> layDanhSachLoai()
        {
            loaiSanPhamDAO data = new loaiSanPhamDAO(); 
            return data.docDanhSachLoai();             
        }

        public void docDsLoaiSP()
        {
            loaiSanPhamDAO data = new loaiSanPhamDAO();
            ds = data.docDanhSachLoai();
        }

        public bool themLoai(loaiDTO ct)
        {
            loaiSanPhamDAO data = new loaiSanPhamDAO();
            bool kq = data.Them(ct);
            if(kq)
            {
                ds.Add(ct);
            }
            return kq;
        }

        public bool suaLoai(loaiDTO ct)
        {
            loaiSanPhamDAO data = new loaiSanPhamDAO();
            bool result = data.Sua(ct);

            if (result)
            {
                var existing = ds.Find(x => x.MaLoai == ct.MaLoai);
                if (existing != null)
                {
                    existing.TenLoai = ct.TenLoai;
                }
                Console.WriteLine("BUS: Sửa loại sản phẩm thành công!");
            }
            else
            {
                Console.WriteLine("BUS: Lỗi khi sửa loại sản phẩm!");
            }

            return result;
        }

        public bool Xoa(int maLoai)
        {
            loaiSanPhamDAO data = new loaiSanPhamDAO();
            bool result = data.Xoa(maLoai);

            if (result)
            {
                ds.RemoveAll(x => x.MaLoai == maLoai);
                Console.WriteLine("BUS: Xóa loại sản phẩm thành công!");
            }
            else
            {
                Console.WriteLine("BUS: Lỗi khi xóa loại sản phẩm!");
            }

            return result;
        }
    }
}
