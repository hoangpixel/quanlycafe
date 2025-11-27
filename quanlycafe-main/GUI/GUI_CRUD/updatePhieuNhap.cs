using BUS;
using DTO;
using GUI.GUI_SELECT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DAO;

namespace GUI.GUI_CRUD
{
    public partial class updatePhieuNhap : Form
    {
        private phieuNhapDTO _pn;
        private phieuNhapBUS _bus = new phieuNhapBUS();
        private ctPhieuNhapDAO _ctDAO = new ctPhieuNhapDAO();
        private List<ctPhieuNhapDTO> _listChiTiet;

        public updatePhieuNhap(phieuNhapDTO pn)
        {
            InitializeComponent();
            this._pn = pn;
        }

        private void updatePhieuNhap_Load(object sender, EventArgs e)
        {
            if (_pn == null) return;

            // 1. Load Thông Tin Chung
            txtMaPN.Text = _pn.MaPN.ToString();
            txtNhaCungCap.Tag = _pn.MaNCC;
            txtNhaCungCap.Text = _pn.TenNCC;
            txtNhanVien.Tag = _pn.MANHANVIEN;
            txtNhanVien.Text = _pn.TenNV;

            if (txtNgayNhap != null) txtNgayNhap.Text = _pn.ThoiGian.ToString("dd/MM/yyyy HH:mm");
            if (txtTongTien != null) txtTongTien.Text = _pn.TongTien.ToString("N0");

            // 2. Load Chi Tiết
            LoadChiTiet();
        }

        // Hàm này chuyên để định dạng cột (Ẩn cột thừa, đổi tên cột đẹp)
        private void FormatGrid()
        {
            // Ẩn các cột ID kỹ thuật
            string[] colsToHide = { "MaPN", "MaCTPN", "MaDonVi", "MaNguyenLieu", "SoLuongCoSo" };
            foreach (string col in colsToHide)
            {
                if (dgvChiTiet.Columns[col] != null) dgvChiTiet.Columns[col].Visible = false;
            }

            // Đổi tên cột hiển thị
            if (dgvChiTiet.Columns["TenNguyenLieu"] != null) dgvChiTiet.Columns["TenNguyenLieu"].HeaderText = "Tên Nguyên Liệu";
            if (dgvChiTiet.Columns["TenDonVi"] != null) dgvChiTiet.Columns["TenDonVi"].HeaderText = "ĐVT";
            if (dgvChiTiet.Columns["SoLuong"] != null) dgvChiTiet.Columns["SoLuong"].HeaderText = "Số Lượng";

            if (dgvChiTiet.Columns["DonGia"] != null)
            {
                dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn Giá";
                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvChiTiet.Columns["ThanhTien"] != null)
            {
                dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadChiTiet()
        {
            // Lấy dữ liệu từ DB
            _listChiTiet = _bus.LayChiTiet(_pn.MaPN);

            dgvChiTiet.DataSource = null;
            dgvChiTiet.DataSource = _listChiTiet;

            // Gọi hàm định dạng ngay sau khi gán dữ liệu
            FormatGrid();
        }

        private void btnChonNCC_Click(object sender, EventArgs e)
        {
            using (var f = new selectNhaCungCap())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    txtNhaCungCap.Tag = f.MaNCC;
                    txtNhaCungCap.Text = f.TenNCC;
                }
            }
        }

        private void btnChonNV_Click(object sender, EventArgs e)
        {
            using (var f = new selectNhanVien())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    txtNhanVien.Tag = f.MaNV;
                    txtNhanVien.Text = f.TenNV;
                }
            }
        }

        private void btnXoaNL_Click(object sender, EventArgs e)
        {
            if (dgvChiTiet.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedItem = dgvChiTiet.SelectedRows[0].DataBoundItem as ctPhieuNhapDTO;

            if (selectedItem != null)
            {
                if (MessageBox.Show($"Xóa '{selectedItem.TenNguyenLieu}'?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _listChiTiet.Remove(selectedItem);

                 
                    dgvChiTiet.DataSource = null;
                    dgvChiTiet.DataSource = _listChiTiet;

                    FormatGrid();
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNhaCungCap.Tag == null || txtNhanVien.Tag == null) return;

                int maNCC = Convert.ToInt32(txtNhaCungCap.Tag);
                int maNV = Convert.ToInt32(txtNhanVien.Tag);

                // 1. Cập nhật thông tin chung
                if (!_bus.CapNhatThongTinPhieu(_pn.MaPN, maNCC, maNV))
                {
                    MessageBox.Show("Lỗi cập nhật thông tin chung!");
                    return;
                }

                // 2. Xử lý xóa Chi Tiết (LOGIC MỚI: So sánh theo MaCTPN)
                // Lấy danh sách đang có trong Database
                var dbDetails = _bus.LayChiTiet(_pn.MaPN);

                
                var itemsToDelete = dbDetails.Where(db => !_listChiTiet.Any(curr => curr.MaCTPN == db.MaCTPN)).ToList();

                foreach (var item in itemsToDelete)
                {
                   
                    new ctPhieuNhapDAO().DeleteByMaCTPN(item.MaCTPN);
                }

                // 3. Cập nhật lại Tổng tiền
                decimal tongTienMoi = _listChiTiet.Sum(x => x.ThanhTien);
                new phieuNhapDAO().CapNhatTongTienDonGian(_pn.MaPN, tongTienMoi);

                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}