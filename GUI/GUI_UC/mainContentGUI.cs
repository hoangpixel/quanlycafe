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

        // Dictionary chỉ lưu những trang ĐÃ ĐƯỢC TẠO
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

            // 1. Navbar bên trái
            nav = new navbarGUI()
            {
                Dock = DockStyle.Left,
                Width = 230 // Khớp với width trong navbarGUI
            };
            nav.OnNavClick += HandleNavClick;
            this.Controls.Add(nav);

            // 2. Panel hiển thị nội dung
            panelMain = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(panelMain);
            panelMain.BringToFront(); // Đảm bảo panel nội dung không bị che

            // 3. MẶC ĐỊNH VÀO TRANG HOME (BÁN HÀNG)
            // Kích hoạt giao diện nút Home trên Navbar
            nav.SelectButtonByTag("home");
            // Load nội dung trang Home
            HandleNavClick("home");
        }

        private void HandleNavClick(string page)
        {
            if (page == "exit")
            {
                Application.Exit();
                return;
            }

            // Tối ưu hiển thị để giảm nháy hình (Flicker)
            panelMain.SuspendLayout();

            try
            {
                // Kiểm tra xem trang này đã từng load chưa?
                if (!pages.ContainsKey(page))
                {
                    UserControl uc = null;

                    // LAZY LOADING: Chỉ new khi cần thiết
                    switch (page)
                    {
                        case "home": uc = new banHangGUI(); break;
                        case "nhaphang": uc = new nhapHangGUI(); break;
                        case "sanpham": uc = new sanPhamGUI(); break;
                        case "nguyenlieu": uc = new nguyenLieuGUI(); break;
                        case "congthuc": uc = new congThucGUI(); break;
                        case "khachhang": uc = new khachHangGUI(); break;
                        case "thongke": uc = new thongKeGUI(); break;
                        case "phanquyen": uc = new phanQuyenGUI(); break;
                        //case "nhanvien": uc = new nhanVienGUI(); break; // Nhớ thêm class này nếu có
                        default:
                            MessageBox.Show($"Đang phát triển trang: {page}", "Thông báo");
                            return;
                    }

                    if (uc != null)
                    {
                        uc.Dock = DockStyle.Fill;
                        pages.Add(page, uc); // Lưu vào bộ nhớ đệm
                        panelMain.Controls.Add(uc);
                    }
                }

                // Ẩn tất cả các trang khác (hoặc dùng BringToFront)
                // BringToFront đôi khi bị lag nếu quá nhiều control chồng lên nhau
                // Cách tốt nhất là BringToFront trang cần hiện
                if (pages.ContainsKey(page))
                {
                    pages[page].BringToFront();

                    // Mẹo nhỏ: Focus vào trang mới để trỏ chuột hoạt động đúng
                    pages[page].Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải trang: " + ex.Message);
            }
            finally
            {
                panelMain.ResumeLayout(); // Vẽ lại giao diện sau khi xử lý xong
            }
        }
    }
}