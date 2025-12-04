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
    public partial class nhanVienriel : UserControl
    {
        private nhanVienBUS busNhanVien = new nhanVienBUS();
        public nhanVienriel()
        {
            InitializeComponent();
        }

        private void nhanVienriel_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            BindingList<nhanVienDTO> ds = new nhanVienBUS().LayDanhSach();
            loadDanhSachNhanVien(ds);
            rdoTimCoBan.Checked = true;
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
                DataPropertyName = "",
                HeaderText = "Tài khoản"
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

        private int lastSelectedRowNhanVien = -1;
        private void tbNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.RowIndex == lastSelectedRowNhanVien)
            {
                tbNhanVien.ClearSelection();
                lastSelectedRowNhanVien = -1;

                btnThemNV.Enabled = true;
                btnSuaNV.Enabled = false;
                btnXoaNV.Enabled = false;
                btnChiTietNV.Enabled = false;
                return;
            }

            tbNhanVien.ClearSelection();
            tbNhanVien.Rows[e.RowIndex].Selected = true;
            lastSelectedRowNhanVien = e.RowIndex;

            btnThemNV.Enabled = true;
            btnSuaNV.Enabled = true;
            btnXoaNV.Enabled = true;
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

        private void resetGiaTri()
        {
            cboTimKiemNV.SelectedIndex = -1;
            cboTaiKhoan.SelectedIndex = -1;
            txtTimKiemNV.Clear();
            txtTenNV.Clear();
            txtTenNV.Clear();
            txtEmailNV.Clear();
            loadFontChuVaSize();
        }

        private void rdoTimCoBan_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimCoBan.Checked == true)
            {
                cboTimKiemNV.Enabled = true;
                txtTimKiemNV.Enabled = true;

                txtSdtNV.Enabled = false;
                txtSdtNV.Enabled = false;
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

            BindingList<nhaCungCapDTO> dskq = busNhaCungCap.timKiemNangCao(tenNCC, sdtNCC, emailNCC, diachiNCC);
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
    }
}
