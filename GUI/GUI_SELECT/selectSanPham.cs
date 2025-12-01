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
        private BindingList<loaiDTO> dsLoai;
        public selectSanPham()
        {
            InitializeComponent();
            //this.FormBorderStyle = FormBorderStyle.None;
        }

        private void loadFontChuVaSizeTableSanPham()
        {
            foreach (DataGridViewColumn col in tableSanPham.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableSanPham.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tableSanPham.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tableSanPham.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            tableSanPham.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableSanPham.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableSanPham.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableSanPham.Refresh();
        }

        private void chooseSanPham_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            dsLoai = new loaiSanPhamBUS().LayDanhSach();

            sanPhamBUS bus = new sanPhamBUS();
            BindingList<sanPhamDTO> dsSP = bus.LayDanhSach();
            loadDanhSachChonSP(dsSP);
            loadFontChuVaSizeTableSanPham();
            
        }

        private void loadDanhSachChonSP(BindingList<sanPhamDTO> ds)
        {
            tableSanPham.AutoGenerateColumns = false;
            tableSanPham.Columns.Clear();

            tableSanPham.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSP", HeaderText = "Mã SP" });
            tableSanPham.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenSP", HeaderText = "Tên SP" });
            tableSanPham.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaLoai", HeaderText = "Tên loại" });
            tableSanPham.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Gia", HeaderText = "Giá", DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });

            tableSanPham.DataSource = ds;
            tableSanPham.ClearSelection();
        }

        private void tableSanPham_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            sanPhamDTO sp = tableSanPham.Rows[e.RowIndex].DataBoundItem as sanPhamDTO;
            if (tableSanPham.Columns[e.ColumnIndex].HeaderText == "Tên loại")
            {
                loaiDTO loai = dsLoai.FirstOrDefault(x => x.MaLoai == sp.MaLoai);
                e.Value = loai?.TenLoai ?? "Không xác định";
            }
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

            if (dskq == null || dskq.Count == 0)
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            loadDanhSachChonSP(dskq);

            loadFontChuVaSizeTableSanPham();

            cboTimKiem.SelectedIndex = -1;
            txtTimKiem.Clear();
        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            sanPhamBUS bus = new sanPhamBUS();
            BindingList<sanPhamDTO> dsMoi = bus.LayDanhSach();
            loadDanhSachChonSP(dsMoi);
            loadFontChuVaSizeTableSanPham();

            cboTimKiem.SelectedIndex = -1;
            txtTimKiem.Clear();
        }

        private void tableSanPham_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = tableSanPham.Rows[e.RowIndex];
                sanPhamDTO sp = row.DataBoundItem as sanPhamDTO;
                MaSP = sp.MaSP;
                TenSP = sp.TenSP;
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
