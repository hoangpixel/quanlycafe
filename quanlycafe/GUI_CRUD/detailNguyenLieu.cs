using quanlycafe.DTO;
using quanlycafe.BUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlycafe.GUI_CRUD
{
    public partial class detailNguyenLieu : Form
    {
        private nguyenLieuDTO ct;
        public detailNguyenLieu(nguyenLieuDTO ct)
        {
            InitializeComponent();
            this.ct = ct;
        }

        private void detailNguyenLieu_Load(object sender, EventArgs e)
        {
            txtMaNL.Text = ct.MaNguyenLieu.ToString();
            txtTenNL.Text = ct.TenNguyenLieu;
            txtDonVi.Text = ct.TenDonViCoSo;
            txtTonKho.Text = ct.TonKho.ToString();

            congThucBUS bus = new congThucBUS();
            var ds = bus.docDSSanPhamTheoNguyenLieu(ct.MaNguyenLieu);
            loadDanhSachSanPham(ds);
        }

        private void loadDanhSachSanPham(List<sanPhamDTO> ds)
        {
            tableNguyenLieu.Columns.Clear();
            tableNguyenLieu.DataSource = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã SP");
            dt.Columns.Add("Tên SP");
            dt.Columns.Add("Giá");
            dt.Columns.Add("Số lượng");

            foreach (var sp in ds)
            {
                dt.Rows.Add(sp.MaSP, sp.TenSP, sp.Gia, sp.SoLuongCoSo);
            }

            tableNguyenLieu.DataSource = dt;
            tableNguyenLieu.RowHeadersVisible = false;

            tableNguyenLieu.ReadOnly = true;

            tableNguyenLieu.ClearSelection();

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtDonVi_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
