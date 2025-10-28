using BUS;
using DTO;
using GUI.FONTS;
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
    public partial class donVivaHeSoGUI : Form
    {
        private int maNL = -1;
        public donVivaHeSoGUI()
        {
            InitializeComponent();
        }

        private void loadDanhSachDonVi(List<donViDTO> ds)
        {
            tableDonVi.Columns.Clear();
            tableDonVi.DataSource = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã đơn vị");
            dt.Columns.Add("Tên đơn vị");

            foreach (var dv in ds)
            {
                dt.Rows.Add(dv.MaDonVi, dv.TenDonVi);
            }

            tableDonVi.DataSource = dt;
            tableDonVi.ReadOnly = true;
            tableDonVi.Columns["Mã đơn vị"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tableDonVi.Columns["Tên đơn vị"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            btnSuaDonVi.Enabled = false;
            btnXoaDonVi.Enabled = false;

            tableDonVi.ClearSelection();
        }

        private void loadDanhSachHeSo(List<heSoDTO> ds)
        {
            tableHeSo.Columns.Clear();
            tableHeSo.DataSource = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("Tên nguyên liệu");
            dt.Columns.Add("Tên đơn vị");
            dt.Columns.Add("Hệ số");

            nguyenLieuBUS busNL = new nguyenLieuBUS();
            List<nguyenLieuDTO> dsNL = busNL.docDSNguyenLieu();

            donViBUS busDV = new donViBUS();
            List<donViDTO> dsDV = busDV.layDanhSachDonVi();

            foreach (var hs in ds)
            {
                string tenNL = dsNL.FirstOrDefault(l => l.MaNguyenLieu == hs.MaNguyenLieu)?.TenNguyenLieu ?? "Không xác định";
                string donVi = dsDV.FirstOrDefault(l => l.MaDonVi == hs.MaDonVi)?.TenDonVi ?? "Không xác định";

                dt.Rows.Add(tenNL, donVi,hs.HeSo);
            }

            tableHeSo.DataSource = dt;
            tableHeSo.ReadOnly = true;
            tableHeSo.Columns["Tên nguyên liệu"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tableHeSo.Columns["Tên đơn vị"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tableHeSo.Columns["Hệ số"].Width = 60;

            btnSuaHS.Enabled = false;
            btnXoaHeSO.Enabled = false;

            tableHeSo.ClearSelection();
        }

        private void donVivaHeSoGUI_Load(object sender, EventArgs e)
        {
            txtHeSo.DecimalPlaces = 3;
            txtHeSo.Increment = 0.001M;
            txtHeSo.Minimum = 0.000M;
            txtHeSo.Maximum = 999.999M;

            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            donViBUS bus = new donViBUS();
            bus.docDanhSachDonVi();
            loadDanhSachDonVi(donViBUS.ds);

            heSoBUS busHS = new heSoBUS();
            busHS.docDanhSachHeSo();
            loadDanhSachHeSo(heSoBUS.ds);

            cboDonVi.DataSource = bus.layDanhSachDonVi();
            cboDonVi.DisplayMember = "TenDonVi";
            cboDonVi.ValueMember = "MaDonVi";
            cboDonVi.SelectedIndex = -1;

            tableHeSo.ClearSelection();
            tableDonVi.ClearSelection();
        }

        private void btnThemDonVI_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDonVi.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đơn vị!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            donViBUS bus = new donViBUS();
            donViDTO ct = new donViDTO();
            ct.TenDonVi = txtDonVi.Text.Trim();

            if(bus.themDonVi(ct))
            {
                MessageBox.Show("Thêm đơn vị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDonVi.Clear();
                txtDonVi.Focus();
                loadDanhSachDonVi(donViBUS.ds);
            }
        }



        private void ResetForm()
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
                    ResetForm();
                    return;
                }

                selectedRowIndex = e.RowIndex;

                DataGridViewRow row = tableDonVi.Rows[e.RowIndex];
                string tenLoai = row.Cells["Tên đơn vị"].Value.ToString();
                txtDonVi.Text = tenLoai;

                btnThemDonVI.Enabled = false;
                btnSuaDonVi.Enabled = true;
                btnXoaDonVi.Enabled = true;
            }
        }

        private void btnSuaDonVi_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tableDonVi.SelectedRows[0];
            int maDonVi = Convert.ToInt32(row.Cells["Mã đơn vị"].Value);
            string tenMoi = txtDonVi.Text.Trim();
            if (string.IsNullOrWhiteSpace(tenMoi))
            {
                MessageBox.Show("Vui lòng nhập tên đơn vị mới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn sửa đơn vị này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
                return;

            donViDTO ct = new donViDTO
            {
                MaDonVi = maDonVi,
                TenDonVi = tenMoi
            };

            donViBUS bus = new donViBUS();
            bool kq = bus.suaDonVi(ct);

            if (kq)
            {
                MessageBox.Show("Sửa đơn vị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bus.docDanhSachDonVi();
                loadDanhSachDonVi(donViBUS.ds);
                ResetForm();
            }
            else
            {
                MessageBox.Show("Lỗi khi sửa loại sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaDonVi_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = tableDonVi.SelectedRows[0];
            int maDonVi = Convert.ToInt32(row.Cells["Mã đơn vị"].Value);
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa đơn vị này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
                return;

            donViBUS bus = new donViBUS();
            bool kq = bus.Xoa(maDonVi);

            if (kq)
            {
                MessageBox.Show("Xóa đơn vị thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bus.docDanhSachDonVi();
                loadDanhSachDonVi(donViBUS.ds);
                ResetForm();
            }
            else
            {
                MessageBox.Show("Lỗi khi xóa đơn vị!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetHeSoForm()
        {
            txtTenNL.Clear();
            cboDonVi.SelectedIndex = -1;
            txtHeSo.Value = 0;
            tableHeSo.ClearSelection();
            btnThemHs.Enabled = true;
            cboDonVi.Enabled = true;
            btnSuaHS.Enabled = false;
            btnXoaHeSO.Enabled = false;
            btnSanPham.Enabled = true;
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

                DataGridViewRow row = tableHeSo.Rows[e.RowIndex];
                string tenDV = row.Cells["Tên đơn vị"].Value.ToString();
                string tenNL = row.Cells["Tên nguyên liệu"].Value.ToString();
                string heSoStr = row.Cells["Hệ số"].Value.ToString();

                txtTenNL.Text = tenNL;
                cboDonVi.SelectedIndex = cboDonVi.FindStringExact(tenDV);

                nguyenLieuBUS nlBUS = new nguyenLieuBUS();
                var dsNL = nlBUS.docDSNguyenLieu();
                var nl = dsNL.FirstOrDefault(x => x.TenNguyenLieu == tenNL);
                if (nl != null)
                {
                    maNL = nl.MaNguyenLieu;  
                }
                else
                {
                    maNL = -1; 
                }

                if (float.TryParse(heSoStr, out float heso))
                {
                    txtHeSo.Value = (decimal)heso;
                }
                else
                {
                    txtHeSo.Value = 0;
                }

                btnThemHs.Enabled = false;
                cboDonVi.Enabled = false;
                btnSanPham.Enabled = false;
                btnSuaHS.Enabled = true;
                btnXoaHeSO.Enabled = true;
            }
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            using(selectNguyenLieu form = new selectNguyenLieu())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if(form.ShowDialog() == DialogResult.OK)
                {
                    maNL = form.MaNL;
                    txtTenNL.Text = form.TenNL;

                    nguyenLieuBUS nlBUS = new nguyenLieuBUS();
                    var dsNL = nlBUS.docDSNguyenLieu();
                    var nl = dsNL.FirstOrDefault(x => x.MaNguyenLieu == maNL);

                    if (nl != null)
                    {
                        cboDonVi.SelectedValue = nl.MaDonViCoSo;
                    }
                    else
                    {
                        cboDonVi.SelectedIndex = -1;
                    }
                }
            }
        }

        private void btnThemHs_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenNL.Text))
            {
                MessageBox.Show("Vui lòng nhập nguyên liệu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(cboDonVi.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn đơn vị!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(txtHeSo.Value == 0)
            {
                MessageBox.Show("Vui lòng nhập hế số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDV = Convert.ToInt32(cboDonVi.SelectedValue);
            decimal heso = (decimal)txtHeSo.Value;
            heSoBUS bus = new heSoBUS();
            heSoDTO ct = new heSoDTO();
            ct.MaNguyenLieu = maNL;
            ct.MaDonVi = maDV;
            ct.HeSo = heso;
            bool kq = bus.Them(ct);
            if(kq)
            {
                MessageBox.Show("Thêm hệ số thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenNL.Clear();
                txtHeSo.Value = 0;
                cboDonVi.SelectedIndex = -1;
                loadDanhSachHeSo(heSoBUS.ds);
            }
        }

        private void btnSuaHS_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenNL.Text))
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cboDonVi.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn đơn vị!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDV = (int)cboDonVi.SelectedValue;
            decimal heso = (decimal)txtHeSo.Value;

            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn sửa hệ số này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
                return;

            heSoDTO ct = new heSoDTO
            {
                MaNguyenLieu = maNL,
                MaDonVi = maDV,
                HeSo = heso
            };

            heSoBUS bus = new heSoBUS();
            bool kq = bus.Sua(ct);

            if (kq)
            {
                MessageBox.Show("Sửa hệ số thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bus.docDanhSachHeSo();
                loadDanhSachHeSo(heSoBUS.ds);
                ResetHeSoForm();
            }
            else
            {
                MessageBox.Show("Lỗi khi sửa hệ số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaHeSO_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenNL.Text))
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cboDonVi.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn đơn vị cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDV = (int)cboDonVi.SelectedValue;
            //MessageBox.Show($"Mã nguyên liệu: {maNL}\nMã đơn vị: {maDV}",
            //    "Kiểm tra dữ liệu trước khi xóa",
            //    MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa hệ số này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
                return;

            heSoBUS bus = new heSoBUS();
            bool kq = bus.Xoa(maNL, maDV);

            if (kq)
            {
                MessageBox.Show("Xóa hệ số thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bus.docDanhSachHeSo();
                loadDanhSachHeSo(heSoBUS.ds);
                ResetHeSoForm();
            }
            else
            {
                MessageBox.Show("Lỗi khi xóa hệ số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
