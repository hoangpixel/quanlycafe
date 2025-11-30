using OfficeOpenXml;
using OfficeOpenXml.Style;
using DTO;
using System;
using System.Drawing;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;

namespace GUI.EXCEL
{
    public static class excelKhachHang
    {
        static excelKhachHang()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        // 🟢 Xuất Excel
        public static void Export(BindingList<khachHangDTO> dsKhachHang, string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                if (File.Exists(path))
                    File.Delete(path);

                using (ExcelPackage package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("Danh sách khách hàng");

                    // ==== Ghi header ====
                    string[] headers = { "Mã KH", "Tên khách hàng", "Số điện thoại", "Email" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        ws.Cells[1, i + 1].Value = headers[i];
                    }

                    // ==== Ghi dữ liệu ====
                    for (int i = 0; i < dsKhachHang.Count; i++)
                    {
                        var kh = dsKhachHang[i];
                        int row = i + 2;
                        ws.Cells[row, 1].Value = kh.MaKhachHang;
                        ws.Cells[row, 2].Value = kh.TenKhachHang;
                        ws.Cells[row, 3].Value = kh.SoDienThoai;
                        ws.Cells[row, 4].Value = kh.Email;
                    }

                    // ==== Định dạng (Style) ====
                    int totalRows = dsKhachHang.Count + 1;
                    int totalCols = headers.Length;

                    // Style Header
                    var headerRange = ws.Cells[1, 1, 1, totalCols];
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(Color.ForestGreen); // Màu khác SP chút cho dễ phân biệt
                    headerRange.Style.Font.Color.SetColor(Color.White);

                    // Border cho dữ liệu
                    if (totalRows > 1)
                    {
                        var dataRange = ws.Cells[1, 1, totalRows, totalCols];
                        dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    // Auto fit và Freeze pane
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    ws.View.FreezePanes(2, 1);

                    package.SaveAs(new FileInfo(path));
                }
                MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🟢 Nhập Excel → List<khachHangDTO>
        public static BindingList<khachHangDTO> Import(string filePath)
        {
            BindingList<khachHangDTO> list = new BindingList<khachHangDTO>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Không tìm thấy file Excel: " + filePath);

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets[0];
                if (ws == null) return list;

                int rowCount = ws.Dimension.End.Row;

                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        var kh = new khachHangDTO
                        {
                            MaKhachHang = int.TryParse(ws.Cells[row, 1].Text, out int ma) ? ma : 0,
                            TenKhachHang = ws.Cells[row, 2].Text,
                            SoDienThoai = ws.Cells[row, 3].Text,
                            Email = ws.Cells[row, 4].Text,
                            TrangThai = 1 // Mặc định khi import là đang hoạt động
                        };

                        // Kiểm tra dữ liệu rỗng cơ bản để tránh lỗi logic sau này
                        if (!string.IsNullOrWhiteSpace(kh.TenKhachHang))
                        {
                            list.Add(kh);
                        }
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