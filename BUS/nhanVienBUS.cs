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
            var dsDB = data.LayDanhSach();
            BindingList<string> danhSachLoi = new BindingList<string>();

            // Hashset để kiểm tra trùng lặp nội bộ trong file Excel ngay lập tức
            HashSet<string> sdtTrongExcel = new HashSet<string>();
            HashSet<string> emailTrongExcel = new HashSet<string>();

            // --- BƯỚC 1: KIỂM TRA LỖI DỮ LIỆU TRONG FILE EXCEL ---
            foreach (var nv in dsExcel)
            {
                // 1.1 Kiểm tra tên rỗng
                if (string.IsNullOrEmpty(nv.HoTen))
                {
                    danhSachLoi.Add($"Có dòng bị thiếu Tên nhân viên.");
                    continue;
                }

                // 1.2 Kiểm tra trùng SĐT trong chính file Excel
                if (!string.IsNullOrEmpty(nv.SoDienThoai) && !sdtTrongExcel.Add(nv.SoDienThoai))
                {
                    danhSachLoi.Add($"SĐT {nv.SoDienThoai} bị lặp lại nhiều lần trong file Excel.");
                }

                // 1.3 Kiểm tra trùng Email trong chính file Excel
                if (!string.IsNullOrEmpty(nv.Email) && !emailTrongExcel.Add(nv.Email))
                {
                    danhSachLoi.Add($"Email {nv.Email} bị lặp lại nhiều lần trong file Excel.");
                }
            }

            // --- BƯỚC 2: KIỂM TRA XUNG ĐỘT VỚI DATABASE ---
            foreach (var nvMoi in dsExcel)
            {
                // Kiểm tra Email có thuộc về người KHÁC không?
                // Logic: Tìm người trong DB có cùng Email nhưng KHÁC SĐT (vì ta dùng SĐT làm định danh chính)
                if (!string.IsNullOrEmpty(nvMoi.Email))
                {
                    var trungEmail = dsDB.FirstOrDefault(x => x.Email == nvMoi.Email && x.SoDienThoai != nvMoi.SoDienThoai);
                    if (trungEmail != null)
                    {
                        danhSachLoi.Add($"Nhân viên '{nvMoi.HoTen}': Email {nvMoi.Email} đã thuộc về NV khác (Mã: {trungEmail.MaNhanVien}).");
                    }
                }
            }

            // Nếu có lỗi thì trả về ngay danh sách lỗi
            if (danhSachLoi.Count > 0)
            {
                return "Phát hiện lỗi dữ liệu:\n• " + string.Join("\n• ", danhSachLoi);
            }

            // --- BƯỚC 3: THỰC HIỆN THÊM / SỬA ---
            int soThem = 0, soCapNhat = 0, soBoQua = 0;

            foreach (var nvMoi in dsExcel)
            {
                // Vì file Import không có Mã NV, ta tìm nhân viên cũ bằng SỐ ĐIỆN THOẠI
                var nvCu = dsDB.FirstOrDefault(x => x.SoDienThoai == nvMoi.SoDienThoai);

                if (nvCu == null)
                {
                    // === TRƯỜNG HỢP THÊM MỚI ===
                    nvMoi.TrangThai = 1; // Mặc định nhân viên mới là Hoạt động (1)
                    nvMoi.NgayTao = DateTime.Now;

                    // Gọi hàm DAO để thêm (bạn cần đảm bảo DAO có hàm Them)
                    data.ThemNhanVien(nvMoi);
                    soThem++;
                }
                else
                {
                    // === TRƯỜNG HỢP CẬP NHẬT ===
                    // Nếu thông tin khác nhau thì mới update để tối ưu
                    if (!LaNhanVienGiongNhau(nvCu, nvMoi))
                    {
                        // Gán Mã NV cũ vào đối tượng mới để biết đường mà sửa
                        nvMoi.MaNhanVien = nvCu.MaNhanVien;

                        // Giữ nguyên trạng thái cũ hoặc cập nhật theo ý bạn
                        // Ở đây mình giữ nguyên trạng thái cũ trong DB (để tránh Excel ghi đè làm nhân viên bị khóa/mở khóa nhầm)
                        nvMoi.TrangThai = nvCu.TrangThai;
                        nvMoi.NgayTao = nvCu.NgayTao; // Giữ ngày tạo gốc

                        // Gọi hàm DAO để sửa (bạn cần đảm bảo DAO có hàm Sua)
                        data.CapNhatNhanVien(nvMoi);
                        soCapNhat++;
                    }
                    else
                    {
                        soBoQua++;
                    }
                }
            }
            return $"Hoàn tất nhập liệu!\n\n- Thêm mới: {soThem}\n- Cập nhật: {soCapNhat}\n- Bỏ qua (không đổi): {soBoQua}";
        }

        public nhanVienDTO LayThongTinNhanVien(int maNV)
        {
            if (ds == null || ds.Count == 0) LayDanhSach();
            return ds.FirstOrDefault(x => x.MaNhanVien == maNV);
        }
    }
}