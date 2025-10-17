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
    public partial class selectNguyenLieu : Form
    {
        public int MaNL { get; private set; }
        public string TenNL { get; private set; }
        public selectNguyenLieu()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void selectNguyenLieu_Load(object sender, EventArgs e)
        {
            nguyenLieuBUS bus = new nguyenLieuBUS();
            bus.napDSNguyenLieu();

            //MessageBox.Show("Số lượng nguyên liệu: " + nguyenLieuBUS.ds.Count);

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã NL");
            dt.Columns.Add("Tên NL");
            dt.Columns.Add("Đơn vị cơ sở");

            donViBUS busDonVi = new donViBUS();
            List<donViDTO> dsDonVi = busDonVi.layDanhSachDonVi();

            foreach (var nl in nguyenLieuBUS.ds.Where(x => x.TrangThai == 1))
            {
                string tenDonVi = dsDonVi.FirstOrDefault(l => l.MaDonVi == nl.MaDonViCoSo)?.TenDonVi ?? "Không xác định";
                dt.Rows.Add(nl.MaNguyenLieu, nl.TenNguyenLieu, tenDonVi);
            }

            tableNguyenLieu.DataSource = dt;
            tableNguyenLieu.ReadOnly = true;
            tableNguyenLieu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tableNguyenLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void tableNguyenLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                MaNL = Convert.ToInt32(tableNguyenLieu.Rows[e.RowIndex].Cells["Mã NL"].Value);
                TenNL = tableNguyenLieu.Rows[e.RowIndex].Cells["Tên NL"].Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void tableNguyenLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
