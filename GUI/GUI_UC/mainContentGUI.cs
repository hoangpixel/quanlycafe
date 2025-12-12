using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            nav = new navbarGUI()
            {
                Dock = DockStyle.Left,
                Width = 230 
            };
            nav.OnNavClick += HandleNavClick;
            this.Controls.Add(nav);

            panelMain = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(panelMain);
            panelMain.BringToFront();

            string trangDauTien = nav.LayTrangDauTienChoPhep();

            if (!string.IsNullOrEmpty(trangDauTien))
            {
                nav.SelectButtonByTag(trangDauTien);
                HandleNavClick(trangDauTien);
            }
            else
            {
                Label lblThongBao = new Label();
                lblThongBao.Text = "Bạn không có quyền truy cập vào hệ thống.\nVui lòng liên hệ Admin.";
                lblThongBao.Dock = DockStyle.Fill;
                lblThongBao.TextAlign = ContentAlignment.MiddleCenter;
                lblThongBao.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                panelMain.Controls.Add(lblThongBao);
            }
        }

        private void HandleNavClick(string page)
        {
            if (page == "exit")
            {
                Application.Exit();
                return;
            }

            panelMain.SuspendLayout();

            try
            {
                if (!pages.ContainsKey(page))
                {
                    UserControl uc = null;

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
                        case "nhacungcap": uc = new nhaCungCapGUI(); break;
                        case "nhanvien": uc = new nhanVienGUI(); break;
                        case "taikhoan": uc = new taiKhoanGUI(); break;

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

                if (pages.ContainsKey(page))
                {
                    UserControl currentPage = pages[page];

                    currentPage.BringToFront();
                    currentPage.Focus();
                    if (page == "nguyenlieu")
                    {
                        if (currentPage is nguyenLieuGUI gui)
                        {
                            gui.LoadData();
                        }
                    }else if(page == "home")
                    {
                        if (currentPage is banHangGUI gui)
                        {
                            gui.LoadData();
                        }
                    }else if(page == "nhaphang")
                    {
                        if(currentPage is nhapHangGUI gui)
                        {
                            gui.LoadData();
                        }
                    }
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