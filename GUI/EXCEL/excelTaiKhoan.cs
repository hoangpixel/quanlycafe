using OfficeOpenXml;
using OfficeOpenXml.Style;
using DTO;
using System;
using System.ComponentModel; // BindingList
using System.Drawing; // Color
using System.IO; // File

namespace GUI.EXCEL
{
    public static class excelTaiKhoan
    {
        static excelTaiKhoan()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        // 🟢 Xuất Excel (Export)
        public static void Export(BindingList<taikhoanDTO> dsTaiKhoan, string path)
        {
            if (File.Exists(path)) File.Delete(path);

            using (ExcelPackage package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Danh sách tài khoản");

                // ==== Ghi header ====
                string[] headers = { "Mã TK", "Mã NV", "Tên Đăng Nhập", "Mật Khẩu (Hash)", "Mã Vai Trò", "Trạng Thái", "Ngày Tạo" };
                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cells[1, i + 1].Value = headers[i];
                }

                // ==== Ghi dữ liệu ====
                for (int i = 0; i < dsTaiKhoan.Count; i++)
                {
                    var tk = dsTaiKhoan[i];
                    int row = i + 2;
                    ws.Cells[row, 1].Value = tk.MATAIKHOAN;
                    ws.Cells[row, 2].Value = tk.MANHANVIEN;
                    ws.Cells[row, 3].Value = tk.TENDANGNHAP;
                    ws.Cells[row, 4].Value = tk.MATKHAU;
                    ws.Cells[row, 5].Value = tk.MAVAITRO;

                    // --- SỬA Ở ĐÂY: Gán trực tiếp giá trị int ---
                    ws.Cells[row, 6].Value = tk.TRANGTHAI;

                    ws.Cells[row, 7].Value = tk.NGAYTAO.ToString("dd/MM/yyyy");
                }

                // ==== Style (Trang trí) ====
                var headerRange = ws.Cells[1, 1, 1, headers.Length];
                headerRange.Style.Font.Bold = true;
                headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.ForestGreen);
                headerRange.Style.Font.Color.SetColor(Color.White);

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                package.SaveAs(new FileInfo(path));
            }
        }

        // 🟢 Nhập Excel (Import)
        public static BindingList<taikhoanDTO> Import(string filePath)
        {
            BindingList<taikhoanDTO> list = new BindingList<taikhoanDTO>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Không tìm thấy file Excel: " + filePath);

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets[0];
                if (ws.Dimension == null) return list;

                int rowCount = ws.Dimension.End.Row;

                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(ws.Cells[row, 3].Text)) continue;

                        var tk = new taikhoanDTO
                        {
                            MATAIKHOAN = int.TryParse(ws.Cells[row, 1].Text, out int maTK) ? maTK : 0,
                            MANHANVIEN = int.TryParse(ws.Cells[row, 2].Text, out int maNV) ? maNV : 0,
                            TENDANGNHAP = ws.Cells[row, 3].Text.Trim(),
                            MATKHAU = ws.Cells[row, 4].Text.Trim(),
                            MAVAITRO = int.TryParse(ws.Cells[row, 5].Text, out int maVT) ? maVT : 0,

                            // --- SỬA Ở ĐÂY: Logic đọc text convert sang int ---
                            // Đọc text từ ô excel -> so sánh -> trả về 1 hoặc 0
                            TRANGTHAI = (ws.Cells[row, 6].Text == "1" || ws.Cells[row, 6].Text.ToLower() == "true") ? 1 : 0,

                            NGAYTAO = DateTime.Now
                        };
                        list.Add(tk);
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