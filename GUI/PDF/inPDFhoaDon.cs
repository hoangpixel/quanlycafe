using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.PDF
{
    public class inPDFhoaDon
    {
        private hoaDonDTO _hd;
        private BindingList<cthoaDonDTO> _chiTiet;
        private string _tenKhachHang;
        private string _tenNhanVien;

        // Font giống phiếu nhập của bạn
        private Font fontTieuDe = new Font("Times New Roman", 18, FontStyle.Bold);
        private Font fontBold = new Font("Times New Roman", 12, FontStyle.Bold);
        private Font fontNormal = new Font("Times New Roman", 11, FontStyle.Regular);
        private Font fontNho = new Font("Times New Roman", 10, FontStyle.Regular);
        private Font fontChuKy = new Font("Times New Roman", 11, FontStyle.Bold);
        private Pen penNet = new Pen(Color.Black, 1.2f);

        public void In(int maHD)
        {
            var bus = new hoaDonBUS();
            _hd = bus.TimTheoMa(maHD);
            if (_hd == null)
            {
                MessageBox.Show("Không tìm thấy hóa đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var ctBus = new hoaDonBUS();
            _chiTiet = new BindingList<cthoaDonDTO>(ctBus.LayChiTiet(maHD));

            _tenKhachHang = string.IsNullOrEmpty(_hd.TenKhachHang) || _hd.TenKhachHang == "Khách lẻ"
                ? "Khách lẻ" : _hd.TenKhachHang;
            _tenNhanVien = _hd.HoTen ?? "Nhân viên";

            PrintDocument pd = new PrintDocument();
            pd.DocumentName = $"HD{_hd.MaHD:00000}_{DateTime.Now:yyyyMMdd_HHmm}";
            pd.PrintPage += Pd_PrintPage;

            // In khổ A5 ngang (hoặc A4 dọc tùy bạn)
            pd.DefaultPageSettings.PaperSize = new PaperSize("A5", 827, 587); // A5 ngang
            pd.DefaultPageSettings.Landscape = true;
            pd.DefaultPageSettings.Margins = new Margins(40, 40, 50, 50);

            PrintPreviewDialog preview = new PrintPreviewDialog
            {
                Document = pd,
                Width = 1000,
                Height = 700
            };
            preview.ShowDialog();
        }

        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            float w = e.MarginBounds.Width;

            StringFormat center = new StringFormat { Alignment = StringAlignment.Center };
            StringFormat left = new StringFormat { Alignment = StringAlignment.Near };
            StringFormat right = new StringFormat { Alignment = StringAlignment.Far };

            // Logo + Tên quán
            y += 10;
            g.DrawString("QUÁN CAFÉ XANGCAFE", fontTieuDe, Brushes.Black, new RectangleF(x, y, w, 40), center);
            y += 35;
            g.DrawString("ĐC: 273 Đường An Duong Vuong, Q.5, TP.HCM", fontNormal, Brushes.Black, new RectangleF(x, y, w, 20), center);
            y += 20;
            g.DrawString("ĐT: 0909.123.456", fontNormal, Brushes.Black, new RectangleF(x, y, w, 20), center);
            y += 30;

            // Tiêu đề hóa đơn
            g.DrawString("HÓA ĐƠN BÁN HÀNG", new Font("Times New Roman", 20, FontStyle.Bold), Brushes.Black,
                new RectangleF(x, y, w, 40), center);
            y += 45;

            g.DrawString($"Số: HD{_hd.MaHD:00000}", fontBold, Brushes.Black, new RectangleF(x, y, w, 25), center);
            y += 25;
            g.DrawString($"Ngày: {_hd.ThoiGianTao:dd/MM/yyyy HH:mm}", fontNormal, Brushes.Black, new RectangleF(x, y, w, 25), center);
            y += 25;
            g.DrawString($"Bàn: {(_hd.MaBan == 0 ? "Mang về" : _hd.MaBan.ToString())}", fontNormal, Brushes.Black, x, y);
            g.DrawString($"Nhân viên: {_tenNhanVien}", fontNormal, Brushes.Black, w / 2, y);
            y += 35;

            // Bảng chi tiết
            float[] colW = { 50, 220, 60, 90, 110 };
            float rowH = 28;
            string[] header = { "STT", "Tên món", "SL", "Đơn giá", "Thành tiền" };

            float curX = x;
            for (int i = 0; i < header.Length; i++)
            {
                g.FillRectangle(Brushes.LightGray, curX, y, colW[i], rowH);
                g.DrawRectangle(Pens.Black, curX, y, colW[i], rowH);
                g.DrawString(header[i], fontBold, Brushes.Black, new RectangleF(curX, y, colW[i], rowH), center);
                curX += colW[i];
            }
            y += rowH;

            int stt = 1;
            foreach (var ct in _chiTiet)
            {
                curX = x;
                DrawCell(g, stt++.ToString(), curX, y, colW[0], rowH, center); curX += colW[0];
                DrawCell(g, ct.TenSP, curX, y, colW[1], rowH, left); curX += colW[1];
                DrawCell(g, ct.SoLuong.ToString(), curX, y, colW[2], rowH, center); curX += colW[2];
                DrawCell(g, ct.DonGia.ToString("#,##0"), curX, y, colW[3], rowH, right); curX += colW[3];
                DrawCell(g, ct.ThanhTien.ToString("#,##0"), curX, y, colW[4], rowH, right);
                y += rowH;
            }

            // Tổng tiền
            y += 10;
            g.DrawString("TỔNG CỘNG:", fontBold, Brushes.Black, new RectangleF(x, y, w * 0.6f, 35), right);
            g.DrawString(_hd.TongTien.ToString("#,##0") + " đ", new Font("Times New Roman", 16, FontStyle.Bold),
                Brushes.Red, new RectangleF(x + w * 0.6f, y, w * 0.4f, 35), right);

            y += 45;
            g.DrawString($"Tiền bằng chữ: {DocSoRaChu((long)_hd.TongTien)}", fontNormal, Brushes.Black, x, y);

            y += 50;
            g.DrawString("Cảm ơn quý khách - Hẹn gặp lại!", fontBold, Brushes.Black, new RectangleF(x, y, w, 30), center);
        }

        private void DrawCell(Graphics g, string text, float x, float y, float w, float h, StringFormat fmt)
        {
            g.DrawRectangle(Pens.Black, x, y, w, h);
            g.DrawString(text, fontNormal, Brushes.Black, new RectangleF(x + 5, y + 5, w - 10, h - 10), fmt);
        }

        // Hàm đổi số ra chữ (copy từ bạn luôn)
        public static string DocSoRaChu(long number)
        {
            if (number == 0) return "Không đồng";

            string[] dv = { "", "nghìn", "triệu", "tỷ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string result = "";
            int groupIndex = 0;

            do
            {
                int group = (int)(number % 1000);
                if (group > 0)
                {
                    string groupText = "";
                    int tram = group / 100;
                    int chuc = (group % 100) / 10;
                    int donvi = group % 10;

                    if (tram > 0)
                        groupText += cs[tram] + " trăm ";

                    if (chuc > 1)
                        groupText += cs[chuc] + " mươi ";
                    else if (chuc == 1)
                        groupText += "mười ";

                    if (donvi > 0)
                    {
                        if (chuc > 1 && donvi == 1) groupText += "mốt";
                        else if (chuc >= 1 && donvi == 5) groupText += "lăm";
                        else groupText += cs[donvi];
                    }
                    else if (chuc > 0 || tram > 0)
                        groupText += "";

                    result = groupText.Trim() + " " + dv[groupIndex] + " " + result;
                }
                number /= 1000;
                groupIndex++;
            } while (number > 0);

            result = char.ToUpper(result[0]) + result.Substring(1);
            return result.Trim() + " đồng.";
        }
    }
}
