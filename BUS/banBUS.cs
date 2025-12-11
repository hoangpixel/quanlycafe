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
        public bool DoiTrangThai(int maBan)
        {
            bool kq = data.DoiTrangThai(maBan);
            if (kq)
            {
                // Cập nhật lại cache để giao diện đổi màu bàn ngay lập tức
                banDTO tontai = ds.FirstOrDefault(x => x.MaBan == maBan);
                if (tontai != null)
                {
                    // Giả sử logic của DAO là reset về 1 (Trống)
                    // Nếu logic DAO là toggle (0->1, 1->0) thì bạn cần chỉnh lại dòng này
                    tontai.DangSuDung = 1;
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
    }
}
