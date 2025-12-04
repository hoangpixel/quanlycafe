using BUS;
using DTO;
using System;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class xoaTk : Form
    {

        public xoaTk()
        {
            InitializeComponent();
        }

        private void btnXacNhan_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            MessageBox.Show("Xóa tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void btnHuy_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}