using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.GUI_PRINT
{
    public class inPDFphieuNhap
    {
        private phieuNhapDTO _pn;
        private List<ctPhieuNhapDTO> _listChiTiet;
        private BindingList<nguyenLieuDTO> _cacheNL;
        private BindingList<donViDTO> _cacheDV;
        private string _tenNCC;
        private string _tenNV;

        // =================== FONT ===================
        private Font fontTieuDe = new Font("Times New Roman", 20, FontStyle.Bold);
        private Font fontHeaderBold = new Font("Times New Roman", 12, FontStyle.Bold);
        private Font fontThuong = new Font("Times New Roman", 12, FontStyle.Regular);
        private Font fontNghieng = new Font("Times New Roman", 12, FontStyle.Italic);
        private Font fontNho = new Font("Times New Roman", 10, FontStyle.Regular);
        private Font fontChuKy = new Font("Times New Roman", 11, FontStyle.Bold);

        Pen penBold = new Pen(Color.Black, 1.5f);
        Pen penThin = new Pen(Color.Black, 1.0f);

        public void InPhieu(int maPN)
        {
            phieuNhapBUS pnBus = new phieuNhapBUS();
            _pn = pnBus.LayDanhSach().FirstOrDefault(x => x.MaPN == maPN);
            if (_pn == null) { MessageBox.Show("Không tìm thấy phiếu nhập!"); return; }

            _listChiTiet = pnBus.LayChiTiet(maPN);
            _cacheNL = new nguyenLieuBUS().LayDanhSach();
            _cacheDV = new donViBUS().LayDanhSach();

            var ncc = new nhaCungCapBUS().LayDanhSach()
                        .FirstOrDefault(x => x.MaNCC == _pn.MaNCC);
            _tenNCC = ncc?.TenNCC ?? "................................";

            _tenNV = new nhanVienBUS().LayDanhSach()
                        .FirstOrDefault(x => x.MaNhanVien == _pn.MaNhanVien)?.HoTen ?? "";

            PrintDocument pd = new PrintDocument();

            string tenFile = $"PhieuNhap_PN{_pn.MaPN}_{DateTime.Now:yyyyMMdd_HHmm}";
            pd.DocumentName = tenFile;

            pd.PrintPage += Pd_PrintPage;
            pd.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);

            PrintPreviewDialog ppd = new PrintPreviewDialog()
            {
                Document = pd,
                Width = 950,
                Height = 800,
                StartPosition = FormStartPosition.CenterScreen
            };
            ppd.ShowDialog();
        }

        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            float width = e.MarginBounds.Width;
            float right = e.MarginBounds.Right;

            StringFormat center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            StringFormat left = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
            StringFormat rightAlign = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };

            // ========== HEADER ==========
            float headerLeftW = 300;
            g.DrawString("CÔNG TY TNHH ABC", fontHeaderBold, Brushes.Black,
                new RectangleF(x, y, headerLeftW, 18), left);
            y += 18;
            g.DrawString("Địa chỉ: ....................................", fontNho, Brushes.Black,
                new RectangleF(x, y, headerLeftW, 16), left);
            y += 16;
            g.DrawString("Điện thoại: .................................", fontNho, Brushes.Black,
                new RectangleF(x, y, headerLeftW, 16), left);

            float headerRightW = 200;
            float headerRightX = right - headerRightW;

            g.DrawString("Mẫu số 01 - VT", fontHeaderBold, Brushes.Black,
                new RectangleF(headerRightX, e.MarginBounds.Top, headerRightW, 20), center);
            g.DrawString("(Theo TT200/2014/TT-BTC)", fontNho, Brushes.Black,
                new RectangleF(headerRightX, e.MarginBounds.Top + 20, headerRightW, 16), center);
            g.DrawString("Ngày 22/12/2014)", fontNho, Brushes.Black,
                new RectangleF(headerRightX, e.MarginBounds.Top + 36, headerRightW, 16), center);

            y = e.MarginBounds.Top + 65;

            // ========== TIÊU ĐỀ ==========
            g.DrawString("PHIẾU NHẬP KHO", fontTieuDe, Brushes.Black,
                new RectangleF(x, y, width, 40), center);
            y += 40;

            g.DrawString($"Ngày {_pn.ThoiGian:dd/MM/yyyy}", fontNghieng, Brushes.Black,
                new RectangleF(x, y, width, 25), center);
            y += 22;

            g.DrawString($"Số: PN{_pn.MaPN:00000}", fontThuong, Brushes.Black,
                new RectangleF(x, y, width, 25), center);
            y += 35;

            // ========== THÔNG TIN ==========
            g.DrawString($"- Họ và tên nhà cung cấp: {_tenNCC}", fontThuong, Brushes.Black, x, y);
            y += 25;

            g.DrawString($"- Nhập tại kho: Kho chính", fontThuong, Brushes.Black, x, y);
            y += 30;

            // ========== BẢNG CHI TIẾT ==========
            float[] colWidths = { 50, 280, 70, 80, 120, 140 };
            float rowHeight = 32;

            string[] headers = { "STT", "Nguyên liệu nhập", "ĐVT", "SL", "Đơn giá", "Thành tiền" };

            float currentX = x;

            for (int i = 0; i < headers.Length; i++)
            {
                RectangleF rect = new RectangleF(currentX, y, colWidths[i], rowHeight);
                g.DrawRectangle(penThin, Rectangle.Round(rect));
                g.DrawString(headers[i], fontHeaderBold, Brushes.Black, rect, center);
                currentX += colWidths[i];
            }
            y += rowHeight;

            int stt = 1;
            foreach (var item in _listChiTiet)
            {
                if (y > e.MarginBounds.Bottom - 150) { e.HasMorePages = true; return; }

                currentX = x;
                string tenNL = _cacheNL.FirstOrDefault(n => n.MaNguyenLieu == item.MaNguyenLieu)?.TenNguyenLieu ?? "";
                string tenDV = _cacheDV.FirstOrDefault(d => d.MaDonVi == item.MaDonVi)?.TenDonVi ?? "";

                DrawCell(g, stt.ToString(), currentX, y, colWidths[0], rowHeight, center); currentX += colWidths[0];
                DrawCell(g, tenNL, currentX, y, colWidths[1], rowHeight, left); currentX += colWidths[1];
                DrawCell(g, tenDV, currentX, y, colWidths[2], rowHeight, center); currentX += colWidths[2];
                DrawCell(g, item.SoLuong.ToString("#,##0.##"), currentX, y, colWidths[3], rowHeight, rightAlign); currentX += colWidths[3];
                DrawCell(g, item.DonGia.ToString("#,##0"), currentX, y, colWidths[4], rowHeight, rightAlign); currentX += colWidths[4];
                DrawCell(g, item.ThanhTien.ToString("#,##0"), currentX, y, colWidths[5], rowHeight, rightAlign);

                y += rowHeight;
                stt++;
            }

            // Tổng cộng
            float wSumTitle = colWidths.Take(5).Sum();

            RectangleF rectCong = new RectangleF(x, y, wSumTitle, rowHeight);
            g.DrawRectangle(penThin, Rectangle.Round(rectCong));
            g.DrawString("Cộng:", fontHeaderBold, Brushes.Black,
                new RectangleF(x, y, wSumTitle - 5, rowHeight), rightAlign);

            RectangleF rectVal = new RectangleF(x + wSumTitle, y, colWidths[5], rowHeight);
            g.DrawRectangle(penThin, Rectangle.Round(rectVal));
            g.DrawString(_pn.TongTien.ToString("#,##0"), fontHeaderBold, Brushes.Black,
                new RectangleF(x + wSumTitle, y, colWidths[5] - 5, rowHeight), rightAlign);

            y += rowHeight + 15;

            // ================= TIỀN BẰNG CHỮ =================
            string tienChu = DocTienBangChu((long)_pn.TongTien);
            g.DrawString("Tổng số tiền (bằng chữ): " + tienChu, fontNghieng, Brushes.Black, x, y);
            y += 40;

            // ================= CHỮ KÝ =================
            // =================== 6. CHỮ KÝ (CHỈ CÒN 2 CỘT) ===================
            float colSignW = width / 2;   // ⭐ Chia 2 cột
            float signY = y + 10;

            string[] roles = { "Người gửi", "Người nhận" };
            string[] subRoles = { "(Ký, ghi rõ họ tên)", "(Ký, ghi rõ họ tên)" };

            // Vẽ 2 tiêu đề chữ ký
            for (int i = 0; i < 2; i++)
            {
                float signX = x + (i * colSignW);

                g.DrawString(roles[i], fontChuKy, Brushes.Black,
                    new RectangleF(signX, signY, colSignW, 20), center);

                g.DrawString(subRoles[i], fontNghieng, Brushes.Black,
                    new RectangleF(signX, signY + 22, colSignW, 20), center);
            }

            // =================== IN TÊN NGƯỜI GỬI & NGƯỜI NHẬN ===================

            // Tên người gửi (NCC)
            g.DrawString(_tenNCC, fontHeaderBold, Brushes.Black,
                new RectangleF(x, signY + 100, colSignW, 30), center);

            // Tên người nhận (nhập kho)
            g.DrawString(_tenNV, fontHeaderBold, Brushes.Black,
                new RectangleF(x + colSignW, signY + 100, colSignW, 30), center);

        }

        private void DrawCell(Graphics g, string text, float x, float y, float w, float h, StringFormat fmt)
        {
            RectangleF rect = new RectangleF(x, y, w, h);
            g.DrawRectangle(penThin, Rectangle.Round(rect));

            RectangleF textRect = new RectangleF(x + 3, y + 5, w - 6, h - 10);
            g.DrawString(text, fontThuong, Brushes.Black, textRect, fmt);
        }

        // =================== ĐỌC TIỀN RA CHỮ ===================
        public static string DocTienBangChu(long soTien)
        {
            if (soTien == 0) return "Không đồng";

            string[] DonVi = { "", "nghìn", "triệu", "tỷ", "nghìn tỷ", "triệu tỷ" };
            string ketQua = "";
            int i = 0;

            while (soTien > 0)
            {
                long baSo = soTien % 1000;
                if (baSo > 0)
                {
                    string str = DocBaSo(baSo);
                    ketQua = str + " " + DonVi[i] + " " + ketQua;
                }
                soTien /= 1000;
                i++;
            }

            ketQua = ketQua.Trim();
            ketQua = char.ToUpper(ketQua[0]) + ketQua.Substring(1);

            return ketQua + " đồng chẵn.";
        }

        static string DocBaSo(long baSo)
        {
            string[] ChuSo = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string kq = "";

            int tram = (int)(baSo / 100);
            int chuc = (int)((baSo % 100) / 10);
            int dv = (int)(baSo % 10);

            if (tram > 0) kq += ChuSo[tram] + " trăm";
            else if (chuc > 0 || dv > 0) kq += "không trăm";

            if (chuc > 1) kq += " " + ChuSo[chuc] + " mươi";
            else if (chuc == 1) kq += " mười";
            else if (dv > 0) kq += " linh";

            if (dv > 0)
            {
                if (dv == 1 && chuc > 1) kq += " mốt";
                else if (dv == 5 && chuc >= 1) kq += " lăm";
                else kq += " " + ChuSo[dv];
            }

            return kq.Trim();
        }
    }
}
