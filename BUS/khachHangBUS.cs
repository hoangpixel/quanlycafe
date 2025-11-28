using DAO;
using DTO;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;

namespace BUS
{
    public class khachHangBUS
    {
        private khachHangDAO data = new khachHangDAO();
        public static BindingList<khachHangDTO> ds = new BindingList<khachHangDTO>();

        public BindingList<khachHangDTO> LayDanhSach()
        {
            if(ds == null || ds.Count <= 0)
            {
                ds = data.LayDanhSach();
            }
            return ds;
        }

        public int layMa()
        {
            return data.layMa();
        }

        public bool them(khachHangDTO kh)
        {
            bool kq = data.Them(kh);
            if(kq)
            {
                ds.Add(kh);
            }
            return kq;
        }
        public bool sua(khachHangDTO kh)
        {
            bool kq = data.Sua(kh);
            if(kq)
            {
                khachHangDTO tontai = ds.FirstOrDefault(x => x.MaKhachHang == kh.MaKhachHang);
                if(tontai != null)
                {
                    tontai.TenKhachHang = kh.TenKhachHang;
                    tontai.SoDienThoai = kh.SoDienThoai;
                    tontai.Email = kh.Email;
                }
            }
            return kq;
        }
        public bool xoa(int maXoa)
        {
            bool kq = data.Xoa(maXoa);
            if(kq)
            {
                khachHangDTO tontai = ds.FirstOrDefault(x => x.MaKhachHang == maXoa);
                if(tontai != null)
                {
                    ds.Remove(tontai);
                }
            }
            return kq;
        }
        public bool kiemTraTrungEmail(string email)
        {
            khachHangDTO kh = ds.FirstOrDefault(x => x.Email.Equals(email));
            if(kh != null)
            {
                return true;
            }
            return false;
        }
        public bool kiemTraTrungSDT(string sdt)
        {
            khachHangDTO kh = ds.FirstOrDefault(x => x.SoDienThoai.Equals(sdt));
            if(kh != null)
            {
                return true;
            }
            return false;
        }
    }
}