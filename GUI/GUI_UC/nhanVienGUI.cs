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
    public partial class nhanVienGUI : UserControl
    {
        private nhanVienBUS busNhanVien = new nhanVienBUS();
        private BindingList<taikhoanDTO> dsTK;
        public BindingList<nhanVienDTO> dsNhanVien = new nhanVienBUS().LayDanhSach();
        public nhanVienGUI()
        {
            InitializeComponent();
        }

        private void nhanVienriel_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
            dsTK = new taikhoanBUS().LayDanhSach();

            loadDanhSachNhanVien(dsNhanVien);
            rdoTimCoBan.Checked = true;
            loadChuVoTxtVaCb();
            CheckQuyen();
        }

        private void CheckQuyen()
        {
            var quyenNV = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 4);

            if (quyenNV != null)
            {
                btnThemNV.Enabled = (quyenNV.CAN_CREATE == 1);

                btnSuaNV.Enabled = (quyenNV.CAN_UPDATE) == 1;

                btnXoaNV.Enabled = (quyenNV.CAN_DELETE) == 1;

                btnExcelNV.Enabled = (quyenNV.CAN_CREATE) == 1;

                btnSuaNV.Enabled = false;
                btnXoaNV.Enabled = false;
            }
            else
            {
                btnThemNV.Enabled = false;
                btnSuaNV.Enabled = false;
                btnXoaNV.Enabled = false;
                btnChiTietNV.Enabled = false;
                btnExcelNV.Enabled = false;
            }
        }

        private void loadDanhSachNhanVien(BindingList<nhanVienDTO> ds)
        {
            tbNhanVien.AutoGenerateColumns = false;
            tbNhanVien.DataSource = ds;

            tbNhanVien.Columns.Clear();

            tbNhanVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaNhanVien",
                HeaderText = "Mã NV"
            });
            tbNhanVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "HoTen",
                HeaderText = "Tên NV"
            });
            tbNhanVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoDienThoai",
                HeaderText = "Số điện thoại"
            });
            tbNhanVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Email",
                HeaderText = "Email"
            });
            tbNhanVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Luong",
                HeaderText = "Lương",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });


            btnSuaNV.Enabled = false;
            btnXoaNV.Enabled = false;
            btnChiTietNV.Enabled = false;
            tbNhanVien.ReadOnly = true;
            tbNhanVien.ClearSelection();
            loadFontChuVaSize();
        }

        private void tbNhanVien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= tbNhanVien.Rows.Count) return;
            var column = tbNhanVien.Columns[e.ColumnIndex];
        }

        private void loadFontChuVaSize()
        {
            foreach (DataGridViewColumn col in tbNhanVien.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tbNhanVien.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tbNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tbNhanVien.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tbNhanVien.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(12);

            tbNhanVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbNhanVien.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbNhanVien.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tbNhanVien.Refresh();
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
            SetupComboBoxData(cboTaiKhoan);

            SetComboBoxPlaceholder(cboTimKiemNV, "Chọn giá trị TK");
            SetPlaceholder(txtTimKiemNV, "Nhập giá trị tìm cần tìm");
            SetPlaceholder(txtTenNV, "Nhập tên NV");
            SetPlaceholder(txtSdtNV, "Nhập sđt NV");
            SetPlaceholder(txtEmailNV, "Nhập email NV");
            SetComboBoxPlaceholder(cboTaiKhoan, "Tài khoản");
        }

        private int lastSelectedRowNhanVien = -1;
        private void tbNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var quyenNV = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 4);

            bool isAdmin = (Session.TaiKhoanHienTai.MAVAITRO == 1);

            bool coQuyenThem = isAdmin || (quyenNV != null && quyenNV.CAN_CREATE == 1);
            bool coQuyenSua = isAdmin || (quyenNV != null && quyenNV.CAN_UPDATE == 1);
            bool coQuyenXoa = isAdmin || (quyenNV != null && quyenNV.CAN_DELETE == 1);

            if (e.RowIndex == lastSelectedRowNhanVien)
            {
                tbNhanVien.ClearSelection();
                lastSelectedRowNhanVien = -1;

                btnThemNV.Enabled = coQuyenThem;
                btnSuaNV.Enabled = false;
                btnXoaNV.Enabled = false;
                btnChiTietNV.Enabled = false;
                return;
            }

            tbNhanVien.ClearSelection();
            tbNhanVien.Rows[e.RowIndex].Selected = true;
            lastSelectedRowNhanVien = e.RowIndex;

            btnThemNV.Enabled = coQuyenThem;
            btnSuaNV.Enabled = coQuyenSua;
            btnXoaNV.Enabled = coQuyenXoa;
            btnChiTietNV.Enabled = true;
        }

        private void btnThemNV_Click(object sender, EventArgs e)
        {
            using (addNv form = new addNv(null))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    nhanVienDTO ct = form.nv;
                    busNhanVien.ThemNhanVien(ct);
                    tbNhanVien.Refresh();
                    tbNhanVien.ClearSelection();
                }
            }
        }

        private void btnSuaNV_Click(object sender, EventArgs e)
        {
            if (tbNhanVien.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tbNhanVien.SelectedRows[0];
                nhanVienDTO nv = row.DataBoundItem as nhanVienDTO;

                if (nv != null)
                {
                    using (suaNV form = new suaNV(nv))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            nhanVienDTO kq = form.nv;
                            busNhanVien.CapNhatNhanVien(kq);
                            tbNhanVien.Refresh();
                        }
                    }
                }
            }
        }

        private void btnXoaNV_Click(object sender, EventArgs e)
        {
            if (tbNhanVien.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tbNhanVien.SelectedRows[0];
                nhanVienDTO kh = row.DataBoundItem as nhanVienDTO;

                using (deleteNhaCungCap form = new deleteNhaCungCap())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        int maNV = kh.MaNhanVien;
                        busNhanVien.XoaNhanVien(maNV);
                        btnSuaNV.Enabled = false;
                        btnXoaNV.Enabled = false;
                        btnChiTietNV.Enabled = false;
                    }
                }
            }
        }

        private void btnChiTietNV_Click(object sender, EventArgs e)
        {
            if (tbNhanVien.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tbNhanVien.SelectedRows[0];
                nhanVienDTO nv = row.DataBoundItem as nhanVienDTO;

                using (detailNhanVien form = new detailNhanVien(nv))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
            }
        }

        private void SetupComboBoxData(ComboBox cbo)
        {
            // Tạo danh sách dữ liệu có Giá trị đi kèm
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

        private void resetGiaTri()
        {
            cboTimKiemNV.SelectedIndex = -1;
            cboTaiKhoan.SelectedIndex = -1;
            txtTimKiemNV.Clear();
            txtTenNV.Clear();
            txtTenNV.Clear();
            txtEmailNV.Clear();
            loadFontChuVaSize();
            loadChuVoTxtVaCb();
        }

        private void rdoTimCoBan_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimCoBan.Checked == true)
            {
                cboTimKiemNV.Enabled = true;
                txtTimKiemNV.Enabled = true;

                txtSdtNV.Enabled = false;
                txtTenNV.Enabled = false;
                txtEmailNV.Enabled = false;
                cboTaiKhoan.Enabled = false;
                rdoTimNangCao.Checked = false;
                resetGiaTri();
            }
            else
            {
                cboTimKiemNV.Enabled = false;
                txtTimKiemNV.Enabled = false;

                txtTenNV.Enabled = true;
                txtSdtNV.Enabled = true;
                txtEmailNV.Enabled = true;
                cboTaiKhoan.Enabled = true;
                rdoTimCoBan.Checked = false;
                resetGiaTri();
            }
        }

        private void rdoTimNangCao_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimNangCao.Checked == true)
            {
                txtTenNV.Enabled = true;
                txtSdtNV.Enabled = true;
                txtEmailNV.Enabled = true;
                cboTaiKhoan.Enabled = true;

                cboTimKiemNV.Enabled = false;
                txtTimKiemNV.Enabled = false;

                rdoTimCoBan.Checked = false;
                resetGiaTri();
            }
            else
            {
                txtTenNV.Enabled = false;
                txtSdtNV.Enabled = false;
                txtEmailNV.Enabled = false;
                cboTaiKhoan.Enabled = false;

                cboTimKiemNV.Enabled = true;
                txtTimKiemNV.Enabled = true;

                rdoTimNangCao.Checked = false;
                resetGiaTri();
            }
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

            BindingList<nhanVienDTO> dskq = busNhanVien.timKiemCoban(tim, index);
            if (dskq != null && dskq.Count > 0)
            {
                loadDanhSachNhanVien(dskq);
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
            string tenNCC = string.IsNullOrWhiteSpace(txtTenNV.Text) ? null : txtTenNV.Text.Trim();
            string sdtNCC = string.IsNullOrWhiteSpace(txtSdtNV.Text) ? null : txtSdtNV.Text.Trim();
            string emailNCC = string.IsNullOrWhiteSpace(txtEmailNV.Text) ? null : txtEmailNV.Text.Trim();
            int tk = (cboTaiKhoan.SelectedIndex == -1) ? -1 : Convert.ToInt32(cboTaiKhoan.SelectedValue);
            if (tenNCC == "Nhập tên NV")
            {
                tenNCC = null;
            }
            if (sdtNCC == "Nhập sđt NV")
            {
                sdtNCC = null;
            }
            if (emailNCC == "Nhập email NV")
            {
                emailNCC = null;
            }


            if (tenNCC == null && sdtNCC == null && emailNCC == null && tk == -1)
            {
                MessageBox.Show("Vui lòng nhập ít nhất một điều kiện để tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BindingList<nhanVienDTO> dskq = busNhanVien.timKiemNangCao(tenNCC, sdtNCC, emailNCC, tk);
            if (dskq != null && dskq.Count > 0)
            {
                loadDanhSachNhanVien(dskq);
                loadFontChuVaSize();
            }
            else
            {
                MessageBox.Show("Không tìm thấy giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void tbNhanVien_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tbNhanVien.ClearSelection();
        }

        private void btnReFreshNV_Click(object sender, EventArgs e)
        {
            resetGiaTri();
            BindingList<nhanVienDTO> ds = new nhanVienBUS().LayDanhSach();
            loadDanhSachNhanVien(ds);
            loadChuVoTxtVaCb();
        }

        private void btnExcelNV_Click(object sender, EventArgs e)
        {
            using(selectExcelNhanVien form = new selectExcelNhanVien())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if(form.ShowDialog() == DialogResult.OK)
                {
                    BindingList<nhanVienDTO> ds = new nhanVienBUS().LayDanhSach();
                    loadDanhSachNhanVien(ds);
                }
            }
        }
    }
}
