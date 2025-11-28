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
            tableKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TrangThai",
                HeaderText = "Trạng thái"
            });
            tableKhachHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NgayTao",
                HeaderText = "Ngày tạo"
            });

            btnSuaKH.Enabled = false;
            btnXoaKH.Enabled = false;
            btnChiTietKH.Enabled = false;
            tableKhachHang.ReadOnly = true;
            tableKhachHang.ClearSelection();
        }

        private void tableKhachHang_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            khachHangDTO kh = tableKhachHang.Rows[e.RowIndex].DataBoundItem as khachHangDTO;
            if (tableKhachHang.Columns[e.ColumnIndex].HeaderText == "Trạng thái")
            {
                e.Value = kh.TrangThai == 1 ? "Đang hoạt động" : "Không còn hoạt động";
            }
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
            if (e.RowIndex == lastSelectedRowKhachHang)
            {
                tableKhachHang.ClearSelection();
                lastSelectedRowKhachHang = -1;

                btnThemKH.Enabled = true;
                btnSuaKH.Enabled = false;
                btnXoaKH.Enabled = false;
                btnChiTietKH.Enabled = false;
                return;
            }

            tableKhachHang.ClearSelection();
            tableKhachHang.Rows[e.RowIndex].Selected = true;
            lastSelectedRowKhachHang = e.RowIndex;

            btnThemKH.Enabled = true;
            btnSuaKH.Enabled = true;
            btnXoaKH.Enabled = true;
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
        }
    }
}
