using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel; // Cần cho BindingList
using System.Linq;           // Cần cho FirstOrDefault

namespace BUS
{
    public class phieuNhapBUS
    {
        private phieuNhapDAO pnDAO = new phieuNhapDAO();
        private ctPhieuNhapDAO ctDAO = new ctPhieuNhapDAO();

        // Danh sách lưu cache để binding lên DataGridView
        public static BindingList<phieuNhapDTO> ds;

        // 1. LẤY DANH SÁCH
        public BindingList<phieuNhapDTO> LayDanhSach()
        {
            if (ds == null)
            {
                ds = pnDAO.LayDanhSach();
            }
            return ds;
        }

        // 2. THÊM PHIẾU NHẬP (Chỉ lưu nháp, TrangThai = 0, Chưa cộng kho)
        public int ThemPhieuNhap(phieuNhapDTO header, List<ctPhieuNhapDTO> details)
        {
            if (header.MaNCC <= 0) throw new Exception("Vui lòng chọn Nhà cung cấp.");
            if (details == null || details.Count == 0) throw new Exception("Danh sách hàng nhập đang trống.");

            // Gọi DAO (Transaction đã xử lý bên trong)
            int newPhieuID = pnDAO.ThemPhieuNhap(header, details);

            // Nếu thêm thành công -> Cập nhật cache UI
            if (newPhieuID > 0)
            {
                header.MaPN = newPhieuID;

                // Tính tổng tiền để hiện lên lưới
                header.TongTien = details.Sum(x => x.ThanhTien);

                // Set trạng thái mặc định là 0
                header.TrangThai = 0;

                // Thêm vào danh sách -> UI tự cập nhật
                LayDanhSach().Add(header);
            }

            return newPhieuID;
        }

        // 3. DUYỆT PHIẾU (Mới thêm - Quan trọng)
        public bool DuyetPhieuNhap(int maPN)
        {
            // Gọi DAO để cộng kho và đổi trạng thái DB
            bool result = pnDAO.DuyetPhieuNhap(maPN);

            if (result)
            {
                // Tìm phiếu trong cache để đổi trạng thái hiển thị
                var phieu = LayDanhSach().FirstOrDefault(x => x.MaPN == maPN);
                if (phieu != null)
                {
                    phieu.TrangThai = 1; // Đổi sang đã duyệt

                    // Báo hiệu cho DataGridView vẽ lại dòng này (để đổi màu chữ/nền)
                    ds.ResetItem(ds.IndexOf(phieu));
                }
            }
            return result;
        }

        // 5. CẬP NHẬT THÔNG TIN CƠ BẢN (NCC, NV)
        public bool CapNhatThongTinPhieu(int maPN, int maNCC, int maNhanVien)
        {
            bool result = pnDAO.CapNhatThongTin(maPN, maNCC, maNhanVien);

            if (result)
            {
                var phieuCanSua = LayDanhSach().FirstOrDefault(x => x.MaPN == maPN);
                if (phieuCanSua != null)
                {
                    phieuCanSua.MaNCC = maNCC;
                    phieuCanSua.MaNhanVien = maNhanVien; // Lưu ý: Property DTO là MaNhanVien hay MANHANVIEN thì sửa cho khớp

                    ds.ResetItem(ds.IndexOf(phieuCanSua));
                }
            }
            return result;
        }

        // 6. THÊM CHI TIẾT VÀO PHIẾU CŨ (Chỉ dùng cho phiếu chưa duyệt)
        public bool ThemChiTietVaoPhieuCu(int mapn, List<ctPhieuNhapDTO> details)
        {
            if (mapn <= 0) throw new Exception("Mã phiếu nhập không hợp lệ.");
            if (details == null || details.Count == 0) throw new Exception("Chưa chọn nguyên liệu để thêm.");

            // Kiểm tra trạng thái trên cache trước
            var phieuHeader = LayDanhSach().FirstOrDefault(x => x.MaPN == mapn);
            if (phieuHeader != null && phieuHeader.TrangThai == 1)
            {
                throw new Exception("Phiếu đã duyệt, không thể thêm chi tiết!");
            }

            bool result = pnDAO.ThemChiTietVaoPhieuCu(mapn, details);

            if (result)
            {
                if (phieuHeader != null)
                {
                    // Cập nhật lại tổng tiền trên giao diện
                    decimal tienMoiThem = details.Sum(x => x.ThanhTien);
                    phieuHeader.TongTien += tienMoiThem;
                    phieuHeader.ThoiGian = DateTime.Now; // DAO có update thời gian, nên BUS cũng update theo

                    ds.ResetItem(ds.IndexOf(phieuHeader));
                }
            }

            return result;
        }

        // --- CÁC HÀM PHỤ TRỢ ---

