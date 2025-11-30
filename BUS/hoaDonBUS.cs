using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

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

        public bool KhoaHoaDon(int maHD) => dao.KhoaHoaDon(maHD);

        public hoaDonDTO TimTheoMa(int maHD) => dao.TimTheoMa(maHD);


        public bool ChuyenTrangThai(int maHD, bool trangThai)
        {
            return dao.UpdateTrangThai(maHD, trangThai);
        }


    }

}
