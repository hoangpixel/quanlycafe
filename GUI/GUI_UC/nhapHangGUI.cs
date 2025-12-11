using DTO;
using BUS;
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
using System.Diagnostics;
using GUI.GUI_SELECT;
using GUI.GUI_CRUD;

namespace GUI.GUI_UC
{
    public partial class nhapHangGUI : UserControl
    {
        private BindingList<donViDTO> dsDV = new donViBUS().LayDanhSach();
        private BindingList<nhaCungCapDTO> dsNCC = new nhaCungCapBUS().LayDanhSach();
        private BindingList<nhanVienDTO> dsNV = new nhanVienBUS().LayDanhSach();
        private BindingList<nguyenLieuDTO> dsNL = new nguyenLieuBUS().LayDanhSach();
        private int lastSelectedRowNguyenLieu = -1;
        private int maNL = -1, maDV = -1, maNV = -1, maNCC = -1;
        private BindingList<ctPhieuNhapDTO> dsChiTietPN = new BindingList<ctPhieuNhapDTO>();
        private phieuNhapBUS busPhieuNhap = new phieuNhapBUS();
        public nhapHangGUI()
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void nhapHangrielGUI_Load(object sender, EventArgs e)
        {
            loadDanhSachNguyenLieu(dsNL);
            loadDanhSachCTPN(dsChiTietPN);

            BindingList<phieuNhapDTO> ds = new phieuNhapBUS().LayDanhSach();
            loadDanhSachPhieuNhap(ds);

            CheckQuyen();
            tuDongLoadTenNhanVien();
            hienThiPlaceHolderPhieuNhap();
            rdoTimCoBan.Checked = true;

        }


        private void tuDongLoadTenNhanVien()
        {
            if(DTO.Session.NhanVienHienTai != null)
            {
                txtTenNV.Text = DTO.Session.NhanVienHienTai.HoTen;
                maNV = DTO.Session.NhanVienHienTai.MaNhanVien;
            }
        }

        private void CheckQuyen()
        {
            var quyenNX = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 2);
            bool isAdmin = (Session.TaiKhoanHienTai.MAVAITRO == 1);

            bool coQuyenThem = isAdmin || (quyenNX != null && quyenNX.CAN_CREATE == 1);

            numSoLuong.Enabled = coQuyenThem;
            btnChonNCC.Enabled = coQuyenThem;
            btnChonDV.Enabled = coQuyenThem;

            btThemCTPN.Enabled = false;
            btnSuaCTPN.Enabled = coQuyenThem;
            btnXoaCTPN.Enabled = coQuyenThem;

            btnXacNhan.Enabled = coQuyenThem;
            btnExcelPN.Enabled = coQuyenThem;
            button5.Enabled = coQuyenThem;

            btnSuaPN.Enabled = false;
            btnXoaPN.Enabled = false;
            btnChotSo.Enabled = false;
            btnChiTietPN.Enabled = false;
            btnChonDV.Enabled = false;
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

        private void dgvNguyenLieu_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
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
                btnChonDV.Enabled = false;

                LamMoiOInput();
                return;
            }

            dgvNguyenLieu.ClearSelection();
            dgvNguyenLieu.Rows[e.RowIndex].Selected = true;
            lastSelectedRowNguyenLieu = e.RowIndex;

            btThemCTPN.Enabled = true;
            btnChonDV.Enabled = true;

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

        private void loadDanhSachCTPN(BindingList<ctPhieuNhapDTO> ds)
        {
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
            dgvChiTietPN.ClearSelection();
            loadFontVaChu(dgvChiTietPN);
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

            var hangDaCo = dsChiTietPN.FirstOrDefault(x => x.MaNguyenLieu == maNL && x.MaDonVi == maDV);

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

                ctpn.MaNguyenLieu = maNL;
                ctpn.MaDonVi = maDV;
                ctpn.HeSo = heSo;
                ctpn.SoLuong = numSoLuong.Value;
                ctpn.SoLuongCoSo = heSo * numSoLuong.Value;
                ctpn.DonGia = donGia;
                ctpn.ThanhTien = ctpn.SoLuong * ctpn.DonGia;

                dsChiTietPN.Add(ctpn);
                btnChonDV.Enabled = false;
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

            var item = dgvChiTietPN.SelectedRows[0].DataBoundItem as ctPhieuNhapDTO;

            item.SoLuong = numSoLuong.Value;
            item.DonGia = donGiaMoi;

            item.SoLuongCoSo = item.SoLuong * item.HeSo;
            item.ThanhTien = item.SoLuong * item.DonGia;

            dgvChiTietPN.Refresh();

            MessageBox.Show("Đã cập nhật thông tin!", "Thông báo");

            LamMoiOInput();
            btnChonDV.Enabled = false;
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
                dsChiTietPN.Remove(item);

                LamMoiOInput();
                btnChonDV.Enabled = false;
            }
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

