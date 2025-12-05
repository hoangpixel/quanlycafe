using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace BUS
{
    public class nguyenLieuBUS
    {
        public static BindingList<nguyenLieuDTO> ds = new BindingList<nguyenLieuDTO>();
        private nguyenLieuDAO data = new nguyenLieuDAO();

        public BindingList<nguyenLieuDTO> LayDanhSach(bool forceReload = false)
        {
            if(ds == null || ds.Count == 0 || forceReload)
            {
                ds = data.docDanhSachNguyenLieu();
            }
            return ds;
        }

        public int layMa()
        {
            return data.layMa();
        }

        public bool themNguyenLieu(nguyenLieuDTO nl)
        {
            bool kq = data.Them(nl);
            if (kq)
            {
                ds.Add(nl);
            }
            return kq;
        }

        public bool suaNguyenLieu(nguyenLieuDTO nl)
        {
            bool result = data.Sua(nl);

            if (result)
            {
                nguyenLieuDTO tontai = ds.FirstOrDefault(x => x.MaNguyenLieu == nl.MaNguyenLieu);
                if (tontai != null)
                {
                    tontai.TenNguyenLieu = nl.TenNguyenLieu;
                    tontai.MaDonViCoSo = nl.MaDonViCoSo;
                    tontai.TonKho = nl.TonKho;
                    tontai.TrangThai = nl.TrangThai;
                }
                Console.WriteLine("Sửa nguyên liệu thành công!");
            }
            else
            {
                Console.WriteLine("Lỗi khi sửa nguyên liệu!");
            }

            return result;
        }

        public bool xoaNguyenLieu(int maNguyenLieu)
        {
            bool result = data.Xoa(maNguyenLieu);

            if (result)
            {
                nguyenLieuDTO ct = ds.FirstOrDefault(x => x.MaNguyenLieu == maNguyenLieu);
                if(ct != null)
                {
                    ds.Remove(ct);
                }
            }
            return result;
        }


        public nguyenLieuDTO TimTheoMa(int ma)
        {
            return data.TimTheoMa(ma);
        }

        public void XoaTatCa()
        {
            var ds = data.docDanhSachNguyenLieu();
            foreach (var nl in ds)
            {
                data.Xoa(nl.MaNguyenLieu);
            }
        }

        private bool LaNguyenLieuGiongNhau(nguyenLieuDTO a, nguyenLieuDTO b)
        {
            return a.TenNguyenLieu == b.TenNguyenLieu &&
                   a.MaDonViCoSo == b.MaDonViCoSo &&
                   a.TonKho == b.TonKho;
        }

        public void NhapExcelThongMinh(BindingList<nguyenLieuDTO> dsExcel)
        {
            int soThem = 0, soCapNhat = 0, soBoQua = 0, soLoi = 0, soTrungTen = 0;

            var dsHienTai = data.docDanhSachNguyenLieu();

            foreach (var nlMoi in dsExcel)
            {
                try
                {
                    if (nlMoi.TrangThai == 0)
                        nlMoi.TrangThai = 1;

                    bool tenTrung = dsHienTai.Any(n =>
                        string.Equals(n.TenNguyenLieu.Trim(), nlMoi.TenNguyenLieu.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (tenTrung)
                    {
                        Console.WriteLine($"⚠️ Nguyên liệu '{nlMoi.TenNguyenLieu}' đã tồn tại → bỏ qua!");
                        soTrungTen++;
                        continue;
                    }

                    var nlCu = data.TimTheoMa(nlMoi.MaNguyenLieu);

                    if (nlCu == null)
                    {
                        data.Them(nlMoi);
                        dsHienTai.Add(nlMoi);
                        soThem++;
                    }
                    else if (!LaNguyenLieuGiongNhau(nlCu, nlMoi))
                    {
                        data.Sua(nlMoi);
                        soCapNhat++;
                    }
                    else
                    {
                        soBoQua++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Lỗi khi xử lý nguyên liệu Excel: " + ex.Message);
                    soLoi++;
                }
            }
        }

        public BindingList<nguyenLieuDTO> timKiemCoBanNL(string tim,int index)
        {
            BindingList<nguyenLieuDTO> kq = new BindingList<nguyenLieuDTO>();
            if(ds == null)
            {
                LayDanhSach();
            }
            foreach (nguyenLieuDTO ct in ds)
            {
                switch(index)
                {
                    case 0:
                        {
                            if(ct.MaNguyenLieu.ToString().Contains(tim))
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                    case 1:
                        {
                            if(ct.TenNguyenLieu.IndexOf(tim,StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                    case 2:
                        {

                            donViBUS bus = new donViBUS();
                            BindingList<donViDTO> dsdv = bus.LayDanhSach();
                            donViDTO donVi = dsdv.FirstOrDefault(x => x.MaDonVi == ct.MaDonViCoSo);
                            string tenDV = donVi != null ? donVi.TenDonVi : "";
                            if (tenDV.IndexOf(tim,StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                    case 3:
                        {
                            decimal tonKhoMin = decimal.Parse(tim.ToString());
                            if(ct.TonKho >= tonKhoMin)
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                    case 4:
                        {
                            decimal tonKhoMax = decimal.Parse(tim.ToString());
                            if(ct.TonKho <= tonKhoMax)
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                }
            }
            return kq;
        }

        public bool kiemTraChuoiRong(string item)
        {
            if(string.IsNullOrWhiteSpace(item))
            {
                return true;
            }
            return false;
        }

        public bool kiemTraTrungTen(string item)
        {
            nguyenLieuDTO tim = ds.FirstOrDefault(x => x.TenNguyenLieu.ToLower().Equals(item.ToLower()));
            if (tim != null)
            {
                return false;
            }
            return true;
        }

        public BindingList<nguyenLieuDTO> timKiemNangCao(int trangThaiNL,string tenNL,string tenDV, decimal tonKhoMin, decimal tonKhoMax)
        {
            BindingList<nguyenLieuDTO> dskq = new BindingList<nguyenLieuDTO>();
            BindingList<donViDTO> dsDV = new donViBUS().LayDanhSach();

            foreach(nguyenLieuDTO ct in ds)
            {
                bool dk = true;
                donViDTO dv = dsDV.FirstOrDefault(x => x.MaDonVi == ct.MaDonViCoSo);
                string tenDVtim = dv?.TenDonVi ?? "";
                if(trangThaiNL != -1 && ct.TrangThaiDV != trangThaiNL)
                {
                    dk = false;
                }
                if(!string.IsNullOrEmpty(tenNL) && ct.TenNguyenLieu.IndexOf(tenNL,StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }
                if(!string.IsNullOrEmpty(tenDV) && tenDV.IndexOf(tenDVtim, StringComparison.OrdinalIgnoreCase) <0 )
                {
                    dk = false;
                }
                if(tonKhoMin != -1 && ct.TonKho < tonKhoMin)
                {
                    dk = false;
                }
                if(tonKhoMax != -1 && ct.TonKho > tonKhoMax)
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
