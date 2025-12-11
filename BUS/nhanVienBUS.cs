using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace BUS
{
    public class nhanVienBUS
    {
        private nhanVienDAO data = new nhanVienDAO();
        public static BindingList<nhanVienDTO> ds = new BindingList<nhanVienDTO>();

        public BindingList<nhanVienDTO> LayDanhSach(bool forceReload = false)
        {
            if(ds == null || ds.Count == 0 || forceReload)
            {
                ds = data.LayDanhSach();
            }
            return ds;
        }

        public bool ThemNhanVien(nhanVienDTO nv)
        {
            bool kq = data.ThemNhanVien(nv);
            if(kq)
            {
                ds.Add(nv);
            }
            return kq;
        }

        public bool CapNhatNhanVien(nhanVienDTO nv)
        {
            bool kq = data.CapNhatNhanVien(nv);
            if(kq)
            {
                nhanVienDTO tontai = ds.FirstOrDefault(x => x.MaNhanVien == nv.MaNhanVien);
                if(tontai != null)
                {
                    tontai.HoTen = nv.HoTen;
                    tontai.SoDienThoai = nv.SoDienThoai;
                    tontai.Email = nv.Email;
                    tontai.Luong = nv.Luong;
                }
            }
            return kq;
        }

        public bool XoaNhanVien(int maNV)
        {
            bool kq = data.XoaNhanVien(maNV);
            if(kq)
            {
                nhanVienDTO nv = ds.FirstOrDefault(x => x.MaNhanVien == maNV);
                if(nv != null)
                {
                    ds.Remove(nv);
                }
            }
            return kq;
        }

        public int LayMa()
        {
            return data.layMa();
        }

        public BindingList<nhanVienDTO> timKiemCoban(string tim, int index)
        {
            BindingList<nhanVienDTO> dskq = new BindingList<nhanVienDTO>();
            if (ds == null || ds.Count == 0)
            {
                LayDanhSach();
            }

            foreach (nhanVienDTO ct in ds)
            {
                switch (index)
                {
                    case 0:
                        {
                            if (ct.MaNhanVien.ToString().Contains(tim))
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 1:
                        {
                            if (ct.HoTen.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
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

        public BindingList<nhanVienDTO> timKiemNangCao(string tenNCC, string sdtNCC, string emailNCC, int tk)
        {
            BindingList<nhanVienDTO> dskq = new BindingList<nhanVienDTO>();
            if (ds == null || ds.Count < 0)
            {
                LayDanhSach();
            }
            BindingList<taikhoanDTO> dsTK = new taikhoanBUS().LayDanhSach();

            foreach (nhanVienDTO ct in ds)
            {
                bool dk = true;
                if (!string.IsNullOrEmpty(tenNCC) && ct.HoTen.IndexOf(tenNCC, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }
                if (!string.IsNullOrEmpty(sdtNCC) && ct.SoDienThoai.IndexOf(sdtNCC, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }
                if (!string.IsNullOrEmpty(emailNCC) && ct.Email.IndexOf(emailNCC, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    dk = false;
                }

                taikhoanDTO tkne = dsTK.FirstOrDefault(x => x.MANHANVIEN == ct.MaNhanVien);
                if(tk != -1)
                {
                    bool coTaiKhoan = (tkne != null);
                    if (tk == 1 && !coTaiKhoan)
                    {
                        dk = false;
                    }
                    else if (tk == 0 && coTaiKhoan)
                    {
                        dk = false;
                    }
                }

                if (dk)
                {
                    dskq.Add(ct);
                }
            }
            return dskq;
        }

        public bool LaNhanVienGiongNhau(nhanVienDTO a, nhanVienDTO b)
        {
            // So sánh Tên, SĐT, Email và Lương
            // Lưu ý: Cột trạng thái 0/1 thường ít khi thay đổi qua excel, nhưng nếu cần bạn có thể thêm vào đây
            return a.HoTen.Trim() == b.HoTen.Trim()
                && (a.SoDienThoai ?? "").Trim() == (b.SoDienThoai ?? "").Trim()
                && (a.Email ?? "").Trim() == (b.Email ?? "").Trim()
                && Math.Abs(a.Luong - b.Luong) < 0.01; // So sánh số thực (float)
        }

        public string NhapExcelThongMinh(BindingList<nhanVienDTO> dsExcel)
        {
            // Lấy dữ liệu mới nhất từ DB lên để so sánh
            // Reload lại để đảm bảo cache không bị cũ
            LayDanhSach(true);
            var dsDB = ds;

            BindingList<string> danhSachLoi = new BindingList<string>();

            // Hashset để kiểm tra trùng lặp nội bộ trong file Excel
            HashSet<string> sdtTrongExcel = new HashSet<string>();
            HashSet<string> emailTrongExcel = new HashSet<string>();

            // --- BƯỚC 1: KIỂM TRA LỖI DỮ LIỆU (VALIDATION) ---
            foreach (var nv in dsExcel)
            {
                // Chuẩn hóa dữ liệu đầu vào (Xóa khoảng trắng thừa)
                nv.HoTen = nv.HoTen?.Trim();
                nv.SoDienThoai = nv.SoDienThoai?.Trim();
                nv.Email = nv.Email?.Trim();

                if (string.IsNullOrEmpty(nv.HoTen))
                {
                    danhSachLoi.Add($"Có dòng bị thiếu Tên nhân viên.");
                    continue;
                }

                // Check trùng SĐT trong file Excel
                if (!string.IsNullOrEmpty(nv.SoDienThoai))
                {
                    if (!sdtTrongExcel.Add(nv.SoDienThoai))
                        danhSachLoi.Add($"SĐT {nv.SoDienThoai} bị lặp lại nhiều lần trong file Excel.");
                }

                // Check trùng Email trong file Excel
                if (!string.IsNullOrEmpty(nv.Email))
                {
                    if (!emailTrongExcel.Add(nv.Email))
                        danhSachLoi.Add($"Email {nv.Email} bị lặp lại nhiều lần trong file Excel.");
                }
            }

            // --- BƯỚC 2: CHECK XUNG ĐỘT DATABASE ---
            // (Kiểm tra xem Email trong Excel có bị trùng với người KHÁC trong DB không)
            foreach (var nvMoi in dsExcel)
            {
                if (!string.IsNullOrEmpty(nvMoi.Email))
                {
                    // Tìm người có cùng Email nhưng KHÁC SĐT (nghĩa là người khác)
                    var trungEmail = dsDB.FirstOrDefault(x =>
                        x.Email != null &&
                        x.Email.Trim().Equals(nvMoi.Email, StringComparison.OrdinalIgnoreCase) &&
                        (x.SoDienThoai ?? "").Trim() != (nvMoi.SoDienThoai ?? "").Trim());

                    if (trungEmail != null)
                    {
                        danhSachLoi.Add($"NV '{nvMoi.HoTen}': Email {nvMoi.Email} đã thuộc về người khác (Mã: {trungEmail.MaNhanVien}).");
                    }
                }
            }

            if (danhSachLoi.Count > 0)
            {
                return "Phát hiện lỗi dữ liệu:\n• " + string.Join("\n• ", danhSachLoi);
            }

            // --- BƯỚC 3: THỰC HIỆN LƯU ---
            int soThem = 0, soCapNhat = 0, soBoQua = 0;

            foreach (var nvMoi in dsExcel)
            {
                // Tìm nhân viên cũ dựa trên SĐT (đã Trim sạch sẽ)
                var nvCu = dsDB.FirstOrDefault(x =>
                    x.SoDienThoai != null &&
                    x.SoDienThoai.Trim() == nvMoi.SoDienThoai
                );

                if (nvCu == null)
                {
                    // === THÊM MỚI ===
                    nvMoi.TrangThai = 1;
                    nvMoi.NgayTao = DateTime.Now;

                    if (data.ThemNhanVien(nvMoi)) // Lưu xuống SQL
                    {
                        soThem++;
                    }
                }
                else
                {
                    // === CẬP NHẬT ===
                    if (!LaNhanVienGiongNhau(nvCu, nvMoi))
                    {
                        nvMoi.MaNhanVien = nvCu.MaNhanVien; // Lấy ID cũ ốp vào
                        nvMoi.TrangThai = nvCu.TrangThai;
                        nvMoi.NgayTao = nvCu.NgayTao;

                        if (data.CapNhatNhanVien(nvMoi)) // Lưu xuống SQL
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

            // --- QUAN TRỌNG NHẤT: RELOAD LẠI DANH SÁCH HIỂN THỊ ---
            // Sau khi vòng lặp xong, Database đã đổi, nhưng List ds thì chưa.
            // Phải gọi hàm này để nó load lại từ DB lên GridView.
            LayDanhSach(true);

            return $"Hoàn tất nhập liệu!\n\n- Thêm mới: {soThem}\n- Cập nhật: {soCapNhat}\n- Bỏ qua: {soBoQua}";
        }

        public nhanVienDTO LayThongTinNhanVien(int maNV)
        {
            if (ds == null || ds.Count == 0) LayDanhSach();
            return ds.FirstOrDefault(x => x.MaNhanVien == maNV);
        }

        public bool kiemTraTrungTenNV(string tenNV)
        {
            nhanVienDTO nv = ds.FirstOrDefault(x => x.HoTen.ToLower().Equals(tenNV.ToLower()));
            if(nv != null)
            {
                return true;
            }
            return false;
        }

        public bool kiemTraTrungSDT(string sdt)
        {
            nhanVienDTO nv = ds.FirstOrDefault(x => x.SoDienThoai.Equals(sdt));
            if(nv!= null)
            {
                return true;
            }
            return false;
        }

        public bool kiemTraTrungEmail(string email)
        {
            nhanVienDTO nv = ds.FirstOrDefault(x => x.Email.ToLower().Equals(email.ToLower()));
            if (nv != null)
            {
                return true;
            }
            return false;
        }
    }
}