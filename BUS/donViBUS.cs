using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace BUS
{
    public class donViBUS
    {
        public static BindingList<donViDTO> ds = new BindingList<donViDTO>();

        public BindingList<donViDTO> LayDanhSach()
        {
            donViDAO data = new donViDAO();
            ds = data.docDangSachDonVi();
            return ds;
        }
        public bool themDonVi(donViDTO ct)
        {
            donViDAO data = new donViDAO();
            bool kq = data.Them(ct);
            if (kq)
            {
                ds.Add(ct);
            }
            return kq;
        }

        public bool suaDonVi(donViDTO ct)
        {
            donViDAO data = new donViDAO();
            bool result = data.Sua(ct);

            if (result)
            {
                donViDTO tontai = ds.FirstOrDefault(x => x.MaDonVi == ct.MaDonVi);
                if (tontai != null)
                {
                    tontai.TenDonVi = ct.TenDonVi;
                    tontai.TrangThai = ct.TrangThai;
                }
            }
            return result;
        }

        public bool Xoa(int maDonVi)
        {
            donViDAO data = new donViDAO();
            bool result = data.Xoa(maDonVi);

            if (result)
            {
                donViDTO dv = ds.FirstOrDefault(x => x.MaDonVi == maDonVi);
                if(dv != null)
                {
                    ds.Remove(dv);
                }
            }
            else
            {
                Console.WriteLine("Lỗi khi xóa đơn vị");
            }

            return result;
        }

        public BindingList<donViDTO> layDanhSachDonViTheoNguyenLieu(int maNguyenLieu)
        {
            donViDAO dao = new donViDAO();
            return dao.layDanhSachDonViTheoNguyenLieu(maNguyenLieu);
        }

        public BindingList<donViDTO> timKiemCoBan(string tim,int index)
        {
            BindingList<donViDTO> dskq = new BindingList<donViDTO>();
            if(ds == null)
            {
                LayDanhSach();
            }
            foreach (donViDTO ct in ds)
            {
                switch(index)
                {
                    case 0:
                        {
                            if(ct.MaDonVi.ToString().Contains(tim))
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 1:
                        {
                            if(ct.TenDonVi.IndexOf(tim,StringComparison.OrdinalIgnoreCase) >= 0)
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
