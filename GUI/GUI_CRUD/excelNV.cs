using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DTO;
using FONTS;

namespace GUI.GUI_CRUD
{
    public partial class excelNV : Form
    {
        public excelNV()
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel Files|*.xlsx";
            save.FileName = "DanhSachNhanVien. xlsx";
            save.Title = "Lưu file Excel Nhân viên";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 1. Lấy dữ liệu từ DB lên
                    nhanVienBUS bus = new nhanVienBUS();
                    BindingList<nhanVienDTO> dsDB = bus.LayDanhSach();

                    // 2. Gọi hàm Export (Phải gọi rõ GUI. EXCEL)
                    GUI.EXCEL.excelNhanVien.Export(dsDB, save.FileName);

                    // (Thông báo thành công đã nằm trong hàm Export rồi, hoặc bạn tự hiện ở đây cũng được)
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnNhapExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Excel Files|*.xlsx;*.xls";
            open.Title = "Chọn file Excel Nhân viên";

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 1. Đọc file Excel (Phải gọi rõ GUI.EXCEL để không nhầm với tên Form)
                    BindingList<nhanVienDTO> dsExcel = GUI.EXCEL.excelNhanVien.Import(open.FileName);

                    // 2. Gọi BUS Nhân Viên
                    nhanVienBUS bus = new nhanVienBUS();

                    // 3. Nhận kết quả trả về (String) để thông báo chi tiết
                    string ketQua = bus.NhapExcelThongMinh(dsExcel);

                    // 4. Hiển thị kết quả
                    MessageBox.Show(ketQua, "Kết quả nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi nhập Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}