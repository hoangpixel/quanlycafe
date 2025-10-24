using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;
using BUS;
namespace GUI.GUI_CRUD
{
    public partial class loaiSPGUI : Form
    {
        public loaiSPGUI()
        {
            InitializeComponent();
        }

        private void loadDanhSachLoaiSP(List<loaiDTO> ds)
        {
            tableLoaiSP.Columns.Clear();
            tableLoaiSP.DataSource = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã Loại");
            dt.Columns.Add("Tên Loại");

            foreach (var ct in ds)
            {
                dt.Rows.Add(ct.MaLoai, ct.TenLoai);
            }

            tableLoaiSP.DataSource = dt;
            tableLoaiSP.ReadOnly = true;
        }

        private void loaiSPGUI_Load(object sender, EventArgs e)
        {
            loaiSanPhamBUS bus = new loaiSanPhamBUS();
            bus.docDsLoaiSP();
            loadDanhSachLoaiSP(loaiSanPhamBUS.ds);
            tableLoaiSP.ClearSelection();

            btnSuaLoaiSp.Enabled = false;
            btnXoaLoaiSP.Enabled = false;
        }

        private void btnThemLoaiSP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLoaiSp.Text))
            {
                MessageBox.Show("Vui lòng nhập tên loại sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            loaiSanPhamBUS bus = new loaiSanPhamBUS();
            loaiDTO ct = new loaiDTO();
            ct.TenLoai = txtLoaiSp.Text;

            if (bus.themLoai(ct))
            {              
                MessageBox.Show("Thêm loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtLoaiSp.Clear();
                txtLoaiSp.Focus();
                loadDanhSachLoaiSP(loaiSanPhamBUS.ds);
            }

            else
            {
                MessageBox.Show("Lỗi khi thêm loại sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private int? selectedRowIndex = null;

        private void tableLoaiSP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < tableLoaiSP.Rows.Count)
            {
                // Nếu bấm lại cùng dòng => reset form
                if (selectedRowIndex == e.RowIndex)
                {
                    ResetForm();
                    return;
                }

                // Ghi nhớ dòng được chọn
                selectedRowIndex = e.RowIndex;

                DataGridViewRow row = tableLoaiSP.Rows[e.RowIndex];
                string tenLoai = row.Cells["Tên Loại"].Value.ToString();
                txtLoaiSp.Text = tenLoai;

                // 👉 Khi chọn dòng: ẩn "Thêm", chỉ hiện "Sửa" & "Xóa"
                btnThemLoaiSP.Enabled = false;
                btnSuaLoaiSp.Enabled = true;
                btnXoaLoaiSP.Enabled = true;
            }
        }

        private void ResetForm()
        {
            txtLoaiSp.Clear();
            txtLoaiSp.Focus();
            tableLoaiSP.ClearSelection();

            // 👉 Khi reset: bật lại "Thêm", ẩn "Sửa" & "Xóa"
            btnThemLoaiSP.Enabled = true;
            btnSuaLoaiSp.Enabled = false;
            btnXoaLoaiSP.Enabled = false;

            selectedRowIndex = null;
        }

        private void btnSuaLoaiSp_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tableLoaiSP.SelectedRows[0];
            int maLoai = Convert.ToInt32(row.Cells["Mã Loại"].Value);
            string tenMoi = txtLoaiSp.Text.Trim();
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
                TenLoai = tenMoi
            };

            loaiSanPhamBUS bus = new loaiSanPhamBUS();
            bool kq = bus.suaLoai(ct);

            if (kq)
            {
                MessageBox.Show("Sửa loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bus.docDsLoaiSP();
                loadDanhSachLoaiSP(loaiSanPhamBUS.ds);
                ResetForm();
            }
            else
            {
                MessageBox.Show("Lỗi khi sửa loại sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaLoaiSP_Click(object sender, EventArgs e)
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
                bus.docDsLoaiSP();
                loadDanhSachLoaiSP(loaiSanPhamBUS.ds);
                ResetForm();
            }
            else
            {
                MessageBox.Show("Lỗi khi xóa loại sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
