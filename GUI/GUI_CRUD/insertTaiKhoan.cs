using BUS;
using DTO;
using FONTS;
using GUI.GUI_SELECT;
using SixLabors.Fonts;
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
    public partial class insertTaiKhoan : Form
    {
        public taikhoanDTO tk;
        private int maNV = -1, maVT = -1;
        private taikhoanBUS bus = new taikhoanBUS();
        public insertTaiKhoan(taikhoanDTO tk)
        {
            InitializeComponent();
            this.tk = tk;
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

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if(maNV == -1)
            {
                MessageBox.Show("Không được để trống nhân viên", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (maVT == -1)
            {
                MessageBox.Show("Không được để trống vai trò", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(bus.KiemTraRong(txtTenDangNhap.Text))
            {
                MessageBox.Show("Không được để trống tên tài khoản", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return;
            }
            if (bus.KiemTraRong(txtMatKhau.Text))
            {
                MessageBox.Show("Không được để trống mật khẩu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return;
            }
            if (bus.KiemTraRong(txtXacNhanMatKhau.Text))
            {
                MessageBox.Show("Không được để trống xác nhận mật khẩu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return;
            }
            if(!txtMatKhau.Text.Trim().Equals(txtXacNhanMatKhau.Text.Trim()))
            {
                MessageBox.Show("Mật khẩu hoặc xác nhận mật khẩu không giống nhau", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return;
            }
            if(txtMatKhau.Text.Trim().Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có 6 ký tự trở lên", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return;
            }
            if (bus.kiemTraTrungNhanVien(maNV))
            {
                MessageBox.Show("Nhân viên này đã có tài khoản", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(bus.kiemTraTrungTenTK(txtTenDangNhap.Text.Trim()))
            {
                MessageBox.Show("Tên tài khoản đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenDangNhap.Focus();
                return;
            }


            taikhoanDTO tkMoi = new taikhoanDTO();
            tkMoi.MATAIKHOAN = bus.LayMa();
            tkMoi.TENDANGNHAP = txtTenDangNhap.Text.Trim();
            tkMoi.MATKHAU = txtMatKhau.Text.Trim();
            tkMoi.MANHANVIEN = maNV;
            tkMoi.MAVAITRO = maVT;
            tkMoi.NGAYTAO = DateTime.Now;

            tk = tkMoi;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkHienMatKhau_CheckedChanged(object sender)
        {
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
            txtXacNhanMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }

        private void insertTaiKhoan_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }
    }
}
