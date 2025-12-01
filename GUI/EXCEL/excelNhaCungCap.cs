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
    public static class excelNhaCungCap
    {
        static excelNhaCungCap()
        {
            // Thiết lập License cho EPPlus (Bắt buộc cho phiên bản mới)
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        // 🟢 Xuất Excel Nhà Cung Cấp
        public static void Export(BindingList<nhaCungCapDTO> dsNhaCungCap, string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                if (File.Exists(path))
                    File.Delete(path);

                using (ExcelPackage package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("Danh sách nhà cung cấp");

                    // ==== Ghi header ====
                    // Đã thêm cột Địa chỉ so với khách hàng
                    string[] headers = { "Mã NCC", "Tên nhà cung cấp", "Số điện thoại", "Email", "Địa chỉ" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        ws.Cells[1, i + 1].Value = headers[i];
                    }

                    // ==== Ghi dữ liệu ====
                    for (int i = 0; i < dsNhaCungCap.Count; i++)
                    {
                        var ncc = dsNhaCungCap[i];
                        int row = i + 2; // Dòng bắt đầu ghi dữ liệu (sau header)

                        ws.Cells[row, 1].Value = ncc.MaNCC;
                        ws.Cells[row, 2].Value = ncc.TenNCC;
                        ws.Cells[row, 3].Value = ncc.SoDienThoai;
                        ws.Cells[row, 4].Value = ncc.Email;
                        ws.Cells[row, 5].Value = ncc.DiaChi;
                    }

                    // ==== Định dạng (Style) ====
                    int totalRows = dsNhaCungCap.Count + 1;
                    int totalCols = headers.Length;

                    // Style Header
                    var headerRange = ws.Cells[1, 1, 1, totalCols];
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue); // Đổi màu xanh đậm cho khác KH
                    headerRange.Style.Font.Color.SetColor(Color.White);

                    // Border cho toàn bộ bảng dữ liệu
                    if (totalRows > 1)
                    {
                        var dataRange = ws.Cells[1, 1, totalRows, totalCols];
                        dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    // Auto fit cột và Freeze pane (cố định dòng 1)
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    ws.View.FreezePanes(2, 1);

                    package.SaveAs(new FileInfo(path));
                }
                MessageBox.Show("Xuất file Excel Nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🟢 Nhập Excel → List<nhaCungCapDTO>
        public static BindingList<nhaCungCapDTO> Import(string filePath)
        {
            BindingList<nhaCungCapDTO> list = new BindingList<nhaCungCapDTO>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Không tìm thấy file Excel: " + filePath);

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets[0];
                if (ws == null) return list;

                int rowCount = ws.Dimension.End.Row;

                // Bắt đầu đọc từ dòng 2 (bỏ qua Header)
                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        var ncc = new nhaCungCapDTO
                        {
                            // Cột 1: Mã
                            MaNCC = int.TryParse(ws.Cells[row, 1].Text, out int ma) ? ma : 0,
                            // Cột 2: Tên
                            TenNCC = ws.Cells[row, 2].Text,
                            // Cột 3: SĐT
                            SoDienThoai = ws.Cells[row, 3].Text,
                            // Cột 4: Email
                            Email = ws.Cells[row, 4].Text,
                            // Cột 5: Địa chỉ
                            DiaChi = ws.Cells[row, 5].Text,

                            ConHoatDong = 1 // Mặc định Import vào là đang hoạt động
                        };

                        // Kiểm tra dữ liệu rỗng cơ bản
                        // Nếu Tên NCC rỗng thì bỏ qua dòng này (tránh lỗi dòng trống cuối file excel)
                        if (!string.IsNullOrWhiteSpace(ncc.TenNCC))
                        {
                            list.Add(ncc);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Ghi log ra Console nếu cần debug
                        Console.WriteLine($"⚠️ Lỗi đọc dòng {row}: {ex.Message}");
                    }
                }
            }

            return list;
        }
    }
}