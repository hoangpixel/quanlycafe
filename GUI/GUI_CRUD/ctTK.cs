using System;
using System.Windows.Forms;
using BUS;
using DTO;

namespace GUI.GUI_CRUD
{
    public partial class ctTK : Form
    {
        private int maTaiKhoan;

        public ctTK()
        {
            InitializeComponent();
        }

        // Constructor nhận mã tài khoản
        public ctTK(int maTK) : this()
        {
            this.maTaiKhoan = maTK;
            this.Load += CtTK_Load;
            this.btnDong.Click += BtnDong_Click;
        }

        // Sự kiện khi form load
        private void CtTK_Load(object sender, EventArgs e)
        {
            // Căn giữa form
            this.StartPosition = FormStartPosition.CenterScreen;

            // Không cho phép resize
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Load dữ liệu
            LoadThongTinTaiKhoan();
        }

        // Phương thức load thông tin tài khoản từ database
        private void LoadThongTinTaiKhoan()
        {
            try
            {
                // Lấy danh sách tài khoản và tìm theo mã
                taikhoanBUS tkBUS = new taikhoanBUS();
                var danhSachTK = tkBUS.LayDanhSach();

                taikhoanDTO taiKhoan = null;
                foreach (var tk in danhSachTK)
                {
                    if (tk.MAtaikHOAN == maTaiKhoan)
                    {
                        taiKhoan = tk;
                        break;
                    }
                }

                if (taiKhoan != null)
                {
                    // === HIỂN THỊ THÔNG TIN TÀI KHOẢN (BÊN TRÁI) ===
                    txtTen.Text = taiKhoan.MAtaikHOAN.ToString();
                    txtSDT.Text = taiKhoan.TENDANGNHAP;
                    txtEmail.Text = taiKhoan.TENVAITRO ?? "N/A";
                    txtLuong.Text = taiKhoan.TrangThaiText;
                    txtNgayTao.Text = taiKhoan.NGAYTAO.ToString("dd/MM/yyyy HH:mm");

                    // === HIỂN THỊ THÔNG TIN NHÂN VIÊN (BÊN PHẢI) ===
                    nhanVienBUS nvBUS = new nhanVienBUS();
                    nhanVienDTO nhanVien = nvBUS.LayTheoMa(taiKhoan.MANHANVIEN);

                    if (nhanVien != null)
                    {
                        textBox1.Text = nhanVien.MaNhanVien.ToString();
                        textBox5.Text = nhanVien.HoTen;
                        textBox4.Text = string.IsNullOrEmpty(nhanVien.SoDienThoai) ? "N/A" : nhanVien.SoDienThoai;
                        textBox3.Text = string.IsNullOrEmpty(nhanVien.Email) ? "N/A" : nhanVien.Email;
                        textBox2.Text = nhanVien.Luong.ToString("#,##0") + " VNĐ";
                    }
                    else
                    {
                        textBox1.Text = "N/A";
                        textBox5.Text = "N/A";
                        textBox4.Text = "N/A";
                        textBox3.Text = "N/A";
                        textBox2.Text = "N/A";
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin tài khoản!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Sự kiện click nút Đóng
        private void BtnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Các sự kiện từ Designer
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void label12_Click(object sender, EventArgs e)
        {
        }

        private void label13_Click(object sender, EventArgs e)
        {
        }
    }
}