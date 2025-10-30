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
    public partial class detailNguyenLieu : Form
    {
        private nguyenLieuDTO ct;
        public detailNguyenLieu(nguyenLieuDTO ct)
        {
            InitializeComponent();
            this.ct = ct;
        }

        private void loadFontChuVaSizeTableNguyenLieu()
        {
            // --- Căn giữa và tắt sort ---
            foreach (DataGridViewColumn col in tableNguyenLieu.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableNguyenLieu.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableNguyenLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // font cho dữ liệu trong table
            tableNguyenLieu.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            //font cho header trong table
            tableNguyenLieu.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            // --- Fix lỗi mất text khi đổi font ---
            tableNguyenLieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableNguyenLieu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableNguyenLieu.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableNguyenLieu.Refresh();
        }

        private void loadFontChuVaSizeTableHeSo()
        {
            // --- Căn giữa và tắt sort ---
            foreach (DataGridViewColumn col in tableHeSo.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableHeSo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableHeSo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // font cho dữ liệu trong table
            tableHeSo.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            //font cho header trong table
            tableNguyenLieu.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            // --- Fix lỗi mất text khi đổi font ---
            tableHeSo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableHeSo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableHeSo.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableHeSo.Refresh();
        }
        private void detailNguyenLieu_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            txtMaNL.Text = ct.MaNguyenLieu.ToString();
            txtTenNL.Text = ct.TenNguyenLieu;
            txtDonVi.Text = ct.TenDonViCoSo;
            txtTonKho.Text = ct.TonKho.ToString();

            congThucBUS bus = new congThucBUS();
            BindingList<sanPhamDTO> ds = bus.docDSSanPhamTheoNguyenLieu(ct.MaNguyenLieu);
            loadDanhSachSanPham(ds);

            heSoBUS busHs = new heSoBUS();
            List<heSoDTO> dshs = busHs.layDanhSachTheoMaNL(ct.MaNguyenLieu);
            loadDanhSachHeSo(dshs);
        }

        private void loadDanhSachSanPham(BindingList<sanPhamDTO> ds)
        {
            tableNguyenLieu.Columns.Clear();
            tableNguyenLieu.DataSource = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã SP");
            dt.Columns.Add("Tên SP");
            dt.Columns.Add("Giá");
            dt.Columns.Add("Số lượng");
            dt.Columns.Add("Đơn vị");


            donViBUS busdv = new donViBUS();
            BindingList<donViDTO> dsdv = busdv.LayDanhSach();

            foreach (var sp in ds)
            {
                string tenDonVi = dsdv.FirstOrDefault(l => l.MaDonVi == sp.MaDonViCoSo)?.TenDonVi ?? "Không xác định";
                dt.Rows.Add(sp.MaSP, sp.TenSP, string.Format("{0:N0}", sp.Gia), sp.SoLuongCoSo,tenDonVi);
            }

            tableNguyenLieu.DataSource = dt;
            loadFontChuVaSizeTableNguyenLieu();
            tableNguyenLieu.RowHeadersVisible = false;

            tableNguyenLieu.ReadOnly = true;

            tableNguyenLieu.ClearSelection();

        }

        private void loadDanhSachHeSo(List<heSoDTO> ds)
        {
            tableHeSo.Columns.Clear();
            tableHeSo.DataSource = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã đơn vị");
            dt.Columns.Add("Tên đơn vị");
            dt.Columns.Add("Hệ số");

            donViBUS busdv = new donViBUS();
            BindingList<donViDTO> dsdv = busdv.LayDanhSach();

            foreach (var sp in ds)
            {
                string tenDonVi = dsdv.FirstOrDefault(l => l.MaDonVi == sp.MaDonVi)?.TenDonVi ?? "Không xác định";
                dt.Rows.Add(sp.MaDonVi,tenDonVi,sp.HeSo);
            }

            tableHeSo.DataSource = dt;
            loadFontChuVaSizeTableHeSo();
            tableHeSo.RowHeadersVisible = false;

            tableHeSo.ReadOnly = true;

            tableHeSo.Columns["Mã đơn vị"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tableHeSo.Columns["Tên đơn vị"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tableHeSo.Columns["Hệ số"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            tableHeSo.ClearSelection();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtDonVi_TextChanged(object sender, EventArgs e)
        {

        }

        private void bigLabel1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
