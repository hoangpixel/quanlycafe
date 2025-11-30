using DAO;
using DTO;
using System.ComponentModel;
using System.Linq;

namespace BUS
{
    public class vaitroBUS
    {
        private vaitroDAO data = new vaitroDAO();
        // Danh sách tĩnh để caching dữ liệu Vai trò trên BUS
        public static BindingList<vaitroDTO> ds = new BindingList<vaitroDTO>();

        // Lấy danh sách Vai trò (dùng cache nếu đã có)
        public BindingList<vaitroDTO> LayDanhSach()
        {
            if (ds == null || ds.Count <= 0)
            {
                ds = data.LayDanhSachVaiTro();
            }
            return ds;
        }

        // Thêm Vai trò mới (Thực hiện bước 1 của chức năng Insert/Update)
        public bool them(vaitroDTO vt)
        {
            // Logic kiểm tra dữ liệu đầu vào (nếu cần)

            bool kq = data.ThemVaiTro(vt);
            if (kq)
            {
                // Sau khi thêm thành công, cần reload lại danh sách để lấy MA mới
                // *LƯU Ý:* Hàm DAO ThemVaiTro hiện tại không trả về MaVaiTro mới
                // Nếu muốn tối ưu, hàm DAO cần trả về MA, hoặc bạn cần clear ds và gọi LayDanhSach()
                ds = data.LayDanhSachVaiTro();
            }
            return kq;
        }

        // Sửa Vai trò
        //public bool sua(vaitroDTO vt)
        //{
        //    bool kq = data.SuaVaiTro(vt);
        //    if (kq)
        //    {
        //        // Cập nhật vào danh sách cache (BindingList)
        //        vaitroDTO tontai = ds.FirstOrDefault(x => x.MaVaiTro == vt.MaVaiTro);
        //        if (tontai != null)
        //        {
        //            tontai.TenVaiTro = vt.TenVaiTro;
        //        }
        //    }
        //    return kq;
        //}

        //// Xóa Vai trò (Xóa logic: đổi TrangThai = 0)
        //public bool xoa(int maXoa)
        //{
        //    bool kq = data.XoaVaiTro(maXoa);
        //    if (kq)
        //    {
        //        // Xóa khỏi danh sách cache
        //        vaitroDTO tontai = ds.FirstOrDefault(x => x.MaVaiTro == maXoa);
        //        if (tontai != null)
        //        {
        //            ds.Remove(tontai);
        //        }
        //    }
        //    return kq;
        //}
    }
}