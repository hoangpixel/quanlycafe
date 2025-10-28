using BUS;
using DTO;
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
using GUI.GUI_SELECT;
using GUI.FONTS;
namespace GUI.GUI_UC
{
    public partial class sanPhamGUI : UserControl
    {
        private int lastSelectedRowSanPham = -1;
        public sanPhamGUI()
        {
            InitializeComponent();
        }

        private void sanPhamGUI_Load(object sender, EventArgs e)
        {

            FontManager.LoadFont();
            hienThiPlaceHolderSanPham();
            FontManager.ApplyFontToAllControls(this);

            sanPhamBUS bus = new sanPhamBUS();
            bus.docDSSanPham();
            loadDanhSachSanPham(sanPhamBUS.ds);
            tbSanPham.ClearSelection();
            loadComboBoxLoaiSPTK();
            rdoTimCoBan.Checked = true;
        }


        private void loadFontChuVaSize()
        {
            // --- Căn giữa và tắt sort ---
            foreach (DataGridViewColumn col in tbSanPham.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tbSanPham.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tbSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // font cho dữ liệu trong table
            tbSanPham.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            //font cho header trong table
            tbSanPham.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(12);

            // --- Fix lỗi mất text khi đổi font ---
            tbSanPham.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbSanPham.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbSanPham.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tbSanPham.Refresh();
        }


        private void loadDanhSachSanPham(List<sanPhamDTO> ds)
            {
                tbSanPham.Columns.Clear();
                tbSanPham.DataSource = null;

                DataTable dt = new DataTable();
                dt.Columns.Add("Mã SP");
                dt.Columns.Add("Tên SP");
                dt.Columns.Add("Tên Loại");
                dt.Columns.Add("Tên nhóm");
                //dt.Columns.Add("Trạng thái SP");
                dt.Columns.Add("Trạng thái CT");
                dt.Columns.Add("Hình");
                dt.Columns.Add("Giá");

                loaiSanPhamBUS loaiBus = new loaiSanPhamBUS();
                List<loaiDTO> dsLoai = loaiBus.layDanhSachLoai();

                nhomBUS nhomBus = new nhomBUS();
                List<nhomDTO> dsNhom = nhomBus.layDanhSach();

                foreach (var sp in ds)
                {
                    string tenLoai = dsLoai.FirstOrDefault(l => l.MaLoai == sp.MaLoai)?.TenLoai ?? "Không xác định";
                    //string trangThai = sp.TrangThai == 1 ? "Đang bán" : "Ngừng bán";
                    loaiDTO loai = dsLoai.FirstOrDefault(l => l.MaLoai == sp.MaLoai);
                    string tenNhom = dsNhom.FirstOrDefault(n => n.MaNhom == (loai != null ? loai.MaNhom : -1))?.TenNhom ?? "Không xác định";

                    string trangThaiCT = sp.TrangThaiCT == 1 ? "Đã có công thức" : "Chưa có công thức";
                    dt.Rows.Add(sp.MaSP, sp.TenSP, tenLoai, tenNhom, trangThaiCT, sp.Hinh, string.Format("{0:N0}", sp.Gia));
                }

                tbSanPham.DataSource = dt;

            loadFontChuVaSize();

            tbSanPham.ReadOnly = true;

                //rdoTimCoBan.Checked = true;

                tbSanPham.ClearSelection();
                btnSuaSP.Enabled = false;
                btnXoaSP.Enabled = false;
                btnChiTiet.Enabled = false;
            }

        private void loadComboBoxLoaiSPTK()
        {
            loaiSanPhamBUS bus = new loaiSanPhamBUS();
            List<loaiDTO> dsLoai = bus.layDanhSachLoai();

            cboLoaiSP.DataSource = dsLoai;
            cboLoaiSP.DisplayMember = "TenLoai";
            cboLoaiSP.ValueMember = "MaLoai";
            cboLoaiSP.SelectedIndex = -1;
            SetComboBoxPlaceholder(cboLoaiSP, "Loại SP");

            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Tất cả");
            cboTrangThai.Items.Add("Chưa có công thức");
            cboTrangThai.Items.Add("Đã có công thức");
            cboTrangThai.SelectedIndex = 0;
            SetComboBoxPlaceholder(cboTrangThai, "Trạng thái CT");
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            using (insertProduct form = new insertProduct())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
                sanPhamBUS bus = new sanPhamBUS();
                bus.docDSSanPham();
                loadDanhSachSanPham(sanPhamBUS.ds);
            }
        }

