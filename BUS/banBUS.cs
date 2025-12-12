using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BUS
{
    public class banBUS
    {
        public static BindingList<banDTO> ds = new BindingList<banDTO>();
        private banDAO data = new banDAO();

        public BindingList<banDTO> LayDanhSach()
        {
            if(ds == null || ds.Count == 0)
            {
                ds = data.LayDanhSachBan();
            }
            return ds;
        }
        //public bool DoiTrangThai(int maBan)
        //{
        //    return data.DoiTrangThai(maBan);
        //}

        public int LayMa()
        {
            return data.LayMa();
        }

        public bool ThemBan(banDTO ban)
        {
            bool kq = data.ThemBan(ban);
            if (kq)
            {
                ds.Add(ban);
            }
            return kq;
        }

        public bool SuaBan(banDTO ban)
        {
            bool kq = data.SuaBan(ban);
            if (kq)
            {
                banDTO tontai = ds.FirstOrDefault(x => x.MaBan == ban.MaBan);
                if (tontai != null)
                {
                    tontai.TenBan = ban.TenBan;
                    tontai.MaKhuVuc = ban.MaKhuVuc;
                }
            }
            return kq;
        }

        public bool XoaBan(int maBan)
        {
            bool kq = data.XoaBan(maBan);
            if (kq)
            {
                banDTO tontai = ds.FirstOrDefault(x => x.MaBan == maBan);
                if (tontai != null)
                {
                    ds.Remove(tontai);
                }
            }
            return kq;
        }
        public BindingList<banDTO> LayDanhSachTheoKhuVuc(int ma)
        {
            return data.LayDanhSachTheoKhuVuc(ma);
        }

        public bool DoiTrangThai(int maBan, int trangThaiMoi)
        {
            bool kq = data.DoiTrangThai(maBan, trangThaiMoi);

            if (kq)
            {
                banDTO tontai = ds.FirstOrDefault(x => x.MaBan == maBan);
                if (tontai != null)
                {
                    tontai.DangSuDung = (byte)trangThaiMoi;
                }
            }
            return kq;
        }

        public bool KiemTraTrungTen(string tenBan)
        {
            if (ds == null || ds.Count == 0) LayDanhSach();

            banDTO tontai = ds.FirstOrDefault(x => x.TenBan.Equals(tenBan.Trim(), StringComparison.OrdinalIgnoreCase));
            if (tontai != null)
            {
                return true;
            }
            return false;
        }
        public bool KiemTraRong(string tenBan)
        {
            return string.IsNullOrWhiteSpace(tenBan);
        }

        /*public bool ThemBan(banDTO ban)
        {
            bool kq = data.ThemBan(ban);
            if (kq)
            {
                ds.Add(ban);
            }
            return kq;
        }
        public bool SuaBan(banDTO ban)
        {
            bool kq = data.SuaBan(ban);
            if (kq)
            {
                // Cập nhật lại danh sách bộ nhớ nếu cần
                var item = ds.FirstOrDefault(x => x.MaBan == ban.MaBan);
                if (item != null)
                {
                    item.TenBan = ban.TenBan;
                }
            }
            return kq;
        }

        public bool XoaBan(int maBan)
        {
            bool kq = data.XoaBan(maBan);
            if (kq)
            {
                // Xóa từ dưới lên để tránh lỗi index khi remove
                for (int i = ds.Count - 1; i >= 0; i--)
                {
                    if (ds[i].MaBan == maBan)
                    {
                        ds.RemoveAt(i);
                    }
                }
            }
            return kq;
        }*/
    }
}
