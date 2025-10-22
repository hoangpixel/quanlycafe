using quanlycafe.BUS;
using quanlycafe.DTO;
using quanlycafe.GUI_CRUD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlycafe.GUI
{
    public partial class congThucGUI : UserControl
    {

        private int lastSelectedRowCongThuc = -1;

        public congThucGUI()
        {
            InitializeComponent();
        }

        private void loadDanhSachCongThuc(List<congThucDTO> ds)
        {
            tableCongThuc.Columns.Clear();
            tableCongThuc.DataSource = null;

            if (ds == null || ds.Count == 0)
            {
                MessageBox.Show("Chưa có công thức nào!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var danhSachSapXep = ds
                .OrderBy(x => x.MaSanPham)
                .ThenBy(x => x.MaNguyenLieu)
                .Select(x => new
                {
                    Mã_SP = x.MaSanPham,
                    Tên_SP = x.TenSanPham,
                    Mã_NL = x.MaNguyenLieu,
                    Tên_NL = x.TenNguyenLieu,
                    Số_lượng = x.SoLuongCoSo,
                    Đơn_vị = x.TenDonViCoSo,
                    Mã_Đơn_vị = x.MaDonViCoSo
                })
                .ToList();

            tableCongThuc.DataSource = danhSachSapXep;

            tableCongThuc.Columns["Mã_SP"].HeaderText = "Mã SP";
            tableCongThuc.Columns["Tên_SP"].HeaderText = "Tên SP";
            tableCongThuc.Columns["Mã_NL"].HeaderText = "Mã NL";
            tableCongThuc.Columns["Tên_NL"].HeaderText = "Tên NL";
            tableCongThuc.Columns["Số_lượng"].HeaderText = "Số lượng";
            tableCongThuc.Columns["Đơn_vị"].HeaderText = "Đơn vị";

            tableCongThuc.Columns["Mã_Đơn_vị"].Visible = false;

            foreach (DataGridViewColumn col in tableCongThuc.Columns)
                col.SortMode = DataGridViewColumnSortMode.Automatic;

            btnSuaCT.Enabled = false;
            btnXoaCT.Enabled = false;
            btnChiTietCT.Enabled = false;

            tableCongThuc.ClearSelection();
        }

        private void congThucGUI_Load(object sender, EventArgs e)
        {
            congThucBUS bus = new congThucBUS();
            var ds = bus.docTatCaCongThuc();
            loadDanhSachCongThuc(ds);
        }

        private void kiemTraClickTable(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.RowIndex == lastSelectedRowCongThuc)
            {
                tableCongThuc.ClearSelection();
                lastSelectedRowCongThuc = -1;
                btnThemCT.Enabled = true;
                btnSuaCT.Enabled = false;
                btnXoaCT.Enabled = false;
                btnChiTietCT.Enabled = false;
                return;
            }

            tableCongThuc.ClearSelection();
            tableCongThuc.Rows[e.RowIndex].Selected = true;
            lastSelectedRowCongThuc = e.RowIndex;

            btnSuaCT.Enabled = true;
            btnXoaCT.Enabled = true;
            btnChiTietCT.Enabled = true;
        }

        private void btnThemCT_Click(object sender, EventArgs e)
        {
            using (insertCongThuc form = new insertCongThuc())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
                congThucBUS bus = new congThucBUS();
                var ds = bus.docTatCaCongThuc();
                loadDanhSachCongThuc(ds);
            }
        }

        private void btnSuaCT_Click(object sender, EventArgs e)
        {
            if (tableCongThuc.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableCongThuc.SelectedRows[0];
                congThucDTO ct = new congThucDTO
                {
                    MaSanPham = Convert.ToInt32(row.Cells["Mã_SP"].Value),
                    MaNguyenLieu = Convert.ToInt32(row.Cells["Mã_NL"].Value),
                    TenSanPham = row.Cells["Tên_SP"].Value.ToString(),
                    TenNguyenLieu = row.Cells["Tên_NL"].Value.ToString(),
                    SoLuongCoSo = float.Parse(row.Cells["Số_lượng"].Value.ToString()),
                    MaDonViCoSo = Convert.ToInt32(row.Cells["Mã_Đơn_vị"].Value),
                    TenDonViCoSo = row.Cells["Đơn_vị"].Value.ToString()
                };

                using (updateCongThuc form = new updateCongThuc(ct))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }

                congThucBUS bus = new congThucBUS();
                var ds = bus.docTatCaCongThuc();
                loadDanhSachCongThuc(ds);
            }
        }

        private void btnXoaCT_Click(object sender, EventArgs e)
        {
            if (tableCongThuc.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableCongThuc.SelectedRows[0];

                int maSP = Convert.ToInt32(row.Cells["Mã_SP"].Value);
                int maNL = Convert.ToInt32(row.Cells["Mã_NL"].Value);

                using (deleteCongThuc form = new deleteCongThuc(maSP, maNL))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }

                congThucBUS bus = new congThucBUS();
                var ds = bus.docTatCaCongThuc();
                loadDanhSachCongThuc(ds);
            }
        }

        private void btnChiTietCT_Click(object sender, EventArgs e)
        {
            if (tableCongThuc.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableCongThuc.SelectedRows[0];

                congThucDTO ct = new congThucDTO
                {
                    MaSanPham = Convert.ToInt32(row.Cells["Mã_SP"].Value),
                    MaNguyenLieu = Convert.ToInt32(row.Cells["Mã_NL"].Value),
                    TenSanPham = row.Cells["Tên_SP"].Value.ToString(),
                    TenNguyenLieu = row.Cells["Tên_NL"].Value.ToString(),
                    SoLuongCoSo = float.Parse(row.Cells["Số_lượng"].Value.ToString()),
                    TenDonViCoSo = row.Cells["Đơn_vị"].Value.ToString()
                };

                using (detailCongThuc form = new detailCongThuc(ct))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn công thức cần xem chi tiết!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReFreshCT_Click(object sender, EventArgs e)
        {
            congThucBUS bus = new congThucBUS();
            var ds = bus.docTatCaCongThuc();
            loadDanhSachCongThuc(ds);
        }

        private void btnExcelCT_Click(object sender, EventArgs e)
        {
            using (selectExcelCongThuc form = new selectExcelCongThuc())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
            congThucBUS bus = new congThucBUS();
            var ds = bus.docTatCaCongThuc();
            loadDanhSachCongThuc(ds);
        }

        // dòng này là để cho khi mà mình load trang nó kh chọn dòng đầu nha
        private void tableCongThuc_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tableCongThuc.ClearSelection();
        }
    }
}
