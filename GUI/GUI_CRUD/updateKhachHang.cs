using BUS;
using DTO;
using FONTS;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class updateKhachHang : Form
    {
        public khachHangDTO kh;
        private khachHangBUS bus = new khachHangBUS();

        public updateKhachHang(khachHangDTO kh)
        {
            InitializeComponent();
            this.kh = kh;
        }

        private void updateKhachHang_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
            txtTen.Text = kh.TenKhachHang;
            txtSDT.Text = kh.SoDienThoai;
            txtEmail.Text = kh.Email;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên khách hàng!");
                txtTen.Focus(); return;
            }

            string sdt = txtSDT.Text.Trim();
            if (!Regex.IsMatch(sdt, @"^\d{10}$"))
            {
                MessageBox.Show("SĐT phải là 10 số!");
                txtSDT.Focus(); return;
            }

            string email = txtEmail.Text.Trim();
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Email sai định dạng!");
                    return;
                }
            }

            if (txtTen.Text.Trim() != kh.TenKhachHang && bus.kiemTraTrungTen(txtTen.Text.Trim()))
            {
                MessageBox.Show("Tên khách hàng đã tồn tại!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTen.Focus();
                return;
            }

            if (txtSDT.Text.Trim() != kh.SoDienThoai && bus.kiemTraTrungSDT(txtSDT.Text.Trim()))
            {
                MessageBox.Show("Số điện thoại khách hàng đã tồn tại!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            // 3. Kiểm tra trùng Email
            if (txtEmail.Text.Trim() != kh.Email && bus.kiemTraTrungEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Email khách hàng đã tồn tại!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            kh.TenKhachHang = txtTen.Text.Trim();
            kh.SoDienThoai = sdt;
            kh.Email = email;

            MessageBox.Show("Cập nhật thành công!");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e) => this.Close();

        private void txtMa_TextChanged(object sender, EventArgs e)
        {

        }
    }
}