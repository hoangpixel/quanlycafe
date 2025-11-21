using DAO;
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
    public partial class FormchonNV : Form
    {
        public int MaNVChon { get; private set; }
        public string TenNVChon { get; private set; }

        private nhanVienDAO nvDAO = new nhanVienDAO();
        private BindingList<nhanVienDTO> dsNhanVien;
        public FormchonNV()
        {
            InitializeComponent();
            this.Text = "Chọn Nhân Viên";
            this.StartPosition = FormStartPosition.CenterParent;
            LoadDanhSachNhanVien();
            txtTimKiem.Focus();
            loadFontChuVaSizeNV();
        }

        private void loadFontChuVaSizeNV()
        {
            foreach (DataGridViewColumn col in dgvNhanVien.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            dgvNhanVien.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvNhanVien.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            dgvNhanVien.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            dgvNhanVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvNhanVien.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvNhanVien.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgvNhanVien.Refresh();
        }
        private void LoadDanhSachNhanVien()
        {
            dsNhanVien = nvDAO.LayDanhSach(); // Lấy tất cả khách hàng
            dgvNhanVien.AutoGenerateColumns = false;
            dgvNhanVien.DataSource = dsNhanVien;

            // Tạo cột
            dgvNhanVien.Columns.Clear();
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNhanVien", HeaderText = "Mã NV" });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HoTen", HeaderText = "Tên Nhân Viên" });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoDienThoai", HeaderText = "Số điện thoại" });
            dgvNhanVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email" });

            // Định dạng đẹp
            dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNhanVien.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvNhanVien.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            dgvNhanVien.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            dgvNhanVien.ClearSelection();
        }

        private void ChonNhanVien()
        {
            if (dgvNhanVien.CurrentRow != null)
            {
                var nv = (nhanVienDTO)dgvNhanVien.CurrentRow.DataBoundItem;
                MaNVChon = nv.MaNhanVien;
                TenNVChon = nv.HoTen;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnChon_Click(object sender, EventArgs e)
        {
            ChonNhanVien();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void dgvNhanVien_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChonNhanVien();
                e.Handled = true;
            }
        }

        private void dgvNhanVien_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ChonNhanVien();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(tuKhoa))
            {
                dgvNhanVien.DataSource = dsNhanVien;
            }
            else
            {
                var ketQua = dsNhanVien.Where(nv =>
                    nv.HoTen.ToLower().Contains(tuKhoa) ||
                    nv.SoDienThoai.Contains(tuKhoa) ||
                    nv.MaNhanVien.ToString().Contains(tuKhoa)
                ).ToList();

                dgvNhanVien.DataSource = ketQua;
            }
        }

        private void dgvNhanVien_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvNhanVien.ClearSelection();
        }
    }
}
