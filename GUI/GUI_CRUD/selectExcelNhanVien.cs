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
    public partial class selectExcelNhanVien : Form
    {
        public selectExcelNhanVien()
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void btnNhapExcel_Click(object sender, EventArgs e)
        {

        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {

        }

        private void btnNhapExcel_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Excel Files|*.xlsx;*.xls";

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    BindingList<nhanVienDTO> ds = excelNhanVien.Import(open.FileName);
                    nhanVienBUS bus = new nhanVienBUS();
                    bus.NhapExcelThongMinh(ds);
                    MessageBox.Show("Nhập Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi nhập Excel: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btnXuatExcel_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel file (*.xlsx)|*.xlsx";
            save.FileName = "DanhSachNhanVien.xlsx";

            if (save.ShowDialog() == DialogResult.OK)
            {
                nhanVienBUS bus = new nhanVienBUS();
                excelNhanVien.Export(bus.LayDanhSach(), save.FileName);
            }
        }
    }
}
