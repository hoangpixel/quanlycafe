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
    public partial class selectExcelNhaCungCap : Form
    {
        public selectExcelNhaCungCap()
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void btnNhapExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Excel Files|*.xlsx;*.xls";

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    BindingList<nhaCungCapDTO> ds = excelNhaCungCap.Import(open.FileName);
                    nhaCungCapBUS bus = new nhaCungCapBUS();
                    bus.NhapExcelThongMinh(ds);
                    MessageBox.Show("Nhập Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi nhập Excel: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel file (*.xlsx)|*.xlsx";
            save.FileName = "DanhSachNhaCungCap.xlsx";

            if (save.ShowDialog() == DialogResult.OK)
            {
                nhaCungCapBUS bus = new nhaCungCapBUS();
                excelNhaCungCap.Export(bus.LayDanhSach(), save.FileName);
                //MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
