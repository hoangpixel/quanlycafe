using OfficeOpenXml;
using OfficeOpenXml.Style;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.ComponentModel;

namespace GUI.EXCEL
{
    public static class excelCongThuc
    {
        static excelCongThuc()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        // 🟢 Xuất Excel
        public static void Export(BindingList<congThucDTO> dsCongThuc, string path)
        {
            if (File.Exists(path))
                File.Delete(path);

            using (ExcelPackage package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Danh sách công thức");

                // Header
                string[] headers = { "Mã SP", "Mã nguyên liệu", "Số lượng cơ sở" };
                for (int i = 0; i < headers.Length; i++)
                    ws.Cells[1, i + 1].Value = headers[i];

                // Data
                for (int i = 0; i < dsCongThuc.Count; i++)
                {
                    var ct = dsCongThuc[i];
                    int row = i + 2;
                    ws.Cells[row, 1].Value = ct.MaSanPham;
                    ws.Cells[row, 2].Value = ct.MaNguyenLieu;
                    ws.Cells[row, 3].Value = ct.SoLuongCoSo;
                }

                // Format
                var range = ws.Cells[1, 1, dsCongThuc.Count + 1, 3];
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ws.Cells[1, 1, 1, 3].Style.Font.Bold = true;
                ws.Cells[1, 1, 1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[1, 1, 1, 3].Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                ws.Cells[1, 1, 1, 3].Style.Font.Color.SetColor(Color.White);
                ws.Cells.AutoFitColumns();
                ws.View.FreezePanes(2, 1);

                package.SaveAs(new FileInfo(path));
            }
        }

        // 🟢 Nhập Excel
        public static BindingList<congThucDTO> Import(string filePath)
        {
            BindingList<congThucDTO> list = new BindingList<congThucDTO>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Không tìm thấy file Excel: " + filePath);

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets[0];
                if (ws.Dimension == null)
                    throw new Exception("File Excel trống!");

                int rowCount = ws.Dimension.End.Row;

                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        var ct = new congThucDTO
                        {
                            MaSanPham = int.TryParse(ws.Cells[row, 1].Text, out int sp) ? sp : 0,
                            MaNguyenLieu = int.TryParse(ws.Cells[row, 2].Text, out int nl) ? nl : 0,
                            SoLuongCoSo = float.TryParse(ws.Cells[row, 3].Text, out float sl) ? sl : 0,
                            TrangThai = 1 // ✅ luôn bật trạng thái khi nhập
                        };
                        list.Add(ct);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Lỗi đọc dòng {row}: {ex.Message}");
                    }
                }
            }

            return list;
        }
    }
}
