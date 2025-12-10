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
        static excelPhieuNhap()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        public static void ExportHaiSheet(BindingList<phieuNhapDTO> dsPhieu, string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (File.Exists(path))
            {
                try { File.Delete(path); }
                catch
                {
                    MessageBox.Show("File Excel đang mở. Vui lòng đóng file trước khi xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    // --- SHEET 1: PHIẾU NHẬP ---
                    var ws1 = package.Workbook.Worksheets.Add("PhieuNhap");
                    string[] header1 = { "Mã PN", "Mã NCC", "Mã NV", "Ngày Lập", "Tổng Tiền", "Trạng Thái" };
                    for (int i = 0; i < header1.Length; i++)
                    {
                        ws1.Cells[1, i + 1].Value = header1[i];
                        FormatHeaderCell(ws1.Cells[1, i + 1], Color.DarkBlue);
                    }

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

                        // Giữ nguyên logic: Xuất thẳng giá trị 0 hoặc 1
                        ws1.Cells[row1, 6].Value = pn.TrangThai;

                        row1++;
                    }
                    ws1.Cells.AutoFitColumns();

                    // --- SHEET 2: CHI TIẾT ---
                    var ws2 = package.Workbook.Worksheets.Add("ChiTiet");
                    string[] header2 = { "Thuộc Mã PN", "Mã NL", "Tên Nguyên Liệu", "ĐVT", "Số Lượng", "Đơn Giá", "Thành Tiền" };
                    for (int i = 0; i < header2.Length; i++)
                    {
                        ws2.Cells[1, i + 1].Value = header2[i];
                        FormatHeaderCell(ws2.Cells[1, i + 1], Color.DarkGreen);
                    }

                    var listNL = new nguyenLieuBUS().LayDanhSach();
                    var listDV = new donViBUS().LayDanhSach();
                    ctPhieuNhapDAO ctDAO = new ctPhieuNhapDAO();

                    int row2 = 2;
                    foreach (var pn in dsPhieu)
                    {
                        var details = ctDAO.LayDanhSachChiTietTheoMaPN(pn.MaPN);
                        foreach (var ct in details)
                        {
                            var nl = listNL.FirstOrDefault(x => x.MaNguyenLieu == ct.MaNguyenLieu);
                            var dv = listDV.FirstOrDefault(x => x.MaDonVi == ct.MaDonVi);

                            ws2.Cells[row2, 1].Value = pn.MaPN;
                            ws2.Cells[row2, 2].Value = ct.MaNguyenLieu;
                            ws2.Cells[row2, 3].Value = nl?.TenNguyenLieu ?? "";
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

                // --- MỚI THÊM: Hỏi người dùng có muốn mở file không ---
                DialogResult result = MessageBox.Show("Xuất file Excel thành công!\nBạn có muốn mở file vừa xuất không?",
                                                      "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static List<PhieuNhapExcelRow> ImportHaiSheet(string filePath)
        {
            var listResult = new List<PhieuNhapExcelRow>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Không tìm thấy file: " + filePath);

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var ws1 = package.Workbook.Worksheets["PhieuNhap"];
                if (ws1 == null) throw new Exception("Thiếu sheet 'PhieuNhap'");

                var dictHeaders = new Dictionary<int, PhieuNhapExcelRow>();
                int rows1 = ws1.Dimension?.End.Row ?? 0;

                for (int r = 2; r <= rows1; r++)
                {
                    int mapn = ws1.Cells[r, 1].GetValue<int>();
                    if (mapn == 0) continue;

                    var p = new PhieuNhapExcelRow
                    {
                        MaPN_Excel = mapn,
                        MaNCC = ws1.Cells[r, 2].GetValue<int>(),
                        MaNV = ws1.Cells[r, 3].GetValue<int>(),
                        // Đọc thêm cột Trạng Thái (Cột 6)
                        TrangThai = ws1.Cells[r, 6].GetValue<int>()
                    };

                    var objDate = ws1.Cells[r, 4].Value;
                    if (objDate is DateTime dt) p.ThoiGian = dt;
                    else if (DateTime.TryParse(objDate?.ToString(), out DateTime dtParse)) p.ThoiGian = dtParse;
                    else p.ThoiGian = DateTime.Now;

                    if (!dictHeaders.ContainsKey(p.MaPN_Excel))
                        dictHeaders.Add(p.MaPN_Excel, p);
                }

                var ws2 = package.Workbook.Worksheets["ChiTiet"];
                if (ws2 == null) throw new Exception("Thiếu sheet 'ChiTiet'");

                int rows2 = ws2.Dimension?.End.Row ?? 0;

                for (int r = 2; r <= rows2; r++)
                {
                    int maPNLink = ws2.Cells[r, 1].GetValue<int>();

                    if (dictHeaders.ContainsKey(maPNLink))
                    {
                        var headerInfo = dictHeaders[maPNLink];

                        var item = new PhieuNhapExcelRow
                        {
                            MaPN_Excel = headerInfo.MaPN_Excel,
                            MaNCC = headerInfo.MaNCC,
                            MaNV = headerInfo.MaNV,
                            ThoiGian = headerInfo.ThoiGian,
                            TrangThai = headerInfo.TrangThai, // <--- Gán giá trị trạng thái vào row kết quả

                            MaNguyenLieu = ws2.Cells[r, 2].GetValue<int>(),
                            TenDonVi = ws2.Cells[r, 4].GetValue<string>(),
                            SoLuong = ws2.Cells[r, 5].GetValue<decimal>(),
                            DonGia = ws2.Cells[r, 6].GetValue<decimal>()
                        };

                        listResult.Add(item);
                    }
                }
            }
            return listResult;
        }

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