using quanlycafe.BUS;
using quanlycafe.DTO;
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

namespace quanlycafe.GUI_CRUD
{
    public partial class detailSanPham : Form
    {
        private sanPhamDTO ct;

        public detailSanPham(sanPhamDTO ct)
        {
            InitializeComponent();
            this.ct = ct;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void detailSanPham_Load(object sender, EventArgs e)
        {
            txtTenSP.Text = ct.TenSP;
            txtMaSP.Text = ct.MaSP.ToString();
            txtGia.Text = ct.Gia.ToString();
            txtLoaiSP.Text = ct.TenLoai;

            string imgPath = Path.Combine(Application.StartupPath, "IMG", ct.Hinh);


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
            var ds = bus.docDSCongThucTheoSP(ct.MaSP);
            loadDanhSachCongThuc(ds);
        }

        private void loadDanhSachCongThuc(List<congThucDTO> ds)
        {
            tableCongThuc.Columns.Clear();
            tableCongThuc.DataSource = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã NL");
            dt.Columns.Add("Tên NL");
            dt.Columns.Add("SL");


            nguyenLieuBUS nlBUS = new nguyenLieuBUS();
            List<nguyenLieuDTO> dsNL= nlBUS.docDSNguyenLieu();
            foreach (var sp in ds)
            {
                string tenLoai = dsNL.FirstOrDefault(l => l.MaNguyenLieu == sp.MaNguyenLieu)?.TenNguyenLieu ?? "Không xác định";
                dt.Rows.Add(sp.MaNguyenLieu, tenLoai, sp.SoLuongCoSo);
            }

            tableCongThuc.DataSource = dt;
            tableCongThuc.ReadOnly = true;
            tableCongThuc.RowHeadersVisible = false;
            tableCongThuc.Columns["Mã NL"].Width = 80;
            tableCongThuc.Columns["Tên NL"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tableCongThuc.Columns["SL"].Width = 60;
            tableCongThuc.ClearSelection();

        }

        private void tableCongThuc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
