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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

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
        private int maHDDangChon = 0;
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

            dgvSanPham.Columns.Clear();

            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSP", HeaderText = "Mã SP" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenSP", HeaderText = "Tên SP" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên loại" });   
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên nhóm" });   
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
                dsHoaDon = busHoaDon.LayDanhSach();
                dgvHoaDon.AutoGenerateColumns = false;
                dgvHoaDon.DataSource = null; // reset
                dgvHoaDon.DataSource = dsHoaDon;

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
                dgvHoaDon.CurrentCell = null; 

                this.BeginInvoke((MethodInvoker)delegate
                {
                    dgvHoaDon.ClearSelection();
                    dgvHoaDon.CurrentCell = null;
                    CapNhatTrangThaiNutHoaDon();
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

            if (gioHang.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống! Vui lòng thêm món ăn.", "Chưa có món",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtBan.Tag == null || !int.TryParse(txtBan.Tag.ToString(), out int maBan))
            {
                MessageBox.Show("Vui lòng chọn bàn hợp lệ!", "Lỗi bàn",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? maKH = null;
            if (txtKhachHang.Tag != null && int.TryParse(txtKhachHang.Tag.ToString(), out int kh))
                maKH = kh;

            if (txtNhanVien.Tag == null || !int.TryParse(txtNhanVien.Tag.ToString(), out int maNV))
            {
                MessageBox.Show("Vui lòng chọn nhân viên phục vụ!", "Lỗi nhân viên",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int maTT = Convert.ToInt32(cbThanhToan.SelectedValue);
            decimal tongTien = gioHang.Sum(g => g.SoLuong * g.DonGia);

            string message = $@"XÁC NHẬN TẠO HÓA ĐƠN
            ────────────────────────────
            Bàn số: {maBan}
            Khách hàng: {(maKH.HasValue ? txtKhachHang.Text : "Khách lẻ")}
            Nhân viên: {txtNhanVien.Text}
            Số món: {gioHang.Count}
            PPTT: {maTT}
            Tổng tiền: {tongTien:N0}
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

            var hd = new hoaDonDTO
            {
                MaBan = maBan,
                MaKhachHang = maKH ??0,   
                MaNhanVien = maNV,
                MaTT = maTT,       
                ThoiGianTao = DateTime.Now,
                TrangThai = true,
                TongTien= tongTien
            };
            banBUS busBan = new banBUS();
            busBan.DoiTrangThai(maBan);
            
            // 8. Gọi BUS thêm hóa đơn
            int maHD = busHoaDon.ThemHoaDon(hd, gioHang);

            if (maHD > 0)
            {
                try
                {
                    // 1. Chuyển đổi List giỏ hàng sang Dictionary<MaSP, SoLuong> để gửi cho DAO
                    // Lý do: Hàm TruTonKhoKhiBanHang mình viết lúc nãy nhận Dictionary
                    Dictionary<int, int> dicGioHang = new Dictionary<int, int>();
                    foreach (var item in gioHang)
                    {
                        if (dicGioHang.ContainsKey(item.MaSP))
                            dicGioHang[item.MaSP] += item.SoLuong;
                        else
                            dicGioHang.Add(item.MaSP, item.SoLuong);
                    }

                    // 2. Gọi DAO Trừ kho (Hàm này nằm trong NguyenLieuDAO bạn đã tạo)
                    nguyenLieuDAO nlDAO = new nguyenLieuDAO();
                    bool ketQuaTruKho = nlDAO.TruTonKhoKhiBanHang(dicGioHang);

                    if (!ketQuaTruKho)
                    {
                        // Nếu trừ kho thất bại (ví dụ lỗi DB), thông báo nhưng vẫn cho qua vì Hóa đơn đã tạo rồi
                        MessageBox.Show("Hóa đơn đã tạo nhưng TRỪ KHO THẤT BẠI. Vui lòng kiểm tra lại tồn kho!",
                                        "Cảnh báo kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hệ thống khi trừ kho: " + ex.Message);
                }
                MessageBox.Show($@"TẠO HÓA ĐƠN THÀNH CÔNG!
                Mã hóa đơn: HD{maHD}
                Bàn: {maBan}

                Nhân viên: {txtNhanVien.Text}",
                                "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                gioHang.Clear();
                CapNhatGioHang();
                txtBan.Clear();
                txtBan.Tag = null;
                txtKhachHang.Clear(); txtKhachHang.Tag = null;
                txtNhanVien.Clear(); txtNhanVien.Tag = null;

                tabControl1.SelectedTab = tabHoaDon;
                this.WindowState = FormWindowState.Maximized;
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
                    chonBanForm.RefreshBan(); 
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
        private FormWindowState WindowState;

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSanPham.CurrentRow != null)
            {
                btThemSP.Enabled = true;

                numSoLuong.Enabled = true;
                numSoLuong.Visible = true;
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
            if (dgvHoaDon.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvHoaDon.SelectedRows[0];
                hoaDonDTO hd = row.DataBoundItem as hoaDonDTO;
                using(frmChiTietHD form = new frmChiTietHD(hd))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
            }
        }

        private void btnRefreshSP_Click(object sender, EventArgs e)
        {
            LoadDanhSachHoaDon();
        }

        private void dgvGioHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= gioHang.Count)
            {
                btThemSP.Enabled = false;
                btnXoaSP.Enabled = false;
                button2.Enabled = false;
                numSoLuong.Visible = false;
                return;
            }
            btnXoaSP.Enabled = true;
            button2.Enabled = true;
            numSoLuong.Enabled = true;
            numSoLuong.Visible = true;

            var itemGioHang = gioHang[e.RowIndex];
            var spTrongGio = danhSachSP.FirstOrDefault(s => s.MaSP == itemGioHang.MaSP);

            if (spTrongGio != null)
            {
                HienThiChiTietSanPham(spTrongGio);

                numSoLuong.Value = itemGioHang.SoLuong;
            }

            btThemSP.Enabled = false;
        }

        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.RowIndex == lastSelectedRowIndex)
            {
                dgvHoaDon.ClearSelection();
                lastSelectedRowIndex = -1;

                btnChiTietHD.Enabled = false;
                btnDonBan.Enabled = false;
                btnXoaHD.Enabled = false;
                btnSuaHD.Enabled = false;
                return;
            }

            lastSelectedRowIndex = e.RowIndex;

            var row = dgvHoaDon.Rows[e.RowIndex];
            var hd = row.DataBoundItem as hoaDonDTO;

            if (hd == null) return;
            if (hd.KhoaSo == 1)
            {
                btnChiTietHD.Enabled = true; 
                btnDonBan.Enabled = false;
                btnXoaHD.Enabled = false;
                btnSuaHD.Enabled = false;
            }
            else
            {
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
                    txtKhachHang.Tag = f.MaKHChon;
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
                    txtNhanVien.Tag = f.MaNVChon;
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
                bool ok = busHoaDon.XoaHoaDon(hd.MaHD);

                if (ok)
                {
                    bool conHoaDonNaoKhac = dsHoaDon.Any(x => x.MaBan == hd.MaBan);
                    if (conHoaDonNaoKhac == false)
                    {
                        busHoaDon.doiTrangThaiBanSauKhiXoaHD(hd.MaBan);

                        foreach (Form f in Application.OpenForms)
                        {
                            if (f is FormChonBan chonBan)
                            {
                                chonBan.CapNhatBanTrong(hd.MaBan);
                            }
                        }
                    }
                    MessageBox.Show(
                        $@"XÓA HÓA ĐƠN THÀNH CÔNG!
                        Hóa đơn HD{hd.MaHD} đã xóa hoàn tất.
                        Bàn {hd.MaBan} đã được giải phóng.",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

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
                btnSuaHD.Enabled = false;
                btnXoaHD.Enabled = false;
                btnChiTietHD.Enabled = false;
                btnDonBan.Enabled = false;
                return;
            }


            var hd = dsHoaDon[dgvHoaDon.CurrentRow.Index];

            if (hd.TrangThai == true) 
            {
                btnChiTietHD.Enabled = true;
                btnSuaHD.Enabled = false;
                btnXoaHD.Enabled = false;
                btnDonBan.Enabled = false;
            }
            else
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

        private void btnSuaHD_Click(object sender, EventArgs e)
        {
            if(dgvHoaDon.SelectedRows.Count >0 )
            {
                DataGridViewRow row = dgvHoaDon.SelectedRows[0];
                hoaDonDTO hd = row.DataBoundItem as hoaDonDTO;

                using(updateHoaDon form = new updateHoaDon(hd))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
            }
        }

        private void btnDonBan_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvHoaDon.SelectedRows[0];
                hoaDonDTO hd = row.DataBoundItem as hoaDonDTO;
                string msg =
                $@"XÁC NHẬN XÓA HÓA ĐƠN
                ────────────────────────────

                Bạn có chắc muốn dọn bàn này không?";
                if (MessageBox.Show(msg, "Khoa so hóa đơn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    busHoaDon.doiTrangThaiBanSauKhiXoaHD(hd.MaBan);
                    
                    foreach (hoaDonDTO ct in dsHoaDon)
                    {
                        if ( ct.MaBan==hd.MaBan)
                        {
                            busHoaDon.UpdateKhoaSo(hd.MaBan);
                        }
                    }
                    MessageBox.Show(
                        $@"KHOA SO THÀNH CÔNG!
                        Hóa đơn HD{hd.MaHD} đã xóa hoàn tất.
                        Bàn {hd.MaBan} đã được giải phóng.",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    foreach (Form f in Application.OpenForms)
                        if (f is FormChonBan chonBan)
                            chonBan.CapNhatBanTrong(hd.MaBan);
                                      
                }
            }
            dgvHoaDon.Refresh();
        }
    }
 }

