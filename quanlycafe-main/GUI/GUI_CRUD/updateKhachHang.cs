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
        private khachHangDTO khHienTai;
        private khachHangBUS bus = new khachHangBUS();

        public updateKhachHang(khachHangDTO kh)
        {
            InitializeComponent();
            this.khHienTai = kh;
        }

        private void updateKhachHang_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            txtMa.Text = khHienTai.MaKhachHang.ToString();
            txtTen.Text = khHienTai.TenKhachHang;
            txtSDT.Text = khHienTai.SoDienThoai;
            txtEmail.Text = khHienTai.Email;

            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Ngừng hoạt động", "Hoạt động" }); // Index 0 = 0, Index 1 = 1
            cboTrangThai.SelectedIndex = (khHienTai.TrangThai == 1) ? 1 : 0;

            txtMa.Enabled = false;
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
            // Check trùng SĐT (trừ chính khách hàng này ra)
            if (bus.KiemTraTrungSDT(sdt, khHienTai.MaKhachHang))
            {
                MessageBox.Show("SĐT đã tồn tại!");
                return;
            }

            string email = txtEmail.Text.Trim();
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Email sai định dạng!");
                    return;
                }
                // Check trùng Email (trừ chính khách hàng này ra)
                if (bus.KiemTraTrungEmail(email, khHienTai.MaKhachHang))
                {
                    MessageBox.Show("Email đã tồn tại!");
                    return;
                }
            }

            try
            {
                khHienTai.TenKhachHang = txtTen.Text.Trim();
                khHienTai.SoDienThoai = sdt;
                khHienTai.Email = email;
                khHienTai.TrangThai = (byte)cboTrangThai.SelectedIndex;

                if (bus.Sua(khHienTai))
                {
                    MessageBox.Show("Cập nhật thành công!");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else MessageBox.Show("Cập nhật thất bại!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e) => this.Close();

        private void txtMa_TextChanged(object sender, EventArgs e)
        {

        }
    }
}