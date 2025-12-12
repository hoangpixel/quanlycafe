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

        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {

        }

        private void btnNhapExcel_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Excel Files|*.xlsx;*.xls";
            open.Title = "Chọn file Excel Phân quyền";

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    BindingList<phanquyenDTO> dsExcel = GUI.EXCEL.excelPhanQuyen.Import(open.FileName);
                    phanquyenBUS bus = new phanquyenBUS();
                    string ketQua = bus.NhapExcelThongMinh(dsExcel);
                    MessageBox.Show(ketQua, "Kết quả nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi nhập Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXuatExcel_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel Files|*.xlsx";
            save.FileName = "DanhSachPhanQuyen.xlsx";
            save.Title = "Lưu file Excel Phân quyền";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    phanquyenBUS bus = new phanquyenBUS();
                    BindingList<phanquyenDTO> dsDB = bus.LayDanhSach();
                    GUI.EXCEL.excelPhanQuyen.Export(dsDB, save.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
