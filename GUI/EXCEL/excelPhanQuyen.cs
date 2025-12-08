using OfficeOpenXml;
using OfficeOpenXml.Style;
using DTO;
using System;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace GUI.EXCEL
{
    public static class excelPhanQuyen
    {
        static excelPhanQuyen()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        // 🟢 Xuất Excel (Đã thêm: Hỏi mở file sau khi xuất)
        public static void Export(BindingList<phanquyenDTO> dsPhanQuyen, string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                // 1. Xóa file cũ nếu tồn tại
                if (File.Exists(path)) File.Delete(path);

                using (ExcelPackage package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("PhanQuyen");

                    // ==== 1. Ghi Header ====
                    string[] headers = { "Mã Vai Trò", "Mã Quyền", "READ", "CREATE", "UPDATE", "DELETE" };

                    for (int i = 0; i < headers.Length; i++)
                    {
                        ws.Cells[1, i + 1].Value = headers[i];
                    }

                    // ==== 2. Ghi Dữ liệu ====
                    for (int i = 0; i < dsPhanQuyen.Count; i++)
                    {
                        var pq = dsPhanQuyen[i];
                        int row = i + 2;

                        ws.Cells[row, 1].Value = pq.MaVaiTro;
                        ws.Cells[row, 2].Value = pq.MaQuyen;

                        // Xuất thẳng số nguyên (0 hoặc 1)
                        ws.Cells[row, 3].Value = pq.CAN_READ;
                        ws.Cells[row, 4].Value = pq.CAN_CREATE;
                        ws.Cells[row, 5].Value = pq.CAN_UPDATE;
                        ws.Cells[row, 6].Value = pq.CAN_DELETE;
                    }

                    // ==== 3. Định dạng ====
                    var headerRange = ws.Cells[1, 1, 1, headers.Length];
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                    // Căn giữa dữ liệu
                    if (dsPhanQuyen.Count > 0)
                    {
                        var dataRange = ws.Cells[2, 1, dsPhanQuyen.Count + 1, headers.Length];
                        dataRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    ws.Cells[ws.Dimension.Address].AutoFitColumns();

                    // 🔥 QUAN TRỌNG: Lưu file xuống ổ cứng
                    package.SaveAs(new FileInfo(path));
                }

                // ==== 4. Hỏi người dùng có muốn mở file không ====
                DialogResult result = MessageBox.Show(
                    "Xuất Excel thành công!\nBạn có muốn mở file vừa xuất không?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    if (File.Exists(path))
                    {
                        System.Diagnostics.Process.Start(path);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy file!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🟢 Nhập Excel (Giữ nguyên logic của bạn)
        public static BindingList<phanquyenDTO> Import(string filePath)
        {
            BindingList<phanquyenDTO> list = new BindingList<phanquyenDTO>();

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Không tìm thấy file: " + filePath);

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets[0];
                if (ws == null || ws.Dimension == null) return list;

                int rowCount = ws.Dimension.End.Row;

                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        var pq = new phanquyenDTO();

                        // Cột 1: Mã Vai Trò
                        pq.MaVaiTro = int.TryParse(ws.Cells[row, 1].Text, out int mvt) ? mvt : 0;

                        // Cột 2: Mã Quyền
                        pq.MaQuyen = int.TryParse(ws.Cells[row, 2].Text, out int mq) ? mq : 0;

                        // Cột 3, 4, 5, 6: Các quyền (0/1)
                        pq.CAN_READ = ParseInt(ws.Cells[row, 3].Text);
                        pq.CAN_CREATE = ParseInt(ws.Cells[row, 4].Text);
                        pq.CAN_UPDATE = ParseInt(ws.Cells[row, 5].Text);
                        pq.CAN_DELETE = ParseInt(ws.Cells[row, 6].Text);

                        if (pq.MaVaiTro != 0 && pq.MaQuyen != 0)
                        {
                            list.Add(pq);
                        }
                    }
                    catch
                    {
                        // Bỏ qua dòng lỗi
                    }
                }
            }
            return list;
        }

        // Hàm phụ trợ
        private static int ParseInt(string text)
        {
            if (int.TryParse(text, out int val))
            {
                return val == 1 ? 1 : 0;
            }
            if (text.ToLower().Contains("true") || text.ToLower().Contains("có")) return 1;
            return 0;
        }
    }
}