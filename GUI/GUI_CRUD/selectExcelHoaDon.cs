using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using GUI.PDF;

namespace GUI.GUI_CRUD
{
    public partial class selectExcelHoaDon : Form
    {
        private BindingList<hoaDonDTO> dsHoaDon;
        private DataGridView dgvHoaDon;
        private int? maHDSelected = null;
        private int SafeInt(object value)
        {
            if (value == null) return 0;
            string s = value.ToString().Trim()
                .Replace(".", "")  
                .Replace(",", "")
                .Replace("₫", "")
                .Replace("đ", "")
                .Replace(" ", "");
            return int.TryParse(s, out int result) ? result : 0;
        }

        private decimal SafeDecimal(object value)
        {
            if (value == null) return 0;
            string s = value.ToString().Trim()
                .Replace(".", "")
                .Replace(",", ".")
                .Replace("₫", "")
                .Replace("đ", "")
                .Replace(" ", "");
            return decimal.TryParse(s, System.Globalization.NumberStyles.Any,
                                   System.Globalization.CultureInfo.InvariantCulture, out decimal result)
                   ? result : 0;
        }

        private string SafeStr(object value)
        {
            return value?.ToString()?.Trim() ?? "";
        }
        public selectExcelHoaDon(BindingList<hoaDonDTO> ds, DataGridView dgv)
        {
            InitializeComponent();
            this.dsHoaDon = ds;
            this.dgvHoaDon= dgv;
            btnInPDF.Visible = false;

            dgvHoaDon.SelectionChanged += (s, e) =>
            {
                if (dgvHoaDon.SelectedRows.Count > 0)
                {
                    // DÙNG CHỈ SỐ CỘT (cột 0 luôn là MaHD vì bạn tạo đầu tiên)
                    maHDSelected = Convert.ToInt32(dgvHoaDon.SelectedRows[0].Cells[0].Value);
                    btnInPDF.Visible = true;
                }
                else
                {
                    maHDSelected = null;
                    btnInPDF.Visible = false;
                }
            };
            if (dgvHoaDon.SelectedRows.Count > 0)
            {
                maHDSelected = Convert.ToInt32(dgvHoaDon.SelectedRows[0].Cells[0].Value);
                btnInPDF.Visible = true;
            }        
        }

        private void btnInPDF_Click(object sender, EventArgs e)
        {
            if (!maHDSelected.HasValue)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn để in!", "Thông báo");
                return;
            }
            var inHD = new inPDFhoaDon();
            inHD.In(maHDSelected.Value);
        }

        private void btnXuatHD_Click(object sender, EventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook|*.xlsx";
                sfd.FileName = $"HoaDon_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var package = new ExcelPackage())
                    {
                        // ==================== SHEET 1: HÓA ĐƠN ====================
                        var wsHoaDon = package.Workbook.Worksheets.Add("Danh sách hóa đơn");

                        // Lấy dữ liệu đã sắp xếp giảm dần theo MaHD
                        var dsHD = dsHoaDon
                            .OrderByDescending(hd => hd.MaHD)
                            .Select(hd => new
                            {
                                Mã_HĐ = hd.MaHD,
                                Bàn = hd.MaBan,
                                Thời_gian = hd.ThoiGianTao,
                                Tổng_tiền = hd.TongTien,
                                Khách_hàng = hd.TenKhachHang,
                                Nhân_viên = hd.HoTen,
                                Hình_thức_TT = hd.MaTT == 1 ?  "Chuyển khoản" : "Tiền mặt"
                            })
                            .ToList();

                        wsHoaDon.Cells["A1"].LoadFromCollection(dsHD, true);

                        // Định dạng header
                        using (var range = wsHoaDon.Cells[1, 1, 1, 7])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 112, 192));
                            range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                        // Định dạng cột
                        wsHoaDon.Column(1).Width = 12;  // Mã HĐ
                        wsHoaDon.Column(2).Width = 10;  // Bàn
                        wsHoaDon.Column(3).Style.Numberformat.Format = "dd/MM/yyyy HH:mm"; // Thời gian
                        wsHoaDon.Column(3).Width = 20;
                        wsHoaDon.Column(4).Style.Numberformat.Format = "#,##0 ₫";         // Tổng tiền
                        wsHoaDon.Column(4).Width = 18;
                        wsHoaDon.Column(5).Width = 20;  // Khách hàng
                        wsHoaDon.Column(6).Width = 20;  // Nhân viên
                        wsHoaDon.Column(7).Width = 18;

                        wsHoaDon.Cells.AutoFitColumns();


