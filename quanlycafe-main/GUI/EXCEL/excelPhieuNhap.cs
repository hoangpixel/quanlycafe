using OfficeOpenXml;
using OfficeOpenXml.Style;
using DTO;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace GUI.EXCEL
{
    public static class excelPhieuNhap
    {
        static excelPhieuNhap()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public static void Export(BindingList<phieuNhapDTO> dsPhieu, string path)
        {
            if (File.Exists(path)) File.Delete(path);

            using (ExcelPackage package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Danh sách phi?u nh?p");

                // Header
                string[] headers = { "Mã PN", "Mã NCC", "Mã NV", "Ngày nh?p", "T?ng ti?n" };
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cells[1, i + 1].Value = headers[i];
                    ws.Cells[1, i + 1].Style.Font.Bold = true;
                    ws.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                    ws.Cells[1, i + 1].Style.Font.Color.SetColor(Color.White);
                }

                // Data
                for (int i = 0; i < dsPhieu.Count; i++)
                {
                    var item = dsPhieu[i];
                    ws.Cells[i + 2, 1].Value = item.MaPN;
                    ws.Cells[i + 2, 2].Value = item.MaNCC;
                    ws.Cells[i + 2, 3].Value = item.MaNV;
                    ws.Cells[i + 2, 4].Value = item.ThoiGian.ToString("dd/MM/yyyy HH:mm");
                    ws.Cells[i + 2, 5].Value = item.TongTien;
                }

                ws.Cells.AutoFitColumns();
                package.SaveAs(new FileInfo(path));
            }
        }
    }
}