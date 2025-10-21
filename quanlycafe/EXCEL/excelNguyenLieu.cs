using OfficeOpenXml;
using OfficeOpenXml.Style;
using quanlycafe.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace quanlycafe.EXCEL
{
    public static class excelNguyenLieu
    {
        static excelNguyenLieu()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        // 🟢 Xuất danh sách nguyên liệu ra Excel
        public static void Export(List<nguyenLieuDTO> dsNguyenLieu, string path)
        {
            if (File.Exists(path))
                File.Delete(path);

            using (ExcelPackage package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Danh sách nguyên liệu");

                // ==== Header ====
                string[] headers = { "Mã NL", "Tên nguyên liệu", "Mã đơn vị cơ sở", "Tồn kho" };
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cells[1, i + 1].Value = headers[i];
                }

                // ==== Ghi dữ liệu ====
                for (int i = 0; i < dsNguyenLieu.Count; i++)
                {
                    var nl = dsNguyenLieu[i];
                    int row = i + 2;
                    ws.Cells[row, 1].Value = nl.MaNguyenLieu;
                    ws.Cells[row, 2].Value = nl.TenNguyenLieu;
                    ws.Cells[row, 3].Value = nl.MaDonViCoSo; // 🔹 mã đơn vị (int)
                    ws.Cells[row, 4].Value = nl.TonKho;
                }

                // ==== Style ====
                int totalRows = dsNguyenLieu.Count + 1;
                int totalCols = headers.Length;

                var headerRange = ws.Cells[1, 1, 1, totalCols];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                headerRange.Style.Font.Color.SetColor(Color.White);
                headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var dataRange = ws.Cells[1, 1, totalRows, totalCols];
                dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.View.FreezePanes(2, 1);

                package.SaveAs(new FileInfo(path));
            }
        }

        // 🟢 Nhập từ Excel (dạng MÃ đơn vị)
        public static List<nguyenLieuDTO> Import(string filePath)
        {
            var list = new List<nguyenLieuDTO>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Không tìm thấy file Excel: " + filePath);

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets[0];
                if (ws.Dimension == null)
                    throw new Exception("File Excel không có dữ liệu!");

                int rowCount = ws.Dimension.End.Row;

                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        var nl = new nguyenLieuDTO
                        {
                            MaNguyenLieu = int.TryParse(ws.Cells[row, 1].Text, out int ma) ? ma : 0,
                            TenNguyenLieu = ws.Cells[row, 2].Text,
                            MaDonViCoSo = int.TryParse(ws.Cells[row, 3].Text, out int madv) ? madv : 0,
                            TonKho = float.TryParse(ws.Cells[row, 4].Text, out float ton) ? ton : 0
                        };
                        list.Add(nl);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Lỗi dòng {row}: {ex.Message}");
                    }
                }
            }

            return list;
        }
    }
}
