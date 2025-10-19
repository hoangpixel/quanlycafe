using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using quanlycafe.DAO;
using quanlycafe.DTO;
namespace quanlycafe.BUS
{
    internal class heSoBUS
    {
        public static List<heSoDTO> ds = new List<heSoDTO>();

        public void docDanhSachHeSo()
        {
            heSoDAO data = new heSoDAO();
            ds = data.DocDanhSachHeSo();
        }

        public List<heSoDTO> layDanhSachheSo()
        {
            heSoDAO data = new heSoDAO();
            return data.DocDanhSachHeSo();
        }

        public bool Them(heSoDTO ct)
        {
            heSoDAO data = new heSoDAO();
            bool kq = data.Them(ct);
            if(kq)
            {
                ds.Add(ct);
            }
            return kq;
        }

        public bool Sua(heSoDTO ct)
        {
            heSoDAO data = new heSoDAO();
            bool kq = data.CapNhat(ct);
            if (kq)
            {
                var old = ds.FirstOrDefault(x => x.MaNguyenLieu == ct.MaNguyenLieu && x.MaDonVi == ct.MaDonVi);
                if (old != null)
                {
                    old.HeSo = ct.HeSo;
                }
            }
            return kq;
        }

        public bool Xoa(int maNL, int maDV)
        {
            heSoDAO data = new heSoDAO();
            bool kq = data.Xoa(maNL, maDV);
            if(kq)
            {
                ds.RemoveAll(x => x.MaNguyenLieu == maNL && x.MaDonVi == maDV);
            }
            else
            {
                Console.WriteLine("Lỗi khi xóa hế số");
            }
            return kq;
        }

        public List<heSoDTO> layDanhSachTheoMaNL(int maNL)
        {
            heSoDAO data = new heSoDAO();
            return data.layDanhSachHeSoTheoNL(maNL);
        }
    }
}
