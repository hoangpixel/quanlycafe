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
    public partial class taiKhoanGUI : UserControl
    {
        private taikhoanBUS busTaiKhoan = new taikhoanBUS();
        private BindingList<nhanVienDTO> dsNV;
        private BindingList<vaitroDTO> dsVT;
        public taiKhoanGUI()
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void taiKhoanRiel_Load(object sender, EventArgs e)
        {
            dsNV = new nhanVienBUS().LayDanhSach();
            dsVT = new vaitroBUS().LayDanhSach();

            BindingList<taikhoanDTO> ds = busTaiKhoan.LayDanhSach();
            loadDanhSachTaiKhoan(ds);
            loadChuVoTxtVaCb();
            CheckQuyen();
            rdoTimCoBan.Checked = true;
        }

        private void CheckQuyen()
        {
            var quyenTK = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 5);

            if (quyenTK != null)
            {
                btnThemTK.Enabled = (quyenTK.CAN_CREATE == 1);

                btnSuaTK.Enabled = (quyenTK.CAN_UPDATE) == 1;

                btnXoaTK.Enabled = (quyenTK.CAN_DELETE) == 1;

                btnExcelTK.Enabled = (quyenTK.CAN_CREATE) == 1;

                btnSuaTK.Enabled = false;
                btnXoaTK.Enabled = false;
            }
            else
            {
                btnThemTK.Enabled = false;
                btnSuaTK.Enabled = false;
                btnXoaTK.Enabled = false;
                btnChiTietTK.Enabled = false;
                btnExcelTK.Enabled = false;
            }
        }
        private void loadDanhSachTaiKhoan(BindingList<taikhoanDTO> ds)
        {
            bool isAdmin = (Session.TaiKhoanHienTai.MAVAITRO == 1);
            tbTaiKhoan.AutoGenerateColumns = false;

            tbTaiKhoan.Columns.Clear();

            tbTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MATAIKHOAN",
                HeaderText = "Mã TK"
            });
            tbTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MAVAITRO",
                HeaderText = "Vai trò"
            });
            tbTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MANHANVIEN",
                HeaderText = "Tên nhân viên"
            });

            if(isAdmin)
            {
                tbTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "TENDANGNHAP",
                    HeaderText = "Tên đăng nhập"
                });
            }

            if(isAdmin)
            {
                tbTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "MATKHAU",
                    HeaderText = "Mật khẩu"
                });
            }


            tbTaiKhoan.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NGAYTAO",
                HeaderText = "Ngày tạo"
            });

            tbTaiKhoan.DataSource = ds;

            btnSuaTK.Enabled = false;
            btnXoaTK.Enabled = false;
            btnChiTietTK.Enabled = false;
            tbTaiKhoan.ReadOnly = true;
            tbTaiKhoan.ClearSelection();
            loadFontChuVaSize();
        }

        private void tbTaiKhoan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            taikhoanDTO tk = tbTaiKhoan.Rows[e.RowIndex].DataBoundItem as taikhoanDTO;
            if (tbTaiKhoan.Columns[e.ColumnIndex].HeaderText == "Tên nhân viên")
            {
                nhanVienDTO nv = dsNV.FirstOrDefault(x => x.MaNhanVien == tk.MANHANVIEN);
                e.Value = nv?.HoTen ?? "Không xác định";
            }
            if (tbTaiKhoan.Columns[e.ColumnIndex].HeaderText == "Vai trò")
            {
                vaitroDTO vt = dsVT.FirstOrDefault(x => x.MaVaiTro == tk.MAVAITRO);
                e.Value = vt?.TenVaiTro ?? "Không xác định";
            }
        }

        private void loadFontChuVaSize()
        {
            foreach (DataGridViewColumn col in tbTaiKhoan.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tbTaiKhoan.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tbTaiKhoan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tbTaiKhoan.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tbTaiKhoan.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(12);

            tbTaiKhoan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbTaiKhoan.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbTaiKhoan.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tbTaiKhoan.Refresh();
        }

        private void tbTaiKhoan_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tbTaiKhoan.ClearSelection();
        }

        private void btnThemTK_Click(object sender, EventArgs e)
        {
            using (insertTaiKhoan form = new insertTaiKhoan(null))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    taikhoanDTO ct = form.tk;
                    busTaiKhoan.Them(ct);
                    tbTaiKhoan.ClearSelection();
                    MessageBox.Show("Thêm tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
            }
        }

        private void btnSuaTK_Click(object sender, EventArgs e)
        {
            if (tbTaiKhoan.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tbTaiKhoan.SelectedRows[0];
                taikhoanDTO tk = row.DataBoundItem as taikhoanDTO;

                if (tk != null)
                {
                    using (updateTaiKhoan form = new updateTaiKhoan(tk))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            taikhoanDTO kq = form.tk;
                            bool coDoiMatKhau = form.coNhapMK;
                            busTaiKhoan.Sua(kq, coDoiMatKhau);
                            tbTaiKhoan.Refresh();
                        }
                    }
                }
            }
        }

        private void btnXoaTK_Click(object sender, EventArgs e)
        {
            if (tbTaiKhoan.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tbTaiKhoan.SelectedRows[0];
                taikhoanDTO tk = row.DataBoundItem as taikhoanDTO;

                using (xoaTk form = new xoaTk())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        int maTK = tk.MATAIKHOAN;
                        busTaiKhoan.Xoa(maTK);
                        btnSuaTK.Enabled = false;
                        btnXoaTK.Enabled = false;
                        btnChiTietTK.Enabled = false;
                    }
                }
            }
        }

        private void btnChiTietTK_Click(object sender, EventArgs e)
        {
            if (tbTaiKhoan.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tbTaiKhoan.SelectedRows[0];
                taikhoanDTO ct = row.DataBoundItem as taikhoanDTO;

                using (detailTaiKhoan form = new detailTaiKhoan(ct))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
            }
        }

        private int lastSelectedRowTaiKhoan = -1;
        private void tbTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var quyenTK = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 5);

            bool isAdmin = (Session.TaiKhoanHienTai.MAVAITRO == 1);

            bool coQuyenThem = isAdmin || (quyenTK != null && quyenTK.CAN_CREATE == 1);
            bool coQuyenSua = isAdmin || (quyenTK != null && quyenTK.CAN_UPDATE == 1);
            bool coQuyenXoa = isAdmin || (quyenTK != null && quyenTK.CAN_DELETE == 1);

            if (e.RowIndex == lastSelectedRowTaiKhoan)
            {
                tbTaiKhoan.ClearSelection();
                lastSelectedRowTaiKhoan = -1;

                btnThemTK.Enabled = coQuyenThem;
                btnSuaTK.Enabled = false;
                btnXoaTK.Enabled = false;
                btnChiTietTK.Enabled = false;
                return;
            }

            tbTaiKhoan.ClearSelection();
            tbTaiKhoan.Rows[e.RowIndex].Selected = true;
            lastSelectedRowTaiKhoan = e.RowIndex;

            btnThemTK.Enabled = coQuyenThem;
            btnSuaTK.Enabled = coQuyenSua;
            btnXoaTK.Enabled = coQuyenXoa;
            btnChiTietTK.Enabled = true;
        }

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

        private void loadChuVoTxtVaCb()
        {
            SetComboBoxPlaceholder(cboTimKiemNV, "Chọn giá trị TK");
            SetPlaceholder(txtTimKiemNV, "Nhập giá trị tìm cần tìm");
            SetPlaceholder(txtTenNV, "Nhập tên NV");
            SetPlaceholder(txtTenTk, "Nhập tên TK");
            SetPlaceholder(txtTenVT, "Nhập tên VT");
        }

        private void resetGiaTri()
        {
            cboTimKiemNV.SelectedIndex = -1;
            txtTimKiemNV.Clear();
            txtTenNV.Clear();
            txtTenTk.Clear();
            txtTenVT.Clear();
            loadChuVoTxtVaCb();
        }

        private void rdoTimCoBan_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimCoBan.Checked == true)
            {
                cboTimKiemNV.Enabled = true;
                txtTimKiemNV.Enabled = true;

                txtTenVT.Enabled = false;
                txtTenTk.Enabled = false;
                txtTenNV.Enabled = false;
                rdoTimNangCao.Checked = false;
                resetGiaTri();
            }
            else
            {
                cboTimKiemNV.Enabled = false;
                txtTimKiemNV.Enabled = false;

                txtTenVT.Enabled = true;
                txtTenTk.Enabled = true;
                txtTenNV.Enabled = true;
                rdoTimCoBan.Checked = false;
                resetGiaTri();
            }
        }

        private void rdoTimNangCao_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimNangCao.Checked == true)
            {
                txtTenVT.Enabled = true;
                txtTenTk.Enabled = true;
                txtTenNV.Enabled = true;
                cboTimKiemNV.Enabled = false;
                txtTimKiemNV.Enabled = false;

                rdoTimCoBan.Checked = false;
                resetGiaTri();
            }
            else
            {
                txtTenVT.Enabled = false;
                txtTenTk.Enabled = false;
                txtTenNV.Enabled = false;

                cboTimKiemNV.Enabled = true;
                txtTimKiemNV.Enabled = true;

                rdoTimNangCao.Checked = false;
                resetGiaTri();
            }
        }

        private void btnReFreshTK_Click(object sender, EventArgs e)
        {
            BindingList<taikhoanDTO> dskh = busTaiKhoan.LayDanhSach();
            loadDanhSachTaiKhoan(dskh);
            loadFontChuVaSize();
            resetGiaTri();
        }

        private void btnThucHienTimKiem_Click(object sender, EventArgs e)
        {
            if (rdoTimCoBan.Checked == true)
            {
                timKiemCoBan();
            }
            else
            {
                timKiemNangCao();
            }
        }

        private void timKiemCoBan()
        {
            if (cboTimKiemNV.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboTimKiemNV.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTimKiemNV.Text))
            {
                MessageBox.Show("Vui lòng Nhập giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimKiemNV.Focus();
                return;
            }

            string tim = txtTimKiemNV.Text.Trim();
            int index = cboTimKiemNV.SelectedIndex;

            BindingList<taikhoanDTO> dskq = busTaiKhoan.timKiemCoban(tim, index);
            if (dskq != null && dskq.Count > 0)
            {
                loadDanhSachTaiKhoan(dskq);
                loadFontChuVaSize();
            }
            else
            {
                MessageBox.Show("Không tìm thấy giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void timKiemNangCao()
        {
            string tenNV = string.IsNullOrWhiteSpace(txtTenNV.Text) ? null : txtTenNV.Text.Trim();
            string tenVT = string.IsNullOrWhiteSpace(txtTenVT.Text) ? null : txtTenVT.Text.Trim();
            string tenTK = string.IsNullOrWhiteSpace(txtTenTk.Text) ? null : txtTenTk.Text.Trim();

            if (tenNV == "Nhập tên NV")
            {
                tenNV = null;
            }
            if (tenVT == "Nhập tên VT")
            {
                tenVT = null;
            }
            if (tenTK == "Nhập tên TK")
            {
                tenTK = null;
            }


            if (tenNV == null && tenVT == null && tenTK == null)
            {
                MessageBox.Show("Vui lòng nhập ít nhất một điều kiện để tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BindingList<taikhoanDTO> dskq = busTaiKhoan.timKiemNangCao(tenNV, tenVT, tenTK);
            if (dskq != null && dskq.Count > 0)
            {
                loadDanhSachTaiKhoan(dskq);
                loadFontChuVaSize();
            }
            else
            {
                MessageBox.Show("Không tìm thấy giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnExcelTK_Click(object sender, EventArgs e)
        {
            using (selectExcelTaiKhoan form = new selectExcelTaiKhoan())
            {
                form.StartPosition = FormStartPosition.CenterParent;

                // Khi form nhập liệu đóng lại và trả về OK (tức là đã nhập xong)
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // 🔥 BƯỚC QUAN TRỌNG: Xóa danh sách cũ đi để ép BUS tải lại
                    busTaiKhoan.ds.Clear();

                    // Lúc này ds trống -> LayDanhSach() sẽ gọi xuống Database lấy dữ liệu mới nhất
                    BindingList<taikhoanDTO> ds = busTaiKhoan.LayDanhSach();

                    // Hiển thị lên GridView
                    loadDanhSachTaiKhoan(ds);
                }
            }
        }
    }
}
