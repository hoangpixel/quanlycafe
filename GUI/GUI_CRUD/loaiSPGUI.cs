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
namespace GUI.GUI_CRUD
{
    public partial class loaiSPGUI : Form
    {
        private int? selectedRowIndex = null;
        public loaiSPGUI()
        {
            InitializeComponent();
        }

        private void loadFontChuVaSize()
        {
            // --- Căn giữa và tắt sort ---
            foreach (DataGridViewColumn col in tableLoaiSP.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableLoaiSP.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableLoaiSP.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // font cho dữ liệu trong table
            tableLoaiSP.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            //font cho header trong table
            tableLoaiSP.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            // --- Fix lỗi mất text khi đổi font ---
            tableLoaiSP.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableLoaiSP.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableLoaiSP.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableLoaiSP.Refresh();
        }

        private void loadDanhSachLoaiSP(BindingList<loaiDTO> ds)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã Loại");
            dt.Columns.Add("Tên Loại");
            dt.Columns.Add("Tên nhóm");

            nhomBUS busNhom = new nhomBUS();
            BindingList<nhomDTO> dsNhom = busNhom.layDanhSach();

            foreach (var ct in ds)
            {
                string tenNhom = dsNhom.FirstOrDefault(l => l.MaNhom == ct.MaNhom)?.TenNhom ?? "Không xác định";
                dt.Rows.Add(ct.MaLoai, ct.TenLoai, tenNhom);
            }

            tableLoaiSP.DataSource = dt;

            tableLoaiSP.ReadOnly = true;

            tableLoaiSP.Columns["Mã Loại"].Width = 90;
            tableLoaiSP.Columns["Tên Loại"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tableLoaiSP.Columns["Tên nhóm"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        public void loadComboNhom()
        {
            nhomBUS bus = new nhomBUS();
            BindingList<nhomDTO> dsNhom = bus.layDanhSach();
            cboNhom.DisplayMember = "TenNhom";
            cboNhom.ValueMember = "MaNhom";
            cboNhom.DataSource = dsNhom;
            cboNhom.SelectedIndex = -1;
        }

        private void loaiSPGUI_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            loaiSanPhamBUS bus = new loaiSanPhamBUS();
            bus.LayDanhSach();
            loadDanhSachLoaiSP(loaiSanPhamBUS.ds);
            loadFontChuVaSize();
            loadComboNhom();
                
            btnSuaLoaiSp.Enabled = false;
            btnXoaLoaiSP.Enabled = false;
        }

        private void ResetForm()
        {
            txtLoaiSp.Clear();
            txtLoaiSp.Focus();
            cboNhom.SelectedIndex = -1;
            tableLoaiSP.ClearSelection();

            btnThemLoaiSP.Enabled = true;
            btnSuaLoaiSp.Enabled = false;
            btnXoaLoaiSP.Enabled = false;

            selectedRowIndex = null;
        }


        private void tableLoaiSP_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < tableLoaiSP.Rows.Count)
            {
                if (selectedRowIndex == e.RowIndex)
                {
                    ResetForm();
                    return;
                }
                selectedRowIndex = e.RowIndex;

                DataGridViewRow row = tableLoaiSP.Rows[e.RowIndex];
                string tenLoai = row.Cells["Tên Loại"].Value.ToString();
                txtLoaiSp.Text = tenLoai;

                string tenNhom = row.Cells["Tên nhóm"].Value.ToString();
                cboNhom.Text = tenNhom;

                btnThemLoaiSP.Enabled = false;
                btnSuaLoaiSp.Enabled = true;
                btnXoaLoaiSP.Enabled = true;
            }
        }

        private void btnThemLoaiSP_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLoaiSp.Text) || cboNhom.SelectedIndex == -1)
            {
                MessageBox.Show("Nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            loaiSanPhamBUS bus = new loaiSanPhamBUS();
            loaiDTO ct = new loaiDTO();
            ct.TenLoai = txtLoaiSp.Text;
            ct.MaNhom = (int)cboNhom.SelectedValue;

            if (bus.themLoai(ct))
            {
                MessageBox.Show("Thêm loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLoaiSp.Clear();
                cboNhom.SelectedIndex = -1;
                txtLoaiSp.Focus();
                tableLoaiSP.ClearSelection();
                bus.LayDanhSach();
                loadDanhSachLoaiSP(loaiSanPhamBUS.ds);
            }

            else
            {
                MessageBox.Show("Lỗi khi thêm loại sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaLoaiSp_Click_1(object sender, EventArgs e)
        {
            DataGridViewRow row = tableLoaiSP.SelectedRows[0];
            int maLoai = Convert.ToInt32(row.Cells["Mã Loại"].Value);
            string tenMoi = txtLoaiSp.Text.Trim();
            int maNhom = (int)cboNhom.SelectedValue;
            if (string.IsNullOrWhiteSpace(tenMoi))
            {
                MessageBox.Show("Vui lòng nhập tên loại sản phẩm mới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn sửa loại sản phẩm này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
                return;

            loaiDTO ct = new loaiDTO
            {
                MaLoai = maLoai,
                TenLoai = tenMoi,
                MaNhom = maNhom
            };

            loaiSanPhamBUS bus = new loaiSanPhamBUS();
            bool kq = bus.suaLoai(ct);

            if (kq)
            {
                MessageBox.Show("Sửa loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bus.LayDanhSach();
                loadDanhSachLoaiSP(loaiSanPhamBUS.ds);
                ResetForm();
            }
            else
            {
                MessageBox.Show("Lỗi khi sửa loại sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaLoaiSP_Click_1(object sender, EventArgs e)
        {
            DataGridViewRow row = tableLoaiSP.SelectedRows[0];
            int maLoai = Convert.ToInt32(row.Cells["Mã Loại"].Value);
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa loại sản phẩm này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
                return;

            loaiSanPhamBUS bus = new loaiSanPhamBUS();
            bool kq = bus.Xoa(maLoai);

            if (kq)
            {
                MessageBox.Show("Xóa loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bus.LayDanhSach();
                loadDanhSachLoaiSP(loaiSanPhamBUS.ds);
                ResetForm();
            }
            else
            {
                MessageBox.Show("Lỗi khi xóa loại sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            if(cboLoai.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtTim.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int index = cboLoai.SelectedIndex;
            string tim = txtTim.Text.Trim();
            loaiSanPhamBUS bus = new loaiSanPhamBUS();
            BindingList<loaiDTO> dskq = bus.timKiemCoBan(tim, index);
            if (dskq != null && dskq.Count > 0)
            {
                loadDanhSachLoaiSP(dskq);
                tableLoaiSP.ClearSelection();
            }else
            {
                MessageBox.Show("Không tìm thấy kết quả cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnRs_Click(object sender, EventArgs e)
        {
            cboLoai.SelectedIndex = -1;
            txtTim.Clear();
            loaiSanPhamBUS bus = new loaiSanPhamBUS();
            bus.LayDanhSach();
            loadDanhSachLoaiSP(loaiSanPhamBUS.ds);
        }

        private void tableLoaiSP_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tableLoaiSP.ClearSelection();
        }
    }
}
