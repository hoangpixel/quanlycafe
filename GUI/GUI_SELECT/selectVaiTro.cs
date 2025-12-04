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
    public partial class selectVaiTro : Form
    {
        public int maVaiTro = -1;
        public string tenVaiTro = "";
        private vaitroBUS bus = new vaitroBUS();
        public selectVaiTro()
        {
            InitializeComponent();
            BindingList<vaitroDTO> ds = new vaitroBUS().LayDanhSach();
            loadDanhSachVaiTro(ds);
        }
        private void loadDanhSachVaiTro(BindingList<vaitroDTO> ds)
        {
            tableDonVi.AutoGenerateColumns = false;
            tableDonVi.Columns.Clear();

            tableDonVi.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaVaiTro", HeaderText = "Mã vai trò" });
            tableDonVi.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenVaiTro", HeaderText = "Tên vai trò" });

            tableDonVi.DataSource = ds;
            tableDonVi.ReadOnly = true;
            tableDonVi.ClearSelection();
            loadFontChuVaSizeTableDonVi();
        }
        private void loadFontChuVaSizeTableDonVi()
        {
            foreach (DataGridViewColumn col in tableDonVi.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableDonVi.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableDonVi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tableDonVi.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tableDonVi.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            tableDonVi.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableDonVi.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableDonVi.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableDonVi.Refresh();
        }

        private void tableDonVi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = tableDonVi.SelectedRows[0];
                vaitroDTO vt = row.DataBoundItem as vaitroDTO;
                maVaiTro = vt.MaVaiTro;
                tenVaiTro = vt.TenVaiTro;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (bus.KiemTraRong(txtTimKiem.Text))
            {
                MessageBox.Show("Vui lòng nhập giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimKiem.Focus();
                return;
            }
            string tim = txtTimKiem.Text.Trim();
            BindingList<vaitroDTO> dsVT = new vaitroBUS().LayDanhSach();
            List<vaitroDTO> dsTim = new List<vaitroDTO>();
            dsTim = (from item in dsVT
                     where item.TenVaiTro.ToLower().Contains(tim)
                     orderby item.MaVaiTro
                     select item).ToList();

            if (dsTim != null && dsTim.Count > 0)
            {
                BindingList<vaitroDTO> dsSauBinding = new BindingList<vaitroDTO>(dsTim);
                loadDanhSachVaiTro(dsSauBinding);
                loadFontChuVaSizeTableDonVi();
            }
            else
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            BindingList<vaitroDTO> ds = new vaitroBUS().LayDanhSach();
            loadDanhSachVaiTro(ds);
        }
    }
}
