using BUS;
using DTO;
using FONTS;
using GUI.GUI_CRUD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI.GUI_UC
{
    public partial class nhanVienGUI : UserControl
    {
        private nhanVienBUS busNhanVien = new nhanVienBUS();
        private DataGridView tbNhanVien;
        private int lastSelectedRow = -1;

        public nhanVienGUI()
        {
            InitializeComponent();

            tbNhanVien = tbSanPham;
            tbNhanVien.Name = "tbNhanVien";
            CauHinhDataGridView();
            tbNhanVien.CellClick += tbNhanVien_CellClick;
            tbNhanVien.DataBindingComplete += tbNhanVien_DataBindingComplete;
        }

        private void CauHinhDataGridView()
        {
            tbNhanVien.BackgroundColor = Color.White;
            tbNhanVien.BorderStyle = BorderStyle.None;
            tbNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tbNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tbNhanVien.MultiSelect = false;
            tbNhanVien.ReadOnly = true;
            tbNhanVien.AllowUserToAddRows = false;
            tbNhanVien.AllowUserToDeleteRows = false;
            tbNhanVien.RowHeadersVisible = false;
            tbNhanVien.Font = new Font("Segoe UI", 10);
            tbNhanVien.RowTemplate.Height = 35;
            tbNhanVien.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            tbNhanVien.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tbNhanVien.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            tbNhanVien.ColumnHeadersHeight = 40;
            tbNhanVien.EnableHeadersVisualStyles = false;
            tbNhanVien.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            tbNhanVien.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
            tbNhanVien.DefaultCellStyle.SelectionForeColor = Color.White;
        }

        private void nhanVienGUI_Load(object sender, EventArgs e)
        {
            try
            {
                FontManager.LoadFont();
                FontManager.ApplyFontToAllControls(this);
                LoadDuLieuBanDau();
                KhoiTaoControlsTimKiem();
                ConnectEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDuLieuBanDau()
        {
            BindingList<nhanVienDTO> ds = busNhanVien.LayDanhSach();
            loadDanhSachNhanVien(ds);
            loadFontChuVaSize();
            tbNhanVien.ClearSelection();
        }

        // ==================== KHỞI TẠO CONTROLS ====================
        private void KhoiTaoControlsTimKiem()
        {
            try
            {
                KhoiTaoTimKiemCoBan();
                KhoiTaoTimKiemNangCao();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khởi tạo controls: " + ex.Message);
            }
        }

        private void KhoiTaoTimKiemCoBan()
        {
            if (cboTimKiemSP != null)
            {
                cboTimKiemSP.Items.Clear();
                cboTimKiemSP.Items.AddRange(new string[] { "Mã NV", "Họ Tên", "Số ĐT", "Email" });
                cboTimKiemSP.SelectedIndex = 1;
                cboTimKiemSP.SelectedIndexChanged += (s, e) =>
                {
                    if (rdoTimCoBan != null && rdoTimCoBan.Checked)
                        XuLyTimKiemCoBan();
                };
            }

            if (txtTimKiemSP != null)
            {
                txtTimKiemSP.TextChanged += txtTimKiemSP_TextChanged;
            }

            if (btnThucHienTimKiem != null)
            {
                btnThucHienTimKiem.Click += btnThucHienTimKiem_Click;
            }

            // ===== RADIO BUTTON CƠ BẢN =====
            if (rdoTimCoBan != null)
            {
                // Khi CLICK vào RadioButton
                rdoTimCoBan.Click += (s, e) =>
                {
                    // Bỏ chọn RadioButton kia
                    if (rdoTimNangCao != null && rdoTimNangCao.Checked)
                        rdoTimNangCao.Checked = false;

                    // Chọn RadioButton này
                    rdoTimCoBan.Checked = true;
                };

                // Khi trạng thái thay đổi
                rdoTimCoBan.CheckedChanged += (s, e) =>
                {
                    if (rdoTimCoBan.Checked)
                        XuLyTimKiemCoBan();
                };
            }
        }

        private void KhoiTaoTimKiemNangCao()
        {
            // Ẩn ComboBox không dùng
            if (cboLoaiSP != null)
                cboLoaiSP.Visible = false;

            if (cboTrangThai != null)
                cboTrangThai.Visible = false;

            // Placeholder
            ThemPlaceholder(txtTenSPTK, "Tên NV");
            ThemPlaceholder(txtGiaMin, "Lương min");
            ThemPlaceholder(txtGiaMax, "Lương max");

            // ===== RADIO BUTTON NÂNG CAO =====
            if (rdoTimNangCao != null)
            {
                // Khi CLICK vào RadioButton
                rdoTimNangCao.Click += (s, e) =>
                {
                    // Bỏ chọn RadioButton kia
                    if (rdoTimCoBan != null && rdoTimCoBan.Checked)
                        rdoTimCoBan.Checked = false;

                    // Chọn RadioButton này
                    rdoTimNangCao.Checked = true;
                };

                // Khi trạng thái thay đổi
                rdoTimNangCao.CheckedChanged += (s, e) =>
                {
                    if (rdoTimNangCao.Checked)
                        XuLyTimKiemNangCao();
                };
            }
        }

        // ==================== HELPER CHO PLACEHOLDER ====================
        private void ThemPlaceholder(TextBox txt, string placeholder)
        {
            if (txt == null) return;
            txt.Text = placeholder;
            txt.ForeColor = Color.Gray;

            txt.Enter += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };

            txt.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                }
            };
        }

        private string LayTextThucSu(TextBox txt, string placeholder)
        {
            if (txt == null) return "";
            if (txt.Text == placeholder) return "";
            if (txt.ForeColor == Color.Gray) return "";
            return txt.Text.Trim();
        }

        // ==================== XỬ LÝ NÚT TÌM KIẾM ====================
        private void btnThucHienTimKiem_Click(object sender, EventArgs e)
        {
            if (rdoTimCoBan != null && rdoTimCoBan.Checked)
            {
                // Đảm bảo chỉ chọn 1
                if (rdoTimNangCao != null && rdoTimNangCao.Checked)
                    rdoTimNangCao.Checked = false;

                XuLyTimKiemCoBan();
            }
            else if (rdoTimNangCao != null && rdoTimNangCao.Checked)
            {
                // Đảm bảo chỉ chọn 1
                if (rdoTimCoBan != null && rdoTimCoBan.Checked)
                    rdoTimCoBan.Checked = false;

                XuLyTimKiemNangCao();
            }
            else
            {
                // Mặc định chọn cơ bản
                if (rdoTimCoBan != null)
                    rdoTimCoBan.Checked = true;

                XuLyTimKiemCoBan();
            }
        }

        // ==================== TÌM KIẾM CƠ BẢN ====================
        private void txtTimKiemSP_TextChanged(object sender, EventArgs e)
        {
            if (rdoTimCoBan != null && rdoTimCoBan.Checked)
                XuLyTimKiemCoBan();
        }

        private void XuLyTimKiemCoBan()
        {
            try
            {
                if (txtTimKiemSP == null || cboTimKiemSP == null)
                    return;

                string tuKhoa = txtTimKiemSP.Text.ToLower().Trim();
                string loaiTimKiem = cboTimKiemSP.SelectedItem?.ToString() ?? "";

                if (string.IsNullOrEmpty(tuKhoa))
                {
                    LoadDuLieuBanDau();
                    return;
                }

                BindingList<nhanVienDTO> ds = busNhanVien.LayDanhSach();
                List<nhanVienDTO> ketQua = TimKiemTheoLoai(ds, tuKhoa, loaiTimKiem);
                HienThiKetQuaTimKiem(ketQua);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<nhanVienDTO> TimKiemTheoLoai(BindingList<nhanVienDTO> ds, string tuKhoa, string loaiTimKiem)
        {
            switch (loaiTimKiem)
            {
                case "Mã NV":
                    return ds.Where(x => x.MaNhanVien.ToString().Contains(tuKhoa)).ToList();
                case "Họ Tên":
                    return ds.Where(x => x.HoTen.ToLower().Contains(tuKhoa)).ToList();
                case "Số ĐT":
                    return ds.Where(x => x.SoDienThoai.Contains(tuKhoa)).ToList();
                case "Email":
                    return ds.Where(x => x.Email.ToLower().Contains(tuKhoa)).ToList();
                default:
                    return ds.Where(x =>
                        x.MaNhanVien.ToString().Contains(tuKhoa) ||
                        x.HoTen.ToLower().Contains(tuKhoa) ||
                        x.SoDienThoai.Contains(tuKhoa) ||
                        x.Email.ToLower().Contains(tuKhoa)
                    ).ToList();
            }
        }


        // ==================== TÌM KIẾM NÂNG CAO ====================
        private void XuLyTimKiemNangCao()
        {
            try
            {
                BindingList<nhanVienDTO> dsGoc = busNhanVien.LayDanhSach();
                List<nhanVienDTO> ketQua = dsGoc.ToList();

                ketQua = LocTheoTen(ketQua);
                ketQua = LocTheoKhoangLuong(ketQua);

                HienThiKetQuaTimKiem(ketQua);

                if (ketQua.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy nhân viên nào phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Tìm thấy {ketQua.Count} nhân viên phù hợp!", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm nâng cao: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<nhanVienDTO> LocTheoTen(List<nhanVienDTO> ketQua)
        {
            string ten = LayTextThucSu(txtTenSPTK, "Tên NV");
            if (!string.IsNullOrEmpty(ten))
            {
                ketQua = ketQua.Where(x => x.HoTen.ToLower().Contains(ten.ToLower())).ToList();
            }
            return ketQua;
        }

        private List<nhanVienDTO> LocTheoKhoangLuong(List<nhanVienDTO> ketQua)
        {
            string strMin = LayTextThucSu(txtGiaMin, "Lương min");
            string strMax = LayTextThucSu(txtGiaMax, "Lương max");

            if (!string.IsNullOrEmpty(strMin) && !string.IsNullOrEmpty(strMax))
            {
                if (decimal.TryParse(strMin, out decimal luongMin) && decimal.TryParse(strMax, out decimal luongMax))
                {
                    if (luongMin > luongMax)
                    {
                        MessageBox.Show("Lương Min không được lớn hơn Lương Max!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return new List<nhanVienDTO>();
                    }
                    ketQua = ketQua.Where(x => x.Luong >= luongMin && x.Luong <= luongMax).ToList();
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập khoảng lương hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return new List<nhanVienDTO>();
                }
            }
            return ketQua;
        }

        private void HienThiKetQuaTimKiem(List<nhanVienDTO> ketQua)
        {
            BindingList<nhanVienDTO> dsKetQua = new BindingList<nhanVienDTO>(ketQua);
            loadDanhSachNhanVien(dsKetQua);
            loadFontChuVaSize();
            tbNhanVien.ClearSelection();
        }

        // ==================== NÚT LÀM MỚI ====================
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            try
            {
                ResetTimKiem();
                LoadDuLieuBanDau();
                MessageBox.Show("Đã làm mới dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi làm mới: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetTimKiem()
        {
            txtTimKiemSP?.Clear();
            if (cboTimKiemSP != null)
                cboTimKiemSP.SelectedIndex = 1;

            if (txtTenSPTK != null)
            {
                txtTenSPTK.Text = "Tên NV";
                txtTenSPTK.ForeColor = Color.Gray;
            }

            if (txtGiaMin != null)
            {
                txtGiaMin.Text = "Lương min";
                txtGiaMin.ForeColor = Color.Gray;
            }

            if (txtGiaMax != null)
            {
                txtGiaMax.Text = "Lương max";
                txtGiaMax.ForeColor = Color.Gray;
            }

            if (rdoTimCoBan != null)
                rdoTimCoBan.Checked = false;
            if (rdoTimNangCao != null)
                rdoTimNangCao.Checked = false;
        }

        // ==================== HIỂN THỊ DỮ LIỆU ====================
        private void loadFontChuVaSize()
        {
            foreach (DataGridViewColumn col in tbNhanVien.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            tbNhanVien.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tbNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (FontManager.GetLightFont(10) != null)
                tbNhanVien.DefaultCellStyle.Font = FontManager.GetLightFont(10);
            if (FontManager.GetBoldFont(12) != null)
                tbNhanVien.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(12);
            tbNhanVien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbNhanVien.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbNhanVien.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            tbNhanVien.Refresh();
        }

        private void loadDanhSachNhanVien(BindingList<nhanVienDTO> ds)
        {
            tbNhanVien.AutoGenerateColumns = false;
            tbNhanVien.DataSource = ds;
            tbNhanVien.Columns.Clear();
            tbNhanVien.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "MaNhanVien", HeaderText = "Mã NV", Width = 80 },
                new DataGridViewTextBoxColumn { DataPropertyName = "HoTen", HeaderText = "Họ Tên" },
                new DataGridViewTextBoxColumn { DataPropertyName = "SoDienThoai", HeaderText = "Số ĐT", Width = 120 },
                new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email", Width = 220 },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Luong",
                    HeaderText = "Lương",
                    Width = 120,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "NgayTao",
                    HeaderText = "Ngày Tạo",
                    Width = 160,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
                }
            });
            tbNhanVien.ReadOnly = true;
            tbNhanVien.ClearSelection();
        }

        // ==================== KẾT NỐI EVENTS ====================
        private void ConnectEvents()
        {
            KetNoiEvent("btnSuaSP", btnSuaSP_Click);
            KetNoiEvent("btnXoaSP", btnXoaSP_Click);
            KetNoiEvent("btnChiTiet", btnChiTiet_Click);
            KetNoiEvent("btnExcelSP", btnExcelSP_Click);
            KetNoiEvent("btnRefreshSP", btnLamMoi_Click);
        }

        private void KetNoiEvent(string btnName, EventHandler handler)
        {
            Control[] controls = this.Controls.Find(btnName, true);
            if (controls.Length > 0 && controls[0] is Button)
                ((Button)controls[0]).Click += handler;
        }

        // ==================== CÁC CHỨC NĂNG CRUD ====================
        private void btnThemNV_Click(object sender, EventArgs e)
        {
            try
            {
                using (addNv form = new addNv())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadDuLieuBanDau();
                        MessageBox.Show("Đã thêm nhân viên mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaSP_Click(object sender, EventArgs e)
        {
            try
            {
                nhanVienDTO nv = LayNhanVienDuocChon();
                if (nv == null) return;
                using (suaNV form = new suaNV(nv))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadDuLieuBanDau();
                        MessageBox.Show("Đã cập nhật thông tin nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            try
            {
                nhanVienDTO nv = LayNhanVienDuocChon();
                if (nv == null) return;
                using (XOAnv form = new XOAnv(nv))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadDuLieuBanDau();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            try
            {
                nhanVienDTO nv = LayNhanVienDuocChon();
                if (nv == null) return;
                using (chiTietNV form = new chiTietNV(nv))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcelSP_Click(object sender, EventArgs e)
        {
            try
            {
                using (excelNV form = new excelNV())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.ShowDialog();
                }
                LoadDuLieuBanDau();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi mở form Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private nhanVienDTO LayNhanVienDuocChon()
        {
            if (tbNhanVien.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            return tbNhanVien.SelectedRows[0].DataBoundItem as nhanVienDTO;
        }

        private void tbNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.RowIndex == lastSelectedRow)
            {
                tbNhanVien.ClearSelection();
                lastSelectedRow = -1;
                return;
            }
            tbNhanVien.ClearSelection();
            tbNhanVien.Rows[e.RowIndex].Selected = true;
            lastSelectedRow = e.RowIndex;
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            btnThemNV_Click(sender, e);
        }

        private void tbNhanVien_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tbNhanVien.ClearSelection();
        }
    }
}