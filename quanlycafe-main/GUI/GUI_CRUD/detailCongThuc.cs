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
            txtSoL.Text = ct.SoLuongCoSo.ToString();

            BindingList<sanPhamDTO> dsSP = new sanPhamBUS().LayDanhSach();
            BindingList<nguyenLieuDTO> dsNL = new nguyenLieuBUS().LayDanhSach();
            BindingList<donViDTO> dsDV = new donViBUS().LayDanhSach();

            sanPhamDTO sp = dsSP.FirstOrDefault(x => x.MaSP == ct.MaSanPham);
            txtTenSP.Text = sp?.TenSP ?? "Không xác định";

            nguyenLieuDTO nl = dsNL.FirstOrDefault(x => x.MaNguyenLieu == ct.MaNguyenLieu);
            txtTenNL.Text = nl?.TenNguyenLieu ?? "Không xác định";

            donViDTO dv = dsDV.FirstOrDefault(x => x.MaDonVi == ct.MaDonViCoSo);
            txtTenDV.Text = dv?.TenDonVi ?? "Không xác định";
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
