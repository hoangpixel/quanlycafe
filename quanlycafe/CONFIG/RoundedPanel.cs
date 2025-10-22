using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace quanlycafe.GUI
{
    public class RoundedPanel : Panel
    {
        public int BorderRadius { get; set; } = 15;
        public int BorderSize { get; set; } = 2;
        public Color BorderColor { get; set; } = Color.Silver;

        public RoundedPanel()
        {
            this.BackColor = Color.White;
            this.DoubleBuffered = true;
            this.Resize += (s, e) => this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Width <= 0 || Height <= 0)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rectSurface = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            using (GraphicsPath path = GetRoundedPath(rectSurface, BorderRadius))
            {
                // 💡 Chỉ set Region khi đang chạy thật (đảm bảo handle đã tồn tại)
                if (!IsInDesignMode())
                {
                    this.Region = new Region(path);
                }

                // 🎨 Nền panel
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                    e.Graphics.FillPath(brush, path);

                // 🩶 Viền — vẫn hiển thị trong Designer
                Color borderColor = IsInDesignMode() ? Color.LightGray : BorderColor;
                using (Pen penBorder = new Pen(borderColor, BorderSize))
                {
                    penBorder.Alignment = PenAlignment.Inset;
                    e.Graphics.DrawPath(penBorder, path);
                }
            }
        }

        // 🧩 Cách xác định đúng DesignMode (chuẩn hơn thuộc tính mặc định)
        private bool IsInDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime
                || System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv";
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curve = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curve, curve, 180, 90);
            path.AddArc(rect.Right - curve, rect.Y, curve, curve, 270, 90);
            path.AddArc(rect.Right - curve, rect.Bottom - curve, curve, curve, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curve, curve, curve, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
