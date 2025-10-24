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
    public partial class selectExcelCongThuc : Form
    {
        public selectExcelCongThuc()
        {
            InitializeComponent();
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel file (*.xlsx)|*.xlsx";
            save.FileName = "DanhSachCongThuc.xlsx";

            if (save.ShowDialog() == DialogResult.OK)
            {
                congThucBUS bus = new congThucBUS();
                excelCongThuc.Export(bus.docTatCaCongThuc(), save.FileName);
                MessageBox.Show("Xuất Excel thành công!");
            }
        }

        private void btnNhapExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Excel Files|*.xlsx;*.xls";

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var ds = excelCongThuc.Import(open.FileName);
                    congThucBUS bus = new congThucBUS();
                    bus.NhapExcelThongMinh(ds);
                    MessageBox.Show("Nhập Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi nhập Excel: " + ex.Message);
                }
            }
        }
    }
}
