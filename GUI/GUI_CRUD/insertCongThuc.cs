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
        public BindingList<congThucDTO> dsTam = new BindingList<congThucDTO>();
        private BindingList<donViDTO> dsDonVi;
        private BindingList<nguyenLieuDTO> dsNguyenLieu;
        private congThucBUS busCT = new congThucBUS();

        private congThucBUS busCongThuc = new congThucBUS();
        public insertCongThuc()
        {
            InitializeComponent();
        }

        private void loadFontChuVaSizeTableCongThuc()
        {
            foreach (DataGridViewColumn col in tableCongThuc.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            tableCongThuc.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableCongThuc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tableCongThuc.DefaultCellStyle.Font = FontManager.GetLightFont(10);
            tableCongThuc.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);
            tableCongThuc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableCongThuc.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableCongThuc.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            tableCongThuc.Refresh();
        }

        private void insertCongThuc_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            dsDonVi = new donViBUS().LayDanhSach();
            dsNguyenLieu = new nguyenLieuBUS().LayDanhSach();

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
            tableCongThuc.AutoGenerateColumns = false;
            tableCongThuc.Columns.Clear();

            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Mã NL"});
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Tên NL" });
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuongCoSo", HeaderText = "Số lượng" });
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonViCoSo", HeaderText = "Đơn vị" });

            tableCongThuc.DataSource = dsTam;

            btnSuaCT.Enabled = false;
            btnXoaCT.Enabled = false;
            tableCongThuc.ClearSelection();
            loadFontChuVaSizeTableCongThuc();
        }

        private void tableCongThuc_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            congThucDTO ct = tableCongThuc.Rows[e.RowIndex].DataBoundItem as congThucDTO;
            if (tableCongThuc.Columns[e.ColumnIndex].HeaderText == "Đơn vị")
            {
                donViDTO dv = dsDonVi.FirstOrDefault(x => x.MaDonVi == ct.MaDonViCoSo);
                e.Value = dv?.TenDonVi ?? "Không xác định";
            }
            if (tableCongThuc.Columns[e.ColumnIndex].HeaderText == "Tên NL")
            {
                nguyenLieuDTO nl = dsNguyenLieu.FirstOrDefault(x => x.MaNguyenLieu == ct.MaNguyenLieu);
                e.Value = nl?.TenNguyenLieu ?? " Không xác định";
            }
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
            if(busCT.kiemTraChuoiRong(txtTenSanPham.Text))
            {
                MessageBox.Show("Không được để trống tên sản phẩm", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSanPham.Focus();
                return;
            }
            if (busCT.kiemTraChuoiRong(txtTenNguyenLieu.Text))
            {
                MessageBox.Show("Không được để trống tên nguyên liệu", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNguyenLieu.Focus();
                return;
            }
            if (busCT.kiemTraChuoiRong(txtDonVi.Text))
            {
                MessageBox.Show("Không được để trống tên đơn vị", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonVi.Focus();
                return;
            }
            if(txtSoLuong.Value == 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }
            congThucDTO tonTai = dsTam.FirstOrDefault(x => x.MaNguyenLieu == maNL && x.MaDonViCoSo == maDonVi);
            if (tonTai != null)
            {
                DialogResult confirm = MessageBox.Show($"Nguyên liệu '{txtTenNguyenLieu.Text}' đã tồn tại.\nCộng dồn số lượng?",
                                                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    tonTai.SoLuongCoSo += txtSoLuong.Value;
                    tonTai.MaDonViCoSo = maDonVi;
                    tonTai.TenDonViCoSo = tenDonVi;
                }
                else return;
            }
            else
            {
                congThucDTO ct = new congThucDTO();
                ct.MaSanPham = maSP;
                ct.MaNguyenLieu = maNL;
                ct.SoLuongCoSo = txtSoLuong.Value;
                ct.MaDonViCoSo = maDonVi;
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

            resetInput();

            if (dsTam.Count == 0)
            {
                btnChonSP.Enabled = true;
                txtTenSanPham.Clear();
                btnSuaCT.Enabled = false;
                btnXoaCT.Enabled = false;
                btnNhapCT.Enabled = true;
            }
        }

        private void btnLuuCT_Click_1(object sender, EventArgs e)
        {
            if (dsTam.Count == 0)
            {
                MessageBox.Show("Chưa có công thức nào để lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Đã lưu công thức thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
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
                // Cho phép thêm mới nếu cùng nguyên liệu nhưng khác đơn vị
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
                        tonTai.SoLuongCoSo += txtSoLuong.Value;
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
                    ctDangSua.SoLuongCoSo = txtSoLuong.Value;
                    ctDangSua.MaDonViCoSo = maDonVi;
                    ctDangSua.TenDonViCoSo = tenDonVi;
                }
            }
            else
            {
                // Không đổi nguyên liệu, chỉ chỉnh số lượng/đơn vị
                ctDangSua.SoLuongCoSo = txtSoLuong.Value;
                ctDangSua.MaDonViCoSo = maDonVi;
                ctDangSua.TenDonViCoSo = tenDonVi;
            }

            MessageBox.Show("Đã cập nhật công thức thành công!",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

            if (e.RowIndex == lastSelectNguyenLieu)
            {
                btnNhapCT.Enabled = true;
                btnSuaCT.Enabled = false;
                btnXoaCT.Enabled = false;
                btnChonNL.Enabled = true;
                resetInput();
                return;
            }

            DataGridViewRow row = tableCongThuc.Rows[e.RowIndex];
            congThucDTO ct = row.DataBoundItem as congThucDTO;
            this.maNL = ct.MaNguyenLieu;
            this.maDonVi = ct.MaDonViCoSo;

            nguyenLieuDTO nl = dsNguyenLieu.FirstOrDefault(x => x.MaNguyenLieu == ct.MaNguyenLieu);
            donViDTO dv = dsDonVi.FirstOrDefault(x => x.MaDonVi == ct.MaDonViCoSo);

            indexMaNL = e.RowIndex;

            txtTenNguyenLieu.Text = nl?.TenNguyenLieu ?? "Không xác định";
            txtSoLuong.Value = (decimal)ct.SoLuongCoSo;
            txtDonVi.Text = dv?.TenDonVi ?? "Không xác định";
            this.tenDonVi = dv?.TenDonVi ?? "Không xác định";

            lastSelectNguyenLieu = e.RowIndex;

            btnNhapCT.Enabled = false;
            btnSuaCT.Enabled = true;
            btnXoaCT.Enabled = true;
            btnChonDonVi.Enabled = true;
            btnChonNL.Enabled = false;
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
