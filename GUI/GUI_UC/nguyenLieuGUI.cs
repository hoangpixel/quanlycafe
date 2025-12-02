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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.GUI_UC
{
    public partial class nguyenLieuGUI : UserControl
    {
        private int lastSelectedRowNguyenLieu = -1;
        private BindingList<donViDTO> dsDonVi;
        private nguyenLieuBUS busNguyenLieu = new nguyenLieuBUS();
        public nguyenLieuGUI()
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
            hienThiPlaceHolderNguyenLieu();
            rdCoBanNL.Checked = true;
        }

        private void nguyenLieuGUI_Load(object sender, EventArgs e)
        {
            donViBUS busDV = new donViBUS();
            dsDonVi = busDV.LayDanhSach();


            BindingList<nguyenLieuDTO> ds = busNguyenLieu.LayDanhSach();
            loadDanhSachNguyenLieu(ds);
            loadFontChuVaSize();
        }

        private void hienThiPlaceHolderNguyenLieu()
        {
            SetupComboBoxData(cbTrangThaiNL);

            SetPlaceholder(txtTimKiemNL, "Nhập giá trị cần tìm");
            SetPlaceholder(txtTenNLTK, "Tên NL");
            SetPlaceholder(txtMinNL, "Tồn min");
            SetPlaceholder(txtMaxNL, "Tồn max");
            SetPlaceholder(txtTenDonViTK, "Tên ĐV");
            SetComboBoxPlaceholder(cboTimKiemNL, "Chọn giá trị TK");
            SetComboBoxPlaceholder(cbTrangThaiNL, "Chọn trạng thái");
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

        private void SetupComboBoxData(ComboBox cbo)
        {
            var items = new List<dynamic>
    {
        new { Text = "Có", Value = 1 },
        new { Text = "Không", Value = 0 }
    };

            cbo.DisplayMember = "Text";
            cbo.ValueMember = "Value";
            cbo.DataSource = items;

            cbo.SelectedIndex = -1;
        }
        public void loadDanhSachNguyenLieu(BindingList<nguyenLieuDTO> ds)
        {
            tableNguyenLieu.AutoGenerateColumns = false;
            tableNguyenLieu.DataSource = ds;

            tableNguyenLieu.Columns.Clear();

            tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn{DataPropertyName = "MaNguyenLieu", HeaderText="Mã NL"});
            tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenNguyenLieu", HeaderText = "Tên NL" });
            tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonViCoSo", HeaderText = "Đơn vị" });
            tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TrangThaiDV", HeaderText = "Trạng thái HS" });
            tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TonKho", HeaderText = "Tồn kho" });

            btnSuaNL.Enabled = false;
            btnXoaNL.Enabled = false;
            btnChiTietNL.Enabled = false;
            tableNguyenLieu.ReadOnly = true;
            tableNguyenLieu.ClearSelection();
        }
        private void tableNguyenLieu_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(e.RowIndex < 0)
            {
                return;
            }
            nguyenLieuDTO ct = tableNguyenLieu.Rows[e.RowIndex].DataBoundItem as nguyenLieuDTO;
            if(ct == null)
            {
                return;
            }
            if (tableNguyenLieu.Columns[e.ColumnIndex].HeaderText == "Đơn vị")
            {
                donViDTO donvi = dsDonVi.FirstOrDefault(x => x.MaDonVi == ct.MaDonViCoSo);
                e.Value = donvi?.TenDonVi ?? "Không xác định";
            }
            if (tableNguyenLieu.Columns[e.ColumnIndex].HeaderText == "Trạng thái HS")
            {
                e.Value = ct.TrangThaiDV == 1 ? "Đã có hệ số" : "Chưa có hệ số";
            }    
        }
        private void loadFontChuVaSize()
        {
            // --- Căn giữa và tắt sort ---
            foreach (DataGridViewColumn col in tableNguyenLieu.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableNguyenLieu.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableNguyenLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // font cho dữ liệu trong table
            tableNguyenLieu.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            //font cho header trong table
            tableNguyenLieu.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(12);

            // --- Fix lỗi mất text khi đổi font ---
            tableNguyenLieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableNguyenLieu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableNguyenLieu.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableNguyenLieu.Refresh();
        }

        private void kiemTraClickTable(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.RowIndex == lastSelectedRowNguyenLieu)
            {
                tableNguyenLieu.ClearSelection();
                lastSelectedRowNguyenLieu = -1;

                btnThemNL.Enabled = true;
                btnSuaNL.Enabled = false;
                btnXoaNL.Enabled = false;
                btnChiTietNL.Enabled = false;
                return;
            }
            tableNguyenLieu.ClearSelection();
            tableNguyenLieu.Rows[e.RowIndex].Selected = true;
            lastSelectedRowNguyenLieu = e.RowIndex;

            btnSuaNL.Enabled = true;
            btnXoaNL.Enabled = true;
            btnChiTietNL.Enabled = true;
        }

        private void btnThemNL_Click(object sender, EventArgs e)
        {
            using (insertNguyenLieu form = new insertNguyenLieu())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if(form.ShowDialog() == DialogResult.OK)
                {
                    nguyenLieuDTO ct = form.ct;
                    busNguyenLieu.themNguyenLieu(ct);
                }
            }
        }

        private void btnSuaNL_Click(object sender, EventArgs e)
        {
            if (tableNguyenLieu.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableNguyenLieu.SelectedRows[0];
                nguyenLieuDTO nl = row.DataBoundItem as nguyenLieuDTO;

                using (updateNguyenLieu form = new updateNguyenLieu(nl))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if(form.ShowDialog() == DialogResult.OK)
                    {
                        nguyenLieuDTO ct = form.suaCT;
                        busNguyenLieu.suaNguyenLieu(ct);
                        tableNguyenLieu.Refresh();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnXoaNL_Click(object sender, EventArgs e)
        {
            if (tableNguyenLieu.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableNguyenLieu.SelectedRows[0];
                nguyenLieuDTO ct = row.DataBoundItem as nguyenLieuDTO;
                int maXoa = ct.MaNguyenLieu;
                using(deleteNguyenLieu form = new deleteNguyenLieu())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if(form.ShowDialog() == DialogResult.OK)
                    {
                        busNguyenLieu.xoaNguyenLieu(maXoa);
                        btnSuaNL.Enabled = false;
                        btnXoaNL.Enabled = false;
                        btnChiTietNL.Enabled = false;
                    }
                }
            }
        }

        private void btnChiTietNL_Click(object sender, EventArgs e)
        {
            if (tableNguyenLieu.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableNguyenLieu.SelectedRows[0];
                nguyenLieuDTO ct = row.DataBoundItem as nguyenLieuDTO;

                using (detailNguyenLieu form = new detailNguyenLieu(ct))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
            }
        }

        private void btnRefreshNL_Click(object sender, EventArgs e)
        {
            BindingList<nguyenLieuDTO> ds = busNguyenLieu.LayDanhSach();
            loadDanhSachNguyenLieu(ds);
            loadFontChuVaSize();
            cboTimKiemNL.SelectedIndex = -1;
            cbTrangThaiNL.SelectedIndex = -1;
            txtMinNL.Clear();
            txtMaxNL.Clear();
            txtTenNLTK.Clear();
            txtTimKiemNL.Clear();
            hienThiPlaceHolderNguyenLieu();
        }

        private void btnDonVi_Click(object sender, EventArgs e)
        {
            using (donVivaHeSoGUI form = new donVivaHeSoGUI())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
            dsDonVi = new donViBUS().LayDanhSach();
        }

        private void btnExcelNL_Click(object sender, EventArgs e)
        {
            using (selectExcelNguyenLieu form = new selectExcelNguyenLieu())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
            busNguyenLieu.LayDanhSach();
            loadDanhSachNguyenLieu(nguyenLieuBUS.ds);
        }

        private void timKiemCoBanNL()
        {
            string tim = txtTimKiemNL.Text.Trim();
            int index = cboTimKiemNL.SelectedIndex;

            if (index == -1 || string.IsNullOrWhiteSpace(tim))
            {
                MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BindingList<nguyenLieuDTO> ds = busNguyenLieu.timKiemCoBanNL(tim, index);
            if (ds != null && ds.Count > 0)
            {
                tableNguyenLieu.Columns.Clear();
                tableNguyenLieu.DataSource = null;
                loadDanhSachNguyenLieu(ds);
                loadFontChuVaSize();
            }
            else
            {
                MessageBox.Show("Không có kết quả tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                busNguyenLieu.LayDanhSach();
                loadDanhSachNguyenLieu(nguyenLieuBUS.ds);

                txtTimKiemNL.Clear();
                cboTimKiemNL.SelectedIndex = -1;
                SetPlaceholder(txtTimKiemNL, "Nhập giá trị cần tìm");
                SetComboBoxPlaceholder(cboTimKiemNL, "Chọn giá trị TK");
            }

        }

        private void timKiemNangCaoNL()
        {
            // 1. Xử lý chuỗi (Loại bỏ placeholder nếu người dùng không nhập)
            string tenNL = txtTenNLTK.Text.Trim();
            if (tenNL == "Tên NL") tenNL = null; // Kiểm tra đúng text placeholder

            string tenDV = txtTenDonViTK.Text.Trim();
            if (tenDV == "Tên ĐV") tenDV = null;

            // 2. Xử lý ComboBox (Dùng SelectedValue thay vì SelectedIndex)
            int trangThaiDV = -1;
            if (cbTrangThaiNL.SelectedIndex != -1 && cbTrangThaiNL.SelectedValue != null)
            {
                if (int.TryParse(cbTrangThaiNL.SelectedValue.ToString(), out int val))
                {
                    trangThaiDV = val;
                }
            }

            // 3. Xử lý số (Min/Max)
            float tonMin = -1;
            string strMin = txtMinNL.Text.Trim();
            // Phải chắc chắn text không phải là Placeholder mới parse
            if (!string.IsNullOrWhiteSpace(strMin) && strMin != "Tồn min")
            {
                float.TryParse(strMin, out tonMin);
            }

            float tonMax = -1;
            string strMax = txtMaxNL.Text.Trim();
            if (!string.IsNullOrWhiteSpace(strMax) && strMax != "Tồn max")
            {
                float.TryParse(strMax, out tonMax);
            }

            // 4. Validate Min > Max
            if (tonMin != -1 && tonMax != -1 && tonMin > tonMax)
            {
                MessageBox.Show("Tồn tối thiểu không được lớn hơn tồn tối đa", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 5. Validate không nhập gì cả
            if (string.IsNullOrEmpty(tenNL) && string.IsNullOrEmpty(tenDV) &&
                tonMin == -1 && tonMax == -1 && trangThaiDV == -1)
            {
                MessageBox.Show("Vui lòng nhập ít nhất một điều kiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 6. Gọi BUS
            BindingList<nguyenLieuDTO> dskq = busNguyenLieu.timKiemNangCao(trangThaiDV, tenNL, tenDV, tonMin, tonMax);

            if (dskq != null && dskq.Count > 0)
            {
                loadDanhSachNguyenLieu(dskq);
                loadFontChuVaSize();
            }
            else
            {
                // Nên clear DataGridView hoặc thông báo
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // loadDanhSachNguyenLieu(new BindingList<nguyenLieuDTO>()); // Dòng này tùy chọn để xóa lưới
            }
        }

        // dòng này là để cho khi mà mình load trang nó kh chọn dòng đầu nha
        private void tableNguyenLieu_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tableNguyenLieu.ClearSelection();
        }

        private void rdCoBanNL_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rdCoBanNL.Checked)
            {
                rdNcNL.Checked = false;
                txtMinNL.Enabled = false;
                txtMaxNL.Enabled = false;
                cbTrangThaiNL.Enabled = false;
                txtTenDonViTK.Enabled = false;
                txtTenNLTK.Enabled = false;
            }
            else
            {
                txtMinNL.Enabled = true;
                txtMaxNL.Enabled = true;
                cbTrangThaiNL.Enabled = true;
                txtTenDonViTK.Enabled = true;
                txtTenNLTK.Enabled = true;
            }
        }

        private void btnTimKiemNL_Click(object sender, EventArgs e)
        {
            if (rdCoBanNL.Checked)
            {
                timKiemCoBanNL();
            }
            else
            {
                timKiemNangCaoNL();
            }
        }

        private void rdNcNL_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rdNcNL.Checked)
            {
                rdCoBanNL.Checked = false;
                cboTimKiemNL.Enabled = false;
                txtTimKiemNL.Enabled = false;
            }
            else
            {
                cboTimKiemNL.Enabled = true;
                txtTimKiemNL.Enabled = true;
            }
        }
    }
}
