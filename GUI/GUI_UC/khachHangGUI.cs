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
    public partial class khachHangGUI : UserControl
    {
        private int lastSelectedRowKhachHang = -1;
        private khachHangBUS busKhachHang = new khachHangBUS();

        public khachHangGUI()
        {
            InitializeComponent();
        }

        private void khachHangGUI_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            BindingList<khachHangDTO> dsKH = new khachHangBUS().LayDanhSach();
            loadDanhSachKhachHang(dsKH);
            loadFontChuVaSize();
            loadChuVoTxtVaCb();
            rdoTimCoBan.Checked = true;
            CheckQuyen();
        }

        private void CheckQuyen()
        {
            var quyenKH = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 6);

            if (quyenKH != null)
            {
                btnThemKH.Enabled = (quyenKH.CAN_CREATE == 1);

                btnSuaKH.Enabled = (quyenKH.CAN_UPDATE) == 1;

                btnXoaKH.Enabled = (quyenKH.CAN_DELETE) == 1;

                btnExcelKH.Enabled = (quyenKH.CAN_CREATE) == 1;

                btnSuaKH.Enabled = false;
                btnXoaKH.Enabled = false;
            }
            else
            {
                btnThemKH.Enabled = false;
                btnSuaKH.Enabled = false;
                btnXoaKH.Enabled = false;
                btnChiTietKH.Enabled = false;
                btnExcelKH.Enabled = false;
            }
        }
        private void loadDanhSachKhachHang(BindingList<khachHangDTO> ds)
        {
            tableKhachHang.AutoGenerateColumns = false;
            tableKhachHang.DataSource = ds;

            tableKhachHang.Columns.Clear();

            tableKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaKhachHang",
                HeaderText = "Mã KH"
            });
            tableKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenKhachHang",
                HeaderText = "Tên KH"
            });
            tableKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoDienThoai",
                HeaderText = "Số điện thoại"
            });
            tableKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Email",
                HeaderText = "Email"
            });

            btnSuaKH.Enabled = false;
            btnXoaKH.Enabled = false;
            btnChiTietKH.Enabled = false;
            tableKhachHang.ReadOnly = true;
            tableKhachHang.ClearSelection();
        }

        private void tableKhachHang_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }
        private void loadFontChuVaSize()
        {
            foreach (DataGridViewColumn col in tableKhachHang.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableKhachHang.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tableKhachHang.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tableKhachHang.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(12);

            tableKhachHang.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableKhachHang.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableKhachHang.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableKhachHang.Refresh();
        }

        private void btnThemKH_Click(object sender, EventArgs e)
        {
            using (insertKhachHang form = new insertKhachHang(null))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    khachHangDTO ct = form.kh;
                    busKhachHang.them(ct);
                    tableKhachHang.Refresh();
                    tableKhachHang.ClearSelection();
                }
            }
        }

        private void btnSuaKH_Click(object sender, EventArgs e)
        {
            if (tableKhachHang.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableKhachHang.SelectedRows[0];
                khachHangDTO kh = row.DataBoundItem as khachHangDTO;

                if (kh != null)
                {
                    using (updateKhachHang form = new updateKhachHang(kh))
                    {
                        form.StartPosition = FormStartPosition.CenterParent;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            khachHangDTO kq = form.kh;
                            busKhachHang.sua(kq);
                            tableKhachHang.Refresh();
                        }
                    }
                }
            }
        }

        private void tableKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var quyenKH = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 6);

            bool isAdmin = (Session.TaiKhoanHienTai.MAVAITRO == 1);

            bool coQuyenThem = isAdmin || (quyenKH != null && quyenKH.CAN_CREATE == 1);
            bool coQuyenSua = isAdmin || (quyenKH != null && quyenKH.CAN_UPDATE == 1);
            bool coQuyenXoa = isAdmin || (quyenKH != null && quyenKH.CAN_DELETE == 1);

            if (e.RowIndex == lastSelectedRowKhachHang)
            {
                tableKhachHang.ClearSelection();
                lastSelectedRowKhachHang = -1;

                btnThemKH.Enabled = coQuyenThem;
                btnSuaKH.Enabled = false;
                btnXoaKH.Enabled = false;
                btnChiTietKH.Enabled = false;
                return;
            }

            tableKhachHang.ClearSelection();
            tableKhachHang.Rows[e.RowIndex].Selected = true;
            lastSelectedRowKhachHang = e.RowIndex;

            btnThemKH.Enabled = coQuyenThem;
            btnSuaKH.Enabled = coQuyenSua;
            btnXoaKH.Enabled = coQuyenXoa;
            btnChiTietKH.Enabled = true;
        }

        private void tableKhachHang_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tableKhachHang.ClearSelection();
        }

        private void btnXoaKH_Click(object sender, EventArgs e)
        {
            if (tableKhachHang.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableKhachHang.SelectedRows[0];
                khachHangDTO kh = row.DataBoundItem as khachHangDTO;

                using (deleteKhachHang form = new deleteKhachHang())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        int maKH = kh.MaKhachHang;
                        busKhachHang.xoa(maKH);
                        btnSuaKH.Enabled = false;
                        btnXoaKH.Enabled = false;
                        btnChiTietKH.Enabled = false;
                    }
                }
            }
        }

        private void btnChiTietKH_Click(object sender, EventArgs e)
        {
            if(tableKhachHang.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableKhachHang.SelectedRows[0];
                khachHangDTO kh = row.DataBoundItem as khachHangDTO;

                using(detailKhachHang form = new detailKhachHang(kh))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
            }
        }

        private void btnReFreshKH_Click(object sender, EventArgs e)
        {
            BindingList<khachHangDTO> dskh = new khachHangBUS().LayDanhSach();
            loadDanhSachKhachHang(dskh);
            loadFontChuVaSize();
            resetGiaTri();
        }

        private void btnExcelKH_Click(object sender, EventArgs e)
        {
            using(selectExcelKhachHangGUI form = new selectExcelKhachHangGUI())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
        }

        private void rdoTimCoBan_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoTimCoBan.Checked == true)
            {
                cboTimKiemKH.Enabled = true;
                txtTimKiemKH.Enabled = true;

                txtTenKH.Enabled = false;
                txtSdtKH.Enabled = false;
                txtEmailKH.Enabled = false;
                rdoTimNangCao.Checked = false;
                resetGiaTri();
            }else
            {
                cboTimKiemKH.Enabled = false;
                txtTimKiemKH.Enabled = false;

                txtTenKH.Enabled = true;
                txtSdtKH.Enabled = true;
                txtEmailKH.Enabled = true;
                rdoTimCoBan.Checked = false;
                resetGiaTri();
            }
        }

        private void rdoTimNangCao_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoTimNangCao.Checked == true)
            {
                txtTenKH.Enabled = true;
                txtSdtKH.Enabled = true;
                txtEmailKH.Enabled = true;

                cboTimKiemKH.Enabled = false;
                txtTimKiemKH.Enabled = false;

                rdoTimCoBan.Checked = false;
                resetGiaTri();
            }
            else
            {
                txtTenKH.Enabled = false;
                txtSdtKH.Enabled = false;
                txtEmailKH.Enabled = false;

                cboTimKiemKH.Enabled = true;
                txtTimKiemKH.Enabled = true;

                rdoTimNangCao.Checked = false;
                resetGiaTri();
            }
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
            SetComboBoxPlaceholder(cboTimKiemKH, "Chọn giá trị TK");
            SetPlaceholder(txtTimKiemKH, "Nhập giá trị tìm cần tìm");
            SetPlaceholder(txtTenKH, "Nhập tên khách hàng");
            SetPlaceholder(txtSdtKH, "Nhập sđt khách hàng");
            SetPlaceholder(txtEmailKH, "Nhập email khách hàng");
        }

        private void resetGiaTri()
        {
            cboTimKiemKH.SelectedIndex = -1;
            txtTimKiemKH.Clear();
            txtTenKH.Clear();
            txtSdtKH.Clear();
            txtEmailKH.Clear();
            loadChuVoTxtVaCb();
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
            if(cboTimKiemKH.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboTimKiemKH.Focus();
                return;
            }
            if(string.IsNullOrWhiteSpace(txtTimKiemKH.Text))
            {
                MessageBox.Show("Vui lòng Nhập giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimKiemKH.Focus();
                return;
            }

            string tim = txtTimKiemKH.Text.Trim();
            int index = cboTimKiemKH.SelectedIndex;

            BindingList<khachHangDTO> dskq = busKhachHang.timKiemCoban(tim, index);
            if(dskq != null && dskq.Count > 0)
            {
                loadDanhSachKhachHang(dskq);
                loadFontChuVaSize();
            }else
            {
                MessageBox.Show("Không tìm thấy giá trị cần tìm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void timKiemNangCao()
        {
            string tenKH = string.IsNullOrWhiteSpace(txtTenKH.Text) ? null : txtTenKH.Text.Trim();
            string sdtKH = string.IsNullOrWhiteSpace(txtSdtKH.Text) ? null : txtSdtKH.Text.Trim();
            string emailKH = string.IsNullOrWhiteSpace(txtEmailKH.Text) ? null : txtEmailKH.Text.Trim();
            if(tenKH == "Nhập tên khách hàng")
            {
                tenKH = null;
            }
            if(sdtKH == "Nhập sđt khách hàng")
            {
                sdtKH = null;
            }
            if(emailKH == "Nhập email khách hàng")
            {
                emailKH = null;
            }

            if(tenKH == null && sdtKH == null && emailKH == null)
            {
                MessageBox.Show("Vui lòng nhập ít nhất một điều kiện để tìm kiếm","Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BindingList<khachHangDTO> dskq = busKhachHang.timKiemNangCao(tenKH, sdtKH, emailKH);
            if (dskq != null && dskq.Count > 0)
            {
                loadDanhSachKhachHang(dskq);
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
