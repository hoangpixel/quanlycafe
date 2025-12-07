using BUS;
using DTO;
using FONTS;
using GUI.GUI_SELECT;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
namespace GUI.GUI_CRUD
{
    public partial class donVivaHeSoGUI : Form
    {
        private int maNL = -1;
        private int maDV = -1;
        private donViBUS busDonVi = new donViBUS();
        private heSoBUS busHeSo = new heSoBUS();
        private BindingList<nguyenLieuDTO> dsNguyenLieu;
        private BindingList<donViDTO> dsDonVi;
        public donVivaHeSoGUI()
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            txtHeSo.DecimalPlaces = 3;
            txtHeSo.Increment = 0.001M;
            txtHeSo.Minimum = 0.000M;
            txtHeSo.Maximum = 999.999M;
        }
        private void donVivaHeSoGUI_Load(object sender, EventArgs e)
        {
            nguyenLieuBUS busNL = new nguyenLieuBUS();
            dsNguyenLieu = busNL.LayDanhSach();

            dsDonVi = busDonVi.LayDanhSach();

            busDonVi.LayDanhSach();
            loadDanhSachDonVi(donViBUS.ds);
            loadFontChuVaSizeTableDonVi();

            busHeSo.LayDanhSach();
            loadDanhSachHeSo(heSoBUS.ds);
            loadFontChuVaSizeTableHeSo();

            tableHeSo.ClearSelection();
            tableDonVi.ClearSelection();
        }
        private void loadFontChuVaSizeTableDonVi()
        {
            foreach (DataGridViewColumn col in tableDonVi.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableDonVi.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableDonVi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tableDonVi.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tableDonVi.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            tableDonVi.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableDonVi.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableDonVi.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableDonVi.Refresh();
        }

        private void loadFontChuVaSizeTableHeSo()
        {
            foreach (DataGridViewColumn col in tableHeSo.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableHeSo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableHeSo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tableHeSo.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tableHeSo.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            tableHeSo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableHeSo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableHeSo.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableHeSo.Refresh();
        }

        private void loadDanhSachDonVi(BindingList<donViDTO> ds)
        {
            tableDonVi.AutoGenerateColumns = false;
            tableDonVi.Columns.Clear();

            tableDonVi.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonVi", HeaderText = "Mã đơn vị" });
            tableDonVi.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenDonVi", HeaderText = "Tên đơn vị" });

            tableDonVi.DataSource = ds;
            btnSuaDonVi.Enabled = false;
            btnXoaDonVi.Enabled = false;
            tableDonVi.ReadOnly = true;
            tableDonVi.ClearSelection();
        }

        private void loadDanhSachHeSo(BindingList<heSoDTO> ds)
        {
            tableHeSo.AutoGenerateColumns = false;
            tableHeSo.Columns.Clear();

            tableHeSo.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Tên NL"});
            tableHeSo.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonVi", HeaderText = "Tên ĐV" });
            tableHeSo.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HeSo", HeaderText = "Hệ số" });

            tableHeSo.DataSource = ds;
            btnSuaHS.Enabled = false;
            btnXoaHeSO.Enabled = false;
            tableHeSo.ReadOnly = true;
            tableHeSo.ClearSelection();
        }

        private void tableHeSo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(e.RowIndex < 0)
            {
                return;
            }

            heSoDTO hs = tableHeSo.Rows[e.RowIndex].DataBoundItem as heSoDTO;

            if (tableHeSo.Columns[e.ColumnIndex].HeaderText == "Tên NL")
            {
                nguyenLieuDTO nl = dsNguyenLieu.FirstOrDefault(x => x.MaNguyenLieu == hs.MaNguyenLieu);
                e.Value = nl?.TenNguyenLieu ?? "Không xác định";
            }

            if (tableHeSo.Columns[e.ColumnIndex].HeaderText == "Tên ĐV")
            {
                donViDTO dv = dsDonVi.FirstOrDefault(x => x.MaDonVi == hs.MaDonVi);
                e.Value = dv?.TenDonVi ?? "Không xác định";
            }

            if (tableHeSo.Columns[e.ColumnIndex].HeaderText == "Hệ số" && e.Value != null)
            {
                if (double.TryParse(e.Value.ToString(), out double tonKho))
                {
                    if (tonKho % 1 == 0)
                    {
                        e.Value = tonKho.ToString("N0");
                    }
                    else
                    {
                        e.Value = tonKho.ToString("0.000");
                    }
                    e.FormattingApplied = true;
                }
            }
        }

        private void btnThemDonVI_Click(object sender, EventArgs e)
        {
            if (busDonVi.kiemTraChuoiRong(txtDonVi.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đơn vị!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonVi.Focus();
                return;
            }

            donViDTO ct = new donViDTO();
            ct.MaDonVi = busDonVi.layMa();
            ct.TenDonVi = txtDonVi.Text.Trim();

            if (ct != null)
            {
                MessageBox.Show("Thêm đơn vị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                busDonVi.themDonVi(ct);
                txtDonVi.Clear();
                txtDonVi.Focus();
            }
        }



        private void ResetFormDV()
        {
            txtDonVi.Clear();
            txtDonVi.Focus();
            tableDonVi.ClearSelection();

            btnThemDonVI.Enabled = true;
            btnXoaDonVi.Enabled = false;
            btnSuaDonVi.Enabled = false;

            selectedRowIndex = null;
        }


        private int? selectedRowIndex = null;
        private void tableDonVi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < tableDonVi.Rows.Count)
            {

                if (selectedRowIndex == e.RowIndex)
                {
                    ResetFormDV();
                    return;
                }

                selectedRowIndex = e.RowIndex;

                DataGridViewRow row = tableDonVi.Rows[e.RowIndex];
                donViDTO dv = row.DataBoundItem as donViDTO;

                txtDonVi.Text = dv.TenDonVi;

                btnThemDonVI.Enabled = false;
                btnSuaDonVi.Enabled = true;
                btnXoaDonVi.Enabled = true;
            }
        }

        private void btnSuaDonVi_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tableDonVi.SelectedRows[0];
            donViDTO dv = row.DataBoundItem as donViDTO;
            int maDonVi = dv.MaDonVi;
            string tenMoi = txtDonVi.Text.Trim();
            if (busDonVi.kiemTraChuoiRong(tenMoi))
            {
                MessageBox.Show("Vui lòng nhập tên đơn vị mới", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonVi.Focus();
                return;
            }
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn sửa đơn vị này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                donViDTO ct = new donViDTO();
                ct.MaDonVi = maDonVi;
                ct.TenDonVi = tenMoi;

                if (ct != null)
                {
                    MessageBox.Show("Sửa đơn vị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busDonVi.suaDonVi(ct);
                    ResetFormDV();
                }
                else
                {
                    MessageBox.Show("Sửa đơn vị thất bại!", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void btnXoaDonVi_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tableDonVi.SelectedRows[0];
            donViDTO ct = row.DataBoundItem as donViDTO;
            int maDonVi = ct.MaDonVi;
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa đơn vị này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                if (maDonVi != -1)
                {
                    MessageBox.Show("Xóa đơn vị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busDonVi.Xoa(maDonVi);
                    ResetFormDV();
                }
                else
                {
                    MessageBox.Show("Lỗi kh xóa đc đơn vị!", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void ResetHeSoForm()
        {
            txtTenNL.Clear();
            txtMaDV.Clear();
            txtHeSo.Value = 0.001M;
            tableHeSo.ClearSelection();
            btnThemHs.Enabled = true;
            btnSuaHS.Enabled = false;
            btnXoaHeSO.Enabled = false;
            btnSanPham.Enabled = true;
            btnChonDV.Enabled = true;
            selectedRowIndexHS = null;
        }

        private int? selectedRowIndexHS = null;
        private void tableHeSo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < tableHeSo.Rows.Count)
            {
                if (selectedRowIndexHS == e.RowIndex)
                {
                    ResetHeSoForm();
                    return;
                }

                selectedRowIndexHS = e.RowIndex;

                heSoDTO hs = tableHeSo.Rows[e.RowIndex].DataBoundItem as heSoDTO;
                if (hs == null) return;

                nguyenLieuDTO nl = dsNguyenLieu.FirstOrDefault(x => x.MaNguyenLieu == hs.MaNguyenLieu);
                donViDTO dv = dsDonVi.FirstOrDefault(x => x.MaDonVi == hs.MaDonVi);

                txtTenNL.Text = nl?.TenNguyenLieu ?? "Không xác định";

                txtMaDV.Text = dv?.TenDonVi ?? "Không xác định";

                txtHeSo.Value = (decimal)hs.HeSo;

                maNL = hs.MaNguyenLieu;
                maDV = hs.MaDonVi;

                btnThemHs.Enabled = false;
                btnSanPham.Enabled = false;
                btnChonDV.Enabled = false;
                btnSuaHS.Enabled = true;
                btnXoaHeSO.Enabled = true;
            }
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            using (selectNguyenLieu form = new selectNguyenLieu())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    maNL = form.MaNL;
                    txtTenNL.Text = form.TenNL;
                }
            }
        }

        private void btnThemHs_Click(object sender, EventArgs e)
        {
            if (busDonVi.kiemTraChuoiRong(txtTenNL.Text))
            {
                MessageBox.Show("Vui lòng nhập nguyên liệu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNL.Focus();
                return;
            }
            if (busDonVi.kiemTraChuoiRong(txtMaDV.Text))
            {
                MessageBox.Show("Vui lòng nhập đơn vị", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaDV.Focus();
                return;
            }
            if (txtHeSo.Value == 0)
            {
                MessageBox.Show("Vui lòng nhập hế số", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHeSo.Focus();
                return;
            }

            if(busHeSo.kiemTraTrungHeSo(maNL,maDV))
            {
                MessageBox.Show("Hệ số bạn vừa nhập đã tồn tại rồi", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal heso = (decimal)txtHeSo.Value;
            heSoBUS bus = new heSoBUS();
            heSoDTO ct = new heSoDTO();
            ct.MaNguyenLieu = maNL;
            ct.MaDonVi = maDV;
            ct.HeSo = heso;
            
            if(ct != null)
            {
                MessageBox.Show("Thêm hệ số thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                busHeSo.Them(ct);

                dsNguyenLieu = new nguyenLieuBUS().LayDanhSach();
                dsDonVi = new donViBUS().LayDanhSach();
                ResetHeSoForm();
            }else
            {
                MessageBox.Show("Lỗi kh thêm đc hs!", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnSuaHS_Click(object sender, EventArgs e)
        {
            if (busDonVi.kiemTraChuoiRong(txtTenNL.Text))
            {
                MessageBox.Show("Vui lòng nhập nguyên liệu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (busDonVi.kiemTraChuoiRong(txtMaDV.Text))
            {
                MessageBox.Show("Vui lòng nhập đơn vị", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtHeSo.Value == 0)
            {
                MessageBox.Show("Vui lòng nhập hế số", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal heso = (decimal)txtHeSo.Value;

            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn sửa hệ số này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                heSoDTO ct = new heSoDTO();
                ct.MaNguyenLieu = maNL;
                ct.MaDonVi = maDV;
                ct.HeSo = heso;

                if(ct != null)
                {
                    MessageBox.Show("Sửa hệ số thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busHeSo.Sua(ct);
                    ResetHeSoForm();
                }else
                {
                    MessageBox.Show("Lỗi kh sửa đc hệ số", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void btnXoaHeSO_Click(object sender, EventArgs e)
        {
            if (maNL == -1 || maDV == -1)
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa hệ số này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                if(maNL != -1 && maDV != -1)
                {
                    MessageBox.Show("Xóa hệ số thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busHeSo.Xoa(maNL, maDV);
                    ResetHeSoForm();
                }else
                {
                    MessageBox.Show("Lỗi kh xóa đc hs", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void tableHeSo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tableHeSo.ClearSelection();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            BindingList<donViDTO> dsDV = new donViBUS().LayDanhSach();
            if (cboTimDV.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboTimDV.Focus();
                return;
            }
            if (busDonVi.kiemTraChuoiRong(txtTimDV.Text))
            {
                MessageBox.Show("Vui lòng nhập giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimDV.Focus();
                return;
            }
            string tim = txtTimDV.Text.Trim().ToLower();
            string giaTriChon = cboTimDV.SelectedItem.ToString();
            List<donViDTO> dsTimKiem = new List<donViDTO>();
            if (giaTriChon == "Mã ĐV")
            {
                dsTimKiem = (from item in dsDV
                             where item.MaDonVi.ToString().Contains(tim)
                             orderby item.MaDonVi
                             select item).ToList();
            }
            if (giaTriChon == "Tên ĐV")
            {
                dsTimKiem = (from item in dsDV
                             where item.TenDonVi.ToLower().Contains(tim)
                             orderby item.MaDonVi
                             select item).ToList();
            }
            if (dsTimKiem != null && dsTimKiem.Count > 0)
            {
                BindingList<donViDTO> kqSauBinding = new BindingList<donViDTO>(dsTimKiem);
                loadDanhSachDonVi(kqSauBinding);
                loadFontChuVaSizeTableDonVi();
            }
            else
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnRs_Click(object sender, EventArgs e)
        {
            txtMaDV.Clear();
            txtTimDV.Clear();
            cboTimDV.SelectedIndex = -1;
            BindingList<donViDTO> dskq = donViBUS.ds;
            loadDanhSachDonVi(dskq);
            loadFontChuVaSizeTableDonVi();
        }

        private void btnTKhs_Click(object sender, EventArgs e)
        {
            if (cboTimHS.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboTimHS.Focus();
                return;
            }
            if (busDonVi.kiemTraChuoiRong(txtTimHS.Text))
            {
                MessageBox.Show("Vui lòng chọn giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimHS.Focus();
                return;
            }
            string tim = txtTimHS.Text.ToLower().Trim();
            string giaChiChon = cboTimHS.SelectedItem.ToString();
            List<heSoDTO> dsTim = new List<heSoDTO>();
            BindingList<heSoDTO> dsHS = new heSoBUS().LayDanhSach();
            BindingList<nguyenLieuDTO> dsNL = new nguyenLieuBUS().LayDanhSach();
            BindingList<donViDTO> dsDV = new donViBUS().LayDanhSach();

            if (giaChiChon == "Mã NL")
            {
                dsTim = (from item in dsHS
                         where item.MaNguyenLieu.ToString().Contains(tim)
                         orderby item.MaNguyenLieu
                         select item).ToList();
            }

            if (giaChiChon == "Tên NL")
            {
                dsTim = (from heso in dsHS
                         join nl in dsNL on heso.MaNguyenLieu equals nl.MaNguyenLieu
                         where nl.TenNguyenLieu.ToLower().Contains(tim)
                         orderby nl.MaNguyenLieu
                         select heso).ToList();
            }

            if(giaChiChon == "Mã ĐV")
            {
                dsTim = (from heso in dsHS
                         where heso.MaDonVi.ToString().Contains(tim)
                         orderby heso.MaDonVi
                         select heso).ToList();
;           }

            if(giaChiChon == "Tên ĐV")
            {
                dsTim = (from heso in dsHS
                         join dv in dsDV on heso.MaDonVi equals dv.MaDonVi
                         where dv.TenDonVi.ToLower().Contains(tim)
                         orderby dv.MaDonVi
                         select heso).ToList();
            }

            if(giaChiChon == "Hệ số Min")
            {
                decimal heSoTim;
                bool isNumber = decimal.TryParse(txtTimHS.Text.Trim(), out heSoTim);

                if (isNumber)
                {
                    dsTim = (from heso in dsHS
                             where heso.HeSo <= heSoTim
                             orderby heso.HeSo
                             select heso).ToList();
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập hệ số là một con số hợp lệ", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTimHS.Focus();
                    return;
                }
            }

            if (giaChiChon == "Hệ số Max")
            {
                decimal heSoTim;
                bool isNumber = decimal.TryParse(txtTimHS.Text.Trim(), out heSoTim);

                if (isNumber)
                {
                    dsTim = (from heso in dsHS
                             where heso.HeSo >= heSoTim
                             orderby heso.HeSo
                             select heso).ToList();
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập hệ số là một con số hợp lệ", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTimHS.Focus();
                    return;
                }
            }

            if(dsTim != null && dsTim.Count > 0)
            {
                BindingList<heSoDTO> dsBinding = new BindingList<heSoDTO>(dsTim);
                loadDanhSachHeSo(dsBinding);
                loadFontChuVaSizeTableHeSo();
            }else
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnChonDV_Click(object sender, EventArgs e)
        {
            using(selectDonVi form = new selectDonVi())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if(form.ShowDialog() == DialogResult.OK)
                {
                    maDV = form.maDonVi;
                    txtMaDV.Text = form.tenDonVi;
                }
            }
        }

        private void btnRsDV_Click(object sender, EventArgs e)
        {
            txtTenNL.Clear();
            maNL = -1;
            txtMaDV.Clear();
            maDV = -1;
            txtHeSo.Value = 0.001M;
            cboTimHS.SelectedIndex = -1;
            txtTimHS.Clear();
            BindingList<heSoDTO> ds = new heSoBUS().LayDanhSach();
            loadDanhSachHeSo(ds);
            loadFontChuVaSizeTableHeSo();
        }
    }
}
