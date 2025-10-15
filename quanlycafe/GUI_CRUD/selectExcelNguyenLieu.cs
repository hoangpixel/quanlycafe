using quanlycafe.BUS;
using quanlycafe.EXCEL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlycafe.GUI_CRUD
{
    public partial class selectExcelNguyenLieu : Form
    {
        public selectExcelNguyenLieu()
        {
            InitializeComponent();
        }

        private void btnNhapExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Excel Files|*.xlsx;*.xls";

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var ds = excelNguyenLieu.Import(open.FileName);
                    nguyenLieuBUS bus = new nguyenLieuBUS();
                    bus.NhapExcelThongMinh(ds);

                    MessageBox.Show("Nhập Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            save.Filter = "Excel file (*.xlsx)|*.xlsx";
            save.FileName = "DanhSachNguyenLieu.xlsx";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    nguyenLieuBUS bus = new nguyenLieuBUS();
                    var ds = bus.docDSNguyenLieu(); // 🔹 Lấy dữ liệu từ DB

                    excelNguyenLieu.Export(ds, save.FileName); // 🔹 Xuất ra file Excel

                    MessageBox.Show("✅ Xuất Excel thành công!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("❌ Lỗi khi xuất Excel: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