        // Trong class phieuNhapBUS
        public bool CapNhatPhieuNhap(phieuNhapDTO header, List<ctPhieuNhapDTO> details)
        {
            // Kiểm tra logic
            if (header.MaPN <= 0) throw new Exception("Mã phiếu không hợp lệ");
            if (details == null || details.Count == 0) throw new Exception("Danh sách chi tiết trống");

            // Kiểm tra trạng thái: CHỈ CHO SỬA KHI CHƯA DUYỆT (TrangThai = 0)
            // Nếu phiếu đã nhập kho rồi mà sửa số lượng thì kho sẽ bị lệch -> Rất nguy hiểm
            var phieuCu = LayDanhSach().FirstOrDefault(x => x.MaPN == header.MaPN);
            if (phieuCu != null && phieuCu.TrangThai == 1)
            {
                throw new Exception("Phiếu này đã nhập kho, không thể chỉnh sửa!");
            }

            // Tính lại tổng tiền
            header.TongTien = details.Sum(x => x.ThanhTien);

            // Gọi DAO
            bool result = pnDAO.CapNhatPhieuNhap(header, details);

            // Cập nhật lại Cache (UI)
            if (result && phieuCu != null)
            {
                phieuCu.MaNCC = header.MaNCC;
                phieuCu.MaNhanVien = header.MaNhanVien;
                phieuCu.TongTien = header.TongTien;
                phieuCu.ThoiGian = header.ThoiGian;

                ds.ResetItem(ds.IndexOf(phieuCu));
            }

            return result;
        }

        public List<ctPhieuNhapDTO> LayChiTiet(int mapn)
        {
            return ctDAO.LayDanhSachChiTietTheoMaPN(mapn);
        }
        public bool kiemTraChuoiRong(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                return true;
            }
            return false;
        }

        public bool XoaPhieuNhap(int maPN)
        {
            bool kq = pnDAO.XoaPhieuNhap(maPN);
            if (kq)
            {
                var tontai = ds.FirstOrDefault(x => x.MaPN == maPN);
                if (tontai != null)
                {
                    ds.Remove(tontai);
                }
            }
            return kq;
        }


        // Thêm đoạn này vào trong class phieuNhapBUS

        public void NhapExcelGop(List<DTO.PhieuNhapExcelRow> rawData)
        {
            if (rawData == null || rawData.Count == 0) return;

            var groups = rawData.GroupBy(x => x.MaPN_Excel);

            List<string> errors = new List<string>();
            int countSuccess = 0;

            var listNCC = new nhaCungCapBUS().LayDanhSach().ToList();
            var listNV = new nhanVienBUS().LayDanhSach().ToList();
            var listNL_Full = new nguyenLieuBUS().LayDanhSach().ToList();
            var listDonVi = new donViBUS().LayDanhSach().ToList();

            foreach (var group in groups)
            {
                var firstRow = group.First();
                int maPN_Excel = firstRow.MaPN_Excel;

                if (!listNCC.Any(x => x.MaNCC == firstRow.MaNCC))
                {
                    errors.Add($"Mã tạm {maPN_Excel}: Mã NCC {firstRow.MaNCC} không tồn tại.");
                    continue;
                }
                if (!listNV.Any(x => x.MaNhanVien == firstRow.MaNV))
                {
                    errors.Add($"Mã tạm {maPN_Excel}: Mã NV {firstRow.MaNV} không tồn tại.");
                    continue;
                }

                phieuNhapDTO header = new phieuNhapDTO
                {
                    MaPN = 0,
                    MaNCC = firstRow.MaNCC,
                    MaNhanVien = firstRow.MaNV,
                    ThoiGian = firstRow.ThoiGian,
                    TrangThai = 0,
                    TrangThaiXoa = 1,
                    TongTien = 0
                };

                List<ctPhieuNhapDTO> details = new List<ctPhieuNhapDTO>();
                bool hasDetailError = false;

                foreach (var row in group)
                {
                    if (row.MaNguyenLieu <= 0) continue;

                    var nlInfo = listNL_Full.FirstOrDefault(n => n.MaNguyenLieu == row.MaNguyenLieu);
                    if (nlInfo == null)
                    {
                        errors.Add($"Mã tạm {maPN_Excel}: Nguyên liệu ID {row.MaNguyenLieu} không tồn tại.");
                        hasDetailError = true; break;
                    }

                    int maDonViDB = nlInfo.MaDonViCoSo;
                    decimal heSo = 1;

                    if (!string.IsNullOrEmpty(row.TenDonVi))
                    {
                        var dvObj = listDonVi.FirstOrDefault(d => d.TenDonVi.Equals(row.TenDonVi.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (dvObj != null)
                        {
                            maDonViDB = dvObj.MaDonVi;
                            heSo = 1;
                        }
                    }

                    details.Add(new ctPhieuNhapDTO
                    {
                        MaNguyenLieu = row.MaNguyenLieu,
                        MaDonVi = maDonViDB,
                        SoLuong = row.SoLuong,
                        HeSo = heSo,
                        SoLuongCoSo = row.SoLuong * heSo,
                        DonGia = row.DonGia,
                        ThanhTien = row.SoLuong * row.DonGia
                    });
                }

                if (hasDetailError) continue;
                if (details.Count == 0)
                {
                    errors.Add($"Mã tạm {maPN_Excel}: Không có chi tiết sản phẩm.");
                    continue;
                }

                try
                {
                    if (ThemPhieuNhap(header, details) > 0)
                    {
                        countSuccess++;
                    }
                }
                catch (Exception ex)
                {
                    errors.Add($"Mã tạm {maPN_Excel}: Lỗi Insert - {ex.Message}");
                }
            }

            string msg = $"Đã nhập thành công: {countSuccess} phiếu.";
            if (errors.Count > 0)
            {
                msg += "\n\n--- CÁC LỖI BỎ QUA ---\n" + string.Join("\n", errors);
            }
            System.Windows.Forms.MessageBox.Show(msg, "Kết quả nhập Excel");
        }
    }
    }