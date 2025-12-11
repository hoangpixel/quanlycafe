using BUS;
using DAO;
using DTO;
using FONTS;
using OfficeOpenXml.Style;
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
    public partial class banVaKhuVuc : Form
    {
        private BindingList<khuVucDTO> dsKhuVuc;
        private BindingList<banDTO> dsBan;
        private banBUS busBan = new banBUS();
        private khuvucBUS busKhuVuc = new khuvucBUS();

        private int? selectedRowIndex = null;

        public banVaKhuVuc()
        {
            InitializeComponent();
        }

        private void banVaKhuVuc_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            dsKhuVuc = busKhuVuc.LayDanhSach();
            dsBan = busBan.LayDanhSach();

            cboKhuVuc.DataSource = dsKhuVuc;
            cboKhuVuc.DisplayMember = "TenKhuVuc";
            cboKhuVuc.ValueMember = "MaKhuVuc";

            loadDanhSachBan(dsBan);
            loadDanhSachKhuVuc(dsKhuVuc);
        }

        private void loadDanhSachBan(BindingList<banDTO> ds)
        {
            tableLoaiSP.AutoGenerateColumns = false;
            tableLoaiSP.Columns.Clear();

            tableLoaiSP.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaBan", HeaderText = "Mã bàn" });
            tableLoaiSP.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenBan", HeaderText = "Tên bàn" });
            tableLoaiSP.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaKhuVuc", HeaderText = "Khu vực" });

            tableLoaiSP.DataSource = ds;

            btnSuaLoaiSp.Enabled = false;
            btnXoaLoaiSP.Enabled = false;
            tableLoaiSP.ReadOnly = true;
            tableLoaiSP.ClearSelection();
            loadFontChuVaSize();
        }

        private void loadDanhSachKhuVuc(BindingList<khuVucDTO> ds)
        {
            tableNhom.AutoGenerateColumns = false;
            tableNhom.Columns.Clear();

            tableNhom.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaKhuVuc", HeaderText = "Mã khu vực" });
            tableNhom.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenKhuVuc", HeaderText = "Tên khu vực" });

            tableNhom.DataSource = ds;

            btnSuaNhom.Enabled = false;
            btnXoaNhom.Enabled = false;
            tableNhom.ReadOnly = true;
            tableNhom.ClearSelection();
            loadFontChuVaSizeNhom();
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

        private void tableLoaiSP_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tableLoaiSP.ClearSelection();
        }

        private void tableNhom_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tableNhom.ClearSelection();
        }

        private void tableLoaiSP_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            banDTO ban = tableLoaiSP.Rows[e.RowIndex].DataBoundItem as banDTO;

            if (tableLoaiSP.Columns[e.ColumnIndex].HeaderText == "Khu vực")
            {
                khuVucDTO kh = dsKhuVuc.FirstOrDefault(x => x.MaKhuVuc == ban.MaKhuVuc);
                e.Value = kh?.TenKhuVuc ?? "Không xác định";
            }
        }

        private void btnThemLoaiSP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenBan.Text) || cboKhuVuc.SelectedIndex == -1)
            {
                MessageBox.Show("Nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(busBan.KiemTraTrungTen(txtTenBan.Text.Trim()))
            {
                MessageBox.Show("Bàn này đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenBan.Focus();
                return;
            }

            int maBan = busBan.LayMa();

            banDTO ct = new banDTO();
            ct.MaBan = maBan;
            ct.TenBan = txtTenBan.Text.Trim();
            ct.MaKhuVuc = (int)cboKhuVuc.SelectedValue;

            if (ct != null)
            {
                MessageBox.Show("Thêm bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                busBan.ThemBan(ct);
                txtTenBan.Clear();
                cboKhuVuc.SelectedIndex = -1;
                tableLoaiSP.ClearSelection();
            }
            else
            {
                MessageBox.Show("Lỗi khi thêm bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaLoaiSp_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tableLoaiSP.SelectedRows[0];

            banDTO ban = row.DataBoundItem as banDTO;

            int maBan = ban.MaBan;
            string tenMoi = txtTenBan.Text.Trim();
            int maKhuVuc = (int)cboKhuVuc.SelectedValue;
            if (string.IsNullOrWhiteSpace(tenMoi))
            {
                MessageBox.Show("Vui lòng nhập tên bàn mới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenBan.Focus();
                return;
            }

            if(txtTenBan.Text.Trim() != ban.TenBan && busBan.KiemTraTrungTen(txtTenBan.Text.Trim()))
            {
                MessageBox.Show("Bàn này đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenBan.Focus();
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
                banDTO ct = new banDTO();
                ct.MaBan = maBan;
                ct.TenBan = tenMoi;
                ct.MaKhuVuc = maKhuVuc;

                if (ct != null)
                {
                    MessageBox.Show("Sửa loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busBan.SuaBan(ct);
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Lỗi kh sửa đc bàn", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void ResetForm()
        {
            txtTenBan.Clear();
            cboKhuVuc.SelectedIndex = -1;
            tableLoaiSP.ClearSelection();

            btnThemLoaiSP.Enabled = true;
            btnSuaLoaiSp.Enabled = false;
            btnXoaLoaiSP.Enabled = false;

            selectedRowIndex = null;
        }

        private void btnXoaLoaiSP_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tableLoaiSP.SelectedRows[0];
            banDTO ban = row.DataBoundItem as banDTO;
            int maXoa = ban.MaBan;
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa bàn này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                if (maXoa != -1)
                {
                    MessageBox.Show("Xóa loại sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busBan.XoaBan(maXoa);
                    ResetForm();
                }
                else
                {
                    MessageBox.Show("Xóa loại sản phẩm thất bại!", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void tableLoaiSP_CellClick(object sender, DataGridViewCellEventArgs e)
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
                banDTO ct = row.DataBoundItem as banDTO;

                txtTenBan.Text = ct.TenBan;
                cboKhuVuc.SelectedValue = ct.MaKhuVuc;

                btnThemLoaiSP.Enabled = false;
                btnSuaLoaiSp.Enabled = true;
                btnXoaLoaiSP.Enabled = true;
            }
        }

        private void btnThemNhom_Click(object sender, EventArgs e)
        {
            if (busKhuVuc.KiemTraRong(txtTenNhom.Text))
            {
                MessageBox.Show("Không được để trống tên khuc vực", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNhom.Focus();
                return;
            }

            if(busKhuVuc.KiemTraTrungTen(txtTenNhom.Text.Trim()))
            {
                MessageBox.Show("Đã tồn tại khu vực này rồi", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNhom.Focus();
                return;
            }
            int maKhucVuc = busKhuVuc.LayMa();
            khuVucDTO ct = new khuVucDTO();
            ct.TenKhuVuc = txtTenNhom.Text.Trim();
            ct.MaKhuVuc = maKhucVuc;

            bool kq = busKhuVuc.ThemKhuVuc(ct);
            if (kq)
            {
                MessageBox.Show("Thêm khu vực thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenNhom.Clear();
            }
            else
            {
                MessageBox.Show("Thêm khu vực thất bại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnSuaNhom_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tableNhom.SelectedRows[0];
            khuVucDTO nhom = row.DataBoundItem as khuVucDTO;

            int maKhuVuc = nhom.MaKhuVuc;
            string tenMoi = txtTenNhom.Text.Trim();
            if (busKhuVuc.KiemTraRong(txtTenNhom.Text))
            {
                MessageBox.Show("Không được để trống tên khu vực", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNhom.Focus();
                return;
            }

            if(txtTenNhom.Text.Trim() != nhom.TenKhuVuc && busKhuVuc.KiemTraTrungTen(txtTenNhom.Text.Trim()))
            {
                MessageBox.Show("Đã tồn tại khu vực này rồi", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNhom.Focus();
                return;
            }

            DialogResult rs = MessageBox.Show("Bạn có chắc chắn muốn sửa tên khu vực này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (rs == DialogResult.Yes)
            {
                khuVucDTO ct = new khuVucDTO();
                ct.MaKhuVuc = maKhuVuc;
                ct.TenKhuVuc = tenMoi;

                if (ct != null)
                {
                    MessageBox.Show("Sửa tên khu vực thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busKhuVuc.SuaKhuVuc(ct);
                    ResetFormNhom();
                }
                else
                {
                    MessageBox.Show("Sửa tên khu vực thất bại", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnXoaNhom_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tableNhom.SelectedRows[0];
            khuVucDTO nhom = row.DataBoundItem as khuVucDTO;

            int maXoa = nhom.MaKhuVuc;
            DialogResult rs = MessageBox.Show("Bạn có chắc chắn muốn xóa khu vực này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (rs == DialogResult.Yes)
            {
                MessageBox.Show("Xóa khu vực thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                busKhuVuc.XoaKhuVuc(maXoa);
                ResetFormNhom();
            }
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
                khuVucDTO ct = row.DataBoundItem as khuVucDTO;

                txtTenNhom.Text = ct.TenKhuVuc;

                btnThemNhom.Enabled = false;
                btnSuaNhom.Enabled = true;
                btnXoaNhom.Enabled = true;
            }
        }

        private void btnTimNhom_Click(object sender, EventArgs e)
        {
            if (cboTimNhom.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị tìm kiếm", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTimNhom.Focus();
                return;
            }
            if (busKhuVuc.KiemTraRong(txtTimNhom.Text))
            {
                MessageBox.Show("Vui lòng nhập giá trị tìm kiếm", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTim.Focus();
                return;
            }

            string tim = txtTimNhom.Text.Trim();
            string giaTriTim = cboTimNhom.SelectedItem.ToString();
            List<khuVucDTO> dsTim = new List<khuVucDTO>();
            if (giaTriTim == "Mã khu vực")
            {
                dsTim = (from item in dsKhuVuc
                         where item.MaKhuVuc.ToString().Contains(tim)
                         orderby item.MaKhuVuc
                         select item).ToList();
            }
            if (giaTriTim == "Tên khu vực")
            {
                dsTim = (from item in dsKhuVuc
                         where item.TenKhuVuc.ToLower().Contains(tim.ToLower())
                         orderby item.MaKhuVuc
                         select item).ToList();
            }
            if (dsTim != null && dsTim.Count > 0)
            {
                BindingList<khuVucDTO> dsSauKhiBinding = new BindingList<khuVucDTO>(dsTim);
                loadDanhSachKhuVuc(dsSauKhiBinding);
                ResetFormNhom();
            }
            else
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRSNHOM_Click(object sender, EventArgs e)
        {
            txtTenNhom.Clear();
            cboTimNhom.SelectedIndex = -1;
            txtTimNhom.Clear();
            BindingList<khuVucDTO> ds = new khuvucBUS().LayDanhSach();
            loadDanhSachKhuVuc(ds);
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            if (cboLoai.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị tìm kiếm", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoai.Focus();
                return;
            }
            if (busBan.KiemTraRong(txtTim.Text))
            {
                MessageBox.Show("Vui lòng nhập giá trị tìm kiếm", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTim.Focus();
                return;
            }

            string tim = txtTim.Text.Trim();
            string giaTriTim = cboLoai.SelectedItem.ToString();
            List<banDTO> dsTim = new List<banDTO>();
            if(giaTriTim == "Mã bàn")
            {
                dsTim = (from ban in dsBan
                         where ban.MaBan.ToString().Contains(tim)
                         orderby ban.MaBan
                         select ban).ToList();
            }
            if(giaTriTim == "Tên bàn")
            {
                dsTim = (from ban in dsBan
                         where ban.TenBan.ToLower().Contains(tim.ToLower())
                         orderby ban.MaBan
                         select ban
                         ).ToList();
            }
            if(giaTriTim == "Khu vực")
            {
                dsTim = (from ban in dsBan
                         join khuvuc in dsKhuVuc on ban.MaKhuVuc equals khuvuc.MaKhuVuc
                         where khuvuc.TenKhuVuc.ToLower().Contains(tim.ToLower())
                         orderby khuvuc.MaKhuVuc
                         select ban).ToList();
            }

            if(dsTim != null && dsTim.Count > 0)
            {
                BindingList<banDTO> dsBinding = new BindingList<banDTO>(dsTim);
                loadDanhSachBan(dsBinding);
                ResetForm();
            }else
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRs_Click(object sender, EventArgs e)
        {
            ResetForm();
            txtTim.Clear();
            cboLoai.SelectedIndex = -1;
            BindingList<banDTO> ds = new banBUS().LayDanhSach();
            loadDanhSachBan(ds);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
