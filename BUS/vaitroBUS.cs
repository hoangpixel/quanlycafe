using DAO;
using DTO;
using System;
using System.ComponentModel;
using System.Linq;

namespace BUS
{
    public class vaitroBUS
    {
        private vaitroDAO data = new vaitroDAO();
        public static BindingList<vaitroDTO> ds = new BindingList<vaitroDTO>();
        public BindingList<vaitroDTO> LayDanhSach()
        {
            if (ds == null || ds.Count <= 0)
            {
                ds = data.LayDanhSachVaiTro();
            }
            return ds;
        }

        public int layMa()
        {
            return data.layMa();
        }

        public bool themVaiTro(vaitroDTO ct)
        {
            bool kq = data.ThemVaiTro(ct);
            if(kq)
            {
                ds.Add(ct);
            }
            return kq;
        }

        public bool suaVaiTro(vaitroDTO vt)
        {
            bool kq = data.SuaVaiTro(vt);
            if (kq)
            {
                vaitroDTO tontai = ds.FirstOrDefault(x => x.MaVaiTro == vt.MaVaiTro);
                if (tontai != null)
                {
                    tontai.TenVaiTro = vt.TenVaiTro;
                }
            }
            return kq;
        }

        public bool xoaVaiTro(int maXoa)
        {
            bool kq = data.XoaVaiTro(maXoa);
            if (kq)
            {
                vaitroDTO tontai = ds.FirstOrDefault(x => x.MaVaiTro == maXoa);
                if (tontai != null)
                {
                    ds.Remove(tontai);
                }
            }
            return kq;
        }

        public bool kiemTraTrungTen(string tenVaiTro)
        {
            vaitroDTO tontai = ds.FirstOrDefault(x => x.TenVaiTro.Equals(tenVaiTro.Trim(), StringComparison.OrdinalIgnoreCase)); 
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