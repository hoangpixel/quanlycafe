using BUS;
using DTO;
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

namespace GUI.GUI_CRUD
{
    public partial class detailNguyenLieu : Form
    {
        private nguyenLieuDTO ct;
        private BindingList<donViDTO> dsDV;
        private BindingList<congThucDTO> dsCongThuc;
        private BindingList<heSoDTO> dsHeSo;
        private int selectSanPham = -1;
        public detailNguyenLieu(nguyenLieuDTO ct)
        {
            InitializeComponent();
            this.ct = ct;
        }

        private void loadFontChuVaSizeTableNguyenLieu()
        {
            foreach (DataGridViewColumn col in tableNguyenLieu.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableNguyenLieu.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableNguyenLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tableNguyenLieu.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tableNguyenLieu.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            tableNguyenLieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableNguyenLieu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableNguyenLieu.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableNguyenLieu.Refresh();
        }

        private void loadFontChuVaSizeTableHeSo()
        {
            foreach (DataGridViewColumn col in tableHeSo.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableHeSo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableHeSo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tableHeSo.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tableHeSo.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            tableHeSo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableHeSo.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableHeSo.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableHeSo.Refresh();
        }
        private void detailNguyenLieu_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            dsDV = new donViBUS().LayDanhSach();
            dsCongThuc = new congThucBUS().LayDanhSach();
            dsHeSo = new heSoBUS().LayDanhSach();

            txtMaNL.Text = ct.MaNguyenLieu.ToString();
            txtTenNL.Text = ct.TenNguyenLieu;
            txtTonKho.Text = ct.TonKho.ToString();

            donViDTO dv = dsDV.FirstOrDefault(x => x.MaDonVi == ct.MaDonViCoSo);
            txtDonVi.Text = dv?.TenDonVi ?? "Không xác định";


            BindingList<sanPhamDTO> dsSP = dsSanPhamSauKhiLoc();
            loadDanhSachSanPham(dsSP);

            BindingList<heSoDTO> dsHS = dsHeSoSauKhiLoc();
            loadDanhSachHeSo(dsHS);
        }

        private BindingList<sanPhamDTO> dsSanPhamSauKhiLoc()
        {
            sanPhamBUS busSP = new sanPhamBUS();
            BindingList<sanPhamDTO> tatCaSP = busSP.LayDanhSach();
            List<congThucDTO> congThucLienQuan = new List<congThucDTO>();
            foreach (congThucDTO c in dsCongThuc)
            {
                if (c.MaNguyenLieu == ct.MaNguyenLieu)
                {
                    congThucLienQuan.Add(c);
                }
            }
            List<sanPhamDTO> danhSachLoc = new List<sanPhamDTO>();
            foreach (sanPhamDTO sp in tatCaSP)
            {
                foreach (congThucDTO cth in congThucLienQuan)
                {
                    if (cth.MaSanPham == sp.MaSP)
                    {
                        danhSachLoc.Add(sp);
                        break;
                    }
                }
            }
            return new BindingList<sanPhamDTO>(danhSachLoc);
        }

        private void loadDanhSachSanPham(BindingList<sanPhamDTO> ds)
        {
            tableNguyenLieu.AutoGenerateColumns = false;
            tableNguyenLieu.Columns.Clear();
            tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSP", HeaderText = "Mã SP" });
            tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenSP", HeaderText = "Tên SP" });
            tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Gia", HeaderText = "Giá", DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
            tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuong", HeaderText = "Số lượng" });
            tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DonVi", HeaderText = "Đơn vị" });

            tableNguyenLieu.DefaultCellStyle.SelectionBackColor = tableNguyenLieu.DefaultCellStyle.BackColor;
            tableNguyenLieu.DefaultCellStyle.SelectionForeColor = tableNguyenLieu.DefaultCellStyle.ForeColor;

            tableNguyenLieu.DataSource = ds;
            tableNguyenLieu.ReadOnly = true;
            tableNguyenLieu.ClearSelection();
            loadFontChuVaSizeTableNguyenLieu();
        }

        private void tableNguyenLieu_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            sanPhamDTO sp = tableNguyenLieu.Rows[e.RowIndex].DataBoundItem as sanPhamDTO;
            if (tableNguyenLieu.Columns[e.ColumnIndex].HeaderText == "Số lượng")
            {
                if (dsCongThuc == null)
                {
                    e.Value = "Không xác định";
                    return;
                }

                congThucDTO cth = dsCongThuc.FirstOrDefault(x => x.MaSanPham == sp.MaSP && x.MaNguyenLieu == ct.MaNguyenLieu);
                e.Value = cth?.SoLuongCoSo.ToString() ?? "Không xác định";
            }

            if (tableNguyenLieu.Columns[e.ColumnIndex].HeaderText == "Đơn vị")
            {
                if (dsCongThuc == null || dsDV == null)
                {
                    e.Value = "Không xác định";
                    return;
                }

                congThucDTO cth = dsCongThuc.FirstOrDefault(x => x.MaSanPham == sp.MaSP && x.MaNguyenLieu == ct.MaNguyenLieu);
                if (cth != null)
                {
                    donViDTO dv = dsDV.FirstOrDefault(x => x.MaDonVi == cth.MaDonViCoSo);
                    e.Value = dv?.TenDonVi ?? "Không xác định";
                }
            }

            if (tableNguyenLieu.Columns[e.ColumnIndex].HeaderText == "Số lượng" && e.Value != null)
            {
                if (double.TryParse(e.Value.ToString(), out double tonKho))
                {
                    if (tonKho % 1 == 0)
                    {
                        e.Value = tonKho.ToString("N0");
                    }
                    else
                    {
                        e.Value = tonKho.ToString("0.000");
                    }
                    e.FormattingApplied = true;
                }
            }
        }

