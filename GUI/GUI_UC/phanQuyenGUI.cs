using DTO;
using BUS;
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
using System.Linq.Expressions;
using GUI.GUI_CRUD;

namespace GUI.GUI_UC
{
    public partial class phanQuyenGUI : UserControl
    {
        private int lastSelectedRowPhanQuyen = -1;
        private phanquyenBUS bus = new phanquyenBUS();
        private BindingList<vaitroDTO> dsVaitro;
        private BindingList<quyenDTO> dsQuyen;
        public phanQuyenGUI()
        {
            InitializeComponent();
        }

        private void phanQuyenGUI_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            dsVaitro = new vaitroBUS().LayDanhSach();
            dsQuyen = new quyenBUS().LayDanhSach();

            BindingList<phanquyenDTO> dsHienThi = new phanquyenBUS().LayDanhSach();
            loadDanhSachPhanQuyen(dsHienThi);
            loadFontChuVaSize();
            loadChuVoTxtVaCb();
            rdoTimCoBan.Checked = true;
        }

        private void loadDanhSachPhanQuyen(BindingList<phanquyenDTO> ds)
        {
            tbPhanQuyen.AutoGenerateColumns = false;
            tbPhanQuyen.Columns.Clear();

            tbPhanQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaVaiTro",
                HeaderText = "Vai trò"
            });
            tbPhanQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaQuyen",
                HeaderText = "Quyền"
            });
            tbPhanQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CAN_CREATE",
                HeaderText = "CREATE"
            });
            tbPhanQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CAN_READ",
                HeaderText = "READ"
            });
            tbPhanQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CAN_UPDATE",
                HeaderText = "UPDATE"
            });
            tbPhanQuyen.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CAN_DELETE",
                HeaderText = "DELETE"
            });

            tbPhanQuyen.DataSource = ds;
            tbPhanQuyen.ReadOnly = true;
            btnChiTietPQ.Enabled = false;
            tbPhanQuyen.ClearSelection();
        }

        private void tbPhanQuyen_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(e.RowIndex < 0)
            {
                return;
            }
            phanquyenDTO pq = tbPhanQuyen.Rows[e.RowIndex].DataBoundItem as phanquyenDTO;

            if (tbPhanQuyen.Columns[e.ColumnIndex].HeaderText == "Vai trò")
            {
                vaitroDTO vt = dsVaitro.FirstOrDefault(x => x.MaVaiTro == pq.MaVaiTro);
                e.Value = vt?.TenVaiTro ?? "Không xác định";
            }

            if (tbPhanQuyen.Columns[e.ColumnIndex].HeaderText == "Quyền")
            {
                quyenDTO q = dsQuyen.FirstOrDefault(x => x.MaQuyen == pq.MaQuyen);
                e.Value = q?.TenQuyen ?? "Không xác định";
            }

            if (tbPhanQuyen.Columns[e.ColumnIndex].HeaderText == "READ")
            {
                e.Value = pq.CAN_READ == 1 ? "Có" : "Không";
            }

            if (tbPhanQuyen.Columns[e.ColumnIndex].HeaderText == "CREATE")
            {
                e.Value = pq.CAN_CREATE == 1 ? "Có" : "Không";
            }

            if (tbPhanQuyen.Columns[e.ColumnIndex].HeaderText == "UPDATE")
            {
                e.Value = pq.CAN_UPDATE == 1 ? "Có" : "Không";
            }


            if (tbPhanQuyen.Columns[e.ColumnIndex].HeaderText == "DELETE")
            {
                e.Value = pq.CAN_DELETE == 1 ? "Có" : "Không";
            }
        }

        private void loadFontChuVaSize()
        {
            foreach (DataGridViewColumn col in tbPhanQuyen.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tbPhanQuyen.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tbPhanQuyen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tbPhanQuyen.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tbPhanQuyen.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(12);

            tbPhanQuyen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbPhanQuyen.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbPhanQuyen.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tbPhanQuyen.Refresh();
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
        private void loadChuVoTxtVaCb()
        {
            SetupComboBoxData(can_create);
            SetupComboBoxData(can_read);
            SetupComboBoxData(can_update);
            SetupComboBoxData(can_delete);

            SetComboBoxPlaceholder(cboTimKiemPQ, "Chọn giá trị TK");
            SetPlaceholder(txtTimKiemPQ, "Nhập giá trị tìm cần tìm");

            SetPlaceholder(txtTenVaiTro, "Tên VT");
            SetPlaceholder(txtTenQuyen, "Tên quyền");

            SetComboBoxPlaceholder(can_create,"create");
            SetComboBoxPlaceholder(can_read, "read");
            SetComboBoxPlaceholder(can_update, "update");
            SetComboBoxPlaceholder(can_delete, "delete");

        }

        private void tbPhanQuyen_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tbPhanQuyen.ClearSelection();
        }

        private void btnCRUDPQ_Click(object sender, EventArgs e)
        {
            using (insertPhanQuyen form = new insertPhanQuyen())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
        }

        private void btnVaiTro_Click(object sender, EventArgs e)
        {
            using(vaiTroVaQuyenGUI form = new vaiTroVaQuyenGUI())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
            dsQuyen = new quyenBUS().LayDanhSach();
            dsVaitro = new vaitroBUS().LayDanhSach();
        }

        private void tbPhanQuyen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.RowIndex == lastSelectedRowPhanQuyen)
            {
                tbPhanQuyen.ClearSelection();
                lastSelectedRowPhanQuyen = -1;

                btnChiTietPQ.Enabled = false;
                return;
            }

            tbPhanQuyen.ClearSelection();
            tbPhanQuyen.Rows[e.RowIndex].Selected = true;
            lastSelectedRowPhanQuyen = e.RowIndex;

            btnChiTietPQ.Enabled = true;
        }

        private void btnChiTietPQ_Click(object sender, EventArgs e)
        {
            if(tbPhanQuyen.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tbPhanQuyen.SelectedRows[0];
                phanquyenDTO pq = row.DataBoundItem as phanquyenDTO;

                using(detailPhanQuyen form = new detailPhanQuyen(pq))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
            }
        }

        private void btnReFreshPQ_Click(object sender, EventArgs e)
        {
            BindingList<phanquyenDTO> ds = new phanquyenBUS().LayDanhSach();
            loadDanhSachPhanQuyen(ds);
            loadFontChuVaSize();
            resetFormTK();
        }

        public void resetFormTK()
        {
            cboTimKiemPQ.SelectedIndex = -1;
            txtTimKiemPQ.Clear();
            txtTenVaiTro.Clear();
            txtTenQuyen.Clear();
            can_create.SelectedIndex = -1;
            can_read.SelectedIndex = -1;
            can_update.SelectedIndex = -1;
            can_delete.SelectedIndex = -1;
            loadChuVoTxtVaCb();
        }
        private void rdoTimCoBan_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimCoBan.Checked == true)
            {
                cboTimKiemPQ.Enabled = true;
                txtTimKiemPQ.Enabled = true;

                txtTenVaiTro.Enabled = false;
                txtTenQuyen.Enabled = false;
                can_create.Enabled = false;
                can_read.Enabled = false;
                can_update.Enabled = false;
                can_delete.Enabled = false;
                rdoTimNangCao.Checked = false;
                resetFormTK();
            }else
            {
                cboTimKiemPQ.Enabled = false;
                txtTimKiemPQ.Enabled = false;

                txtTenVaiTro.Enabled = true;
                txtTenQuyen.Enabled = true;
                can_create.Enabled = true;
                can_read.Enabled = true;
                can_update.Enabled = true;
                can_delete.Enabled = true;
                rdoTimCoBan.Checked = false;
                resetFormTK();
            }
        }

        private void rdoTimNangCao_CheckedChanged(object sender, EventArgs e)
        {
            if(rdoTimNangCao.Checked == true)
            {
                cboTimKiemPQ.Enabled = false;
                txtTimKiemPQ.Enabled = false;

                txtTenVaiTro.Enabled = true;
                txtTenQuyen.Enabled = true;
                can_create.Enabled = true;
                can_read.Enabled = true;
                can_update.Enabled = true;
                can_delete.Enabled = true;

                rdoTimCoBan.Checked = false;
                resetFormTK();
            }else
            {
                cboTimKiemPQ.Enabled = true;
                txtTimKiemPQ.Enabled = true;

                txtTenVaiTro.Enabled = false;
                txtTenQuyen.Enabled = false;
                can_create.Enabled = false;
                can_read.Enabled = false;
                can_update.Enabled = false;
                can_delete.Enabled = false;

                rdoTimNangCao.Checked = false;
                resetFormTK();
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
            if (cboTimKiemPQ.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboTimKiemPQ.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTimKiemPQ.Text))
            {
                MessageBox.Show("Vui lòng Nhập giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTimKiemPQ.Focus();
                return;
            }

            string tim = txtTimKiemPQ.Text.Trim();
            int index = cboTimKiemPQ.SelectedIndex;

            BindingList<phanquyenDTO> dskq = bus.timKiemCoBan(tim, index);
            if (dskq != null && dskq.Count > 0)
            {
                loadDanhSachPhanQuyen(dskq);
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
            string tenVaiTro = string.IsNullOrWhiteSpace(txtTenVaiTro.Text) ? null : txtTenVaiTro.Text.Trim();
            string tenQuyen = string.IsNullOrWhiteSpace(txtTenQuyen.Text) ? null : txtTenQuyen.Text.Trim();
            int create = (can_create.SelectedIndex == -1) ? -1 : Convert.ToInt32(can_create.SelectedValue);
            int read = (can_read.SelectedIndex == -1) ? -1 : Convert.ToInt32(can_read.SelectedValue);
            int update = (can_update.SelectedIndex == -1) ? -1 : Convert.ToInt32(can_update.SelectedValue);
            int delete = (can_delete.SelectedIndex == -1) ? -1 : Convert.ToInt32(can_delete.SelectedValue);

            if (tenVaiTro == "Tên VT")
            {
                tenVaiTro = null;
            }    
            if(tenQuyen == "Tên quyền")
            {
                tenQuyen = null;
            }    
            if(tenVaiTro == null && tenQuyen == null && create == -1 && read == -1 && update == -1 && delete == -1)
            {
                MessageBox.Show("Vui lòng nhập ít nhất một điều kiện để tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            BindingList<phanquyenDTO> ds = bus.timKiemNangCao(tenVaiTro, tenQuyen, create, read, update, delete);
            if(ds != null && ds.Count > 0)
            {
                loadDanhSachPhanQuyen(ds);
                loadFontChuVaSize();
            }else
            {
                MessageBox.Show("Không tìm thấy kết quả tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnExcelPQ_Click(object sender, EventArgs e)
        {
            using (excelPhanQuyen form = new excelPhanQuyen())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    BindingList<phanquyenDTO> dsMoi = bus.LayDanhSach();
                    loadDanhSachPhanQuyen(dsMoi);
                    loadFontChuVaSize();
                }
            }
        }
    }
}
