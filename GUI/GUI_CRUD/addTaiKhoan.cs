using BUS;
using DAO;
using DTO;
using GUI.GUI_UC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class addtaikhoan : Form
    {
        private taikhoanBUS bustaikhoan = new taikhoanBUS();

        private ComboBox cboNhanVien;
        private ComboBox cboVaiTro;
        private TextBox txtTenDangNhap;
        private TextBox txtMatKhau;
        private TextBox txtXacNhanMatKhau;
        private CheckBox chkHienMatKhau;
        private CheckBox chkTrangThai;
        private Button btnLuu;
        private Button btnHuy;

        public addtaikhoan()
        {
            InitializeComponent();
            TaoGiaoDien();
            LoadDuLieu();
        }

        private void TaoGiaoDien()
        {
            this.Text = "Thêm Tài Khoản";
            this.Size = new Size(500, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // ===== TIÊU ĐỀ =====
            Label lblTitle = new Label
            {
                Text = "THÊM TÀI KHOẢN MỚI",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(20, 20),
                Size = new Size(440, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitle);

            int yPos = 80;
            int labelWidth = 150;
            int controlWidth = 280;
            int rowHeight = 60;

            // ===== NHÂN VIÊN =====
            Label lblNhanVien = new Label
            {
                Text = "Nhân viên:",
                Location = new Point(30, yPos),
                Size = new Size(labelWidth, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblNhanVien);

            cboNhanVien = new ComboBox
            {
                Location = new Point(190, yPos),
                Size = new Size(controlWidth, 30),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cboNhanVien);

            yPos += rowHeight;

            // ===== VAI TRÒ =====
            Label lblVaiTro = new Label
            {
                Text = "Vai trò:",
                Location = new Point(30, yPos),
                Size = new Size(labelWidth, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblVaiTro);

            cboVaiTro = new ComboBox
            {
                Location = new Point(190, yPos),
                Size = new Size(controlWidth, 30),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cboVaiTro);

            yPos += rowHeight;

            // ===== TÊN ĐĂNG NHẬP =====
            Label lblTenDangNhap = new Label
            {
                Text = "Tên đăng nhập:",
                Location = new Point(30, yPos),
                Size = new Size(labelWidth, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblTenDangNhap);

            txtTenDangNhap = new TextBox
            {
                Location = new Point(190, yPos),
                Size = new Size(controlWidth, 30),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtTenDangNhap);

            yPos += rowHeight;

            // ===== MẬT KHẨU =====
            Label lblMatKhau = new Label
            {
                Text = "Mật khẩu:",
                Location = new Point(30, yPos),
                Size = new Size(labelWidth, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblMatKhau);

            txtMatKhau = new TextBox
            {
                Location = new Point(190, yPos),
                Size = new Size(controlWidth, 30),
                Font = new Font("Segoe UI", 10),
                UseSystemPasswordChar = true
            };
            this.Controls.Add(txtMatKhau);

            yPos += rowHeight;

            // ===== XÁC NHẬN MẬT KHẨU =====
            Label lblXacNhan = new Label
            {
                Text = "Xác nhận mật khẩu:",
                Location = new Point(30, yPos),
                Size = new Size(labelWidth, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblXacNhan);

            txtXacNhanMatKhau = new TextBox
            {
                Location = new Point(190, yPos),
                Size = new Size(controlWidth, 30),
                Font = new Font("Segoe UI", 10),
                UseSystemPasswordChar = true
            };
            this.Controls.Add(txtXacNhanMatKhau);

            yPos += 50;

            // ===== HIỆN MẬT KHẨU =====
            chkHienMatKhau = new CheckBox
            {
                Text = "Hiện mật khẩu",
                Location = new Point(190, yPos),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 9)
            };
            chkHienMatKhau.CheckedChanged += (s, e) =>
            {
                txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
                txtXacNhanMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
            };
            this.Controls.Add(chkHienMatKhau);

            yPos += 30;

            // ===== TRẠNG THÁI =====
            chkTrangThai = new CheckBox
            {
                Text = "Kích hoạt tài khoản",
                Location = new Point(190, yPos),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 9),
                Checked = true
            };
            this.Controls.Add(chkTrangThai);

            yPos += 50;

            // ===== BUTTONS =====
            btnLuu = new Button
            {
                Text = "💾 Lưu",
                Location = new Point(190, yPos),
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLuu.FlatAppearance.BorderSize = 0;
            btnLuu.Click += btnLuu_Click;
            this.Controls.Add(btnLuu);

            btnHuy = new Button
            {
                Text = "❌ Hủy",
                Location = new Point(340, yPos),
                Size = new Size(130, 40),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnHuy.FlatAppearance.BorderSize = 0;
            btnHuy.Click += (s, e) => this.Close();
            this.Controls.Add(btnHuy);
        }

        private void LoadDuLieu()
        {
            try
            {
                // Load danh sách nhân viên chưa có tài khoản
                var dsNhanVien = bustaikhoan.LayDanhSachNhanVienChuaCoTK();
                cboNhanVien.DataSource = dsNhanVien;
                cboNhanVien.DisplayMember = "Value";
                cboNhanVien.ValueMember = "Key";

                // Load danh sách vai trò
                var dsVaiTro = bustaikhoan.LayDanhSachVaiTro();
                cboVaiTro.DataSource = dsVaiTro;
                cboVaiTro.DisplayMember = "Value";
                cboVaiTro.ValueMember = "Key";
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

                if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMatKhau.Focus();
                    return;
                }

                if (txtMatKhau.Text.Length < 6)
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

                // ===== ✅ ĐOẠN NÀY ĐÃ SỬA =====
                taikhoanDTO tk = new taikhoanDTO
                {
                    MANHANVIEN = (int)cboNhanVien.SelectedValue,
                    MAVAITRO = (int)cboVaiTro.SelectedValue,
                    TENDANGNHAP = txtTenDangNhap.Text.Trim(),
                    MATKHAU = txtMatKhau.Text,
                    TRANGTHAI = chkTrangThai.Checked
                };
                // ================================

                // Thêm vào DB
                if (bustaikhoan.Them(tk))
                {
                    MessageBox.Show("Thêm tài khoản thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}