using BUS;
using DAO;
using DTO;
using FONTS;
using GUI.GUI_CRUD;
using GUI.GUI_SELECT;
using GUI.PDF;
using OfficeOpenXml;
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
using LicenseContext = OfficeOpenXml.LicenseContext;

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
        private int maHDDangChon = -1;
        private int maNV = -1, maKH = 0;
        BindingList<nhanVienDTO> dsNV;
        BindingList<khachHangDTO> dsKH;
        BindingList<ppThanhToanDTO> dsThanhToan;
        private BindingList<khachHangDTO> dsKhachHang = new khachHangBUS().LayDanhSach();
        private BindingList<nhanVienDTO> dsNhanVien = new nhanVienBUS().LayDanhSach();

        public banHangGUI()
        {
            InitializeComponent();
            try
            {
                FontManager.LoadFont();
                FontManager.ApplyFontToAllControls(this);
            }
            catch { }
        }

        private void banHangGUI_Load(object sender, EventArgs e)
        {
            dsLoai = new loaiSanPhamBUS().LayDanhSach();
            dsNhom = new nhomBUS().layDanhSach();
            dsKH = new khachHangBUS().LayDanhSach();
            dsNV = new nhanVienBUS().LayDanhSach();
            busSanPham = new sanPhamBUS();
            danhSachSP = new sanPhamBUS().LayDanhSachCoCongThuc();
            LoadDanhSachSanPham(danhSachSP);
            CapNhatGioHang();
            loadFontChuVaSizeGioHang();
            LoadDanhSachHoaDon();
            loadFontChuVaSizeHoaDon();
            CapNhatTrangThaiNutHoaDon();
            AnNutHoaDon();
            
            dsThanhToan = new ppThanhToanBUS().LayDanhSach();
            cbThanhToan.DataSource = dsThanhToan;
            cbThanhToan.DisplayMember = "HinhThuc";
            cbThanhToan.ValueMember = "MaTT";
            tuDongLoadTenNhanVien();
            hienThiPlaceHolderHoaDon();
            hienThiPlaceHolderSanPham();
            rdoTimCoBan.Checked = true;
            txtKhachHang.Text = "Khách lẻ";
            CheckQuyen();
        }

        private void tuDongLoadTenNhanVien()
        {
            if (DTO.Session.NhanVienHienTai != null)
            {
                txtNhanVien.Text = DTO.Session.NhanVienHienTai.HoTen;
                maNV = DTO.Session.NhanVienHienTai.MaNhanVien;
            }
        }
        private void CheckQuyen()
        {
            var quyenNX = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 3);
            bool isAdmin = (Session.TaiKhoanHienTai.MAVAITRO == 1);
            bool coQuyenThem = isAdmin || (quyenNX != null && quyenNX.CAN_CREATE == 1);
            btThemSP.Enabled = false;
            btnXacNhan.Enabled = coQuyenThem;
            btnExcelSP.Enabled = coQuyenThem;
            btnChonBan.Enabled = coQuyenThem;
            btnChonKH.Enabled = coQuyenThem;
            numSoLuong.Enabled = coQuyenThem;

            button2.Enabled = false;
            button5.Enabled = coQuyenThem;
            btnXoaSP.Enabled = false;
            btnSuaHD.Enabled = false;
            btnXoaHD.Enabled = false;
            btnDonBan.Enabled = false;
            btnChiTietHD.Enabled = false;
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

        public void LoadData()
        {
            var data = busSanPham.LayDanhSachCoCongThuc(true);
            LoadDanhSachSanPham(data);
            dgvSanPham.Refresh();
        }

        private void LoadDanhSachSanPham(BindingList<sanPhamDTO> ds)
        {
            foreach (var sp in ds)
            {
                sp.SoLuongToiDa = busSanPham.TinhSoLuongToiDa(sp.MaSP);
            }

            dgvSanPham.AutoGenerateColumns = false;
            dgvSanPham.DataSource = null;
            dgvSanPham.DataSource = ds;

            dgvSanPham.Columns.Clear();

            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSP", HeaderText = "Mã SP", Width = 70 });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenSP", HeaderText = "Tên SP", Width = 200 });

            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên loại" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Tên nhóm" });
            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Gia",
                HeaderText = "Giá",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoLuongToiDa",
                HeaderText = "SL có thể bán",
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, Font = new Font(dgvSanPham.Font, FontStyle.Bold) } // Căn giữa và in đậm cho dễ nhìn
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
            dsKH = new khachHangBUS().LayDanhSach();
            dsNV = new nhanVienBUS().LayDanhSach();
            dsHoaDon = busHoaDon.LayDanhSach();
                dgvHoaDon.AutoGenerateColumns = false;
                dgvHoaDon.DataSource = null; // reset
                dgvHoaDon.DataSource = dsHoaDon;

                if (dgvHoaDon.Columns.Count == 0)
                {
                    dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name="MaHD",
                        DataPropertyName = "MaHD",
                        HeaderText = "Mã HD",
                    });
                    dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "MaBan",
                        HeaderText = "Bàn",
                    });
                dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "MaTT",
                    HeaderText = "Thanh toán",
                });
                dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "MaNhanVien",
                    HeaderText = "Tên nhân viên",
                }); dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "MaKhachHang",
                    HeaderText = "Tên khách hàng",
                });
                dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "ThoiGianTao",
                        HeaderText = "Thời gian",
                        DefaultCellStyle = new DataGridViewCellStyle { Format = "HH:mm dd/MM" },
                    });
                    dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "TongTien",
                        HeaderText = "Tổng tiền",
                        DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" },
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
            if (dsLoai == null || dsNhom == null) return;
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

                    //LoadDanhSachHoaDon();
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

            if (sp.SoLuongToiDa <= 0)
            {
                MessageBox.Show($"Sản phẩm '{sp.TenSP}' đã hết nguyên liệu!", "Hết hàng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int soLuongMuonThem = (int)numSoLuong.Value;
            if (soLuongMuonThem <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var itemInCart = gioHang.FirstOrDefault(i => i.MaSP == sp.MaSP);
            int soLuongDangCoTrongGio = (itemInCart != null) ? itemInCart.SoLuong : 0;

            if (soLuongDangCoTrongGio + soLuongMuonThem > sp.SoLuongToiDa)
            {
                MessageBox.Show($"Không đủ nguyên liệu! \nKho chỉ còn đủ làm tối đa {sp.SoLuongToiDa} ly.\nTrong giỏ đã có: {soLuongDangCoTrongGio}.",
                                "Cảnh báo tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (itemInCart != null)
            {
                itemInCart.SoLuong += soLuongMuonThem;
                itemInCart.ThanhTien = itemInCart.SoLuong * itemInCart.DonGia;
            }
            else
            {
                gioHang.Add(new cthoaDonDTO
                {
                    MaSP = sp.MaSP,
                    SoLuong = soLuongMuonThem,
                    DonGia = (decimal)sp.Gia,
                    ThanhTien = soLuongMuonThem * (decimal)sp.Gia
                });
                dgvSanPham.ClearSelection();
            }
            sp.SoLuongToiDa -= soLuongMuonThem;
            dgvSanPham.Refresh();
            CapNhatGioHang();
            dgvGioHang.ClearSelection();
            ResetInput();
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
            loadFontChuVaSizeGioHang();
        }

        private void ResetInput()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtLoaiSP.Clear();
            txtGia.Clear();
            picSanPham.Image = null;
            numSoLuong.Value = 1;
            numSoLuong.Visible = false;
            dgvGioHang.ClearSelection();
            dgvSanPham.ClearSelection();
        }

        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null) return;
            int index = dgvGioHang.CurrentRow.Index;
            if (index < 0 || index >= gioHang.Count) return;

            var item = gioHang[index];

            if (MessageBox.Show("Xóa món này khỏi giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var sp = danhSachSP.FirstOrDefault(s => s.MaSP == item.MaSP);
                if (sp != null)
                {
                    sp.SoLuongToiDa += item.SoLuong;
                    dgvSanPham.Refresh();
                }

                gioHang.RemoveAt(index);
                CapNhatGioHang();
                ResetInput();
                btThemSP.Enabled = false;
                button2.Enabled = false;
                btnXoaSP.Enabled = false;
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xóa toàn bộ giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                BindingList<sanPhamDTO> ds = new sanPhamBUS().LayDanhSachCoCongThuc();
                gioHang.Clear();
                LoadDanhSachSanPham(ds);
                CapNhatGioHang();
                ResetInput();
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

            if (maNV == -1)
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
            Khách hàng: {txtKhachHang.Text}
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
                MaKhachHang = maKH,   
                MaNhanVien = maNV,
                MaTT = maTT,       
                ThoiGianTao = DateTime.Now,
                TrangThai = true,
                TongTien= tongTien
            };
            banBUS busBan = new banBUS();
            busBan.DoiTrangThai(maBan,0);
            
            // 8. Gọi BUS thêm hóa đơn
            int maHD = busHoaDon.ThemHoaDon(hd, gioHang);

            if (maHD > 0)
            {
                try
                {
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

                tabControl1.SelectedTab = tabHoaDon;
                this.WindowState = FormWindowState.Maximized;
                ResetInputControls();
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
            BindingList<sanPhamDTO> ds = new sanPhamBUS().LayDanhSachCoCongThuc();
            LoadDanhSachSanPham(ds);
            LoadDanhSachLoaiVaNhom();
            CapNhatGioHang();
            ResetInput();
        }

        private int lastSelectedRowIndex = -1;
        private FormWindowState WindowState;

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var quyenNX = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 3);
            bool isAdmin = (Session.TaiKhoanHienTai.MAVAITRO == 1);

            bool coQuyenThem = isAdmin || (quyenNX != null && quyenNX.CAN_CREATE == 1);
            bool coQuyenSua = isAdmin || (quyenNX != null && quyenNX.CAN_UPDATE == 1);
            bool coQuyenXoa = isAdmin || (quyenNX != null && quyenNX.CAN_DELETE == 1);


            dgvGioHang.ClearSelection();
            lastSelectedRowIndexGio = -1; // Reset biến lưu của giỏ hàng

            // Tắt các nút chức năng của giỏ hàng
            btnXoaSP.Enabled = false;
            button2.Enabled = false; // Nút sửa

            // 2. KIỂM TRA CLICK HỢP LỆ
            if (e.RowIndex < 0 || e.RowIndex >= danhSachSP.Count)
                return;

            // 3. LOGIC TOGGLE (Bấm lại dòng đã chọn thì hủy)
            if (e.RowIndex == lastSelectedRowIndex)
            {
                dgvSanPham.ClearSelection();
                lastSelectedRowIndex = -1;
                ResetInputControls(); // Hàm xóa trắng ô nhập (xem bên dưới)
                return;
            }

            // 4. CHỌN DÒNG MỚI
            lastSelectedRowIndex = e.RowIndex; // Cập nhật dòng mới

            var sp = danhSachSP[e.RowIndex];
            HienThiChiTietSanPham(sp); // Hiển thị thông tin lên textbox

            // Cấu hình nút bấm cho chế độ THÊM
            btThemSP.Enabled = coQuyenThem;
            numSoLuong.Enabled = true;
            numSoLuong.Visible = true;
            numSoLuong.Value = 1; // Mặc định là 1 khi chọn món mới
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
                using(detailHoaDon form = new detailHoaDon(hd))
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

        private int lastSelectedRowIndexGio = -1;
        private void dgvGioHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvSanPham.ClearSelection();
            lastSelectedRowIndex = -1;
            btThemSP.Enabled = false;
            if (e.RowIndex < 0 || e.RowIndex >= gioHang.Count)
                return;
            if (e.RowIndex == lastSelectedRowIndexGio)
            {
                dgvGioHang.ClearSelection();
                lastSelectedRowIndexGio = -1;
                ResetInputControls();
                return;
            }

            lastSelectedRowIndexGio = e.RowIndex;

            var itemGioHang = gioHang[e.RowIndex];
            var spTrongGio = danhSachSP.FirstOrDefault(s => s.MaSP == itemGioHang.MaSP);

            if (spTrongGio != null)
            {
                HienThiChiTietSanPham(spTrongGio);
                numSoLuong.Value = itemGioHang.SoLuong;
            }
            btnXoaSP.Enabled = true;
            button2.Enabled = true;
            numSoLuong.Enabled = true;
            numSoLuong.Visible = true;
        }
        private void ResetInputControls()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtLoaiSP.Clear();
            txtGia.Clear();
            if (picSanPham.Image != null) picSanPham.Image = null;

            numSoLuong.Value = 1;
            numSoLuong.Visible = false;

            btThemSP.Enabled = false;
            btnXoaSP.Enabled = false;
            button2.Enabled = false;
        }
        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var quyenNX = Session.QuyenHienTai.FirstOrDefault(x => x.MaQuyen == 3);
            bool isAdmin = (Session.TaiKhoanHienTai.MAVAITRO == 1);

             bool coQuyenThem = isAdmin || (quyenNX != null && quyenNX.CAN_CREATE == 1);
            bool coQuyenSua = isAdmin || (quyenNX != null && quyenNX.CAN_UPDATE == 1);
            bool coQuyenXoa = isAdmin || (quyenNX != null && quyenNX.CAN_DELETE == 1);

            if (e.RowIndex == lastSelectedRowIndex)
            {
                dgvHoaDon.ClearSelection();
                lastSelectedRowIndex = -1;
                btnChiTietHD.Enabled = false;
                btnDonBan.Enabled = false;
                btnXoaHD.Enabled = false;
                btnSuaHD.Enabled = false;
                btnInPDF.Enabled = false;
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
                btnDonBan.Enabled = coQuyenSua;
                btnXoaHD.Enabled = coQuyenXoa;
                btnSuaHD.Enabled = coQuyenThem;
                btnExcelSP.Enabled = coQuyenThem;
                btnInPDF.Enabled = true;
            }
        }

        private void btnChonKH_Click(object sender, EventArgs e)
        {
            using (var f = new FormchonKH())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    txtKhachHang.Text = f.TenKHChon;
                    maKH = f.MaKHChon;
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
            if (dgvGioHang.CurrentRow == null) return;
            int index = dgvGioHang.CurrentRow.Index;
            var item = gioHang[index];
            var sp = danhSachSP.FirstOrDefault(s => s.MaSP == item.MaSP);

            int soLuongMoi = (int)numSoLuong.Value;
            int soLuongCu = item.SoLuong;
            int chenhLech = soLuongMoi - soLuongCu;
            if (chenhLech > 0 && chenhLech > sp.SoLuongToiDa)
            {
                MessageBox.Show($"Kho chỉ còn thêm được {sp.SoLuongToiDa} ly thôi!", "Thiếu hàng");
                return;
            }
            item.SoLuong = soLuongMoi;
            item.ThanhTien = item.SoLuong * item.DonGia;
            sp.SoLuongToiDa -= chenhLech;

            dgvSanPham.Refresh();
            CapNhatGioHang();
            ResetInput();
            btThemSP.Enabled = false;
            button2.Enabled = false;
            btnXoaSP.Enabled = false;
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

                using(dialogSuaHoacThemHD formChon = new dialogSuaHoacThemHD())
                {
                    formChon.StartPosition = FormStartPosition.CenterParent;
                    if(formChon.ShowDialog() == DialogResult.OK)
                    {
                        if(formChon.luaChon == 1)
                        {
                            using(updateThongTinHD formSua = new updateThongTinHD(hd))
                            {
                                formSua.StartPosition = FormStartPosition.CenterParent;
                                if(formSua.ShowDialog() == DialogResult.OK)
                                {
                                    hoaDonDTO hdsua = formSua.hd;
                                    busHoaDon.capNhatThongTinHoaDon(hdsua);
                                    dgvHoaDon.Refresh();
                                    BindingList<sanPhamDTO> dsMoi = busSanPham.LayDanhSachCoCongThuc();
                                    LoadDanhSachSanPham(dsMoi);
                                }
                            }
                        }else
                        {
                            using (updateHoaDon form = new updateHoaDon(hd))
                            {
                                form.StartPosition = FormStartPosition.CenterParent;
                                if(form.ShowDialog() == DialogResult.OK)
                                {
                                    BindingList<sanPhamDTO> dsMoi = busSanPham.LayDanhSachCoCongThuc();
                                    LoadDanhSachSanPham(dsMoi);
                                }
                            }
                        }
                    }
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
            dgvHoaDon.ClearSelection();
            btnChiTietHD.Enabled = false;
        }

        private void btnExcelSP_Click(object sender, EventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook|*.xlsx";
                sfd.FileName = $"HoaDon_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var package = new ExcelPackage())
                    {
                        // ==================== SHEET 1: HÓA ĐƠN ====================
                        var wsHoaDon = package.Workbook.Worksheets.Add("Danh sách hóa đơn");

                        // Lấy dữ liệu đã sắp xếp giảm dần theo MaHD
                        var dsHD = dsHoaDon
                            .OrderByDescending(hd => hd.MaHD)
                            .Select(hd =>
                            {
                                var tenKH = dsKhachHang.FirstOrDefault(k => k.MaKhachHang == hd.MaKhachHang)?.TenKhachHang
                                            ?? "Khách lẻ";

                                var tenNV = dsNhanVien.FirstOrDefault(n => n.MaNhanVien == hd.MaNhanVien)?.HoTen
                                            ?? "Không xác định";

                                return new
                                {
                                    Mã_HĐ = hd.MaHD,
                                    Bàn = hd.MaBan,
                                    Thời_gian = hd.ThoiGianTao,
                                    Tổng_tiền = hd.TongTien,
                                    Khách_hàng = tenKH,
                                    Nhân_viên = tenNV,
                                    Hình_thức_TT = hd.MaTT == 1 ? "Chuyển khoản" : "Tiền mặt"
                                };
                            })
                            .ToList();

                        wsHoaDon.Cells["A1"].LoadFromCollection(dsHD, true);

                        // Định dạng header
                        using (var range = wsHoaDon.Cells[1, 1, 1, 7])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 112, 192));
                            range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }

                        // Định dạng cột
                        wsHoaDon.Column(1).Width = 12;  // Mã HĐ
                        wsHoaDon.Column(2).Width = 10;  // Bàn
                        wsHoaDon.Column(3).Style.Numberformat.Format = "dd/MM/yyyy HH:mm"; // Thời gian
                        wsHoaDon.Column(3).Width = 20;
                        wsHoaDon.Column(4).Style.Numberformat.Format = "#,##0 ₫";         // Tổng tiền
                        wsHoaDon.Column(4).Width = 18;
                        wsHoaDon.Column(5).Width = 20;  // Khách hàng
                        wsHoaDon.Column(6).Width = 20;  // Nhân viên
                        wsHoaDon.Column(7).Width = 18;

                        wsHoaDon.Cells.AutoFitColumns();


                        // ==================== SHEET 2: CHI TIẾT HÓA ĐƠN ====================
                        var wsCT = package.Workbook.Worksheets.Add("Chi tiết hóa đơn");

                        hoaDonBUS ctBus = new hoaDonBUS();
                        var dsCT = ctBus.LayTatCaChiTiet()
                            .OrderByDescending(ct => ct.maHD)  // Sắp xếp theo hóa đơn mới nhất trước
                            .ThenBy(ct => ct.MaSP)
                            .Select(ct => new
                            {
                                Mã_HĐ = ct.maHD,
                                Mã_SP = ct.MaSP,
                                Tên_sản_phẩm = ct.TenSP,
                                Số_lượng = ct.SoLuong,
                                Đơn_giá = ct.DonGia,
                                Thành_tiền = ct.ThanhTien
                            })
                            .ToList();

                        wsCT.Cells["A1"].LoadFromCollection(dsCT, true);

                        // Header đẹp
                        using (var range = wsCT.Cells[1, 1, 1, 6])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(0, 112, 192));
                            range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                        }

                        // Định dạng
                        wsCT.Column(1).Width = 12;
                        wsCT.Column(2).Width = 12;
                        wsCT.Column(3).Width = 35;
                        wsCT.Column(4).Width = 12;
                        wsCT.Column(5).Style.Numberformat.Format = "#,##0 ₫";
                        wsCT.Column(5).Width = 18;
                        wsCT.Column(6).Style.Numberformat.Format = "#,##0 ₫";
                        wsCT.Column(6).Width = 20;

                        wsCT.Cells.AutoFitColumns();

                        // Lưu file
                        package.SaveAs(new FileInfo(sfd.FileName));
                    }

                    MessageBox.Show($"Xuất Excel thành công!\nĐường dẫn: {sfd.FileName}",
                                  "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở file luôn cho tiện
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = sfd.FileName,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất Excel:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void rdoTimCoBan_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimCoBan.Checked == true)
            {
                rdoTimNangCao.Checked = false;
                txtNV.Enabled = false;
                txtTenBan.Enabled = false;
                txtKH.Enabled = false;
                txtGiaMax.Enabled = false;
                txtGiaMin.Enabled = false;
                rdoTimNangCao.Checked = false;
                hienThiPlaceHolderHoaDon();
                ResetInputTimKiem();
            }
            else
            {
                txtTimKiemHD.Enabled = false;
                cboTimKiemHD.Enabled = false;
                txtTenBan.Enabled = true;
                txtKH.Enabled = true;
                txtNV.Enabled = true;
                txtGiaMax.Enabled = true;
                txtGiaMin.Enabled = true;
                rdoTimCoBan.Checked = false;
                ResetInputTimKiem();
            }
        }

        private void rdoTimNangCao_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimNangCao.Checked == true)
            {
                rdoTimCoBan.Checked = false;
                cboTimKiemHD.Enabled = false;
                txtTimKiemHD.Enabled = false;

                cboTimKiemHD.SelectedItem = -1;
                hienThiPlaceHolderHoaDon();
                ResetInputTimKiem();
            }
            else
            {
                cboTimKiemHD.Enabled = true;
                txtTimKiemHD.Enabled = true;
                rdoTimCoBan.Checked = true;
                ResetInputTimKiem();
            }
        }
        public void loadLaiDanhSachTimKiem()
        {
            LoadDanhSachHoaDon();
            loadFontChuVaSize();
            ResetInputTimKiem();
        }
        private void hienThiPlaceHolderHoaDon()
        {
            SetPlaceholder(txtTimKiemHD, "Nhập giá trị cần tìm");
            SetPlaceholder(txtKH, "Tên Khách Hàng");
            SetPlaceholder(txtNV, "Tên Nhân Viên");
            SetPlaceholder(txtTenBan, "Bàn");
            SetPlaceholder(txtGiaMax, "Max Tiền");
            SetPlaceholder(txtGiaMin, "Min Tiền");
            SetComboBoxPlaceholder(cboTimKiemHD, "Chọn giá trị TK");
        }
        // set placehover cho tìm kiếm nâng cao sản phẩm
        private void SetPlaceholder(TextBox txt, string placeholder)
        {
            txt.ForeColor = Color.Gray;
            txt.Text = placeholder;
            txt.GotFocus += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };
            txt.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.ForeColor = Color.Gray;
                    txt.Text = placeholder;
                }
            };
        }


        // set placehover cho tìm kiếm nâng cao sản phẩm
        private void SetComboBoxPlaceholder(ComboBox cbo, string placeholder)
        {

            cbo.ForeColor = Color.Gray;
            cbo.Text = placeholder;

            cbo.GotFocus += (s, e) =>
            {
                if (cbo.Text == placeholder)
                {
                    cbo.Text = "";
                    cbo.ForeColor = Color.Black;
                }
            };
            cbo.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(cbo.Text))
                {
                    cbo.Text = placeholder;
                    cbo.ForeColor = Color.Gray;
                }
            };
        }
        public void ResetInputTimKiem()
        {
            cboTimKiemHD.SelectedIndex = -1;
            txtTimKiemHD.Clear();
            txtKH.Clear();
            txtNV.Clear();
            txtTenBan.Clear();
            txtGiaMin.Clear();
            txtGiaMax.Clear();

            hienThiPlaceHolderHoaDon();
        }
        public void timKiemCoBan()
        {
            string tim = txtTimKiemHD.Text.Trim();
            int index = cboTimKiemHD.SelectedIndex;

            if (index == -1 || string.IsNullOrWhiteSpace(tim))
            {
                MessageBox.Show("Vui lòng nhập giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BindingList<hoaDonDTO> dskq = busHoaDon.timKiemCoBan(tim, index);
            if (dskq != null && dskq.Count > 0)
            {
                dgvHoaDon.DataSource = dskq;                //loadFontChuVaSize();
            }
            else
            {
                MessageBox.Show("Không tìm thấy kết quả tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadLaiDanhSachTimKiem();
                return;
            }
        }
        public void timKiemNangCao()
        {
            string tenBan = string.IsNullOrWhiteSpace(txtTenBan.Text) ? null : txtTenBan.Text.Trim();
            string tenKH = string.IsNullOrWhiteSpace(txtKH.Text) ? null : txtKH.Text.Trim();
            string tenNV = string.IsNullOrWhiteSpace(txtNV.Text) ? null : txtNV.Text.Trim();

            if (tenBan == "Bàn")
            {
                tenBan = null;
            }
            if (tenKH == "Tên Khách Hàng")
            {
                tenKH = null;
            }
            if (tenNV == "Tên Nhân Viên")
            {
                tenNV = null;
            }

            decimal slMin = -1;
            string strMin = txtGiaMin.Text.Trim();
            if (!string.IsNullOrWhiteSpace(strMin) && strMin != "Min Tiền")
            {
                decimal.TryParse(strMin, out slMin);
            }

            decimal slMax = -1;
            string strMax = txtGiaMax.Text.Trim();
            if (!string.IsNullOrWhiteSpace(strMax) && strMax != "Max Tiền")
            {
                decimal.TryParse(strMax, out slMax);
            }

            if (slMin != -1 && slMax != -1 && slMin > slMax)
            {
                MessageBox.Show("Số lượng tối thiểu không được lớn hơn tồn tối đa", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tenBan == null && tenKH == null && tenNV == null && slMin == -1 && slMax == -1)
            {
                MessageBox.Show("Vui lòng nhập ít nhất một điều kiện", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BindingList<hoaDonDTO> dsTimKiemNangCao = busHoaDon.timKiemNangCao(tenBan, tenKH, tenNV, slMin, slMax);
            if (dsTimKiemNangCao != null && dsTimKiemNangCao.Count > 0)
            {
                dgvHoaDon.DataSource = dsTimKiemNangCao;
            }
            else
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnThucHienTimKiem_Click(object sender, EventArgs e)
        {
            if (rdoTimCoBan.Checked == true)
            {
                timKiemCoBan();
            }
            else
            {
                timKiemNangCao();
            }
        }
        private void dgvHoaDon_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dsNV == null || dsKH == null) return;

            hoaDonDTO hd = dgvHoaDon.Rows[e.RowIndex].DataBoundItem as hoaDonDTO;
            if (hd == null) return;

            if (dgvHoaDon.Columns[e.ColumnIndex].HeaderText == "Tên nhân viên")
            {
                nhanVienDTO nv = dsNV.FirstOrDefault(x => x.MaNhanVien == hd.MaNhanVien);
                e.Value = nv?.HoTen ?? "Không xác định";
            }

            if (dgvHoaDon.Columns[e.ColumnIndex].HeaderText == "Tên khách hàng")
            {
                if (hd.MaKhachHang == 0)
                {
                    e.Value = "Khách lẻ";
                }
                else
                {
                    khachHangDTO kh = dsKH.FirstOrDefault(x => x.MaKhachHang == hd.MaKhachHang);
                    e.Value = kh?.TenKhachHang ?? "Không xác định";
                }
            }

            if (dgvHoaDon.Columns[e.ColumnIndex].HeaderText == "Thanh toán")
            {
                ppThanhToanDTO nv = dsThanhToan.FirstOrDefault(x => x.MaTT == hd.MaTT);
                e.Value = nv?.HinhThuc ?? "Không xác định";
            }
        }

        private void hienThiPlaceHolderSanPham()
        {
            SetPlaceholder(txtTKSanPham, "Nhập giá trị cần tìm");
            SetComboBoxPlaceholder(cboSanPham, "Chọn giá trị TK");
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboSanPham.Focus();
                return;
            }
            if (busSanPham.kiemTraChuoiRong(txtTKSanPham.Text))
            {
                MessageBox.Show("Vui lòng nhập giá trị tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTKSanPham.Focus();
                return;
            }
            List<sanPhamDTO> dsTim = new List<sanPhamDTO>();
            string tim = txtTKSanPham.Text.ToLower().Trim();
            string giaTriTim = cboSanPham.SelectedItem.ToString();

            if (giaTriTim == "Mã SP")
            {
                dsTim = (from sp in danhSachSP
                         where sp.MaSP.ToString().Contains(tim)
                         orderby sp.MaSP
                         select sp).ToList();
            }
            if (giaTriTim == "Tên SP")
            {
                dsTim = (from sp in danhSachSP
                         where sp.TenSP.ToLower().Contains(tim)
                         orderby sp.MaSP
                         select sp).ToList();
            }
            if (giaTriTim == "Tên Loại")
            {
                dsTim = (from nl in danhSachSP
                         join dv in dsLoai on nl.MaLoai equals dv.MaLoai
                         where dv.TenLoai.ToLower().Contains(tim)
                         orderby dv.MaLoai
                         select nl).ToList();
            }
            if (giaTriTim == "Tên nhóm")
            {
                string timBoDau = RemoveDiacritics(tim);

                dsTim = (
                    from sp in danhSachSP
                    join loai in dsLoai on sp.MaLoai equals loai.MaLoai
                    join nhom in dsNhom on loai.MaNhom equals nhom.MaNhom
                    let tenNhomBoDau = RemoveDiacritics(nhom.TenNhom.ToLower())
                    where tenNhomBoDau.Contains(timBoDau)
                    orderby nhom.MaNhom
                    select sp
                ).ToList();
            }

            if (dsTim != null && dsTim.Count > 0)
            {
                BindingList<sanPhamDTO> dsBinding = new BindingList<sanPhamDTO>(dsTim);
                LoadDanhSachSanPham(dsBinding);
            }
            else
            {
                MessageBox.Show("Không tim thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnInPDF_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.SelectedRows.Count == 0) return;

            var row = dgvHoaDon.SelectedRows[0];
            var hd = row.DataBoundItem as hoaDonDTO;

            if (hd == null) return;

            var inHD = new inPDFhoaDon();
            inHD.In(hd);
        }

        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var normalized = text.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }

    }
}