        private int lastSelectedRowChiTiet = -1;
        private void dgvChiTietPN_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            dgvNguyenLieu.ClearSelection();
            lastSelectedRowNguyenLieu = -1;

            if (e.RowIndex == lastSelectedRowChiTiet)
            {
                dgvChiTietPN.ClearSelection();
                lastSelectedRowChiTiet = -1;
                btnChonDV.Enabled = false;


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
            btnChonDV.Enabled = true;
        }

        private void dgvChiTietPN_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvChiTietPN.ClearSelection();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa toàn bộ giỏ hay không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var item = dgvChiTietPN.SelectedRows[0].DataBoundItem as ctPhieuNhapDTO;
                dsChiTietPN.Remove(item);

                LamMoiForm();
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

        private void btnChonNV_Click(object sender, EventArgs e)
        {
            using (var f = new selectNhanVien())
            {
                if (f.ShowDialog() == DialogResult.OK) { maNV = f.MaNV; txtTenNV.Text = f.TenNV; }
            }
        }

        private void btnChonNCC_Click(object sender, EventArgs e)
        {
            using (var f = new selectNhaCungCap())
            {
                if (f.ShowDialog() == DialogResult.OK) { maNCC = f.MaNCC; txtTenNCC.Text = f.TenNCC; }
            }
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
                    e.Value = dv?.TenDonVi ?? "Unknown";
                }
            }
        }

        private void LamMoiForm()
        {
            dsChiTietPN.Clear();
            dgvChiTietPN.Refresh();
            txtDonGia.Clear();
            numSoLuong.Value = 0;
            txtTenNL.Clear();
            txtTenDV.Clear();
            txtTenNCC.Clear();
            cboTimKiemNL.SelectedIndex = -1;
            txtTimKiemNL.Clear();
            maNCC = -1;
            maNL = -1;
            maDV = -1;
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (maNCC == -1)
            {
                MessageBox.Show("Vui lòng chọn Nhà cung cấp!", "Cảnh báo");
                return;
            }
            if (maNV == -1)
            {
                MessageBox.Show("Không xác định được nhân viên đang nhập!", "Lỗi");
                return;
            }

            if (dsChiTietPN.Count == 0)
            {
                MessageBox.Show("Danh sách nhập hàng đang trống!", "Cảnh báo");
                return;
            }

            try
            {
                phieuNhapDTO phieuHeader = new phieuNhapDTO();
                phieuHeader.MaNCC = maNCC;
                phieuHeader.MaNhanVien = maNV;
                phieuHeader.ThoiGian = DateTime.Now;
                phieuHeader.TrangThai = 0;
                phieuHeader.TongTien = dsChiTietPN.Sum(x => x.ThanhTien);
                int ketQuaId = busPhieuNhap.ThemPhieuNhap(phieuHeader, dsChiTietPN.ToList());

                if (ketQuaId > 0)
                {
                    MessageBox.Show($"Lưu phiếu thành công! Mã phiếu: {ketQuaId}\n(Trạng thái: Chưa duyệt)", "Thông báo");
                    LamMoiForm();
                    tabControl1.SelectedTab = tabCTPN;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu phiếu: " + ex.Message, "Lỗi Nghiêm Trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadDanhSachPhieuNhap(BindingList<phieuNhapDTO> ds)
        {
            dgvPhieuNhap.AutoGenerateColumns = false;
            dgvPhieuNhap.Columns.Clear();

            dgvPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaPN", HeaderText = "Mã PN" });
            dgvPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNCC", HeaderText = "Tên NCC" });
            dgvPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNhanVien", HeaderText = "Tên NV" });
            dgvPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ThoiGian", HeaderText = "Thời gian tạo", DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" } });
            dgvPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TrangThai", HeaderText = "Trạng thái PN" });
            dgvPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TongTien", HeaderText = "Tổng tiền", DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });

            dgvPhieuNhap.DataSource = ds;
            dgvPhieuNhap.ReadOnly = true;
            loadFontVaChu(dgvPhieuNhap);

            btnSuaPN.Enabled = false;
            btnXoaPN.Enabled = false;
            btnChiTietPN.Enabled = false;
            btnInPDF.Enabled = false;
            dgvPhieuNhap.ClearSelection();
        }

        private void dgvPhieuNhap_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            phieuNhapDTO pn = dgvPhieuNhap.Rows[e.RowIndex].DataBoundItem as phieuNhapDTO;

            if (dgvPhieuNhap.Columns[e.ColumnIndex].HeaderText == "Tên NCC")
            {
                nhaCungCapDTO ncc = dsNCC.FirstOrDefault(x => x.MaNCC == pn.MaNCC);
                e.Value = ncc?.TenNCC ?? "Không xác định";
            }

            if (dgvPhieuNhap.Columns[e.ColumnIndex].HeaderText == "Tên NV")
            {
                nhanVienDTO nv = dsNV.FirstOrDefault(x => x.MaNhanVien == pn.MaNhanVien);
                e.Value = nv?.HoTen ?? "Không xác định";
            }

            if (dgvPhieuNhap.Columns[e.ColumnIndex].HeaderText == "Trạng thái PN")
            {
                e.Value = pn.TrangThai == 1 ? "Đã chốt sổ" : "Chưa chốt sổ"; 
                e.CellStyle.ForeColor = pn.TrangThai == 1 ? Color.Green : Color.Red;
            }
        }

        private void dgvPhieuNhap_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvPhieuNhap.ClearSelection();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            cboTimKiemNL.SelectedIndex = -1;
            txtTimKiemNL.Clear();
            BindingList<nguyenLieuDTO> ds = new nguyenLieuBUS().LayDanhSach();
            loadDanhSachNguyenLieu(ds);
        }

        private void btnTimKiemNL_Click(object sender, EventArgs e)
        {
            if(cboTimKiemNL.SelectedIndex == -1)
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

            if(giaTriTim == "Mã nguyên liệu")
            {
                dsTim = (from nl in dsNL
                         where nl.MaNguyenLieu.ToString().Contains(tim)
                         orderby nl.MaNguyenLieu
                         select nl).ToList();
            }
            if(giaTriTim == "Tên nguyên liệu")
            {
                dsTim = (from nl in dsNL
                         where nl.TenNguyenLieu.ToLower().Contains(tim)
                         orderby nl.MaNguyenLieu
                         select nl).ToList();
            }
            if(giaTriTim == "Tên đơn vị")
            {
                dsTim = (from nl in dsNL
                         join dv in dsDV on nl.MaDonViCoSo equals dv.MaDonVi
                         where dv.TenDonVi.ToLower().Contains(tim)
                         orderby dv.MaDonVi
                         select nl).ToList();
            }
            if(dsTim != null && dsTim.Count > 0)
            {
                BindingList<nguyenLieuDTO> dsBinding = new BindingList<nguyenLieuDTO>(dsTim);
                loadDanhSachNguyenLieu(dsBinding);
            }else
            {
                MessageBox.Show("Không tim thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private int lastSelectedRowPhieuNhap = -1;

        private void dgvPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var quyenNX = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 2);
            bool isAdmin = (Session.TaiKhoanHienTai.MAVAITRO == 1);

            bool coQuyenThem = isAdmin || (quyenNX != null && quyenNX.CAN_CREATE == 1);
            bool coQuyenSua = isAdmin || (quyenNX != null && quyenNX.CAN_UPDATE == 1);
            bool coQuyenXoa = isAdmin || (quyenNX != null && quyenNX.CAN_DELETE == 1);

            if (e.RowIndex == lastSelectedRowPhieuNhap)
            {
                dgvPhieuNhap.ClearSelection();
                lastSelectedRowPhieuNhap = -1;

                btnSuaPN.Enabled = coQuyenThem;
                btnXoaPN.Enabled = false;
                btnChotSo.Enabled = false;

                btnChiTietPN.Enabled = false;
                btnInPDF.Enabled = false;
                btnSuaPN.Enabled = false;
                return;
            }

            dgvPhieuNhap.ClearSelection();
            dgvPhieuNhap.Rows[e.RowIndex].Selected = true;
            lastSelectedRowPhieuNhap = e.RowIndex;

            phieuNhapDTO pn = dgvPhieuNhap.Rows[e.RowIndex].DataBoundItem as phieuNhapDTO;

            if (pn != null)
            {
                bool daKhoaSo = (pn.TrangThai == 1);

                btnSuaPN.Enabled = coQuyenThem && !daKhoaSo;

                btnXoaPN.Enabled = coQuyenXoa && !daKhoaSo;

                btnChotSo.Enabled = coQuyenSua && !daKhoaSo;

                btnChiTietPN.Enabled = true;
                btnInPDF.Enabled = true;
            }
        }

        private void btnChiTietPN_Click(object sender, EventArgs e)
        {
            if (dgvPhieuNhap.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvPhieuNhap.SelectedRows[0];
                phieuNhapDTO pn = row.DataBoundItem as phieuNhapDTO;

                using (detailPhieuNhap f = new detailPhieuNhap(pn))
                {
                    f.ShowDialog();
                }
            }
        }

        private void btnExcelPN_Click(object sender, EventArgs e)
        {
            using(selectExcelPhieuNhapvaCTPN form = new selectExcelPhieuNhapvaCTPN())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
        }

        private void btnInPDF_Click(object sender, EventArgs e)
        {
            if (dgvPhieuNhap.SelectedRows.Count == 0) return;

            var row = dgvPhieuNhap.SelectedRows[0];
            var pnDTO = row.DataBoundItem as phieuNhapDTO;

            if (pnDTO == null) return;

            try
            {
                GUI.GUI_PRINT.inPDFphieuNhap printer = new GUI.GUI_PRINT.inPDFphieuNhap();
                printer.InPhieu(pnDTO.MaPN);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in ấn: " + ex.Message);
            }
        }

        private void btnSuaPN_Click(object sender, EventArgs e)
        {
            if(dgvPhieuNhap.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvPhieuNhap.SelectedRows[0];
                phieuNhapDTO pn = row.DataBoundItem as phieuNhapDTO;

                using(updatePhieuNhap form = new updatePhieuNhap(pn))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if(form.ShowDialog() == DialogResult.OK)
                    {
                        btnSuaPN.Enabled = false;
                        btnXoaPN.Enabled = false;
                        btnChiTietPN.Enabled = false;
                        btnInPDF.Enabled = false;
                        btnChotSo.Enabled = false;
                    }
                }
            }
        }

        private void btnXoaPN_Click(object sender, EventArgs e)
        {
            if (dgvPhieuNhap.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvPhieuNhap.SelectedRows[0];
                phieuNhapDTO pn = row.DataBoundItem as phieuNhapDTO;

                using (deletePhieuNhap form = new deletePhieuNhap())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        int maPN = pn.MaPN;
                        busPhieuNhap.XoaPhieuNhap(maPN);
                        btnSuaPN.Enabled = false;
                        btnXoaPN.Enabled = false;
                        btnChiTietPN.Enabled = false;
                        btnInPDF.Enabled = false;
                        btnChotSo.Enabled = false;
                    }
                }
            }
        }
        private void btnChotSo_Click(object sender, EventArgs e)
        {
            if (dgvPhieuNhap.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvPhieuNhap.SelectedRows[0];
                phieuNhapDTO pn = row.DataBoundItem as phieuNhapDTO;

                using (khoaSoPhieuNhap form = new khoaSoPhieuNhap())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        int maPN = pn.MaPN;
                        busPhieuNhap.DuyetPhieuNhap(maPN);
                        btnSuaPN.Enabled = false;
                        btnXoaPN.Enabled = false;
                        btnChiTietPN.Enabled = false;
                        btnInPDF.Enabled = false;
                        btnChotSo.Enabled = false;
                    }
                }
            }
        }

        private void resetInputTim()
        {
            cboTimKiemPN.SelectedIndex = -1;
            txtTimKiemPN.Clear();
            cboLoaiTrangThai.SelectedIndex = -1;
            txtNccTim.Clear();
            txtNhanVienTim.Clear();
            timeBD.Value = DateTime.Now.AddMonths(-1);
            timeKT.Value = DateTime.Now;
            hienThiPlaceHolderPhieuNhap();
        }

        private void rdoTimCoBan_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoTimCoBan.Checked == true)
            {
                cboTimKiemPN.Enabled = true;
                txtTimKiemPN.Enabled = true;

                cboLoaiTrangThai.Enabled = false;
                txtNccTim.Enabled = false;
                txtNhanVienTim.Enabled = false;
                timeBD.Enabled = false;
                timeKT.Enabled = false;

                rdoTimNangCao.Checked = false;
                resetInputTim();
            }
            else
            {
                cboTimKiemPN.Enabled = false;
                txtTimKiemPN.Enabled = false;

                cboLoaiTrangThai.Enabled = true;
                txtNccTim.Enabled = true;
                txtNhanVienTim.Enabled = true;
                timeBD.Enabled = true;
                timeKT.Enabled = true;
                rdoTimCoBan.Checked = false;
                resetInputTim();

            }
        }

        private void rdoTimNangCao_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoTimNangCao.Checked == true)
            {
                cboLoaiTrangThai.Enabled = true;
                txtNccTim.Enabled = true;
                txtNhanVienTim.Enabled = true;
                timeBD.Enabled = true;
                timeKT.Enabled = true;

                cboTimKiemPN.Enabled = false;
                txtTimKiemPN.Enabled = false;
                rdoTimCoBan.Checked = false;
                resetInputTim();
            }
            else
            {
                cboTimKiemPN.Enabled = true;
                txtTimKiemPN.Enabled = true;

                cboLoaiTrangThai.Enabled = false;
                txtNccTim.Enabled = false;
                txtNhanVienTim.Enabled = false;
                timeBD.Enabled = false;
                timeKT.Enabled = false;
                rdoTimNangCao.Checked = false;
                resetInputTim();

            }
        }

        private void btnThucHienTimKiem_Click(object sender, EventArgs e)
        {
            if(rdoTimCoBan.Checked == true)
            {
                timKiemCoban();
            }else
            {
                timKiemNangCao();
            }
        }

        private void timKiemCoban()
        {
            if(cboTimKiemPN.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboTimKiemPN.Focus();
                return;
            }
            if(string.IsNullOrWhiteSpace(txtTimKiemPN.Text))
            {
                MessageBox.Show("Vui lòng nhập giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimKiemPN.Focus();
                return;
            }
            string tim = txtTimKiemPN.Text.ToLower().Trim();
            int index = cboTimKiemPN.SelectedIndex;

            BindingList<phieuNhapDTO> dsTim = busPhieuNhap.timKiemCoBan(tim, index);
            if(dsTim != null && dsTim.Count > 0)
            {
                loadDanhSachPhieuNhap(dsTim);
            }else
            {
                MessageBox.Show("Không tìm thấy giá trị", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void timKiemNangCao()
        {
            string tenNCC = txtNccTim.Text.Trim();
            if (tenNCC == "Tên NCC") tenNCC = "";

            string tenNV = txtNhanVienTim.Text.Trim();
            if (tenNV == "Tên NV") tenNV = "";

            int trangThaiPN = -1;
            if (cboLoaiTrangThai.SelectedIndex >= 0 && cboLoaiTrangThai.SelectedValue is int val)
            {
                trangThaiPN = val;
            }

            DateTime ngayBD = timeBD.Value.Date;
            DateTime ngayKT = timeKT.Value.Date.AddDays(1).AddSeconds(-1);

            BindingList<phieuNhapDTO> dsTim = busPhieuNhap.timKiemNangCao(trangThaiPN, tenNV, tenNCC, ngayBD, ngayKT);

            loadDanhSachPhieuNhap(dsTim);

            if (dsTim == null || dsTim.Count == 0)
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRefreshPN_Click(object sender, EventArgs e)
        {
            BindingList<phieuNhapDTO> ds = busPhieuNhap.LayDanhSach();
            loadDanhSachPhieuNhap(ds);
            resetInputTim();
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

        private void SetComboBoxPlaceholder(ComboBox cbo, string placeholder)
        {

            cbo.ForeColor = Color.Gray;
            cbo.Text = placeholder;

            cbo.GotFocus += (s, e) =>
            {
                if (cbo.Text == placeholder)
                {
                    cbo.Text = "";
                    cbo.ForeColor = Color.Black;
                }
            };
            cbo.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(cbo.Text))
                {
                    cbo.Text = placeholder;
                    cbo.ForeColor = Color.Gray;
                }
            };
        }

        private void SetupComboBoxData(ComboBox cbo)
        {
            var items = new List<dynamic>
    {
        new { Text = "Tất cả", Value = -1 },
        new { Text = "Chưa", Value = 0 },
        new { Text = "Rồi", Value = 1 }

    };

            cbo.DisplayMember = "Text";
            cbo.ValueMember = "Value";
            cbo.DataSource = items;

            cbo.SelectedIndex = -1;
        }

        private void hienThiPlaceHolderPhieuNhap()
        {
            SetPlaceholder(txtNccTim, "Tên NCC");
            SetPlaceholder(txtNhanVienTim, "Tên NV");
            SetPlaceholder(txtTimKiemPN, "Nhập giá trị cần tìm");
            SetupComboBoxData(cboLoaiTrangThai);

            SetComboBoxPlaceholder(cboLoaiTrangThai, "Trạng thái CT");
            SetComboBoxPlaceholder(cboTimKiemPN, "Chọn giá trị TK");
        }
    }
}
