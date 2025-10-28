using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.GUI_UC
{
    public partial class mainContentGUI : UserControl
    {
        private Panel panelMain;
        private navbarGUI nav;
        private Dictionary<string, UserControl> pages = new Dictionary<string, UserControl>();

        public mainContentGUI()
        {
            InitializeComponent();
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            // 🧱 1. Panel hiển thị nội dung
            panelMain = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(panelMain);

            // 🧭 2. Navbar bên trái
            nav = new navbarGUI()
            {
                Dock = DockStyle.Left,
                Width = 220
            };
            this.Controls.Add(nav);

            // 🧩 3. Gán sự kiện chuyển trang
            nav.OnNavClick += HandleNavClick;

            // ⚡ 4. Khởi tạo các trang
            pages["home"] = new homeGUI() { Dock = DockStyle.Fill };
            pages["sanpham"] = new sanPhamGUI() { Dock = DockStyle.Fill };
            pages["congthuc"] = new congThucGUI() { Dock = DockStyle.Fill };
            pages["nguyenlieu"] = new nguyenLieuGUI() { Dock = DockStyle.Fill };
            pages["banhang"] = new testGUI() { Dock = DockStyle.Fill };

            foreach (var p in pages.Values)
                panelMain.Controls.Add(p);

            // Hiển thị mặc định trang home
            pages["home"].BringToFront();
        }

        private void HandleNavClick(string page)
        {
            if (page == "exit")
            {
                Application.Exit();
                return;
            }

            if (pages.ContainsKey(page))
            {
                pages[page].BringToFront();
            }
            else
            {
                MessageBox.Show($"Chưa có trang cho '{page}'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
