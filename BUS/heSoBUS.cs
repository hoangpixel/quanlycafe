using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BUS
{
    public class heSoBUS
    {
        public static BindingList<heSoDTO> ds = new BindingList<heSoDTO>();

        public BindingList<heSoDTO> LayDanhSach()
        {
            heSoDAO data = new heSoDAO();
            ds = data.DocDanhSachHeSo();
            return ds;
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
                heSoDTO old = ds.FirstOrDefault(x => x.MaNguyenLieu == ct.MaNguyenLieu && x.MaDonVi == ct.MaDonVi);
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
                heSoDTO hs = ds.FirstOrDefault(x => x.MaNguyenLieu == maNL && x.MaDonVi == maDV);
                if(hs != null)
                {
                    ds.Remove(hs);
                }
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

        public bool kiemTraTrungHeSo(int maNL,int maDV)
        {
            heSoDTO hs = ds.FirstOrDefault(x => x.MaDonVi == maDV && x.MaNguyenLieu == maNL);
            if(hs != null)
            {
                return true;
            }
            return false;
        }
    }
}
