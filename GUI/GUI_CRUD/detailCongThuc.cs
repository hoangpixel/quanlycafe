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
    public partial class detailCongThuc : Form
    {
        private congThucDTO ct;

        public detailCongThuc(congThucDTO ct)
        {
            InitializeComponent();
            this.ct = ct;
        }

        private void detailCongThuc_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            txtMaSP.Text = ct.MaSanPham.ToString();
            txtMaNL.Text = ct.MaNguyenLieu.ToString();
            txtTenSP.Text = ct.TenSanPham;
            txtTenNL.Text = ct.TenNguyenLieu;
            txtSoL.Text = ct.SoLuongCoSo.ToString();
            txtTenDV.Text = ct.TenDonViCoSo;
        }

        private void panel2_Resize(object sender, EventArgs e)
        {
        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
