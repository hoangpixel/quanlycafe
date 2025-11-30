using DAO;
using DTO;
using System.ComponentModel;
using System.Linq;

namespace BUS
{
    public class quyenBUS
    {
        private quyenDAO data = new quyenDAO();
        // Danh sách tĩnh để caching dữ liệu Quyền trên BUS
        public static BindingList<quyenDTO> ds = new BindingList<quyenDTO>();

        // Lấy danh sách Quyền (dùng cache)
        public BindingList<quyenDTO> LayDanhSach()
        {
            if (ds == null || ds.Count <= 0)
            {
                ds = data.LayDanhSachQuyen();
            }
            return ds;
        }

        // *Bạn có thể thêm hàm Them, Sua, Xoa cho Quyen tương tự như VaiTro nếu cần quản lý Quyen*
        // ...
    }
}