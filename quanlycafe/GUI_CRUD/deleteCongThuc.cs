using quanlycafe.BUS;
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
    public partial class deleteCongThuc : Form
    {
        private int maSP = -1, maNL = -1;
        public deleteCongThuc(int maSP,int maNL)
        {
            InitializeComponent();
            this.maSP = maSP;
            this.maNL = maNL;
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                congThucBUS bus = new congThucBUS();
                bus.xoaCongThuc(maSP,maNL);
                MessageBox.Show("Xóa công thức thành công");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa công thức thất bại: " + ex);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
