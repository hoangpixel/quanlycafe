using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
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

        public int LayMa()
        {
            return dao.layMa();
        }
        public bool UpdateKhoaSo(int maBan)
        {
            bool kq = dao.UpdateKhoaSo(maBan);
            foreach(hoaDonDTO hd in ds)
            {
                if(hd.MaBan == maBan)
                {
                    hd.KhoaSo = 1;
                }
            }
            return kq;
        }
        public int ThemHoaDon(hoaDonDTO hd, BindingList<cthoaDonDTO> dsChiTiet)
        {
            int newID = dao.Them(hd, dsChiTiet);

            if (newID > 0)
            {
                hoaDonDTO hoaDonMoi = dao.LayThongTinHoaDon(newID);

                if (hoaDonMoi != null)
                {
                    ds.Insert(0, hoaDonMoi);
                }
            }

            return newID;
        }
        public int SuaHoaDon(hoaDonDTO hd, BindingList<cthoaDonDTO> dsChiTiet)
        {
            int newID = dao.SuaHD(hd, dsChiTiet);

            if (newID > 0)
            {
                hoaDonDTO hoaDonMoi = dao.LayThongTinHoaDon(newID);

                if (hoaDonMoi != null)
                {
                    ds.Insert(0, hoaDonMoi);
                }
            }

            return newID;
        }
        public bool CapNhatTrangThai(int maHD, string trangThai)
            => dao.CapNhatTrangThai(maHD, trangThai);

        public bool KhoaHoaDon(int maHD) => dao.KhoaHoaDon(maHD);

        public hoaDonDTO TimTheoMa(int maHD) => dao.TimTheoMa(maHD);


        public bool XoaHoaDon(int maHD)
        {
            bool result = dao.UpdateTrangThai(maHD);

            if (result)
            {
                var item = ds.FirstOrDefault(x => x.MaHD == maHD);
                if (item != null)
                {
                    ds.Remove(item);
                }
            }

            return result;
        }

        public bool doiTrangThaiBanSauKhiXoaHD(int maBan)
        {
            return dao.doiTrangThaiBanSauKhiXoaHD(maBan);
        }
        public BindingList<cthoaDonDTO> LayChiTiet(int maHD)
        {
            return dao.LayChiTietHoaDon(maHD);
        }

        public DateTime LayThoiGianTaoCuaBan(int maBan)
        {
            return dao.LayThoiGianTaoCuaBan(maBan);
        }
    }
}
