using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAO;
using DTO;

namespace GUI.GUI_CRUD
{
    public partial class FormChonBan : Form
    {
        public int maBan { get; private set; }
        private hoaDonDAO hdDAO = new hoaDonDAO();
        private banDAO banDAO = new banDAO();
        private Dictionary<string, int> MaKhuVucMap = new Dictionary<string, int>
    {
        { "KhuVuc1", 1 },
        { "KhuVuc2", 2 }
    };
        public FormChonBan()
        {
            InitializeComponent();
            cbbKhuVuc.Items.AddRange(MaKhuVucMap.Keys.ToArray());
            cbbKhuVuc.SelectedIndex = 0;
            cbbKhuVuc.SelectedIndexChanged += cbbKhuVuc_SelectedIndexChanged;
            LoadBan();
        }

        private void LoadBan()
        {
            flowLayoutPanel1.Controls.Clear();

            string tenKhuVuc = cbbKhuVuc.SelectedItem.ToString();
            if (!MaKhuVucMap.TryGetValue(tenKhuVuc, out int maKhuVuc))
            {
                MessageBox.Show("Không tìm thấy mã khu vực.");
                return;
            }

            BindingList<banDTO> danhSachBan = banDAO.LayDanhSachTheoKhuVuc(maKhuVuc);

            foreach (banDTO ban in danhSachBan)
            {
                int maSoBan = ban.MaBan;
                string tenBan = ban.TenBan;

                Button btn = new Button
                {
                    Text = tenBan,
                    Width = 80,
                    Height = 55,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Margin = new Padding(5),
                    Tag = maSoBan 
                };
                bool dangCoKhach = ban.DangSuDung == 0;

                if (dangCoKhach)
                {
                    btn.BackColor = Color.IndianRed;
                    btn.ForeColor = Color.White;
                    btn.Text = tenBan + "\n(Bận)";
                    btn.Enabled = false; 
                }
                else
                {
                    btn.BackColor = Color.LightGreen;
                    btn.ForeColor = Color.Black;
                    btn.Text = tenBan + "\n(Trống)";
                    btn.Enabled = true;

                    btn.Click += (s, e) =>
                    {
                        maBan = maSoBan;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    };
                }

                flowLayoutPanel1.Controls.Add(btn);
            }
        }
        public void CapNhatBanTrong(int maBan)
        {
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                if (ctrl is Button btn && btn.Tag != null && (int)btn.Tag == maBan)
                {
                    btn.BackColor = Color.FromArgb(144, 238, 144);
                    btn.Text = $"Bàn {maBan}\nTrống";
                    btn.Enabled = true;
                    break;
                }
            }
        }

        public void RefreshBan()
        {
            LoadBan();
        }

        private void cbbKhuVuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBan();
        }
    }
}
