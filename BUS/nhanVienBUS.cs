using DAO;
using DTO;
using System;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace BUS
{
    public class nhanVienBUS
    {
        private nhanVienDAO data = new nhanVienDAO();
        public static BindingList<nhanVienDTO> ds = new BindingList<nhanVienDTO>();

        public BindingList<nhanVienDTO> LayDanhSach()
        {
            if(ds == null || ds.Count == 0)
            {
                ds = data.LayDanhSach();
            }
            return ds;
        }

        public bool ThemNhanVien(nhanVienDTO nv)
        {
            bool kq = data.ThemNhanVien(nv);
            if(kq)
            {
                ds.Add(nv);
            }
            return kq;
        }

        public bool CapNhatNhanVien(nhanVienDTO nv)
        {
            bool kq = data.CapNhatNhanVien(nv);
            if(kq)
            {
                nhanVienDTO tontai = ds.FirstOrDefault(x => x.MaNhanVien == nv.MaNhanVien);
                if(tontai != null)
                {
                    tontai.HoTen = nv.HoTen;
                    tontai.SoDienThoai = nv.SoDienThoai;
                    tontai.Email = nv.Email;
                    tontai.Luong = nv.Luong;
                }
            }
            return kq;
        }

        public bool XoaNhanVien(int maNV)
        {
            bool kq = data.XoaNhanVien(maNV);
            if(kq)
            {
                nhanVienDTO nv = ds.FirstOrDefault(x => x.MaNhanVien == maNV);
                if(nv != null)
                {
                    ds.Remove(nv);
                }
            }
            return kq;
        }

        public int LayMa()
        {
            return data.layMa();
        }

        public BindingList<nhanVienDTO> timKiemCoban(string tim, int index)
        {
            BindingList<nhanVienDTO> dskq = new BindingList<nhanVienDTO>();
            if (ds == null || ds.Count == 0)
            {
                LayDanhSach();
            }

            foreach (nhanVienDTO ct in ds)
            {
                switch (index)
                {
                    case 0:
                        {
                            if (ct.MaNhanVien.ToString().Contains(tim))
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 1:
                        {
                            if (ct.HoTen.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 2:
                        {
                            if (ct.SoDienThoai.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 3:
                        {
                            if (ct.Email.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                }
            }
            return dskq;
        }
    }
}