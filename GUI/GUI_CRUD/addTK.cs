using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BUS;
using DTO;

namespace GUI.GUI_CRUD
{
    public partial class addTK : Form
    {
        public addTK()
        {
            InitializeComponent();
            // Thêm sự kiện nếu chưa gán ở Designer
            this.Load += addTK_Load;
            this.chkHienMatKhau.Click += chkHienMatKhau_CheckedChanged;
            btnLuu.Click += btnLuu_Click;
            btnHuy.Click += btnHuy_Click;
        }

        // Sự kiện LOAD Form — load dữ liệu cho các combobox
        private void addTK_Load(object sender, EventArgs e)
        {
            try
            {
                taikhoanBUS bus = new taikhoanBUS();

                // Load danh sách vai trò
                cboVaiTro.DataSource = bus.LayDanhSachVaiTro();
                cboVaiTro.DisplayMember = "Value";
                cboVaiTro.ValueMember = "Key";

                // Load danh sách nhân viên chưa có tài khoản
                cboNhanVien.DataSource = bus.LayDanhSachNhanVienChuaCoTK();
                cboNhanVien.DisplayMember = "Value";
                cboNhanVien.ValueMember = "Key";

                cboNhanVien.SelectedIndex = -1;
                cboVaiTro.SelectedIndex = -1;
                chkTrangThai.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện của CheckBox "Hiện mật khẩu"
        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
            txtXacNhanMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }

        // Sự kiện của nút Lưu
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu nhập
                if (cboNhanVien.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboNhanVien.Focus();
                    return;
                }
                if (cboVaiTro.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn vai trò!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboVaiTro.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenDangNhap.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtMatKhau.Text) || txtMatKhau.Text.Length < 6)
                {
                    MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }
                if (txtMatKhau.Text != txtXacNhanMatKhau.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtXacNhanMatKhau.Focus();
                    return;
                }

                // Tạo DTO (Dữ liệu tài khoản)
                taikhoanDTO tk = new taikhoanDTO
                {
                    MANHANVIEN = (int)cboNhanVien.SelectedValue,
                    MAVAITRO = (int)cboVaiTro.SelectedValue,
                    TENDANGNHAP = txtTenDangNhap.Text.Trim(),
                    MATKHAU = txtMatKhau.Text,
                    TRANGTHAI = chkTrangThai.Checked
                };

                // Thêm tài khoản
                taikhoanBUS bus = new taikhoanBUS();
                if (bus.Them(tk))
                {
                    MessageBox.Show("Thêm tài khoản thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Gọi lại DataGridView khi đóng form
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm tài khoản thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện nút Hủy
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Các sự kiện khác nếu cần
        private void bigLabel1_Click(object sender, EventArgs e)
        {
            // Nếu có logic xử lý riêng cho label
        }
        private void txtTenSP_TextChanged(object sender, EventArgs e)
        {
            // Logic xử lý ô nhập liệu nếu cần
        }

        private void chkTrangThai_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void bigLabel1_Click_1(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chkHienMatKhau_CheckedChanged(object sender)
        {

        }

        private void btnHuy_Click_1(object sender, EventArgs e)
        {

        }

        private void btnLuu_Click_1(object sender, EventArgs e)
        {

        }

        private void txtXacNhanMatKhau_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtTenDangNhap_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cboVaiTro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}