using ReaLTaiizor.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace quanlycafe.GUI
{
    public partial class mainGUI : Form
    {
        // 🔹 Form con đang được mở hiện tại
        private Form activeForm;

        // 🔹 Khu vực hiển thị nội dung (form con)
        private Panel panelMain;

        public mainGUI()
        {
            InitializeComponent();
            LoadLayout(); // Gọi hàm khởi tạo giao diện tổng
        }

        /// <summary>
        /// Hàm tạo layout tổng: gồm Navbar bên trái và vùng hiển thị bên phải
        /// </summary>
        private void LoadLayout()
        {
            // 🧱 1. Tạo vùng hiển thị chính
            panelMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(panelMain);

            // 🧭 2. Tạo thanh Navbar bên trái
            navbarGUI nav = new navbarGUI
            {
                Dock = DockStyle.Left,
                Width = 220
            };

            // 🧩 3. Gán sự kiện click cho từng nút trong Navbar
            nav.OnNavClick += (page) =>
            {
                switch (page)
                {
                    case "home":
                        OpenChildForm(new homeGUI(), panelMain);
                        break;
                    case "product":
                        OpenChildForm(new productGUI(), panelMain);
                        break;
                    default:
                        MessageBox.Show($"Chưa có trang cho '{page}'!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            };

            // Thêm Navbar vào giao diện chính
            this.Controls.Add(nav);

            // Mặc định hiển thị trang Home khi chạy
            OpenChildForm(new homeGUI(), panelMain);
        }

        /// <summary>
        /// Hàm mở form con bên trong panelMain
        /// </summary>
        /// <param name="form">Form con cần mở</param>
        /// <param name="container">Panel chứa form con</param>
        //private void OpenChildForm(UserControl control, Panel container)
        //{
        //    // Đóng form con hiện tại nếu có
        //    if (activeForm != null)
        //        activeForm.Close();

        //    activeForm = form;
        //    form.TopLevel = false;
        //    form.FormBorderStyle = FormBorderStyle.None;
        //    form.Dock = DockStyle.Fill;
        //    container.Controls.Clear(); // Xóa nội dung cũ
        //    container.Controls.Add(form);
        //    form.BringToFront();
        //    form.Show();
        //}
        private UserControl activeControl;
        private void OpenChildForm(UserControl control, Panel container)
        {
            if (activeControl != null)
                container.Controls.Remove(activeControl);

            activeControl = control;
            control.Dock = DockStyle.Fill;
            container.Controls.Add(control);
            control.BringToFront();
        }

        private void formGUI_Load(object sender, EventArgs e)
        {

        }

        private void mainGUI_Load(object sender, EventArgs e)
        {

        }

        // ⚙️ Auto-generated method (do Visual Studio tạo)
    }
}
