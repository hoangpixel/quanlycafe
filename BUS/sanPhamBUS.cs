using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BUS
{
    public class sanPhamBUS
    {
        public static BindingList<sanPhamDTO> ds = new BindingList<sanPhamDTO>();
        private sanPhamDAO data = new sanPhamDAO();
        public BindingList<sanPhamDTO> LayDanhSach()
        {
            if (ds == null || ds.Count == 0)
            {
                ds = data.DocDanhSachSanPham();
            }
            return ds;
        }

        public int layMaSP()
        {
            return data.layMa();
        }

        public bool them(sanPhamDTO ct)
        {
            bool kq = data.Them(ct);
            if(kq)
            {
                ds.Add(ct);
            }
            return kq;
        }


        public bool Sua(sanPhamDTO sp)
        {
            bool result = data.Sua(sp);

            if (result)
            {
                sanPhamDTO tontai = ds.FirstOrDefault(x => x.MaSP == sp.MaSP);
                if (tontai != null)
                {
                    tontai.MaLoai = sp.MaLoai;
                    tontai.TenSP = sp.TenSP;
                    tontai.Hinh = sp.Hinh;
                    tontai.TrangThai = sp.TrangThai;
                    tontai.TrangThaiCT = sp.TrangThaiCT;
                    tontai.Gia = sp.Gia;
                }
            }
            return result;
        }

        public bool Xoa(int maSP)
        {
            bool result = data.Xoa(maSP);

            if (result)
            {
                sanPhamDTO sp = ds.FirstOrDefault(x => x.MaSP == maSP);
                if(sp != null)
                {
                    ds.Remove(sp);
                }
            }
            else
            {
                Console.WriteLine("BUS: Lỗi khi xóa sản phẩm!");
            }

            return result;
        }

        public bool capNhatTrangThaiCT(int maSP, int trangThaiCT)
        {
            bool result = data.CapNhatTrangThaiCT(maSP, trangThaiCT);

            if (result)
            {
                var sp = ds.FirstOrDefault(x => x.MaSP == maSP);
                if (sp != null)
                    sp.TrangThaiCT = trangThaiCT;

                Console.WriteLine($"Cập nhật trạng thái CT của sản phẩm {maSP} = {trangThaiCT}");
            }
            else
            {
                Console.WriteLine($"Lỗi cập nhật trạng thái CT cho sản phẩm {maSP}");
            }

            return result;
        }

        public bool LaSanPhamGiongNhau(sanPhamDTO a, sanPhamDTO b)
        {
            return a.MaLoai == b.MaLoai
                && a.TenSP.Trim() == b.TenSP.Trim()
                && Math.Abs(a.Gia - b.Gia) < 0.001f
                && (a.Hinh ?? "") == (b.Hinh ?? "");
        }
        public void NhapExcelThongMinh(BindingList<sanPhamDTO> dsExcel)
        {
            loaiSanPhamDAO loaiDAO = new loaiSanPhamDAO();

            var dsLoaiTonTai = loaiDAO.LayDanhSach().Select(l => l.MaLoai).ToList();

            HashSet<int> maSPDaGap = new HashSet<int>();
            BindingList<string> danhSachLoi = new BindingList<string>();

            foreach (var sp in dsExcel)
            {
                if (!maSPDaGap.Add(sp.MaSP))
                {
                    danhSachLoi.Add($"Mã SP {sp.MaSP} bị trùng trong file Excel (SP: {sp.TenSP}).");
                }

                if (!dsLoaiTonTai.Contains(sp.MaLoai))
                {
                    danhSachLoi.Add($"Mã loại {sp.MaLoai} của sản phẩm '{sp.TenSP}' không tồn tại trong DB.");
                }
            }
            if (danhSachLoi.Count > 0)
            {
                string loiHienThi = string.Join("\n• ", danhSachLoi);
                return;
            }
            int soThem = 0, soCapNhat = 0, soBoQua = 0;

            foreach (var spMoi in dsExcel)
            {
                var spCu = data.TimTheoMa(spMoi.MaSP);

                if (spCu == null)
                {
                    them(spMoi);
                    soThem++;
                }
                else if (!LaSanPhamGiongNhau(spCu, spMoi))
                {
                    Sua(spMoi);
                    soCapNhat++;
                }
                else
                {
                    soBoQua++;
                }
            }
        }
        public BindingList<sanPhamDTO> timKiemCoBan(string tim, int index)
        {
            BindingList<sanPhamDTO> kq = new BindingList<sanPhamDTO>();

            if (ds == null)
                LayDanhSach();

            loaiSanPhamBUS busLoai = new loaiSanPhamBUS();
            BindingList<loaiDTO> dsLoai = busLoai.LayDanhSach();

            nhomBUS busNhom = new nhomBUS();
            BindingList<nhomDTO> dsNhom = busNhom.layDanhSach();

            foreach (sanPhamDTO ct in ds)
            {
                switch (index)
                {
                    case 0:
                        {
                            if (ct.MaSP.ToString().Contains(tim))
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                    case 1:
                        {
                            if (ct.TenSP.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                kq.Add(ct);
                            }                            
                            break;
                        }
                    case 2:
                        {
                            loaiDTO loai = dsLoai.FirstOrDefault(x => x.MaLoai == ct.MaLoai);
                            string tenLoai = loai != null ? loai.TenLoai : "";
                            if (tenLoai.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                    case 3:
                        {
                            loaiDTO loai = dsLoai.FirstOrDefault(l => l.MaLoai == ct.MaLoai);
                            string tenNhom = dsNhom.FirstOrDefault(n => n.MaNhom == (loai != null ? loai.MaNhom : -1))?.TenNhom ?? "";
                            if(tenNhom.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                kq.Add(ct);
                            }
                            break;
                        }
                    case 4:
                        {
                            if (float.TryParse(tim, out float giaMin))
                            {
                                if (ct.Gia >= giaMin)
                                {
                                    kq.Add(ct);
                                }
                            }
                            break;
                        }
                    case 5:
                        {
                            if (float.TryParse(tim, out float giaMax))
                            {
                                if (ct.Gia <= giaMax)
                                {
                                    kq.Add(ct);
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }

            return kq;
        }

        public BindingList<sanPhamDTO> timKiemNangCaoSP(int maLoai, int trangThaiCT, float giaMin, float giaMax, string tenSP)
        {
            BindingList<sanPhamDTO> dskq = new BindingList<sanPhamDTO>();
            if (ds == null) LayDanhSach();

            foreach (sanPhamDTO ct in ds)
            {
                bool dk = true;

                if (maLoai != -1 && ct.MaLoai != maLoai) dk = false;
                if (trangThaiCT != -1 && ct.TrangThaiCT != trangThaiCT) dk = false;
                if (giaMin != -1 && ct.Gia < giaMin) dk = false;
                if (giaMax != -1 && ct.Gia > giaMax) dk = false;
                if (!string.IsNullOrEmpty(tenSP) &&
                    ct.TenSP.IndexOf(tenSP, StringComparison.OrdinalIgnoreCase) < 0) dk = false;

                if (dk) dskq.Add(ct);
            }

            return dskq;
        }

        public bool kiemTraChuoiRong(string item)
        {
            if(string.IsNullOrWhiteSpace(item))
            {
                return true;
            }
            return false;
        }

        public bool kiemTraSo(string item)
        {
            string ktra = @"^[0-9]+$";
            if(Regex.IsMatch(item,ktra))
            {
                return true;
            }
            return false;
        }

        public bool kiemTraSoDuong(float so)
        {
            if(so <= 0)
            {
                return false;
            }
            return true;
        }

        // Trong SanPhamBUS
        public int TinhSoLuongToiDa(int maSP)
        {
            congThucBUS ctBUS = new congThucBUS();
            nguyenLieuBUS nlBUS = new nguyenLieuBUS();
            heSoBUS hsBUS = new heSoBUS();

            BindingList<congThucDTO> listCT = ctBUS.docDSCongThucTheoSP(maSP);

            if (listCT.Count == 0) return 9999;

            decimal maxQty = 999999;

            foreach (var ct in listCT)
            {
                nguyenLieuDTO nl = nlBUS.LayNguyenLieuTheoID(ct.MaNguyenLieu);
                if (nl == null) continue;
                decimal tuSo = nl.TonKho;
                decimal mauSo = 0;
                if (ct.MaDonViCoSo == nl.MaDonViCoSo)
                {
                    mauSo = ct.SoLuongCoSo;
                }
                else
                {
                    decimal heSo = hsBUS.LayHeSo(ct.MaNguyenLieu, ct.MaDonViCoSo);
                    if (heSo == 0) return 0;
                    mauSo = ct.SoLuongCoSo * heSo;
                }

                if (mauSo == 0) continue;

                decimal sl = Math.Floor(tuSo / mauSo);

                if (sl < maxQty) maxQty = sl;
            }
            return (int)maxQty;
        }

        // Bỏ chữ static đi
        public BindingList<sanPhamDTO> dsSanPhamBanhang = new BindingList<sanPhamDTO>();

        public BindingList<sanPhamDTO> LayDanhSachCoCongThuc(bool forceReload = false)
        {
            // Luôn luôn gọi DAO lấy dữ liệu mới nhất
            // Không cần check if(...) nữa, vì bán hàng cần số liệu chính xác
            sanPhamDAO data = new sanPhamDAO();
            dsSanPhamBanhang = data.DocDanhSachSanPhamCoCongThuc();

            return dsSanPhamBanhang;
        }
    }
}
