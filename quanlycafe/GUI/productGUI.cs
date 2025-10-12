using quanlycafe.BUS;
using quanlycafe.DTO;
using quanlycafe.GUI_CRUD;
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
        public productGUI()
        {
            InitializeComponent();
        }

        private void loadDanhSachTable(List<sanPhamDTO> ds)
        {
            tableSanPham.Columns.Clear();
            tableSanPham.DataSource = null;

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
                dt.Rows.Add(sp.MaSP, tenLoai, sp.TenSP, trangThai, trangThaiCT, sp.Hinh, sp.Gia);
            }

            tableSanPham.DataSource = dt;
            tableSanPham.ReadOnly = true;


            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnChiTiet.Enabled = false;
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

        // Thêm sản phẩm
        private void button5_Click(object sender, EventArgs e)
        {
            using (insertProduct form = new insertProduct())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }

            SanPhamBUS bus = new SanPhamBUS();
            bus.docDSSanPham();
            loadDanhSachTable(SanPhamBUS.ds);
        }

        // load sản phẩm
        private void productGUI_Load(object sender, EventArgs e)
        {
            SanPhamBUS bus = new SanPhamBUS();
            bus.docDSSanPham();
            loadDanhSachTable(SanPhamBUS.ds);
        }

        // xóa sản phẩm
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if(tableSanPham.SelectedRows.Count > 0)
            {
                int maSP = Convert.ToInt32(tableSanPham.SelectedRows[0].Cells["Mã SP"].Value);
                deleteProduct form = new deleteProduct(maSP);
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();

                SanPhamBUS bus = new SanPhamBUS();
                bus.docDSSanPham();
                loadDanhSachTable(SanPhamBUS.ds);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        // kiểm tra xem sản phẩm nào ngừng bán thì disable button sủa xóa
        private int lastSelectedRow = -1;
        private void kiemTraNgungBan(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua header
            if (e.RowIndex < 0) return;

            // Nếu người dùng click lại dòng đang chọn → bỏ chọn
            if (e.RowIndex == lastSelectedRow)
            {
                tableSanPham.ClearSelection();
                lastSelectedRow = -1;

                // Disable các nút khi không có dòng nào được chọn
                btnThem.Enabled = true;   // vẫn cho thêm
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnChiTiet.Enabled = false;
                return;
            }

            // Nếu click dòng khác → chọn dòng đó
            tableSanPham.ClearSelection();
            tableSanPham.Rows[e.RowIndex].Selected = true;
            lastSelectedRow = e.RowIndex;

            // Kiểm tra trạng thái sản phẩm
            string trangThai = tableSanPham.Rows[e.RowIndex].Cells["Trạng thái SP"].Value.ToString();
            bool isDisabled = trangThai.Equals("Ngừng bán", StringComparison.OrdinalIgnoreCase);

            // Nếu “Ngừng bán” → disable các nút, ngược lại thì enable
            //btnThem.Enabled = !isDisabled;
            btnSua.Enabled = !isDisabled;
            btnXoa.Enabled = !isDisabled;
            btnChiTiet.Enabled = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (tableSanPham.SelectedRows.Count > 0)
            {
                // Lấy dòng đang chọn
                DataGridViewRow row = tableSanPham.SelectedRows[0];

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
                loadDanhSachTable(SanPhamBUS.ds);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


    }
}
