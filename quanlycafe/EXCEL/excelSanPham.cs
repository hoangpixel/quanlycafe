using OfficeOpenXml;
using OfficeOpenXml.Style;
using quanlycafe.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace quanlycafe.EXCEL
{
    public static class excelSanPham
    {
        static excelSanPham()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        // 🟢 Xuất Excel
        public static void Export(List<sanPhamDTO> dsSanPham, string path)
        {
            if (File.Exists(path))
                File.Delete(path);

            using (ExcelPackage package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Danh sách sản phẩm");

                // ==== Ghi header ====
                string[] headers = { "Mã SP", "Mã loại", "Tên SP", "Giá", "Hình" };
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cells[1, i + 1].Value = headers[i];
                }

                // ==== Ghi dữ liệu ====
                for (int i = 0; i < dsSanPham.Count; i++)
                {
                    var sp = dsSanPham[i];
                    int row = i + 2;
                    ws.Cells[row, 1].Value = sp.MaSP;
                    ws.Cells[row, 2].Value = sp.MaLoai;
                    ws.Cells[row, 3].Value = sp.TenSP;
                    ws.Cells[row, 4].Value = sp.Gia;
                    ws.Cells[row, 5].Value = sp.Hinh;
                }

                // ==== Định dạng cơ bản ====
                int totalRows = dsSanPham.Count + 1;
                int totalCols = headers.Length;

                // Header style
                var headerRange = ws.Cells[1, 1, 1, totalCols];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                headerRange.Style.Font.Color.SetColor(Color.White);

                // Border
                var dataRange = ws.Cells[1, 1, totalRows, totalCols];
                dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                // Căn trái toàn bộ
                for (int c = 1; c <= totalCols; c++)
                    ws.Column(c).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                ws.View.FreezePanes(2, 1);

                package.SaveAs(new FileInfo(path));
            }
        }

        // 🟢 Nhập Excel → List<sanPhamDTO>
        public static List<sanPhamDTO> Import(string filePath)
        {
            var list = new List<sanPhamDTO>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Không tìm thấy file Excel: " + filePath);

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets[0]; // Lấy sheet đầu tiên
                int rowCount = ws.Dimension.End.Row;

                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        var sp = new sanPhamDTO
                        {
                            MaSP = int.TryParse(ws.Cells[row, 1].Text, out int ma) ? ma : 0,
                            MaLoai = int.TryParse(ws.Cells[row, 2].Text, out int loai) ? loai : 0,
                            TenSP = ws.Cells[row, 3].Text,
                            Gia = float.TryParse(ws.Cells[row, 4].Text, out float g) ? g : 0,
                            Hinh = ws.Cells[row, 5].Text
                        };
                        list.Add(sp);
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
