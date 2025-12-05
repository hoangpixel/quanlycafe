using BUS;
using DTO;
using FONTS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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
            if (btnThongKeCT != null) btnThongKeCT.Click += btnThongKeCT_Click;

            if (cboLoaiTKNV != null) cboLoaiTKNV.SelectedIndexChanged += cboLoaiTKNV_SelectedIndexChanged;
            if (btnThongKenNV != null) btnThongKenNV.Click += btnThongKenNV_Click;
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
                List<thongKeDTO> data = new List<thongKeDTO>();
                string title = "THỐNG KÊ DOANH THU";

                if (loai.Contains("Theo năm"))
                {
                    int nam = tu.Year;
                    data = bus.GetDoanhThuTheoThang(nam);
                    title += " NĂM " + nam;
                }
                else if (loai.Contains("Theo tháng"))
                {
                    tu = new DateTime(tu.Year, tu.Month, 1);
                    var cuoi = new DateTime(den.Year, den.Month, 1).AddMonths(1).AddDays(-1);
                    den = new DateTime(cuoi.Year, cuoi.Month, cuoi.Day, 23, 59, 59);
                    if (tu > den) { MessageBox.Show("Tháng bắt đầu không được lớn hơn tháng kết thúc!"); return; }
                    data = bus.GetDoanhThuTheoKhoang_GroupThang(tu, den);
                }
                else if (loai.Contains("Theo quý"))
                {
                    int nam = tu.Year;
                    int quy = cboQuyDT.SelectedIndex + 1;
                    tu = new DateTime(nam, (quy - 1) * 3 + 1, 1);
                    den = tu.AddMonths(3).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                    data = bus.GetDoanhThuTheoNgay(tu, den);
                }
                else // Theo ngày
                {
                    if (tu.Date > den.Date) { MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!"); return; }
                    den = den.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                    data = bus.GetDoanhThuTheoNgay(tu, den);
                }

                VeBieuDoMicrosoft(data, title, chartDT);
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

        private void btnThongKeCT_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboLoaiTKCT == null || dtpBatDauCT == null || dtpKetThucCT == null || chartCT == null) return;

                string loai = cboLoaiTKCT.SelectedItem.ToString();
                DateTime tu = dtpBatDauCT.Value;
                DateTime den = dtpKetThucCT.Value;
                thongKeBUS bus = new thongKeBUS();
                List<thongKeDTO> data = new List<thongKeDTO>();
                string title = "THỐNG KÊ CHI TIÊU";

                if (loai.Contains("Theo năm"))
                {
                    int nam = tu.Year;
                    data = bus.GetChiTieuTheoThang(nam);
                    title += " NĂM " + nam;
                }
                else if (loai.Contains("Theo tháng"))
                {
                    tu = new DateTime(tu.Year, tu.Month, 1);
                    var cuoi = new DateTime(den.Year, den.Month, 1).AddMonths(1).AddDays(-1);
                    den = new DateTime(cuoi.Year, cuoi.Month, cuoi.Day, 23, 59, 59);
                    if (tu > den) { MessageBox.Show("Tháng bắt đầu không được lớn hơn tháng kết thúc!"); return; }
                    data = bus.GetChiTieuTheoKhoang_GroupThang(tu, den);
                }
                else if (loai.Contains("Theo quý"))
                {
                    int nam = tu.Year;
                    int quy = cboQuyCT.SelectedIndex + 1;
                    tu = new DateTime(nam, (quy - 1) * 3 + 1, 1);
                    den = tu.AddMonths(3).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                    data = bus.GetChiTieuTheoNgay(tu, den);
                }
                else // Theo ngày
                {
                    if (tu.Date > den.Date) { MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!"); return; }
                    den = den.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                    data = bus.GetChiTieuTheoNgay(tu, den);
                }

                VeBieuDoMicrosoft(data, title, chartCT);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thống kê chi tiêu: " + ex.Message);
            }
        }

        // ===================== LƯƠNG NHÂN VIÊN =====================
        public void setMacDinhLuong()
        {
            if (cboLoaiTKNV != null)
            {
                cboLoaiTKNV.Items.Clear();
                cboLoaiTKNV.Items.AddRange(new object[] {
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
            if (cboLoaiTKNV == null || dtpBatDauNV == null || dtpKetThucNV == null || cboQuyNV == null) return;
            string chon = cboLoaiTKNV.SelectedItem.ToString();

            if (chon.Contains("Theo tháng"))
            {
                dtpBatDauNV.CustomFormat = dtpKetThucNV.CustomFormat = "MM/yyyy";
                dtpBatDauNV.Enabled = dtpKetThucNV.Enabled = true;
                cboQuyNV.Enabled = false;
            }
            else if (chon.Contains("Theo quý"))
            {
                dtpBatDauNV.CustomFormat = "yyyy";
                dtpBatDauNV.Enabled = true;
                dtpKetThucNV.Enabled = false;
                cboQuyNV.Enabled = true;
            }
            else if (chon.Contains("Theo năm"))
            {
                dtpBatDauNV.CustomFormat = "yyyy";
                dtpBatDauNV.Enabled = true;
                dtpKetThucNV.Enabled = cboQuyNV.Enabled = false;
            }
        }

        private void btnThongKenNV_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboLoaiTKNV == null || dtpBatDauNV == null || dtpKetThucNV == null || chartNV == null) return;

                string loai = cboLoaiTKNV.SelectedItem.ToString();
                DateTime tu = dtpBatDauNV.Value;
                DateTime den = dtpKetThucNV.Value;
                thongKeBUS bus = new thongKeBUS();
                List<thongKeDTO> data = new List<thongKeDTO>();
                decimal tongLuongThang = bus.GetTongLuong(); // giả định: tổng lương cố định/tháng
                string title = "THỐNG KÊ LƯƠNG NHÂN VIÊN";

                if (loai.Contains("Theo năm"))
                {
                    int nam = tu.Year;
                    for (int i = 1; i <= 12; i++)
                        data.Add(new thongKeDTO { Nhan = $"Tháng {i}", GiaTri = tongLuongThang });
                    title += " NĂM " + nam;
                }
                else if (loai.Contains("Theo tháng"))
                {
                    tu = new DateTime(tu.Year, tu.Month, 1);
                    var tmp = new DateTime(den.Year, den.Month, 1).AddMonths(1).AddDays(-1);
                    den = new DateTime(tmp.Year, tmp.Month, tmp.Day, 23, 59, 59);
                    if (tu > den) { MessageBox.Show("Thời gian không hợp lệ!"); return; }

                    var current = tu;
                    while (current <= den)
                    {
                        data.Add(new thongKeDTO { Nhan = current.ToString("MM/yyyy"), GiaTri = tongLuongThang });
                        current = current.AddMonths(1);
                    }
                }
                else if (loai.Contains("Theo quý"))
                {
                    int quy = cboQuyNV.SelectedIndex + 1;
                    data.Add(new thongKeDTO { Nhan = $"Quý {quy}/{tu.Year}", GiaTri = tongLuongThang * 3 });
                }
                else // Theo ngày (chia đều 30 ngày)
                {
                    if (tu.Date > den.Date) { MessageBox.Show("Ngày không hợp lệ!"); return; }
                    decimal luongNgay = tongLuongThang / 30m;
                    for (DateTime d = tu.Date; d <= den.Date; d = d.AddDays(1))
                        data.Add(new thongKeDTO { Nhan = d.ToString("dd/MM"), GiaTri = luongNgay });
                }

                VeBieuDoMicrosoft(data, title, chartNV);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thống kê lương: " + ex.Message);
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
            ca.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
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
    }
}
