using BUS;
using DTO;
using FONTS;
using System;
using System.Text.RegularExpressions; 
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class updateNhaCungCap : Form
    {
        public nhaCungCapDTO nccHienTai;
        private nhaCungCapBUS busNCC = new nhaCungCapBUS();

        public updateNhaCungCap(nhaCungCapDTO ncc)
        {
            InitializeComponent();
            this.nccHienTai = ncc;
        }

        private void updateNhaCungCap_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            txtTen.Text = nccHienTai.TenNCC;
            txtSDT.Text = nccHienTai.SoDienThoai;
            txtEmail.Text = nccHienTai.Email;
            txtDiaChi.Text = nccHienTai.DiaChi;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra Tên
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên nhà cung cấp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTen.Focus();
                return;
            }

            // 2. Kiểm tra Số điện thoại (Bắt buộc 10 số)
            string sdt = txtSDT.Text.Trim();
            if (string.IsNullOrWhiteSpace(sdt))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }
            if (!Regex.IsMatch(sdt, @"^\d{10}$"))
            {
                MessageBox.Show("Số điện thoại phải bao gồm đúng 10 chữ số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            // 3. Kiểm tra Email
            string email = txtEmail.Text.Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Vui lòng nhập Email!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email không đúng định dạng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            try
            {
                nccHienTai.TenNCC = txtTen.Text.Trim();
                nccHienTai.SoDienThoai = sdt;
                nccHienTai.Email = email;
                nccHienTai.DiaChi = txtDiaChi.Text.Trim();

                if (nccHienTai != null)
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}