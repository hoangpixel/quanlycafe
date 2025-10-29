using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;
using DTO;

namespace BUS
{
    public class loaiSanPhamBUS
    {
        public static BindingList<loaiDTO> ds = new BindingList<loaiDTO>();

        public BindingList<loaiDTO> LayDanhSach()
        {
            loaiSanPhamDAO data = new loaiSanPhamDAO();
            ds = data.docDanhSachLoai();
            return ds;
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
                loaiDTO tontai = ds.FirstOrDefault(x => x.MaLoai == ct.MaLoai);
                if (tontai != null)
                {
                    tontai.TenLoai = ct.TenLoai;
                }
            }
            return result;
        }

        public bool Xoa(int maLoai)
        {
            loaiSanPhamDAO data = new loaiSanPhamDAO();
            bool result = data.Xoa(maLoai);
            if (result)
            {
                loaiDTO loai = ds.FirstOrDefault(x => x.MaLoai == maLoai);
                if(loai != null)
                {
                    ds.Remove(loai);
                }
            }

            return result;
        }
    }
}
