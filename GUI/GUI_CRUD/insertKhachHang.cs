using BUS;
using DTO;
using FONTS;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class insertKhachHang : Form
    {
        private khachHangBUS bus = new khachHangBUS();
        public khachHangDTO kh;

        public insertKhachHang(khachHangDTO kh)
        {
            InitializeComponent();
            this.kh = kh;
        }

        private void insertKhachHang_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Validate Tên
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên khách hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTen.Focus(); return;
            }

            // 2. Validate SĐT
            string sdt = txtSDT.Text.Trim();
            if (string.IsNullOrWhiteSpace(sdt) || !Regex.IsMatch(sdt, @"^\d{10}$"))
            {
                MessageBox.Show("Số điện thoại phải là 10 chữ số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus(); return;
            }
            if (bus.kiemTraTrungSDT(sdt))
            {
                MessageBox.Show("Số điện thoại này đã tồn tại trong hệ thống!", "Lỗi trùng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSDT.Focus(); return;
            }

            // 3. Validate Email
            string email = txtEmail.Text.Trim();
            if (!string.IsNullOrWhiteSpace(email)) // Email có thể null, nhưng nếu nhập phải đúng
            {
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Email không đúng định dạng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus(); return;
                }
                if (bus.kiemTraTrungEmail(email))
                {
                    MessageBox.Show("Email này đã được sử dụng!", "Lỗi trùng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmail.Focus(); return;
                }
            }

            khachHangDTO khnew = new khachHangDTO();

            khnew.MaKhachHang = bus.layMa();
            khnew.TenKhachHang = txtTen.Text.Trim();
            khnew.SoDienThoai = sdt;
            khnew.Email = email;

            kh = khnew;

                MessageBox.Show("Thêm khách hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
        }

        private void btnHuy_Click    (object sender, EventArgs e) => this.Close();

        private void txtMa_TextChanged(object sender, EventArgs e)
        {

        }
    }
}