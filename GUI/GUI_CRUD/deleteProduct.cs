using BUS;
using DTO;
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
    public partial class deleteProduct : Form
    {
        private int maSP;    
        public deleteProduct(int maSP)
        {
            InitializeComponent();
            this.maSP = maSP;
        }

        private void deleteProduct_Load(object sender, EventArgs e)
        {

        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    sanPhamBUS bus = new sanPhamBUS();
            //    bus.Xoa(maSP);
            //    MessageBox.Show("Xóa sản phẩm thành công");
            //    this.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Xóa sản phẩm thất bại: " + ex);
            //}
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnXacNhan_Click_1(object sender, EventArgs e)
        {
            try
            {
                sanPhamBUS bus = new sanPhamBUS();
                bus.Xoa(maSP);
                MessageBox.Show("Xóa sản phẩm thành công");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xóa sản phẩm thất bại: " + ex);
            }
        }

        private void btnHuy_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
