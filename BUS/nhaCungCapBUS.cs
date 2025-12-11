using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace BUS
{
    public class nhaCungCapBUS
    {
        private nhaCungCapDAO data = new nhaCungCapDAO();
        public static BindingList<nhaCungCapDTO> ds = new BindingList<nhaCungCapDTO>();

        public BindingList<nhaCungCapDTO> LayDanhSach()
        {
            if(ds == null || ds.Count == 0)
            {
                ds = data.LayDanhSach();
            }
            return ds;
        }

        public int LayMa()
        {
            return data.layMa();
        }

        public bool them(nhaCungCapDTO ct)
        {
            bool kq = data.Them(ct);
            if(kq)
            {
                ds.Add(ct);
            }
            return kq;
        }

        public bool sua(nhaCungCapDTO ct)
        {
            bool kq = data.Sua(ct);
            if(kq)
            {
                nhaCungCapDTO tontai = ds.FirstOrDefault(x => x.MaNCC == ct.MaNCC);
                if(tontai != null)
                {
                    tontai.TenNCC = ct.TenNCC;
                    tontai.SoDienThoai = ct.SoDienThoai;
                    tontai.Email = ct.Email;
                    tontai.DiaChi = ct.DiaChi;
                }
            }
            return kq;
        }

        public bool xoa(int maXoa)
        {
            bool kq = data.Xoa(maXoa);
            if(kq)
            {
                nhaCungCapDTO tontai = ds.FirstOrDefault(x => x.MaNCC == maXoa);
                if(tontai != null)
                {
                    ds.Remove(tontai);
                }
            }
            return kq;
        }

        public bool kiemTraTrungTenNV(string tenNV)
        {
            nhaCungCapDTO nv = ds.FirstOrDefault(x => x.TenNCC.ToLower().Equals(tenNV.ToLower()));
            if (nv != null)
            {
                return true;
            }
            return false;
        }

        public bool kiemTraTrungSDT(string sdt)
        {
            nhaCungCapDTO nv = ds.FirstOrDefault(x => x.SoDienThoai.Equals(sdt));
            if (nv != null)
            {
                return true;
            }
            return false;
        }

        public bool kiemTraTrungEmail(string email)
        {
            nhaCungCapDTO nv = ds.FirstOrDefault(x => x.Email.ToLower().Equals(email.ToLower()));
            if (nv != null)
            {
                return true;
            }
            return false;
        }
        public BindingList<nhaCungCapDTO> timKiemCoban(string tim, int index)
        {
            BindingList<nhaCungCapDTO> dskq = new BindingList<nhaCungCapDTO>();
            if (ds == null || ds.Count == 0)
            {
                LayDanhSach();
            }

            foreach (nhaCungCapDTO ct in ds)
            {
                switch (index)
                {
                    case 0:
                        {
                            if (ct.MaNCC.ToString().Contains(tim))
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                    case 1:
                        {
                            if (ct.TenNCC.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
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
                    case 4:
                        {
                            if(ct.DiaChi.IndexOf(tim, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                dskq.Add(ct);
                            }
                            break;
                        }
                }
            }
            return dskq;
        }

        public BindingList<nhaCungCapDTO> timKiemNangCao(string tenNCC, string sdtNCC, string emailNCC, string diachiNCC)
        {
            BindingList<nhaCungCapDTO> dskq = new BindingList<nhaCungCapDTO>();
            if (ds == null || ds.Count < 0)
            {
                LayDanhSach();
            }

            foreach (nhaCungCapDTO ct in ds)
            {
                bool dk = true;
                if (!string.IsNullOrEmpty(tenNCC) && ct.TenNCC.IndexOf(tenNCC, StringComparison.OrdinalIgnoreCase) < 0)
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
                if (!string.IsNullOrEmpty(diachiNCC) && ct.DiaChi.IndexOf(diachiNCC, StringComparison.OrdinalIgnoreCase) < 0)
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




        private bool LaNhaCungCapGiongNhau(nhaCungCapDTO cu, nhaCungCapDTO moi)
        {
            return cu.TenNCC == moi.TenNCC &&
                   cu.SoDienThoai == moi.SoDienThoai &&
                   cu.Email == moi.Email &&
                   cu.DiaChi == moi.DiaChi;
        }

        public string NhapExcelThongMinh(BindingList<nhaCungCapDTO> dsExcel)
        {
            var dsDB = data.LayDanhSach(); // Lấy dữ liệu hiện có trong DB
            BindingList<string> danhSachLoi = new BindingList<string>();

            HashSet<string> sdtTrongExcel = new HashSet<string>();
            HashSet<string> emailTrongExcel = new HashSet<string>();

            // ================================
            // 1. Kiểm tra lỗi nội bộ trong file Excel
            // ================================
            foreach (var ncc in dsExcel)
            {
                // Tên NCC không được để trống
                if (string.IsNullOrWhiteSpace(ncc.TenNCC))
                {
                    danhSachLoi.Add($"Mã {ncc.MaNCC}: Tên nhà cung cấp không được để trống.");
                }

                // Trùng số điện thoại trong Excel
                if (!string.IsNullOrWhiteSpace(ncc.SoDienThoai) &&
                    !sdtTrongExcel.Add(ncc.SoDienThoai))
                {
                    danhSachLoi.Add($"SĐT {ncc.SoDienThoai} bị lặp lại trong file Excel.");
                }

                // Trùng email trong Excel
                if (!string.IsNullOrWhiteSpace(ncc.Email) &&
                    !emailTrongExcel.Add(ncc.Email))
                {
                    danhSachLoi.Add($"Email {ncc.Email} bị lặp lại trong file Excel.");
                }
            }

            // Nếu file Excel có lỗi thì dừng NGAY — không kiểm tra DB, không nhập
            if (danhSachLoi.Count > 0)
            {
                return "Phát hiện lỗi dữ liệu trong file Excel, vui lòng sửa trước khi nhập:\n• " +
                       string.Join("\n• ", danhSachLoi);
            }

            // ================================
            // 2. Kiểm tra trùng với Database
            // ================================
            foreach (var nccMoi in dsExcel)
            {
                // Trùng SĐT với NCC khác
                if (!string.IsNullOrWhiteSpace(nccMoi.SoDienThoai))
                {
                    var trungSDT = dsDB.FirstOrDefault(x =>
                        x.SoDienThoai == nccMoi.SoDienThoai &&
                        x.MaNCC != nccMoi.MaNCC);

                    if (trungSDT != null)
                        danhSachLoi.Add($"NCC '{nccMoi.TenNCC}': SĐT {nccMoi.SoDienThoai} đã tồn tại (Mã: {trungSDT.MaNCC}).");
                }

                // Trùng Email với NCC khác
                if (!string.IsNullOrWhiteSpace(nccMoi.Email))
                {
                    var trungEmail = dsDB.FirstOrDefault(x =>
                        x.Email == nccMoi.Email &&
                        x.MaNCC != nccMoi.MaNCC);

                    if (trungEmail != null)
                        danhSachLoi.Add($"NCC '{nccMoi.TenNCC}': Email {nccMoi.Email} đã tồn tại (Mã: {trungEmail.MaNCC}).");
                }
            }

            // Nếu trùng với DB thì dừng, không thêm/sửa
            if (danhSachLoi.Count > 0)
            {
                return "Phát hiện dữ liệu bị trùng với Database:\n• " +
                       string.Join("\n• ", danhSachLoi);
            }

            // ================================
            // 3. File sạch hoàn toàn → tiến hành thêm/cập nhật
            // ================================
            int soThem = 0, soCapNhat = 0, soBoQua = 0;

            foreach (var nccMoi in dsExcel)
            {
                var nccCu = dsDB.FirstOrDefault(x => x.MaNCC == nccMoi.MaNCC);

                if (nccCu == null)
                {
                    data.Them(nccMoi);
                    soThem++;
                }
                else if (!LaNhaCungCapGiongNhau(nccCu, nccMoi))
                {
                    data.Sua(nccMoi);
                    soCapNhat++;
                }
                else
                {
                    soBoQua++;
                }
            }

            // Refresh lại cache
            ds = data.LayDanhSach();

            return $"Hoàn tất nhập liệu!\n- Thêm mới: {soThem}\n- Cập nhật: {soCapNhat}\n- Bỏ qua: {soBoQua}";
        }

    }
}