                        // ==================== SHEET 2: CHI TIẾT HÓA ĐƠN ====================
                        var wsCT = package.Workbook.Worksheets.Add("Chi tiết hóa đơn");

                        hoaDonBUS ctBus = new hoaDonBUS();
                        var dsCT = ctBus.LayTatCaChiTiet()
                            .OrderByDescending(ct => ct.maHD)  // Sắp xếp theo hóa đơn mới nhất trước
                            .ThenBy(ct => ct.MaSP)
                            .Select(ct => new
                            {
                                Mã_HĐ = ct.maHD,
                                Mã_SP = ct.MaSP,
                                Tên_sản_phẩm = ct.TenSP,
                                Số_lượng = ct.SoLuong,
                                Đơn_giá = ct.DonGia,
                                Thành_tiền = ct.ThanhTien
                            })
                            .ToList();

                        wsCT.Cells["A1"].LoadFromCollection(dsCT, true);

                        // Header đẹp
                        using (var range = wsCT.Cells[1, 1, 1, 6])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 112, 192));
                            range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        }

                        // Định dạng
                        wsCT.Column(1).Width = 12;
                        wsCT.Column(2).Width = 12;
                        wsCT.Column(3).Width = 35;
                        wsCT.Column(4).Width = 12;
                        wsCT.Column(5).Style.Numberformat.Format = "#,##0 ₫";
                        wsCT.Column(5).Width = 18;
                        wsCT.Column(6).Style.Numberformat.Format = "#,##0 ₫";
                        wsCT.Column(6).Width = 20;

                        wsCT.Cells.AutoFitColumns();

                        // Lưu file
                        package.SaveAs(new FileInfo(sfd.FileName));
                    }

                    MessageBox.Show($"Xuất Excel thành công!\nĐường dẫn: {sfd.FileName}",
                                  "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở file luôn cho tiện
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = sfd.FileName,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất Excel:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnNhapHD_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files|*.xlsx";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(new FileInfo(ofd.FileName)))
                    {
                        var wsHD = package.Workbook.Worksheets["Danh sách hóa đơn"] ?? package.Workbook.Worksheets[0];
                        var wsCT = package.Workbook.Worksheets["Chi tiết hóa đơn"] ?? package.Workbook.Worksheets[1];

                        var busHD = new hoaDonBUS();

                        for (int rowHD = 2; rowHD <= wsHD.Dimension.End.Row; rowHD++)
                        {
                            // DÙNG HÀM SAFE ĐỂ TRÁNH LỖI DẤU CHẤM, ₫, KHOẢNG TRẮNG
                            var hd = new hoaDonDTO
                            {
                                MaBan = SafeInt(wsHD.Cells[rowHD, 2].Value),           // Bàn
                                ThoiGianTao = DateTime.Now,                                 // Lấy giờ hiện tại
                                TongTien = SafeDecimal(wsHD.Cells[rowHD, 4].Value),      // 45.000 ₫ → 45000
                                TenKhachHang = SafeStr(wsHD.Cells[rowHD, 5].Value),          // Khách lẻ
                                HoTen = SafeStr(wsHD.Cells[rowHD, 6].Value),           // Nhân viên
                                MaTT = SafeInt(wsHD.Cells[rowHD, 7].Value)            // Hình thức TT
                            };

                            // Đọc chi tiết
                            var dsChiTiet = new BindingList<cthoaDonDTO>();
                            for (int rowCT = 2; rowCT <= wsCT.Dimension.End.Row; rowCT++)
                            {
                                int maHD_CT = SafeInt(wsCT.Cells[rowCT, 1].Value);
                                int maHD_HD = SafeInt(wsHD.Cells[rowHD, 1].Value);

                                if (maHD_CT == maHD_HD)
                                {
                                    dsChiTiet.Add(new cthoaDonDTO
                                    {
                                        MaSP = SafeInt(wsCT.Cells[rowCT, 2].Value),
                                        TenSP = SafeStr(wsCT.Cells[rowCT, 3].Value),
                                        SoLuong = SafeInt(wsCT.Cells[rowCT, 4].Value),
                                        DonGia = SafeDecimal(wsCT.Cells[rowCT, 5].Value),     // 15.000 ₫ → 15000
                                        ThanhTien = SafeDecimal(wsCT.Cells[rowCT, 6].Value)      // 45.000 ₫ → 45000
                                    });
                                }
                            }

                            busHD.ThemHoaDon(hd, dsChiTiet); // Dùng hàm bạn đã có
                        }
                    }

                    MessageBox.Show("Nhập Excel thành công!", "Thành công");
                    // Refresh lại danh sách ở form chính nếu cần
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
    }
}
