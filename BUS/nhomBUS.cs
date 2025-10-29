using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class nhomBUS
    {
        public static BindingList<nhomDTO> ds = new BindingList<nhomDTO>();
        public void docDanhSach()
        {
            nhomDAO data = new nhomDAO();
            ds = data.layDanhSachNhom();
        }
        public BindingList<nhomDTO> layDanhSach()
        {
            nhomDAO data = new nhomDAO();
            return data.layDanhSachNhom();
        }
        public bool them(nhomDTO ct)
        {
            nhomDAO data = new nhomDAO();
            bool kq = data.them(ct);
            if(kq)
            {
                ds.Add(ct);
            }
            return kq;
        }
        public bool sua(nhomDTO ct)
        {
            nhomDAO data = new nhomDAO();
            bool kq = data.sua(ct);
            if(kq)
            {
                nhomDTO tonTai = ds.FirstOrDefault(x => x.MaNhom == ct.MaNhom);
                if(tonTai != null)
                {
                    tonTai.TenNhom = ct.TenNhom;
                }
            }
            return kq;
        }
        public bool xoa(int maXoa)
        {
            nhomDAO data = new nhomDAO();
            bool kq = data.xoa(maXoa);
            if(kq)
            {
                nhomDTO nhom = ds.FirstOrDefault(x => x.MaNhom == maXoa);
                if(nhom != null)
                {
                    ds.Remove(nhom);
                }
            }
            return kq;
        }
    }
}
