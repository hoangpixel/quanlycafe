using BUS;
using BUS;
using DAO;
using DTO;
using FONTS;
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

namespace GUI.GUI_CRUD
{
    public partial class updateHoaDon : Form
    {
        private BindingList<sanPhamDTO> danhSachSP;
        private BindingList<loaiDTO> dsLoai;
        private BindingList<nhomDTO> dsNhom;
        private sanPhamBUS busSanPham = new sanPhamBUS();
        private BindingList<cthoaDonDTO> gioHang = new BindingList<cthoaDonDTO>();
        private BindingList<hoaDonDTO> dsHoaDon;
        private hoaDonBUS busHoaDon = new hoaDonBUS();
        private int lastSelectedRowIndex = -1;
        private BindingList<banDTO> dsBan;
        private int maHDDangSua;
        private bool dangLamMoi = false;
        private hoaDonDTO doiTuong;
        private readonly int maBanChinh;        // bàn cố định
        private readonly int? maKhachHangChinh; // khách cố định (có thể null = khách lẻ)
        private readonly string tenKhachHangChinh;
        public int maBan = -1, maKH = -1;

        public updateHoaDon(hoaDonDTO doiTuong)
        {
            InitializeComponent();
            this.doiTuong = doiTuong;
            this.maHDDangSua = doiTuong.MaHD;

            maBanChinh = doiTuong.MaBan;
            maKhachHangChinh = doiTuong.MaKhachHang > 0 ? doiTuong.MaKhachHang : (int?)null;
            tenKhachHangChinh = string.IsNullOrEmpty(doiTuong.TenKhachHang) ? "Khách lẻ" : doiTuong.TenKhachHang;

            txtBan.Text = banBUS.ds.FirstOrDefault(b => b.MaBan == maBanChinh)?.TenBan ?? "Bàn không xác định"; txtKhachHang.Text = tenKhachHangChinh;

            if (maHDDangSua > 0) 
            {
                txtBan.Enabled = false;
                txtKhachHang.Enabled = false; 
                txtBan.BackColor = Color.LightGray;
                txtKhachHang.BackColor = Color.LightGray;
            }
        }
        private void updateHoaDon_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            dsLoai = new loaiSanPhamBUS().LayDanhSach();
            dsNhom = new nhomBUS().layDanhSach();
            dsBan = new banBUS().LayDanhSach();

            busSanPham = new sanPhamBUS();
            LoadDanhSachSanPham();

            BindingList<ppThanhToanDTO> dsThanhToan = new ppThanhToanBUS().LayDanhSach();
            cbThanhToan.DataSource = dsThanhToan;
            cbThanhToan.DisplayMember = "HinhThuc";
            cbThanhToan.ValueMember = "MaTT";

            banDTO ban = dsBan.FirstOrDefault(x => x.MaBan == doiTuong.MaBan);
            txtBan.Text = ban?.TenBan ?? "Khong xac dinh";
            txtKhachHang.Text = doiTuong.TenKhachHang;

            maBan = doiTuong.MaBan;
            maKH = doiTuong.MaKhachHang;
        }
        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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

        private void dgvSanPham_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvSanPham.ClearSelection();
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
            btnXoaSP.Enabled = true;
            button2.Enabled = true; // nút cập nhật số lượng
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

        private void dgvGioHang_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvGioHang.ClearSelection();
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
            //loadFontChuVaSizeGioHang();
            dgvGioHang.ClearSelection();
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

        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null || dgvGioHang.CurrentRow.Index < 0) return;

            int index = dgvGioHang.CurrentRow.Index;
            if (index >= gioHang.Count) return;
            var item = gioHang[index];
            if (MessageBox.Show("Xóa món này khỏi giỏ hàng?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xóa toàn bộ giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                gioHang.Clear();
                LoadDanhSachSanPham();
                CapNhatGioHang();
                ResetInput();
            }
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

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"[DEBUG] txtBan.Text = '{txtBan.Text}'");

            if (gioHang.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống! Vui lòng thêm món ăn.", "Chưa có món",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            

            if (txtNhanVien.Tag == null || !int.TryParse(txtNhanVien.Tag.ToString(), out int maNV))
            {
                MessageBox.Show("Vui lòng chọn nhân viên phục vụ!", "Lỗi nhân viên",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int maTT = Convert.ToInt32(cbThanhToan.SelectedValue);
            decimal tongTien = gioHang.Sum(g => g.SoLuong * g.DonGia);

            int maBan = doiTuong.MaBan;
            int maKhachHang = doiTuong.MaKhachHang;
            //string tenKhach = string.IsNullOrEmpty(doiTuong.TenKhachHang) ? "Khách lẻ" : doiTuong.TenKhachHang;

            string message = $@"XÁC NHẬN TẠO HÓA ĐƠN
            ────────────────────────────
            Bàn số: {maBan}
            
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
            var hd = new hoaDonDTO
            {
                MaBan = maBan,
                MaKhachHang = maKhachHang,
                MaNhanVien = maNV,
                MaTT = maTT,
                ThoiGianTao = DateTime.Now,
                TrangThai = true,
                //KhoaSo = dangLamMoi ? (byte)1 : (byte)0
            };
            banBUS busBan = new banBUS();
            //busBan.DoiTrangThais(maBan);
            int maHD = busHoaDon.SuaHoaDon(hd, gioHang);
            
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
                Tổng tiền: {tongTien:N0} VNĐ
                Nhân viên: {txtNhanVien.Text}",
                "Thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                gioHang.Clear();
                CapNhatGioHang();
                txtBan.Clear();txtBan.Tag = null;
                txtKhachHang.Clear(); txtKhachHang.Tag = null;
                txtNhanVien.Clear(); txtNhanVien.Tag = null;
            }

            else
                {
                MessageBox.Show("Tạo hóa đơn thất bại! Vui lòng thử lại.", "Lỗi hệ thống",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
