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
    public partial class vaiTroVaQuyenGUI : Form
    {
        private vaitroBUS busVaiTro = new vaitroBUS();
        private quyenBUS busQuyen = new quyenBUS();
        public vaiTroVaQuyenGUI()
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);


            BindingList<vaitroDTO> dsVaiTro = new vaitroBUS().LayDanhSach();
            loadDanhSachVaiTro(dsVaiTro);

            BindingList<quyenDTO> dsQuyen = new quyenBUS().LayDanhSach();
            loadDanhSachQuyen(dsQuyen);
        }

        public void loadDanhSachVaiTro(BindingList<vaitroDTO> ds)
        {
            tbVaiTro.AutoGenerateColumns = false;
            tbVaiTro.Columns.Clear();

            tbVaiTro.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaVaiTro", HeaderText = "Mã vai trò" });
            tbVaiTro.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenVaiTro", HeaderText = "Tên vai trò" });

            tbVaiTro.DataSource = ds;
            tbVaiTro.ReadOnly = true;

            btnSuaVaiTro.Enabled = false;
            btnXoaVaiTro.Enabled = false;
            tbVaiTro.ClearSelection();
            loadFontVaChu(tbVaiTro);
        }

        public void loadDanhSachQuyen(BindingList<quyenDTO> ds)
        {
            tbQuyen.AutoGenerateColumns = false;
            tbQuyen.Columns.Clear();

            tbQuyen.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaQuyen", HeaderText = "Mã quyền" });
            tbQuyen.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenQuyen", HeaderText = "Tên quyền" });

            tbQuyen.DataSource = ds;
            tbQuyen.ReadOnly = true;

            btnSuaQuyen.Enabled = false;
            btnXoaQuyen.Enabled = false;
            tbQuyen.ClearSelection();
            loadFontVaChu(tbQuyen);
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

        private void tbVaiTro_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tbVaiTro.ClearSelection();
        }

        private void btnThemVaiTro_Click(object sender, EventArgs e)
        {
            string tenVaiTro = txtTenVaiTro.Text.Trim();

            if(busVaiTro.KiemTraRong(tenVaiTro))
            {
                MessageBox.Show("Không được để trống tên vai trò","Cảnh báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtTenVaiTro.Focus();
                return;
            }

            if (busVaiTro.kiemTraTrungTen(tenVaiTro))
            {
                MessageBox.Show("Tên vai trò này đã tồn tại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenVaiTro.Focus();
                return;
            }

            vaitroDTO ct = new vaitroDTO();
            ct.MaVaiTro = busVaiTro.layMa();
            ct.TenVaiTro = tenVaiTro;

            bool kq = busVaiTro.themVaiTro(ct);

            if(kq)
            {
                MessageBox.Show("Thêm vai trò thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenVaiTro.Clear();
            }else
            {
                MessageBox.Show("Thêm vai trò thất bại", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private int? selectedRowIndexVaiTro = null;

        private void tbVaiTro_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < tbVaiTro.Rows.Count)
            {

                if (selectedRowIndexVaiTro == e.RowIndex)
                {
                    txtTenVaiTro.Clear();
                    btnThemVaiTro.Enabled = true;
                    btnSuaVaiTro.Enabled = false;
                    btnXoaVaiTro.Enabled = false;
                    selectedRowIndexVaiTro = null;
                    tbVaiTro.ClearSelection();
                    return;
                }

                selectedRowIndexVaiTro = e.RowIndex;

                DataGridViewRow row = tbVaiTro.Rows[e.RowIndex];
                vaitroDTO dv = row.DataBoundItem as vaitroDTO;

                txtTenVaiTro.Text = dv.TenVaiTro;

                btnThemVaiTro.Enabled = false;
                btnSuaVaiTro.Enabled = true;
                btnXoaVaiTro.Enabled = true;
            }
        }

        private void btnSuaVaiTro_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tbVaiTro.SelectedRows[0];
            vaitroDTO dv = row.DataBoundItem as vaitroDTO;
            int maVaiTro = dv.MaVaiTro;
            string tenMoi = txtTenVaiTro.Text.Trim();
            if (busVaiTro.KiemTraRong(tenMoi))
            {
                MessageBox.Show("Vui lòng nhập tên vai trò mới", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenVaiTro.Focus();
                return;
            }
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn sửa vai trò này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                vaitroDTO ct = new vaitroDTO();
                ct.MaVaiTro = maVaiTro;
                ct.TenVaiTro = tenMoi;

                if (ct != null)
                {
                    MessageBox.Show("Sửa vai trò thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busVaiTro.suaVaiTro(ct);
                    txtTenVaiTro.Clear();
                    tbVaiTro.Refresh();
                    tbVaiTro.ClearSelection();
                }
                else
                {
                    MessageBox.Show("Sửa vai trò thất bại", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void btnXoaVaiTro_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tbVaiTro.SelectedRows[0];
            vaitroDTO ct = row.DataBoundItem as vaitroDTO;

            int maVT = ct.MaVaiTro;

            DialogResult result = MessageBox.Show(
    "Bạn có chắc muốn xóa vai trò này không?",
    "Xác nhận",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question
);

            if (result == DialogResult.Yes)
            {
                if (ct != null)
                {
                    MessageBox.Show("Xóa vai trò thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busVaiTro.xoaVaiTro(maVT);
                    txtTenVaiTro.Clear();
                    tbVaiTro.Refresh();
                }
                else
                {
                    MessageBox.Show("Xóa vai trò thất bại", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            BindingList<vaitroDTO> dsVT = new vaitroBUS().LayDanhSach();
            string timVT = txtTimVaiTro.Text.ToLower().Trim();
            string giaTriTim = cboTimVaiTro.SelectedItem != null ? cboTimVaiTro.SelectedItem.ToString() : "";
            if (cboTimVaiTro.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboTimVaiTro.Focus();
                return;
            }
            if(busVaiTro.KiemTraRong(timVT))
            {
                MessageBox.Show("Vui lòng nhập giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimVaiTro.Focus();
                return;
            }
            List<vaitroDTO> dsTim = new List<vaitroDTO>();
            if(giaTriTim == "Mã vai trò")
            {
                dsTim = (from item in dsVT
                         where item.MaVaiTro.ToString().Contains(timVT)
                         orderby item.MaVaiTro
                         select item
                         ).ToList();
            }
            if(giaTriTim == "Tên vai trò")
            {
                dsTim = (from item in dsVT
                         where item.TenVaiTro.ToLower().Contains(timVT)
                         orderby item.MaVaiTro
                         select item).ToList();
            }

            if(dsTim != null && dsTim.Count > 0)
            {
                BindingList<vaitroDTO> dsBinding = new BindingList<vaitroDTO>(dsTim);
                loadDanhSachVaiTro(dsBinding);
                loadFontVaChu(tbVaiTro);
            }else
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnRs_Click(object sender, EventArgs e)
        {
            txtTenVaiTro.Clear();
            txtTimVaiTro.Clear();
            cboTimVaiTro.SelectedIndex = -1;
            BindingList<vaitroDTO> ds = new vaitroBUS().LayDanhSach();
            loadDanhSachVaiTro(ds);
            loadFontVaChu(tbVaiTro);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tbQuyen_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tbQuyen.ClearSelection();
        }

        private void btnThemQuyen_Click(object sender, EventArgs e)
        {
            string tenQuyen = txtTenQuyen.Text.Trim();
            if(busQuyen.KiemTraRong(tenQuyen))
            {
                MessageBox.Show("Không được để trống tên quyền", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenQuyen.Focus();
                return;
            }
            if(busQuyen.kiemTraTrungTen(tenQuyen))
            {
                MessageBox.Show("Tên quyền đã tồn tại trong hệ thống", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenQuyen.Focus();
                return;
            }
            quyenDTO ct = new quyenDTO();
            ct.MaQuyen = busQuyen.LayMa();
            ct.TenQuyen = tenQuyen;

            bool kq = busQuyen.themQuyen(ct);
            if(kq)
            {
                MessageBox.Show("Thêm quyền mới thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenQuyen.Clear();
            }else
            {
                MessageBox.Show("Thêm quyền mới thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenQuyen.Clear();
                return;
            }
        }

        private int? selectedRowIndexQuyen = null;

        private void tbQuyen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < tbQuyen.Rows.Count)
            {

                if (selectedRowIndexQuyen == e.RowIndex)
                {
                    txtTenQuyen.Clear();
                    btnThemQuyen.Enabled = true;
                    btnSuaQuyen.Enabled = false;
                    btnXoaQuyen.Enabled = false;
                    selectedRowIndexQuyen = null;
                    tbQuyen.ClearSelection();
                    return;
                }

                selectedRowIndexQuyen = e.RowIndex;

                DataGridViewRow row = tbQuyen.Rows[e.RowIndex];
                quyenDTO dv = row.DataBoundItem as quyenDTO;

                txtTenQuyen.Text = dv.TenQuyen;

                btnThemQuyen.Enabled = false;
                btnSuaQuyen.Enabled = true;
                btnXoaQuyen.Enabled = true;
            }
        }

        private void btnSuaQuyen_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tbQuyen.SelectedRows[0];
            quyenDTO dv = row.DataBoundItem as quyenDTO;
            int maQuyen = dv.MaQuyen;
            string tenMoi = txtTenQuyen.Text.Trim();
            if (busQuyen.KiemTraRong(tenMoi))
            {
                MessageBox.Show("Vui lòng nhập tên quyền mới", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenQuyen.Focus();
                return;
            }
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn sửa quyền này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                quyenDTO ct = new quyenDTO();
                ct.MaQuyen = maQuyen;
                ct.TenQuyen = tenMoi;

                if (ct != null)
                {
                    MessageBox.Show("Sửa quyền thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busQuyen.suaQuyen(ct);
                    txtTenQuyen.Clear();
                    tbQuyen.Refresh();
                    tbQuyen.ClearSelection();
                }
                else
                {
                    MessageBox.Show("Sửa quyền thất bại", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void btnXoaQuyen_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tbQuyen.SelectedRows[0];
            quyenDTO ct = row.DataBoundItem as quyenDTO;

            int maQuyen = ct.MaQuyen;

            DialogResult result = MessageBox.Show(
    "Bạn có chắc muốn xóa quyền này không?",
    "Xác nhận",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question
);

            if (result == DialogResult.Yes)
            {
                if (ct != null)
                {
                    MessageBox.Show("Xóa quyền thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    busQuyen.xoaQuyen(maQuyen);
                    txtTenQuyen.Clear();
                    tbQuyen.Refresh();
                    tbQuyen.ClearSelection();
                }
                else
                {
                    MessageBox.Show("Xóa quyền thất bại", "Báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void timQuyen_Click(object sender, EventArgs e)
        {
            if(cboTimKiemQuyen.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboTimKiemQuyen.Focus();
                return;
            }
            if(string.IsNullOrWhiteSpace(txtTimKiemQuyen.Text))
            {
                MessageBox.Show("Vui lòng nhập giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimKiemQuyen.Focus();
                return;
            }
            string tim = txtTimKiemQuyen.Text.ToLower().Trim();
            string giaTriTim = cboTimKiemQuyen.SelectedItem.ToString();
            BindingList<quyenDTO> dsQuyen = new quyenBUS().LayDanhSach();
            List<quyenDTO> dsTim = new List<quyenDTO>();
            if(giaTriTim == "Mã quyền")
            {
                dsTim = (from item in dsQuyen
                         where item.MaQuyen.ToString().Contains(tim)
                         orderby item.MaQuyen
                         select item).ToList();
            }
            if(giaTriTim == "Tên quyền")
            {
                dsTim = (from item in dsQuyen
                         where item.TenQuyen.ToLower().Contains(tim)
                         orderby item.MaQuyen
                         select item).ToList();
            }
            if(dsTim != null && dsTim.Count > 0)
            {
                BindingList<quyenDTO> dsBinding = new BindingList<quyenDTO>(dsTim);
                loadDanhSachQuyen(dsBinding);
                loadFontVaChu(tbQuyen);
            }else
            {
                MessageBox.Show("Không tìm thấy giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnRSquyen_Click(object sender, EventArgs e)
        {
            txtTenQuyen.Clear();
            cboTimKiemQuyen.SelectedIndex = -1;
            txtTimKiemQuyen.Clear();
            BindingList<quyenDTO> ds = new quyenBUS().LayDanhSach();
            loadDanhSachQuyen(ds);
            loadFontVaChu(tbQuyen);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
