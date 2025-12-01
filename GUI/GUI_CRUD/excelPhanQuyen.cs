using BUS;
using DTO;
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
    public partial class excelPhanQuyen : Form
    {
        public excelPhanQuyen()
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void btnNhapExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Excel Files|*.xlsx;*.xls";
            open.Title = "Chọn file Excel Phân quyền";

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 1. Đọc file Excel (Phải gọi rõ GUI.EXCEL để không nhầm với tên Form)
                    BindingList<phanquyenDTO> dsExcel = GUI.EXCEL.excelPhanQuyen.Import(open.FileName);

                    // 2. Gọi BUS Phân Quyền (Sửa lại chỗ này, không dùng khachHangBUS)
                    phanquyenBUS bus = new phanquyenBUS();

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

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel Files|*.xlsx";
            save.FileName = "DanhSachPhanQuyen.xlsx";
            save.Title = "Lưu file Excel Phân quyền";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 1. Lấy dữ liệu từ DB lên
                    phanquyenBUS bus = new phanquyenBUS();
                    BindingList<phanquyenDTO> dsDB = bus.LayDanhSach();

                    // 2. Gọi hàm Export (Phải gọi rõ GUI.EXCEL)
                    GUI.EXCEL.excelPhanQuyen.Export(dsDB, save.FileName);

                    // (Thông báo thành công đã nằm trong hàm Export rồi, hoặc bạn tự hiện ở đây cũng được)
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
