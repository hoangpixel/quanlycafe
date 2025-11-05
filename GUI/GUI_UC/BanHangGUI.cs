using BUS;
using DTO;
using FONTS;
using System.IO;
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
    public partial class banHangGUI : UserControl
    {
        private BindingList<sanPhamDTO> danhSachSP;
        private BindingList<loaiDTO> dsLoai;
        private BindingList<nhomDTO> dsNhom;
        private BindingList<gioHangItemDTO> gioHang = new BindingList<gioHangItemDTO>();
        private sanPhamBUS busSanPham = new sanPhamBUS();
        private BindingList<hoaDonDTO> dsHoaDon;
        private hoaDonBUS busHoaDon = new hoaDonBUS();
        public banHangGUI()
        {
            InitializeComponent();
        }

        private void banHangGUI_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            dsLoai = new loaiSanPhamBUS().LayDanhSach();
            dsNhom = new nhomBUS().layDanhSach();

            busSanPham = new sanPhamBUS();
            dgvGioHang.DataSource = gioHang;
            LoadDanhSachSanPham();
            CapNhatGioHang();
            loadFontChuVaSizeGioHang();
        }
        private void loadFontChuVaSize()
        {
            foreach (DataGridViewColumn col in dgvSanPham.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            dgvSanPham.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvSanPham.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            dgvSanPham.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            dgvSanPham.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSanPham.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvSanPham.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgvSanPham.Refresh();
        }

        private void loadFontChuVaSizeGioHang()
        {
            foreach (DataGridViewColumn col in dgvGioHang.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            dgvGioHang.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvGioHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvGioHang.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            dgvGioHang.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            dgvGioHang.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGioHang.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvGioHang.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgvGioHang.Refresh();
        }
        private void LoadDanhSachLoaiVaNhom()
        {
            loaiSanPhamBUS loaiBus = new loaiSanPhamBUS();
            dsLoai = loaiBus.LayDanhSach();

            nhomBUS nhomBus = new nhomBUS();
            dsNhom = nhomBus.layDanhSach();
        }

        private void LoadDanhSachSanPham()
        {
            danhSachSP = busSanPham.LayDanhSach();

            if (danhSachSP == null || danhSachSP.Count == 0)
            {
                MessageBox.Show("Không có sản phẩm nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgvSanPham.AutoGenerateColumns = false;
            dgvSanPham.DataSource = null;
            dgvSanPham.DataSource = danhSachSP;

            // XÓA CỘT CŨ
            dgvSanPham.Columns.Clear();

            // THÊM CỘT
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSP", HeaderText = "Mã SP" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenSP", HeaderText = "Tên SP" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên loại" });   // Không DataPropertyName
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên nhóm" });   // Không DataPropertyName
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Gia",
                HeaderText = "Giá",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvSanPham.ReadOnly = true;
            dgvSanPham.ClearSelection();
            loadFontChuVaSize();
        }
        private void dgvSanPham_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            sanPhamDTO sp = dgvSanPham.Rows[e.RowIndex].DataBoundItem as sanPhamDTO;
            if (dgvSanPham.Columns[e.ColumnIndex].HeaderText == "Tên loại")
            {
                loaiDTO loai = dsLoai.FirstOrDefault(x => x.MaLoai == sp.MaLoai);
                e.Value = loai?.TenLoai ?? "Không xác định";
            }
            if (dgvSanPham.Columns[e.ColumnIndex].HeaderText == "Tên nhóm")
            {
                loaiDTO loai = dsLoai.FirstOrDefault(l => l.MaLoai == sp.MaLoai);
                nhomDTO nhom = dsNhom.FirstOrDefault(n => n.MaNhom == (loai != null ? loai.MaNhom : -1));
                e.Value = nhom?.TenNhom ?? "Không xác định";
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void homeGUI_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            
            FontManager.ApplyFontToAllControls(this);
        }

        private void bigLabel1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }
        private void button6_Click(object sender, EventArgs e)
        {
            using (var f = new FormChonBan())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    txtBan.Text = f.maBan;
                }
            }

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSP.Text) || !int.TryParse(txtMaSP.Text, out int maSP))
                return;

            var sp = danhSachSP.FirstOrDefault(s => s.MaSP == maSP);
            if (sp == null)
            {
                MessageBox.Show("Sản phẩm không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soLuong = (int)numSoLuong.Value;
            if (soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = gioHang.FirstOrDefault(i => i.SanPham.MaSP == sp.MaSP);
            if (item != null)
            {
                item.SoLuong += soLuong;
            }
            else
            {
                gioHang.Add(new gioHangItemDTO { SanPham = sp, SoLuong = soLuong });
            }

            CapNhatGioHang();
            loadFontChuVaSizeGioHang();
        }
        private void CapNhatGioHang()
        {
            dgvGioHang.Columns.Clear(); // XÓA CỘT CŨ (QUAN TRỌNG!)

            var data = gioHang.Select(g => new
            {
                MaSP = g.SanPham.MaSP,
                TenSP = g.SanPham.TenSP,
                SoLuong = g.SoLuong,
                DonGia = g.SanPham.Gia,
                ThanhTien = g.ThanhTien
            }).ToList();

            dgvGioHang.DataSource = data;

            if (dgvGioHang.Columns.Count >= 5)
            {
                var c = dgvGioHang.Columns;
                c[0].Name = c[0].HeaderText = "Mã SP";
                c[1].Name = c[1].HeaderText = "Tên sản phẩm";
                c[2].Name = c[2].HeaderText = "Số lượng";
                c[3].Name = c[3].HeaderText = "Đơn giá"; c[3].DefaultCellStyle.Format = "N0";
                c[4].Name = c[4].HeaderText = "Thành tiền"; c[4].DefaultCellStyle.Format = "N0";
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow != null)
            {
                // Lấy giá trị ô "MaSP" → chuyển thành int
                if (int.TryParse(dgvGioHang.CurrentRow.Cells["MaSP"].Value?.ToString(), out int maSP))
                {
                    gioHang.RemoveAt(0);
                    CapNhatGioHang();
                }
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xóa toàn bộ giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                gioHang.Clear();
                LoadDanhSachSanPham();
                CapNhatGioHang();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"[DEBUG] txtBan.Text = '{txtBan.Text}'");
            if (gioHang.Count == 0)
            {
                MessageBox.Show("Chưa có sản phẩm trong giỏ hàng!");
                return;
            }
            string maBan = txtBan.Text.Trim();
            if (string.IsNullOrWhiteSpace(maBan))
            {
                MessageBox.Show("Vui lòng nhập khu vực - bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBan.Focus();
                return;
            }
            gioHang.Clear();
            txtBan.Clear();
            CapNhatGioHang();
        }


        private void dgvSanPham_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvSanPham.ClearSelection();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            LoadDanhSachSanPham();
            LoadDanhSachLoaiVaNhom();
            CapNhatGioHang();
        }

        private int lastSelectedRowIndex = -1;

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= danhSachSP.Count)
                return;

            if (e.RowIndex == lastSelectedRowIndex)
            {
                dgvSanPham.ClearSelection();
                lastSelectedRowIndex = -1;

                txtMaSP.Clear();
                txtTenSP.Clear();
                txtLoaiSP.Clear();
                txtGia.Clear();
                picSanPham.Image = null;
                return;
            }

            lastSelectedRowIndex = e.RowIndex;
            var sp = danhSachSP[e.RowIndex];
            HienThiChiTietSanPham(sp);
        }
        private void HienThiChiTietSanPham(sanPhamDTO sp)
        {
            txtMaSP.Text = sp.MaSP.ToString();
            txtTenSP.Text = sp.TenSP;

            var loai = dsLoai?.FirstOrDefault(l => l.MaLoai == sp.MaLoai);
            txtLoaiSP.Text = loai?.TenLoai ?? "Chưa xác định";

            txtGia.Text = sp.Gia.ToString("N0");

            string imgPath = Path.Combine(Application.StartupPath, "IMG", "SP", sp.Hinh);


            if (File.Exists(imgPath))
            {
                picSanPham.Image = Image.FromFile(imgPath);
                picSanPham.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                picSanPham.Image = null;
                Console.WriteLine("Ảnh không tồn tại: " + imgPath);
            }
        }
    }
}
