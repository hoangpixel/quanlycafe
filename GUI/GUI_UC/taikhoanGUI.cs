using BUS;
using DTO;
using FONTS;
using GUI.GUI_CRUD;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GUI.GUI_UC
{
    public partial class taikhoanGUI : UserControl
    {
        private taikhoanBUS bustaikhoan = new taikhoanBUS();
        private int lastSelectedRow = -1;

        public taikhoanGUI()
        {
            InitializeComponent();
            CauHinhDataGridView();

            // Đăng ký sự kiện cho các nút
            if (btnThemSP != null)
                btnThemSP.Click += btnThem_Click;

            if (btnSuaSP != null)
                btnSuaSP.Click += btnSua_Click;

            if (btnXoaSP != null)
                btnXoaSP.Click += btnXoa_Click;

            // ✅ THÊM SỰ KIỆN CHO NÚT CHI TIẾT
            if (btnChiTiet != null)
                btnChiTiet.Click += btnChiTiet_Click;

            // ✅ THÊM SỰ KIỆN CHO NÚT EXCEL
            if (btnExcelSP != null)
                btnExcelSP.Click += btnExcel_Click;

            // ✅ THÊM SỰ KIỆN CHO TÌM KIẾM
            if (rdoTimCoBan != null)
                rdoTimCoBan.CheckedChanged += rdoTimCoBan_CheckedChanged;

            if (rdoTimNangCao != null)
                rdoTimNangCao.CheckedChanged += rdoTimNangCao_CheckedChanged;

            if (btnThucHienTimKiem != null)
                btnThucHienTimKiem.Click += btnThucHienTimKiem_Click;
        }

        private void CauHinhDataGridView()
        {
            tbSanPham.BackgroundColor = Color.White;
            tbSanPham.BorderStyle = BorderStyle.None;
            tbSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tbSanPham.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tbSanPham.MultiSelect = false;
            tbSanPham.ReadOnly = true;
            tbSanPham.AllowUserToAddRows = false;
            tbSanPham.AllowUserToDeleteRows = false;
            tbSanPham.RowHeadersVisible = false;
            tbSanPham.Font = new Font("Segoe UI", 10);
            tbSanPham.RowTemplate.Height = 35;

            tbSanPham.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            tbSanPham.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tbSanPham.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            tbSanPham.ColumnHeadersHeight = 40;
            tbSanPham.EnableHeadersVisualStyles = false;

            tbSanPham.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            tbSanPham.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
            tbSanPham.DefaultCellStyle.SelectionForeColor = Color.White;

            tbSanPham.CellClick += tbtaikhoan_CellClick;
            tbSanPham.DataBindingComplete += tbtaikhoan_DataBindingComplete;
            tbSanPham.CellDoubleClick += tbtaikhoan_CellDoubleClick;
        }

        private void taikhoanGUI_Load(object sender, EventArgs e)
        {
            try
            {
                FontManager.LoadFont();
                FontManager.ApplyFontToAllControls(this);
                LoadDanhSach();
                HienThiPlaceHolder();
                LoadComboBoxTimKiem();
                rdoTimCoBan.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSach()
        {
            try
            {
                BindingList<taikhoanDTO> ds = bustaikhoan.LayDanhSach();

                if (ds == null || ds.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu tài khoản!", "Thông báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tbSanPham.DataSource = null;
                    return;
                }

                tbSanPham.AutoGenerateColumns = false;
                tbSanPham.DataSource = ds;
                tbSanPham.Columns.Clear();

                tbSanPham.Columns.AddRange(new DataGridViewColumn[]
                {
                    new DataGridViewTextBoxColumn { DataPropertyName = "MAtaikHOAN", HeaderText = "Mã TK", Width = 80 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "TENDANGNHAP", HeaderText = "Tên Đăng Nhập", Width = 150 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "TENVAITRO", HeaderText = "Vai Trò", Width = 120 },
                    new DataGridViewTextBoxColumn { DataPropertyName = "TENNHANVIEN", HeaderText = "Nhân Viên", Width = 150 },
                    new DataGridViewCheckBoxColumn { DataPropertyName = "TRANGTHAI", HeaderText = "Hoạt Động", Width = 100 },
                    new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "NGAYTAO",
                        HeaderText = "Ngày Tạo",
                        Width = 160,
                        DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
                    }
                });

                tbSanPham.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                using (addTK form = new addTK())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadDanhSach();
                        MessageBox.Show("Đã thêm tài khoản mới!", "Thông báo",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbSanPham.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản cần sửa!", "Cảnh báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy dòng được chọn
                DataGridViewRow row = tbSanPham.SelectedRows[0];

                // Lấy trực tiếp từ DataSource để tránh lỗi tên cột
                BindingList<taikhoanDTO> ds = (BindingList<taikhoanDTO>)tbSanPham.DataSource;
                taikhoanDTO tkCanSua = ds[row.Index];

                // Mở form sửa
                using (suaTK form = new suaTK(tkCanSua))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadDanhSach();
                        MessageBox.Show("Đã cập nhật tài khoản thành công!", "Thông báo",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message + "\n\n" + ex.StackTrace, "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbSanPham.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản cần xóa!", "Cảnh báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy dòng được chọn
                DataGridViewRow row = tbSanPham.SelectedRows[0];

                // Lấy trực tiếp từ DataSource để tránh lỗi tên cột
                BindingList<taikhoanDTO> ds = (BindingList<taikhoanDTO>)tbSanPham.DataSource;
                taikhoanDTO tkCanXoa = ds[row.Index];

                // Mở form xác nhận xóa
                using (xoaTk form = new xoaTk(tkCanXoa))
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadDanhSach();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message + "\n\n" + ex.StackTrace, "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ THÊM METHOD MỚI: XEM CHI TIẾT TÀI KHOẢN
        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra có chọn dòng nào không
                if (tbSanPham.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản để xem chi tiết!", "Cảnh báo",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy dòng được chọn
                DataGridViewRow row = tbSanPham.SelectedRows[0];

                // Lấy mã tài khoản từ DataSource thay vì từ Cells
                BindingList<taikhoanDTO> ds = (BindingList<taikhoanDTO>)tbSanPham.DataSource;
                taikhoanDTO tkSelected = ds[row.Index];

                int maTaiKhoan = tkSelected.MAtaikHOAN;

                // Mở form chi tiết
                using (ctTK formChiTiet = new ctTK(maTaiKhoan))
                {
                    formChiTiet.StartPosition = FormStartPosition.CenterParent;
                    formChiTiet.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message + "\n\n" + ex.StackTrace, "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ THÊM METHOD MỚI: XUẤT/NHẬP EXCEL
        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (excelTK formExcel = new excelTK())
                {
                    formExcel.StartPosition = FormStartPosition.CenterParent;
                    formExcel.ShowDialog();

                    // Refresh lại danh sách sau khi đóng form Excel
                    LoadDanhSach();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDanhSach();

            // Clear các control tìm kiếm
            txtTimKiemSP.Clear();
            txtTenSPTK.Clear();
            cboTimKiemSP.SelectedIndex = -1;
            cboLoaiSP.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = 0;

            // Hiển thị lại placeholder
            HienThiPlaceHolder();

            MessageBox.Show("Đã làm mới danh sách!", "Thông báo",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tbtaikhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.RowIndex == lastSelectedRow)
            {
                tbSanPham.ClearSelection();
                lastSelectedRow = -1;
                return;
            }

            tbSanPham.ClearSelection();
            tbSanPham.Rows[e.RowIndex].Selected = true;
            lastSelectedRow = e.RowIndex;
        }

        private void tbtaikhoan_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            tbSanPham.ClearSelection();
        }

        private void tbtaikhoan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Double click để xem chi tiết thay vì sửa
                btnChiTiet_Click(sender, e);
            }
        }

        // ========== TÌM KIẾM ==========

        private void LoadComboBoxTimKiem()
        {
            cboTimKiemSP.Items.Clear();
            cboTimKiemSP.Items.Add("Mã TK");
            cboTimKiemSP.Items.Add("Tên đăng nhập");
            cboTimKiemSP.Items.Add("Tên nhân viên");
            cboTimKiemSP.SelectedIndex = -1;
            SetComboBoxPlaceholder(cboTimKiemSP, "Chọn tiêu chí TK");

            // ComboBox Vai trò (Tìm kiếm nâng cao)
            var dsVaiTro = bustaikhoan.LayDanhSachVaiTro();
            cboLoaiSP.DataSource = dsVaiTro;
            cboLoaiSP.DisplayMember = "Value";
            cboLoaiSP.ValueMember = "Key";
            cboLoaiSP.SelectedIndex = -1;
            SetComboBoxPlaceholder(cboLoaiSP, "Vai trò");

            // ComboBox Trạng thái
            cboTrangThai.Items.Clear();
            cboTrangThai.Items.Add("Tất cả");
            cboTrangThai.Items.Add("Hoạt động");
            cboTrangThai.Items.Add("Bị khóa");
            cboTrangThai.SelectedIndex = 0;
            SetComboBoxPlaceholder(cboTrangThai, "Trạng thái");
        }

        private void HienThiPlaceHolder()
        {
            SetPlaceholder(txtTimKiemSP, "Nhập giá trị cần tìm");
            SetPlaceholder(txtTenSPTK, "Tên đăng nhập");
            SetComboBoxPlaceholder(cboTimKiemSP, "Chọn tiêu chí TK");
            SetComboBoxPlaceholder(cboLoaiSP, "Vai trò");
            SetComboBoxPlaceholder(cboTrangThai, "Trạng thái");
        }

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

        private void TimKiemCoBan()
        {
            string tim = txtTimKiemSP.Text.Trim();
            int index = cboTimKiemSP.SelectedIndex;

            if (index < 0 || string.IsNullOrWhiteSpace(tim) || tim == "Nhập giá trị cần tìm")
            {
                MessageBox.Show("Vui lòng nhập thông tin tìm kiếm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                BindingList<taikhoanDTO> danhSachFull = bustaikhoan.LayDanhSach();
                BindingList<taikhoanDTO> ketQua = new BindingList<taikhoanDTO>();

                foreach (var tk in danhSachFull)
                {
                    bool match = false;
                    switch (index)
                    {
                        case 0: // Mã TK
                            match = tk.MAtaikHOAN.ToString().Contains(tim);
                            break;
                        case 1: // Tên đăng nhập
                            match = tk.TENDANGNHAP.ToLower().Contains(tim.ToLower());
                            break;
                        case 2: // Tên nhân viên
                            match = tk.TENNHANVIEN != null && tk.TENNHANVIEN.ToLower().Contains(tim.ToLower());
                            break;
                    }

                    if (match) ketQua.Add(tk);
                }

                if (ketQua.Count > 0)
                {
                    tbSanPham.DataSource = ketQua;
                    MessageBox.Show($"Tìm thấy {ketQua.Count} kết quả!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không có kết quả tìm kiếm!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSach();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TimKiemNangCao()
        {
            try
            {
                // Lấy giá trị từ các control
                int maVaiTro = (cboLoaiSP.SelectedValue == null || cboLoaiSP.SelectedIndex < 0) ? -1 : Convert.ToInt32(cboLoaiSP.SelectedValue);
                int trangThai = cboTrangThai.SelectedIndex; // 0=Tất cả, 1=Hoạt động, 2=Bị khóa
                string tenDangNhap = (txtTenSPTK.Text == "Tên đăng nhập" || string.IsNullOrWhiteSpace(txtTenSPTK.Text)) ? null : txtTenSPTK.Text.Trim();

                // Kiểm tra có điều kiện nào không
                if (maVaiTro == -1 && trangThai == 0 && tenDangNhap == null)
                {
                    MessageBox.Show("Vui lòng nhập ít nhất một điều kiện để tìm kiếm!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Lấy toàn bộ danh sách và lọc
                BindingList<taikhoanDTO> danhSachFull = bustaikhoan.LayDanhSach();
                BindingList<taikhoanDTO> ketQua = new BindingList<taikhoanDTO>();

                foreach (var tk in danhSachFull)
                {
                    bool match = true;

                    // Lọc theo vai trò
                    if (maVaiTro != -1 && tk.MAVAITRO != maVaiTro)
                        match = false;

                    // Lọc theo trạng thái
                    if (trangThai == 1 && !tk.TRANGTHAI) // Hoạt động
                        match = false;
                    if (trangThai == 2 && tk.TRANGTHAI) // Bị khóa
                        match = false;

                    // Lọc theo tên đăng nhập
                    if (tenDangNhap != null && !tk.TENDANGNHAP.ToLower().Contains(tenDangNhap.ToLower()))
                        match = false;

                    if (match) ketQua.Add(tk);
                }

                if (ketQua.Count > 0)
                {
                    tbSanPham.DataSource = ketQua;
                    MessageBox.Show($"Tìm thấy {ketQua.Count} kết quả!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không có kết quả tìm kiếm!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSach();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm nâng cao: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rdoTimCoBan_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimCoBan.Checked)
            {
                rdoTimNangCao.Checked = false;
                txtTenSPTK.Enabled = false;
                cboLoaiSP.Enabled = false;
                cboTrangThai.Enabled = false;

                txtTimKiemSP.Enabled = true;
                cboTimKiemSP.Enabled = true;
            }
            else
            {
                txtTenSPTK.Enabled = true;
                cboLoaiSP.Enabled = true;
                cboTrangThai.Enabled = true;
            }
        }

        private void rdoTimNangCao_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTimNangCao.Checked)
            {
                rdoTimCoBan.Checked = false;
                txtTimKiemSP.Enabled = false;
                cboTimKiemSP.Enabled = false;

                txtTenSPTK.Enabled = true;
                cboLoaiSP.Enabled = true;
                cboTrangThai.Enabled = true;
            }
            else
            {
                txtTimKiemSP.Enabled = true;
                cboTimKiemSP.Enabled = true;
            }
        }

        private void btnThucHienTimKiem_Click(object sender, EventArgs e)
        {
            if (rdoTimCoBan.Checked)
            {
                TimKiemCoBan();
            }
            else
            {
                TimKiemNangCao();
            }
        }
    }
}