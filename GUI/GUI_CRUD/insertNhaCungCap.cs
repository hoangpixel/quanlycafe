using BUS;
using DTO;
using FONTS;
using System;
using System.Text.RegularExpressions; // Thêm thư viện này để dùng Regex
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class insertNhaCungCap : Form
    {
        private nhaCungCapBUS busNCC = new nhaCungCapBUS();

        public insertNhaCungCap()
        {
            InitializeComponent();
        }

        private void insertNhaCungCap_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra Mã (Nhập tay)
            if (string.IsNullOrWhiteSpace(txtMa.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã nhà cung cấp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMa.Focus();
                return;
            }

            if (!int.TryParse(txtMa.Text.Trim(), out int maNCC))
            {
                MessageBox.Show("Mã nhà cung cấp phải là số nguyên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMa.Focus();
                return;
            }

            // --- CHECK TRÙNG MÃ ---
            if (busNCC.KiemTraTrungMa(maNCC))
            {
                MessageBox.Show($"Mã nhà cung cấp '{maNCC}' đã tồn tại trong hệ thống!", "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMa.SelectAll();
                txtMa.Focus();
                return;
            }

            // 2. Kiểm tra Tên
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên nhà cung cấp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTen.Focus();
                return;
            }

            // 3. Kiểm tra Số điện thoại (Bắt buộc 10 số)
            string sdt = txtSDT.Text.Trim();
            if (string.IsNullOrWhiteSpace(sdt))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }
            if (!Regex.IsMatch(sdt, @"^\d{10}$")) // Kiểm tra đúng 10 chữ số
            {
                MessageBox.Show("Số điện thoại phải bao gồm đúng 10 chữ số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            // 4. Kiểm tra Email (Đúng định dạng)
            string email = txtEmail.Text.Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Vui lòng nhập Email!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }
            // Regex đơn giản để check email: chuỗi + @ + chuỗi + . + chuỗi
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email không đúng định dạng (ví dụ: abc@gmail.com)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            try
            {
                nhaCungCapDTO ncc = new nhaCungCapDTO();

                ncc.MaNCC = maNCC;
                ncc.TenNCC = txtTen.Text.Trim();
                ncc.SoDienThoai = sdt;
                ncc.Email = email;
                ncc.DiaChi = txtDiaChi.Text.Trim();
                ncc.ConHoatDong = 1;

                if (busNCC.Them(ncc))
                {
                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtMa_TextChanged(object sender, EventArgs e) { }
    }
}