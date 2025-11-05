using DAO;
using DTO;
using System.Collections.Generic;
using System.ComponentModel;

namespace BUS
{
    public class hoaDonBUS
    {
        private hoaDonDAO dao = new hoaDonDAO();

        public BindingList<hoaDonDTO> LayDanhSach() => dao.LayDanhSach();

        public int ThemHoaDon(hoaDonDTO hd, List<gioHangItemDTO> gioHang)
            => dao.Them(hd, gioHang);

        public bool CapNhatTrangThai(int maHD, string trangThai)
            => dao.CapNhatTrangThai(maHD, trangThai);

        public bool Xoa(int maHD) => dao.Xoa(maHD);

        public hoaDonDTO TimTheoMa(int maHD) => dao.TimTheoMa(maHD);
    }
}