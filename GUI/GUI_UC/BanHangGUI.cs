using BUS;
using DTO;
using FONTS;
using System.IO;
using GUI.GUI_CRUD;
using GUI.GUI_SELECT;
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
        private BindingList<cthoaDonDTO> gioHang = new BindingList<cthoaDonDTO>();
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
            LoadDanhSachHoaDon();
            loadFontChuVaSizeHoaDon();
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

        private void loadFontChuVaSizeHoaDon()
        {
            foreach (DataGridViewColumn col in dgvHoaDon.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            dgvHoaDon.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            dgvHoaDon.DefaultCellStyle.Font = FontManager.GetLightFont(10);
            
            dgvHoaDon.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);
            
            dgvHoaDon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHoaDon.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvHoaDon.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            
            dgvHoaDon.Refresh();
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
            btThemSP.Enabled = false;
            btnXoaSP.Enabled = false;
            button2.Enabled = false;

            dgvSanPham.ReadOnly = true;
            dgvSanPham.ClearSelection();
            loadFontChuVaSize();
            numSoLuong.Visible = false;
        }

        private void LoadDanhSachHoaDon()
        {
            
            try
            {
                dsHoaDon = busHoaDon.LayDanhSach();

                dgvHoaDon.Columns.Clear();      // XÓA CỘT CŨ
                dgvHoaDon.AutoGenerateColumns = false; // TẮT auto để tự định nghĩa

                dgvHoaDon.DataSource = null;
                dgvHoaDon.DataSource = dsHoaDon;

                // === TẠO CỘT ===
                dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "MaHD",
                    HeaderText = "Mã HD"
                });

                dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "MaBan",
                    HeaderText = "Bàn"
                });

                dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ThoiGianTao",
                    HeaderText = "Thời gian",
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "HH:mm dd/MM" }
                });

                /*dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "TrangThai",
                    HeaderText = "Trạng thái"
                });*/

                dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "TongTien",
                    HeaderText = "Tổng tiền",
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
                });

                dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                Console.WriteLine($"[LOAD] Đã tải {dsHoaDon.Count} hóa đơn.");

                AnNutHoaDon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách hóa đơn: " + ex.Message);
            }
            
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
        private void btnChonBan_Click(object sender, EventArgs e)
        {
            using (var f = new FormChonBan())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    int maBanChon = f.maBan;

                    txtBan.Text = maBanChon.ToString();
                    txtBan.Tag = maBanChon;

                    // TỰ ĐỘNG LOAD LẠI DANH SÁCH HÓA ĐƠN ĐỂ HIỂN THỊ BÀN MỚI TẠO
                    LoadDanhSachHoaDon();
                }
            }

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btThemSP_Click(object sender, EventArgs e)
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

            // Tìm trong giỏ theo MaSP
            var item = gioHang.FirstOrDefault(i => i.MaSP == sp.MaSP);
            if (item != null)
            {
                item.SoLuong += soLuong;
                item.ThanhTien = item.SoLuong * item.DonGia;
            }
            else
            {
                gioHang.Add(new cthoaDonDTO
                {
                    MaSP = sp.MaSP,
                    SoLuong = soLuong,
                    DonGia = (decimal)sp.Gia,
                    ThanhTien = soLuong * (decimal)sp.Gia
                });

            }

            CapNhatGioHang();
            loadFontChuVaSizeGioHang();
            dgvGioHang.ClearSelection();
        }
        private void CapNhatGioHang()
        {
            var data = gioHang.Select(g =>
            {
                var sp = danhSachSP.FirstOrDefault(s => s.MaSP == g.MaSP);
                return new
                {
                    MaSP = g.MaSP,
                    TenSP = sp?.TenSP ?? "",
                    SoLuong = g.SoLuong,
                    DonGia = g.DonGia,
                    ThanhTien = g.ThanhTien
                };
            }).ToList();

            dgvGioHang.DataSource = null;
            dgvGioHang.DataSource = data;

            if (dgvGioHang.Columns.Count >= 5 && dgvGioHang.Columns[0].HeaderText != "Mã SP")
            {
                var c = dgvGioHang.Columns;
                c[0].Name = c[0].HeaderText = "Mã SP";
                c[1].Name = c[1].HeaderText = "Tên sản phẩm";
                c[2].Name = c[2].HeaderText = "Số lượng";
                c[3].Name = c[3].HeaderText = "Đơn giá"; c[3].DefaultCellStyle.Format = "N0";
                c[4].Name = c[4].HeaderText = "Thành tiền"; c[4].DefaultCellStyle.Format = "N0";
            }
        }

        private void btnXoaSP_Click(object sender, EventArgs e)
        {
    if (dgvGioHang.CurrentRow != null)
    {
        if (int.TryParse(dgvGioHang.CurrentRow.Cells["Mã SP"].Value?.ToString(), out int maSP))
        {
            var item = gioHang.FirstOrDefault(g => g.MaSP == maSP);
            if (item != null)
            {
                gioHang.Remove(item);
                CapNhatGioHang();
            }
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

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"[DEBUG] txtBan.Text = '{txtBan.Text}'");
            if (gioHang.Count == 0)
            {
                MessageBox.Show("Chưa có sản phẩm trong giỏ hàng!");
                return;
            }
            if (txtBan.Tag == null || !(txtBan.Tag is int maBan))
            {
                MessageBox.Show("Vui lòng chọn bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // TÍNH TỔNG TIỀN
            decimal tongTien = gioHang.Sum(g => g.ThanhTien);

            // HIỂN THỊ HỘP THOẠI XÁC NHẬN
            string message = $@"XÁC NHẬN HÓA ĐƠN

            Bàn: {maBan}
            Sản phẩm: {gioHang.Count} món
            Tổng tiền: {tongTien:N0} VNĐ

            Bạn có muốn tạo hóa đơn không?";

            var result = MessageBox.Show(message, "Xác nhận tạo hóa đơn",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                MessageBox.Show("Đã hủy tạo hóa đơn. Bạn có thể chỉnh sửa giỏ hàng.",
                    "Hủy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // TẠO HÓA ĐƠN
            var hd = new hoaDonDTO
            {   
                MaBan = maBan,
                TongTien = tongTien,
                MaNhanVien = 1,     // Mặc định
                MaTT = 1,
                MaKhachHang = 1
            };

            int maHD = busHoaDon.ThemHoaDon(hd, gioHang);

            if (maHD > 0)
            {
                MessageBox.Show($"Tạo hóa đơn thành công!\nMã HD: {maHD}\nBàn: {maBan}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // XÓA GIỎ HÀNG + CẬP NHẬT
                gioHang.Clear();
                CapNhatGioHang();
                txtBan.Clear();

                // CHUYỂN TAB + TẢI LẠI DANH SÁCH
                tabControl1.SelectedTab = tabHoaDon;
                LoadDanhSachHoaDon(); // ĐẢM BẢO TẢI LẠI
            }
            else
            {
                MessageBox.Show("Lỗi tạo hóa đơn! Vui lòng thử lại.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is FormChonBan chonBanForm)
                {
                    chonBanForm.RefreshBan(); // hàm public trong FormChonBan
                }
            }
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
            if (dgvSanPham.CurrentRow != null)
            {
                btThemSP.Enabled = true;
                btnXoaSP.Enabled = true;
                button2.Enabled = true;

                numSoLuong.Enabled = true;
                numSoLuong.Visible = true;

                // Chỉ set lại khi người dùng đang chọn SP mới
                numSoLuong.Value = 0;
            }
            else
            {
                numSoLuong.Enabled = false;
                numSoLuong.Visible = false;

                btThemSP.Enabled = false;
                btnXoaSP.Enabled = false;
                button2.Enabled = false;
            }
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

        private void dgvHoaDon_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow != null && dgvHoaDon.CurrentRow.Index < dsHoaDon.Count)
            {
                btnChiTietHD.Enabled = btnTinhTien.Enabled = true;

                var hd = dsHoaDon[dgvHoaDon.CurrentRow.Index];

                // TrangThai là bool
                btnTinhTien.Enabled = hd.TrangThai;   // true = cho thanh toán, false = không
                btnSuaHD.Enabled = true;
                btnXoaHD.Enabled = true;
            }
            else
            {
                AnNutHoaDon();
            }
        }
        private void AnNutHoaDon()
        {
            btnChiTietHD.Enabled = btnTinhTien.Enabled = btnXoaHD.Enabled = false;
        }

        private void btnChiTietHD_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null) return;
            int maHD = dsHoaDon[dgvHoaDon.CurrentRow.Index].MaHD;
            var frm = new frmChiTietHD() ;
            frm.ShowDialog();
        }

        private void btnRefreshSP_Click(object sender, EventArgs e)
        {
            LoadDanhSachHoaDon();
        }

        private void dgvGioHang_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dsHoaDon.Count)
                return;

            if (e.RowIndex == lastSelectedRowIndex)
            {
                dgvHoaDon.ClearSelection();
                lastSelectedRowIndex = -1;
                return;
            }
        }

        private void btnChonKH_Click(object sender, EventArgs e)
        {
            using (var f = new FormchonKH())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    txtKhachHang.Text = f.TenKHChon;
                    txtKhachHang.Tag = f.MaKHChon; // lưu mã khách hàng
                }
            }
        }

        private void btnChonNV_Click(object sender, EventArgs e)
        {
            using (var f = new FormchonNV())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    txtNhanVien.Text = f.TenNVChon;
                    txtNhanVien.Tag = f.MaNVChon; // lưu mã khách hàng
                }
            }
        }

        private void dgvHoaDon_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvHoaDon.ClearSelection();
        }
    }
}
