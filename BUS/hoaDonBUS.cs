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
                    if (hoaDonMoi.MaNhanVien == 0 || hoaDonMoi.MaNhanVien == null)
                        hoaDonMoi.MaNhanVien = hd.MaNhanVien;

                    if (hoaDonMoi.MaKhachHang == 0 || hoaDonMoi.MaKhachHang == null)
                        hoaDonMoi.MaKhachHang = hd.MaKhachHang;

                    if (hoaDonMoi.MaBan == 0)
                        hoaDonMoi.MaBan = hd.MaBan;
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
                    if (hoaDonMoi.MaNhanVien == 0 || hoaDonMoi.MaNhanVien == null)
                        hoaDonMoi.MaNhanVien = hd.MaNhanVien;

                    if (hoaDonMoi.MaKhachHang == 0 || hoaDonMoi.MaKhachHang == null)
                        hoaDonMoi.MaKhachHang = hd.MaKhachHang;

                    if (hoaDonMoi.MaBan == 0)
                        hoaDonMoi.MaBan = hd.MaBan;
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
        public BindingList<cthoaDonDTO> LayTatCaChiTiet()
        {
            return dao.LayTatCaChiTiet();
        }
        public DateTime LayThoiGianTaoCuaBan(int maBan)
        {
            return dao.LayThoiGianTaoCuaBan(maBan);
        }

        public bool capNhatThongTinHoaDon(hoaDonDTO hd)
        {
            bool kq = dao.capNhatThongTinHoaDon(hd);
            if (kq)
            {
                hoaDonDTO tontai = ds.FirstOrDefault(x => x.MaHD == hd.MaHD);
                if (tontai != null)
                {
                    tontai.MaBan = hd.MaBan;
                    tontai.MaTT = hd.MaTT;
                    tontai.MaKhachHang = hd.MaKhachHang;
                    tontai.MaNhanVien = hd.MaNhanVien;
                    tontai.TongTien = hd.TongTien;
                }
            }
            return kq;
        }

        public bool KiemTraBanCoHoaDonMo(int maBan, int maHDDangSua)
        {
            return dao.KiemTraBanCoHoaDonMo(maBan, maHDDangSua);
        }

        public BindingList<hoaDonDTO> timKiemCoBan(string tim, int index)
        {
            BindingList<hoaDonDTO> dskq = new BindingList<hoaDonDTO>();
            if (ds == null)
            {
                LayDanhSach();
            }
            BindingList<banDTO> dsSp = new banBUS().LayDanhSach();
            BindingList<khachHangDTO> dsNl = new khachHangBUS().LayDanhSach();
            BindingList<nhanVienDTO> dsDV = new nhanVienBUS().LayDanhSach();
            foreach (hoaDonDTO ct in ds)
            {
                switch (index)
                {
                    case 0:
                        {
                            if (ct.MaHD.ToString().Contains(tim))
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 1:
                        {
                            banDTO sp = dsSp.FirstOrDefault(x => x.MaBan == ct.MaBan);
                            string tenBan = sp != null ? sp.TenBan : "";
                            if (tenBan.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 2:
                        {
                            khachHangDTO nl = dsNl.FirstOrDefault(x => x.MaKhachHang == ct.MaKhachHang);
                            string tenNL = nl != null ? nl.TenKhachHang : "";
                            if (tenNL.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 3:
                        {
                            nhanVienDTO dv = dsDV.FirstOrDefault(x => x.MaNhanVien == ct.MaNhanVien);
                            string tendv = dv != null ? dv.HoTen : "";
                            if (tendv.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    }
            }
            return dskq;
        }

        public BindingList<hoaDonDTO> timKiemNangCao(string tenBan, string tenKH, string tenNV, decimal slMin, decimal slMax)
        {
            BindingList<hoaDonDTO> dskq = new BindingList<hoaDonDTO>();
            BindingList<banDTO> dsSP = new banBUS().LayDanhSach();
            BindingList<khachHangDTO> dsNL = new khachHangBUS().LayDanhSach();
            BindingList<nhanVienDTO> dsDV = new nhanVienBUS().LayDanhSach();

            foreach (hoaDonDTO ct in ds)
            {
                bool dk = true;

                banDTO sp = dsSP.FirstOrDefault(x => x.MaBan == ct.MaBan);
                khachHangDTO nl = dsNL.FirstOrDefault(x => x.MaKhachHang == ct.MaKhachHang);
                nhanVienDTO dv = dsDV.FirstOrDefault(x => x.MaNhanVien == ct.MaNhanVien);

                string tenBangoc = sp?.TenBan ?? "";
                string tenKHgoc = nl?.TenKhachHang ?? "";
                string tenNVgoc = dv?.HoTen ?? "";

                if (!string.IsNullOrEmpty(tenBan) && tenBangoc.IndexOf(tenBan, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }
                if (!string.IsNullOrEmpty(tenKH) && tenKHgoc.IndexOf(tenKH, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }
                if (!string.IsNullOrEmpty(tenNV) && tenNVgoc.IndexOf(tenNV, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }
                if (slMin != -1 && ct.TongTien < slMin)
                {
                    dk = false;
                }
                if (slMax != -1 && ct.TongTien > slMax)
                {
                    dk = false;
                }
                if (dk)
                {
                    dskq.Add(ct);
                }
            }
            return dskq;
        }
      
    }
}
