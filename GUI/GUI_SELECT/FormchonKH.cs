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
    public partial class FormchonKH : Form
    {
        public int MaKHChon { get; private set; }
        public string TenKHChon { get; private set; }

        private khachHangDAO khDAO = new khachHangDAO();
        private BindingList<khachHangDTO> dsKhachHang;
        public FormchonKH()
        {
            InitializeComponent();
            this.Text = "Chọn Khách Hàng";
            this.StartPosition = FormStartPosition.CenterParent;
            LoadDanhSachKhachHang();
            txtTimKiem.Focus();
            loadFontChuVaSizeKH();
        }
        private void loadFontChuVaSizeKH()
        {
            foreach (DataGridViewColumn col in dgvKhachHang.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            dgvKhachHang.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvKhachHang.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            dgvKhachHang.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            dgvKhachHang.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKhachHang.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvKhachHang.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgvKhachHang.Refresh();
        }
        private void LoadDanhSachKhachHang()
        {
            dsKhachHang = khDAO.LayDanhSach(); // Lấy tất cả khách hàng
            dgvKhachHang.AutoGenerateColumns = false;
            dgvKhachHang.DataSource = dsKhachHang;

            // Tạo cột
            dgvKhachHang.Columns.Clear();
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaKhachHang", HeaderText = "Mã KH" });
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenKhachHang", HeaderText = "Tên khách hàng" });
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoDienThoai", HeaderText = "Số điện thoại" });
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email" });

            // Định dạng đẹp
            dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKhachHang.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvKhachHang.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            dgvKhachHang.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
            dgvKhachHang.ClearSelection();
        }

        private void ChonKhachHang()
        {
            if (dgvKhachHang.CurrentRow != null)
            {
                var kh = (khachHangDTO)dgvKhachHang.CurrentRow.DataBoundItem;
                MaKHChon = kh.MaKhachHang;
                TenKHChon = kh.TenKhachHang;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        private void btnChon_Click(object sender, EventArgs e)
        {
            ChonKhachHang();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void dgvKhachHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChonKhachHang();
                e.Handled = true;
            }
        }

        private void dgvKhachHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ChonKhachHang();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(tuKhoa))
            {
                dgvKhachHang.DataSource = dsKhachHang;
            }
            else
            {
                var ketQua = dsKhachHang.Where(kh =>
                    kh.TenKhachHang.ToLower().Contains(tuKhoa) ||
                    kh.SoDienThoai.Contains(tuKhoa) ||
                    kh.MaKhachHang.ToString().Contains(tuKhoa)
                ).ToList();

                dgvKhachHang.DataSource = ketQua;
            }
        }

        private void dgvKhachHang_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvKhachHang.ClearSelection();
        }
    }
}
