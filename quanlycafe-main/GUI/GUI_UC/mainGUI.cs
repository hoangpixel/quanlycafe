using ReaLTaiizor.Forms;
using System;
using System.Windows.Forms;

namespace GUI.GUI_UC
{
    public partial class mainGUI : Form
    {
        public mainGUI()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                this.Size = new System.Drawing.Size(1480, 800);
                //this.WindowState = FormWindowState.Maximized;
                this.StartPosition = FormStartPosition.CenterScreen;
                this.FormBorderStyle = FormBorderStyle.None;

                LoadMainContent();
            }
        }
        private void mainGUI_Load(object sender, EventArgs e)
        {
            // Để trống cũng được
        }


        private void LoadMainContent()
        {
            var mainContent = new mainContentGUI
            {
                Dock = DockStyle.Fill
            };
            this.Controls.Add(mainContent);
        }
    }
}
