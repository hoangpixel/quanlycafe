using BUS;
using DTO;
using FONTS;
using GUI.GUI_SELECT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class updatePhieuNhap : Form
    {
        private phieuNhapDTO pn;
        private BindingList<donViDTO> dsDV = new donViBUS().LayDanhSach();
        private BindingList<nhaCungCapDTO> dsNCC = new nhaCungCapBUS().LayDanhSach();
        private BindingList<nhanVienDTO> dsNV = new nhanVienBUS().LayDanhSach();
        private BindingList<nguyenLieuDTO> dsNL = new nguyenLieuBUS().LayDanhSach();
        private int lastSelectedRowNguyenLieu = -1;
        private int lastSelectedRowChiTiet = -1;
        private int maNL = -1, maDV = -1, maNV = -1, maNCC = -1, maPN = -1;
        private phieuNhapBUS busPhieuNhap = new phieuNhapBUS();

        private BindingList<ctPhieuNhapDTO> dsChiTietPNsua = new BindingList<ctPhieuNhapDTO>();

        public updatePhieuNhap(phieuNhapDTO pn)
        {
            InitializeComponent();
            this.pn = pn;
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void updatePhieuNhap_Load(object sender, EventArgs e)
        {
            loadDanhSachNguyenLieu(dsNL);

            maNCC = pn.MaNCC;
            maNV = pn.MaNhanVien;
            maPN = pn.MaPN;

            nhaCungCapDTO ct = dsNCC.FirstOrDefault(x => x.MaNCC == pn.MaNCC);
            txtTenNCC.Text = ct?.TenNCC ?? "Không xác định";

            nhanVienDTO nv = dsNV.FirstOrDefault(x => x.MaNhanVien == pn.MaNhanVien);
            txtTenNV.Text = nv?.HoTen ?? "Không xác định";

            BindingList<ctPhieuNhapDTO> dsCTPN = new BindingList<ctPhieuNhapDTO>(busPhieuNhap.LayChiTiet(maPN));
            dsChiTietPNsua = dsCTPN;
            loadDanhSachCTPN(dsChiTietPNsua);
        }

        private void dgvNguyenLieu_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvNguyenLieu.ClearSelection();
        }
        private void LamMoiOInput()
        {
            numSoLuong.Value = 0;
            txtDonGia.Clear();
            txtTenNL.Clear();
            txtTenDV.Clear();

            maNL = -1;
            maDV = -1;

            btThemCTPN.Enabled = false;
            btnSuaCTPN.Enabled = false;
            btnXoaCTPN.Enabled = false;

            dgvChiTietPN.ClearSelection();
            dgvNguyenLieu.ClearSelection();
        }
        private void dgvNguyenLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            dgvChiTietPN.ClearSelection();
            lastSelectedRowChiTiet = -1;

            if (e.RowIndex == lastSelectedRowNguyenLieu)
            {
                dgvNguyenLieu.ClearSelection();
                lastSelectedRowNguyenLieu = -1;

                LamMoiOInput();
                return;
            }

            dgvNguyenLieu.ClearSelection();
            dgvNguyenLieu.Rows[e.RowIndex].Selected = true;
            lastSelectedRowNguyenLieu = e.RowIndex;

            btThemCTPN.Enabled = true;

            btnSuaCTPN.Enabled = false;
            btnXoaCTPN.Enabled = false;

            DataGridViewRow row = dgvNguyenLieu.SelectedRows[0];
            nguyenLieuDTO nl = row.DataBoundItem as nguyenLieuDTO;

            maNL = nl.MaNguyenLieu;
            maDV = nl.MaDonViCoSo;
            txtTenNL.Text = nl.TenNguyenLieu;

            donViDTO dv = dsDV.FirstOrDefault(x => x.MaDonVi == nl.MaDonViCoSo);
            txtTenDV.Text = dv?.TenDonVi ?? "Không xác định";

            numSoLuong.Value = 0;
            txtDonGia.Clear();
        }

        private void btnChonNCC_Click(object sender, EventArgs e)
        {
            using (var f = new selectNhaCungCap())
            {
                if (f.ShowDialog() == DialogResult.OK) { maNCC = f.MaNCC; txtTenNCC.Text = f.TenNCC; }
            }
        }

        private void btnChonNV_Click(object sender, EventArgs e)
        {
            using (var f = new selectNhanVien())
            {
                if (f.ShowDialog() == DialogResult.OK) { maNV = f.MaNV; txtTenNV.Text = f.TenNV; }
            }
        }

        private void btnChonDV_Click(object sender, EventArgs e)
        {
            using (selectDonVi form = new selectDonVi())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.chiLayHeSo = true;
                form.maNguyenLieu = maNL;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    maDV = form.maDonVi;
                    txtTenDV.Text = form.tenDonVi;
                }
            }
        }

        private void dgvChiTietPN_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            dgvNguyenLieu.ClearSelection();
            lastSelectedRowNguyenLieu = -1;

            if (e.RowIndex == lastSelectedRowChiTiet)
            {
                dgvChiTietPN.ClearSelection();
                lastSelectedRowChiTiet = -1;

                LamMoiOInput();
                return;
            }

            dgvChiTietPN.ClearSelection();
            dgvChiTietPN.Rows[e.RowIndex].Selected = true;
            lastSelectedRowChiTiet = e.RowIndex;
            var item = dgvChiTietPN.Rows[e.RowIndex].DataBoundItem as ctPhieuNhapDTO;
            if (item == null) return;

            numSoLuong.Value = item.SoLuong;
            txtDonGia.Text = item.DonGia.ToString("0.##");

            var nl = dsNL.FirstOrDefault(x => x.MaNguyenLieu == item.MaNguyenLieu);
            txtTenNL.Text = nl?.TenNguyenLieu ?? "";

            var dv = dsDV.FirstOrDefault(x => x.MaDonVi == item.MaDonVi);
            txtTenDV.Text = dv?.TenDonVi ?? "";

            maNL = item.MaNguyenLieu;
            maDV = item.MaDonVi;

            btThemCTPN.Enabled = false;
            btnSuaCTPN.Enabled = true;
            btnXoaCTPN.Enabled = true;
        }

        private void loadFontVaChu(DataGridView table)
        {
            table.EnableHeadersVisualStyles = false;
            table.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            table.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            table.DefaultCellStyle.Font = FontManager.GetLightFont(10);
            table.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);
            table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            table.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            table.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            if (table.Columns.Count > 0)
            {
                foreach (DataGridViewColumn col in table.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            table.Refresh();
        }

        private void loadDanhSachNguyenLieu(BindingList<nguyenLieuDTO> ds)
        {
            dgvNguyenLieu.AutoGenerateColumns = false;
            dgvNguyenLieu.Columns.Clear();

            dgvNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Mã nguyên liệu" });
            dgvNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenNguyenLieu", HeaderText = "Tên nguyên liệu" });
            dgvNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonViCoSo", HeaderText = "Đơn vị gốc" });

            dgvNguyenLieu.DataSource = ds;
            dgvNguyenLieu.ReadOnly = true;
            btnSuaCTPN.Enabled = false;
            btnXoaCTPN.Enabled = false;
            dgvNguyenLieu.ClearSelection();
            loadFontVaChu(dgvNguyenLieu);
        }

        private void dgvNguyenLieu_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            nguyenLieuDTO nl = dgvNguyenLieu.Rows[e.RowIndex].DataBoundItem as nguyenLieuDTO;
            if (dgvNguyenLieu.Columns[e.ColumnIndex].HeaderText == "Đơn vị gốc")
            {
                donViDTO dv = dsDV.FirstOrDefault(x => x.MaDonVi == nl.MaDonViCoSo);
                e.Value = dv?.TenDonVi ?? "Không xác định";
            }
        }

        private void loadDanhSachCTPN(BindingList<ctPhieuNhapDTO> ds)
        {
            BindingList<heSoDTO> dsHeSo = new heSoBUS().LayDanhSach();

            foreach (var item in ds)
            {
                if (item.HeSo <= 0)
                {
                    var heSoObj = dsHeSo.FirstOrDefault(x => x.MaDonVi == item.MaDonVi && x.MaNguyenLieu == item.MaNguyenLieu);
                    item.HeSo = heSoObj?.HeSo ?? 1;
                }
            }

            dgvChiTietPN.AutoGenerateColumns = false;
            dgvChiTietPN.Columns.Clear();

            dgvChiTietPN.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Mã NL" });
            dgvChiTietPN.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Tên NL" });
            dgvChiTietPN.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonVi", HeaderText = "Đơn vị" });
            dgvChiTietPN.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "HeSo",
                HeaderText = "Hệ số",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "0.###" }
            });
            dgvChiTietPN.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuong", HeaderText = "SL nhập", DefaultCellStyle = new DataGridViewCellStyle { Format = "0.##" } });
            dgvChiTietPN.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoLuongCoSo",
                HeaderText = "SL tổng",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "0.###" }
            });
            dgvChiTietPN.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DonGia", HeaderText = "Đơn giá", DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
            dgvChiTietPN.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ThanhTien", HeaderText = "Thành tiền", DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });

            dgvChiTietPN.DataSource = ds;
            dgvChiTietPN.ReadOnly = true;
            dgvNguyenLieu.ClearSelection();
            loadFontVaChu(dgvChiTietPN);
        }
        private void dgvChiTietPN_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvChiTietPN.Rows.Count) return;
            string header = dgvChiTietPN.Columns[e.ColumnIndex].HeaderText;
            ctPhieuNhapDTO ctpn = dgvChiTietPN.Rows[e.RowIndex].DataBoundItem as ctPhieuNhapDTO;
            if (ctpn == null) return;
            if (header == "Tên NL")
            {
                if (dsNL != null)
                {
                    nguyenLieuDTO nl = dsNL.FirstOrDefault(x => x.MaNguyenLieu == ctpn.MaNguyenLieu);
                    e.Value = nl?.TenNguyenLieu ?? "Không tìm thấy";
                }
            }
            if (header == "Đơn vị")
            {
                if (dsDV != null)
                {
                    donViDTO dv = dsDV.FirstOrDefault(x => x.MaDonVi == ctpn.MaDonVi);
                    e.Value = dv?.TenDonVi ?? "Không tìm thấy";
                }
            }
        }

        private void dgvChiTietPN_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvChiTietPN.ClearSelection();
        }
        private void btThemCTPN_Click(object sender, EventArgs e)
        {
            if (maNL == -1 || maDV == -1 || numSoLuong.Value <= 0 || string.IsNullOrWhiteSpace(txtDonGia.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo");
                return;
            }
            if (!decimal.TryParse(txtDonGia.Text.Trim(), out decimal donGia))
            {
                MessageBox.Show("Đơn giá không hợp lệ!", "Lỗi");
                return;
            }

            BindingList<heSoDTO> dsHeSo = new heSoBUS().LayDanhSach();
            heSoDTO hs = dsHeSo.FirstOrDefault(x => x.MaDonVi == maDV && x.MaNguyenLieu == maNL);
            decimal heSo = hs?.HeSo ?? 1;

            var hangDaCo = dsChiTietPNsua.FirstOrDefault(x => x.MaNguyenLieu == maNL && x.MaDonVi == maDV);

            if (hangDaCo != null)
            {
                DialogResult result = MessageBox.Show(
                    $"Nguyên liệu này đã có trong giỏ.\nBạn có muốn cộng thêm {numSoLuong.Value} đơn vị vào không?",
                    "Xác nhận cộng dồn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    hangDaCo.SoLuong += numSoLuong.Value;

                    hangDaCo.SoLuongCoSo = hangDaCo.SoLuong * heSo;
                    hangDaCo.DonGia = donGia;
                    hangDaCo.ThanhTien = hangDaCo.SoLuong * donGia;

                    dgvChiTietPN.Refresh();
                }
                else
                {
                    return;
                }
            }
            else
            {
                ctPhieuNhapDTO ctpn = new ctPhieuNhapDTO();
                ctpn.MaPN = maPN;
                ctpn.MaNguyenLieu = maNL;
                ctpn.MaDonVi = maDV;
                ctpn.HeSo = heSo;
                ctpn.SoLuong = numSoLuong.Value;
                ctpn.SoLuongCoSo = heSo * numSoLuong.Value;
                ctpn.DonGia = donGia;
                ctpn.ThanhTien = ctpn.SoLuong * ctpn.DonGia;

                dsChiTietPNsua.Add(ctpn);
            }

            LamMoiOInput();
        }

        private void btnSuaCTPN_Click(object sender, EventArgs e)
        {
            if (dgvChiTietPN.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo");
                return;
            }

            if (numSoLuong.Value <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "Lỗi");
                return;
            }
            if (!decimal.TryParse(txtDonGia.Text.Trim(), out decimal donGiaMoi))
            {
                MessageBox.Show("Đơn giá không hợp lệ!", "Lỗi");
                return;
            }

            ctPhieuNhapDTO item = dgvChiTietPN.SelectedRows[0].DataBoundItem as ctPhieuNhapDTO;



            item.SoLuong = numSoLuong.Value;
            item.DonGia = donGiaMoi;

            item.SoLuongCoSo = item.SoLuong * item.HeSo;
            item.ThanhTien = item.SoLuong * item.DonGia;

            dgvChiTietPN.Refresh();

            MessageBox.Show("Đã cập nhật thông tin!", "Thông báo");

            LamMoiOInput();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            cboTimKiemNL.SelectedIndex = -1;
            txtTimKiemNL.Clear();
            BindingList<nguyenLieuDTO> ds = new nguyenLieuBUS().LayDanhSach();
            loadDanhSachNguyenLieu(ds);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (cboTimKiemNL.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboTimKiemNL.Focus();
                return;
            }
            if (busPhieuNhap.kiemTraChuoiRong(txtTimKiemNL.Text))
            {
                MessageBox.Show("Vui lòng nhập giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimKiemNL.Focus();
                return;
            }
            List<nguyenLieuDTO> dsTim = new List<nguyenLieuDTO>();
            string tim = txtTimKiemNL.Text.ToLower().Trim();
            string giaTriTim = cboTimKiemNL.SelectedItem.ToString();

            if (giaTriTim == "Mã nguyên liệu")
            {
                dsTim = (from nl in dsNL
                         where nl.MaNguyenLieu.ToString().Contains(tim)
                         orderby nl.MaNguyenLieu
                         select nl).ToList();
            }
            if (giaTriTim == "Tên nguyên liệu")
            {
                dsTim = (from nl in dsNL
                         where nl.TenNguyenLieu.ToLower().Contains(tim)
                         orderby nl.MaNguyenLieu
                         select nl).ToList();
            }
            if (giaTriTim == "Tên đơn vị")
            {
                dsTim = (from nl in dsNL
                         join dv in dsDV on nl.MaDonViCoSo equals dv.MaDonVi
                         where dv.TenDonVi.ToLower().Contains(tim)
                         orderby dv.MaDonVi
                         select nl).ToList();
            }
            if (dsTim != null && dsTim.Count > 0)
            {
                BindingList<nguyenLieuDTO> dsBinding = new BindingList<nguyenLieuDTO>(dsTim);
                loadDanhSachNguyenLieu(dsBinding);
            }
            else
            {
                MessageBox.Show("Không tim thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnXoaCTPN_Click(object sender, EventArgs e)
        {
            if (dgvChiTietPN.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm trong giỏ để xóa!", "Thông báo");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này khỏi giỏ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var item = dgvChiTietPN.SelectedRows[0].DataBoundItem as ctPhieuNhapDTO;
                dsChiTietPNsua.Remove(item);

                LamMoiOInput();
            }
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (maNCC == -1) { MessageBox.Show("Vui lòng chọn Nhà cung cấp!", "Cảnh báo"); return; }
            if (maNV == -1) { MessageBox.Show("Không xác định được nhân viên!", "Lỗi"); return; }
            if (dsChiTietPNsua.Count == 0) { MessageBox.Show("Danh sách hàng trống!", "Cảnh báo"); return; }

            try
            {
                phieuNhapDTO phieuHeader = new phieuNhapDTO();

                phieuHeader.MaPN = maPN;

                phieuHeader.MaNCC = maNCC;
                phieuHeader.MaNhanVien = maNV;
                phieuHeader.ThoiGian = DateTime.Now;
                phieuHeader.TongTien = dsChiTietPNsua.Sum(x => x.ThanhTien);

                bool ketQua = busPhieuNhap.CapNhatPhieuNhap(phieuHeader, dsChiTietPNsua.ToList());

                if (ketQua)
                {
                    MessageBox.Show("Cập nhật phiếu thành công!", "Thông báo");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi Nghiêm Trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LamMoiForm()
        {
            dsChiTietPNsua.Clear();
            dgvChiTietPN.Refresh();
            txtDonGia.Clear();
            numSoLuong.Value = 0;
            txtTenNL.Clear();
            txtTenDV.Clear();
            txtTenNV.Clear();
            txtTenNCC.Clear();
            cboTimKiemNL.SelectedIndex = -1;
            txtTimKiemNL.Clear();
            maNV = -1;
            maNCC = -1;
            maNL = -1;
            maDV = -1;
        }

        private void btnXoaGio_Click(object sender, EventArgs e)
        {
            if (dsChiTietPNsua.Count == 0) return;

            if (MessageBox.Show("Bạn có chắc muốn xóa TOÀN BỘ sản phẩm trong phiếu này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // Xóa hết danh sách
                dsChiTietPNsua.Clear();

                // Reset form nhập
                LamMoiOInput();
            }
        }

    }
}
