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
    public partial class dialogSuaHoacThemHD : Form
    {
        public int luaChon = 0;
        public dialogSuaHoacThemHD()
        {
            InitializeComponent();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThayDoiHoaDon_Click(object sender, EventArgs e)
        {
            luaChon = 1;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnThemMon_Click(object sender, EventArgs e)
        {
            luaChon = 2;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
