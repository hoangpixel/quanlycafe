using BUS;
using DTO;
using GUI.GUI_SELECT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DAO; // Cần gọi trực tiếp DAO để lấy giá nhanh

namespace GUI.GUI_CRUD
{
    public partial class addPhieuNhap : Form
    {
        private nguyenLieuBUS nlBus = new nguyenLieuBUS();
        private phieuNhapBUS pnBus = new phieuNhapBUS();
        private donViBUS dvBus = new donViBUS();
        private heSoBUS hsBus = new heSoBUS();
        private ctPhieuNhapDAO ctDAO = new ctPhieuNhapDAO();
        private List<ctPhieuNhapDTO> listChiTiet = new List<ctPhieuNhapDTO>();
        private phieuNhapDTO _existingPN = null;

        
        private decimal _basePrice = 0;

        public class DonViItem
        {
            public int MaDonVi { get; set; }
            public string TenDonVi { get; set; }
            public decimal HeSo { get; set; }
        }

        public addPhieuNhap(phieuNhapDTO pn)
        {
            InitializeComponent();
            _existingPN = pn;
        }

        public addPhieuNhap()
        {
            InitializeComponent();
        }

        private void addPhieuNhap_Load(object sender, EventArgs e)
        {
            // Set thời gian hiện tại (sẽ được update vào DB khi lưu)
            txtNgayNhap.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            LoadNguyenLieu();
            FormatGrid();

            // KHÓA Ô NHẬP GIÁ THEO YÊU CẦU
            txtDonGia.Enabled = false;

            if (_existingPN != null)
            {
                lblTitle.Text = "BỔ SUNG NGUYÊN LIỆU - PHIẾU " + _existingPN.MaPN;
                txtNhaCungCap.Text = _existingPN.TenNCC;
                txtNhanVien.Text = _existingPN.TenNV;
                txtNhaCungCap.Tag = _existingPN.MaNCC;
                txtNhanVien.Tag = _existingPN.MANHANVIEN;
            }

            cboHang.SelectedIndexChanged += cboHang_SelectedIndexChanged;
            cboDonVi.SelectedIndexChanged += cboDonVi_SelectedIndexChanged;
        }

        private void LoadNguyenLieu()
        {
            var listNL = nlBus.LayDanhSach();
            cboHang.DataSource = listNL;
            cboHang.DisplayMember = "TenNguyenLieu";
            cboHang.ValueMember = "MaNguyenLieu";
            cboHang.SelectedIndex = -1;
        }

        private void FormatGrid()
        {
            dgvChiTiet.Columns.Clear();
            dgvChiTiet.AutoGenerateColumns = false;
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã NL", DataPropertyName = "MaNguyenLieu", Width = 70 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên Hàng", DataPropertyName = "TenNguyenLieu", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ĐVT", DataPropertyName = "TenDonVi", Width = 80 });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "SL", DataPropertyName = "SoLuong", Width = 70, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Đơn Giá", DataPropertyName = "DonGia", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Thành Tiền", DataPropertyName = "ThanhTien", Width = 150, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight } });
        }

        private void cboHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboHang.SelectedItem == null) return;

            var nl = cboHang.SelectedItem as nguyenLieuDTO;

            // 1. LẤY GIÁ NHẬP GẦN NHẤT TỪ LỊCH SỬ (DATABASE)
            // Nếu chưa từng nhập, giá = 0
            _basePrice = ctDAO.GetGiaNhapGanNhat(nl.MaNguyenLieu);

            // Load danh sách đơn vị
            List<DonViItem> listDV = new List<DonViItem>();
            var dvCoSo = dvBus.LayDanhSach().FirstOrDefault(x => x.MaDonVi == nl.MaDonViCoSo);
            if (dvCoSo != null)
            {
                listDV.Add(new DonViItem { MaDonVi = dvCoSo.MaDonVi, TenDonVi = dvCoSo.TenDonVi, HeSo = 1 });
            }

