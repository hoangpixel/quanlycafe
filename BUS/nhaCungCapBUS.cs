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
            var dsDB = data.LayDanhSach(); // Lấy dữ liệu hiện tại từ DB
            BindingList<string> danhSachLoi = new BindingList<string>();

            HashSet<string> sdtTrongExcel = new HashSet<string>();
            HashSet<string> emailTrongExcel = new HashSet<string>();

            // 1. Check lỗi nội bộ trong file Excel trước
            foreach (var ncc in dsExcel)
            {
                // Check tên rỗng
                if (string.IsNullOrEmpty(ncc.TenNCC))
                {
                    danhSachLoi.Add($"Dòng có mã {ncc.MaNCC}: Tên nhà cung cấp không được để trống.");
                    continue;
                }

                // Check trùng SĐT trong chính file Excel
                if (!string.IsNullOrEmpty(ncc.SoDienThoai) && !sdtTrongExcel.Add(ncc.SoDienThoai))
                {
                    danhSachLoi.Add($"SĐT {ncc.SoDienThoai} bị lặp lại nhiều lần trong file Excel.");
                }

                // Check trùng Email trong chính file Excel
                if (!string.IsNullOrEmpty(ncc.Email) && !emailTrongExcel.Add(ncc.Email))
                {
                    danhSachLoi.Add($"Email {ncc.Email} bị lặp lại nhiều lần trong file Excel.");
                }
            }

            // 2. Check trùng với Database (Chỉ check nếu mã khác nhau - trường hợp thêm mới hoặc sửa sai mã)
            foreach (var nccMoi in dsExcel)
            {
                // Check SĐT tồn tại ở NCC khác
                if (!string.IsNullOrEmpty(nccMoi.SoDienThoai))
                {
                    var trungSDT = dsDB.FirstOrDefault(x => x.SoDienThoai == nccMoi.SoDienThoai && x.MaNCC != nccMoi.MaNCC);
                    if (trungSDT != null)
                    {
                        danhSachLoi.Add($"NCC '{nccMoi.TenNCC}': SĐT {nccMoi.SoDienThoai} đã thuộc về NCC khác (Mã: {trungSDT.MaNCC}).");
                    }
                }

                // Check Email tồn tại ở NCC khác
                if (!string.IsNullOrEmpty(nccMoi.Email))
                {
                    var trungEmail = dsDB.FirstOrDefault(x => x.Email == nccMoi.Email && x.MaNCC != nccMoi.MaNCC);
                    if (trungEmail != null)
                    {
                        danhSachLoi.Add($"NCC '{nccMoi.TenNCC}': Email {nccMoi.Email} đã thuộc về NCC khác (Mã: {trungEmail.MaNCC}).");
                    }
                }
            }

            // Nếu có lỗi thì trả về luôn, không xử lý tiếp
            if (danhSachLoi.Count > 0)
            {
                return "Phát hiện lỗi dữ liệu:\n• " + string.Join("\n• ", danhSachLoi);
            }

            // 3. Thực hiện Thêm / Sửa / Bỏ qua
            int soThem = 0, soCapNhat = 0, soBoQua = 0;
            foreach (var nccMoi in dsExcel)
            {
                var nccCu = dsDB.FirstOrDefault(x => x.MaNCC == nccMoi.MaNCC);

                if (nccCu == null)
                {
                    // Không tìm thấy ID -> Thêm mới
                    data.Them(nccMoi);
                    soThem++;
                }
                else if (!LaNhaCungCapGiongNhau(nccCu, nccMoi))
                {
                    // Tìm thấy ID nhưng thông tin khác -> Cập nhật
                    data.Sua(nccMoi);
                    soCapNhat++;
                }
                else
                {
                    // Thông tin y hệt -> Bỏ qua cho nhẹ DB
                    soBoQua++;
                }
            }

            ds = data.LayDanhSach(); // Refresh lại danh sách
            return $"Hoàn tất nhập liệu!\n- Thêm mới: {soThem}\n- Cập nhật: {soCapNhat}\n- Bỏ qua (không đổi): {soBoQua}";
        }
    }
}