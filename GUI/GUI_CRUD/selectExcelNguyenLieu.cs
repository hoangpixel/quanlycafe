using BUS;
using GUI.EXCEL;
using FONTS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;

namespace GUI.GUI_CRUD
{
    public partial class selectExcelNguyenLieu : Form
    {
        public selectExcelNguyenLieu()
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
                    BindingList<nguyenLieuDTO> ds = excelNguyenLieu.Import(open.FileName);
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
                    BindingList<nguyenLieuDTO> ds = bus.LayDanhSach();

                    excelNguyenLieu.Export(ds, save.FileName);

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

        private void selectExcelNguyenLieu_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }
    }
}
