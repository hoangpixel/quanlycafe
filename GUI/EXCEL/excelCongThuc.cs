using DTO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GUI.EXCEL
{
    public static class excelCongThuc
    {
        static excelCongThuc()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        // 🟢 Xuất Excel (ĐÃ SỬA: Thêm cột Mã Đơn Vị)
        public static void Export(BindingList<congThucDTO> dsCongThuc, string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                if (File.Exists(path)) File.Delete(path);

                using (ExcelPackage package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("Danh sách công thức");

                    // Header (Thêm cột Mã Đơn Vị ở cột 4)
                    string[] headers = { "Mã SP", "Mã nguyên liệu", "Số lượng cơ sở", "Mã Đơn Vị" };
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
                        ws.Cells[row, 4].Value = ct.MaDonViCoSo; // ✅ Thêm dòng này
                    }

                    // Format cho đẹp
                    var range = ws.Cells[1, 1, dsCongThuc.Count + 1, 4]; // Sửa thành 4 cột
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    // Style Header
                    var headerRange = ws.Cells[1, 1, 1, 4];
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                    headerRange.Style.Font.Color.SetColor(Color.White);
                    headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Cells.AutoFitColumns();
                    ws.View.FreezePanes(2, 1);

                    // Lưu file
                    package.SaveAs(new FileInfo(path));
                }

                // Hỏi mở file (Code chuẩn)
                DialogResult result = System.Windows.Forms.MessageBox.Show(
                    "Xuất file thành công!\nBạn có muốn mở file không?", "Thông báo",
                    System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(path);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Lỗi xuất file: " + ex.Message);
            }
        }

        // 🟢 Nhập Excel (ĐÃ SỬA: Dùng .Value và Đọc cột Đơn Vị)
        public static BindingList<congThucDTO> Import(string filePath)
        {
            BindingList<congThucDTO> list = new BindingList<congThucDTO>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Không tìm thấy file Excel: " + filePath);

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets[0];
                if (ws == null || ws.Dimension == null || ws.Dimension.End.Row < 2)
                    return list;

                int rowCount = ws.Dimension.End.Row;

                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        // 1. Lấy Value thô để tránh lỗi định dạng Text
                        object vMaSP = ws.Cells[row, 1].Value;
                        object vMaNL = ws.Cells[row, 2].Value;
                        object vSoLuong = ws.Cells[row, 3].Value;
                        object vMaDV = ws.Cells[row, 4].Value; // ✅ Đọc cột Đơn Vị

                        // 2. Kiểm tra dữ liệu rỗng
                        if (vMaSP == null || vMaNL == null) continue;

                        // 3. Convert an toàn
                        int maSP = Convert.ToInt32(vMaSP);
                        int maNL = Convert.ToInt32(vMaNL);
                        int maDV = vMaDV != null ? Convert.ToInt32(vMaDV) : 0; // Mặc định 0 nếu không có

                        // Xử lý Decimal an toàn
                        decimal soLuong = 0;
                        if (vSoLuong != null) decimal.TryParse(vSoLuong.ToString(), out soLuong);

                        var ct = new congThucDTO
                        {
                            MaSanPham = maSP,
                            MaNguyenLieu = maNL,
                            SoLuongCoSo = soLuong,
                            MaDonViCoSo = maDV, // ✅ Gán vào DTO
                            TrangThai = 1
                        };
                        list.Add(ct);
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