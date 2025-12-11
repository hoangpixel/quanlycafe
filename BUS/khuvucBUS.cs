using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class khuvucBUS
    {
        public static BindingList<khuVucDTO> ds = new BindingList<khuVucDTO>();
        private khuVucDAO data = new khuVucDAO();
        public BindingList<khuVucDTO> LayDanhSach()
        {
            if(ds == null || ds.Count == 0)
            {
                ds = data.LayDanhSach();
            }
            return ds;
        }

        public int LayMa()
        {
            return data.LayMa();
        }

        // 3. Thêm khu vực
        public bool ThemKhuVuc(khuVucDTO kv)
        {
            bool kq = data.ThemKhuVuc(kv);
            if (kq)
            {
                ds.Add(kv);
            }
            return kq;
        }

        // 4. Sửa khu vực
        public bool SuaKhuVuc(khuVucDTO kv)
        {
            bool kq = data.SuaKhuVuc(kv);
            if (kq)
            {
                khuVucDTO tontai = ds.FirstOrDefault(x => x.MaKhuVuc == kv.MaKhuVuc);
                if (tontai != null)
                {
                    tontai.TenKhuVuc = kv.TenKhuVuc;
                }
            }
            return kq;
        }

        // 5. Xóa khu vực
        public bool XoaKhuVuc(int maKV)
        {
            bool kq = data.XoaKhuVuc(maKV);
            if (kq)
            {
                khuVucDTO tontai = ds.FirstOrDefault(x => x.MaKhuVuc == maKV);
                if (tontai != null)
                {
                    ds.Remove(tontai);
                }
            }
            return kq;
        }

        // 6. Kiểm tra trùng tên
        public bool KiemTraTrungTen(string tenKV)
        {
            if (ds == null || ds.Count == 0) LayDanhSach();

            khuVucDTO tontai = ds.FirstOrDefault(x => x.TenKhuVuc.Equals(tenKV.Trim(), StringComparison.OrdinalIgnoreCase));
            if (tontai != null)
            {
                return true;
            }
            return false;
        }

        // 7. Kiểm tra rỗng
        public bool KiemTraRong(string tenKV)
        {
            return string.IsNullOrWhiteSpace(tenKV);
        }
        public bool ThemKV(khuVucDTO kv)
        {
            int kq = data.ThemKV(kv);
            if (kq>0)
            {
                ds.Add(kv);
                return true;
            }
            return false;
        }
        public bool SuaKV(khuVucDTO kv)
        {
            bool kq = data.SuaKV(kv);
            if (kq)
            {
                // Cập nhật lại danh sách bộ nhớ nếu cần
                var item = ds.FirstOrDefault(x => x.MaKhuVuc == kv.MaKhuVuc);
                if (item != null)
                {
                    item.TenKhuVuc = kv.TenKhuVuc;
                }
            }
            return kq;
        }

        public bool XoaKV(int maKV)
        {
            bool kq = data.XoaKV(maKV);
            if (kq)
            {
                // Xóa từ dưới lên để tránh lỗi index khi remove
                for (int i = ds.Count - 1; i >= 0; i--)
                {
                    if (ds[i].MaKhuVuc == maKV)
                    {
                        ds.RemoveAt(i);
                    }
                }
            }
            return kq;
        }
    }
}
