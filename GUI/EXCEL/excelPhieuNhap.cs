using BUS;
using DAO;
using DTO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GUI.EXCEL
{
    public static class excelPhieuNhap
    {
        // Constructor tĩnh để set License
        static excelPhieuNhap()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        // =============================================================
        // 🟢 EXPORT: XUẤT RA 2 SHEET RIÊNG BIỆT (Header & Detail)
        // =============================================================
        public static void ExportHaiSheet(BindingList<phieuNhapDTO> dsPhieu, string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            // Kiểm tra file đang mở
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {
                    MessageBox.Show("File Excel đang mở. Vui lòng đóng file trước khi xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            using (ExcelPackage package = new ExcelPackage())
            {
                // ---------------------------------------------------------
                // SHEET 1: DANH SÁCH PHIẾU NHẬP (HEADER)
                // ---------------------------------------------------------
                var ws1 = package.Workbook.Worksheets.Add("PhieuNhap");

                // Header Sheet 1
                string[] header1 = { "Mã PN", "Mã NCC", "Mã NV", "Ngày Lập", "Tổng Tiền", "Trạng Thái" };
                for (int i = 0; i < header1.Length; i++)
                {
                    ws1.Cells[1, i + 1].Value = header1[i];
                    FormatHeaderCell(ws1.Cells[1, i + 1], Color.DarkBlue);
                }

                // Data Sheet 1
                int row1 = 2;
                foreach (var pn in dsPhieu)
                {
                    ws1.Cells[row1, 1].Value = pn.MaPN;
                    ws1.Cells[row1, 2].Value = pn.MaNCC;
                    ws1.Cells[row1, 3].Value = pn.MaNhanVien;

                    ws1.Cells[row1, 4].Value = pn.ThoiGian;
                    ws1.Cells[row1, 4].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";

                    ws1.Cells[row1, 5].Value = pn.TongTien;
                    ws1.Cells[row1, 5].Style.Numberformat.Format = "#,##0";

                    ws1.Cells[row1, 6].Value = pn.TrangThai == 1 ? "Đã duyệt" : "Chưa duyệt";

                    row1++;
                }
                ws1.Cells.AutoFitColumns(); // Tự động giãn cột

                // ---------------------------------------------------------
                // SHEET 2: CHI TIẾT HÀNG HÓA (DETAILS)
                // ---------------------------------------------------------
                var ws2 = package.Workbook.Worksheets.Add("ChiTiet");

                // Header Sheet 2
                string[] header2 = { "Thuộc Mã PN", "Mã NL", "Tên Nguyên Liệu", "ĐVT", "Số Lượng", "Đơn Giá", "Thành Tiền" };
                for (int i = 0; i < header2.Length; i++)
                {
                    ws2.Cells[1, i + 1].Value = header2[i];
                    FormatHeaderCell(ws2.Cells[1, i + 1], Color.DarkGreen);
                }

                // Data Sheet 2
                int row2 = 2;

                // Load dữ liệu tham chiếu
                var listNL = new nguyenLieuBUS().LayDanhSach();
                var listDV = new donViBUS().LayDanhSach();
                ctPhieuNhapDAO ctDAO = new ctPhieuNhapDAO();

                foreach (var pn in dsPhieu)
                {
                    // Lấy chi tiết của phiếu này từ DB
                    var details = ctDAO.LayDanhSachChiTietTheoMaPN(pn.MaPN);

                    foreach (var ct in details)
                    {
                        var nl = listNL.FirstOrDefault(x => x.MaNguyenLieu == ct.MaNguyenLieu);
                        var dv = listDV.FirstOrDefault(x => x.MaDonVi == ct.MaDonVi);

                        ws2.Cells[row2, 1].Value = pn.MaPN; // Key để link với Sheet 1
                        ws2.Cells[row2, 2].Value = ct.MaNguyenLieu;
                        ws2.Cells[row2, 3].Value = nl?.TenNguyenLieu ?? "Không xác định";
                        ws2.Cells[row2, 4].Value = dv?.TenDonVi ?? "";

                        ws2.Cells[row2, 5].Value = ct.SoLuong;

                        ws2.Cells[row2, 6].Value = ct.DonGia;
                        ws2.Cells[row2, 6].Style.Numberformat.Format = "#,##0";

                        ws2.Cells[row2, 7].Value = ct.ThanhTien;
                        ws2.Cells[row2, 7].Style.Numberformat.Format = "#,##0";

                        row2++;
                    }
                }
                ws2.Cells.AutoFitColumns();

                // Lưu file
                package.SaveAs(new FileInfo(path));
            }
        }

        // =============================================================
        // 🟢 IMPORT: ĐỌC TỪ 2 SHEET VÀ GỘP LẠI THÀNH DANH SÁCH XỬ LÝ
        // =============================================================
        public static List<PhieuNhapExcelRow> ImportHaiSheet(string filePath)
        {
            var listResult = new List<PhieuNhapExcelRow>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Không tìm thấy file Excel tại đường dẫn: " + filePath);

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // 1. ĐỌC SHEET 1 (HEADER - PHIẾU NHẬP)
                var ws1 = package.Workbook.Worksheets["PhieuNhap"];
                if (ws1 == null)
                    throw new Exception("File Excel thiếu sheet 'PhieuNhap'. Vui lòng kiểm tra lại mẫu.");

                // Dictionary để lưu tạm Header (Key: Mã PN)
                var dictHeaders = new Dictionary<int, PhieuNhapExcelRow>();
                int rows1 = ws1.Dimension?.End.Row ?? 0;

                for (int r = 2; r <= rows1; r++)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(ws1.Cells[r, 1].Text)) continue;

                        var p = new PhieuNhapExcelRow();

                        // Parse thông tin Header
                        if (int.TryParse(ws1.Cells[r, 1].Text, out int mapn)) p.MaPN_Excel = mapn;
                        if (int.TryParse(ws1.Cells[r, 2].Text, out int mancc)) p.MaNCC = mancc;
                        if (int.TryParse(ws1.Cells[r, 3].Text, out int manv)) p.MaNV = manv;

                        // Parse Ngày tháng
                        if (DateTime.TryParse(ws1.Cells[r, 4].Text, out DateTime date)) p.ThoiGian = date;
                        else p.ThoiGian = DateTime.Now;

                        // Lưu vào Dict để lát nữa Sheet 2 dùng
                        if (!dictHeaders.ContainsKey(p.MaPN_Excel))
                        {
                            dictHeaders.Add(p.MaPN_Excel, p);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi đọc dòng {r} sheet PhieuNhap: {ex.Message}");
                    }
                }

                // 2. ĐỌC SHEET 2 (DETAIL - CHI TIẾT)
                var ws2 = package.Workbook.Worksheets["ChiTiet"];
                if (ws2 == null)
                    throw new Exception("File Excel thiếu sheet 'ChiTiet'. Vui lòng kiểm tra lại mẫu.");

                int rows2 = ws2.Dimension?.End.Row ?? 0;

                for (int r = 2; r <= rows2; r++)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(ws2.Cells[r, 1].Text)) continue;

                        // Lấy Mã PN để đối chiếu
                        if (!int.TryParse(ws2.Cells[r, 1].Text, out int maPNLink)) continue;

                        // Nếu Mã PN này có tồn tại bên Sheet 1
                        if (dictHeaders.ContainsKey(maPNLink))
                        {
                            var headerInfo = dictHeaders[maPNLink];

                            // Tạo một dòng dữ liệu phẳng (Gồm Header + Detail của dòng này)
                            var item = new PhieuNhapExcelRow
                            {
                                // Copy từ Header
                                MaPN_Excel = headerInfo.MaPN_Excel,
                                MaNCC = headerInfo.MaNCC,
                                MaNV = headerInfo.MaNV,
                                ThoiGian = headerInfo.ThoiGian,

                                // Đọc từ Detail hiện tại
                                TenDonVi = ws2.Cells[r, 4].Text.Trim() // Cột ĐVT (D)
                            };

                            if (int.TryParse(ws2.Cells[r, 2].Text, out int manl)) item.MaNguyenLieu = manl;
                            if (decimal.TryParse(ws2.Cells[r, 5].Text, out decimal sl)) item.SoLuong = sl;
                            if (decimal.TryParse(ws2.Cells[r, 6].Text, out decimal dg)) item.DonGia = dg;

                            listResult.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi đọc dòng {r} sheet ChiTiet: {ex.Message}");
                    }
                }
            }

            return listResult;
        }

        // =============================================================
        // HÀM PHỤ TRỢ: FORMAT STYLE HEADER
        // =============================================================
        private static void FormatHeaderCell(ExcelRange cell, Color bgColor)
        {
            cell.Style.Font.Bold = true;
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(bgColor);
            cell.Style.Font.Color.SetColor(Color.White);
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }
    }
}