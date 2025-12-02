using BUS;
using DTO;
using FONTS;
using GUI.GUI_CRUD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GUI.GUI_UC
{
    public partial class congThucGUI : UserControl
    {

        private int lastSelectedRowCongThuc = -1;
        private congThucBUS busCongThuc = new congThucBUS();
        private BindingList<nguyenLieuDTO> dsNguyenLieu;
        private BindingList<donViDTO> dsDonVi;
        private BindingList<sanPhamDTO> dsSanPham;

        public congThucGUI()
        {
            InitializeComponent();
        }

        private void congThucGUI_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            dsNguyenLieu = new nguyenLieuBUS().LayDanhSach();
            dsDonVi = new donViBUS().LayDanhSach();
            dsSanPham = new sanPhamBUS().LayDanhSach();

            BindingList<congThucDTO> ds = busCongThuc.LayDanhSach();
            loadDanhSachCongThuc(ds);
            rdoTimCoBan.Checked = true;
            hienThiPlaceHolderCongThuc();
        }

        private void loadDanhSachCongThuc(BindingList<congThucDTO> ds)
        {
            tableCongThuc.AutoGenerateColumns = false;
            tableCongThuc.Columns.Clear();

            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSanPham", HeaderText = "Mã SP" });
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSanPham", HeaderText = "Tên SP" });
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Mã NL" });
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Tên NL" });
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuongCoSo", HeaderText = "Số lượng" });
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonViCoSo", HeaderText = "Đơn vị" });

            tableCongThuc.DataSource = ds;

            btnSuaCT.Enabled = false;
            btnXoaCT.Enabled = false;
            btnChiTietCT.Enabled = false;
            tableCongThuc.ReadOnly = true;
            tableCongThuc.ClearSelection();

            loadFontChuVaSize();
        }

        private void tableCongThuc_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            congThucDTO ct = tableCongThuc.Rows[e.RowIndex].DataBoundItem as congThucDTO;

            if (tableCongThuc.Columns[e.ColumnIndex].HeaderText == "Tên SP")
            {
                sanPhamDTO sp = dsSanPham.FirstOrDefault(x => x.MaSP == ct.MaSanPham);
                e.Value = sp?.TenSP ?? "Không xác định";
            }

            if (tableCongThuc.Columns[e.ColumnIndex].HeaderText == "Tên NL")
            {
                nguyenLieuDTO nl = dsNguyenLieu.FirstOrDefault(x => x.MaNguyenLieu == ct.MaNguyenLieu);
                e.Value = nl?.TenNguyenLieu ?? "Không xác định";
            }

            if (tableCongThuc.Columns[e.ColumnIndex].HeaderText == "Đơn vị")
            {
                donViDTO dv = dsDonVi.FirstOrDefault(x => x.MaDonVi == ct.MaDonViCoSo);
                e.Value = dv?.TenDonVi ?? "Không xác định";
            }
        }

        private void loadFontChuVaSize()
        {
            // --- Căn giữa và tắt sort ---
            foreach (DataGridViewColumn col in tableCongThuc.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableCongThuc.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableCongThuc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // font cho dữ liệu trong table
            tableCongThuc.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            //font cho header trong table
            tableCongThuc.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(12);

            // --- Fix lỗi mất text khi đổi font ---
            tableCongThuc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableCongThuc.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableCongThuc.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableCongThuc.Refresh();
        }

        private void kiemTraClickTable(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.RowIndex == lastSelectedRowCongThuc)
            {
                tableCongThuc.ClearSelection();
                lastSelectedRowCongThuc = -1;
                btnThemCT.Enabled = true;
                btnSuaCT.Enabled = false;
                btnXoaCT.Enabled = false;
                btnChiTietCT.Enabled = false;
                return;
            }

            tableCongThuc.ClearSelection();
            tableCongThuc.Rows[e.RowIndex].Selected = true;
            lastSelectedRowCongThuc = e.RowIndex;

            btnSuaCT.Enabled = true;
            btnXoaCT.Enabled = true;
            btnChiTietCT.Enabled = true;
        }

        private void btnThemCT_Click(object sender, EventArgs e)
        {
            using (insertCongThuc form = new insertCongThuc())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if(form.ShowDialog() == DialogResult.OK)
                {
                    BindingList<congThucDTO> dsKQ = form.dsTam;
                    foreach (congThucDTO ct in dsKQ)
                    {
                        busCongThuc.themCongThuc(ct);
                    }
                }
            }
        }

        private void btnSuaCT_Click(object sender, EventArgs e)
        {
            if (tableCongThuc.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableCongThuc.SelectedRows[0];
                congThucDTO ct = row.DataBoundItem as congThucDTO;

                using (updateCongThuc form = new updateCongThuc(ct))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if(form.ShowDialog() == DialogResult.OK)
                    {
                        congThucDTO ctSuaNe = form.ctSUA;
                        busCongThuc.suaCongThuc(ctSuaNe);
                        tableCongThuc.Refresh();
                    }
                }
            }
        }

        private void btnXoaCT_Click(object sender, EventArgs e)
        {
            if (tableCongThuc.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableCongThuc.SelectedRows[0];
                congThucDTO ct = row.DataBoundItem as congThucDTO;
                int maSP = ct.MaSanPham;
                int maNL = ct.MaNguyenLieu;

                using (deleteCongThuc form = new deleteCongThuc())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if(form.ShowDialog() == DialogResult.OK)
                    {
                        busCongThuc.xoaCongThuc(maSP, maNL);
                        btnSuaCT.Enabled = false;
                        btnXoaCT.Enabled = false;
                        btnChiTietCT.Enabled = false;
                    }
                }
            }
        }

        private void btnChiTietCT_Click(object sender, EventArgs e)
        {
            if (tableCongThuc.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableCongThuc.SelectedRows[0];
                congThucDTO ct = row.DataBoundItem as congThucDTO;

                using (detailCongThuc form = new detailCongThuc(ct))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
            }
        }

        private void btnReFreshCT_Click(object sender, EventArgs e)
        {
            congThucBUS bus = new congThucBUS();
            BindingList<congThucDTO> ds = bus.LayDanhSach();
            loadDanhSachCongThuc(ds);

            ResetInputTimKiem();
        }

        private void btnExcelCT_Click(object sender, EventArgs e)
        {
            using (selectExcelCongThuc form = new selectExcelCongThuc())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
            congThucBUS bus = new congThucBUS();
            BindingList<congThucDTO> ds = bus.LayDanhSach();
            loadDanhSachCongThuc(ds);
        }

        // dòng này là để cho khi mà mình load trang nó kh chọn dòng đầu nha
        private void tableCongThuc_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tableCongThuc.ClearSelection();
        }

        private void rdoTimCoBan_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoTimCoBan.Checked == true)
            {
                rdoTimNangCao.Checked = false;
                txtTenSP.Enabled = false;
                txtTenNL.Enabled = false;
                txtTenDV.Enabled = false;
                txtSoLuongMin.Enabled = false;
                txtSoLuongMax.Enabled = false;

                txtTenDV.Clear();
                txtTenNL.Clear();
                txtTenSP.Clear();
                txtSoLuongMin.Clear();
                txtSoLuongMax.Clear();
                hienThiPlaceHolderCongThuc();
            }
            else
            {
                txtTimKiemCT.Enabled = false;
                cboTimKiemCT.Enabled = false;
                txtTenSP.Enabled = true;
                txtTenNL.Enabled = true;
                txtTenDV.Enabled = true;
                txtSoLuongMin.Enabled = true;
                txtSoLuongMax.Enabled = true;
            }
        }

        private void rdoTimNangCao_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoTimNangCao.Checked == true)
            {
                rdoTimCoBan.Checked = false;
                cboTimKiemCT.Enabled = false;
                txtTimKiemCT.Enabled = false;

                cboTimKiemCT.SelectedItem = -1;
                txtTimKiemCT.Clear();
                hienThiPlaceHolderCongThuc();
            }else
            {
                cboTimKiemCT.Enabled = true;
                txtTimKiemCT.Enabled = true;
            }
        }

        private void hienThiPlaceHolderCongThuc()
        {
            SetPlaceholder(txtTimKiemCT, "Nhập giá trị cần tìm");
            SetPlaceholder(txtTenNL, "Tên NL");
            SetPlaceholder(txtSoLuongMin, "SL min");
            SetPlaceholder(txtSoLuongMax, "SL max");
            SetPlaceholder(txtTenSP, "Tên SP");
            SetPlaceholder(txtTenDV, "Tên ĐV");
            SetComboBoxPlaceholder(cboTimKiemCT, "Chọn giá trị TK");
        }
        // set placehover cho tìm kiếm nâng cao sản phẩm
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


        // set placehover cho tìm kiếm nâng cao sản phẩm
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

        private void btnThucHienTimKiem_Click(object sender, EventArgs e)
        {
            if (rdoTimCoBan.Checked == true)
            {
                timKiemCoBan();
            }else
            {
                timKiemNangCao();
            }
        }

        public void loadLaiDanhSachTimKiem()
        {
            BindingList<congThucDTO> ds = new congThucBUS().LayDanhSach();
            loadDanhSachCongThuc(ds);
            loadFontChuVaSize();
            ResetInputTimKiem();
        }

        public void ResetInputTimKiem()
        {
            cboTimKiemCT.SelectedIndex = -1;
            txtTimKiemCT.Clear();
            txtTenDV.Clear();
            txtTenNL.Clear();
            txtTenSP.Clear();
            txtSoLuongMin.Clear();
            txtSoLuongMax.Clear();

            hienThiPlaceHolderCongThuc();
        }
        public void timKiemCoBan()
        {
            string tim = txtTimKiemCT.Text.Trim();
            int index = cboTimKiemCT.SelectedIndex;

            if(index == -1 || string.IsNullOrWhiteSpace(tim))
            {
                MessageBox.Show("Vui lòng nhập giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BindingList<congThucDTO> dskq = busCongThuc.timKiemCoBan(tim, index);
            if (dskq != null && dskq.Count > 0)
            {
                loadDanhSachCongThuc(dskq);
                loadFontChuVaSize();
            }else
            {
                MessageBox.Show("Không tìm thấy kết quả tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadLaiDanhSachTimKiem();
                return;
            }
        }
        public void timKiemNangCao()
        {
            string tenSP = string.IsNullOrWhiteSpace(txtTenSP.Text) ? null : txtTenSP.Text.Trim();
            string tenNL = string.IsNullOrWhiteSpace(txtTenNL.Text) ? null : txtTenNL.Text.Trim();
            string tenDV = string.IsNullOrWhiteSpace(txtTenDV.Text) ? null : txtTenDV.Text.Trim();

            if(tenSP == "Tên SP")
            {
                tenSP = null;
            }
            if(tenNL == "Tên NL")
            {
                tenNL = null;
            }
            if(tenDV == "Tên ĐV")
            {
                tenDV = null;
            }

            float slMin = -1;
            string strMin = txtSoLuongMin.Text.Trim();
            if (!string.IsNullOrWhiteSpace(strMin) && strMin != "SL min")
            {
                float.TryParse(strMin, out slMin);
            }

            float slMax = -1;
            string strMax = txtSoLuongMax.Text.Trim();
            if (!string.IsNullOrWhiteSpace(strMax) && strMax != "SL max")
            {
                float.TryParse(strMax, out slMax);
            }

            if(slMin != -1 && slMax != -1 && slMin > slMax)
            {
                MessageBox.Show("Số lượng tối thiểu không được lớn hơn tồn tối đa", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(tenSP == null && tenNL == null && tenDV == null && slMin == -1 && slMax == -1)
            {
                MessageBox.Show("Vui lòng nhập ít nhất một điều kiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BindingList<congThucDTO> dsTimKiemNangCao = busCongThuc.timKiemNangCao(tenSP, tenNL, tenDV, slMin, slMax);
            if(dsTimKiemNangCao != null && dsTimKiemNangCao.Count > 0)
            {
                loadDanhSachCongThuc(dsTimKiemNangCao);
                loadFontChuVaSize();
            }else
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
    }
}