        private void loadDanhSachHeSo(BindingList<heSoDTO> ds)
        {
            tableHeSo.AutoGenerateColumns = false;
            tableHeSo.Columns.Clear();

            tableHeSo.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonVi", HeaderText = "Mã ĐV" });
            tableHeSo.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonVi", HeaderText = "Tên đơn vị" });
            tableHeSo.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HeSo", HeaderText = "Hệ số" });

            tableHeSo.DefaultCellStyle.SelectionBackColor = tableNguyenLieu.DefaultCellStyle.BackColor;
            tableHeSo.DefaultCellStyle.SelectionForeColor = tableNguyenLieu.DefaultCellStyle.ForeColor;


            tableHeSo.DataSource = ds;

            tableHeSo.ReadOnly = true;
            tableHeSo.ClearSelection();
            loadFontChuVaSizeTableHeSo();
        }

        private BindingList<heSoDTO> dsHeSoSauKhiLoc()
        {
            List<heSoDTO> dsKQ = new List<heSoDTO>();
            foreach (heSoDTO hs in dsHeSo)
            {
                if(hs.MaNguyenLieu == ct.MaNguyenLieu)
                {
                    dsKQ.Add(hs);
                }
            }
            return new BindingList<heSoDTO>(dsKQ);
        }
        private void tableHeSo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            heSoDTO hs = tableHeSo.Rows[e.RowIndex].DataBoundItem as heSoDTO;
            if (tableHeSo.Columns[e.ColumnIndex].HeaderText == "Tên đơn vị")
            {
                donViDTO dv = dsDV.FirstOrDefault(x => x.MaDonVi == hs.MaDonVi);
                e.Value = dv?.TenDonVi ?? "Không xác định";
            }

            if (tableHeSo.Columns[e.ColumnIndex].HeaderText == "Hệ số" && e.Value != null)
            {
                if (double.TryParse(e.Value.ToString(), out double tonKho))
                {
                    if (tonKho % 1 == 0)
                    {
                        e.Value = tonKho.ToString("N0");
                    }
                    else
                    {
                        e.Value = tonKho.ToString("0.000");
                    }
                    e.FormattingApplied = true;
                }
            }
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtDonVi_TextChanged(object sender, EventArgs e)
        {

        }

        private void bigLabel1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
