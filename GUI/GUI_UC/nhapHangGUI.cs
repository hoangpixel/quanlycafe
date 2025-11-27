using BUS;
using DTO;
using FONTS;
using GUI.GUI_CRUD;
using GUI.GUI_SELECT;
using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
namespace GUI.GUI_UC
{
    public partial class nhapHangGUI : UserControl
    {
        private nguyenLieuBUS busNguyenLieu = new nguyenLieuBUS();
        private phieuNhapBUS busPhieuNhap = new phieuNhapBUS();
        private donViBUS busDonVi = new donViBUS();
        private heSoBUS busHeSo = new heSoBUS();

        private BindingList<nguyenLieuDTO> dsNguyenLieu;
        private BindingList<donViDTO> dsDonVi;
        private BindingList<heSoDTO> dsHeSo;
        private BindingList<phieuNhapDTO> dsPhieuNhap;

        private DataTable dtPhieu;
        private int maNguyenLieuChon = -1;

        public class DonViHienThi
        {
            public int MaDonVi { get; set; }
            public string TenDonVi { get; set; }
            public decimal HeSo { get; set; }
        }

        public nhapHangGUI()
        {
            InitializeComponent();
            SetupDataTablePhieu();

            this.Load += nhapHangGUI_Load;


            btnAddToPhieu.Click -= btnAddToPhieu_Click;
            btnAddToPhieu.Click += btnAddToPhieu_Click;
            btnSavePhieu.Click -= btnSavePhieu_Click;
            btnSavePhieu.Click += btnSavePhieu_Click;
            btnDeletePhieu.Click -= btnDeletePhieu_Click;
            btnDeletePhieu.Click += btnDeletePhieu_Click;
            btnRemoveItem.Click -= btnRemoveItem_Click;
            btnRemoveItem.Click += btnRemoveItem_Click;
            btnUpdateItem.Click -= btnUpdateItem_Click;
            btnUpdateItem.Click += btnUpdateItem_Click;
            btnSelectNCC.Click -= btnSelectNCC_Click;
            btnSelectNCC.Click += btnSelectNCC_Click;
            btnChiTietPN.Click += btnChiTietPN_Click;
            btnSelectNhanVien.Click -= btnSelectNhanVien_Click;
            btnSelectNhanVien.Click += btnSelectNhanVien_Click;
            dgvSanPham.CellClick += dgvSanPham_CellClick;
            cboDonVi.SelectedIndexChanged += CboDonVi_SelectedIndexChanged;
            btnSuaPN.Click += btnSuaPN_Click;
            btnRefreshSP.Click += btnRefreshSP_Click;
            buttonSearch.Click += ButtonSearch_Click;
            buttonReset.Click += ButtonReset_Click;
            dgvPhieuNhapList.SelectionChanged += dgvPhieuNhapList_SelectionChanged;
            btnXoaPN.Click += btnXoaPN_Click;
            dgvPhieuNhap.CellClick += dgvPhieuNhap_CellClick;
            btnSortGiam.Click += BtnSortGiam_Click;
            btnSortTang.Click += BtnSortTang_Click;
            btnExcelSP.Click += BtnExcelSP_Click;
            btnNhapHang.Click += btnNhapHang_Click;
            btnThucHienTimKiem.Click += btnThucHienTimKiem_Click;
            btnThemSP.Click += btnThemSP_Click;

        }

