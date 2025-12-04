using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class suaTK : Form
    {
        private taikhoanBUS bustaikhoan = new taikhoanBUS();
        private taikhoanDTO taikhoanHienTai;

        public suaTK(taikhoanDTO tk)
        {
            InitializeComponent();
            this.taikhoanHienTai = tk;

            // Gán sự kiện
            btnLuu.Click += btnLuu_Click;
            btnHuy.Click += btnHuy_Click;
      

            LoadDuLieu();
        }

        private void LoadDuLieu()
        {
            try
            {
                // 1. Load danh sách vai trò
                List<KeyValuePair<int, string>> dsVaiTro = bustaikhoan.LayDanhSachVaiTro();
                cboVaiTro.DataSource = dsVaiTro;
                cboVaiTro.DisplayMember = "Value";
                cboVaiTro.ValueMember = "Key";
                cboVaiTro.SelectedValue = taikhoanHienTai.MAVAITRO;

                // 2. Hiển thị tên nhân viên và DISABLE
                cboNhanVien.Items.Clear();
                cboNhanVien.Items.Add(new KeyValuePair<int, string>(
                    taikhoanHienTai.MANHANVIEN,
                    taikhoanHienTai.TENNHANVIEN
                ));
                cboNhanVien.DisplayMember = "Value";
                cboNhanVien.ValueMember = "Key";
                cboNhanVien.SelectedIndex = 0;
                cboNhanVien.Enabled = false;
                cboNhanVien.BackColor = Color.LightGray;

                // 3. Load thông tin tài khoản
                txtTenDangNhap.Text = taikhoanHienTai.TENDANGNHAP;
                chkTrangThai.Checked = taikhoanHienTai.TRANGTHAI;

                // 4. Để trống mật khẩu (giữ nguyên mật khẩu cũ nếu không nhập)
                txtMatKhau.Text = "";
                txtMatKhau.UseSystemPasswordChar = true;
                txtXacNhanMatKhau.Text = "";
                txtXacNhanMatKhau.UseSystemPasswordChar = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message, "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Cảnh báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenDangNhap.Focus();
                    return;
                }

                if (cboVaiTro.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn vai trò!", "Cảnh báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra mật khẩu nếu có nhập
                string matKhauMoi = txtMatKhau.Text.Trim();
                if (!string.IsNullOrEmpty(matKhauMoi))
                {
                    if (matKhauMoi.Length < 6)
                    {
                        MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Cảnh báo",
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMatKhau.Focus();
                        return;
                    }

                    if (matKhauMoi != txtXacNhanMatKhau.Text)
                    {
                        MessageBox.Show("Mật khẩu xác nhận không khớp!", "Cảnh báo",
                                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtXacNhanMatKhau.Focus();
                        return;
                    }
                }

                // Cập nhật thông tin
                taikhoanDTO tkCapNhat = new taikhoanDTO
                {
                    MAtaikHOAN = taikhoanHienTai.MAtaikHOAN,
                    MANHANVIEN = taikhoanHienTai.MANHANVIEN,
                    TENDANGNHAP = txtTenDangNhap.Text.Trim(),
                    MAVAITRO = (int)cboVaiTro.SelectedValue,
                    TRANGTHAI = chkTrangThai.Checked,
                    MATKHAU = string.IsNullOrEmpty(matKhauMoi) ? null : matKhauMoi
                };

                // Gọi BUS
                if (bustaikhoan.Sua(tkCapNhat))
                {
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Thành công",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật tài khoản thất bại!", "Lỗi",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
            txtXacNhanMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }

        private void bigLabel1_Click(object sender, EventArgs e)
        {
            // Designer generated
        }
    }
}