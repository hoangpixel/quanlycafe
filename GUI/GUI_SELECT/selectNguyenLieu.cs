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
    public partial class selectNguyenLieu : Form
    {
        public int MaNL { get; private set; }
        public string TenNL { get; private set; }
        public selectNguyenLieu()
        {
            InitializeComponent();
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

        private void selectNguyenLieu_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            nguyenLieuBUS bus = new nguyenLieuBUS();
            bus.LayDanhSach();

            //MessageBox.Show("Số lượng nguyên liệu: " + nguyenLieuBUS.ds.Count);

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã NL");
            dt.Columns.Add("Tên NL");
            dt.Columns.Add("Đơn vị");
            dt.Columns.Add("Tồn kho");

            donViBUS busDonVi = new donViBUS();
            BindingList<donViDTO> dsDonVi = busDonVi.LayDanhSach();

            foreach (var nl in nguyenLieuBUS.ds.Where(x => x.TrangThai == 1))
            {
                string tenDonVi = dsDonVi.FirstOrDefault(l => l.MaDonVi == nl.MaDonViCoSo)?.TenDonVi ?? "Không xác định";
                dt.Rows.Add(nl.MaNguyenLieu, nl.TenNguyenLieu, tenDonVi, nl.TonKho.ToString("0.################"));
            }

            tableNguyenLieu.DataSource = dt;
            loadFontChuVaSizeTableNguyenLieu();
            tableNguyenLieu.ReadOnly = true;
            tableNguyenLieu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tableNguyenLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void tableNguyenLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            int index = cboTimKiem.SelectedIndex;
            string tim = txtTimKiem.Text.Trim();
            if(index == -1 || string.IsNullOrWhiteSpace(tim))
            {
                MessageBox.Show("Vui lòng nhập dữ liệu tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            nguyenLieuBUS bus = new nguyenLieuBUS();
            BindingList<nguyenLieuDTO> dsNL = bus.timKiemCoBanNL(tim,index);
            if(dsNL!=null && dsNL.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Mã NL");
                dt.Columns.Add("Tên NL");
                dt.Columns.Add("Đơn vị cơ sở");
                dt.Columns.Add("Tồn kho");

                donViBUS busDonVi = new donViBUS();
                BindingList<donViDTO> dsDonVi = busDonVi.LayDanhSach();

                foreach (var nl in dsNL.Where(x => x.TrangThai == 1))
                {
                    string tenDonVi = dsDonVi.FirstOrDefault(l => l.MaDonVi == nl.MaDonViCoSo)?.TenDonVi ?? "Không xác định";
                    dt.Rows.Add(nl.MaNguyenLieu, nl.TenNguyenLieu, tenDonVi,nl.TonKho);
                }

                tableNguyenLieu.DataSource = dt;
                tableNguyenLieu.ReadOnly = true;
                tableNguyenLieu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                tableNguyenLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                tableNguyenLieu.ClearSelection();
                cboTimKiem.SelectedIndex = -1;
                txtTimKiem.Clear();
            }
            else
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            nguyenLieuBUS bus = new nguyenLieuBUS();
            bus.LayDanhSach();

            //MessageBox.Show("Số lượng nguyên liệu: " + nguyenLieuBUS.ds.Count);

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã NL");
            dt.Columns.Add("Tên NL");
            dt.Columns.Add("Đơn vị cơ sở");
            dt.Columns.Add("Tồn kho");

            donViBUS busDonVi = new donViBUS();
            BindingList<donViDTO> dsDonVi = busDonVi.LayDanhSach();

            foreach (var nl in nguyenLieuBUS.ds.Where(x => x.TrangThai == 1))
            {
                string tenDonVi = dsDonVi.FirstOrDefault(l => l.MaDonVi == nl.MaDonViCoSo)?.TenDonVi ?? "Không xác định";
                dt.Rows.Add(nl.MaNguyenLieu, nl.TenNguyenLieu, tenDonVi, nl.TonKho);
            }

            tableNguyenLieu.DataSource = dt;
            tableNguyenLieu.ReadOnly = true;
            tableNguyenLieu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tableNguyenLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tableNguyenLieu.ClearSelection();
            cboTimKiem.SelectedIndex = -1;
            txtTimKiem.Clear();
        }

        private void tableNguyenLieu_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                MaNL = Convert.ToInt32(tableNguyenLieu.Rows[e.RowIndex].Cells["Mã NL"].Value);
                TenNL = tableNguyenLieu.Rows[e.RowIndex].Cells["Tên NL"].Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
