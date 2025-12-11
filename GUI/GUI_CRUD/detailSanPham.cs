using BUS;
using DTO;
using FONTS;
using OfficeOpenXml.Drawing.Vml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class detailSanPham : Form
    {
        private sanPhamDTO ct;
        private BindingList<nguyenLieuDTO> dsNguyenLieu;
        private BindingList<donViDTO> dsDonVi;
        public detailSanPham(sanPhamDTO ct)
        {
            InitializeComponent();
            this.ct = ct;
            //this.FormBorderStyle = FormBorderStyle.None;

        }

        private void loadFontChuVaSizeTableNguyenLieu()
        {
            foreach (DataGridViewColumn col in tableCongThuc.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tableCongThuc.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableCongThuc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tableCongThuc.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tableCongThuc.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            tableCongThuc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableCongThuc.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableCongThuc.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tableCongThuc.Refresh();
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void detailSanPham_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            dsNguyenLieu = new nguyenLieuBUS().LayDanhSach();

            dsDonVi = new donViBUS().LayDanhSach();

            txtTenSP.Text = ct.TenSP;
            txtMaSP.Text = ct.MaSP.ToString();
            txtGia.Text = ct.Gia.ToString("N0");

            loaiSanPhamBUS busLoai = new loaiSanPhamBUS();
            BindingList<loaiDTO> dsLoai = busLoai.LayDanhSach();

            nhomBUS nhomBus = new nhomBUS();
            BindingList<nhomDTO> dsNhom = nhomBus.layDanhSach();

            loaiDTO loai = dsLoai.FirstOrDefault(x => x.MaLoai == ct.MaLoai);
            nhomDTO nhom = dsNhom.FirstOrDefault(x => x.MaNhom == (loai != null ? loai.MaNhom : -1));

            txtLoaiSP.Text = loai?.TenLoai ?? "Không xác định";

            string imgPath = Path.Combine(Application.StartupPath, "IMG","SP", ct.Hinh);


            if (File.Exists(imgPath))
            {
                picHinhSPCT.Image = Image.FromFile(imgPath);
                picHinhSPCT.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                picHinhSPCT.Image = null;
                Console.WriteLine("Ảnh không tồn tại: " + imgPath);
            }

            congThucBUS bus = new congThucBUS();
            BindingList<congThucDTO> ds = bus.docDSCongThucTheoSP(ct.MaSP);
            loadDanhSachCongThuc(ds);
        }

        private void loadDanhSachCongThuc(BindingList<congThucDTO> ds)
        {
            tableCongThuc.AutoGenerateColumns = false;
            tableCongThuc.Columns.Clear();
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn {DataPropertyName = "MaNguyenLieu", HeaderText = "Mã NL"});
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Tên NL" });
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuongCoSo", HeaderText = "Số lượng" });
            tableCongThuc.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonViCoSo", HeaderText = "Đơn vị" });
            tableCongThuc.DataSource = ds;
            tableCongThuc.ReadOnly = true;
            tableCongThuc.ClearSelection();
            loadFontChuVaSizeTableNguyenLieu();
        }

        private void tableCongThuc_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(e.RowIndex < 0)
            {
                return;
            }
            congThucDTO ct = tableCongThuc.Rows[e.RowIndex].DataBoundItem as congThucDTO;
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
            if (tableCongThuc.Columns[e.ColumnIndex].HeaderText == "Số lượng" && e.Value != null)
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

        private void tableCongThuc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
