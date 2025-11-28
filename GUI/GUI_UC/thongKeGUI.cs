using BUS;
using DAO;
using DTO;
using FONTS;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GUI.GUI_UC
{
    public partial class thongKeGUI : UserControl
    {
        public thongKeGUI()
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            setMacDinhDoanhThu();
            this.Load += (s, e) =>
            {
                this.BeginInvoke((MethodInvoker)delegate { this.btnThongKeDT.Focus(); });
            };
        }

        public void setMacDinhDoanhThu()
        {
            System.Drawing.Font fontChuan = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            dtpBatDauDT.Font = fontChuan;
            dtpKetThucDT.Font = fontChuan;
            cboLoaiTKDT.SelectedIndex = 0;
            cboQuyDT.SelectedIndex = 0;
            dtpBatDauDT.Value = DateTime.Now.AddMonths(-1);
            dtpKetThucDT.Value = DateTime.Now;
        }

        private void cboLoaiTKDT_SelectedIndexChanged(object sender, EventArgs e)
        {
            string chonLoai = cboLoaiTKDT.SelectedItem.ToString();
            if (chonLoai.Contains("Theo ngày"))
            {
                dtpBatDauDT.Enabled = true;
                dtpBatDauDT.CustomFormat = "dd/MM/yyyy";

                dtpKetThucDT.Enabled = true;
                dtpKetThucDT.CustomFormat = "dd/MM/yyyy";

                cboQuyDT.Enabled = false;
            }
            else if (chonLoai.Contains("Theo tháng") || chonLoai.Contains("Theo khoảng"))
            {
                dtpBatDauDT.Enabled = true;
                dtpBatDauDT.CustomFormat = "MM/yyyy";

                dtpKetThucDT.Enabled = true;
                dtpKetThucDT.CustomFormat = "MM/yyyy";

                cboQuyDT.Enabled = false;
            }
            else if (chonLoai.Contains("Theo quý"))
            {
                dtpBatDauDT.Enabled = true;
                dtpBatDauDT.CustomFormat = "yyyy";

                dtpKetThucDT.Enabled = false;
                cboQuyDT.Enabled = true;
            }
            else if (chonLoai.Contains("Theo năm"))
            {
                dtpBatDauDT.Enabled = true;
                dtpBatDauDT.CustomFormat = "yyyy";

                dtpKetThucDT.Enabled = false;
                cboQuyDT.Enabled = false;
            }
        }

        private void btnThongKeDT_Click(object sender, EventArgs e)
        {
            string loaiTK = cboLoaiTKDT.SelectedItem.ToString();
            DateTime tuNgay = dtpBatDauDT.Value;
            DateTime denNgay = dtpKetThucDT.Value;

            thongKeBUS bus = new thongKeBUS();
            List<thongKeDTO> data = new List<thongKeDTO>();
            string title = "THỐNG KÊ DOANH THU";

            if (loaiTK.Contains("Theo năm"))
            {
                data = bus.GetDoanhThuTheoThang(tuNgay.Year);
            }
            else if (loaiTK.Contains("Theo khoảng") || loaiTK.Contains("Theo tháng"))
            {
                // Tính toán ngày đầu tháng và cuối tháng
                tuNgay = new DateTime(tuNgay.Year, tuNgay.Month, 1);

                // Tính ngày cuối cùng của tháng "Đến ngày"
                DateTime cuoiThang = new DateTime(denNgay.Year, denNgay.Month, 1).AddMonths(1).AddDays(-1);
                denNgay = new DateTime(cuoiThang.Year, cuoiThang.Month, cuoiThang.Day, 23, 59, 59);

                // --- 👇 KIỂM TRA LỖI Ở ĐÂY 👇 ---
                if (tuNgay > denNgay)
                {
                    MessageBox.Show("Tháng bắt đầu không được lớn hơn tháng kết thúc!", "Lỗi thời gian", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Dừng lại, không chạy tiếp
                }

                data = bus.GetDoanhThuTheoKhoang_GroupThang(tuNgay, denNgay);
            }
            else if (loaiTK.Contains("Theo quý"))
            {
                int nam = tuNgay.Year;
                int quy = cboQuyDT.SelectedIndex + 1;
                int thangDau = (quy - 1) * 3 + 1;

                tuNgay = new DateTime(nam, thangDau, 1);
                DateTime cuoiQuy = tuNgay.AddMonths(3).AddDays(-1);
                denNgay = new DateTime(cuoiQuy.Year, cuoiQuy.Month, cuoiQuy.Day, 23, 59, 59);

                data = bus.GetDoanhThuTheoNgay(tuNgay, denNgay);
            }
            else // Theo ngày (Xem chi tiết)
            {
                // Chỉ lấy phần Ngày (Date) để so sánh, bỏ qua giờ phút
                if (tuNgay.Date > denNgay.Date)
                {
                    MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!", "Lỗi thời gian", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Dừng lại
                }

                // Set giờ chót ngày hôm đó
                denNgay = new DateTime(denNgay.Year, denNgay.Month, denNgay.Day, 23, 59, 59);
                data = bus.GetDoanhThuTheoNgay(tuNgay, denNgay);
            }

            VeBieuDoMicrosoft(data, title, chartDT);
        }

        private void VeBieuDoMicrosoft(List<thongKeDTO> data, string title, Chart chartTK)
        {
            // 1. Dọn sạch biểu đồ cũ trước khi vẽ mới
            chartTK.Series.Clear();
            chartTK.Titles.Clear();

            // 2. Đặt tên tiêu đề biểu đồ
            Title chartTitle = chartTK.Titles.Add(title);
            chartTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            chartTitle.ForeColor = Color.DarkBlue;

            // 3. Tạo một Series mới (Loại Cột)
            System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series("DuLieu");
            series.ChartType = SeriesChartType.Column; // Chọn loại biểu đồ Cột (Column)

            // Trang trí màu sắc
            series.Color = Color.SteelBlue; // Màu xanh đẹp
            series.BackGradientStyle = GradientStyle.TopBottom; // Hiệu ứng đổ màu
            series.BackSecondaryColor = Color.LightSkyBlue;

            // Hiển thị số tiền trên đầu cột
            series.IsValueShownAsLabel = true;
            series.Font = new Font("Arial", 10, FontStyle.Bold);
            series.LabelFormat = "N0"; // Format số tiền có dấu phẩy (VD: 100,000)

            // 4. Đổ dữ liệu từ List vào Biểu đồ
            foreach (var item in data)
            {
                // AddXY(Trục Ngang, Trục Dọc)
                series.Points.AddXY(item.Nhan, item.GiaTri);
            }

            // 5. Thêm Series vào Chart
            chartTK.Series.Add(series);

            // 6. Cấu hình trục (Cho đẹp hơn)
            var chartArea = chartTK.ChartAreas[0];

            // Trục X (Thời gian)
            chartArea.AxisX.Title = "Thời gian";
            chartArea.AxisX.Interval = 1; // Bắt buộc hiện hết tên các cột (không được nhảy cóc)
            chartArea.AxisX.MajorGrid.Enabled = false; // Tắt lưới dọc nhìn cho thoáng

            // Trục Y (Tiền)
            chartArea.AxisY.Title = "Số tiền (VNĐ)";
            chartArea.AxisY.LabelStyle.Format = "N0"; // Số trục Y cũng có dấu phẩy
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray; // Lưới ngang màu mờ
            chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash; // Lưới nét đứt
        }


        }
    }
