using DTO;
using BUS;
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
    public partial class detailNhanVien : Form
    {
        private nhanVienDTO nv;
        public detailNhanVien(nhanVienDTO nv)
        {
            InitializeComponent();
            this.nv = nv;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void detailNhanVien_Load(object sender, EventArgs e)
        {
            txtTen.Text = nv.HoTen;
            txtEmail.Text = nv.Email;
            txtSDT.Text = nv.SoDienThoai;
            txtLuong.Text = nv.Luong.ToString("N0");

            BindingList<taikhoanDTO> dsTK = new taikhoanBUS().LayDanhSach();
            taikhoanDTO tk = dsTK.FirstOrDefault(x => x.MANHANVIEN == nv.MaNhanVien);
            txtTaiKhoan.Text = tk?.TENDANGNHAP ?? "Chưa có tài khoản";
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
