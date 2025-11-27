using DAO;
using DTO;
using System.ComponentModel;
using System.Linq;

namespace BUS
{
    public class khachHangBUS
    {
        private khachHangDAO dao = new khachHangDAO();

        public BindingList<khachHangDTO> LayDanhSach() => dao.LayDanhSach();
        public bool Them(khachHangDTO kh) => dao.Them(kh);
        public bool Sua(khachHangDTO kh) => dao.Sua(kh);
        public bool Xoa(int ma) => dao.Xoa(ma);

        public bool KiemTraTrungSDT(string sdt, int maLoaiTru = -1) => dao.KiemTraTrungSDT(sdt, maLoaiTru);
        public bool KiemTraTrungEmail(string email, int maLoaiTru = -1) => dao.KiemTraTrungEmail(email, maLoaiTru);

        public BindingList<khachHangDTO> TimKiemNangCao(string kw, string type, int status)
        {
            var list = dao.LayDanhSach();
            var query = list.AsEnumerable();

            if (!string.IsNullOrEmpty(kw))
            {
                kw = kw.ToLower();
                if (type == "Mã KH") query = query.Where(x => x.MaKhachHang.ToString().Contains(kw));
                else if (type == "Tên KH") query = query.Where(x => x.TenKhachHang.ToLower().Contains(kw));
                else if (type == "SĐT") query = query.Where(x => x.SoDienThoai.Contains(kw));
                else if (type == "Email") query = query.Where(x => x.Email.ToLower().Contains(kw));
                else
                {
                    query = query.Where(x => x.TenKhachHang.ToLower().Contains(kw)
                                          || x.MaKhachHang.ToString().Contains(kw)
                                          || x.SoDienThoai.Contains(kw)
                                          || x.Email.ToLower().Contains(kw));
                }
            }

            // status = -1 là lấy tất cả
            if (status != -1)
                query = query.Where(x => x.TrangThai == status);

            return new BindingList<khachHangDTO>(query.ToList());
        }
    }
}