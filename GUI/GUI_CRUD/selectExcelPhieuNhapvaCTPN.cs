using BUS;
using FONTS;
using GUI.EXCEL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class selectExcelPhieuNhapvaCTPN : Form
    {
        private phieuNhapBUS busPhieuNhap = new phieuNhapBUS();
        public selectExcelPhieuNhapvaCTPN()
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void btnNhapPN_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Excel Files|*.xlsx;*.xls";
            open.Title = "Chọn file Excel tổng hợp (2 Sheet)";

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 1. Đọc dữ liệu thô từ Excel (Dùng hàm đọc 2 Sheet mới viết)
                    var rawData = GUI.EXCEL.excelPhieuNhap.ImportHaiSheet(open.FileName);

                    // 2. Kiểm tra dữ liệu rỗng TRƯỚC khi xử lý
                    if (rawData.Count == 0)
                    {
                        MessageBox.Show("File Excel trống hoặc sai định dạng (thiếu Sheet PhieuNhap/ChiTiet)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 3. Đẩy vào BUS để xử lý nghiệp vụ (Gom nhóm, Validate, Insert)
                    // CHỈ GỌI 1 LẦN DUY NHẤT Ở ĐÂY
                    busPhieuNhap.NhapExcelGop(rawData);

                    // 4. (Tùy chọn) Đóng form này sau khi nhập xong
                    // this.DialogResult = DialogResult.OK;
                    // this.Close(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi nhập liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnNhapCTPN_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel file (*.xlsx)|*.xlsx";
            save.FileName = $"DanhSachPhieuNhapVaCTPN.xlsx";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Lấy danh sách mới nhất từ DB
                    var listPhieu = busPhieuNhap.LayDanhSach();

                    if (listPhieu.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu phiếu nhập để xuất!", "Thông báo");
                        return;
                    }

                    // Gọi hàm Export ra 2 Sheet
                    GUI.EXCEL.excelPhieuNhap.ExportHaiSheet(listPhieu, save.FileName);

                    MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}