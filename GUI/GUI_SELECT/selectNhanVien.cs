using BUS;
using DTO;
using FONTS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace GUI.GUI_SELECT
{
    public partial class selectNhanVien : Form
    {
        public int MaNV { get; private set; }
        public string TenNV { get; private set; }

        private nhanVienBUS bus = new nhanVienBUS();
        private List<nhanVienDTO> listGoc; // Lưu list gốc để tìm kiếm nhanh

        public selectNhanVien()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Chọn Nhân Viên";
        }

        private void selectNhanVien_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            LoadData();
            loadFontChuVaSizeTableDonVi();
        }

        private void LoadData()
        {
            BindingList<nhanVienDTO> ds = new nhanVienBUS().LayDanhSach();
            listGoc = ds.ToList();
            dgvNV.AutoGenerateColumns = false;
            dgvNV.Columns.Clear();

            dgvNV.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNhanVien", HeaderText = "Mã NV" });
            dgvNV.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HoTen", HeaderText = "Tên NV" });
            dgvNV.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoDienThoai", HeaderText = "Số điện thoại" });
            dgvNV.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email" });

            dgvNV.DataSource = ds;
            dgvNV.ReadOnly = true;
            dgvNV.ClearSelection();
        }
        private void loadFontChuVaSizeTableDonVi()
        {
            foreach (DataGridViewColumn col in dgvNV.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            dgvNV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvNV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvNV.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            dgvNV.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            dgvNV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvNV.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvNV.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgvNV.Refresh();
        }
        // --- XỬ LÝ TÌM KIẾM ---
        private void XulyTimKiem()
        {
            if (listGoc == null) return;

            string keyword = txtTim.Text.ToLower().Trim();
            string tieuChi = cboLoaiTimKiem.Text;

            List<nhanVienDTO> ketQua;

            if (string.IsNullOrEmpty(keyword))
            {
                ketQua = listGoc;
            }
            else
            {
                if (tieuChi == "Mã NV")
                {
                    // Tìm theo Mã
                    ketQua = listGoc.Where(x => x.MaNhanVien.ToString().Contains(keyword)).ToList();
                }
                else if(tieuChi == "Tên NV")
                {
                    // Tìm theo Tên (HoTen)
                    ketQua = listGoc.Where(x => x.HoTen.ToLower().Contains(keyword)).ToList();
                }
                else if(tieuChi == "SĐT")
                {
                    ketQua = listGoc.Where(x => x.SoDienThoai.ToLower().Contains(keyword)).ToList();
                }
                else
                {
                    ketQua = listGoc.Where(x => x.Email.ToLower().Contains(keyword)).ToList();
                }
            }
            dgvNV.DataSource = ketQua;
        }

        // --- CÁC SỰ KIỆN NÚT BẤM (Required bởi Designer) ---

        private void txtTim_TextChanged(object sender, EventArgs e)
        {
            //XulyTimKiem();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            XulyTimKiem();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtTim.Clear();
            cboLoaiTimKiem.SelectedIndex = -1;
            LoadData();
            loadFontChuVaSizeTableDonVi();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChon_Click(object sender, EventArgs e)
        {
            ChonVaDong();
        }

        private void dgvNV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ChonVaDong();
        }

        private void ChonVaDong()
        {
            if (dgvNV.CurrentRow != null)
            {
                var item = dgvNV.CurrentRow.DataBoundItem as nhanVienDTO;
                if (item != null)
                {
                    MaNV = item.MaNhanVien;
                    TenNV = item.HoTen;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên!");
            }
        }

        private void dgvNV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ChonVaDong();
        }
    }
}