        private void btnSuaSP_Click(object sender, EventArgs e)
        {
            if (tbSanPham.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tbSanPham.SelectedRows[0];

                sanPhamDTO sp = new sanPhamDTO
                {
                    MaSP = Convert.ToInt32(row.Cells["Mã SP"].Value),
                    MaLoai = new loaiSanPhamBUS().layDanhSachLoai()
                                .FirstOrDefault(l => l.TenLoai == row.Cells["Tên Loại"].Value.ToString())?.MaLoai ?? 0,
                    TenSP = row.Cells["Tên SP"].Value.ToString(),
                    Hinh = row.Cells["Hình"].Value.ToString(),
                    Gia = float.Parse(row.Cells["Giá"].Value.ToString()),
                    //TrangThai = row.Cells["Trạng thái SP"].Value.ToString() == "Đang bán" ? 1 : 0,
                    TrangThaiCT = row.Cells["Trạng thái CT"].Value.ToString() == "Đã có công thức" ? 1 : 0
                };

                using (updateProduct form = new updateProduct(sp))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }

                sanPhamBUS bus = new sanPhamBUS();
                bus.docDSSanPham();
                loadDanhSachSanPham(sanPhamBUS.ds);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            if (tbSanPham.SelectedRows.Count > 0)
            {
                int maSP = Convert.ToInt32(tbSanPham.SelectedRows[0].Cells["Mã SP"].Value);
                deleteProduct form = new deleteProduct(maSP);
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();

                sanPhamBUS bus = new sanPhamBUS();
                bus.docDSSanPham();
                loadDanhSachSanPham(sanPhamBUS.ds);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            if (tbSanPham.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tbSanPham.SelectedRows[0];

                sanPhamDTO sp = new sanPhamDTO
                {
                    MaSP = Convert.ToInt32(row.Cells["Mã SP"].Value),
                    TenSP = row.Cells["Tên SP"].Value.ToString(),
                    TenLoai = row.Cells["Tên Loại"].Value.ToString(),
                    Hinh = row.Cells["Hình"].Value.ToString(),
                    Gia = float.Parse(row.Cells["Giá"].Value.ToString()),
                    //TrangThai = row.Cells["Trạng thái SP"].Value.ToString() == "Đang bán" ? 1 : 0,
                    TrangThaiCT = row.Cells["Trạng thái CT"].Value.ToString() == "Đã có công thức" ? 1 : 0
                };
                using (detailSanPham form = new detailSanPham(sp))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void kiemTraClickTable(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.RowIndex == lastSelectedRowSanPham)
            {
                tbSanPham.ClearSelection();
                lastSelectedRowSanPham = -1;

                btnThemSP.Enabled = true;
                btnSuaSP.Enabled = false;
                btnXoaSP.Enabled = false;
                btnChiTiet.Enabled = false;
                return;
            }

            tbSanPham.ClearSelection();
            tbSanPham.Rows[e.RowIndex].Selected = true;
            lastSelectedRowSanPham = e.RowIndex;

            btnThemSP.Enabled = true;
            btnSuaSP.Enabled = true;
            btnXoaSP.Enabled = true;
            btnChiTiet.Enabled = true;
        }

        private void btnLoaiSP_Click(object sender, EventArgs e)
        {
            using (loaiSPGUI form = new loaiSPGUI())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
            loadComboBoxLoaiSPTK();
        }

        private void btnExcelSP_Click(object sender, EventArgs e)
        {
            using (selectExcelSanPham form = new selectExcelSanPham())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
            sanPhamBUS bus = new sanPhamBUS();
            bus.docDSSanPham();
            loadDanhSachSanPham(sanPhamBUS.ds);
        }


        private void timKiemCoBan()
        {
            string tim = txtTimKiemSP.Text.Trim();
            int index = cboTimKiemSP.SelectedIndex;

            if (index < 0 || string.IsNullOrWhiteSpace(tim))
            {
                MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            sanPhamBUS bus = new sanPhamBUS();
            List<sanPhamDTO> kq = bus.timKiemCoBan(tim, index);

            if (kq != null && kq.Count > 0)
            {
                tbSanPham.Columns.Clear();
                tbSanPham.DataSource = null;
                loadDanhSachSanPham(kq);
            }
            else
            {
                MessageBox.Show("Không có kết quả tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                bus.docDSSanPham();
                loadDanhSachSanPham(sanPhamBUS.ds);

                txtTimKiemSP.Clear();
                cboTimKiemSP.SelectedIndex = -1;
                SetPlaceholder(txtTimKiemSP, "Nhập giá trị cần tìm");
                SetComboBoxPlaceholder(cboTimKiemSP, "Chọn giá trị TK");
            }
        }

        private void timKiemNangCao()
        {
            int maLoai = (cboLoaiSP.SelectedValue == null) ? -1 : Convert.ToInt32(cboLoaiSP.SelectedValue);
            int trangThaiCT = (cboTrangThai.SelectedIndex <= 0) ? -1 : cboTrangThai.SelectedIndex - 1;

            string tenSP = string.IsNullOrWhiteSpace(txtTenSPTK.Text) ? null : txtTenSPTK.Text.Trim();
            // Nếu đang là placeholder thì xem như rỗng
            if (txtTenSPTK.Text == "Tên SP") tenSP = null;
            float giaMin = -1, giaMax = -1;
            string txtMin = txtGiaMin.Text.Trim();
            string txtMax = txtGiaMax.Text.Trim();

            // Nếu đang là placeholder thì xem như rỗng
            if (txtMin == "Giá min") txtMin = "";
            if (txtMax == "Giá max") txtMax = "";

            if (!string.IsNullOrEmpty(txtMin))
            {
                if (!float.TryParse(txtMin.Replace(".", "").Replace(",", "."), System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out giaMin))
                {
                    MessageBox.Show("Giá tối thiểu không hợp lệ. Vui lòng nhập số hợp lệ!",
                        "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(txtMax))
            {
                if (!float.TryParse(txtMax.Replace(".", "").Replace(",", "."), System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out giaMax))
                {
                    MessageBox.Show("Giá tối đa không hợp lệ. Vui lòng nhập số hợp lệ!",
                        "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (giaMin != -1 && giaMax != -1 && giaMin > giaMax)
            {
                MessageBox.Show("Giá tối thiểu phải ≤ giá tối đa.", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (maLoai == -1 && trangThaiCT == -1 && tenSP == null && giaMin == -1 && giaMax == -1)
            {
                MessageBox.Show("Vui lòng nhập ít nhất một điều kiện để tìm kiếm!",
                    "Thông báo rỗng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //// Debug
            string debugInfo =
                $"[DEBUG - TÌM KIẾM NÂNG CAO]\n\n" +
                $"Mã loại: {maLoai}\n" +
                $"Trạng thái CT: {trangThaiCT}\n" +
                $"Tên SP: {(tenSP ?? "(null)")}\n" +
                $"Giá tối thiểu: {(giaMin == -1 ? "(không lọc)" : giaMin.ToString())}\n" +
                $"Giá tối đa: {(giaMax == -1 ? "(không lọc)" : giaMax.ToString())}";

            MessageBox.Show(debugInfo, "DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Information);

            sanPhamBUS bus = new sanPhamBUS();
            var dskq = bus.timKiemNangCaoSP(maLoai, trangThaiCT, giaMin, giaMax, tenSP);

            if (dskq != null && dskq.Count > 0)
            {
                loadDanhSachSanPham(dskq);
                hienThiPlaceHolderSanPham();
            }
            else
            {
                MessageBox.Show("Không có kết quả tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                bus.docDSSanPham();
                loadDanhSachSanPham(sanPhamBUS.ds);
            }
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



        private void hienThiPlaceHolderSanPham()
        {
            SetPlaceholder(txtTenSPTK, "Tên SP");
            SetPlaceholder(txtGiaMin, "Giá min");
            SetPlaceholder(txtGiaMax, "Giá max");
            SetPlaceholder(txtTimKiemSP, "Nhập giá trị cần tìm");
            //SetPlaceholder(txtTenDonViTK, "Tên đơn vị");
            SetComboBoxPlaceholder(cboLoaiSP, "Loại SP");
            SetComboBoxPlaceholder(cboTrangThai, "Trạng thái CT");
            SetComboBoxPlaceholder(cboTimKiemSP, "Chọn giá trị TK");
        }

        private void btnRefreshSP_Click(object sender, EventArgs e)
        {
            sanPhamBUS bus = new sanPhamBUS();
            bus.docDSSanPham();
            loadDanhSachSanPham(sanPhamBUS.ds);

            txtTimKiemSP.Clear();
            txtGiaMin.Clear();
            txtGiaMax.Clear();
            txtTenSPTK.Clear();

            cboTimKiemSP.SelectedIndex = -1;
            cboLoaiSP.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = -1;

            hienThiPlaceHolderSanPham();
        }


        // dòng này là để cho khi mà mình load trang nó kh chọn dòng đầu nha
        private void tbSanPham_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tbSanPham.ClearSelection();
        }

        private void rdoTimNangCao_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimNangCao.Checked)
            {
                rdoTimCoBan.Checked = false;
                txtTimKiemSP.Enabled = false;
                cboTimKiemSP.Enabled = false;
            }
            else
            {
                txtTimKiemSP.Enabled = true;
                cboTimKiemSP.Enabled = true;
            }
        }

        private void rdoTimCoBan_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rdoTimCoBan.Checked)
            {
                rdoTimNangCao.Checked = false;
                txtGiaMin.Enabled = false;
                txtGiaMax.Enabled = false;
                txtTenSPTK.Enabled = false;
                cboTrangThai.Enabled = false;
                cboLoaiSP.Enabled = false;
            }
            else
            {
                txtGiaMin.Enabled = true;
                txtGiaMax.Enabled = true;
                txtTenSPTK.Enabled = true;
                cboTrangThai.Enabled = true;
                cboLoaiSP.Enabled = true;
            }
        }

        private void btnThucHienTimKiem_Click_1(object sender, EventArgs e)
        {
            if (rdoTimCoBan.Checked)
            {
                timKiemCoBan();
            }
            else
            {
                timKiemNangCao();
            }
        }
    }
}
