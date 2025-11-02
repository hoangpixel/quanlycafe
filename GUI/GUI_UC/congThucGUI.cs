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
using System.Xml.Linq;

namespace GUI.GUI_UC
{
    public partial class congThucGUI : UserControl
    {

        private int lastSelectedRowCongThuc = -1;
        private congThucBUS busCongThuc = new congThucBUS();
        private BindingList<nguyenLieuDTO> dsNguyenLieu;
        private BindingList<donViDTO> dsDonVi;
        private BindingList<sanPhamDTO> dsSanPham;

        public congThucGUI()
        {
            InitializeComponent();
        }

        private void congThucGUI_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            dsNguyenLieu = new nguyenLieuBUS().LayDanhSach();
            dsDonVi = new donViBUS().LayDanhSach();
            dsSanPham = new sanPhamBUS().LayDanhSach();

            BindingList<congThucDTO> ds = busCongThuc.LayDanhSach();
            loadDanhSachCongThuc(ds);
        }

        private void loadDanhSachCongThuc(BindingList<congThucDTO> ds)
        {
            tableCongThuc.AutoGenerateColumns = false;
            tableCongThuc.Columns.Clear();

            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSanPham", HeaderText = "Mã SP" });
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSanPham", HeaderText = "Tên SP" });
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Mã NL" });
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Tên NL" });
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuongCoSo", HeaderText = "Số lượng" });
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonViCoSo", HeaderText = "Đơn vị" });

            tableCongThuc.DataSource = ds;

            btnSuaCT.Enabled = false;
            btnXoaCT.Enabled = false;
            btnChiTietCT.Enabled = false;
            tableCongThuc.ReadOnly = true;
            tableCongThuc.ClearSelection();

            loadFontChuVaSize();
        }

        private void tableCongThuc_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            congThucDTO ct = tableCongThuc.Rows[e.RowIndex].DataBoundItem as congThucDTO;

            if (tableCongThuc.Columns[e.ColumnIndex].HeaderText == "Tên SP")
            {
                sanPhamDTO sp = dsSanPham.FirstOrDefault(x => x.MaSP == ct.MaSanPham);
                e.Value = sp?.TenSP ?? "Không xác định";
            }

            if (tableCongThuc.Columns[e.ColumnIndex].HeaderText == "Tên NL")
            {
                nguyenLieuDTO nl = dsNguyenLieu.FirstOrDefault(x => x.MaNguyenLieu == ct.MaNguyenLieu);
                e.Value = nl?.TenNguyenLieu ?? "Không xác định";
            }

            if (tableCongThuc.Columns[e.ColumnIndex].HeaderText == "Đơn vị")
            {
                donViDTO dv = dsDonVi.FirstOrDefault(x => x.MaDonVi == ct.MaDonViCoSo);
                e.Value = dv?.TenDonVi ?? "Không xác định";
            }
        }

        private void loadFontChuVaSize()
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
            tableCongThuc.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(12);

            // --- Fix lỗi mất text khi đổi font ---
            tableCongThuc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableCongThuc.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableCongThuc.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableCongThuc.Refresh();
        }

        private void kiemTraClickTable(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.RowIndex == lastSelectedRowCongThuc)
            {
                tableCongThuc.ClearSelection();
                lastSelectedRowCongThuc = -1;
                btnThemCT.Enabled = true;
                btnSuaCT.Enabled = false;
                btnXoaCT.Enabled = false;
                btnChiTietCT.Enabled = false;
                return;
            }

            tableCongThuc.ClearSelection();
            tableCongThuc.Rows[e.RowIndex].Selected = true;
            lastSelectedRowCongThuc = e.RowIndex;

            btnSuaCT.Enabled = true;
            btnXoaCT.Enabled = true;
            btnChiTietCT.Enabled = true;
        }

        private void btnThemCT_Click(object sender, EventArgs e)
        {
            using (insertCongThuc form = new insertCongThuc())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
                congThucBUS bus = new congThucBUS();
                BindingList<congThucDTO> ds = bus.LayDanhSach();
                loadDanhSachCongThuc(ds);
            }
        }

        private void btnSuaCT_Click(object sender, EventArgs e)
        {
            if (tableCongThuc.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableCongThuc.SelectedRows[0];
                congThucDTO ct = row.DataBoundItem as congThucDTO;

                using (updateCongThuc form = new updateCongThuc(ct))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if(form.ShowDialog() == DialogResult.OK)
                    {
                        congThucDTO ctSuaNe = form.ctSUA;
                        busCongThuc.suaCongThuc(ctSuaNe);
                        tableCongThuc.Refresh();
                    }
                }
            }
        }

        private void btnXoaCT_Click(object sender, EventArgs e)
        {
            if (tableCongThuc.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableCongThuc.SelectedRows[0];
                congThucDTO ct = row.DataBoundItem as congThucDTO;
                int maSP = ct.MaSanPham;
                int maNL = ct.MaNguyenLieu;

                using (deleteCongThuc form = new deleteCongThuc())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if(form.ShowDialog() == DialogResult.OK)
                    {
                        busCongThuc.xoaCongThuc(maSP, maNL);
                        btnSuaCT.Enabled = false;
                        btnXoaCT.Enabled = false;
                        btnChiTietCT.Enabled = false;
                    }
                }
            }
        }

        private void btnChiTietCT_Click(object sender, EventArgs e)
        {
            if (tableCongThuc.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableCongThuc.SelectedRows[0];
                congThucDTO ct = row.DataBoundItem as congThucDTO;

                using (detailCongThuc form = new detailCongThuc(ct))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
            }
        }

        private void btnReFreshCT_Click(object sender, EventArgs e)
        {
            congThucBUS bus = new congThucBUS();
            BindingList<congThucDTO> ds = bus.LayDanhSach();
            loadDanhSachCongThuc(ds);
        }

        private void btnExcelCT_Click(object sender, EventArgs e)
        {
            using (selectExcelCongThuc form = new selectExcelCongThuc())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
            congThucBUS bus = new congThucBUS();
            BindingList<congThucDTO> ds = bus.LayDanhSach();
            loadDanhSachCongThuc(ds);
        }

        // dòng này là để cho khi mà mình load trang nó kh chọn dòng đầu nha
        private void tableCongThuc_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tableCongThuc.ClearSelection();
        }


    }
}
