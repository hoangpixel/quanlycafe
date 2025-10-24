using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DAO;
using DTO;
namespace BUS
{
    public class donViBUS
    {
        public static List<donViDTO> ds = new List<donViDTO>();
        public void docDanhSachDonVi()
        {
            donViDAO data = new donViDAO();
            ds = data.docDangSachDonVi();
        }
        public List<donViDTO> layDanhSachDonVi()
        {
            donViDAO data = new donViDAO();
            return data.docDangSachDonVi();
        }

        public static List<donViDTO> layDanhSachTK()
        {
            donViDAO data = new donViDAO();
            return data.docDangSachDonVi();
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
                var existing = ds.Find(x => x.MaDonVi == ct.MaDonVi);
                if (existing != null)
                {
                    existing.TenDonVi = ct.TenDonVi;
                    existing.TrangThai = ct.TrangThai;
                }
            }
            else
            {
                Console.WriteLine("Lỗi khi sửa đơn vị");
            }

            return result;
        }

        public bool Xoa(int maDonVi)
        {
            donViDAO data = new donViDAO();
            bool result = data.Xoa(maDonVi);

            if (result)
            {
                ds.RemoveAll(x => x.MaDonVi == maDonVi);
            }
            else
            {
                Console.WriteLine("Lỗi khi xóa đơn vị");
            }

            return result;
        }
    }
}
