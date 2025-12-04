using BUS;
using DTO;
using FONTS;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class suaNV : Form
    {
        public nhanVienDTO nv;

        public suaNV(nhanVienDTO nv)
        {
            InitializeComponent();
            this.nv = nv;
        }

        private void suaNV_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            txtTen.Text = nv.HoTen;
            txtSDT.Text = nv.SoDienThoai;
            txtEmail.Text = nv.Email;
            txtLuong.Text = nv.Luong.ToString();

            txtTen.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên nhân viên!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTen.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSDT.Text))
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("Vui lòng nhập email!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }

                if (!float.TryParse(txtLuong.Text, out float luong) || luong <= 0)
                {
                    MessageBox.Show("Lương phải là số dương!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLuong.Focus();
                    return;
                }

                nv.HoTen = txtTen.Text.Trim();
                nv.SoDienThoai = txtSDT.Text.Trim();
                nv.Email = txtEmail.Text.Trim();
                nv.Luong = luong;


                if (nv != null)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi",
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

        private void checkBox4_CheckedChanged(object sender)
        {

        }
    }
}