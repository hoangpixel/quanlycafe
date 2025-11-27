using BUS;
using DTO;
using FONTS;
using GUI.GUI_CRUD;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.GUI_UC
{
    public partial class nhaCungCapGUI : UserControl
    {
        private nhaCungCapBUS bus = new nhaCungCapBUS();
        private int lastSelectedRow = -1;

        public nhaCungCapGUI()
        {
            InitializeComponent();
        }

        private void nhaCungCapGUI_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            // Setup ComboBox Loại Tìm Kiếm
            cboLoaiTimKiem.Items.Clear();
            cboLoaiTimKiem.Items.AddRange(new object[] {
                "Tất cả",
                "Mã NCC",
                "Tên NCC",
                "Địa chỉ", // Thêm mới
                "SĐT"      // Thêm mới
            });
            cboLoaiTimKiem.SelectedIndex = 0;

            // Setup ComboBox Trạng Thái
            cboTrangThai.Items.Clear();
            this.cboTrangThai.Items.AddRange(new object[] {
                "Tất cả",
                "Còn hoạt động",
                "Ngừng hoạt động"
            });
            cboTrangThai.SelectedIndex = 0;

            dgvNCC.CellFormatting += dgvNCC_CellFormatting;

            SetupDataGridView();
            LoadData();
        }

        private void LoadData()
        {
            dgvNCC.DataSource = bus.LayDanhSach();
            ResetButtons();
        }

        private void SetupDataGridView()
        {
            dgvNCC.AutoGenerateColumns = false;
            dgvNCC.Columns.Clear();

            dgvNCC.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNCC", HeaderText = "Mã NCC", Width = 80 });
            dgvNCC.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenNCC", HeaderText = "Tên Nhà Cung Cấp", Width = 250 });
            dgvNCC.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DiaChi", HeaderText = "Địa chỉ", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvNCC.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoDienThoai", HeaderText = "SĐT", Width = 120 });
            dgvNCC.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email", Width = 150 });

            var colTrangThai = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ConHoatDong",
                HeaderText = "Trạng thái",
                Name = "colTrangThai",
                Width = 150
            };
            dgvNCC.Columns.Add(colTrangThai);

            dgvNCC.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);
            dgvNCC.DefaultCellStyle.Font = FontManager.GetLightFont(10);
            dgvNCC.ColumnHeadersHeight = 40;
            dgvNCC.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNCC.ReadOnly = true;
        }

        private void dgvNCC_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvNCC.Columns[e.ColumnIndex].Name == "colTrangThai")
            {
                if (e.Value != null && int.TryParse(e.Value.ToString(), out int status))
                {
                    if (status == 1)
                    {
                        e.Value = "Còn hoạt động";
                        e.CellStyle.ForeColor = Color.Green;
                    }
                    else
                    {
                        e.Value = "Ngừng hoạt động";
                        e.CellStyle.ForeColor = Color.Red;
                    }
                    e.FormattingApplied = true;
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            using (insertNhaCungCap frm = new insertNhaCungCap())
            {
                if (frm.ShowDialog() == DialogResult.OK) LoadData();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvNCC.CurrentRow == null) return;
            var ncc = dgvNCC.CurrentRow.DataBoundItem as nhaCungCapDTO;

            // Truyền đối tượng ncc vào form update
            using (updateNhaCungCap frm = new updateNhaCungCap(ncc))
            {
                if (frm.ShowDialog() == DialogResult.OK) LoadData();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNCC.CurrentRow == null) return;
            var ncc = dgvNCC.CurrentRow.DataBoundItem as nhaCungCapDTO;

            if (MessageBox.Show($"Bạn chắc chắn muốn xóa NCC: {ncc.TenNCC}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (bus.Xoa(ncc.MaNCC))
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại (Có thể do ràng buộc dữ liệu)!");
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboLoaiTimKiem.SelectedIndex = 0;
            cboTrangThai.SelectedIndex = 0;
            LoadData();
        }

        private void ThucHienTimKiem()
        {
            string tuKhoa = txtTimKiem.Text.Trim();
            string loaiTK = cboLoaiTimKiem.SelectedItem?.ToString() ?? "Tất cả";

            int trangThai = -1;
            if (cboTrangThai.SelectedItem?.ToString() == "Còn hoạt động") trangThai = 1;
            else if (cboTrangThai.SelectedItem?.ToString() == "Ngừng hoạt động") trangThai = 0;

            dgvNCC.DataSource = bus.TimKiemNangCao(tuKhoa, loaiTK, trangThai);
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e) { }

        private void btnTim_Click(object sender, EventArgs e)
        {
            ThucHienTimKiem();
        }

        private void dgvNCC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.RowIndex == lastSelectedRow) { ResetButtons(); return; }

            lastSelectedRow = e.RowIndex;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void ResetButtons()
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            dgvNCC.ClearSelection();
            lastSelectedRow = -1;
        }
    }
}