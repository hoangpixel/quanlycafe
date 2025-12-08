using DTO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GUI.EXCEL
{
    public static class excelNguyenLieu
    {
        static excelNguyenLieu()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        // 🟢 Xuất danh sách nguyên liệu ra Excel
        public static void Export(BindingList<nguyenLieuDTO> dsNguyenLieu, string path)
        {
            // Kiểm tra đường dẫn rỗng
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                // Xóa file cũ nếu tồn tại (để ghi đè)
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
                        ws.Cells[row, 3].Value = nl.MaDonViCoSo;
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

                    // ⚠️ QUAN TRỌNG: PHẢI LƯU FILE TRƯỚC!
                    package.SaveAs(new FileInfo(path));
                } // Kết thúc using -> File đã chắc chắn nằm trên ổ cứng

                // ==== Giờ mới hỏi người dùng mở file ====
                DialogResult result = MessageBox.Show(
                    "Xuất file thành công!\nBạn có muốn mở file Excel vừa xuất không?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    // Kiểm tra chắc chắn file có tồn tại không trước khi mở
                    if (File.Exists(path))
                    {
                        System.Diagnostics.Process.Start(path);
                    }
                    else
                    {
                        MessageBox.Show("File không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