            var listHeSo = hsBus.LayDanhSach().Where(x => x.MaNguyenLieu == nl.MaNguyenLieu).ToList();
            foreach (var hs in listHeSo)
            {
                var dvQD = dvBus.LayDanhSach().FirstOrDefault(d => d.MaDonVi == hs.MaDonVi);
                if (dvQD != null)
                {
                    listDV.Add(new DonViItem { MaDonVi = hs.MaDonVi, TenDonVi = dvQD.TenDonVi, HeSo = hs.HeSo });
                }
            }

            cboDonVi.DataSource = listDV;
            cboDonVi.DisplayMember = "TenDonVi";
            cboDonVi.ValueMember = "MaDonVi";

            if (listDV.Count > 0) cboDonVi.SelectedIndex = 0;
            numSL.Value = 1;
        }

        private void cboDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDonVi.SelectedItem is DonViItem dv)
            {
                
                decimal giaMoi = _basePrice * dv.HeSo;
                txtDonGia.Text = giaMoi.ToString("N0");
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (cboHang.SelectedItem == null) { MessageBox.Show("Chưa chọn hàng!"); return; }
            if (cboDonVi.SelectedItem == null) { MessageBox.Show("Chưa chọn đơn vị!"); return; }

            decimal gia = 0;
            decimal.TryParse(txtDonGia.Text.Replace(",", "").Replace(".", ""), out gia);


            var nl = cboHang.SelectedItem as nguyenLieuDTO;
            var dv = cboDonVi.SelectedItem as DonViItem;

            var existing = listChiTiet.FirstOrDefault(x => x.MaNguyenLieu == nl.MaNguyenLieu && x.MaDonVi == dv.MaDonVi);

            if (existing != null)
            {
                existing.SoLuong += numSL.Value;
                existing.SoLuongCoSo += numSL.Value * dv.HeSo;
                existing.ThanhTien = existing.SoLuong * existing.DonGia;
            }
            else
            {
                listChiTiet.Add(new ctPhieuNhapDTO
                {
                    MaNguyenLieu = nl.MaNguyenLieu,
                    TenNguyenLieu = nl.TenNguyenLieu,
                    MaDonVi = dv.MaDonVi,
                    TenDonVi = dv.TenDonVi,
                    SoLuong = numSL.Value,
                    SoLuongCoSo = numSL.Value * dv.HeSo,
                    DonGia = gia,
                    ThanhTien = numSL.Value * gia,
                    MaPN = _existingPN != null ? _existingPN.MaPN : 0
                });
            }
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            dgvChiTiet.DataSource = null;
            dgvChiTiet.DataSource = listChiTiet;
            FormatGrid();
            txtTongTien.Text = listChiTiet.Sum(x => x.ThanhTien).ToString("N0");
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (listChiTiet.Count == 0) { MessageBox.Show("Danh sách trống!"); return; }

            try
            {
                if (_existingPN != null)
                {
                    foreach (var item in listChiTiet) item.MaPN = _existingPN.MaPN;

                    // Hàm này sẽ update cả TONGTIEN và THOIGIAN trong DB
                    if (pnBus.ThemChiTietVaoPhieuCu(_existingPN.MaPN, listChiTiet))
                    {
                        MessageBox.Show("Đã thêm nguyên liệu thành công!");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    // ... (Code tạo mới giữ nguyên) ...
                    if (txtNhaCungCap.Tag == null || txtNhanVien.Tag == null) { MessageBox.Show("Thiếu thông tin!"); return; }
                    phieuNhapDTO pn = new phieuNhapDTO
                    {
                        MaNCC = Convert.ToInt32(txtNhaCungCap.Tag),
                        MANHANVIEN = Convert.ToInt32(txtNhanVien.Tag),
                        ThoiGian = DateTime.Now,
                        TongTien = listChiTiet.Sum(x => x.ThanhTien)
                    };
                    if (pnBus.ThemPhieuNhap(pn, listChiTiet) > 0)
                    {
                        MessageBox.Show("Thêm phiếu mới thành công!");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}