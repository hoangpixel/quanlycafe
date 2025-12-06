using BUS;
using DTO;
using FONTS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices; // Dùng để giải phóng bộ nhớ

namespace GUI.GUI_UC
{
    public partial class thongKeGUI : UserControl
    {
        public thongKeGUI()
        {
            InitializeComponent();
            // nếu bạn có FontManager dùng custom font
            try { FontManager.LoadFont(); FontManager.ApplyFontToAllControls(this); } catch { /* ignore nếu không có */ }

            // Thiết lập mặc định cho 3 tab: Doanh thu, Chi tiêu, Lương
            setMacDinhDoanhThu();
            setMacDinhChiTieu();
            setMacDinhLuong();

            // đặt focus khi load
            this.Load += (s, e) => this.BeginInvoke((MethodInvoker)delegate { if (btnThongKeDT != null) btnThongKeDT.Focus(); });

            // Focus đúng khi đổi tab
            if (tabControl1 != null) tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;

            // Gán event handlers (nếu chưa gán trong designer)
            if (cboLoaiTKDT != null) cboLoaiTKDT.SelectedIndexChanged += cboLoaiTKDT_SelectedIndexChanged;
            if (btnThongKeDT != null) btnThongKeDT.Click += btnThongKeDT_Click;

            if (cboLoaiTKCT != null) cboLoaiTKCT.SelectedIndexChanged += cboLoaiTKCT_SelectedIndexChanged;

            if (cboLoaiTKNV != null) cboLoaiTKNV.SelectedIndexChanged += cboLoaiTKNV_SelectedIndexChanged;
        }
        private void loadFontVaChu(DataGridView table)
        {
            table.EnableHeadersVisualStyles = false;
            table.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            table.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            table.DefaultCellStyle.Font = FontManager.GetLightFont(10);
            table.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);
            table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            table.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            table.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            if (table.Columns.Count > 0)
            {
                foreach (DataGridViewColumn col in table.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            table.Refresh();
        }
        private void loadDanhSachDoanhThu(BindingList<hoaDonDTO> ds)
        {
            tableDoanhThu.AutoGenerateColumns = false;

            tableDoanhThu.Columns.Clear();

            tableDoanhThu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaHD",
                HeaderText = "Mã HĐ"
            });
            tableDoanhThu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaNhanVien",    
                HeaderText = "Tên nhân viên"
            });
            tableDoanhThu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaKhachHang",
                HeaderText = "Tên khách hàng"
            });
            tableDoanhThu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ThoiGianTao",
                HeaderText = "Thời gian tạo"
            });
            tableDoanhThu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TongTien",
                HeaderText = "Tổng tiền"
            });
            tableDoanhThu.DataSource = ds;
            tableDoanhThu.ClearSelection();
            loadFontVaChu(tableDoanhThu);
        }

        private void loadDanhSachThongKe(BindingList<phieuNhapDTO> ds)
        {
            tableChiTieu.AutoGenerateColumns = false;

            tableChiTieu.Columns.Clear();

            tableChiTieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaPN",
                HeaderText = "Mã PN"
            });
            tableChiTieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaNhanVien",
                HeaderText = "Tên nhân viên"
            });
            tableChiTieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaNCC",
                HeaderText = "Tên nhà cung cấp"
            });
            tableChiTieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ThoiGian",
                HeaderText = "Thời gian tạo"
            });
            tableChiTieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TongTien",
                HeaderText = "Tổng tiền"
            });
            tableChiTieu.DataSource = ds;
            tableChiTieu.ClearSelection();
            loadFontVaChu(tableChiTieu);
        }

        // ===================== DOANH THU =====================
        public void setMacDinhDoanhThu()
        {
            // Gán mặc định cho combobox loại
            if (cboLoaiTKDT != null)
            {
                cboLoaiTKDT.Items.Clear();
                cboLoaiTKDT.Items.AddRange(new object[] {
                    "Theo ngày",
                    "Theo tháng",
                    "Theo quý",
                    "Theo năm",
                });
                cboLoaiTKDT.SelectedIndex = 0;
            }

            // Datepickers default
            if (dtpBatDauDT != null) dtpBatDauDT.Value = DateTime.Now.AddMonths(-1);
            if (dtpKetThucDT != null) dtpKetThucDT.Value = DateTime.Now;
            if (cboQuyDT != null)
            {
                cboQuyDT.Items.Clear();
                cboQuyDT.Items.AddRange(new object[] { "Quý 1", "Quý 2", "Quý 3", "Quý 4" });
                cboQuyDT.SelectedIndex = 0;
            }
        }

        private void cboLoaiTKDT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLoaiTKDT == null || dtpBatDauDT == null || dtpKetThucDT == null || cboQuyDT == null) return;
            string chon = cboLoaiTKDT.SelectedItem.ToString();
            if (chon.Contains("Theo ngày"))
            {
                dtpBatDauDT.CustomFormat = "dd/MM/yyyy";
                dtpKetThucDT.CustomFormat = "dd/MM/yyyy";
                dtpBatDauDT.Enabled = dtpKetThucDT.Enabled = true;
                cboQuyDT.Enabled = false;
            }
            else if (chon.Contains("Theo tháng"))
            {
                dtpBatDauDT.CustomFormat = dtpKetThucDT.CustomFormat = "MM/yyyy";
                dtpBatDauDT.Enabled = dtpKetThucDT.Enabled = true;
                cboQuyDT.Enabled = false;
            }
            else if (chon.Contains("Theo quý"))
            {
                dtpBatDauDT.CustomFormat = "yyyy";
                dtpBatDauDT.Enabled = true;
                dtpKetThucDT.Enabled = false;
                cboQuyDT.Enabled = true;
            }
            else if (chon.Contains("Theo năm"))
            {
                dtpBatDauDT.CustomFormat = "yyyy";
                dtpBatDauDT.Enabled = true;
                dtpKetThucDT.Enabled = cboQuyDT.Enabled = false;
            }
        }

        private void btnThongKeDT_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboLoaiTKDT == null || dtpBatDauDT == null || dtpKetThucDT == null || chartDT == null) return;

                string loai = cboLoaiTKDT.SelectedItem.ToString();
                DateTime tu = dtpBatDauDT.Value;
                DateTime den = dtpKetThucDT.Value;
                thongKeBUS bus = new thongKeBUS();

                List<thongKeDTO> dataChart = new List<thongKeDTO>(); // Dữ liệu cho Biểu đồ
                List<hoaDonDTO> dataGrid = new List<hoaDonDTO>();    // Dữ liệu cho DataGridView

                string title = "THỐNG KÊ DOANH THU";

                // --- BƯỚC 1: XỬ LÝ THỜI GIAN VÀ LẤY DỮ LIỆU BIỂU ĐỒ ---
                if (loai.Contains("Theo năm"))
                {
                    int nam = tu.Year;
                    // 1. Chart
                    dataChart = bus.GetDoanhThuTheoThang(nam);
                    title += " NĂM " + nam;

                    // 2. Chuẩn bị thời gian để lấy Grid (Cả năm)
                    tu = new DateTime(nam, 1, 1);
                    den = new DateTime(nam, 12, 31, 23, 59, 59);
                }
                else if (loai.Contains("Theo tháng"))
                {
                    tu = new DateTime(tu.Year, tu.Month, 1);
                    var cuoi = new DateTime(den.Year, den.Month, 1).AddMonths(1).AddDays(-1);
                    den = new DateTime(cuoi.Year, cuoi.Month, cuoi.Day, 23, 59, 59);

                    if (tu > den) { MessageBox.Show("Tháng bắt đầu không được lớn hơn tháng kết thúc!"); return; }

                    // 1. Chart
                    dataChart = bus.GetDoanhThuTheoKhoang_GroupThang(tu, den);
                    // Thời gian tu, den đã chuẩn để lấy Grid
                }
                else if (loai.Contains("Theo quý"))
                {
                    int nam = tu.Year;
                    int quy = cboQuyDT.SelectedIndex + 1;
                    tu = new DateTime(nam, (quy - 1) * 3 + 1, 1);
                    den = tu.AddMonths(3).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);

                    // 1. Chart
                    dataChart = bus.GetDoanhThuTheoNgay(tu, den);
                    // Thời gian tu, den đã chuẩn để lấy Grid
                }
                else // Theo ngày
                {
                    if (tu.Date > den.Date) { MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!"); return; }
                    den = den.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                    tu = tu.Date;

                    // 1. Chart
                    dataChart = bus.GetDoanhThuTheoNgay(tu, den);
                    // Thời gian tu, den đã chuẩn để lấy Grid
                }

                // --- BƯỚC 2: HIỂN THỊ LÊN GIAO DIỆN ---

                // A. Vẽ biểu đồ
                VeBieuDoMicrosoft(dataChart, title, chartDT);

                // B. Đổ dữ liệu vào DataGridView (QUAN TRỌNG: ĐOẠN NÀY LÚC NÃY BẠN THIẾU)
                // Gọi xuống BUS lấy danh sách chi tiết hóa đơn dựa vào thời gian đã tính toán ở trên
                dataGrid = bus.GetListHoaDon(tu, den);

                // Đổ vào Grid
                loadDanhSachDoanhThu(new BindingList<hoaDonDTO>(dataGrid));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thống kê doanh thu: " + ex.Message);
            }
        }

        // ===================== CHI TIÊU =====================
        public void setMacDinhChiTieu()
        {
            if (cboLoaiTKCT != null)
            {
                cboLoaiTKCT.Items.Clear();
                cboLoaiTKCT.Items.AddRange(new object[] {
                    "Theo ngày",
                    "Theo tháng",
                    "Theo quý",
                    "Theo năm",
                });
                cboLoaiTKCT.SelectedIndex = 0;
            }
            if (dtpBatDauCT != null) dtpBatDauCT.Value = DateTime.Now.AddMonths(-1);
            if (dtpKetThucCT != null) dtpKetThucCT.Value = DateTime.Now;
            if (cboQuyCT != null)
            {
                cboQuyCT.Items.Clear();
                cboQuyCT.Items.AddRange(new object[] { "Quý 1", "Quý 2", "Quý 3", "Quý 4" });
                cboQuyCT.SelectedIndex = 0;
            }
        }

        private void cboLoaiTKCT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLoaiTKCT == null || dtpBatDauCT == null || dtpKetThucCT == null || cboQuyCT == null) return;
            string chon = cboLoaiTKCT.SelectedItem.ToString();
            if (chon.Contains("Theo ngày"))
            {
                dtpBatDauCT.CustomFormat = "dd/MM/yyyy";
                dtpKetThucCT.CustomFormat = "dd/MM/yyyy";
                dtpBatDauCT.Enabled = dtpKetThucCT.Enabled = true;
                cboQuyCT.Enabled = false;
            }
            else if (chon.Contains("Theo tháng"))
            {
                dtpBatDauCT.CustomFormat = dtpKetThucCT.CustomFormat = "MM/yyyy";
                dtpBatDauCT.Enabled = dtpKetThucCT.Enabled = true;
                cboQuyCT.Enabled = false;
            }
            else if (chon.Contains("Theo quý"))
            {
                dtpBatDauCT.CustomFormat = "yyyy";
                dtpBatDauCT.Enabled = true;
                dtpKetThucCT.Enabled = false;
                cboQuyCT.Enabled = true;
            }
            else if (chon.Contains("Theo năm"))
            {
                dtpBatDauCT.CustomFormat = "yyyy";
                dtpBatDauCT.Enabled = true;
                dtpKetThucCT.Enabled = cboQuyCT.Enabled = false;
            }
        }

        // ===================== LƯƠNG NHÂN VIÊN =====================
        public void setMacDinhLuong()
        {
            if (cboLoaiTKNV != null)
            {
                cboLoaiTKNV.Items.Clear();
                cboLoaiTKNV.Items.AddRange(new object[] {
                    "Theo ngày",
                    "Theo tuần",
                    "Theo tháng",
                    "Theo quý",
                    "Theo năm",
                });
                cboLoaiTKNV.SelectedIndex = 0;
            }
            if (dtpBatDauNV != null) dtpBatDauNV.Value = DateTime.Now.AddMonths(-1);
            if (dtpKetThucNV != null) dtpKetThucNV.Value = DateTime.Now;
            if (cboQuyNV != null)
            {
                cboQuyNV.Items.Clear();
                cboQuyNV.Items.AddRange(new object[] { "Quý 1", "Quý 2", "Quý 3", "Quý 4" });
                cboQuyNV.SelectedIndex = 0;
            }
        }

        private void cboLoaiTKNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra null (thêm label12 vào check)
            if (cboLoaiTKNV == null || dtpBatDauNV == null || dtpKetThucNV == null || cboQuyNV == null || label12 == null) return;

            string chon = cboLoaiTKNV.SelectedItem.ToString();

            // Reset ComboBox chọn Tuần/Quý
            cboQuyNV.Items.Clear();

            if (chon.Contains("Theo tuần"))
            {
                // --- CẤU HÌNH CHO TUẦN ---
                dtpBatDauNV.CustomFormat = "MM/yyyy";
                dtpBatDauNV.Enabled = true;
                dtpKetThucNV.Enabled = false;

                // Hiện và đổi tên Label
                label12.Visible = true;
                label12.Text = "Tuần:";

                cboQuyNV.Enabled = true;
                cboQuyNV.Items.AddRange(new object[] {
            "Tuần 1 (Ngày 1-7)",
            "Tuần 2 (Ngày 8-14)",
            "Tuần 3 (Ngày 15-21)",
            "Tuần 4 (Ngày 22-Cuối tháng)"
        });
                cboQuyNV.SelectedIndex = 0;
            }
            else if (chon.Contains("Theo quý"))
            {
                // --- CẤU HÌNH CHO QUÝ ---
                dtpBatDauNV.CustomFormat = "yyyy";
                dtpBatDauNV.Enabled = true;
                dtpKetThucNV.Enabled = false;

                // Hiện và đổi tên Label
                label12.Visible = true;
                label12.Text = "Quý:";

                cboQuyNV.Enabled = true;
                cboQuyNV.Items.AddRange(new object[] { "Quý 1", "Quý 2", "Quý 3", "Quý 4" });
                cboQuyNV.SelectedIndex = 0;
            }
            else
            {
                // --- CÁC TRƯỜNG HỢP KHÁC (Ngày, Tháng, Năm) ---
                // Ẩn Label và ComboBox Tuần/Quý đi cho gọn
                label12.Visible = true;
                cboQuyNV.Enabled = false;

                if (chon.Contains("Theo ngày"))
                {
                    dtpBatDauNV.CustomFormat = "dd/MM/yyyy";
                    dtpKetThucNV.CustomFormat = "dd/MM/yyyy";
                    dtpBatDauNV.Enabled = dtpKetThucNV.Enabled = true;
                }
                else if (chon.Contains("Theo tháng"))
                {
                    dtpBatDauNV.CustomFormat = dtpKetThucNV.CustomFormat = "MM/yyyy";
                    dtpBatDauNV.Enabled = dtpKetThucNV.Enabled = true;
                }
                else if (chon.Contains("Theo năm"))
                {
                    dtpBatDauNV.CustomFormat = "yyyy";
                    dtpBatDauNV.Enabled = true;
                    dtpKetThucNV.Enabled = false;
                }
            }
        }

        // ===================== HÀM VẼ BIỂU ĐỒ CHUNG =====================
        private void VeBieuDoMicrosoft(List<thongKeDTO> data, string title, Chart chartTK)
        {
            if (chartTK == null) return;

            chartTK.Series.Clear();
            chartTK.ChartAreas.Clear();
            chartTK.Titles.Clear();

            ChartArea ca = new ChartArea("ca");
            ca.AxisX.Interval = 1;
            ca.AxisX.LabelStyle.Angle = -45;
            ca.AxisX.MajorGrid.Enabled = false;
            //ca.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            ca.AxisY.MajorGrid.Enabled = false;
            chartTK.ChartAreas.Add(ca);

            Series s = new Series("Số tiền");
            s.ChartType = SeriesChartType.Column;
            s.IsValueShownAsLabel = true;
            s.LabelFormat = "N0"; // không phân biệt locale
            s.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
            chartTK.Series.Add(s);

            chartTK.Titles.Add(title);

            // Nếu dữ liệu rỗng -> show message
            if (data == null || data.Count == 0)
            {
                // thêm 1 điểm 0 để còn hiển thị chart trống
                s.Points.AddXY("Không có dữ liệu", 0);
                return;
            }

            // thêm dữ liệu
            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                decimal val = item.GiaTri;
                string label = item.Nhan;
                int idx = s.Points.AddXY(label, Convert.ToDouble(val));
                // tooltip & label format
                s.Points[idx].ToolTip = $"{label}: {FormatCurrency(val)}";
                s.Points[idx].Label = FormatCurrency(val);
            }

            // điều chỉnh trục Y để dễ nhìn (tối thiểu 0)
            double max = s.Points.Max(p => p.YValues[0]);
            ca.AxisY.Minimum = 0;
            ca.AxisY.Maximum = Math.Max(max * 1.1, 1); // padding 10%
            ca.AxisY.LabelStyle.Format = "N0";

            chartTK.Invalidate();
        }

        private string FormatCurrency(decimal value)
        {
            // format VNĐ: 1.234.567
            try
            {
                return string.Format("{0:N0}", value) + " đ";
            }
            catch { return value.ToString(); }
        }

        // ===================== FOCUS KHI ĐỔI TAB =====================
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1 == null) return;
            if (tabControl1.SelectedTab == tabPage1) BeginInvoke((MethodInvoker)(() => { if (btnThongKeDT != null) btnThongKeDT.Focus(); }));
            else if (tabControl1.SelectedTab == tabPage2) BeginInvoke((MethodInvoker)(() => { if (btnThongKeCT != null) btnThongKeCT.Focus(); }));
            else if (tabControl1.SelectedTab == tabPage3) BeginInvoke((MethodInvoker)(() => { if (btnThongKenNV != null) btnThongKenNV.Focus(); }));
        }

        private void btnThongKeCT_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (cboLoaiTKCT == null || dtpBatDauCT == null || dtpKetThucCT == null || chartCT == null) return;

                string loai = cboLoaiTKCT.SelectedItem.ToString();
                DateTime tu = dtpBatDauCT.Value;
                DateTime den = dtpKetThucCT.Value;
                thongKeBUS bus = new thongKeBUS();

                // Khai báo 2 list riêng biệt
                List<thongKeDTO> dataChart = new List<thongKeDTO>(); // Cho biểu đồ
                List<phieuNhapDTO> dataGrid = new List<phieuNhapDTO>(); // Cho bảng

                string title = "THỐNG KÊ CHI TIÊU";

                // --- XỬ LÝ THỜI GIAN ---
                if (loai.Contains("Theo năm"))
                {
                    int nam = tu.Year;
                    dataChart = bus.GetChiTieuTheoThang(nam);
                    title += " NĂM " + nam;

                    // Set ngày để lấy list phiếu nhập cả năm
                    tu = new DateTime(nam, 1, 1);
                    den = new DateTime(nam, 12, 31, 23, 59, 59);
                }
                else if (loai.Contains("Theo tháng"))
                {
                    tu = new DateTime(tu.Year, tu.Month, 1);
                    var cuoi = new DateTime(den.Year, den.Month, 1).AddMonths(1).AddDays(-1);
                    den = new DateTime(cuoi.Year, cuoi.Month, cuoi.Day, 23, 59, 59);
                    if (tu > den) { MessageBox.Show("Tháng bắt đầu không được lớn hơn tháng kết thúc!"); return; }

                    dataChart = bus.GetChiTieuTheoKhoang_GroupThang(tu, den);
                    // tu và den đã chuẩn để lấy list
                }
                else if (loai.Contains("Theo quý"))
                {
                    int nam = tu.Year;
                    int quy = cboQuyCT.SelectedIndex + 1;
                    tu = new DateTime(nam, (quy - 1) * 3 + 1, 1);
                    den = tu.AddMonths(3).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);

                    dataChart = bus.GetChiTieuTheoNgay(tu, den);
                }
                else // Theo ngày
                {
                    if (tu.Date > den.Date) { MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!"); return; }
                    den = den.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                    tu = tu.Date;

                    dataChart = bus.GetChiTieuTheoNgay(tu, den);
                }

                // --- HIỂN THỊ ---

                // 1. Vẽ biểu đồ
                VeBieuDoMicrosoft(dataChart, title, chartCT);

                // 2. Đổ dữ liệu vào bảng (ĐÂY LÀ PHẦN BẠN THIẾU)
                dataGrid = bus.GetListPhieuNhap(tu, den);
                loadDanhSachThongKe(new BindingList<phieuNhapDTO>(dataGrid));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thống kê chi tiêu: " + ex.Message);
            }
        }

        private void btnThongKenNV_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (cboLoaiTKNV == null || dtpBatDauNV == null || dtpKetThucNV == null || chartNV == null) return;

                string loai = cboLoaiTKNV.SelectedItem.ToString();
                DateTime tu = dtpBatDauNV.Value;
                DateTime den = dtpKetThucNV.Value;
                thongKeBUS bus = new thongKeBUS();

                // --- 1. XỬ LÝ THỜI GIAN ---
                if (loai.Contains("Theo tuần"))
                {
                    int thang = tu.Month;
                    int nam = tu.Year;
                    int tuanIndex = cboQuyNV.SelectedIndex; // 0=Tuần 1, 1=Tuần 2...

                    if (tuanIndex == 0) // Tuần 1: 1 -> 7
                    {
                        tu = new DateTime(nam, thang, 1);
                        den = new DateTime(nam, thang, 7, 23, 59, 59);
                    }
                    else if (tuanIndex == 1) // Tuần 2: 8 -> 14
                    {
                        tu = new DateTime(nam, thang, 8);
                        den = new DateTime(nam, thang, 14, 23, 59, 59);
                    }
                    else if (tuanIndex == 2) // Tuần 3: 15 -> 21
                    {
                        tu = new DateTime(nam, thang, 15);
                        den = new DateTime(nam, thang, 21, 23, 59, 59);
                    }
                    else // Tuần 4: 22 -> Cuối tháng
                    {
                        tu = new DateTime(nam, thang, 22);
                        // Tìm ngày cuối cùng của tháng đó
                        int daysInMonth = DateTime.DaysInMonth(nam, thang);
                        den = new DateTime(nam, thang, daysInMonth, 23, 59, 59);
                    }
                }
                else if (loai.Contains("Theo năm"))
                {
                    tu = new DateTime(tu.Year, 1, 1);
                    den = new DateTime(tu.Year, 12, 31, 23, 59, 59);
                }
                else if (loai.Contains("Theo tháng"))
                {
                    tu = new DateTime(tu.Year, tu.Month, 1);
                    int daysInMonth = DateTime.DaysInMonth(den.Year, den.Month);
                    den = new DateTime(den.Year, den.Month, daysInMonth, 23, 59, 59);
                }
                else if (loai.Contains("Theo quý"))
                {
                    int nam = tu.Year;
                    int quy = cboQuyNV.SelectedIndex + 1;
                    tu = new DateTime(nam, (quy - 1) * 3 + 1, 1);
                    den = tu.AddMonths(3).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                }
                else // Theo ngày
                {
                    tu = tu.Date;
                    den = den.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                }

                // --- 2. GỌI BUS & HIỂN THỊ (Giữ nguyên) ---
                List<topSanPhamDTO> data = bus.GetTopSanPham(tu, den);

                // Kiểm tra dữ liệu rỗng
                if (data == null || data.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu sản phẩm bán ra trong khoảng thời gian này!");
                    chartNV.Series.Clear();
                    tableLuongNV.DataSource = null;
                    return;
                }

                string title = "TOP SẢN PHẨM BÁN CHẠY " + loai.ToUpper();
                // Nếu là tuần thì thêm chi tiết vào tiêu đề cho rõ
                if (loai.Contains("Theo tuần"))
                {
                    title += " (" + tu.ToString("dd/MM") + " - " + den.ToString("dd/MM") + ")";
                }

                VeBieuDoSanPham(data, title, chartNV);
                loadDanhSachTopSP(new BindingList<topSanPhamDTO>(data));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thống kê sản phẩm: " + ex.Message);
            }
        }
        private void VeBieuDoSanPham(List<topSanPhamDTO> data, string title, Chart chart)
        {
            if (chart == null) return;
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Clear();

            ChartArea ca = new ChartArea("ca");
            ca.AxisX.Interval = 1;
            ca.AxisX.LabelStyle.Angle = -45; // Nghiêng chữ cho dễ đọc tên SP
            chart.ChartAreas.Add(ca);

            Series s = new Series("Số lượng");
            s.ChartType = SeriesChartType.Column; // Hoặc Bar để vẽ ngang nếu tên dài
            s.IsValueShownAsLabel = true;
            chart.Series.Add(s);
            chart.Titles.Add(title);

            // Chỉ vẽ top 10 món để biểu đồ không bị rối
            var topData = data.Take(10).ToList();

            foreach (var item in topData)
            {
                s.Points.AddXY(item.TenSP, item.SoLuongBan);
            }
        }

        // Hàm load Grid cho sản phẩm
        private void loadDanhSachTopSP(BindingList<topSanPhamDTO> ds)
        {
            tableLuongNV.AutoGenerateColumns = false;
            tableLuongNV.DataSource = ds;
            tableLuongNV.Columns.Clear();

            // Cột 1: Mã SP
            tableLuongNV.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaSP",
                HeaderText = "Mã SP",
            });

            // Cột 2: Tên SP
            tableLuongNV.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenSP",
                HeaderText = "Tên Sản Phẩm",
            });

            // Cột 3: Số Lượng
            tableLuongNV.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoLuongBan",
                HeaderText = "Số Lượt Bán"
            });

            // Cột 4: Doanh Thu
            tableLuongNV.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DoanhThu",
                HeaderText = "Doanh Thu",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            tableLuongNV.Refresh();
            loadFontVaChu(tableLuongNV);
        }

        private void btnExcelDoanhThu_Click(object sender, EventArgs e)
        {
            XuatRaExcel(tableDoanhThu, "BaoCaoDoanhThu");
        }

        private void XuatRaExcel(DataGridView dgv, string tenFile)
        {
            if (dgv.Rows.Count == 0 || dgv.Columns.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 1. Tạo đối tượng Excel
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Add();
            Excel.Worksheet xlWorksheet = (Excel.Worksheet)xlWorkbook.Sheets[1];

            try
            {
                // 2. Xuất tiêu đề cột
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    // Excel bắt đầu từ 1, mảng C# bắt đầu từ 0 nên phải +1
                    xlWorksheet.Cells[1, i + 1] = dgv.Columns[i].HeaderText;
                }

                // Định dạng dòng tiêu đề (In đậm, Nền xám)
                Excel.Range headerRange = xlWorksheet.Range[xlWorksheet.Cells[1, 1], xlWorksheet.Cells[1, dgv.Columns.Count]];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                // 3. Xuất dữ liệu từng dòng
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    for (int j = 0; j < dgv.Columns.Count; j++)
                    {
                        if (dgv.Rows[i].Cells[j].Value != null)
                        {
                            // Thêm dấu ' trước dữ liệu để Excel hiểu là Text (tránh lỗi mất số 0 ở đầu ngày tháng hoặc mã số)
                            xlWorksheet.Cells[i + 2, j + 1] = "'" + dgv.Rows[i].Cells[j].Value.ToString();
                        }
                    }
                }

                // 4. Tự động giãn cột cho đẹp
                xlWorksheet.Columns.AutoFit();

                // 5. Mở hộp thoại lưu file
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = tenFile + "_" + DateTime.Now.ToString("ddMMyyyy_HHmm");
                saveFileDialog.Filter = "Excel Files|*.xlsx;*.xls";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    xlWorkbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở file sau khi lưu (tùy chọn)
                    // System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi xuất Excel: " + ex.Message);
            }
            finally
            {
                // 6. Dọn dẹp bộ nhớ (Quan trọng để không bị treo tiến trình Excel ngầm)
                xlWorkbook.Close(false);
                xlApp.Quit();
                Marshal.ReleaseComObject(xlWorksheet);
                Marshal.ReleaseComObject(xlWorkbook);
                Marshal.ReleaseComObject(xlApp);
            }
        }
    }
}
