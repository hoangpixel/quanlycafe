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
using FONTS;
namespace GUI.GUI_UC
{
    public partial class sanPhamGUI : UserControl
    {
        private int lastSelectedRowSanPham = -1;
        private BindingList<loaiDTO> dsLoai;
        private BindingList<nhomDTO> dsNhom;
        private sanPhamBUS busSanPham = new sanPhamBUS();
        public sanPhamGUI()
        {
            InitializeComponent();
        }

        private void sanPhamGUI_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            hienThiPlaceHolderSanPham();
            FontManager.ApplyFontToAllControls(this);

            loaiSanPhamBUS loaiBus = new loaiSanPhamBUS();
            dsLoai = loaiBus.LayDanhSach();

            nhomBUS nhomBus = new nhomBUS();
            dsNhom = nhomBus.layDanhSach();

            BindingList<sanPhamDTO> ds = busSanPham.LayDanhSach();
            loadDanhSachSanPham(ds);
            loadFontChuVaSize();

            tbSanPham.ClearSelection();
            loadComboBoxLoaiSPTK();
            rdoTimCoBan.Checked = true;

            CheckQuyen();
        }

        private void CheckQuyen()
        {
            var quyenSP = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 1);

            if (quyenSP != null)
            {
                btnThemSP.Enabled = (quyenSP.CAN_CREATE == 1);

                btnSuaSP.Enabled = (quyenSP.CAN_UPDATE) == 1;

                btnXoaSP.Enabled = (quyenSP.CAN_DELETE) == 1;

                btnLoaiSP.Enabled = (quyenSP.CAN_CREATE) == 1;

                btnExcelSP.Enabled = (quyenSP.CAN_CREATE) == 1;

                btnSuaSP.Enabled = false;
                btnXoaSP.Enabled = false;
            }
            else
            {
                btnThemSP.Enabled = false;
                btnSuaSP.Enabled = false;
                btnXoaSP.Enabled = false;
                btnChiTiet.Enabled = false;
                btnLoaiSP.Enabled = false;
                btnExcelSP.Enabled = false;
            }
        }
        private void loadFontChuVaSize()
        {
            foreach (DataGridViewColumn col in tbSanPham.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tbSanPham.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tbSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tbSanPham.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tbSanPham.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(12);

            tbSanPham.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbSanPham.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbSanPham.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tbSanPham.Refresh();
        }


        private void loadDanhSachSanPham(BindingList<sanPhamDTO> ds)
        {
            tbSanPham.AutoGenerateColumns = false;
            tbSanPham.DataSource = ds;
            
            tbSanPham.Columns.Clear();

            tbSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaSP",
                HeaderText = "Mã SP"
            });
            tbSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenSP",
                HeaderText = "Tên SP"
            });
            tbSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaLoai",
                HeaderText = "Tên loại"
            });
            tbSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaLoai",
                HeaderText = "Tên nhóm"
            });
            tbSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TrangThaiCT",
                HeaderText = "Trạng thái CT"
            });
            tbSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Hinh",
                HeaderText = "Hình"
            });
            tbSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Gia",
                HeaderText = "Giá",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });


            btnSuaSP.Enabled = false;
            btnXoaSP.Enabled = false;
            btnChiTiet.Enabled = false;
            tbSanPham.ReadOnly = true;
            tbSanPham.ClearSelection();
        }
        private void tbSanPham_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            sanPhamDTO sp = tbSanPham.Rows[e.RowIndex].DataBoundItem as sanPhamDTO;
            if (sp == null) return;

            if (tbSanPham.Columns[e.ColumnIndex].HeaderText == "Tên loại")
            {
                loaiDTO loai = dsLoai.FirstOrDefault(l => l.MaLoai == sp.MaLoai);
                e.Value = loai?.TenLoai ?? "Không xác định";
            }

            if (tbSanPham.Columns[e.ColumnIndex].HeaderText == "Tên nhóm")
            {
                loaiDTO loai = dsLoai.FirstOrDefault(l => l.MaLoai == sp.MaLoai);
                nhomDTO nhom = dsNhom.FirstOrDefault(n => n.MaNhom == (loai != null ? loai.MaNhom : -1));
                e.Value = nhom?.TenNhom ?? "Không xác định";
            }

            if (tbSanPham.Columns[e.ColumnIndex].HeaderText == "Trạng thái CT")
            {
                e.Value = sp.TrangThaiCT == 1 ? "Đã có công thức" : "Chưa có công thức";
            }
        }
        private void loadComboBoxLoaiSPTK()
        {
            dsLoai = new loaiSanPhamBUS().LayDanhSach();

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
            using (insertSanPham form = new insertSanPham(null))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    sanPhamDTO ct = form.ct;
                    busSanPham.them(ct);
                    tbSanPham.Refresh();
                    tbSanPham.ClearSelection();
                }
            }
        }

        private void btnSuaSP_Click(object sender, EventArgs e)
        {
            if (tbSanPham.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tbSanPham.SelectedRows[0];
                sanPhamDTO sp = row.DataBoundItem as sanPhamDTO;

                if(sp != null)
                {
                    using (updateProduct form = new updateProduct(sp))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        if(form.ShowDialog() == DialogResult.OK)
                        {
                            sanPhamDTO kq = form.sp;
                            busSanPham.Sua(kq);
                            tbSanPham.Refresh();
                        }
                    }
                }
            }
        }

        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            if (tbSanPham.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tbSanPham.SelectedRows[0];
                sanPhamDTO sp = row.DataBoundItem as sanPhamDTO;

                using (deleteProduct form = new deleteProduct())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        int maSP = sp.MaSP;
                        busSanPham.Xoa(maSP);
                        btnSuaSP.Enabled = false;
                        btnXoaSP.Enabled = false;
                        btnChiTiet.Enabled = false;
                    }
                }
            }
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            if (tbSanPham.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = tbSanPham.SelectedRows[0];
                sanPhamDTO sp = selectedRow.DataBoundItem as sanPhamDTO;

                if (sp != null)
                {
                    using (detailSanPham form = new detailSanPham(sp))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        form.ShowDialog();
                    }
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

            var quyenSP = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 1);

            bool isAdmin = (Session.TaiKhoanHienTai.MAVAITRO == 1);

            bool coQuyenThem = isAdmin || (quyenSP != null && quyenSP.CAN_CREATE == 1);
            bool coQuyenSua = isAdmin || (quyenSP != null && quyenSP.CAN_UPDATE == 1);
            bool coQuyenXoa = isAdmin || (quyenSP != null && quyenSP.CAN_DELETE == 1);

            if (e.RowIndex == lastSelectedRowSanPham)
            {
                tbSanPham.ClearSelection();
                lastSelectedRowSanPham = -1;

                btnThemSP.Enabled = coQuyenThem;
                btnSuaSP.Enabled = false;
                btnXoaSP.Enabled = false;
                btnChiTiet.Enabled = false;
                return;
            }

            tbSanPham.ClearSelection();
            tbSanPham.Rows[e.RowIndex].Selected = true;
            lastSelectedRowSanPham = e.RowIndex;

            btnThemSP.Enabled = coQuyenThem;
            btnSuaSP.Enabled = coQuyenSua;
            btnXoaSP.Enabled = coQuyenXoa;
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
            dsLoai = new loaiSanPhamBUS().LayDanhSach();
            dsNhom = new nhomBUS().layDanhSach();
            tbSanPham.Refresh();
        }

        private void btnExcelSP_Click(object sender, EventArgs e)
        {
            using (selectExcelSanPham form = new selectExcelSanPham())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
            busSanPham.LayDanhSach();
            loadDanhSachSanPham(sanPhamBUS.ds);
            loadFontChuVaSize();
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

            BindingList<sanPhamDTO> kq = busSanPham.timKiemCoBan(tim, index);

            if (kq != null && kq.Count > 0)
            {
                tbSanPham.Columns.Clear();
                tbSanPham.DataSource = null;
                loadDanhSachSanPham(kq);
                loadFontChuVaSize();
            }
            else
            {
                MessageBox.Show("Không có kết quả tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
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
                MessageBox.Show("Giá tối thiểu phải bé hơn hoặc bằng giá tối đa.", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (maLoai == -1 && trangThaiCT == -1 && tenSP == null && giaMin == -1 && giaMax == -1)
            {
                MessageBox.Show("Vui lòng nhập ít nhất một điều kiện để tìm kiếm",
                    "Thông báo rỗng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            BindingList<sanPhamDTO> dskq = busSanPham.timKiemNangCaoSP(maLoai, trangThaiCT, giaMin, giaMax, tenSP);

            if (dskq != null && dskq.Count > 0)
            {
                loadDanhSachSanPham(dskq);
                hienThiPlaceHolderSanPham();
                loadFontChuVaSize();
            }
            else
            {
                MessageBox.Show("Không có kết quả tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
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
            busSanPham.LayDanhSach();
            loadDanhSachSanPham(sanPhamBUS.ds);
            loadFontChuVaSize();

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

        private void resetInput()
        {
            cboTimKiemSP.SelectedIndex = -1;
            txtTimKiemSP.Clear();
            txtTenSPTK.Clear();
            txtGiaMin.Clear();
            txtGiaMax.Clear();
            cboTrangThai.SelectedIndex = -1;
            hienThiPlaceHolderSanPham();
        }
        private void rdoTimNangCao_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimNangCao.Checked)
            {
                rdoTimCoBan.Checked = false;
                txtTimKiemSP.Enabled = false;
                cboTimKiemSP.Enabled = false;
                resetInput();
            }
            else
            {
                txtTimKiemSP.Enabled = true;
                cboTimKiemSP.Enabled = true;
                resetInput();
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
                resetInput();
            }
            else
            {
                txtGiaMin.Enabled = true;
                txtGiaMax.Enabled = true;
                txtTenSPTK.Enabled = true;
                cboTrangThai.Enabled = true;
                cboLoaiSP.Enabled = true;
                resetInput();
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
