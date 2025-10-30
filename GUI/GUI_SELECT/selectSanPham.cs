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

namespace GUI.GUI_SELECT
{
    public partial class selectSanPham : Form
    {

        public int MaSP { get; private set; }
        public string TenSP { get; private set; }
        public selectSanPham()
        {
            InitializeComponent();
            //this.FormBorderStyle = FormBorderStyle.None;
        }

        private void loadFontChuVaSizeTableSanPham()
        {
            // --- Căn giữa và tắt sort ---
            foreach (DataGridViewColumn col in tableSanPham.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableSanPham.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // font cho dữ liệu trong table
            tableSanPham.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            //font cho header trong table
            tableSanPham.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            // --- Fix lỗi mất text khi đổi font ---
            tableSanPham.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableSanPham.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableSanPham.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableSanPham.Refresh();
        }

        private void chooseSanPham_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            sanPhamBUS bus = new sanPhamBUS();
            bus.LayDanhSach();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã SP");
            dt.Columns.Add("Tên SP");
            dt.Columns.Add("Tên loại");
            dt.Columns.Add("Giá");

            loaiSanPhamBUS busLoai = new loaiSanPhamBUS();
            BindingList<loaiDTO> dsLoai = busLoai.LayDanhSach();

            foreach (var sp in sanPhamBUS.ds.Where(x => x.TrangThai == 1))
            {
                string tenLoai = dsLoai.FirstOrDefault(l => l.MaLoai == sp.MaLoai)?.TenLoai ?? "Không xác định";
                dt.Rows.Add(sp.MaSP, sp.TenSP, tenLoai, string.Format("{0:N0}", sp.Gia));
            }

            tableSanPham.DataSource = dt;
            loadFontChuVaSizeTableSanPham();
            tableSanPham.ReadOnly = true;
            tableSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tableSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tableSanPham.ClearSelection();
        }

        private void tableSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }


        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            int index = cboTimKiem.SelectedIndex;
            string tim = txtTimKiem.Text.Trim();
            if (index == -1 || string.IsNullOrWhiteSpace(tim))
            {
                MessageBox.Show("Vui lòng nhập dữ liệu tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sanPhamBUS bus = new sanPhamBUS();
            BindingList<sanPhamDTO> dskq = bus.timKiemCoBan(tim, index);

            loaiSanPhamBUS busLoai = new loaiSanPhamBUS();
            BindingList<loaiDTO> dsLoai = busLoai.LayDanhSach();

            if (dskq != null && dskq.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Mã SP");
                dt.Columns.Add("Tên SP");
                dt.Columns.Add("Tên loại");
                dt.Columns.Add("Giá");

                foreach (var sp in dskq.Where(x => x.TrangThai == 1))
                {
                    string tenLoai = dsLoai.FirstOrDefault(l => l.MaLoai == sp.MaLoai)?.TenLoai ?? "Không xác định";
                    dt.Rows.Add(sp.MaSP, sp.TenSP, tenLoai, string.Format("{0:N0}", sp.Gia));
                }

                tableSanPham.DataSource = dt;
                tableSanPham.ReadOnly = true;
                tableSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                tableSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                tableSanPham.ClearSelection();

            }
            else
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            sanPhamBUS bus = new sanPhamBUS();
            bus.LayDanhSach();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã SP");
            dt.Columns.Add("Tên SP");
            dt.Columns.Add("Tên loại");
            dt.Columns.Add("Giá");

            loaiSanPhamBUS busLoai = new loaiSanPhamBUS();
            BindingList<loaiDTO> dsLoai = busLoai.LayDanhSach();

            foreach (var sp in sanPhamBUS.ds.Where(x => x.TrangThai == 1))
            {
                string tenLoai = dsLoai.FirstOrDefault(l => l.MaLoai == sp.MaLoai)?.TenLoai ?? "Không xác định";
                dt.Rows.Add(sp.MaSP, sp.TenSP, tenLoai, string.Format("{0:N0}", sp.Gia));
            }

            tableSanPham.DataSource = dt;
            tableSanPham.ReadOnly = true;
            tableSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tableSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tableSanPham.ClearSelection();


            cboTimKiem.SelectedIndex = -1;
            txtTimKiem.Clear();
        }

        private void tableSanPham_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                MaSP = Convert.ToInt32(tableSanPham.Rows[e.RowIndex].Cells["Mã SP"].Value);
                TenSP = tableSanPham.Rows[e.RowIndex].Cells["Tên SP"].Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
