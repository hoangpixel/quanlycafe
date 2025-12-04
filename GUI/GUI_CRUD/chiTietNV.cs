using BUS;
using DTO;
using FONTS;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class chiTietNV : Form
    {
        public nhanVienDTO nv;

        public chiTietNV(nhanVienDTO nv)
        {
            InitializeComponent();
            this.nv = nv;

            // Tùy chỉnh giao diện
            TuyChinhGiaoDien();
        }

        private void TuyChinhGiaoDien()
        {
            // Cấu hình form
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(245, 246, 250);

            // Tùy chỉnh GroupBox
            if (this.Controls.Find("groupBox1", true).Length > 0)
            {
                GroupBox gb = (GroupBox)this.Controls.Find("groupBox1", true)[0];
                gb.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                gb.ForeColor = Color.FromArgb(52, 73, 94);
            }

            // Tùy chỉnh nút Đóng
            if (this.Controls.Find("btnDong", true).Length > 0)
            {
                Button btn = (Button)this.Controls.Find("btnDong", true)[0];
                btn.BackColor = Color.FromArgb(52, 152, 219);
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                btn.Cursor = Cursors.Hand;
                btn.Size = new Size(150, 45);
            }
        }

        private void chiTietNV_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            // Hiển thị thông tin
            txtMaNV.Text = nv.MaNhanVien.ToString();
            txtTen.Text = nv.HoTen;
            txtSDT.Text = nv.SoDienThoai;
            txtEmail.Text = nv.Email;
            txtLuong.Text = nv.Luong.ToString("N0") + " VNĐ";
            txtNgayTao.Text = nv.NgayTao.ToString("dd/MM/yyyy HH:mm");

            // Khóa tất cả TextBox
            txtMaNV.ReadOnly = true;
            txtTen.ReadOnly = true;
            txtSDT.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtLuong.ReadOnly = true;
            txtNgayTao.ReadOnly = true;

            // Màu nền xám nhạt đẹp hơn
            Color mauXam = Color.FromArgb(236, 240, 241);
            txtMaNV.BackColor = mauXam;
            txtTen.BackColor = mauXam;
            txtSDT.BackColor = mauXam;
            txtEmail.BackColor = mauXam;
            txtLuong.BackColor = mauXam;
            txtNgayTao.BackColor = mauXam;

            // Màu chữ đậm hơn
            Color mauChu = Color.FromArgb(44, 62, 80);
            txtMaNV.ForeColor = mauChu;
            txtTen.ForeColor = mauChu;
            txtSDT.ForeColor = mauChu;
            txtEmail.ForeColor = mauChu;
            txtLuong.ForeColor = mauChu;
            txtNgayTao.ForeColor = mauChu;

            // Font chữ đẹp
            Font fontDep = new Font("Segoe UI", 10, FontStyle.Regular);
            txtMaNV.Font = fontDep;
            txtTen.Font = fontDep;
            txtSDT.Font = fontDep;
            txtEmail.Font = fontDep;
            txtLuong.Font = fontDep;
            txtNgayTao.Font = fontDep;

            // Border TextBox (nếu có)
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox txt = (TextBox)ctrl;
                    txt.BorderStyle = BorderStyle.FixedSingle;
                }
            }

            btnDong.Focus();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}