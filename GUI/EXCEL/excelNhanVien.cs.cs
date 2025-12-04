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
    public static class excelNhanVien
    {
        static excelNhanVien()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        // 🟢 Xuất Excel
        public static void Export(BindingList<nhanVienDTO> dsNhanVien, string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                if (File.Exists(path))
                    File.Delete(path);

                using (ExcelPackage package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("Danh sách nhân viên");

                    // ==== Ghi header ====
                    string[] headers = { "Mã NV", "Họ Tên", "Số Điện Thoại", "Email", "Lương", "Ngày Tạo" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        ws.Cells[1, i + 1].Value = headers[i];
                    }

                    // ==== Ghi dữ liệu ====
                    for (int i = 0; i < dsNhanVien.Count; i++)
                    {
                        var nv = dsNhanVien[i];
                        int row = i + 2;
                        ws.Cells[row, 1].Value = nv.MaNhanVien;
                        ws.Cells[row, 2].Value = nv.HoTen;
                        ws.Cells[row, 3].Value = nv.SoDienThoai;
                        ws.Cells[row, 4].Value = nv.Email;
                        ws.Cells[row, 5].Value = nv.Luong;
                        ws.Cells[row, 5].Style.Numberformat.Format = "#,##0"; // Format số tiền
                        ws.Cells[row, 6].Value = nv.NgayTao.ToString("dd/MM/yyyy HH:mm");
                    }

                    // ==== Định dạng (Style) ====
                    int totalRows = dsNhanVien.Count + 1;
                    int totalCols = headers.Length;

                    // Style Header
                    var headerRange = ws.Cells[1, 1, 1, totalCols];
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(Color.DodgerBlue); // Màu xanh dương cho Nhân viên
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

                    // Căn giữa cột Mã NV
                    if (totalRows > 1)
                    {
                        ws.Cells[2, 1, totalRows, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    // Căn phải cột Lương
                    if (totalRows > 1)
                    {
                        ws.Cells[2, 5, totalRows, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    }

                    // Auto fit và Freeze pane
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    ws.View.FreezePanes(2, 1);

                    package.SaveAs(new FileInfo(path));
                }

                MessageBox.Show(
                    $"Xuất Excel thành công!\n\n📊 Số lượng: {dsNhanVien.Count} nhân viên\n📁 Đường dẫn: {path}",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Hỏi có muốn mở file không
                DialogResult result = MessageBox.Show(
                    "Bạn có muốn mở file Excel vừa xuất không?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(path);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🟢 Nhập Excel → BindingList<nhanVienDTO>
        public static BindingList<nhanVienDTO> Import(string filePath)
        {
            BindingList<nhanVienDTO> list = new BindingList<nhanVienDTO>();

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Không tìm thấy file Excel: " + filePath, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return list;
            }

            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets[0];
                    if (ws == null || ws.Dimension == null)
                    {
                        MessageBox.Show("File Excel rỗng hoặc không đúng định dạng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return list;
                    }

                    int rowCount = ws.Dimension.End.Row;

                    for (int row = 2; row <= rowCount; row++) // Bỏ qua header
                    {
                        try
                        {
                            string hoTen = ws.Cells[row, 2].Text.Trim();
                            string soDienThoai = ws.Cells[row, 3].Text.Trim();
                            string email = ws.Cells[row, 4].Text.Trim();

                            // Kiểm tra dữ liệu rỗng cơ bản
                            if (string.IsNullOrWhiteSpace(hoTen))
                            {
                                continue; // Bỏ qua dòng không có tên
                            }

                            var nv = new nhanVienDTO
                            {
                                // MaNhanVien sẽ được tạo tự động khi thêm vào DB
                                HoTen = hoTen,
                                SoDienThoai = soDienThoai,
                                Email = email,
                                Luong = float.TryParse(ws.Cells[row, 5].Text, out float luong) ? luong : 0,
                                NgayTao = DateTime.Now
                            };

                            list.Add(nv);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"⚠️ Lỗi đọc dòng {row}: {ex.Message}");
                        }
                    }
                }

                if (list.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu hợp lệ để nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đọc file Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return list;
        }
    }
}