using DAO;
using DTO;
using System.Collections.Generic;
using System.ComponentModel;

namespace BUS
{
    public class hoaDonBUS
    {
        private hoaDonDAO dao = new hoaDonDAO();
        public static BindingList<hoaDonDTO> ds = new BindingList<hoaDonDTO>();

        public BindingList<hoaDonDTO> LayDanhSach()
        {
            ds = dao.LayDanhSach();
            return ds;
        }

        // Dùng luôn cthoaDonDTO
        public int ThemHoaDon(hoaDonDTO hd, BindingList<cthoaDonDTO> dsChiTiet)
            => dao.Them(hd, dsChiTiet);

        public bool CapNhatTrangThai(int maHD, string trangThai)
            => dao.CapNhatTrangThai(maHD, trangThai);

        public bool Xoa(int maHD) => dao.Xoa(maHD);

        public hoaDonDTO TimTheoMa(int maHD) => dao.TimTheoMa(maHD);
    }
}
