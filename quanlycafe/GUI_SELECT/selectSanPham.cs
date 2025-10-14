using quanlycafe.BUS;
using quanlycafe.DTO;

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
    public partial class selectSanPham : Form
    {

        public int MaSP { get; private set; }
        public string TenSP { get; private set; }
        public selectSanPham()
        {
            InitializeComponent();
        }

        private void chooseSanPham_Load(object sender, EventArgs e)
        {
            sanPhamBUS bus = new sanPhamBUS();
            bus.docDSSanPham();

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã SP");
            dt.Columns.Add("Tên SP");
            dt.Columns.Add("Giá");

            foreach (var sp in sanPhamBUS.ds.Where(x => x.TrangThai == 1))
            {
                dt.Rows.Add(sp.MaSP, sp.TenSP, string.Format("{0:N0}", sp.Gia));
            }

            tableSanPham.DataSource = dt;
            tableSanPham.ReadOnly = true;
            tableSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tableSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void tableSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tableSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                MaSP = Convert.ToInt32(tableSanPham.Rows[e.RowIndex].Cells["Mã SP"].Value);
                TenSP = tableSanPham.Rows[e.RowIndex].Cells["Tên SP"].Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
