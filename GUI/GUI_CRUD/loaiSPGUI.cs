using BUS;
using DTO;
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
namespace GUI.GUI_CRUD
{
    public partial class loaiSPGUI : Form
    {
        private int? selectedRowIndex = null;
        private loaiSanPhamBUS busLoai = new loaiSanPhamBUS();
        private nhomBUS busNhom = new nhomBUS();
        private BindingList<nhomDTO> dsNhom;
        public loaiSPGUI()
        {
            InitializeComponent();
        }
        private void loaiSPGUI_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            nhomBUS busNhom = new nhomBUS();
            dsNhom = busNhom.layDanhSach();
            loadComboNhom();

            busLoai.LayDanhSach();
            loadDanhSachLoaiSP(loaiSanPhamBUS.ds);
            loadFontChuVaSize();

            busNhom.layDanhSach();
            loadDanhSachNhom(nhomBUS.ds);
            loadFontChuVaSizeNhom();

            btnSuaLoaiSp.Enabled = false;
            btnXoaLoaiSP.Enabled = false;
        }
        private void loadFontChuVaSize()
        {
            foreach (DataGridViewColumn col in tableLoaiSP.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableLoaiSP.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableLoaiSP.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tableLoaiSP.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tableLoaiSP.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            tableLoaiSP.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableLoaiSP.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableLoaiSP.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableLoaiSP.Refresh();
        }

        private void loadFontChuVaSizeNhom()
        {
            foreach (DataGridViewColumn col in tableNhom.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableNhom.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableNhom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tableNhom.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tableNhom.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            tableNhom.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableNhom.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableNhom.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableNhom.Refresh();
        }

        private void loadDanhSachLoaiSP(BindingList<loaiDTO> ds)
        {
            tableLoaiSP.AutoGenerateColumns = false;
            tableLoaiSP.Columns.Clear();

            tableLoaiSP.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaLoai", HeaderText = "Mã loại" });
            tableLoaiSP.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenLoai", HeaderText = "Tên loại" });
            tableLoaiSP.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNhom", HeaderText = "Tên nhóm" });

            tableLoaiSP.DataSource = ds;

            btnSuaLoaiSp.Enabled = false;
            btnXoaLoaiSP.Enabled = false;
            tableLoaiSP.ReadOnly = true;
            tableLoaiSP.ClearSelection();
        }

        private void loadDanhSachNhom(BindingList<nhomDTO> ds)
        {
            tableNhom.AutoGenerateColumns = false;
            tableNhom.Columns.Clear();

            tableNhom.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNhom", HeaderText = "Mã nhóm" });
            tableNhom.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenNhom", HeaderText = "Tên nhóm" });

            tableNhom.DataSource = ds;

            btnSuaNhom.Enabled = false;
            btnXoaNhom.Enabled = false;
            tableNhom.ReadOnly = true;
            tableNhom.ClearSelection();
        }
        private void tableLoaiSP_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            loaiDTO loai = tableLoaiSP.Rows[e.RowIndex].DataBoundItem as loaiDTO;
            if(loai == null)
            {
                return;
            }
            if (tableLoaiSP.Columns[e.ColumnIndex].HeaderText == "Tên nhóm")
            {
                nhomDTO nhom = dsNhom.FirstOrDefault(x => x.MaNhom == loai.MaNhom);
                e.Value = nhom?.TenNhom ?? "Không xác định";
            }    

        }
        public void loadComboNhom()
        {
            nhomBUS bus = new nhomBUS();
            BindingList<nhomDTO> dsNhom = bus.layDanhSach();
            cboNhom.DisplayMember = "TenNhom";
            cboNhom.ValueMember = "MaNhom";
            cboNhom.DataSource = dsNhom;
            cboNhom.SelectedIndex = -1;
        }



        private void ResetForm()
        {
            txtLoaiSp.Clear();
            txtLoaiSp.Focus();
            cboNhom.SelectedIndex = -1;
            tableLoaiSP.ClearSelection();

            btnThemLoaiSP.Enabled = true;
            btnSuaLoaiSp.Enabled = false;
            btnXoaLoaiSP.Enabled = false;

            selectedRowIndex = null;
        }


        private void tableLoaiSP_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < tableLoaiSP.Rows.Count)
            {
                if (selectedRowIndex == e.RowIndex)
                {
                    ResetForm();
                    return;
                }
                selectedRowIndex = e.RowIndex;

                DataGridViewRow row = tableLoaiSP.Rows[e.RowIndex];
                loaiDTO ct = row.DataBoundItem as loaiDTO;

                txtLoaiSp.Text = ct.TenLoai;
                cboNhom.SelectedValue = ct.MaNhom;

                btnThemLoaiSP.Enabled = false;
                btnSuaLoaiSp.Enabled = true;
                btnXoaLoaiSP.Enabled = true;
            }
        }

        private void btnThemLoaiSP_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLoaiSp.Text) || cboNhom.SelectedIndex == -1)
            {
                MessageBox.Show("Nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maLoai = busLoai.layMa();

            loaiDTO ct = new loaiDTO();
            ct.MaLoai = maLoai;
            ct.TenLoai = txtLoaiSp.Text;
            ct.MaNhom = (int)cboNhom.SelectedValue;

            if (ct != null)
            {
                MessageBox.Show("Thêm loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                busLoai.themLoai(ct);
                txtLoaiSp.Clear();
                cboNhom.SelectedIndex = -1;
                txtLoaiSp.Focus();
                tableLoaiSP.ClearSelection();
            }
            else
            {
                MessageBox.Show("Lỗi khi thêm loại sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaLoaiSp_Click_1(object sender, EventArgs e)
        {
            DataGridViewRow row = tableLoaiSP.SelectedRows[0];

            loaiDTO loai = row.DataBoundItem as loaiDTO;

            int maLoai = loai.MaLoai;
            string tenMoi = txtLoaiSp.Text.Trim();
            int maNhom = (int)cboNhom.SelectedValue;
            if (string.IsNullOrWhiteSpace(tenMoi))
            {
                MessageBox.Show("Vui lòng nhập tên loại sản phẩm mới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn sửa loại sản phẩm này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                loaiDTO ct = new loaiDTO();
                ct.MaLoai = maLoai;
                ct.TenLoai = tenMoi;
                ct.MaNhom = maNhom;

                if(ct != null)
                {
                    MessageBox.Show("Sửa loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busLoai.suaLoai(ct);
                    ResetForm();
                }else
                {
                    MessageBox.Show("Lỗi kh sửa đc loại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void btnXoaLoaiSP_Click_1(object sender, EventArgs e)
        {
            DataGridViewRow row = tableLoaiSP.SelectedRows[0];
            loaiDTO loai = row.DataBoundItem as loaiDTO;
            int maXoa = loai.MaLoai;
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa loại sản phẩm này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                if(maXoa != -1)
                {
                    MessageBox.Show("Xóa loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busLoai.Xoa(maXoa);
                    ResetForm();
                }else
                {
                    MessageBox.Show("Xóa loại sản phẩm thất bại!", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            if(cboLoai.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtTim.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int index = cboLoai.SelectedIndex;
            string tim = txtTim.Text.Trim();
            BindingList<loaiDTO> dskq = busLoai.timKiemCoBan(tim, index);
            if (dskq != null && dskq.Count > 0)
            {
                loadDanhSachLoaiSP(dskq);
                loadFontChuVaSize();
                tableLoaiSP.ClearSelection();
            }else
            {
                MessageBox.Show("Không tìm thấy kết quả cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnRs_Click(object sender, EventArgs e)
        {
            cboLoai.SelectedIndex = -1;
            txtTim.Clear();
            busLoai.LayDanhSach();
            loadDanhSachLoaiSP(loaiSanPhamBUS.ds);
            loadFontChuVaSize();
        }

        private void tableLoaiSP_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tableLoaiSP.ClearSelection();
        }

        private void btnThemNhom_Click(object sender, EventArgs e)
        {
            if(busNhom.kiemTraRong(txtTenNhom.Text))
            {
                MessageBox.Show("Không được để trống tên nhóm","Cảnh báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtTenNhom.Focus();
                return;
            }
            int maNhom = busNhom.layMa();
            nhomDTO ct = new nhomDTO();
            ct.TenNhom = txtTenNhom.Text.Trim();
            ct.MaNhom = maNhom;

            bool kq = busNhom.them(ct);
            if(kq)
            {
                MessageBox.Show("Thêm nhóm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenNhom.Clear();
            }else
            {
                MessageBox.Show("Thêm nhóm thất bại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }    
        }

        private void tableNhom_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tableNhom.ClearSelection();
        }

        private void btnSuaNhom_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tableNhom.SelectedRows[0];
            nhomDTO nhom = row.DataBoundItem as nhomDTO;

            int maNhom = nhom.MaNhom;
            string tenMoi = txtTenNhom.Text.Trim();
            if(busNhom.kiemTraRong(txtTenNhom.Text))
            {
                MessageBox.Show("Không được để trống tên nhóm", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNhom.Focus();
                return;
            }

            DialogResult rs = MessageBox.Show("Bạn có chắc chắn muốn sửa tên nhóm này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if(rs == DialogResult.Yes)
            {
                nhomDTO ct = new nhomDTO();
                ct.MaNhom = maNhom;
                ct.TenNhom = tenMoi;

                if(ct != null)
                {
                    MessageBox.Show("Sửa tên nhóm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busNhom.sua(ct);
                    ResetFormNhom();
                }else
                {
                    MessageBox.Show("Sửa tên nhóm thất bại", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }
        int selectedRowIndexNhom = -1;
        private void ResetFormNhom()
        {
            txtTenNhom.Clear();
            btnThemNhom.Enabled = true;
            btnSuaNhom.Enabled = false;
            btnXoaNhom.Enabled = false;
            selectedRowIndexNhom = -1;
            tableNhom.ClearSelection();
        }

        private void tableNhom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < tableNhom.Rows.Count)
            {
                if (selectedRowIndexNhom == e.RowIndex)
                {
                    ResetFormNhom();
                    return;
                }
                selectedRowIndexNhom = e.RowIndex;

                DataGridViewRow row = tableNhom.Rows[e.RowIndex];
                nhomDTO ct = row.DataBoundItem as nhomDTO;

                txtTenNhom.Text = ct.TenNhom;

                btnThemNhom.Enabled = false;
                btnSuaNhom.Enabled = true;
                btnXoaNhom.Enabled = true;
            }
        }

        private void btnXoaNhom_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tableNhom.SelectedRows[0];
            nhomDTO nhom = row.DataBoundItem as nhomDTO;

            int maXoa = nhom.MaNhom;
            DialogResult rs = MessageBox.Show("Bạn có chắc chắn muốn xóa nhóm này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if(rs == DialogResult.Yes)
            {
                MessageBox.Show("Xóa nhóm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                busNhom.xoa(maXoa);
                ResetFormNhom();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTimNhom_Click(object sender, EventArgs e)
        {
            if(cboTimNhom.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị tìm kiếm", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTimNhom.Focus();
                return;
            }
            if(busNhom.kiemTraRong(txtTimNhom.Text))
            {
                MessageBox.Show("Vui lòng nhập giá trị tìm kiếm", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTim.Focus();
                return;
            }
            BindingList<nhomDTO> dsNHOM = new nhomBUS().layDanhSach();
            string tim = txtTimNhom.Text.Trim();
            string giaTriTim = cboTimNhom.SelectedItem.ToString();
            List<nhomDTO> dsTim = new List<nhomDTO>();
            if(giaTriTim == "Mã nhóm")
            {
                dsTim = (from item in dsNHOM
                         where item.MaNhom.ToString().Contains(tim)
                         orderby item.MaNhom
                         select item).ToList();
            }
            if(giaTriTim == "Tên nhóm")
            {
                dsTim = (from item in dsNHOM
                         where item.TenNhom.ToLower().Contains(tim)
                         orderby item.MaNhom
                         select item).ToList();
            }
            if(dsTim != null && dsTim.Count > 0)
            {
                BindingList<nhomDTO> dsSauKhiBinding = new BindingList<nhomDTO>(dsTim);
                loadDanhSachNhom(dsSauKhiBinding);
                loadFontChuVaSizeNhom();
                ResetFormNhom();
            }else
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnRSNHOM_Click(object sender, EventArgs e)
        {
            ResetFormNhom();
            txtTimNhom.Clear();
            cboTimNhom.SelectedIndex = -1;
            BindingList<nhomDTO> ds = new nhomBUS().layDanhSach();
            loadDanhSachNhom(ds);
            loadFontChuVaSizeNhom();
        }

        private void btnRSNHOM_Click_1(object sender, EventArgs e)
        {
            txtTenNhom.Clear();
            cboTimNhom.SelectedIndex = -1;
            txtTimNhom.Clear();
            BindingList<nhomDTO> ds = new nhomBUS().layDanhSach();
            loadDanhSachNhom(ds);
            loadFontChuVaSizeNhom();
        }
    }
}