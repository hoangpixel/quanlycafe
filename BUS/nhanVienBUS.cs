using DAO;
using DTO;
using System;
using System.ComponentModel;
using System.Linq;

namespace BUS
{
    public class nhanVienBUS
    {
        // Khởi tạo lớp DAO để giao tiếp với CSDL
        private nhanVienDAO dao = new nhanVienDAO();

        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        public BindingList<nhanVienDTO> LayDanhSach()
        {
            try
            {
                return dao.LayDanhSach();
            }
            catch (Exception ex)
            {
                // Có thể ghi log lỗi ở đây nếu cần
                throw ex;
            }
        }

        /// <summary>
        /// Lấy tên nhân viên theo mã (Hỗ trợ hiển thị trên phiếu nhập)
        /// </summary>
        public string LayTenNhanVien(int maNV)
        {
            var list = dao.LayDanhSach();
            var nv = list.FirstOrDefault(x => x.MaNhanVien == maNV);
            return nv != null ? nv.HoTen : "Không xác định";
        }

        // ==================== CÁC METHOD BỔ SUNG ====================

        /// <summary>
        /// Thêm nhân viên mới
        /// </summary>
        public bool ThemNhanVien(nhanVienDTO nv)
        {
            try
            {
                // Validate business logic
                if (string.IsNullOrWhiteSpace(nv.HoTen))
                    throw new Exception("Tên nhân viên không được để trống!");

                if (nv.Luong < 0)
                    throw new Exception("Lương không hợp lệ!");

                // Kiểm tra email trùng trước khi thêm
                if (!string.IsNullOrEmpty(nv.Email) && dao.KiemTraEmailTonTai(nv.Email))
                    throw new Exception("Email đã tồn tại trong hệ thống!");

                return dao.ThemNhanVien(nv);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        public bool CapNhatNhanVien(nhanVienDTO nv)
        {
            try
            {
                // Validate business logic
                if (string.IsNullOrWhiteSpace(nv.HoTen))
                    throw new Exception("Tên nhân viên không được để trống!");

                if (nv.Luong < 0)
                    throw new Exception("Lương không hợp lệ!");

                // Kiểm tra email trùng (ngoại trừ chính nó)
                if (!string.IsNullOrEmpty(nv.Email))
                {
                    var nvCu = dao.LayTheoMa(nv.MaNhanVien);
                    if (nvCu != null && nvCu.Email != nv.Email && dao.KiemTraEmailTonTai(nv.Email))
                    {
                        throw new Exception("Email đã được sử dụng bởi nhân viên khác!");
                    }
                }

                return dao.CapNhatNhanVien(nv);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Xóa nhân viên
        /// </summary>
        public bool XoaNhanVien(int maNV)
        {
            try
            {
                // Có thể thêm logic kiểm tra ràng buộc ở đây
                // VD: Không cho xóa nếu nhân viên đang có hóa đơn/phiếu nhập

                return dao.XoaNhanVien(maNV);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi ràng buộc khóa ngoại
                if (ex.Message.Contains("foreign key") || ex.Message.Contains("FOREIGN KEY"))
                {
                    throw new Exception("Không thể xóa nhân viên này vì đang có dữ liệu liên quan (hóa đơn, phiếu nhập...)!");
                }
                throw ex;
            }
        }

        /// <summary>
        /// Kiểm tra email đã tồn tại chưa
        /// </summary>
        public bool KiemTraEmailTonTai(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return false;

                return dao.KiemTraEmailTonTai(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Tìm kiếm nhân viên theo từ khóa
        /// </summary>
        public BindingList<nhanVienDTO> TimKiemNhanVien(string tuKhoa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tuKhoa))
                    return LayDanhSach();

                return dao.TimKiemNhanVien(tuKhoa);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy thông tin nhân viên theo mã
        /// </summary>
        public nhanVienDTO LayTheoMa(int maNV)
        {
            try
            {
                return dao.LayTheoMa(maNV);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Kiểm tra số điện thoại đã tồn tại chưa (optional)
        /// </summary>
        public bool KiemTraSDTTonTai(string sdt)
        {
            try
            {
                if (string.IsNullOrEmpty(sdt))
                    return false;

                return dao.KiemTraSDTTonTai(sdt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Đếm tổng số nhân viên (optional - dùng cho thống kê)
        /// </summary>
        public int DemTongNhanVien()
        {
            try
            {
                return dao.LayDanhSach().Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy nhân viên mới nhất (optional)
        /// </summary>
        public nhanVienDTO LayNhanVienMoiNhat()
        {
            try
            {
                var list = dao.LayDanhSach();
                return list.OrderByDescending(x => x.MaNhanVien).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool kiemTraChuoiRong(string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Kiểm tra chuỗi có phải là số không
        /// </summary>
        public bool kiemTraSo(string str)
        {
            return decimal.TryParse(str, out _);
        }

        /// <summary>
        /// Kiểm tra số có dương không
        /// </summary>
        public bool kiemTraSoDuong(decimal so)
        {
            return so > 0;
        }

        // ==================== THÊM HÀM NHẬP EXCEL THÔNG MINH ====================

        /// <summary>
        /// Nhập danh sách nhân viên từ Excel (Kiểm tra trùng lặp thông minh)
        /// </summary>
        public string NhapExcelThongMinh(BindingList<nhanVienDTO> dsExcel)
        {
            int themMoi = 0;
            int trung = 0;
            int loi = 0;

            foreach (var nv in dsExcel)
            {
                try
                {
                    // Validate dữ liệu cơ bản
                    if (string.IsNullOrWhiteSpace(nv.HoTen))
                    {
                        loi++;
                        continue;
                    }

                    // Kiểm tra trùng email
                    if (!string.IsNullOrEmpty(nv.Email) && KiemTraEmailTonTai(nv.Email))
                    {
                        trung++;
                        continue;
                    }

                    // Kiểm tra trùng số điện thoại
                    if (!string.IsNullOrEmpty(nv.SoDienThoai) && KiemTraSDTTonTai(nv.SoDienThoai))
                    {
                        trung++;
                        continue;
                    }

                    // Kiểm tra lương hợp lệ
                    if (nv.Luong < 0)
                    {
                        loi++;
                        continue;
                    }

                    // Set ngày tạo
                    nv.NgayTao = DateTime.Now;

                    // Thêm nhân viên
                    bool kq = ThemNhanVien(nv);

                    if (kq)
                    {
                        themMoi++;
                    }
                    else
                    {
                        loi++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Lỗi thêm nhân viên '{nv.HoTen}': {ex.Message}");
                    loi++;
                }
            }

            return $"✅ Nhập Excel hoàn tất!\n\n" +
                   $"📊 Tổng: {dsExcel.Count} dòng\n" +
                   $"✔️ Thêm mới: {themMoi}\n" +
                   $"⚠️ Trùng lặp: {trung}\n" +
                   $"❌ Lỗi: {loi}";
        }
    }
}