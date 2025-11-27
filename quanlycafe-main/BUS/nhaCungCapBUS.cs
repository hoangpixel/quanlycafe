using DAO;
using DTO;
using System.ComponentModel;
using System.Linq;

namespace BUS
{
    public class nhaCungCapBUS
    {
        private nhaCungCapDAO dao = new nhaCungCapDAO();

        public BindingList<nhaCungCapDTO> LayDanhSach() => dao.LayDanhSach();
        public bool KiemTraTrungMa(int ma) => dao.KiemTraTrungMa(ma);
        public bool Them(nhaCungCapDTO ncc) => dao.Them(ncc);
        public bool Sua(nhaCungCapDTO ncc) => dao.Sua(ncc);
        public bool Xoa(int ma) => dao.Xoa(ma);

        public BindingList<nhaCungCapDTO> TimKiemNangCao(string kw, string type, int status)
        {
            var list = dao.LayDanhSach();
            var query = list.AsEnumerable();

            // 1. Lọc theo từ khóa và tiêu chí
            if (!string.IsNullOrEmpty(kw))
            {
                kw = kw.ToLower();
                if (type == "Mã NCC")
                    query = query.Where(x => x.MaNCC.ToString().Contains(kw));
                else if (type == "Tên NCC")
                    query = query.Where(x => x.TenNCC.ToLower().Contains(kw));
                else if (type == "Địa chỉ")
                    query = query.Where(x => x.DiaChi.ToLower().Contains(kw));
                else if (type == "SĐT")
                    query = query.Where(x => x.SoDienThoai.Contains(kw));
                else
                {
                    query = query.Where(x => x.TenNCC.ToLower().Contains(kw)
                                          || x.MaNCC.ToString().Contains(kw)
                                          || x.DiaChi.ToLower().Contains(kw)
                                          || x.SoDienThoai.Contains(kw));
                }
            }

            // 2. Lọc theo trạng thái (status = -1 là lấy tất cả)
            if (status != -1)
                query = query.Where(x => x.ConHoatDong == status);

            return new BindingList<nhaCungCapDTO>(query.ToList());
        }
    }
}