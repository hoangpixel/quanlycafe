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

        // 🟢 NHẬP EXCEL THÔNG MINH
        public string NhapExcelThongMinh(BindingList<taikhoanDTO> dsExcel)
        {
            nhanVienDAO dataNV = new nhanVienDAO();
            vaitroDAO dataVT = new vaitroDAO();
            // 1. Lấy dữ liệu tham chiếu để kiểm tra
            var listMaNV = data.LayDanhSach().Select(x => x.MANHANVIEN).ToList(); // Lấy list Mã NV
            var listMaVT = dataVT.LayDanhSachVaiTro().Select(x => x.MaVaiTro).ToList();   // Lấy list Mã Vai Trò

            // Lấy danh sách tài khoản hiện tại trong DB để so sánh
            var dsDB = data.LayDanhSach();

            BindingList<string> danhSachLoi = new BindingList<string>();
            HashSet<string> tenDangNhapDaGap = new HashSet<string>();

            // 2. CHECK LỖI TRƯỚC KHI NHẬP
            foreach (var tk in dsExcel)
            {
                // Kiểm tra trùng lặp trong file Excel
                if (!tenDangNhapDaGap.Add(tk.TENDANGNHAP))
                {
                    danhSachLoi.Add($"Tên đăng nhập '{tk.TENDANGNHAP}' bị lặp lại trong file Excel.");
                }

                // Kiểm tra Mã Nhân Viên có tồn tại không
                if (!listMaNV.Contains(tk.MANHANVIEN))
                {
                    danhSachLoi.Add($"Nhân viên mã {tk.MANHANVIEN} (User: {tk.TENDANGNHAP}) không tồn tại trong hệ thống.");
                }

                // Kiểm tra Mã Vai Trò có tồn tại không
                if (!listMaVT.Contains(tk.MAVAITRO))
                {
                    danhSachLoi.Add($"Vai trò mã {tk.MAVAITRO} (User: {tk.TENDANGNHAP}) không tồn tại.");
                }
            }

            if (danhSachLoi.Count > 0)
            {
                return "Phát hiện lỗi dữ liệu:\n• " + string.Join("\n• ", danhSachLoi);
            }

            // 3. BẮT ĐẦU NHẬP
            int soThem = 0, soCapNhat = 0, soBoQua = 0;

            foreach (var tkMoi in dsExcel)
            {
                // Tìm xem tài khoản đã tồn tại chưa (Tìm theo Tên Đăng Nhập hoặc Mã TK)
                var tkCu = dsDB.FirstOrDefault(x => x.TENDANGNHAP == tkMoi.TENDANGNHAP);

                // Nếu tìm theo Mã TK trong Excel (nếu người dùng có nhập Mã TK)
                if (tkCu == null && tkMoi.MATAIKHOAN != 0)
                {
                    tkCu = dsDB.FirstOrDefault(x => x.MATAIKHOAN == tkMoi.MATAIKHOAN);
                }

                if (tkCu == null)
                {
                    // === TRƯỜNG HỢP THÊM MỚI ===
                    // Mật khẩu trong Excel là pass thô -> Cần Hash
                    if (string.IsNullOrEmpty(tkMoi.MATKHAU))
                    {
                        tkMoi.MATKHAU = "123456"; // Mặc định nếu Excel để trống
                    }
                    tkMoi.MATKHAU = MaHoaMatKhau.ToSHA256(tkMoi.MATKHAU);

                    if (data.Them(tkMoi))
                    {
                        ds.Add(tkMoi);
                        soThem++;
                    }
                }
                else
                {
                    // === TRƯỜNG HỢP CẬP NHẬT ===
                    bool canCapNhat = false;
                    bool coDoiMatKhau = false;

                    // 1. Kiểm tra thông tin cơ bản có khác không
                    if (!LaTaiKhoanGiongNhau(tkCu, tkMoi))
                    {
                        canCapNhat = true;
                    }

                    // 2. Kiểm tra mật khẩu trong Excel
                    // - Nếu Excel có nhập mật khẩu -> Hash và cập nhật
                    // - Nếu Excel để trống -> Giữ nguyên mật khẩu cũ
                    if (!string.IsNullOrEmpty(tkMoi.MATKHAU))
                    {
                        string hashMoi = MaHoaMatKhau.ToSHA256(tkMoi.MATKHAU);
                        // So sánh hash mới với hash trong DB
                        if (hashMoi != tkCu.MATKHAU)
                        {
                            tkMoi.MATKHAU = hashMoi; // Gán hash mới vào
                            canCapNhat = true;
                            coDoiMatKhau = true;
                        }
                    }
                    else
                    {
                        tkMoi.MATKHAU = tkCu.MATKHAU; // Giữ nguyên hash cũ
                    }

                    if (canCapNhat)
                    {
                        tkMoi.MATAIKHOAN = tkCu.MATAIKHOAN; // Đảm bảo đúng ID để update
                        // Gọi hàm Sua nhưng không cần hash lại nữa vì đã xử lý ở trên
                        // Lưu ý: data.Sua nhận vào object đã có mật khẩu chuẩn
                        if (data.Sua(tkMoi))
                        {
                            // Cập nhật lại vào Cache list ds (để GridView tự nhảy)
                            tkCu.MANHANVIEN = tkMoi.MANHANVIEN;
                            tkCu.MAVAITRO = tkMoi.MAVAITRO;
                            tkCu.TRANGTHAI = tkMoi.TRANGTHAI;
                            if (coDoiMatKhau) tkCu.MATKHAU = tkMoi.MATKHAU;

                            soCapNhat++;
                        }
                    }
                    else
                    {
                        soBoQua++;
                    }
                }
            }

            return $"Hoàn tất nhập liệu!\n\n- Thêm mới: {soThem}\n- Cập nhật: {soCapNhat}\n- Bỏ qua: {soBoQua}";
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