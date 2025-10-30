using BUS;
using DTO;
using FONTS;
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

        public detailSanPham(sanPhamDTO ct)
        {
            InitializeComponent();
            this.ct = ct;
            //this.FormBorderStyle = FormBorderStyle.None;

        }

        private void loadFontChuVaSizeTableNguyenLieu()
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
            tableCongThuc.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            // --- Fix lỗi mất text khi đổi font ---
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

            txtTenSP.Text = ct.TenSP;
            txtMaSP.Text = ct.MaSP.ToString();
            txtGia.Text = ct.Gia.ToString("N0");
            txtLoaiSP.Text = ct.TenLoai;

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
            tableCongThuc.Columns.Clear();
            tableCongThuc.DataSource = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã NL");
            dt.Columns.Add("Tên NL");
            dt.Columns.Add("SL");
            dt.Columns.Add("Đơn vị");


            nguyenLieuBUS nlBUS = new nguyenLieuBUS();
            BindingList<nguyenLieuDTO> dsNL= nlBUS.LayDanhSach();

            donViBUS dvBUS = new donViBUS();
            BindingList<donViDTO> dsDV = dvBUS.LayDanhSach();
            foreach (var sp in ds)
            {
                string tenLoai = dsNL.FirstOrDefault(l => l.MaNguyenLieu == sp.MaNguyenLieu)?.TenNguyenLieu ?? "Không xác định";
                string tenDonVI = dsDV.FirstOrDefault(l => l.MaDonVi == sp.MaDonViCoSo)?.TenDonVi ?? "Không xác định";

                dt.Rows.Add(sp.MaNguyenLieu, tenLoai, sp.SoLuongCoSo,tenDonVI);
            }

            tableCongThuc.DataSource = dt;
            loadFontChuVaSizeTableNguyenLieu();
            tableCongThuc.ReadOnly = true;
            tableCongThuc.RowHeadersVisible = false;
            tableCongThuc.Columns["Mã NL"].Width = 80;
            tableCongThuc.Columns["Tên NL"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tableCongThuc.Columns["SL"].Width = 60;
            tableCongThuc.Columns["Đơn vị"].Width = 80;
            tableCongThuc.ClearSelection();

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
