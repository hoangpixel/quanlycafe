using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Security.Policy;

namespace BUS
{
    public class congThucBUS
    {
        public static BindingList<congThucDTO> ds = new BindingList<congThucDTO>();

        public BindingList<congThucDTO> docDSCongThucTheoSP(int maSP)
        {
            congThucDAO data = new congThucDAO();
            ds = data.docDanhSachCongThucTheoSP(maSP);
            return ds;
        }

        public BindingList<congThucDTO> LayDanhSach()
        {
            congThucDAO data = new congThucDAO();
            
                ds = data.layDanhSach();
            
            return ds;
        }

        public bool themCongThuc(congThucDTO ct)
        {
            ct.TrangThai = 1;
            congThucDAO data = new congThucDAO();
            bool kq = data.ThemHoacCapNhat(ct);

            if (kq)
            {
                ds.Add(ct);
                sanPhamDAO spDAO = new sanPhamDAO();
                spDAO.CapNhatTrangThaiCT(ct.MaSanPham, 1);
            }

            return kq;
        }

        public bool suaCongThuc(congThucDTO ct)
        {
            congThucDAO data = new congThucDAO();
            bool result = data.Sua(ct);
            if (result)
            {
                congThucDTO tontai = ds.FirstOrDefault(x => x.MaSanPham == ct.MaSanPham && x.MaNguyenLieu == ct.MaNguyenLieu);
                if (tontai != null)
                {
                    tontai.SoLuongCoSo = ct.SoLuongCoSo;
                    tontai.MaDonViCoSo = ct.MaDonViCoSo;
                    tontai.TrangThai = ct.TrangThai;
                }
            }
            return result;
        }


        public bool xoaCongThuc(int maSP, int maNL)
        {
            congThucDAO data = new congThucDAO();
            bool result = data.Xoa(maSP, maNL);
            if (result)
            {
                congThucDTO cth = ds.FirstOrDefault(x => x.MaSanPham == maSP && x.MaNguyenLieu == maNL);
                if (cth != null)
                {
                    ds.Remove(cth);
                }
            }
            return result;
        }

        public void NhapExcelThongMinh(BindingList<congThucDTO> dsExcel)
        {
            int soThem = 0, soCapNhat = 0, soBoQua = 0, soLoi = 0;
            congThucDAO data = new congThucDAO();

            var dsHienTai = data.layDanhSach();

            foreach (var ctMoi in dsExcel)
            {
                try
                {
                    if (ctMoi.MaSanPham == 0 || ctMoi.MaNguyenLieu == 0)
                    {
                        soLoi++;
                        continue;
                    }

                    ctMoi.TrangThai = 1;

                    var ctCu = dsHienTai.FirstOrDefault(x =>
                        x.MaSanPham == ctMoi.MaSanPham &&
                        x.MaNguyenLieu == ctMoi.MaNguyenLieu);

                    if (ctCu == null)
                    {
                        data.Them(ctMoi);
                        soThem++;
                    }
                    else if (Math.Abs(ctCu.SoLuongCoSo - ctMoi.SoLuongCoSo) > 0.0001m ||
                             ctCu.MaDonViCoSo != ctMoi.MaDonViCoSo)
                    {
                        data.Sua(ctMoi);
                        soCapNhat++;
                    }
                    else soBoQua++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi xử lý công thức Excel: " + ex.Message);
                    soLoi++;
                }
            }
        }

        public bool kiemTraChuoiRong(string item)
        {
            if(string.IsNullOrWhiteSpace(item))
            {
                return true;
            }
            return false;
        }

        public BindingList<congThucDTO> timKiemCoBan(string tim, int index)
        {
            BindingList<congThucDTO> dskq = new BindingList<congThucDTO>();
            if(ds == null)
            {
                LayDanhSach();
            }
            BindingList<sanPhamDTO> dsSp = new sanPhamBUS().LayDanhSach();
            BindingList<nguyenLieuDTO> dsNl = new nguyenLieuBUS().LayDanhSach();
            BindingList<donViDTO> dsDV = new donViBUS().LayDanhSach();
            foreach(congThucDTO ct in ds)
            {
                switch(index)
                {
                    case 0:
                        {
                            if(ct.MaSanPham.ToString().Contains(tim))
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 1:
                        {
                            sanPhamDTO sp = dsSp.FirstOrDefault(x => x.MaSP == ct.MaSanPham);
                            string tenSp = sp != null ? sp.TenSP : "";
                            if(tenSp.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 2:
                        {
                            if(ct.MaNguyenLieu.ToString().Contains(tim))
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 3:
                        {
                            nguyenLieuDTO nl = dsNl.FirstOrDefault(x => x.MaNguyenLieu == ct.MaNguyenLieu);
                            string tenNL = nl != null ? nl.TenNguyenLieu : "";
                            if (tenNL.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 4:
                        {
                            donViDTO dv = dsDV.FirstOrDefault(x => x.MaDonVi == ct.MaDonViCoSo);
                            string tendv = dv != null ? dv.TenDonVi : "";
                            if(tendv.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 5:
                        {
                            decimal soLuong = decimal.Parse(tim);
                            if(ct.SoLuongCoSo <= soLuong)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 6:
                        {
                            decimal soLuong = decimal.Parse(tim);
                            if(ct.SoLuongCoSo >= soLuong)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                }
            }
            return dskq;
        }

        public BindingList<congThucDTO> timKiemNangCao(string tenSP,string tenNL,string tenDV, decimal soLuongMin, decimal soLuongMax)
        {
            BindingList<congThucDTO> dskq = new BindingList<congThucDTO>();
            BindingList<sanPhamDTO> dsSP = new sanPhamBUS().LayDanhSach();
            BindingList<nguyenLieuDTO> dsNL = new nguyenLieuBUS().LayDanhSach();
            BindingList<donViDTO> dsDV = new donViBUS().LayDanhSach();

            foreach(congThucDTO ct in ds)
            {
                bool dk = true;

                sanPhamDTO sp = dsSP.FirstOrDefault(x => x.MaSP == ct.MaSanPham);
                nguyenLieuDTO nl = dsNL.FirstOrDefault(x => x.MaNguyenLieu == ct.MaNguyenLieu);
                donViDTO dv = dsDV.FirstOrDefault(x => x.MaDonVi == ct.MaDonViCoSo);

                string tenSPgoc = sp?.TenSP ?? "";
                string tenNLgoc = nl?.TenNguyenLieu ?? "";
                string tenDVgoc = dv?.TenDonVi ?? "";

                if(!string.IsNullOrEmpty(tenSP) && tenSPgoc.IndexOf(tenSP,StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }
                if(!string.IsNullOrEmpty(tenNL) && tenNLgoc.IndexOf(tenNL, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }
                if(!string.IsNullOrEmpty(tenDV) && tenDVgoc.IndexOf(tenDV, StringComparison.OrdinalIgnoreCase) <0 )
                {
                    dk = false;
                }
                if(soLuongMin != -1 && ct.SoLuongCoSo < soLuongMin)
                {
                    dk = false;
                }
                if(soLuongMax != -1 && ct.SoLuongCoSo > soLuongMax)
                {
                    dk = false;
                }
                if(dk)
                {
                    dskq.Add(ct);
                }
            }
            return dskq;
        }
    }
}