        private void nhapHangGUI_Load(object sender, EventArgs e)
        {
            try { FontManager.LoadFont(); FontManager.ApplyFontToAllControls(this); } catch { }
            SetupDataGridViews();
            LoadData();
            txtSoPhieu.Enabled = false;
            txtSoPhieu.Text = "Tự động";
            cboTimKiemSP.Items.Clear();
            cboTimKiemSP.Items.Add("Mã phiếu");
            cboTimKiemSP.Items.Add("Nhà cung cấp");
            cboTimKiemSP.Items.Add("Nhân viên");
            cboTimKiemSP.SelectedIndex = 0;
            btnNhapHang.Enabled = true;
            if (this.Controls.Find("dtpTuNgay", true).Length > 0)
            {
                DateTimePicker dtpTu = (DateTimePicker)this.Controls.Find("dtpTuNgay", true)[0];
                dtpTu.Format = DateTimePickerFormat.Custom;
                dtpTu.CustomFormat = "dd/MM/yyyy";
            }
            if (this.Controls.Find("dtpDenNgay", true).Length > 0)
            {
                DateTimePicker dtpDen = (DateTimePicker)this.Controls.Find("dtpDenNgay", true)[0];
                dtpDen.Format = DateTimePickerFormat.Custom;
                dtpDen.CustomFormat = "dd/MM/yyyy";
            }
            btnTimNgay.Click -= BtnTimNgay_Click;
            btnTimNgay.Click += BtnTimNgay_Click;
        }

        private void SetupDataTablePhieu()
        {
            dtPhieu = new DataTable();
            dtPhieu.Columns.Add("MANL", typeof(int));
            dtPhieu.Columns.Add("TENNL", typeof(string));
            dtPhieu.Columns.Add("MADV", typeof(int));
            dtPhieu.Columns.Add("TENDV", typeof(string));
            dtPhieu.Columns.Add("HESO", typeof(decimal));
            dtPhieu.Columns.Add("SOLUONG", typeof(decimal));
            dtPhieu.Columns.Add("DONGIA", typeof(decimal));
            dtPhieu.Columns.Add("THANHTIEN", typeof(decimal));
            dgvPhieuNhap.DataSource = dtPhieu;
        }

