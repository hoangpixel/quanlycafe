using FONTS;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using DTO;

namespace GUI.GUI_UC
{
    public partial class navbarGUI : UserControl
    {
        public event Action<string> OnNavClick;

        private Button currentButton;
        private Panel highlightPanel;
        private Label lblTenNhanVien;

        private Color colorBackground = Color.FromArgb(61, 34, 22);
        private Color colorButtonHover = Color.FromArgb(85, 55, 40);
        private Color colorActive = Color.FromArgb(100, 60, 40);
        private Color colorHighlight = Color.FromArgb(210, 180, 140);

        public navbarGUI()
        {
            InitializeComponent();
            InitializeLayout();
            LoadThongTinUser();
        }

        private void LoadThongTinUser()
        {
            if (DTO.Session.NhanVienHienTai != null)
            {
                SetTenNhanVien(DTO.Session.NhanVienHienTai.HoTen);
            }
            else
            {
                SetTenNhanVien("Admin");
            }
        }

        private void InitializeLayout()
        {
            this.Dock = DockStyle.Left;
            this.Width = 230;
            this.BackColor = colorBackground;

            highlightPanel = new Panel
            {
                Size = new Size(5, 50),
                BackColor = colorHighlight,
                Visible = false
            };
            this.Controls.Add(highlightPanel);

            Label lblHeader = new Label
            {
                Text = "XANGCAFE",
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = colorBackground
            };
            this.Controls.Add(lblHeader);

            Panel panelUser = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = colorBackground
            };

            Label lblUserIcon = new Label
            {
                Text = "👤",
                Font = new Font("Segoe UI", 24),
                ForeColor = colorHighlight,
                AutoSize = true,
                Location = new Point(95, 0)
            };

            lblTenNhanVien = new Label
            {
                Text = "Xin chào, Nhân viên",
                Dock = DockStyle.Bottom,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.LightGray
            };

            panelUser.Controls.Add(lblUserIcon);
            panelUser.Controls.Add(lblTenNhanVien);
            this.Controls.Add(panelUser);

            CheckAndAddButton("🏠 Trang chủ", "home", 3);

            CheckAndAddButton("☕ Sản phẩm", "sanpham", 1);
            CheckAndAddButton("📖 Công thức", "congthuc", 1);
            CheckAndAddButton("🌾 Nguyên liệu", "nguyenlieu", 1);

            CheckAndAddButton("🚚 Nhập hàng", "nhaphang", 2);

            CheckAndAddButton("🏭 Nhà cung cấp", "nhacungcap", 7);

            CheckAndAddButton("👥 Khách hàng", "khachhang", 6);

            AddNavButton("📈 Báo cáo", "thongke");

            CheckAndAddButton("🧑‍🍳 Nhân viên", "nhanvien", 4);
            CheckAndAddButton("🛡️ Tài khoản", "taikhoan", 5);
            CheckAndAddButton("🔐 Phân quyền", "phanquyen", 8);

            AddNavButton("👋 Thoát", "exit");
        }

        private void CheckAndAddButton(string text, string tag, int maQuyenCanThiet)
        {
            if (DTO.Session.TaiKhoanHienTai != null && DTO.Session.TaiKhoanHienTai.MAVAITRO == 1)
            {
                AddNavButton(text, tag);
                return;
            }

            if (DTO.Session.QuyenHienTai != null)
            {
                var quyen = DTO.Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == maQuyenCanThiet);

                if (quyen != null && quyen.CAN_READ == 1)
                {
                    AddNavButton(text, tag);
                }
            }
        }

        public void SetTenNhanVien(string ten)
        {
            if (lblTenNhanVien != null)
            {
                lblTenNhanVien.Text = "Hi, " + ten;
            }
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
                BackColor = colorBackground,
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;

            btn.MouseEnter += (s, e) =>
            {
                if (btn != currentButton)
                    btn.BackColor = colorButtonHover;
            };
            btn.MouseLeave += (s, e) =>
            {
                if (btn != currentButton)
                    btn.BackColor = colorBackground;
            };

            btn.Click += (s, e) => ActivateButton(btn);

            this.Controls.Add(btn);
            this.Controls.SetChildIndex(btn, 0); // Đảm bảo thứ tự
        }

        private void ActivateButton(Button btn)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button b)
                {
                    b.BackColor = colorBackground;
                    b.ForeColor = Color.White;
                }
            }

            currentButton = btn;
            btn.BackColor = colorActive;
            btn.ForeColor = colorHighlight;

            highlightPanel.Visible = true;
            highlightPanel.BringToFront();
            highlightPanel.Location = new Point(0, btn.Top);

            string tag = btn.Tag.ToString();
            if (tag == "exit") Application.Exit();
            else OnNavClick?.Invoke(tag);
        }

        private void navbarGUI_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        public void SelectButtonByTag(string tag)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn && btn.Tag != null && btn.Tag.ToString() == tag)
                {
                    ActivateButton(btn);
                    break;
                }
            }
        }

        // Thêm hàm này vào navbarGUI để MainGUI gọi
        public string LayTrangDauTienChoPhep()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn && btn.Tag.ToString() == "home")
                {
                    return "home"; // Nếu thấy nút home thì trả về ngay lập tức
                }
            }
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    string tag = btn.Tag.ToString();
                    if (tag != "exit")
                    {
                        return tag;
                    }
                }
            }

            return null;
        }
    }
}