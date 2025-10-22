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
    public partial class nguyenLieuGUI : UserControl
    {
        private int lastSelectedRowNguyenLieu = -1;
        public nguyenLieuGUI()
        {
            InitializeComponent();
            hienThiPlaceHolderNguyenLieu();
        }

        private void hienThiPlaceHolderNguyenLieu()
        {
            SetPlaceholder(txtTimKiemNL, "Nhập giá trị cần tìm");
            SetPlaceholder(txtTenNLTK, "Tên NL");
            SetPlaceholder(txtMinNL, "Tồn min");
            SetPlaceholder(txtMaxNL, "Tồn max");
            SetPlaceholder(txtTenDonViTK, "Tên đơn vị");
            SetComboBoxPlaceholder(cboTimKiemNL, "Chọn giá trị TK");
            SetComboBoxPlaceholder(cbTrangThaiNL, "Chọn trạng thái");
        }

        // set placehover cho tìm kiếm nâng cao sản phẩm
        private void SetPlaceholder(TextBox txt, string placeholder)
        {
            txt.ForeColor = Color.Gray;
            txt.Text = placeholder;
            txt.GotFocus += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };
            txt.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.ForeColor = Color.Gray;
                    txt.Text = placeholder;
                }
            };
        }


        // set placehover cho tìm kiếm nâng cao sản phẩm
        private void SetComboBoxPlaceholder(ComboBox cbo, string placeholder)
        {

            cbo.ForeColor = Color.Gray;
            cbo.Text = placeholder;

            cbo.GotFocus += (s, e) =>
            {
                if (cbo.Text == placeholder)
                {
                    cbo.Text = "";
                    cbo.ForeColor = Color.Black;
                }
            };
            cbo.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(cbo.Text))
                {
                    cbo.Text = placeholder;
                    cbo.ForeColor = Color.Gray;
                }
            };
        }

        public void loadDanhSachNguyenLieu(List<nguyenLieuDTO> ds)
        {
            if (ds == null || ds.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu nguyên liệu để hiển thị!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            tableNguyenLieu.Columns.Clear();
            tableNguyenLieu.DataSource = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã NL");
            dt.Columns.Add("Tên NL");
            dt.Columns.Add("Đơn vị cơ sở");
            dt.Columns.Add("Trạng thái ĐV");
            dt.Columns.Add("Tồn kho");
            dt.Columns.Add("Mã đơn vị");

            donViBUS bus = new donViBUS();
            List<donViDTO> dsDonVi = bus.layDanhSachDonVi() ?? new List<donViDTO>();

            foreach (var nl in ds)
            {
                string tenDonVi = dsDonVi.FirstOrDefault(l => l.MaDonVi == nl.MaDonViCoSo)?.TenDonVi ?? "Không xác định";
                string trangThaiDV = nl.TrangThaiDV == 1 ? "Đã có hệ số" : "Chưa xét hệ số";
                dt.Rows.Add(nl.MaNguyenLieu, nl.TenNguyenLieu, tenDonVi, trangThaiDV, string.Format("{0:N0}", nl.TonKho), nl.MaDonViCoSo);
            }

            tableNguyenLieu.DataSource = dt;

            if (tableNguyenLieu.Columns.Contains("Mã đơn vị"))
            {
                tableNguyenLieu.Columns["Mã đơn vị"].Visible = false;
            }
            btnSuaNL.Enabled = false;
            btnXoaNL.Enabled = false;
            btnChiTietNL.Enabled = false;
            rdCoBanNL.Checked = true;
            tableNguyenLieu.ReadOnly = true;
            tableNguyenLieu.ClearSelection();
        }

        private void nguyenLieuGUI_Load(object sender, EventArgs e)
        {
            nguyenLieuBUS bus = new nguyenLieuBUS();
            bus.napDSNguyenLieu();

            if (nguyenLieuBUS.ds == null || nguyenLieuBUS.ds.Count == 0)
            {
                MessageBox.Show("Danh sách nguyên liệu đang trống hoặc chưa được tải!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            loadDanhSachNguyenLieu(nguyenLieuBUS.ds);
        }

        private void kiemTraClickTable(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.RowIndex == lastSelectedRowNguyenLieu)
            {
                tableNguyenLieu.ClearSelection();
                lastSelectedRowNguyenLieu = -1;

                btnThemNL.Enabled = true;
                btnSuaNL.Enabled = false;
                btnXoaNL.Enabled = false;
                btnChiTietNL.Enabled = false;
                return;
            }
            tableNguyenLieu.ClearSelection();
            tableNguyenLieu.Rows[e.RowIndex].Selected = true;
            lastSelectedRowNguyenLieu = e.RowIndex;

            btnSuaNL.Enabled = true;
            btnXoaNL.Enabled = true;
            btnChiTietNL.Enabled = true;
        }

        private void btnThemNL_Click(object sender, EventArgs e)
        {
            using (insertNguyenLieu form = new insertNguyenLieu())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
                nguyenLieuBUS bus = new nguyenLieuBUS();
                bus.napDSNguyenLieu();
                loadDanhSachNguyenLieu(nguyenLieuBUS.ds);
            }
        }

        private void btnSuaNL_Click(object sender, EventArgs e)
        {
            if (tableNguyenLieu.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableNguyenLieu.SelectedRows[0];
                nguyenLieuDTO nl = new nguyenLieuDTO
                {
                    MaNguyenLieu = Convert.ToInt32(row.Cells["Mã NL"].Value),
                    TenNguyenLieu = row.Cells["Tên NL"].Value.ToString(),
                    MaDonViCoSo = Convert.ToInt32(row.Cells["Mã đơn vị"].Value),
                    TrangThai = 1
                };

                using (updateNguyenLieu form = new updateNguyenLieu(nl))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }

                nguyenLieuBUS bus = new nguyenLieuBUS();
                bus.docDSNguyenLieu();
                loadDanhSachNguyenLieu(nguyenLieuBUS.ds);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnXoaNL_Click(object sender, EventArgs e)
        {
            if (tableNguyenLieu.SelectedRows.Count > 0)
            {
                int maNL = Convert.ToInt32(tableNguyenLieu.SelectedRows[0].Cells["Mã NL"].Value);
                deleteNguyenLieu form = new deleteNguyenLieu(maNL);
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();

                nguyenLieuBUS bus = new nguyenLieuBUS();
                bus.docDSNguyenLieu();
                loadDanhSachNguyenLieu(nguyenLieuBUS.ds);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnChiTietNL_Click(object sender, EventArgs e)
        {
            if (tableNguyenLieu.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableNguyenLieu.SelectedRows[0];

                nguyenLieuDTO ct = new nguyenLieuDTO();
                ct.MaNguyenLieu = Convert.ToInt32(row.Cells["Mã NL"].Value);
                ct.TenNguyenLieu = row.Cells["Tên NL"].Value.ToString();
                ct.TenDonViCoSo = row.Cells["Đơn vị cơ sở"].Value.ToString();
                ct.TonKho = Convert.ToInt32(row.Cells["Tồn kho"].Value);

                using (detailNguyenLieu form = new detailNguyenLieu(ct))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRefreshNL_Click(object sender, EventArgs e)
        {
            nguyenLieuBUS bus = new nguyenLieuBUS();
            bus.napDSNguyenLieu();
            loadDanhSachNguyenLieu(nguyenLieuBUS.ds);
            cboTimKiemNL.SelectedIndex = -1;
            cbTrangThaiNL.SelectedIndex = -1;
            txtMinNL.Clear();
            txtMaxNL.Clear();
            txtTenNLTK.Clear();
            txtTimKiemNL.Clear();
            hienThiPlaceHolderNguyenLieu();
        }

        private void btnDonVi_Click(object sender, EventArgs e)
        {
            using (donVivaHeSoGUI form = new donVivaHeSoGUI())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
            nguyenLieuBUS bus = new nguyenLieuBUS();
            bus.napDSNguyenLieu();
            loadDanhSachNguyenLieu(nguyenLieuBUS.ds);
        }

        private void btnExcelNL_Click(object sender, EventArgs e)
        {
            using (selectExcelNguyenLieu form = new selectExcelNguyenLieu())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
            nguyenLieuBUS bus = new nguyenLieuBUS();
            bus.docDSNguyenLieu();
            loadDanhSachNguyenLieu(nguyenLieuBUS.ds);
        }

        private void timKiemCoBanNL()
        {
            string tim = txtTimKiemNL.Text.Trim();
            int index = cboTimKiemNL.SelectedIndex;

            if (index == -1 || string.IsNullOrWhiteSpace(tim))
            {
                MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            nguyenLieuBUS bus = new nguyenLieuBUS();
            List<nguyenLieuDTO> ds = bus.timKiemCoBanNL(tim, index);
            if (ds != null && ds.Count > 0)
            {
                tableNguyenLieu.Columns.Clear();
                tableNguyenLieu.DataSource = null;
                loadDanhSachNguyenLieu(ds);
            }
            else
            {
                MessageBox.Show("Không có kết quả tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                bus.docDSNguyenLieu();
                loadDanhSachNguyenLieu(nguyenLieuBUS.ds);

                txtTimKiemNL.Clear();
                cboTimKiemNL.SelectedIndex = -1;
                SetPlaceholder(txtTimKiemNL, "Nhập giá trị cần tìm");
                SetComboBoxPlaceholder(cboTimKiemNL, "Chọn giá trị TK");
            }

        }

        private void timKiemNangCaoNL()
        {

        }

        // dòng này là để cho khi mà mình load trang nó kh chọn dòng đầu nha
        private void tableNguyenLieu_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tableNguyenLieu.ClearSelection();
        }

        private void rdCoBanNL_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rdCoBanNL.Checked)
            {
                rdNcNL.Checked = false;
                txtMinNL.Enabled = false;
                txtMaxNL.Enabled = false;
                cbTrangThaiNL.Enabled = false;
                txtTenDonViTK.Enabled = false;
                txtTenNLTK.Enabled = false;
            }
            else
            {
                txtMinNL.Enabled = true;
                txtMaxNL.Enabled = true;
                cbTrangThaiNL.Enabled = true;
                txtTenDonViTK.Enabled = true;
                txtTenNLTK.Enabled = true;
            }
        }

        private void btnTimKiemNL_Click(object sender, EventArgs e)
        {
            if (rdCoBanNL.Checked)
            {
                timKiemCoBanNL();
            }
            else
            {
                timKiemNangCaoNL();
            }
        }

        private void rdNcNL_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rdNcNL.Checked)
            {
                rdCoBanNL.Checked = false;
                cboTimKiemNL.Enabled = false;
                txtTimKiemNL.Enabled = false;
            }
            else
            {
                cboTimKiemNL.Enabled = true;
                txtTimKiemNL.Enabled = true;
            }
        }
    }
}
