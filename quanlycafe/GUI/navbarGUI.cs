using System;
using System.Drawing;
using System.Windows.Forms;

namespace quanlycafe.GUI
{
    public partial class navbarGUI : UserControl
    {
        public event Action<string> OnNavClick;

        private Button currentButton;
        private Panel highlightPanel;

        public navbarGUI()
        {
            InitializeComponent();
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            this.Dock = DockStyle.Left;
            this.Width = 230;
            this.BackColor = Color.FromArgb(40, 42, 54);

            // 🔹 Thanh highlight bên trái
            highlightPanel = new Panel
            {
                Size = new Size(5, 50),
                BackColor = Color.MediumSlateBlue,
                Visible = false
            };
            this.Controls.Add(highlightPanel);

            // 🔹 Logo / Header
            Label lblHeader = new Label
            {
                Text = "☕  Cafe Manager",
                Dock = DockStyle.Top,
                Height = 70,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),
                ForeColor = Color.White
            };
            this.Controls.Add(lblHeader);

            // 🔹 Các nút menu
            AddNavButton("🏠  Trang chủ", "home");
            AddNavButton("📦  Sản phẩm", "sanpham");
            AddNavButton("👨‍🍳  Công thức", "congthuc");
            AddNavButton("🥣  Nguyên liệu", "nguyenlieu");
            AddNavButton("👨‍💼  Nhân viên", "nhanvien");
            AddNavButton("📊  Báo cáo", "baocao");
            AddNavButton("🚪  Thoát", "exit");
        }

        private void AddNavButton(string text, string tag)
        {
            Button btn = new Button
            {
                Text = text,
                Tag = tag,
                Height = 50,
                Dock = DockStyle.Top,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(25, 0, 0, 0),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 47, 59),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;

            // Hiệu ứng hover
            btn.MouseEnter += (s, e) =>
            {
                if (btn != currentButton)
                    btn.BackColor = Color.FromArgb(60, 62, 78);
            };
            btn.MouseLeave += (s, e) =>
            {
                if (btn != currentButton)
                    btn.BackColor = Color.FromArgb(45, 47, 59);
            };

            // Xử lý click
            btn.Click += (s, e) => ActivateButton(btn);

            this.Controls.Add(btn);
            this.Controls.SetChildIndex(btn, 1); // Giữ thứ tự menu đúng
        }

        private void ActivateButton(Button btn)
        {
            // Reset màu nút cũ
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button b)
                {
                    b.BackColor = Color.FromArgb(45, 47, 59);
                    b.ForeColor = Color.White;
                }
            }

            // Cập nhật nút đang chọn
            currentButton = btn;
            btn.BackColor = Color.MediumSlateBlue;
            btn.ForeColor = Color.WhiteSmoke;

            // Hiển thị highlight
            highlightPanel.Visible = true;
            highlightPanel.BringToFront();
            highlightPanel.Location = new Point(0, btn.Top);

            // Gọi sự kiện điều hướng
            string tag = btn.Tag.ToString();
            if (tag == "exit") Application.Exit();
            else OnNavClick?.Invoke(tag);
        }
    }
}
