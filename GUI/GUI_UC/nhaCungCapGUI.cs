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
    public partial class nhaCungCapGUI : UserControl
    {
        private nhaCungCapBUS busNhaCungCap = new nhaCungCapBUS();
        public nhaCungCapGUI()
        {
            InitializeComponent();
        }
        private void nhaCungCapGUI_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            BindingList<nhaCungCapDTO> ds = new nhaCungCapBUS().LayDanhSach();
            loadDanhSachNhaCungCap(ds);
            loadFontChuVaSize();
            loadChuVoTxtVaCb();
            rdoTimCoBan.Checked = true;
            CheckQuyen();
        }

        private void CheckQuyen()
        {
            var quyenNCC = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 6);

            if (quyenNCC != null)
            {
                btnThemNCC.Enabled = (quyenNCC.CAN_CREATE == 1);

                btnSuaNCC.Enabled = (quyenNCC.CAN_UPDATE) == 1;

                btnXoaNCC.Enabled = (quyenNCC.CAN_DELETE) == 1;

                btnExcelNCC.Enabled = (quyenNCC.CAN_CREATE) == 1;

                btnSuaNCC.Enabled = false;
                btnXoaNCC.Enabled = false;
            }
            else
            {
                btnThemNCC.Enabled = false;
                btnSuaNCC.Enabled = false;
                btnXoaNCC.Enabled = false;
                btnChiTietNCC.Enabled = false;
                btnExcelNCC.Enabled = false;
            }
        }

        private void loadDanhSachNhaCungCap(BindingList<nhaCungCapDTO> ds)
        {
            tbNhaCungCap.AutoGenerateColumns = false;
            tbNhaCungCap.DataSource = ds;

            tbNhaCungCap.Columns.Clear();

            tbNhaCungCap.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaNCC",
                HeaderText = "Mã NCC"
            });
            tbNhaCungCap.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenNCC",
                HeaderText = "Tên NCC"
            });
            tbNhaCungCap.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoDienThoai",
                HeaderText = "Số điện thoại"
            });
            tbNhaCungCap.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Email",
                HeaderText = "Email"
            });
            tbNhaCungCap.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DiaChi",
                HeaderText = "Địa chỉ"
            });


            btnSuaNCC.Enabled = false;
            btnXoaNCC.Enabled = false;
            btnChiTietNCC.Enabled = false;
            tbNhaCungCap.ReadOnly = true;
            tbNhaCungCap.ClearSelection();
        }

        private void loadFontChuVaSize()
        {
            foreach (DataGridViewColumn col in tbNhaCungCap.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tbNhaCungCap.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tbNhaCungCap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tbNhaCungCap.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tbNhaCungCap.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(12);

            tbNhaCungCap.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbNhaCungCap.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbNhaCungCap.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tbNhaCungCap.Refresh();
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
            SetComboBoxPlaceholder(cboTimKiemNCC, "Chọn giá trị TK");
            SetPlaceholder(txtTimKiemNCC, "Nhập giá trị tìm cần tìm");
            SetPlaceholder(txtTenNCC, "Nhập tên NCC");
            SetPlaceholder(txtSdtNCC, "Nhập sđt NCC");
            SetPlaceholder(txtEmailNCC, "Nhập email NCC");
            SetPlaceholder(txtDiaChi, "Nhập địa chỉ");
        }

        private void btnThemNCC_Click(object sender, EventArgs e)
        {
            using(insertNhaCungCap form = new insertNhaCungCap(null))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    nhaCungCapDTO ct = form.kq;
                    busNhaCungCap.them(ct);
                    tbNhaCungCap.Refresh();
                    tbNhaCungCap.ClearSelection();
                }
            }
        }

        private void btnSuaNCC_Click(object sender, EventArgs e)
        {
            if (tbNhaCungCap.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tbNhaCungCap.SelectedRows[0];
                nhaCungCapDTO ncc = row.DataBoundItem as nhaCungCapDTO;

                if (ncc != null)
                {
                    using (updateNhaCungCap form = new updateNhaCungCap(ncc))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            nhaCungCapDTO kq = form.nccHienTai;
                            busNhaCungCap.sua(kq);
                            tbNhaCungCap.Refresh();
                        }
                    }
                }
            }
        }

        private void btnXoaNCC_Click(object sender, EventArgs e)
        {
            if (tbNhaCungCap.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tbNhaCungCap.SelectedRows[0];
                nhaCungCapDTO kh = row.DataBoundItem as nhaCungCapDTO;

                using (deleteNhaCungCap form = new deleteNhaCungCap())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        int maNCC = kh.MaNCC;
                        busNhaCungCap.xoa(maNCC);
                        btnSuaNCC.Enabled = false;
                        btnXoaNCC.Enabled = false;
                        btnChiTietNCC.Enabled = false;
                    }
                }
            }
        }

        private void tbNhaCungCap_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tbNhaCungCap.ClearSelection();
        }

        private int lastSelectedRowNhaCungCap = -1;

        private void tbNhaCungCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;


            var quyenNCC = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 6);

            bool isAdmin = (Session.TaiKhoanHienTai.MAVAITRO == 1);

            bool coQuyenThem = isAdmin || (quyenNCC != null && quyenNCC.CAN_CREATE == 1);
            bool coQuyenSua = isAdmin || (quyenNCC != null && quyenNCC.CAN_UPDATE == 1);
            bool coQuyenXoa = isAdmin || (quyenNCC != null && quyenNCC.CAN_DELETE == 1);

            if (e.RowIndex == lastSelectedRowNhaCungCap)
            {
                tbNhaCungCap.ClearSelection();
                lastSelectedRowNhaCungCap = -1;

                btnThemNCC.Enabled = coQuyenThem;
                btnSuaNCC.Enabled = false;
                btnXoaNCC.Enabled = false;
                btnChiTietNCC.Enabled = false;
                return;
            }

            tbNhaCungCap.ClearSelection();
            tbNhaCungCap.Rows[e.RowIndex].Selected = true;
            lastSelectedRowNhaCungCap = e.RowIndex;

            btnThemNCC.Enabled = coQuyenThem;
            btnSuaNCC.Enabled = coQuyenSua;
            btnXoaNCC.Enabled = coQuyenXoa;
            btnChiTietNCC.Enabled = true;
        }

        private void btnChiTietNCC_Click(object sender, EventArgs e)
        {
            if(tbNhaCungCap.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tbNhaCungCap.SelectedRows[0];
                nhaCungCapDTO ct = row.DataBoundItem as nhaCungCapDTO;

                using(detailNhaCungCap form = new detailNhaCungCap(ct))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
            }
        }

        private void resetGiaTri()
        {
            cboTimKiemNCC.SelectedIndex = -1;
            txtTimKiemNCC.Clear();
            txtTenNCC.Clear();
            txtTenNCC.Clear();
            txtEmailNCC.Clear();
            loadChuVoTxtVaCb();
        }

        private void rdoTimCoBan_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimCoBan.Checked == true)
            {
                cboTimKiemNCC.Enabled = true;
                txtTimKiemNCC.Enabled = true;

                txtTenNCC.Enabled = false;
                txtSdtNCC.Enabled = false;
                txtEmailNCC.Enabled = false;
                txtDiaChi.Enabled = false;
                rdoTimNangCao.Checked = false;
                resetGiaTri();
            }
            else
            {
                cboTimKiemNCC.Enabled = false;
                txtTimKiemNCC.Enabled = false;

                txtTenNCC.Enabled = true;
                txtSdtNCC.Enabled = true;
                txtEmailNCC.Enabled = true;
                txtDiaChi.Enabled = true;
                rdoTimCoBan.Checked = false;
                resetGiaTri();
            }
        }

        private void rdoTimNangCao_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimNangCao.Checked == true)
            {
                txtTenNCC.Enabled = true;
                txtSdtNCC.Enabled = true;
                txtEmailNCC.Enabled = true;
                txtDiaChi.Enabled = true;

                cboTimKiemNCC.Enabled = false;
                txtTimKiemNCC.Enabled = false;

                rdoTimCoBan.Checked = false;
                resetGiaTri();
            }
            else
            {
                txtTenNCC.Enabled = false;
                txtSdtNCC.Enabled = false;
                txtEmailNCC.Enabled = false;
                txtDiaChi.Enabled = false;

                cboTimKiemNCC.Enabled = true;
                txtTimKiemNCC.Enabled = true;

                rdoTimNangCao.Checked = false;
                resetGiaTri();
            }
        }

        private void btnThucHienTimKiem_Click(object sender, EventArgs e)
        {
            if(rdoTimCoBan.Checked == true)
            {
                timKiemCoBan();
            }else
            {
                timKiemNangCao();
            }
        }

        private void timKiemCoBan()
        {
            if (cboTimKiemNCC.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboTimKiemNCC.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTimKiemNCC.Text))
            {
                MessageBox.Show("Vui lòng Nhập giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimKiemNCC.Focus();
                return;
            }

            string tim = txtTimKiemNCC.Text.Trim();
            int index = cboTimKiemNCC.SelectedIndex;

            BindingList<nhaCungCapDTO> dskq = busNhaCungCap.timKiemCoban(tim, index);
            if (dskq != null && dskq.Count > 0)
            {
                loadDanhSachNhaCungCap(dskq);
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
            string tenNCC = string.IsNullOrWhiteSpace(txtTenNCC.Text) ? null : txtTenNCC.Text.Trim();
            string sdtNCC = string.IsNullOrWhiteSpace(txtSdtNCC.Text) ? null : txtSdtNCC.Text.Trim();
            string emailNCC = string.IsNullOrWhiteSpace(txtEmailNCC.Text) ? null : txtEmailNCC.Text.Trim();
            string diachiNCC = string.IsNullOrWhiteSpace(txtDiaChi.Text) ? null : txtDiaChi.Text.Trim();
            if (tenNCC == "Nhập tên NCC")
            {
                tenNCC = null;
            }
            if (sdtNCC == "Nhập sđt NCC")
            {
                sdtNCC = null;
            }
            if (emailNCC == "Nhập email NCC")
            {
                emailNCC = null;
            }
            if (diachiNCC == "Nhập địa chỉ")
            {
                diachiNCC = null;
            }

            if (tenNCC == null && sdtNCC == null && emailNCC == null && diachiNCC == null)
            {
                MessageBox.Show("Vui lòng nhập ít nhất một điều kiện để tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BindingList<nhaCungCapDTO> dskq = busNhaCungCap.timKiemNangCao(tenNCC, sdtNCC, emailNCC,diachiNCC);
            if (dskq != null && dskq.Count > 0)
            {
                loadDanhSachNhaCungCap(dskq);
                loadFontChuVaSize();
            }
            else
            {
                MessageBox.Show("Không tìm thấy giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnReFreshNCC_Click(object sender, EventArgs e)
        {
            BindingList<nhaCungCapDTO> dskh = new nhaCungCapBUS().LayDanhSach();
            loadDanhSachNhaCungCap(dskh);
            loadFontChuVaSize();
            resetGiaTri();
        }

        private void btnExcelNCC_Click(object sender, EventArgs e)
        {
            using(selectExcelNhaCungCap form = new selectExcelNhaCungCap())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
        }
    }
}
