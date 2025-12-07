using BUS;
using DTO;
using FONTS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.GUI_UC
{
    public partial class dangNhapGUI : Form
    {
        private taikhoanBUS busTaiKhoan = new taikhoanBUS();
        public dangNhapGUI()
        {
            InitializeComponent();

            txtTenDangNhap.Click += (s, e) => {
                panelUser.BackColor = Color.White;
                panelPass.BackColor = SystemColors.Control;
            };
            txtMatKhau.Click += (s, e) => {
                panelUser.BackColor = SystemColors.Control;
                panelPass.BackColor = Color.White;
            };

            txtTenDangNhap.Select();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string user = txtTenDangNhap.Text.Trim();
            string pass = txtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                taikhoanDTO tk = busTaiKhoan.KiemTraDangNhap(user, pass);
                if (tk != null)
                {
                    // 2. LƯU TÀI KHOẢN VÀO SESSION
                    Session.TaiKhoanHienTai = tk;
                    // 👇 3. LOAD QUYỀN VÀO SESSION 👇
                    phanquyenBUS busPQ = new phanquyenBUS();
                    Session.QuyenHienTai = busPQ.LayChiTietQuyenTheoVaiTro(tk.MAVAITRO);
                    // 3. LẤY THÔNG TIN NHÂN VIÊN TỪ MÃ NV
                    nhanVienBUS busNV = new nhanVienBUS();
                    nhanVienDTO nv = busNV.LayThongTinNhanVien(tk.MANHANVIEN);

                    if (nv != null)
                    {
                        // 4. LƯU NHÂN VIÊN VÀO SESSION
                        Session.NhanVienHienTai = nv;

                        MessageBox.Show($"Xin chào, {nv.HoTen}!", "Đăng nhập thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Trường hợp có TK nhưng không tìm thấy thông tin nhân viên (dữ liệu lỗi)
                        MessageBox.Show("Đăng nhập thành công (Không tìm thấy thông tin nhân viên)!", "Thông báo");
                    }

                    // 5. Mở MainGUI
                    mainGUI main = new mainGUI();
                    this.Hide();
                    main.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }
    }
}
