using BUS;
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

namespace GUI.GUI_CRUD
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
                bool kq = bus.xoaCongThuc(maSP, maNL);

                if (kq)
                {
                    MessageBox.Show("Xóa công thức thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Xóa công thức thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa công thức: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Abort;
            }
        }

        private void deleteCongThuc_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
