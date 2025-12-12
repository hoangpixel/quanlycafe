using BUS;
using DAO;
using DTO;
using FONTS;
using GUI.GUI_CRUD;
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
        private khachHangBUS busKhachHang = new khachHangBUS();
        private BindingList<khachHangDTO> dsKhachHang;
        public FormchonKH()
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
            this.Text = "Chọn Khách Hàng";
            this.StartPosition = FormStartPosition.CenterParent;
            
        }
        private void FormchonKH_Load(object sender, EventArgs e)
        {
            dsKhachHang = busKhachHang.LayDanhSach();
            LoadDanhSachKhachHang(dsKhachHang);
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
        private void LoadDanhSachKhachHang(BindingList<khachHangDTO> ds)
        {
            dgvKhachHang.AutoGenerateColumns = false;
            dgvKhachHang.DataSource = ds;

            dgvKhachHang.Columns.Clear();
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaKhachHang", HeaderText = "Mã KH" });
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenKhachHang", HeaderText = "Tên khách hàng" });
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoDienThoai", HeaderText = "Số điện thoại" });
            dgvKhachHang.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email" });

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

        }


        private void dgvKhachHang_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvKhachHang.ClearSelection();
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

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

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (cboTimKiemKH.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboTimKiemKH.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTimKiemKH.Text))
            {
                MessageBox.Show("Vui lòng Nhập giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimKiemKH.Focus();
                return;
            }

            string tim = txtTimKiemKH.Text.Trim();
            int index = cboTimKiemKH.SelectedIndex;

            BindingList<khachHangDTO> dskq = busKhachHang.timKiemCoban(tim, index);
            if (dskq != null && dskq.Count > 0)
            {
                LoadDanhSachKhachHang(dskq);
                loadFontChuVaSizeKH();
            }
            else
            {
                MessageBox.Show("Không tìm thấy giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            cboTimKiemKH.SelectedIndex = -1;
            txtTimKiemKH.Clear();
            BindingList<khachHangDTO> ds = busKhachHang.LayDanhSach();
            LoadDanhSachKhachHang(ds);
            loadFontChuVaSizeKH();
        }

        private void btnKL_Click_1(object sender, EventArgs e)
        {
            MaKHChon = 0;
            TenKHChon = "Khách lẻ";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnThemMoiKhachHang_Click_1(object sender, EventArgs e)
        {
            using (insertKhachHang form = new insertKhachHang(null))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    khachHangDTO ct = form.kh;
                    busKhachHang.them(ct);
                    dgvKhachHang.Refresh();
                    dgvKhachHang.ClearSelection();
                }
            }
        }

        private void dgvKhachHang_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ChonKhachHang();
            }
        }

        private void dgvKhachHang_DataBindingComplete_1(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvKhachHang.ClearSelection();
        }
    }
}
