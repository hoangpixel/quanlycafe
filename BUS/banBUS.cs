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

        // 3. Thêm bàn mới
        public bool ThemBan(banDTO ban)
        {
            bool kq = data.ThemBan(ban);
            if (kq)
            {
                // Thêm thành công vào DB thì thêm luôn vào list cache để hiện lên GridView ngay
                ds.Add(ban);
            }
            return kq;
        }

        // 4. Sửa thông tin bàn
        public bool SuaBan(banDTO ban)
        {
            bool kq = data.SuaBan(ban);
            if (kq)
            {
                // Sửa thành công trong DB thì tìm thằng đó trong list cache để sửa thông tin
                banDTO tontai = ds.FirstOrDefault(x => x.MaBan == ban.MaBan);
                if (tontai != null)
                {
                    tontai.TenBan = ban.TenBan;
                    tontai.MaKhuVuc = ban.MaKhuVuc;
                    // Lưu ý: Không cập nhật trạng thái ở đây vì hàm Sửa thường chỉ sửa Tên/Khu vực
                }
            }
            return kq;
        }

        // 5. Xóa bàn
        public bool XoaBan(int maBan)
        {
            bool kq = data.XoaBan(maBan);
            if (kq)
            {
                // Xóa thành công trong DB thì xóa luôn khỏi list cache
                banDTO tontai = ds.FirstOrDefault(x => x.MaBan == maBan);
                if (tontai != null)
                {
                    ds.Remove(tontai);
                }
            }
            return kq;
        }

        // 6. Đổi trạng thái bàn (Dùng khi thanh toán/gộp bàn)
        // Trong class banBUS
        public BindingList<banDTO> LayDanhSachTheoKhuVuc(int ma)
        {
            return data.LayDanhSachTheoKhuVuc(ma); // luôn truy vấn DB
        }

        public bool DoiTrangThai(int maBan, int trangThaiMoi) // Thêm tham số ở đây
        {
            // Gọi DAO với tham số mới
            bool kq = data.DoiTrangThai(maBan, trangThaiMoi);

            if (kq)
            {
                // Cập nhật lại cache để giao diện đổi màu ngay lập tức
                banDTO tontai = ds.FirstOrDefault(x => x.MaBan == maBan);
                if (tontai != null)
                {
                    // Quan trọng: Gán theo đúng cái mình vừa truyền vào
                    // (Không gán cứng số 1 nữa)
                    tontai.DangSuDung = (byte)trangThaiMoi;
                }
            }
            return kq;
        }

        // 7. Kiểm tra trùng tên bàn
        public bool KiemTraTrungTen(string tenBan)
        {
            // Đảm bảo list đã có dữ liệu để kiểm tra
            if (ds == null || ds.Count == 0) LayDanhSach();

            banDTO tontai = ds.FirstOrDefault(x => x.TenBan.Equals(tenBan.Trim(), StringComparison.OrdinalIgnoreCase));
            if (tontai != null)
            {
                return true;
            }
            return false;
        }

        // 8. Kiểm tra chuỗi rỗng
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
