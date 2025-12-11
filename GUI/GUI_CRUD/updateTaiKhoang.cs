using BUS;
using DTO;
using FONTS;
using GUI.GUI_SELECT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class updateTaiKhoan : Form
    {
        private BindingList<nhanVienDTO> dsNV;
        private BindingList<vaitroDTO> dsVT;
        private taikhoanBUS bus = new taikhoanBUS();
        public bool coNhapMK = false;
        public taikhoanDTO tk;
        private int maNV = -1, maVT = -1;

        public updateTaiKhoan(taikhoanDTO tk)
        {
            InitializeComponent();
            this.tk = tk;
            dsNV = new nhanVienBUS().LayDanhSach();
            dsVT = new vaitroBUS().LayDanhSach();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void btnChonNV_Click(object sender, EventArgs e)
        {
            using (var f = new selectNhanVien())
            {
                if (f.ShowDialog() == DialogResult.OK) { maNV = f.MaNV; txtTenNV.Text = f.TenNV; }
            }
        }

        private void btnChonVT_Click(object sender, EventArgs e)
        {
            using (selectVaiTro form = new selectVaiTro())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    maVT = form.maVaiTro;
                    txtTenVT.Text = form.tenVaiTro;
                }
            }
        }

        private void chkHienMatKhau_CheckedChanged(object sender)
        {
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
            txtXacNhanMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (maVT == -1)
            {
                MessageBox.Show("Vui lòng chọn vai trò!", "Cảnh báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (maNV == -1)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Cảnh báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return;
            }

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
                tk.MATKHAU = matKhauMoi;
                coNhapMK = true;
            }

            if (maNV != tk.MANHANVIEN && bus.kiemTraTrungNhanVien(maNV))
            {
                MessageBox.Show("Nhân viên này đã có tài khoản rồi!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnChonNV.Focus();
                return;
            }

            if (bus.kiemTraTrungTenTK(txtTenDangNhap.Text.Trim(),tk.MATAIKHOAN))
            {
                MessageBox.Show("Tên tài khoản đã tồn tại!", "Cảnh báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return;
            }

            tk.MANHANVIEN = maNV;
            tk.TENDANGNHAP = txtTenDangNhap.Text.Trim();
            tk.MAVAITRO = maVT;
            if (tk != null)
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

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void updateTaiKhoan_Load(object sender, EventArgs e)
        {
            nhanVienDTO nv = dsNV.FirstOrDefault(x => x.MaNhanVien == tk.MANHANVIEN);
            vaitroDTO vt = dsVT.FirstOrDefault(x => x.MaVaiTro == tk.MAVAITRO);

            maNV = tk.MANHANVIEN;
            maVT = tk.MAVAITRO;
            txtTenDangNhap.Text = tk.TENDANGNHAP;
            txtTenNV.Text = nv?.HoTen ?? "Không xác định";
            txtTenVT.Text = vt?.TenVaiTro ?? "Không xác định";

            // 4. Để trống mật khẩu (giữ nguyên mật khẩu cũ nếu không nhập)
            txtMatKhau.Text = "";
            txtMatKhau.UseSystemPasswordChar = true;
            txtXacNhanMatKhau.Text = "";
            txtXacNhanMatKhau.UseSystemPasswordChar = true;
        }
    }
}
