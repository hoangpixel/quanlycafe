using BUS;
using GUI.FONTS;
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
    public partial class deleteNguyenLieu : Form
    {
        private int maNL;
        public deleteNguyenLieu(int maNL)
        {
            InitializeComponent();
            this.maNL = maNL;
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                nguyenLieuBUS bus = new nguyenLieuBUS();
                bus.xoaNguyenLieu(maNL);
                MessageBox.Show("Xóa nguyên liệu thành công");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa nguyên liệu thất bại: " + ex);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteNguyenLieu_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }
    }
}
