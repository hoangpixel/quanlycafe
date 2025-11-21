using DAO;
using DTO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace BUS
{
    public class nhomBUS
    {
        private nhomDAO data = new nhomDAO();
        public static BindingList<nhomDTO> ds = new BindingList<nhomDTO>();
        public BindingList<nhomDTO> layDanhSach()
        {
            if (ds == null || ds.Count == 0)
            {
                ds = new BindingList<nhomDTO>(data.LayDanhSach());
            }
            return ds;
        }
        public bool them(nhomDTO ct)
        {
            bool kq = data.ThemNhom(ct);
            if (kq)
            {
                ds.Add(ct);
            }
            return kq;
        }

        public bool sua(nhomDTO ct)
        {
            bool kq = data.SuaNhom(ct);
            if (kq)
            {
                nhomDTO tontai = ds.FirstOrDefault(x => x.MaNhom == ct.MaNhom);
                if (tontai != null)
                {
                    tontai.TenNhom = ct.TenNhom;
                }
            }
            return kq;
        }

        public bool xoa(int maXoa)
        {
            bool kq = data.XoaNhom(maXoa);
            if (kq)
            {
                nhomDTO n = ds.FirstOrDefault(x => x.MaNhom == maXoa);
                if (n != null)
                {
                    ds.Remove(n);
                }
            }
            return kq;
        }
    }
}