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
    public partial class insertCongThuc : Form
    {
        private int maSP = -1;
        private int maNL = -1;
        private int indexMaNL = -1;
        private int maDonVi = -1;
        private string tenDonVi = "";
        private List<congThucDTO> dsTam = new List<congThucDTO>();
        public insertCongThuc()
        {
            InitializeComponent();
        }

        private void loadFontChuVaSizeTableCongThuc()
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
            tableCongThuc.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            // --- Fix lỗi mất text khi đổi font ---
            tableCongThuc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableCongThuc.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableCongThuc.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableCongThuc.Refresh();
        }

        private void insertCongThuc_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            btnSuaCT.Enabled = false;
            btnXoaCT.Enabled = false;
        }

        private void resetInput()
        {
            tableCongThuc.ClearSelection();
            txtTenNguyenLieu.Clear();
            txtSoLuong.Value = 0;
            txtDonVi.Clear();
            maNL = -1;
            maDonVi = -1;
            tenDonVi = "";
            lastSelectNguyenLieu = -1;
        }

        private void loadBangCongThuc()
        {
            tableCongThuc.DataSource = null;
            tableCongThuc.DataSource = dsTam.Select(x => new
            {
                MaNL = x.MaNguyenLieu,
                TenNL = x.TenNguyenLieu,
                SoLuong = x.SoLuongCoSo,
                DonVi = x.TenDonViCoSo,
                MaDonVi = x.MaDonViCoSo
            }).ToList();

            tableCongThuc.Columns["MaNL"].HeaderText = "Mã NL";
            tableCongThuc.Columns["TenNL"].HeaderText = "Tên NL";
            tableCongThuc.Columns["SoLuong"].HeaderText = "Số lượng";
            tableCongThuc.Columns["DonVi"].HeaderText = "Đơn vị";
            tableCongThuc.Columns["MaDonVi"].Visible = false;

            loadFontChuVaSizeTableCongThuc();

            tableCongThuc.ClearSelection();
        }

        private void btnChonSP_Click_1(object sender, EventArgs e)
        {
            using (selectSanPham form = new selectSanPham())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    maSP = form.MaSP;
                    txtTenSanPham.Text = form.TenSP;
                    txtTenNguyenLieu.Focus();
                }
            }
        }

        private void btnChonNL_Click_1(object sender, EventArgs e)
        {
            if (maSP == -1)
            {
                MessageBox.Show("Bạn chưa có nhập sản phẩm",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (selectNguyenLieu form = new selectNguyenLieu())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    maNL = form.MaNL;
                    txtTenNguyenLieu.Text = form.TenNL;
                    txtSoLuong.Focus();
                }
            }
        }

        private void btnNhapCT_Click_1(object sender, EventArgs e)
        {
            if (maSP == -1 || maNL == -1 || txtSoLuong.Value <= 0 || maDonVi == -1)
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            congThucDTO tonTai = dsTam.FirstOrDefault(x => x.MaNguyenLieu == maNL && x.MaDonViCoSo == maDonVi);
            if (tonTai != null)
            {
                DialogResult confirm = MessageBox.Show($"Nguyên liệu '{txtTenNguyenLieu.Text}' đã tồn tại.\nCộng dồn số lượng?",
                                                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    tonTai.SoLuongCoSo += (float)txtSoLuong.Value;
                    tonTai.MaDonViCoSo = maDonVi;
                    tonTai.TenDonViCoSo = tenDonVi;
                }
                else return;
            }
            else
            {
                congThucDTO ct = new congThucDTO
                {
                    MaSanPham = maSP,
                    MaNguyenLieu = maNL,
                    TenNguyenLieu = txtTenNguyenLieu.Text,
                    SoLuongCoSo = (float)txtSoLuong.Value,
                    MaDonViCoSo = maDonVi,
                    TenDonViCoSo = tenDonVi,
                    TrangThai = 1
                };
                dsTam.Add(ct);
            }

            loadBangCongThuc();
            resetInput();
            btnChonSP.Enabled = false;
        }

        private void btnXoaCT_Click_1(object sender, EventArgs e)
        {
            if (tableCongThuc.SelectedRows.Count == 0) return;

            int index = tableCongThuc.SelectedRows[0].Index;
            dsTam.RemoveAt(index);

            loadBangCongThuc();
            resetInput();

            if (dsTam.Count == 0)
            {
                btnChonSP.Enabled = true;
                txtTenSanPham.Clear();
            }
        }

        private void btnLuuCT_Click_1(object sender, EventArgs e)
        {
            if (dsTam.Count == 0)
            {
                MessageBox.Show("Chưa có công thức nào để lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            congThucBUS bus = new congThucBUS();
            foreach (congThucDTO ct in dsTam)
                bus.themCongThuc(ct);

            MessageBox.Show("Đã lưu công thức thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dsTam.Clear();
            tableCongThuc.DataSource = null;
            btnChonSP.Enabled = true;
            resetInput();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSuaCT_Click(object sender, EventArgs e)
        {
            if (indexMaNL < 0 || txtSoLuong.Value <= 0 || maDonVi == -1)
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin hợp lệ!",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            congThucDTO ctDangSua = dsTam[indexMaNL];

            // Nếu đổi nguyên liệu sang nguyên liệu khác
            if (ctDangSua.MaNguyenLieu != maNL || ctDangSua.MaDonViCoSo != maDonVi)
            {
                // ✅ Cho phép thêm mới nếu cùng nguyên liệu nhưng khác đơn vị
                congThucDTO tonTai = dsTam.FirstOrDefault(x =>
                    x.MaNguyenLieu == maNL && x.MaDonViCoSo == maDonVi);

                if (tonTai != null)
                {
                    // Nếu trùng cả mã nguyên liệu + mã đơn vị thì hỏi cộng dồn
                    DialogResult confirm = MessageBox.Show(
                        $"Nguyên liệu '{txtTenNguyenLieu.Text}' với đơn vị '{tenDonVi}' đã tồn tại.\n" +
                        $"Bạn có muốn cộng dồn số lượng không?",
                        "Xác nhận trùng nguyên liệu",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirm == DialogResult.Yes)
                    {
                        tonTai.SoLuongCoSo += (float)txtSoLuong.Value;
                        dsTam.RemoveAt(indexMaNL);
                        MessageBox.Show($"Đã cộng dồn vào nguyên liệu '{tonTai.TenNguyenLieu}' ({tenDonVi}).",
                                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else return;
                }
                else
                {
                    // Nếu khác đơn vị hoặc nguyên liệu hoàn toàn → cập nhật như bình thường
                    ctDangSua.MaNguyenLieu = maNL;
                    ctDangSua.TenNguyenLieu = txtTenNguyenLieu.Text;
                    ctDangSua.SoLuongCoSo = (float)txtSoLuong.Value;
                    ctDangSua.MaDonViCoSo = maDonVi;
                    ctDangSua.TenDonViCoSo = tenDonVi;
                }
            }
            else
            {
                // Không đổi nguyên liệu, chỉ chỉnh số lượng/đơn vị
                ctDangSua.SoLuongCoSo = (float)txtSoLuong.Value;
                ctDangSua.MaDonViCoSo = maDonVi;
                ctDangSua.TenDonViCoSo = tenDonVi;
            }

            MessageBox.Show("Đã cập nhật công thức thành công!",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadBangCongThuc();
            resetInput();
            btnNhapCT.Enabled = true;
            btnSuaCT.Enabled = false;
            btnXoaCT.Enabled = false;
            btnChonNL.Enabled = true;
            indexMaNL = -1;
        }

        private int lastSelectNguyenLieu = -1;

        private void tableCongThuc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.RowIndex != lastSelectNguyenLieu)
            {
                DataGridViewRow row = tableCongThuc.Rows[e.RowIndex];

                string tenNL = row.Cells["TenNL"].Value?.ToString() ?? "";
                float soLuong = 0;
                float.TryParse(row.Cells["SoLuong"].Value?.ToString(), out soLuong);

                int.TryParse(row.Cells["MaNL"].Value?.ToString(), out maNL);
                string tenDonVi = row.Cells["DonVi"].Value?.ToString() ?? "";
                int.TryParse(row.Cells["MaDonVi"].Value?.ToString(), out maDonVi);

                indexMaNL = e.RowIndex;

                txtTenNguyenLieu.Text = tenNL;
                txtSoLuong.Value = (decimal)soLuong;
                txtDonVi.Text = tenDonVi;
                this.tenDonVi = tenDonVi;

                lastSelectNguyenLieu = e.RowIndex;

                //btnChonNL.Enabled = false;
                btnNhapCT.Enabled = false;
                btnSuaCT.Enabled = true;
                btnXoaCT.Enabled = true;
                btnChonDonVi.Enabled = true;
            }
            else
            {
                btnNhapCT.Enabled = true;
                btnSuaCT.Enabled = false;
                btnXoaCT.Enabled = false;
                btnChonNL.Enabled = true;
                btnChonDonVi.Enabled = false;
                btnChonDonVi.Enabled = true;

                resetInput();
            }
        }

        private void btnChonDonVi_Click(object sender, EventArgs e)
        {
            if(maNL == -1)
            {
                MessageBox.Show("Bạn chưa có nhập nguyên liệu", "Thông báo", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }else
            {
                using (selectDonVi form = new selectDonVi())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.chiLayHeSo = true;
                    form.maNguyenLieu = maNL;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        maDonVi = form.maDonVi;
                        tenDonVi = form.tenDonVi;
                        txtDonVi.Text = form.tenDonVi;
                        txtDonVi.Focus();
                    }
                }
            }
        }

        private void tableCongThuc_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tableCongThuc.ClearSelection();
        }
    }
}
