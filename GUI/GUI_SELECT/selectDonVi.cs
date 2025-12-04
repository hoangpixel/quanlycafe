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
    public partial class selectDonVi : Form
    {
        public int maDonVi { get; private set; }
        public string tenDonVi { get; private set; }

        public bool chiLayHeSo { get; set; } = false;
        public int maNguyenLieu { get; set; } = -1;
        donViBUS bus = new donViBUS();

        public selectDonVi()
        {
            InitializeComponent();
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
        private void selectDonVi_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            BindingList<donViDTO> dsDonVi = new BindingList<donViDTO>();
            if(chiLayHeSo && maNguyenLieu > 0)
            {
                dsDonVi = bus.layDanhSachDonViTheoNguyenLieu(maNguyenLieu);
                if (dsDonVi.Count == 0)
                {
                    MessageBox.Show("Nguyên liệu này chưa có đơn vị quy đổi trong hệ số đơn vị!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }
            }else
            {
                bus.LayDanhSach();
                dsDonVi = donViBUS.ds;
            }

            loadDanhSachDonVi(dsDonVi);
            loadFontChuVaSizeTableDonVi();
        }
        private void loadDanhSachDonVi(BindingList<donViDTO> ds)
        {
            tableDonVi.AutoGenerateColumns = false;
            tableDonVi.Columns.Clear();

            tableDonVi.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonVi", HeaderText = "Mã đơn vị" });
            tableDonVi.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenDonVi", HeaderText = "Tên đơn vị" });

            tableDonVi.DataSource = ds;
            tableDonVi.ReadOnly = true;
            tableDonVi.ClearSelection();
        }

        private void tableDonVi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = tableDonVi.SelectedRows[0];
                donViDTO donvi = row.DataBoundItem as donViDTO;
                maDonVi = donvi.MaDonVi;
                tenDonVi = donvi.TenDonVi;
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
            if(bus.kiemTraChuoiRong(txtTimKiem.Text))
            {
                MessageBox.Show("Vui lòng nhập giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimKiem.Focus();
                return;
            }
            string tim = txtTimKiem.Text.Trim();
            BindingList<donViDTO> dsDV = new donViBUS().LayDanhSach();
            List<donViDTO> dsTim = new List<donViDTO>();
            dsTim = (from item in dsDV
                     where item.TenDonVi.ToLower().Contains(tim)
                     orderby item.MaDonVi
                     select item).ToList();

            if(dsTim != null && dsTim.Count > 0)
            {
                BindingList<donViDTO> dsSauBinding = new BindingList<donViDTO>(dsTim);
                loadDanhSachDonVi(dsSauBinding);
                loadFontChuVaSizeTableDonVi();
            }else
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            BindingList<donViDTO> dsDonVi = new BindingList<donViDTO>();
            if (chiLayHeSo && maNguyenLieu > 0)
            {
                dsDonVi = bus.layDanhSachDonViTheoNguyenLieu(maNguyenLieu);
                if (dsDonVi.Count == 0)
                {
                    MessageBox.Show("Nguyên liệu này chưa có đơn vị quy đổi trong hệ số đơn vị!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }
            }
            else
            {
                bus.LayDanhSach();
                dsDonVi = donViBUS.ds;
            }

            loadDanhSachDonVi(dsDonVi);
            loadFontChuVaSizeTableDonVi();
        }
    }
}
