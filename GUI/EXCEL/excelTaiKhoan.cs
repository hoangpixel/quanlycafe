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
    public static class excelTaiKhoan
    {
        static excelTaiKhoan()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        // 🟢 Xuất Excel
        public static void Export(BindingList<taikhoanDTO> dsTaiKhoan, string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                if (File.Exists(path))
                    File.Delete(path);

                using (ExcelPackage package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("Danh sách tài khoản");

                    // ==== Ghi header ====
                    string[] headers = { "Mã TK", "Tên Đăng Nhập", "Vai Trò", "Nhân Viên", "Mã NV", "Trạng Thái", "Ngày Tạo" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        ws.Cells[1, i + 1].Value = headers[i];
                    }

                    // ==== Ghi dữ liệu ====
                    for (int i = 0; i < dsTaiKhoan.Count; i++)
                    {
                        var tk = dsTaiKhoan[i];
                        int row = i + 2;
                        ws.Cells[row, 1].Value = tk.MAtaikHOAN;
                        ws.Cells[row, 2].Value = tk.TENDANGNHAP;
                        ws.Cells[row, 3].Value = tk.TENVAITRO;
                        ws.Cells[row, 4].Value = tk.TENNHANVIEN;
                        ws.Cells[row, 5].Value = tk.MANHANVIEN;
                        ws.Cells[row, 6].Value = tk.TRANGTHAI ? "Hoạt động" : "Bị khóa";
                        ws.Cells[row, 7].Value = tk.NGAYTAO.ToString("dd/MM/yyyy HH:mm");
                    }

                    // ==== Định dạng (Style) ====
                    int totalRows = dsTaiKhoan.Count + 1;
                    int totalCols = headers.Length;

                    // Style Header
                    var headerRange = ws.Cells[1, 1, 1, totalCols];
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(Color.Green); // Màu xanh lá cho Tài khoản
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

                    // Căn giữa cột Mã TK, Mã NV, Trạng Thái
                    if (totalRows > 1)
                    {
                        ws.Cells[2, 1, totalRows, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[2, 5, totalRows, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[2, 6, totalRows, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    // Auto fit và Freeze pane
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    ws.View.FreezePanes(2, 1);

                    package.SaveAs(new FileInfo(path));
                }

                MessageBox.Show(
                    $"Xuất Excel thành công!\n\n📊 Số lượng: {dsTaiKhoan.Count} tài khoản\n📁 Đường dẫn: {path}",
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

        // 🟢 Nhập Excel → BindingList<taikhoanDTO>
        public static BindingList<taikhoanDTO> Import(string filePath)
        {
            BindingList<taikhoanDTO> list = new BindingList<taikhoanDTO>();

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
                            string tenDangNhap = ws.Cells[row, 2].Text.Trim();
                            string vaiTro = ws.Cells[row, 3].Text.Trim();
                            string trangThai = ws.Cells[row, 6].Text.Trim();

                            // Kiểm tra dữ liệu rỗng cơ bản
                            if (string.IsNullOrWhiteSpace(tenDangNhap))
                            {
                                continue; // Bỏ qua dòng không có tên đăng nhập
                            }

                            // Parse Mã Nhân Viên
                            int maNV = 0;
                            if (!int.TryParse(ws.Cells[row, 5].Text, out maNV))
                            {
                                continue; // Bỏ qua nếu không có mã nhân viên hợp lệ
                            }

                            // Parse Vai Trò (1=Admin, 2=Nhân viên, 3=Khách hàng)
                            int maVaiTro = 2; // Mặc định là Nhân viên
                            if (vaiTro.Contains("Admin") || vaiTro.Contains("admin"))
                                maVaiTro = 1;
                            else if (vaiTro.Contains("Nhân viên") || vaiTro.Contains("nhan vien"))
                                maVaiTro = 2;
                            else if (vaiTro.Contains("Khách hàng") || vaiTro.Contains("khach hang"))
                                maVaiTro = 3;

                            // Parse Trạng Thái
                            bool trangThaiTK = !trangThai.ToLower().Contains("khóa");

                            var tk = new taikhoanDTO
                            {
                                TENDANGNHAP = tenDangNhap,
                                MATKHAU = "123456", // Mật khẩu mặc định
                                MANHANVIEN = maNV,
                                MAVAITRO = maVaiTro,
                                TRANGTHAI = trangThaiTK,
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