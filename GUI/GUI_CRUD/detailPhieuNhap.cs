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

namespace GUI.GUI_CRUD
{
    public partial class detailPhieuNhap : Form
    {
        private phieuNhapDTO pn;
        private BindingList<nhanVienDTO> dsNV;
        private BindingList<nhaCungCapDTO> dsNCC;
        private BindingList<nguyenLieuDTO> dsNL;
        private BindingList<donViDTO> dsDV;
        private phieuNhapBUS bus = new phieuNhapBUS();
        public detailPhieuNhap(phieuNhapDTO pn)
        {
            FontManager.LoadFont(); 
            FontManager.ApplyFontToAllControls(this);
            InitializeComponent();
            this.pn = pn;
            dsNV = new nhanVienBUS().LayDanhSach();
            dsNCC = new nhaCungCapBUS().LayDanhSach();
            dsNL = new nguyenLieuBUS().LayDanhSach();
            dsDV = new donViBUS().LayDanhSach();
        }

        private void detailPhieuNhap_Load(object sender, EventArgs e)
        {
            txtMaPN.Text = pn.MaPN.ToString();

            nhaCungCapDTO ncc = dsNCC.FirstOrDefault(x => x.MaNCC == pn.MaNCC);
            nhanVienDTO nv = dsNV.FirstOrDefault(x => x.MaNhanVien == pn.MaNhanVien);

            txtTenNV.Text = nv?.HoTen ?? "Không xác định";
            txtTenNCC.Text = ncc?.TenNCC ?? "Không xác định";

            txtThoiGian.Text = pn.ThoiGian.ToString("dd/MM/yyyy HH:mm");
            txtTongTien.Text = pn.TongTien.ToString("N0") + " VNĐ";

            List<ctPhieuNhapDTO> ds = bus.LayChiTiet(pn.MaPN);
            loadDanhSachCTPN(ds);
        }

        private void loadDanhSachCTPN(List<ctPhieuNhapDTO> ds)
        {
            tbChiTIetPhieuNhap.AutoGenerateColumns = false;
            tbChiTIetPhieuNhap.Columns.Clear();

            tbChiTIetPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaPN", HeaderText = "Mã PN" });
            tbChiTIetPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Tên NL" });
            tbChiTIetPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonVi", HeaderText = "Tên ĐV" });
            tbChiTIetPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuongCoSo", HeaderText = "Số lượng" });
            tbChiTIetPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DonGia", HeaderText = "Đơn giá", DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });
            tbChiTIetPhieuNhap.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ThanhTien", HeaderText = "Thành tiền", DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } });

            tbChiTIetPhieuNhap.DataSource = ds;
            tbChiTIetPhieuNhap.ReadOnly = true;
            tbChiTIetPhieuNhap.ClearSelection();
            loadFontChuVaSizeTableNguyenLieu();
        }

        private void loadFontChuVaSizeTableNguyenLieu()
        {
            foreach (DataGridViewColumn col in tbChiTIetPhieuNhap.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tbChiTIetPhieuNhap.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tbChiTIetPhieuNhap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tbChiTIetPhieuNhap.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tbChiTIetPhieuNhap.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            tbChiTIetPhieuNhap.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbChiTIetPhieuNhap.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbChiTIetPhieuNhap.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tbChiTIetPhieuNhap.Refresh();
        }

        private void tbChiTIetPhieuNhap_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            ctPhieuNhapDTO ctpn = tbChiTIetPhieuNhap.Rows[e.RowIndex].DataBoundItem as ctPhieuNhapDTO;
            if (tbChiTIetPhieuNhap.Columns[e.ColumnIndex].HeaderText == "Tên NL")
            {
                nguyenLieuDTO nl = dsNL.FirstOrDefault(x => x.MaNguyenLieu == ctpn.MaNguyenLieu);
                e.Value = nl?.TenNguyenLieu ?? "Không xác định";
            }
            if (tbChiTIetPhieuNhap.Columns[e.ColumnIndex].HeaderText == "Tên ĐV")
            {
                donViDTO dv = dsDV.FirstOrDefault(x => x.MaDonVi == ctpn.MaDonVi);
                e.Value = dv?.TenDonVi ?? "Không xác định";
            }
            if (tbChiTIetPhieuNhap.Columns[e.ColumnIndex].HeaderText == "Số lượng" && e.Value != null)
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
