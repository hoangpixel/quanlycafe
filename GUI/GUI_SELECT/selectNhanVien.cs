using BUS;
using DTO;
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
            // Mặc định chọn tiêu chí đầu tiên (Tên NV)
            if (cboLoaiTimKiem.Items.Count > 0)
                cboLoaiTimKiem.SelectedIndex = 0;

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Lấy danh sách từ BUS và chuyển thành List để xử lý tìm kiếm
                listGoc = bus.LayDanhSach().ToList();
                dgvNV.DataSource = listGoc;

                dgvNV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvNV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvNV.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
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
                else
                {
                    // Tìm theo Tên (HoTen)
                    ketQua = listGoc.Where(x => x.HoTen.ToLower().Contains(keyword)).ToList();
                }
            }
            dgvNV.DataSource = ketQua;
        }

        // --- CÁC SỰ KIỆN NÚT BẤM (Required bởi Designer) ---

        private void txtTim_TextChanged(object sender, EventArgs e)
        {
            XulyTimKiem();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            XulyTimKiem();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtTim.Clear();
            LoadData();
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
    }
}