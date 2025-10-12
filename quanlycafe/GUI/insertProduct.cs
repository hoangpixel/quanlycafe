using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlycafe.GUI
{
    public partial class insertProduct : Form
    {
        private string imagePath = "";
        public insertProduct()
        {
            InitializeComponent();
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Ảnh sản phẩm (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            if (open.ShowDialog() == DialogResult.OK)
            {
                imagePath = open.FileName;
                picHinh.Image = Image.FromFile(imagePath);
                picHinh.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void btnXoaAnh_Click(object sender, EventArgs e)
        {
            picHinh.Image = null;
            imagePath = "";
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
