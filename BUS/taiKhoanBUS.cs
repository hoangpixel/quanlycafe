using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BUS
{
    public class taikhoanBUS
    {
        public BindingList<taikhoanDTO> ds = new BindingList<taikhoanDTO>();
        private taikhoanDAO data = new taikhoanDAO();
        public BindingList<taikhoanDTO> LayDanhSach()
        {
            if (ds == null || ds.Count == 0)
            {
                ds = data.LayDanhSach();
            }
            return ds;
        }

        public int LayMa()
        {
            return data.layMa();
        }

        public bool Them(taikhoanDTO tk)
        {
            tk.MATKHAU = MaHoaMatKhau.ToSHA256(tk.MATKHAU);
            bool kq = data.Them(tk);
            if (kq)
            {
                ds.Add(tk);
            }
            return kq;
        }

        public bool Sua(taikhoanDTO tk, bool isNewPass)
        {
            if (isNewPass)
            {
                tk.MATKHAU = MaHoaMatKhau.ToSHA256(tk.MATKHAU);
            }

            bool kq = data.Sua(tk);

            if (kq)
            {
                taikhoanDTO tontai = ds.FirstOrDefault(x => x.MATAIKHOAN == tk.MATAIKHOAN);
                if (tontai != null)
                {
                    tontai.MANHANVIEN = tk.MANHANVIEN;
                    tontai.TENDANGNHAP = tk.TENDANGNHAP;
                    tontai.MAVAITRO = tk.MAVAITRO;
                    tontai.TRANGTHAI = tk.TRANGTHAI;

                    if (isNewPass)
                    {
                        tontai.MATKHAU = tk.MATKHAU;
                    }
                }
            }
            return kq;
        }

        public bool Xoa(int maTK)
        {
            bool kq = data.Xoa(maTK);
            if (kq)
            {
                taikhoanDTO tontai = ds.FirstOrDefault(x => x.MATAIKHOAN == maTK);
                if(tontai != null)
                {
                    ds.Remove(tontai);
                }
            }
            return kq;
        }

        public taikhoanDTO KiemTraDangNhap(string user, string pass)
        {
            string passHash = MaHoaMatKhau.ToSHA256(pass);
            return data.DangNhap(user, passHash);
        }

        public BindingList<taikhoanDTO> timKiemCoban(string tim, int index)
        {
            BindingList<taikhoanDTO> dskq = new BindingList<taikhoanDTO>();
            if (ds == null || ds.Count == 0)
            {
                LayDanhSach();
            }

            BindingList<nhanVienDTO> dsNV = new nhanVienBUS().LayDanhSach();
            BindingList<vaitroDTO> dsVT = new vaitroBUS().LayDanhSach();

            foreach (taikhoanDTO ct in ds)
            {
                switch (index)
                {
                    case 0:
                        {
                            if (ct.MATAIKHOAN.ToString().Contains(tim))
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 1:
                        {
                            if (ct.TENDANGNHAP.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 2:
                        {
                            nhanVienDTO nv = dsNV.FirstOrDefault(x => x.MaNhanVien == ct.MANHANVIEN);
                            string tenNV = nv?.HoTen ?? "";
                            if (tenNV.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 3:
                        {
                            vaitroDTO vt = dsVT.FirstOrDefault(x => x.MaVaiTro == ct.MAVAITRO);
                            string tenVT = vt?.TenVaiTro ?? "";
                            if (tenVT.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                }
            }
            return dskq;
        }

        public BindingList<taikhoanDTO> timKiemNangCao(string tenNV, string tenVT, string tenTK)
        {
            BindingList<taikhoanDTO> dskq = new BindingList<taikhoanDTO>();
            if (ds == null || ds.Count < 0)
            {
                LayDanhSach();
            }

            BindingList<nhanVienDTO> dsNV = new nhanVienBUS().LayDanhSach();
            BindingList<vaitroDTO> dsVT = new vaitroBUS().LayDanhSach();

            foreach (taikhoanDTO ct in ds)
            {
                nhanVienDTO nv = dsNV.FirstOrDefault(x => x.MaNhanVien == ct.MANHANVIEN);
                string tenNVtim = nv?.HoTen ?? "";
                vaitroDTO vt = dsVT.FirstOrDefault(x => x.MaVaiTro == ct.MAVAITRO);
                string tenVTtim = vt?.TenVaiTro ?? "";
                bool dk = true;
                if (!string.IsNullOrEmpty(tenNV) && tenNVtim.IndexOf(tenNV, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }
                if (!string.IsNullOrEmpty(tenVT) && tenVTtim.IndexOf(tenVT, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }
                if (!string.IsNullOrEmpty(tenTK) && ct.TENDANGNHAP.IndexOf(tenTK, StringComparison.OrdinalIgnoreCase) < 0)
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

        public bool LaTaiKhoanGiongNhau(taikhoanDTO a, taikhoanDTO b)
        {
            return a.MANHANVIEN == b.MANHANVIEN
                && a.TENDANGNHAP == b.TENDANGNHAP
                && a.MAVAITRO == b.MAVAITRO
                && a.TRANGTHAI == b.TRANGTHAI;
        }

        public string NhapExcelThongMinh(BindingList<taikhoanDTO> dsExcel)
        {
            nhanVienBUS busNV = new nhanVienBUS();
            vaitroBUS busVT = new vaitroBUS();

            var listMaNV = busNV.LayDanhSach().Select(x => x.MaNhanVien).ToList();
            var listMaVT = busVT.LayDanhSach().Select(x => x.MaVaiTro).ToList();

            var dsDB = LayDanhSach();

            BindingList<string> danhSachLoi = new BindingList<string>();
            HashSet<string> tenDangNhapDaGap = new HashSet<string>();

            foreach (var tk in dsExcel)
            {
                tk.TENDANGNHAP = tk.TENDANGNHAP?.Trim();
                tk.MATKHAU = tk.MATKHAU?.Trim();

                if (string.IsNullOrEmpty(tk.TENDANGNHAP)) continue;
                if (!tenDangNhapDaGap.Add(tk.TENDANGNHAP)) danhSachLoi.Add($"Trùng user trong file: {tk.TENDANGNHAP}");
            }

            if (danhSachLoi.Count > 0) return "Lỗi:\n• " + string.Join("\n• ", danhSachLoi);

            int soThem = 0, soCapNhat = 0, soBoQua = 0;

            foreach (var tkMoi in dsExcel)
            {
                if (string.IsNullOrEmpty(tkMoi.TENDANGNHAP)) continue;

                var tkCu = dsDB.FirstOrDefault(x => x.TENDANGNHAP == tkMoi.TENDANGNHAP);
                if (tkCu == null && tkMoi.MATAIKHOAN != 0) tkCu = dsDB.FirstOrDefault(x => x.MATAIKHOAN == tkMoi.MATAIKHOAN);
                if (tkCu == null && tkMoi.MANHANVIEN != 0) tkCu = dsDB.FirstOrDefault(x => x.MANHANVIEN == tkMoi.MANHANVIEN);

                if (tkCu == null)
                {
                    if (string.IsNullOrEmpty(tkMoi.MATKHAU)) tkMoi.MATKHAU = "123456";

                    if (this.Them(tkMoi)) soThem++;
                }
                else
                {
                    bool canCapNhat = false;
                    bool isNewPass = false;
                    if (!LaTaiKhoanGiongNhau(tkCu, tkMoi)) canCapNhat = true;
                    if (!string.IsNullOrEmpty(tkMoi.MATKHAU))
                    {
                        if (tkMoi.MATKHAU == tkCu.MATKHAU)
                        {
                            isNewPass = false;
                        }
                        else
                        {
                            string thuHash = MaHoaMatKhau.ToSHA256(tkMoi.MATKHAU);

                            if (thuHash != tkCu.MATKHAU)
                            {
                                isNewPass = true;
                                canCapNhat = true;
                            }
                            else
                            {
                                isNewPass = false;
                                tkMoi.MATKHAU = tkCu.MATKHAU;
                            }
                        }
                    }
                    else
                    {
                        isNewPass = false;
                        tkMoi.MATKHAU = tkCu.MATKHAU;
                    }

                    if (canCapNhat)
                    {
                        tkMoi.MATAIKHOAN = tkCu.MATAIKHOAN;
                        if (this.Sua(tkMoi, isNewPass))
                        {
                            soCapNhat++;
                        }
                    }
                    else
                    {
                        soBoQua++;
                    }
                }
            }

            // Reload lần cuối để đồng bộ GridView
            LayDanhSach();
            return $"Xong!\n- Thêm: {soThem}\n- Sửa: {soCapNhat}\n- Bỏ qua: {soBoQua}";
        }

        public bool kiemTraTrungNhanVien(int maNV)
        {
            if (ds == null || ds.Count == 0)
            {
                LayDanhSach();
            }
            taikhoanDTO tontai = ds.FirstOrDefault(x => x.MANHANVIEN == maNV);
            if(tontai != null)
            {
                return true;
            }
            return false;
        }

        public bool kiemTraTrungTenTK(string tenTK,int maTK = 0)
        {
            taikhoanDTO tontai = ds.FirstOrDefault(x =>
                    x.TENDANGNHAP.ToLower() == tenTK.ToLower()
                    && x.MATAIKHOAN != maTK
                );

            if (tontai != null)
            {
                return true;
            }
            return false;
        }
        public bool KiemTraRong(string tenVaiTro)
        {
            return string.IsNullOrWhiteSpace(tenVaiTro);
        }
    }
}