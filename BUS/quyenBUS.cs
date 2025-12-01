using DAO;
using DTO;
using System;
using System.ComponentModel;
using System.Linq;

namespace BUS
{
    public class quyenBUS
    {
        private quyenDAO data = new quyenDAO();
        public static BindingList<quyenDTO> ds = new BindingList<quyenDTO>();

        public BindingList<quyenDTO> LayDanhSach()
        {
            if (ds == null || ds.Count <= 0)
            {
                ds = data.LayDanhSachQuyen();
            }
            return ds;
        }

        public bool themQuyen(quyenDTO ct)
        {
            bool kq = data.ThemQuyen(ct);
            if(kq)
            {
                ds.Add(ct);
            }
            return kq;
        }

        public int LayMa()
        {
            return data.layMa();
        }

        public bool suaQuyen(quyenDTO ct)
        {
            bool kq = data.SuaQuyen(ct);
            if(kq)
            {
                quyenDTO tontai = ds.FirstOrDefault(x => x.MaQuyen == ct.MaQuyen);
                if(tontai != null)
                {
                    tontai.TenQuyen = ct.TenQuyen;
                }
            }
            return kq;
        }

        public bool xoaQuyen(int maXoa)
        {
            bool kq = data.XoaQuyen(maXoa);
            if(kq)
            {
                quyenDTO tontai = ds.FirstOrDefault(x => x.MaQuyen == maXoa);
                if(tontai != null)
                {
                    ds.Remove(tontai);
                }
            }
            return kq;
        }


        public bool kiemTraTrungTen(string tenQuyen)
        {
            quyenDTO tontai = ds.FirstOrDefault(x => x.TenQuyen.Equals(tenQuyen.Trim(), StringComparison.OrdinalIgnoreCase));
            if (tontai != null)
            {
                return true;
            }
            return false;
        }

        public bool KiemTraRong(string tenVaiTro)
        {
            return string.IsNullOrWhiteSpace(tenVaiTro);
        }
    }
}