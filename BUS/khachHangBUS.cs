using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;

namespace BUS
{
    public class khachHangBUS
    {
        private khachHangDAO data = new khachHangDAO();
        public static BindingList<khachHangDTO> ds = new BindingList<khachHangDTO>();

        public BindingList<khachHangDTO> LayDanhSach()
        {
            if (ds == null || ds.Count <= 0)
            {
                ds = data.LayDanhSach();
            }
            return ds;
        }

        public int layMa()
        {
            return data.layMa();
        }

        public bool them(khachHangDTO kh)
        {
            bool kq = data.Them(kh);
            if (kq)
            {
                ds.Add(kh);
            }
            return kq;
        }
        public bool sua(khachHangDTO kh)
        {
            bool kq = data.Sua(kh);
            if (kq)
            {
                khachHangDTO tontai = ds.FirstOrDefault(x => x.MaKhachHang == kh.MaKhachHang);
                if (tontai != null)
                {
                    tontai.TenKhachHang = kh.TenKhachHang;
                    tontai.SoDienThoai = kh.SoDienThoai;
                    tontai.Email = kh.Email;
                }
            }
            return kq;
        }
        public bool xoa(int maXoa)
        {
            bool kq = data.Xoa(maXoa);
            if (kq)
            {
                khachHangDTO tontai = ds.FirstOrDefault(x => x.MaKhachHang == maXoa);
                if (tontai != null)
                {
                    ds.Remove(tontai);
                }
            }
            return kq;
        }

        public bool kiemTraTrungTen(string tenKH)
        {
            khachHangDTO kh = ds.FirstOrDefault(x => x.TenKhachHang.ToLower().Equals(tenKH.ToLower()));
            if(kh!=null)
            {
                return true;
            }
            return false;
        }
        public bool kiemTraTrungEmail(string email)
        {
            khachHangDTO kh = ds.FirstOrDefault(x => x.Email.Equals(email));
            if (kh != null)
            {
                return true;
            }
            return false;
        }
        public bool kiemTraTrungSDT(string sdt)
        {
            khachHangDTO kh = ds.FirstOrDefault(x => x.SoDienThoai.Equals(sdt));
            if (kh != null)
            {
                return true;
            }
            return false;
        }

        public bool LaKhachHangGiongNhau(khachHangDTO a, khachHangDTO b)
        {
            return a.TenKhachHang.Trim() == b.TenKhachHang.Trim()
                && (a.SoDienThoai ?? "").Trim() == (b.SoDienThoai ?? "").Trim()
                && (a.Email ?? "").Trim() == (b.Email ?? "").Trim();
        }

        public string NhapExcelThongMinh(BindingList<khachHangDTO> dsExcel)
        {
            var dsDB = data.LayDanhSach();
            BindingList<string> danhSachLoi = new BindingList<string>();

            HashSet<string> sdtTrongExcel = new HashSet<string>();
            HashSet<string> emailTrongExcel = new HashSet<string>();

            foreach (var kh in dsExcel)
            {
                if (string.IsNullOrEmpty(kh.TenKhachHang))
                {
                    danhSachLoi.Add($"Dòng có mã {kh.MaKhachHang}: Tên khách hàng không được để trống.");
                    continue;
                }

                if (!string.IsNullOrEmpty(kh.SoDienThoai) && !sdtTrongExcel.Add(kh.SoDienThoai))
                {
                    danhSachLoi.Add($"SĐT {kh.SoDienThoai} bị lặp lại nhiều lần trong file Excel.");
                }

                if (!string.IsNullOrEmpty(kh.Email) && !emailTrongExcel.Add(kh.Email))
                {
                    danhSachLoi.Add($"Email {kh.Email} bị lặp lại nhiều lần trong file Excel.");
                }
            }

            foreach (var khMoi in dsExcel)
            {
                var trungSDT = dsDB.FirstOrDefault(x => x.SoDienThoai == khMoi.SoDienThoai && x.MaKhachHang != khMoi.MaKhachHang);
                if (trungSDT != null)
                {
                    danhSachLoi.Add($"Khách hàng '{khMoi.TenKhachHang}': SĐT {khMoi.SoDienThoai} đã thuộc về KH khác (Mã: {trungSDT.MaKhachHang}).");
                }
                if (!string.IsNullOrEmpty(khMoi.Email))
                {
                    var trungEmail = dsDB.FirstOrDefault(x => x.Email == khMoi.Email && x.MaKhachHang != khMoi.MaKhachHang);
                    if (trungEmail != null)
                    {
                        danhSachLoi.Add($"Khách hàng '{khMoi.TenKhachHang}': Email {khMoi.Email} đã thuộc về KH khác (Mã: {trungEmail.MaKhachHang}).");
                    }
                }
            }

            if (danhSachLoi.Count > 0)
            {
                return string.Join("\n• ", danhSachLoi);
            }

            int soThem = 0, soCapNhat = 0, soBoQua = 0;
            foreach (var khMoi in dsExcel)
            {
                var khCu = dsDB.FirstOrDefault(x => x.MaKhachHang == khMoi.MaKhachHang);

                if (khCu == null)
                {
                    data.Them(khMoi);
                    soThem++;
                }
                else if (!LaKhachHangGiongNhau(khCu, khMoi))
                {
                    data.Sua(khMoi);
                    soCapNhat++;
                }
                else
                {
                    soBoQua++;
                }
            }
            ds = data.LayDanhSach();
            return $"Hoàn tất!\n- Thêm mới: {soThem}\n- Cập nhật: {soCapNhat}\n- Bỏ qua: {soBoQua}";
        }

        public BindingList<khachHangDTO> timKiemCoban(string tim, int index)
        {
            BindingList<khachHangDTO> dskq = new BindingList<khachHangDTO>();
            if (ds == null || ds.Count == 0)
            {
                LayDanhSach();
            }

            foreach (khachHangDTO ct in ds)
            {
                switch (index)
                {
                    case 0:
                        {
                            if (ct.MaKhachHang.ToString().Contains(tim))
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 1:
                        {
                            if (ct.TenKhachHang.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 2:
                        {
                            if (ct.SoDienThoai.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 3:
                        {
                            if (ct.Email.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                }
            }
            return dskq;
        }

        public BindingList<khachHangDTO> timKiemNangCao(string tenKH, string sdtKH, string emailKH)
        {
            BindingList<khachHangDTO> dskq = new BindingList<khachHangDTO>();
            if(ds == null || ds.Count < 0)
            {
                LayDanhSach();
            }

            foreach(khachHangDTO ct in ds)
            {
                bool dk = true;
                if(!string.IsNullOrEmpty(tenKH) && ct.TenKhachHang.IndexOf(tenKH, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }
                if(!string.IsNullOrEmpty(sdtKH) && ct.SoDienThoai.IndexOf(sdtKH, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }
                if(!string.IsNullOrEmpty(emailKH) && ct.Email.IndexOf(emailKH,StringComparison.OrdinalIgnoreCase) < 0)
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