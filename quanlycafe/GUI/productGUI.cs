using quanlycafe.BUS;
using quanlycafe.DAO;
using quanlycafe.DTO;
using quanlycafe.GUI_CRUD;
using ReaLTaiizor.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace quanlycafe.GUI
{
    public partial class productGUI : UserControl
    {
        bool isSanPhamLoaded = false;
        bool isNguyenLieuLoaded = false;
        bool isCongThucLoaded = false;
        private int lastSelectedRowSanPham = -1;
        private int lastSelectedRowNguyenLieu = -1;

        public productGUI()
        {
            InitializeComponent();
        }

        private void loadDanhSachSanPham(List<sanPhamDTO> ds)
        {
            tbSanPham.Columns.Clear();
            tbSanPham.DataSource = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã SP");
            dt.Columns.Add("Tên Loại");
            dt.Columns.Add("Tên SP");
            dt.Columns.Add("Trạng thái SP");
            dt.Columns.Add("Trạng thái CT");
            dt.Columns.Add("Hình");
            dt.Columns.Add("Giá");

            loaiSanPhamBUS loaiBus = new loaiSanPhamBUS();
            List<loaiDTO> dsLoai = loaiBus.layDanhSachLoai();

            foreach (var sp in ds)
            {
                string tenLoai = dsLoai.FirstOrDefault(l => l.MaLoai == sp.MaLoai)?.TenLoai ?? "Không xác định";
                string trangThai = sp.TrangThai == 1 ? "Đang bán" : "Ngừng bán";
                string trangThaiCT = sp.TrangThaiCT == 1 ? "Đã có công thức" : "Chưa có công thức";
                dt.Rows.Add(sp.MaSP, tenLoai, sp.TenSP, trangThai, trangThaiCT, sp.Hinh, string.Format("{0:N0}", sp.Gia));
            }

            tbSanPham.DataSource = dt;
            tbSanPham.ReadOnly = true;

            tbSanPham.ClearSelection();

            btnSuaSP.Enabled = false;
            btnXoaSP.Enabled = false;
            btnChiTiet.Enabled = false;
        }

        public void loadDanhSachNguyenLieu(List<nguyenLieuDTO> ds)
        {
            tableNguyenLieu.Columns.Clear();
            tableNguyenLieu.DataSource = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("Mã NL");
            dt.Columns.Add("Tên NL");
            dt.Columns.Add("Đơn vị cơ sở");
            dt.Columns.Add("Trạng thái");
            dt.Columns.Add("Tồn kho");

            foreach (var nl in ds)
            {
                string trangThai = nl.TrangThai == 1 ? "Còn hoạt động" : "Ngừng bán";
                dt.Rows.Add(nl.MaNguyenLieu, nl.TenNguyenLieu, nl.DonViCoSo, trangThai, string.Format("{0:N0}",nl.TonKho));
            }

            tableNguyenLieu.DataSource = dt;
            tableNguyenLieu.ReadOnly = true;

            tableNguyenLieu.ClearSelection();

            btnSuaNL.Enabled = false;
            btnXoaNL.Enabled = false;
            btnChiTietNL.Enabled = false;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        // load sản phẩm
        private void productGUI_Load(object sender, EventArgs e)
        {
            SanPhamBUS bus = new SanPhamBUS();
            bus.docDSSanPham();
            loadDanhSachSanPham(SanPhamBUS.ds);
            tbSanPham.ClearSelection();
        }




        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            using (insertProduct form = new insertProduct())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
                SanPhamBUS bus = new SanPhamBUS();
                bus.docDSSanPham();
                loadDanhSachSanPham(SanPhamBUS.ds);
            }
        }

        private void btnSuaSP_Click(object sender, EventArgs e)
        {
            if (tbSanPham.SelectedRows.Count > 0)
            {
                // Lấy dòng đang chọn
                DataGridViewRow row = tbSanPham.SelectedRows[0];

                // Tạo đối tượng sanPhamDTO từ dữ liệu dòng
                sanPhamDTO sp = new sanPhamDTO
                {
                    MaSP = Convert.ToInt32(row.Cells["Mã SP"].Value),
                    MaLoai = new loaiSanPhamBUS().layDanhSachLoai()
                                .FirstOrDefault(l => l.TenLoai == row.Cells["Tên Loại"].Value.ToString())?.MaLoai ?? 0,
                    TenSP = row.Cells["Tên SP"].Value.ToString(),
                    Hinh = row.Cells["Hình"].Value.ToString(),
                    Gia = float.Parse(row.Cells["Giá"].Value.ToString()),
                    TrangThai = row.Cells["Trạng thái SP"].Value.ToString() == "Đang bán" ? 1 : 0,
                    TrangThaiCT = row.Cells["Trạng thái CT"].Value.ToString() == "Đã có công thức" ? 1 : 0
                };

                // Mở form updateProduct và truyền dữ liệu
                using (updateProduct form = new updateProduct(sp))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }

                // Sau khi đóng form update → reload lại table
                SanPhamBUS bus = new SanPhamBUS();
                bus.docDSSanPham();
                loadDanhSachSanPham(SanPhamBUS.ds);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            if (tbSanPham.SelectedRows.Count > 0)
            {
                int maSP = Convert.ToInt32(tbSanPham.SelectedRows[0].Cells["Mã SP"].Value);
                deleteProduct form = new deleteProduct(maSP);
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();

                SanPhamBUS bus = new SanPhamBUS();
                bus.docDSSanPham();
                loadDanhSachSanPham(SanPhamBUS.ds);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ktraTableSanPham(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua header
            if (e.RowIndex < 0) return;

            // Nếu người dùng click lại dòng đang chọn → bỏ chọn
            if (e.RowIndex == lastSelectedRowSanPham)
            {
                tbSanPham.ClearSelection();
                lastSelectedRowSanPham = -1;

                // Disable các nút khi không có dòng nào được chọn
                btnThemSP.Enabled = true;   // vẫn cho thêm
                btnSuaSP.Enabled = false;
                btnXoaSP.Enabled = false;
                btnChiTiet.Enabled = false;
                return;
            }

            // Nếu click dòng khác → chọn dòng đó
            tbSanPham.ClearSelection();
            tbSanPham.Rows[e.RowIndex].Selected = true;
            lastSelectedRowSanPham = e.RowIndex;

            // Kiểm tra trạng thái sản phẩm
            string trangThai = tbSanPham.Rows[e.RowIndex].Cells["Trạng thái SP"].Value.ToString();
            bool isDisabled = trangThai.Equals("Ngừng bán", StringComparison.OrdinalIgnoreCase);

            // Nếu “Ngừng bán” → disable các nút, ngược lại thì enable
            //btnThem.Enabled = !isDisabled;
            btnSuaSP.Enabled = !isDisabled;
            btnXoaSP.Enabled = !isDisabled;
            btnChiTiet.Enabled = true;
        }

        private void kiemTraTabNaoDcLoad(object sender, EventArgs e)
        {
            // tabPage1 = sản phẩm
            if (tabControl1.SelectedTab == tabSanPham && !isSanPhamLoaded)
            {
                SanPhamBUS bus = new SanPhamBUS();
                bus.docDSSanPham();
                loadDanhSachSanPham(SanPhamBUS.ds);
                isSanPhamLoaded = true;
            }

            // tabPage2 = nguyên liệu
            else if (tabControl1.SelectedTab == tabNguyenLieu && !isNguyenLieuLoaded)
            {
                nguyenLieuBUS bus = new nguyenLieuBUS();
                bus.napDSNguyenLieu();
                loadDanhSachNguyenLieu(nguyenLieuBUS.ds);
                isNguyenLieuLoaded = true;
            }

            // tabPage3 = công thức
            else if (tabControl1.SelectedTab == tabCongThuc && !isCongThucLoaded)
            {
                //loadDanhSachCongThuc();
                isCongThucLoaded = true;
            }
        }

        private void btnReFresh_Click(object sender, EventArgs e)
        {
            SanPhamBUS bus = new SanPhamBUS();
            bus.docDSSanPham();
            loadDanhSachSanPham(SanPhamBUS.ds);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            using (loaiSPGUI form = new loaiSPGUI())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
        }

        private void btnThemNL_Click(object sender, EventArgs e)
        {
            using (insertNguyenLieu form = new insertNguyenLieu())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
                nguyenLieuBUS bus = new nguyenLieuBUS();
                bus.napDSNguyenLieu();
                loadDanhSachNguyenLieu(nguyenLieuBUS.ds);
            }
        }

        private void tableNguyenLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua header
            if (e.RowIndex < 0) return;

            // Nếu người dùng click lại dòng đang chọn → bỏ chọn
            if (e.RowIndex == lastSelectedRowNguyenLieu)
            {
                tableNguyenLieu.ClearSelection();
                lastSelectedRowNguyenLieu = -1;

                // Disable các nút khi không có dòng nào được chọn
                btnThemNL.Enabled = true;   // vẫn cho thêm
                btnSuaNL.Enabled = false;
                btnXoaNL.Enabled = false;
                btnChiTietNL.Enabled = false;
                return;
            }

            // Nếu click dòng khác → chọn dòng đó
            tableNguyenLieu.ClearSelection();
            tableNguyenLieu.Rows[e.RowIndex].Selected = true;
            lastSelectedRowNguyenLieu = e.RowIndex;

            // Vì đã lọc bỏ "Ngừng bán" nên không cần kiểm tra trạng thái nữa
            btnSuaNL.Enabled = true;
            btnXoaNL.Enabled = true;
            btnChiTietNL.Enabled = true;
        }

        private void btnSuaNL_Click(object sender, EventArgs e)
        {
            if (tableNguyenLieu.SelectedRows.Count > 0)
            {
                DataGridViewRow row = tableNguyenLieu.SelectedRows[0];
                nguyenLieuDTO nl = new nguyenLieuDTO
                {
                    MaNguyenLieu = Convert.ToInt32(row.Cells["Mã NL"].Value),
                    TenNguyenLieu = row.Cells["Tên NL"].Value.ToString(),
                    DonViCoSo = row.Cells["Đơn vị cơ sở"].Value.ToString(),
                    TrangThai = 1 
                };

                using (updateNguyenLieu form = new updateNguyenLieu(nl))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }

                nguyenLieuBUS bus = new nguyenLieuBUS();
                bus.docDSNguyenLieu();
                loadDanhSachNguyenLieu(nguyenLieuBUS.ds);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnXoaNL_Click(object sender, EventArgs e)
        {
            if (tableNguyenLieu.SelectedRows.Count > 0)
            {
                int maNL = Convert.ToInt32(tableNguyenLieu.SelectedRows[0].Cells["Mã NL"].Value);
                deleteNguyenLieu form = new deleteNguyenLieu(maNL);
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();

                nguyenLieuBUS bus = new nguyenLieuBUS();
                bus.docDSNguyenLieu();
                loadDanhSachNguyenLieu(nguyenLieuBUS.ds);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRefreshNL_Click(object sender, EventArgs e)
        {
            nguyenLieuBUS bus = new nguyenLieuBUS();
            bus.docDSNguyenLieu();
            loadDanhSachNguyenLieu(nguyenLieuBUS.ds);
        }
    }
}
