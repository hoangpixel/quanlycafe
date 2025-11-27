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
        public selectDonVi()
        {
            InitializeComponent();
        }

        private void loadFontChuVaSizeTableDonVi()
        {
            // --- Căn giữa và tắt sort ---
            foreach (DataGridViewColumn col in tableDonVi.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableDonVi.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableDonVi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // font cho dữ liệu trong table
            tableDonVi.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            //font cho header trong table
            tableDonVi.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            // --- Fix lỗi mất text khi đổi font ---
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
            donViBUS bus = new donViBUS();

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

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã đơn vị");
            dt.Columns.Add("Tên đơn vị");

            foreach (donViDTO ct in dsDonVi)
            {
                dt.Rows.Add(ct.MaDonVi, ct.TenDonVi);
            }

            tableDonVi.DataSource = dt;
            loadFontChuVaSizeTableDonVi();
            tableDonVi.ReadOnly = true;
            tableDonVi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tableDonVi.ClearSelection();

        }

        private void tableDonVi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                maDonVi = Convert.ToInt32(tableDonVi.Rows[e.RowIndex].Cells["Mã đơn vị"].Value);
                tenDonVi = tableDonVi.Rows[e.RowIndex].Cells["Tên đơn vị"].Value.ToString();
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
