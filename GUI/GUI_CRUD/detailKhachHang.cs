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

namespace GUI.GUI_CRUD
{
    public partial class detailKhachHang : Form
    {
        private khachHangDTO kh;
        public detailKhachHang(khachHangDTO kh)
        {
            InitializeComponent();
            this.kh = kh;
        }

        private void detailKhachHang_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            txtMaKH.Text = kh.MaKhachHang.ToString();
            txtTen.Text = kh.TenKhachHang;
            txtSDT.Text = kh.SoDienThoai;
            txtEmail.Text = kh.Email;
            txtNgayTao.Text = kh.NgayTao.ToString("dd/MM/yyyy");
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