        private void SetupDataGridViews()
        {
            dgvSanPham.AutoGenerateColumns = false;
            dgvSanPham.Columns.Clear();
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "MANL", HeaderText = "Mã Nguyên Liệu", DataPropertyName = "MaNguyenLieu", Width = 60 });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { Name = "TENNL", HeaderText = "Tên Nguyên Liệu", DataPropertyName = "TenNguyenLieu", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            dgvPhieuNhap.AutoGenerateColumns = false;
            dgvPhieuNhap.Columns.Clear();
            dgvPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { Name = "MANL", HeaderText = "Mã NL", DataPropertyName = "MANL", ReadOnly = true, Width = 60 });
            dgvPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { Name = "TENNL", HeaderText = "Tên NL", DataPropertyName = "TENNL", ReadOnly = true, Width = 150 });
            dgvPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { Name = "DVT", HeaderText = "ĐVT", DataPropertyName = "TENDV", ReadOnly = true, Width = 80 });
            dgvPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { Name = "SOLUONG", HeaderText = "SL", DataPropertyName = "SOLUONG", Width = 70 });
            dgvPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { Name = "DONGIA", HeaderText = "Đơn Giá", DataPropertyName = "DONGIA", DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }, Width = 100 });
            dgvPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { Name = "THANHTIEN", HeaderText = "Thành Tiền", DataPropertyName = "THANHTIEN", ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });

            dgvPhieuNhapList.AutoGenerateColumns = false;
            dgvPhieuNhapList.Columns.Clear();

            dgvPhieuNhapList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MAPN",
                HeaderText = "Mã PN",
                DataPropertyName = "MaPN",
                Width = 80
            });
            dgvPhieuNhapList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TENNCC",
                HeaderText = "Nhà Cung Cấp",
                DataPropertyName = "TenNCC",
                Width = 200
            });
            dgvPhieuNhapList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TENNV",
                HeaderText = "Nhân Viên",
                DataPropertyName = "TenNV",
                Width = 150
            });
            dgvPhieuNhapList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NGAYNHAP",
                HeaderText = "Ngày Nhập",
                DataPropertyName = "ThoiGian",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" },
                Width = 150
            });
            dgvPhieuNhapList.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TONGTIEN",
                HeaderText = "Tổng Tiền",
                DataPropertyName = "TongTien",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" },
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        private void LoadData()
        {
            dsNguyenLieu = busNguyenLieu.LayDanhSach();
            dsDonVi = busDonVi.LayDanhSach();
            dsHeSo = busHeSo.LayDanhSach();
            dsPhieuNhap = busPhieuNhap.LayDanhSach();

            dgvSanPham.DataSource = dsNguyenLieu;
            dgvPhieuNhapList.DataSource = dsPhieuNhap;
            ClearInputFields();
        }
        private void btnThucHienTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiemSP.Text.Trim().ToLower();
            string criteria = cboTimKiemSP.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(keyword))
            {
                dgvPhieuNhapList.DataSource = dsPhieuNhap;
                return;
            }
            var filteredList = dsPhieuNhap.Where(pn =>
            {
                switch (criteria)
                {
                    case "Mã phiếu":
                        return pn.MaPN.ToString().Contains(keyword);
                    case "Nhà cung cấp":
                        return pn.TenNCC.ToLower().Contains(keyword);
                    case "Nhân viên":
                        return pn.TenNV.ToLower().Contains(keyword);
                    default:
                        return false;
                }
            }).ToList();


            dgvPhieuNhapList.DataSource = filteredList;
        }
        private void ResetFormChiTiet()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtGia.Clear();
            cboDonVi.DataSource = null;
            numSoLuong.Value = 1;

            maNguyenLieuChon = -1;
            dgvSanPham.ClearSelection();

            btnAddToPhieu.Enabled = false;
            btnUpdateItem.Enabled = true;
            btnRemoveItem.Enabled = true;
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (maNguyenLieuChon == -1)
            {
                MessageBox.Show("Vui lòng chọn một dòng trong bảng 'Phiếu Nhập' (bảng dưới) để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dtPhieu.AsEnumerable()
                             .FirstOrDefault(r => r.Field<int>("MANL") == maNguyenLieuChon);

            if (row != null)
            {
                dtPhieu.Rows.Remove(row);
                ResetFormChiTiet();
            }
        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvSanPham.Rows.Count) return;
            var nl = dgvSanPham.Rows[e.RowIndex].DataBoundItem as nguyenLieuDTO;
            if (nl != null) HienThiChiTietNguyenLieu(nl);
        }

        private void HienThiChiTietNguyenLieu(nguyenLieuDTO nl)
        {
            maNguyenLieuChon = nl.MaNguyenLieu;
            txtMaSP.Text = nl.MaNguyenLieu.ToString();
            txtTenSP.Text = nl.TenNguyenLieu;

            var listDonViHienThi = new List<DonViHienThi>();
            var cacHeSoCuaNL = dsHeSo.Where(x => x.MaNguyenLieu == nl.MaNguyenLieu).ToList();

            foreach (var item in cacHeSoCuaNL)
            {
                var dv = dsDonVi.FirstOrDefault(d => d.MaDonVi == item.MaDonVi);
                if (dv != null)
                {
                    listDonViHienThi.Add(new DonViHienThi
                    {
                        MaDonVi = item.MaDonVi,
                        TenDonVi = dv.TenDonVi,
                        HeSo = item.HeSo
                    });
                }
            }

            if (listDonViHienThi.Count == 0)
            {
                var dvCoSo = dsDonVi.FirstOrDefault(d => d.MaDonVi == nl.MaDonViCoSo);
                if (dvCoSo != null)
                {
                    listDonViHienThi.Add(new DonViHienThi { MaDonVi = dvCoSo.MaDonVi, TenDonVi = dvCoSo.TenDonVi, HeSo = 1 });
                }
            }

            cboDonVi.DataSource = listDonViHienThi;
            cboDonVi.DisplayMember = "TenDonVi";
            cboDonVi.ValueMember = "MaDonVi";

            if (cboDonVi.Items.Count > 0)
            {
                cboDonVi.SelectedIndex = 0;
                CboDonVi_SelectedIndexChanged(null, null);
            }

            numSoLuong.Value = 1;
            btnAddToPhieu.Enabled = true;
        }

        private void dgvPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvPhieuNhap.Rows[e.RowIndex];

            if (row.Cells["MANL"].Value != null && int.TryParse(row.Cells["MANL"].Value.ToString(), out int maNL))
            {
                maNguyenLieuChon = maNL;
            }

            txtMaSP.Text = row.Cells["MANL"].Value.ToString();
            txtTenSP.Text = row.Cells["TENNL"].Value.ToString();

            if (decimal.TryParse(row.Cells["DONGIA"].Value.ToString(), out decimal gia))
            {
                txtGia.Text = gia.ToString("N0");
            }

            if (decimal.TryParse(row.Cells["SOLUONG"].Value.ToString(), out decimal sl))
            {
                numSoLuong.Value = sl;
            }

            btnAddToPhieu.Enabled = true;
            btnUpdateItem.Enabled = true;
            btnRemoveItem.Enabled = true;
        }

        private void CboDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDonVi.SelectedItem is DonViHienThi dvChon)
            {
                decimal giaNhap = dvChon.HeSo * 1;
                txtGia.Text = giaNhap.ToString("N0");
            }
        }

        private void btnAddToPhieu_Click(object sender, EventArgs e)
        {
            if (maNguyenLieuChon == -1) { MessageBox.Show("Chọn nguyên liệu trước!"); return; }
            if (cboDonVi.SelectedItem == null) { MessageBox.Show("Chọn đơn vị tính!"); return; }
            if (numSoLuong.Value <= 0) { MessageBox.Show("Số lượng > 0"); return; }

            DonViHienThi dvChon = cboDonVi.SelectedItem as DonViHienThi;
            decimal sl = numSoLuong.Value;

            decimal donGia = 0;
            decimal.TryParse(txtGia.Text.Replace(",", "").Replace(".", ""), out donGia);
            decimal tt = sl * donGia;

            var row = dtPhieu.AsEnumerable().FirstOrDefault(r =>
                r.Field<int>("MANL") == maNguyenLieuChon &&
                r.Field<int>("MADV") == dvChon.MaDonVi);

            if (row != null)
            {
                row["SOLUONG"] = row.Field<decimal>("SOLUONG") + sl;
                row["THANHTIEN"] = (decimal)row["SOLUONG"] * donGia;
            }
            else
            {
                DataRow newRow = dtPhieu.NewRow();
                newRow["MANL"] = maNguyenLieuChon;
                newRow["TENNL"] = txtTenSP.Text;
                newRow["MADV"] = dvChon.MaDonVi;
                newRow["TENDV"] = dvChon.TenDonVi;
                newRow["HESO"] = dvChon.HeSo;
                newRow["SOLUONG"] = sl;
                newRow["DONGIA"] = donGia;
                newRow["THANHTIEN"] = tt;

                dtPhieu.Rows.Add(newRow);
            }

            dtPhieu.AcceptChanges();
            btnSavePhieu.Enabled = true;

            numSoLuong.Value = 1;
            numSoLuong.Focus();
        }

        private void btnSavePhieu_Click(object sender, EventArgs e)
        {


            if (txtNhaCungCap.Tag == null)
            {
                MessageBox.Show("Nhà cung cấp chưa được chọn!\nVui lòng nhấn nút '...' để chọn.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNhaCungCap.Focus();
                return;
            }

            if (txtNhanVien.Tag == null)
            {
                MessageBox.Show("Nhân viên chưa được chọn!\nVui lòng nhấn nút '...' để chọn.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNhanVien.Focus();
                return;
            }



            try
            {


                List<ctPhieuNhapDTO> details = new List<ctPhieuNhapDTO>();
                decimal tongTienPhieu = 0;

                foreach (DataRow r in dtPhieu.Rows)
                {
                    decimal soLuongNhap = Convert.ToDecimal(r["SOLUONG"]);
                    decimal heSo = Convert.ToDecimal(r["HESO"]);
                    decimal soLuongQuyDoi = soLuongNhap * heSo;
                    decimal thanhTien = Convert.ToDecimal(r["THANHTIEN"]);

                    tongTienPhieu += thanhTien;

                    details.Add(new ctPhieuNhapDTO
                    {
                        MaNguyenLieu = Convert.ToInt32(r["MANL"]),
                        MaDonVi = Convert.ToInt32(r["MADV"]),
                        SoLuong = soLuongNhap,
                        SoLuongCoSo = soLuongQuyDoi,
                        DonGia = Convert.ToDecimal(r["DONGIA"]),
                        ThanhTien = thanhTien
                    });
                }

                phieuNhapDTO pn = new phieuNhapDTO
                {
                    MaPN = 0,
                    MaNCC = Convert.ToInt32(txtNhaCungCap.Tag),
                    MANHANVIEN = Convert.ToInt32(txtNhanVien.Tag),
                    ThoiGian = DateTime.Now,
                    TrangThai = 1,
                    TongTien = tongTienPhieu
                };

                if (busPhieuNhap.ThemPhieuNhap(pn, details) > 0)
                {
                    MessageBox.Show($"Lưu phiếu nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    dtPhieu.Clear();
                    ClearInputFields();

                    dgvPhieuNhapList.DataSource = null;
                    LoadData();
                    tabControl1.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateItem_Click(object sender, EventArgs e)
        {
            if (maNguyenLieuChon == -1)
            {
                MessageBox.Show("Vui lòng chọn một dòng trong bảng 'Phiếu Nhập' (bảng dưới) để cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal soLuongMoi = numSoLuong.Value;
            if (soLuongMoi <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dtPhieu.AsEnumerable()
                             .FirstOrDefault(r => r.Field<int>("MANL") == maNguyenLieuChon);

            if (row != null)
            {
                decimal donGia = row.Field<decimal>("DONGIA");
                row["SOLUONG"] = soLuongMoi;
                row["THANHTIEN"] = soLuongMoi * donGia;

                dtPhieu.AcceptChanges();

                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResetFormChiTiet();
            }
            else
            {
                MessageBox.Show("Không tìm thấy dòng dữ liệu để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            string keyword = textBox9.Text.Trim().ToLower();
            string type = comboBox2.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(keyword))
            {
                dgvSanPham.DataSource = dsNguyenLieu;
                return;
            }

            var filteredList = dsNguyenLieu.Where(x =>
            {
                if (type == "Mã NL")
                    return x.MaNguyenLieu.ToString().Contains(keyword);
                else if (type == "Tên NL")
                    return x.TenNguyenLieu.ToLower().Contains(keyword);
                return false;
            }).ToList();

            dgvSanPham.DataSource = filteredList;
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            textBox9.Clear();
            dgvSanPham.DataSource = dsNguyenLieu;
        }

        private void ClearInputFields()
        {
            ResetFormChiTiet();

            txtSoPhieu.Clear();
            txtSoPhieu.Enabled = false;
            txtSoPhieu.Text = "Tự động";
            txtNhaCungCap.Clear(); txtNhaCungCap.Tag = null;
            txtNhanVien.Clear(); txtNhanVien.Tag = null;

            dtPhieu.Clear();
            btnSavePhieu.Enabled = false;
        }

        private void btnSelectNCC_Click(object sender, EventArgs e)
        {
            using (var f = new selectNhaCungCap())
            {
                if (f.ShowDialog() == DialogResult.OK) { txtNhaCungCap.Tag = f.MaNCC; txtNhaCungCap.Text = f.TenNCC; }
            }
        }

        private void btnSelectNhanVien_Click(object sender, EventArgs e)
        {
            using (var f = new selectNhanVien())
            {
                if (f.ShowDialog() == DialogResult.OK) { txtNhanVien.Tag = f.MaNV; txtNhanVien.Text = f.TenNV; }
            }
        }

        private void btnDeletePhieu_Click(object sender, EventArgs e) { dtPhieu.Clear(); btnSavePhieu.Enabled = false; }
        private void btnRefreshSP_Click(object sender, EventArgs e)
        {
            txtTimKiemSP.Clear();
            cboTimKiemSP.SelectedIndex = -1;

            LoadData();

            dgvPhieuNhapList.ClearSelection();
            btnXoaPN.Enabled = false;
            btnXoaPN.BackColor = Color.Silver;

            MessageBox.Show("Đã làm mới danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        

        private void txtSoPhieu_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvPhieuNhapList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPhieuNhapList.SelectedRows.Count > 0)
            {
               
                btnXoaPN.Enabled = true;
                btnSuaPN.Enabled = true;
                btnChiTietPN.Enabled = true;

                btnThemSP.Enabled = true; 

                btnXoaPN.BackColor = Color.IndianRed;
            }
            else
            {
                btnXoaPN.Enabled = false;
                btnSuaPN.Enabled = false;
                btnChiTietPN.Enabled = false;
                btnThemSP.Enabled = false; 
                btnXoaPN.BackColor = Color.Silver;
            }
        }
        private void btnXoaPN_Click(object sender, EventArgs e)
        {
            if (dgvPhieuNhapList.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
                
            DataGridViewRow row = dgvPhieuNhapList.SelectedRows[0];
            int maPN = Convert.ToInt32(row.Cells["MAPN"].Value);

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa Phiếu nhập ?\n\nLƯU Ý: Hành động này sẽ xóa toàn bộ chi tiết hàng hóa bên trong phiếu!",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                if (busPhieuNhap.XoaPhieu(maPN))
                {
                    MessageBox.Show("Xóa phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại! Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnChiTietPN_Click(object sender, EventArgs e)
        {

            if (dgvPhieuNhapList.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập cần xem chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataGridViewRow row = dgvPhieuNhapList.SelectedRows[0];
            if (row.Cells["MAPN"].Value == null) return;
            int maPN = Convert.ToInt32(row.Cells["MAPN"].Value);
            using (detailPhieuNhap f = new detailPhieuNhap(maPN))
            {
                f.ShowDialog();
            }
        }


        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn file phiếu nhập Excel";
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Excel.Application excelApp = null;
                Excel.Workbook workbook = null;
                Excel.Worksheet worksheet = null;

                try
                {
                    excelApp = new Excel.Application();
                    workbook = excelApp.Workbooks.Open(openFileDialog.FileName);
                    worksheet = (Excel.Worksheet)workbook.Sheets[1];


                    var cellMaPN = (worksheet.Cells[3, 3] as Excel.Range).Value2;
                    int maPN = 0;

                    if (cellMaPN != null && int.TryParse(cellMaPN.ToString(), out maPN))
                    {

                        if (busPhieuNhap.KiemTraTonTai(maPN))
                        {
                            MessageBox.Show($"Cảnh báo: Mã phiếu '{maPN}' ĐÃ TỒN TẠI!\nKhông thể nhập file này.",
                                            "Trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }


                        Excel.Range usedRange = worksheet.UsedRange;
                        int rowCount = usedRange.Rows.Count;
                        int countSuccess = 0;
                        string errorLog = "";

                        dtPhieu.Clear();

                        for (int i = 9; i <= rowCount; i++)
                        {
                            var cellMaNL = (worksheet.Cells[i, 2] as Excel.Range).Value2; // Cột B: Mã NL
                            var cellSL = (worksheet.Cells[i, 5] as Excel.Range).Value2;   // Cột E: Số Lượng
                            var cellGia = (worksheet.Cells[i, 6] as Excel.Range).Value2;  // Cột F: Đơn Giá

                            if (cellMaNL == null || cellMaNL.ToString().ToLower().Contains("tổng")) break;

                            // --- SỬA LỖI BIẾN CHƯA KHỞI TẠO TẠI ĐÂY ---
                            int maNL = 0;
                            decimal soLuong = 0;
                            decimal donGia = 0; // Gán giá trị mặc định = 0

                            if (int.TryParse(Convert.ToString(cellMaNL), out maNL) &&
                                decimal.TryParse(Convert.ToString(cellSL), out soLuong) &&
                                decimal.TryParse(Convert.ToString(cellGia), out donGia))
                            {
                                // Tìm thông tin nguyên liệu để hiển thị tên và đơn vị
                                var nl = busNguyenLieu.LayDanhSach().FirstOrDefault(x => x.MaNguyenLieu == maNL);

                                if (nl != null)
                                {
                                    var dv = busDonVi.LayDanhSach().FirstOrDefault(d => d.MaDonVi == nl.MaDonViCoSo);

                                    // Thêm dòng vào DataTable (dtPhieu)
                                    DataRow r = dtPhieu.NewRow();
                                    r["MANL"] = maNL;
                                    r["TENNL"] = nl.TenNguyenLieu;
                                    r["MADV"] = nl.MaDonViCoSo;
                                    r["TENDV"] = dv != null ? dv.TenDonVi : "N/A";
                                    r["HESO"] = 1;
                                    r["SOLUONG"] = soLuong;
                                    r["DONGIA"] = donGia;
                                    r["THANHTIEN"] = soLuong * donGia;

                                    dtPhieu.Rows.Add(r);
                                    countSuccess++;
                                }
                                else
                                {
                                    errorLog += $"- Dòng {i}: Mã NL {maNL} không tồn tại trong hệ thống.\n";
                                }
                            }
                        }


                        if (countSuccess > 0)
                        {
                            btnSavePhieu.Enabled = true;
                            txtSoPhieu.Text = maPN.ToString();
                            MessageBox.Show($"Đã nhập {countSuccess} sản phẩm từ Excel.\nVui lòng chọn NCC, Nhân viên và bấm 'Lưu Phiếu'.",
                                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        if (!string.IsNullOrEmpty(errorLog))
                        {
                            MessageBox.Show("Các lỗi nhỏ:\n" + errorLog, "Chú ý");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mã phiếu tại không hợp lệ.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (workbook != null) { workbook.Close(false); Marshal.ReleaseComObject(workbook); }
                    if (excelApp != null) { excelApp.Quit(); Marshal.ReleaseComObject(excelApp); }
                }
            }
        }
        private void BtnExcelSP_Click(object sender, EventArgs e)
        {

            if (dgvPhieuNhapList.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập trong danh sách để xuất chi tiết!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {

                DataGridViewRow row = dgvPhieuNhapList.SelectedRows[0];
                int maPN = Convert.ToInt32(row.Cells["MAPN"].Value);
                string tenNCC = row.Cells["TENNCC"].Value.ToString();
                string tenNV = row.Cells["TENNV"].Value.ToString();
                string ngayNhap = Convert.ToDateTime(row.Cells["NGAYNHAP"].Value).ToString("dd/MM/yyyy HH:  mm");
                string tongTien = row.Cells["TONGTIEN"].Value.ToString();

                var listChiTiet = busPhieuNhap.LayChiTiet(maPN);

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                saveDialog.FileName = $"ChiTietPhieu_{maPN}_{DateTime.Now:ddMMyy}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {

                    excelApp = new Excel.Application();
                    excelApp.Visible = true;
                    workbook = excelApp.Workbooks.Add(Type.Missing);
                    worksheet = (Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "ChiTiet_" + maPN;
                    Excel.Range titleRange = worksheet.Range["A1", "F1"];
                    titleRange.Merge();
                    titleRange.Value = "CHI TIẾT PHIẾU NHẬP HÀNG";
                    titleRange.Font.Size = 16;
                    titleRange.Font.Bold = true;
                    titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;


                    worksheet.Cells[3, 2] = "Mã Phiếu:"; worksheet.Cells[3, 3] = maPN;
                    worksheet.Cells[4, 2] = "Nhà Cung Cấp:"; worksheet.Cells[4, 3] = tenNCC;
                    worksheet.Cells[5, 2] = "Nhân Viên:"; worksheet.Cells[5, 3] = tenNV;
                    worksheet.Cells[6, 2] = "Ngày Nhập:"; worksheet.Cells[6, 3] = "'" + ngayNhap;


                    int startRow = 8;
                    string[] headers = { "STT", "Mã NL", "Tên Nguyên Liệu", "ĐVT", "Số Lượng", "Đơn Giá", "Thành Tiền" };

                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[startRow, i + 1] = headers[i];
                        Excel.Range headerCell = worksheet.Cells[startRow, i + 1];
                        headerCell.Font.Bold = true;
                        headerCell.Interior.Color = System.Drawing.Color.LightSkyBlue;
                        headerCell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        headerCell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    }


                    int currentRow = startRow + 1;
                    for (int i = 0; i < listChiTiet.Count; i++)
                    {
                        var item = listChiTiet[i];
                        worksheet.Cells[currentRow, 1] = i + 1;
                        worksheet.Cells[currentRow, 2] = item.MaNguyenLieu;
                        worksheet.Cells[currentRow, 3] = item.TenNguyenLieu;
                        worksheet.Cells[currentRow, 4] = item.TenDonVi;
                        worksheet.Cells[currentRow, 5] = item.SoLuong;
                        worksheet.Cells[currentRow, 6] = item.DonGia;
                        worksheet.Cells[currentRow, 7] = item.ThanhTien;


                        Excel.Range rowRange = worksheet.Range[$"A{currentRow}", $"G{currentRow}"];
                        rowRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                        currentRow++;
                    }

                    worksheet.Cells[currentRow + 1, 6] = "TỔNG CỘNG:";
                    worksheet.Cells[currentRow + 1, 7] = tongTien;
                    worksheet.Range[$"F{currentRow + 1}", $"G{currentRow + 1}"].Font.Bold = true;
                    worksheet.Range[$"F{currentRow + 1}", $"G{currentRow + 1}"].Font.Color = System.Drawing.Color.Red;


                    worksheet.Columns.AutoFit();
                    workbook.SaveAs(saveDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (excelApp != null) excelApp.Quit();
            }
            finally
            {
                if (workbook != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        private void BtnTimNgay_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ketQua = dsPhieuNhap.Where(pn => pn.ThoiGian >= tuNgay && pn.ThoiGian <= denNgay).ToList();
            dgvPhieuNhapList.DataSource = ketQua;

            if (ketQua.Count == 0)
            {
                MessageBox.Show("Không tìm thấy phiếu nhập nào trong khoảng thời gian này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void BtnSortGiam_Click(object sender, EventArgs e)
        {

            if (dsPhieuNhap != null && dsPhieuNhap.Count > 0)
            {
                var sortedList = dsPhieuNhap.OrderByDescending(x => x.TongTien).ToList();
                dgvPhieuNhapList.DataSource = new BindingList<phieuNhapDTO>(sortedList);
            }
        }

        private void BtnSortTang_Click(object sender, EventArgs e)
        {

            if (dsPhieuNhap != null && dsPhieuNhap.Count > 0)
            {
                var sortedList = dsPhieuNhap.OrderBy(x => x.TongTien).ToList();
                dgvPhieuNhapList.DataSource = new BindingList<phieuNhapDTO>(sortedList);
            }
        }
      

 
        private void btnSuaPN_Click(object sender, EventArgs e)
        {
            if (dgvPhieuNhapList.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var pn = dgvPhieuNhapList.SelectedRows[0].DataBoundItem as phieuNhapDTO;

            if (pn == null) return;

            using (GUI.GUI_CRUD.updatePhieuNhap f = new GUI.GUI_CRUD.updatePhieuNhap(pn))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }
        private void btnThemSP_Click(object sender, EventArgs e)
        {
            if (dgvPhieuNhapList.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập để thêm hàng!", "Thông báo");
                return;
            }

            var pn = dgvPhieuNhapList.SelectedRows[0].DataBoundItem as phieuNhapDTO;
            if (pn == null) return;

            using (var f = new GUI.GUI_CRUD.addPhieuNhap(pn))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    LoadData(); 
                }
            }
        }
    }
}