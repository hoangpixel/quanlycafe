using BUS;
using DAO;
using DTO;
using FONTS;
using GUI.GUI_CRUD;
using GUI.GUI_SELECT;
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
        private hoaDonDAO hoaDonDAO = new hoaDonDAO();

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
            LoadDanhSachSanPham();
            CapNhatGioHang();
            loadFontChuVaSizeGioHang();
            LoadDanhSachHoaDon();
            loadFontChuVaSizeHoaDon();
            CapNhatTrangThaiNutHoaDon();
            AnNutHoaDon();

            BindingList<ppThanhToanDTO> dsThanhToan = new ppThanhToanBUS().LayDanhSach();
            cbThanhToan.DataSource = dsThanhToan;
            cbThanhToan.DisplayMember = "HinhThuc";
            cbThanhToan.ValueMember = "MaTT";
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
                dsHoaDon = busHoaDon.LayDanhSach(); // Lấy hết từ DB

                // Gán DataSource
                dgvHoaDon.AutoGenerateColumns = false;
                dgvHoaDon.DataSource = null; // reset
                dgvHoaDon.DataSource = dsHoaDon;

                // Tạo lại cột (nếu chưa có)
                if (dgvHoaDon.Columns.Count == 0)
                {
                    dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "MaHD",
                        HeaderText = "Mã HD",
                        Width = 80
                    });
                    dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "MaBan",
                        HeaderText = "Bàn",
                        Width = 60
                    });
                    dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "ThoiGianTao",
                        HeaderText = "Thời gian",
                        DefaultCellStyle = new DataGridViewCellStyle { Format = "HH:mm dd/MM" },
                        Width = 120
                    });
                    dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "TongTien",
                        HeaderText = "Tổng tiền",
                        DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" },
                        Width = 100
                    });
                }

                dgvHoaDon.ClearSelection();
                dgvHoaDon.CurrentCell = null; // cực kỳ quan trọng!

                this.BeginInvoke((MethodInvoker)delegate
                {
                    dgvHoaDon.ClearSelection();
                    dgvHoaDon.CurrentCell = null;
                    CapNhatTrangThaiNutHoaDon(); // tắt hết nút
                });

            AnNutHoaDon();
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
                dgvSanPham.ClearSelection();
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
            if (dgvGioHang.CurrentRow == null || dgvGioHang.CurrentRow.Index < 0) return;

            int index = dgvGioHang.CurrentRow.Index;
            if (index >= gioHang.Count) return;

            if (MessageBox.Show("Xóa món này khỏi giỏ hàng?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                gioHang.RemoveAt(index);
                CapNhatGioHang();
                numSoLuong.Value = 0;
                numSoLuong.Visible = false;
                dgvGioHang.ClearSelection();
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

            // 1. Kiểm tra giỏ hàng
            if (gioHang.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống! Vui lòng thêm món ăn.", "Chưa có món",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Kiểm tra bàn
            if (txtBan.Tag == null || !int.TryParse(txtBan.Tag.ToString(), out int maBan))
            {
                MessageBox.Show("Vui lòng chọn bàn hợp lệ!", "Lỗi bàn",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Kiểm tra khách hàng (có thể là khách lẻ → cho phép null)
            int? maKH = null;
            if (txtKhachHang.Tag != null && int.TryParse(txtKhachHang.Tag.ToString(), out int kh))
                maKH = kh;

            // 4. Kiểm tra nhân viên (bắt buộc)
            if (txtNhanVien.Tag == null || !int.TryParse(txtNhanVien.Tag.ToString(), out int maNV))
            {
                MessageBox.Show("Vui lòng chọn nhân viên phục vụ!", "Lỗi nhân viên",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int maTT = Convert.ToInt32(cbThanhToan.SelectedValue);
            // 5. Tính tổng tiền
            decimal tongTien = gioHang.Sum(g => g.ThanhTien);

            // 6. Xác nhận tạo hóa đơn (hộp thoại đẹp)
            string message = $@"XÁC NHẬN TẠO HÓA ĐƠN
            ────────────────────────────
            Bàn số: {maBan}
            Khách hàng: {(maKH.HasValue ? txtKhachHang.Text : "Khách lẻ")}
            Nhân viên: {txtNhanVien.Text}
            Số món: {gioHang.Count}
            PPTT: {maTT}
            Tổng tiền: {tongTien:N0} VNĐ
            ────────────────────────────
            Bạn có chắc chắn muốn tạo hóa đơn không?";

            var result = MessageBox.Show(message, "Xác nhận tạo hóa đơn",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                        MessageBoxDefaultButton.Button2);

            if (result != DialogResult.Yes)
            {
                MessageBox.Show("Đã hủy tạo hóa đơn. Bạn có thể tiếp tục chỉnh sửa.", "Hủy bỏ",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }



            // 7. Tạo hóa đơn DTO
            var hd = new hoaDonDTO
            {
                MaBan = maBan,
                MaKhachHang = maKH ??0,           // có thể null → khách lẻ
                MaNhanVien = maNV,
                MaTT = maTT,                     // mặc định hình thức thanh toán tiền mặt
                ThoiGianTao = DateTime.Now,
                TrangThai = true             // chưa thanh toán
            };

            // 8. Gọi BUS thêm hóa đơn
            int maHD = busHoaDon.ThemHoaDon(hd, gioHang);

            if (maHD > 0)
            {
                MessageBox.Show($@"TẠO HÓA ĐƠN THÀNH CÔNG!
                Mã hóa đơn: HD{maHD}
                Bàn: {maBan}
                Tổng tiền: {tongTien:N0} VNĐ
                Nhân viên: {txtNhanVien.Text}",
                                "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 9. Dọn dẹp giao diện
                gioHang.Clear();
                CapNhatGioHang();
                txtBan.Clear();
                txtBan.Tag = null;
                txtKhachHang.Clear(); txtKhachHang.Tag = null;
                txtNhanVien.Clear(); txtNhanVien.Tag = null;

                // 10. Chuyển sang tab hóa đơn + reload
                tabControl1.SelectedTab = tabHoaDon;
                //LoadDanhSachHoaDon();

                // 11. Cập nhật trạng thái bàn (đổi màu thành đang sử dụng)
                //CapNhatTrangThaiBan(maBan, dangSuDung: true);
            }
            else
            {
                MessageBox.Show("Tạo hóa đơn thất bại! Vui lòng thử lại.", "Lỗi hệ thống",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            CapNhatTrangThaiNutHoaDon();
        }
        private void AnNutHoaDon()
        {
            btnChiTietHD.Enabled = btnDonBan.Enabled = btnXoaHD.Enabled = btnSuaHD.Enabled = false;
        }

        private void btnChiTietHD_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null) return;
            int maHD = dsHoaDon[dgvHoaDon.CurrentRow.Index].MaHD;
            if (dgvHoaDon.CurrentRow == null) return;

            var frm = new frmChiTietHD(maHD);  // TRUYỀN MÃ HÓA ĐƠN VÀO ĐÂY!!!
            frm.ShowDialog();
        }

        private void btnRefreshSP_Click(object sender, EventArgs e)
        {
            LoadDanhSachHoaDon();
        }

        private void dgvGioHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= gioHang.Count)
            {
                // Không có dòng hợp lệ
                btThemSP.Enabled = false;
                btnXoaSP.Enabled = false;
                button2.Enabled = false;
                numSoLuong.Visible = false;
                return;
            }

            // Bật các nút liên quan đến giỏ hàng
            btnXoaSP.Enabled = true;
            button2.Enabled = true; // nút cập nhật số lượng
            numSoLuong.Enabled = true;
            numSoLuong.Visible = true;

            // LẤY ĐÚNG SẢN PHẨM TỪ GIỎ HÀNG
            var itemGioHang = gioHang[e.RowIndex];
            var spTrongGio = danhSachSP.FirstOrDefault(s => s.MaSP == itemGioHang.MaSP);

            if (spTrongGio != null)
            {
                // Hiển thị chi tiết sản phẩm từ danh sách đầy đủ (có ảnh, loại, v.v.)
                HienThiChiTietSanPham(spTrongGio);

                // Tự động điền số lượng hiện tại trong giỏ để người dùng chỉnh sửa
                numSoLuong.Value = itemGioHang.SoLuong;
            }

            // Cập nhật lại nút thêm (không cho thêm khi đang chọn trong giỏ)
            btThemSP.Enabled = false;
        }

        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 1. Kiểm tra index hợp lệ (tránh click vào header)
            if (e.RowIndex < 0) return;

            // 2. Xử lý logic Click lần 2 để bỏ chọn (như code cũ của bạn)
            if (e.RowIndex == lastSelectedRowIndex)
            {
                dgvHoaDon.ClearSelection();
                lastSelectedRowIndex = -1;

                // Khi bỏ chọn thì tắt hết nút
                btnChiTietHD.Enabled = false;
                btnDonBan.Enabled = false;
                btnXoaHD.Enabled = false;
                btnSuaHD.Enabled = false;
                return;
            }

            // 3. Ghi nhận dòng mới chọn
            lastSelectedRowIndex = e.RowIndex;

            // 4. LẤY DỮ LIỆU TỪ DÒNG ĐƯỢC CHỌN
            // Lưu ý: Phải ép kiểu (cast) về đúng DTO của bạn
            var row = dgvHoaDon.Rows[e.RowIndex];
            var hd = row.DataBoundItem as hoaDonDTO;

            if (hd == null) return; // Phòng hờ null

            // 5. KIỂM TRA KHÓA SỔ ĐỂ ẨN/HIỆN NÚT
            // Giả sử KhoaSo là kiểu int (1 hoặc 0). Nếu trong DTO bạn để bool thì sửa thành (hd.KhoaSo == true)
            if (hd.KhoaSo == 1)
            {
                // === TRƯỜNG HỢP: ĐÃ KHÓA SỔ (Khách đã về) ===
                // Chỉ cho xem chi tiết, CẤM sửa/xóa/dọn bàn
                btnChiTietHD.Enabled = true;  // Vẫn nên cho xem

                btnDonBan.Enabled = false;    // Đã dọn rồi thì không dọn nữa
                btnXoaHD.Enabled = false;     // Cấm xóa
                btnSuaHD.Enabled = false;     // Cấm sửa
            }
            else
            {
                // === TRƯỜNG HỢP: ĐANG PHỤC VỤ ===
                // Mở hết các nút để thao tác
                btnChiTietHD.Enabled = true;
                btnDonBan.Enabled = true;
                btnXoaHD.Enabled = true;
                btnSuaHD.Enabled = true;
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
            CapNhatTrangThaiNutHoaDon();
        }

        private void btnXoaHD_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần Xóa!",
                                "Chưa chọn",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            var hd = dsHoaDon[dgvHoaDon.CurrentRow.Index];

            string msg =
        $@"XÁC NHẬN XÓA HÓA ĐƠN
        ────────────────────────────

        Bạn có chắc muốn xóa hóa đơn này không?";

            if (MessageBox.Show(msg, "Xóa hóa đơn",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // 🔥 Chỉ chuyển trạng thái của hóa đơn, không xoá!
                bool ok = busHoaDon.XoaHoaDon(hd.MaHD);

                if (ok)
                {
                    MessageBox.Show(
                        $@"XÓA HÓA ĐƠN THÀNH CÔNG!
                        Hóa đơn HD{hd.MaHD} đã xóa hoàn tất.
                        Bàn {hd.MaBan} đã được giải phóng.",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // 🎯 Cập nhật bàn trong FormChonBan
                    foreach (Form f in Application.OpenForms)
                        if (f is FormChonBan chonBan)
                            chonBan.CapNhatBanTrong(hd.MaBan);

                }
                else
                {
                    MessageBox.Show("Xóa Hóa Đơn thất bại!", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvHoaDon_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (dgvHoaDon.Rows[e.RowIndex].DataBoundItem is hoaDonDTO hd)
            {
                if (hd.KhoaSo == 1)
                {
                    dgvHoaDon.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    dgvHoaDon.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkGray;
                }
                else
                {
                    dgvHoaDon.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    dgvHoaDon.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null || dgvGioHang.CurrentRow.Index < 0) return;

            int rowIndex = dgvGioHang.CurrentRow.Index;
            if (rowIndex >= gioHang.Count) return;

            int soLuongMoi = (int)numSoLuong.Value;
            if (soLuongMoi <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = gioHang[rowIndex];
            item.SoLuong = soLuongMoi;
            item.ThanhTien = item.SoLuong * item.DonGia;

            CapNhatGioHang();
            dgvGioHang.ClearSelection();
        }

        private void dgvGioHang_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvGioHang.ClearSelection();
        }
        private void CapNhatTrangThaiNutHoaDon()
        {
            bool coChonDong = dgvHoaDon.CurrentRow != null && dgvHoaDon.CurrentRow.Index >= 0
                && dgvHoaDon.CurrentRow.Index < dgvHoaDon.Rows.Count
                  && dgvHoaDon.CurrentCell != null;

            if (!coChonDong)
            {
                // CHƯA CHỌN GÌ → TẮT HẾT
                btnSuaHD.Enabled = false;
                btnXoaHD.Enabled = false;
                btnChiTietHD.Enabled = false;
                btnDonBan.Enabled = false;
                return;
            }


            var hd = dsHoaDon[dgvHoaDon.CurrentRow.Index];

            if (hd.TrangThai == true) // đã thanh toán / đã xóa
            {
                btnChiTietHD.Enabled = true;
                btnSuaHD.Enabled = false;
                btnXoaHD.Enabled = false;
                btnDonBan.Enabled = false;
            }
            else // chưa thanh toán → cho phép thao tác
            {
                btnChiTietHD.Enabled = true;
                btnSuaHD.Enabled = true;
                btnXoaHD.Enabled = true;
                btnDonBan.Enabled = true;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AnNutHoaDon();
        }
    }
 }

