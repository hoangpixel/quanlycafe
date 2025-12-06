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

namespace GUI.GUI_CRUD
{
    public partial class selectExcelHoaDon : Form
    {
        private BindingList<hoaDonDTO> dsHoaDon;
        private DataGridView dgvHoaDon;
        private int? maHDSelected = null;
        public selectExcelHoaDon(BindingList<hoaDonDTO> ds, DataGridView dgv)
        {
            InitializeComponent();
            this.dsHoaDon = ds;
            this.dgvHoaDon= dgv;
            btnInPDF.Visible = false;

            // Theo dõi sự thay đổi chọn dòng (dù chọn ở form nào cũng bắt được)
            dgvHoaDon.SelectionChanged += (s, e) =>
            {
                btnInPDF.Visible = dgvHoaDon.SelectedRows.Count > 0;
                maHDSelected = dgvHoaDon.SelectedRows.Count > 0
                    ? Convert.ToInt32(dgvHoaDon.SelectedRows[0].Cells[0].Value)
                    : (int?)null;
            };

            // Gọi 1 lần để kiểm tra xem lúc mở có đang chọn dòng nào không
            if (dgvHoaDon.SelectedRows.Count > 0)
                btnInPDF.Visible = true;
        }
        
        private void CapNhatNutInPDF()
        {
            if (dgvHoaDon.CurrentRow != null && dgvHoaDon.CurrentRow.Selected)
            {
                maHDSelected = Convert.ToInt32(dgvHoaDon.CurrentRow.Cells["MaHD"].Value);
                btnInPDF.Enabled = true;
            }
            else
            {
                maHDSelected = null;
                btnInPDF.Enabled = false;
            }
        }

        private void btnInPDF_Click(object sender, EventArgs e)
        {
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
    }
}
