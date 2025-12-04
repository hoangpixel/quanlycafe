using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DAO;
using DTO;
using BUS;

namespace GUI.GUI_CRUD
{
    public partial class addNv : Form
    {
        private nhanVienBUS nvBus = new nhanVienBUS();
        private nhanVienDTO _existingNV = null;

        // Constructor mặc định (Thêm mới)
        public addNv()
        {
            InitializeComponent();
        }

        // Constructor có tham số (Sửa nhân viên)
        public addNv(nhanVienDTO nv)
        {
            InitializeComponent();
            _existingNV = nv;
        }

        // ==================== EVENT LOAD FORM ====================
        private void addNv_Load(object sender, EventArgs e)
        {
            // Focus vào ô tên
            txtTen.Focus();

            // Nếu là chỉnh sửa, load dữ liệu cũ
            if (_existingNV != null)
            {
                bigLabel1.Text = "Sửa Thông Tin Nhân Viên";
                LoadExistingData();
            }
        }

        // ==================== LOAD DỮ LIỆU CŨ ====================
        private void LoadExistingData()
        {
            txtTen.Text = _existingNV.HoTen;
            txtSDT.Text = _existingNV.SoDienThoai;
            txtEmail.Text = _existingNV.Email;
            txtDiaChi.Text = _existingNV.Luong.ToString("N0");
        }

        // ==================== NÚT LƯU ====================
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validate dữ liệu
                if (!ValidateInput())
                    return;

                // 2.  Tạo đối tượng DTO
                nhanVienDTO nv = new nhanVienDTO
                {
                    HoTen = txtTen.Text.Trim(),
                    SoDienThoai = txtSDT.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Luong = ParseLuong(txtDiaChi.Text)
                };

                // 3. Kiểm tra nếu đang sửa hay thêm mới
                if (_existingNV != null)
                {
                    // === SỬA NHÂN VIÊN ===
                    nv.MaNhanVien = _existingNV.MaNhanVien;

                    if (nvBus.CapNhatNhanVien(nv))
                    {
                        MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // === THÊM MỚI NHÂN VIÊN ===
                    if (nvBus.ThemNhanVien(nv))
                    {
                        MessageBox.Show("Thêm nhân viên thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== NÚT HỦY ====================
        private void btnHuy_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn hủy thao tác? ",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // ==================== VALIDATE DỮ LIỆU ====================
        private bool ValidateInput()
        {
            // 1. Kiểm tra tên (BẮT BUỘC)
            if (string.IsNullOrWhiteSpace(txtTen.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTen.Focus();
                return false;
            }

            // 2. Kiểm tra số điện thoại (nếu có)
            if (!string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                string sdt = txtSDT.Text.Trim();
                if (!Regex.IsMatch(sdt, @"^0\d{9,10}$"))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!\nVí dụ: 0912345678", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus();
                    return false;
                }
            }

            // 3. Kiểm tra email (nếu có)
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                string email = txtEmail.Text.Trim();
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Email không hợp lệ!\nVí dụ: example@gmail.com", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }
            }

            // 4. Kiểm tra lương (BẮT BUỘC)
            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập lương!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return false;
            }

            decimal luong = ParseLuong(txtDiaChi.Text);
            if (luong <= 0)
            {
                MessageBox.Show("Lương phải lớn hơn 0!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return false;
            }

            return true;
        }

        // ==================== PARSE LƯƠNG ====================
        private decimal ParseLuong(string text)
        {
            decimal result = 0;
            string cleaned = text.Replace(",", "").Replace(".", "");
            decimal.TryParse(cleaned, out result);
            return result;
        }

        // ==================== FORMAT LƯƠNG ====================
        private void txtDiaChi_Leave(object sender, EventArgs e)
        {
            decimal luong = ParseLuong(txtDiaChi.Text);
            if (luong > 0)
            {
                txtDiaChi.Text = luong.ToString("N0");
            }
        }

        // ==================== CHỈ CHO PHÉP NHẬP SỐ ====================
        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtDiaChi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        // ==================== ENTER ĐỂ CHUYỂN Ô ====================
        private void txtTen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtSDT.Focus();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void txtSDT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtEmail.Focus();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtDiaChi.Focus();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void txtDiaChi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLuu.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        // ==================== CÁC EVENT KHÁC ====================
        private void bigLabel1_Click(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }

        private void txtTen_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtTen_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtDiaChi_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}