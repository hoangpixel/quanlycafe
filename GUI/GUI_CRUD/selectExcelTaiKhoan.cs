using BUS;
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
    public partial class selectExcelTaiKhoan : Form
    {
        public selectExcelTaiKhoan()
        {
            InitializeComponent();
        }
        private taikhoanBUS busTaiKhoan = new taikhoanBUS();

        private void btnNhapExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 1. Đọc file Excel lên List DTO
                    var listTuExcel = excelTaiKhoan.Import(openFileDialog.FileName);

                    if (listTuExcel.Count > 0)
                    {
                        // 2. Gọi BUS xử lý thông minh
                        string ketQua = busTaiKhoan.NhapExcelThongMinh(listTuExcel);

                        // 3. Thông báo và làm mới lưới
                        this.DialogResult = DialogResult.OK;
                        MessageBox.Show(ketQua, "Kết quả nhập liệu");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            // 1. Lấy dữ liệu mới nhất từ BUS
            var dsTaiKhoan = busTaiKhoan.LayDanhSach();

            // Kiểm tra nếu danh sách rỗng
            if (dsTaiKhoan == null || dsTaiKhoan.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu tài khoản để xuất!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Cấu hình hộp thoại lưu file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Xuất danh sách tài khoản ra Excel";
            saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            // Tự động đặt tên file kèm ngày giờ để không bị trùng
            saveFileDialog.FileName = "DanhSachTaiKhoan.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 3. Gọi hàm Export từ Helper (excelTaiKhoan)
                    excelTaiKhoan.Export(dsTaiKhoan, saveFileDialog.FileName);

                    // 4. Thông báo thành công và hỏi mở file
                    DialogResult result = MessageBox.Show(
                        $"Xuất file thành công!\nĐường dẫn: {saveFileDialog.FileName}\n\nBạn có muốn mở file ngay không?",
                        "Thành công",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Mở file bằng phần mềm mặc định (Excel)
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
