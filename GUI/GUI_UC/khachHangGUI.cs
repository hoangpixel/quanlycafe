using BUS;
using DTO;
using FONTS;
using GUI.GUI_CRUD;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.GUI_UC
{
    public partial class khachHangGUI : UserControl
    {
        private khachHangBUS bus = new khachHangBUS();
        private int lastSelectedRow = -1;

        public khachHangGUI()
        {
            InitializeComponent();
        }

        private void khachHangGUI_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            // Setup ComboBox Tìm kiếm
            cboLoaiTimKiem.Items.Clear();
            cboLoaiTimKiem.Items.AddRange(new object[] { "Tất cả", "Mã KH", "Tên KH", "SĐT", "Email" });
            cboLoaiTimKiem.SelectedIndex = 0;

            // Setup ComboBox Trạng thái
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new object[] { "Tất cả", "Hoạt động", "Ngừng hoạt động" });
            cboTrangThai.SelectedIndex = 0;

            dgvKH.CellFormatting += dgvKH_CellFormatting;
            SetupDataGridView();
            LoadData();
        }

        private void LoadData()
        {
            dgvKH.DataSource = bus.LayDanhSach();
            ResetButtons();
        }

        // --- CẬP NHẬT HÀM NÀY ĐỂ HIỂN THỊ NGÀY TẠO ---
        private void SetupDataGridView()
        {
            dgvKH.AutoGenerateColumns = false;
            dgvKH.Columns.Clear();

            dgvKH.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaKhachHang", HeaderText = "Mã KH", Width = 80 });
            dgvKH.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenKhachHang", HeaderText = "Tên Khách Hàng", Width = 200 });
            dgvKH.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoDienThoai", HeaderText = "SĐT", Width = 120 });
            dgvKH.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email", Width = 200 });

            // --- THÊM CỘT NGÀY TẠO ---
            dgvKH.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NgayTao",
                HeaderText = "Ngày tạo",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } // Định dạng ngày giờ đẹp
            });
            // --------------------------

            // Cột trạng thái
            var colTrangThai = new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TrangThai",
                HeaderText = "Trạng thái",
                Name = "colTrangThai",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill // Đẩy cột cuối ra lấp đầy
            };
            dgvKH.Columns.Add(colTrangThai);

            // Style chung
            dgvKH.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);
            dgvKH.DefaultCellStyle.Font = FontManager.GetLightFont(10);
            dgvKH.ColumnHeadersHeight = 40;
            dgvKH.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKH.ReadOnly = true;
        }

        private void dgvKH_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Kiểm tra nếu là cột Trạng thái
            if (e.RowIndex >= 0 && dgvKH.Columns[e.ColumnIndex].Name == "colTrangThai" && e.Value != null)
            {
                // Vì DTO đã sửa thành byte, ta parse sang int để so sánh cho dễ
                int status = Convert.ToInt32(e.Value);
                if (status == 1)
                {
                    e.Value = "Hoạt động";
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.SelectionForeColor = Color.LightGreen;
                }
                else
                {
                    e.Value = "Ngừng hoạt động";
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.SelectionForeColor = Color.MistyRose;
                }
                e.FormattingApplied = true;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            using (insertKhachHang frm = new insertKhachHang())
            {
                if (frm.ShowDialog() == DialogResult.OK) LoadData();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvKH.CurrentRow == null) return;
            var kh = dgvKH.CurrentRow.DataBoundItem as khachHangDTO;
            using (updateKhachHang frm = new updateKhachHang(kh))
            {
                if (frm.ShowDialog() == DialogResult.OK) LoadData();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKH.CurrentRow == null) return;
            var kh = dgvKH.CurrentRow.DataBoundItem as khachHangDTO;

            if (MessageBox.Show($"Bạn có chắc muốn xóa khách hàng: {kh.TenKhachHang}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (bus.Xoa(kh.MaKhachHang))
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại (Khách hàng này có thể đang có hóa đơn)!");
                }
            }
        }

        private void btnTim_Click(object sender, EventArgs e) => ThucHienTimKiem();
        private void btnLamMoi_Click(object sender, EventArgs e) { txtTimKiem.Clear(); cboLoaiTimKiem.SelectedIndex = 0; cboTrangThai.SelectedIndex = 0; LoadData(); }

        private void ThucHienTimKiem()
        {
            string kw = txtTimKiem.Text.Trim();
            string type = cboLoaiTimKiem.SelectedItem?.ToString() ?? "Tất cả";
            int status = -1;
            if (cboTrangThai.SelectedIndex == 1) status = 1;
            else if (cboTrangThai.SelectedIndex == 2) status = 0;

            dgvKH.DataSource = bus.TimKiemNangCao(kw, type, status);
        }

        private void dgvKH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            lastSelectedRow = e.RowIndex;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void ResetButtons()
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            dgvKH.ClearSelection();
        }
    }
}