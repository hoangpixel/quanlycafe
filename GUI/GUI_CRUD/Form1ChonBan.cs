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
    public partial class FormChonBan : Form
    {
        public int maBan { get; private set; }
        private Dictionary<int, bool> TrangThaiBan = new Dictionary<int, bool>();

        public FormChonBan()
        {
            InitializeComponent();
            cbbKhuVuc.Items.AddRange(new[] { "KV1", "KV2" });
            cbbKhuVuc.SelectedIndex = 0;
            cbbKhuVuc.SelectedIndexChanged += cbbKhuVuc_SelectedIndexChanged;
            LoadBan();
        }

        private void LoadBan()
        {
            flowLayoutPanel1.Controls.Clear();
            string kv = cbbKhuVuc.SelectedItem.ToString();
            int offset = (kv == "KV1") ? 0 : 20;
            for (int i = 1; i <= 20; i++)
            {
                int maSoBan = offset + i;
                string tenBan = $"{kv}-Bàn{i}";
                Button btn = new Button();
                btn.Text = tenBan;
                btn.Width = 85;
                btn.Height = 60;

                // Kiểm tra trạng thái bàn (đã chọn thì disable)
                if (TrangThaiBan.ContainsKey(maSoBan) && TrangThaiBan[maSoBan])
                    btn.Enabled = false;

                // Tạo bản sao cục bộ để tránh capture lỗi
                int banHienTai = maSoBan;
                btn.Click += (s, e) =>
                {
                    maBan = banHienTai;
                    TrangThaiBan[banHienTai] = true; // đánh dấu đã chọn
                    DialogResult = DialogResult.OK;
                    Close();
                };

                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        private void cbbKhuVuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBan();
        }
    }


}
