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
    public partial class detailNhaCungCap : Form
    {
        private nhaCungCapDTO ct;
        public detailNhaCungCap(nhaCungCapDTO ct)
        {
            InitializeComponent();
            this.ct = ct;
        }

        private void detailNhaCungCap_Load(object sender, EventArgs e)
        {
            txtTen.Text = ct.TenNCC;
            txtSDT.Text = ct.SoDienThoai;
            txtEmail.Text = ct.Email;
            txtDiaChi.Text = ct.DiaChi;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
