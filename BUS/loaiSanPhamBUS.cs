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
        private loaiSanPhamDAO data = new loaiSanPhamDAO();
        public BindingList<loaiDTO> LayDanhSach()
        {
            if(ds == null || ds.Count == 0)
            {
                ds = new BindingList<loaiDTO>(data.LayDanhSach());
            }
            return ds;
        }

        public int layMa()
        {
            return data.LayMa();
        }

        public bool themLoai(loaiDTO ct)
        {
            bool kq = data.them(ct);
            if(kq)
            {
                ds.Add(ct);
            }
            return kq;
        }

        public bool suaLoai(loaiDTO ct)
        {
            bool result = data.sua(ct);
            if (result)
            {
                loaiDTO tontai = ds.FirstOrDefault(x => x.MaLoai == ct.MaLoai);
                if (tontai != null)
                {
                    tontai.TenLoai = ct.TenLoai;
                    tontai.MaNhom = ct.MaNhom;
                }
            }
            return result;
        }

        public bool Xoa(int maLoai)
        {
            bool result = data.xoa(maLoai);
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

        public BindingList<loaiDTO> timKiemCoBan(string tim, int index)
        {
            BindingList<loaiDTO> dskq = new BindingList<loaiDTO>();
            if(ds == null)
            {
                LayDanhSach();
            }

            nhomBUS busNhom = new nhomBUS();
            BindingList<nhomDTO> dsNhom = busNhom.layDanhSach();

            foreach (loaiDTO ct in ds)
            {
                switch(index)
                {
                    case 0:
                        {
                            if(ct.MaLoai.ToString().Contains(tim))
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 1:
                        {
                            if(ct.MaNhom.ToString().Contains(tim))
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 2:
                        {
                            if (ct.TenLoai.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 3:
                        {
                            nhomDTO nhom = dsNhom.FirstOrDefault(x => x.MaNhom == ct.MaNhom);
                            string tenNhom = nhom != null ? nhom.TenNhom : "";
                            if(tenNhom.IndexOf(tim,StringComparison.OrdinalIgnoreCase) >= 0)
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
