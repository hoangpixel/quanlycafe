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

namespace GUI.GUI_CRUD
{
    public partial class FormChonBan : Form
    {
        public int maBan { get; private set; }
        private hoaDonDAO hdDAO = new hoaDonDAO();
        private Dictionary<int, bool> TrangThaiBan = new Dictionary<int, bool>();

        public FormChonBan()
        {
            InitializeComponent();
            cbbKhuVuc.Items.AddRange(new[] { "KhuVuc1", "KhuVuc2" });
            cbbKhuVuc.SelectedIndex = 0;
            cbbKhuVuc.SelectedIndexChanged += cbbKhuVuc_SelectedIndexChanged;
            LoadBan();
        }

        private void LoadBan()
        {
            flowLayoutPanel1.Controls.Clear();
            string kv = cbbKhuVuc.SelectedItem.ToString();
            int offset = (kv == "KhuVuc1") ? 0 : 20;

            for (int i = 1; i <= 20; i++)
            {
                int maSoBan = offset + i;
                string tenBan = $"Bàn {i}";

                Button btn = new Button
                {
                    Text = tenBan,
                    Width = 80,
                    Height = 50,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Margin = new Padding(5),
                    Tag = maSoBan // lưu mã bàn vào Tag để dùng sau
                };

                // Kiểm tra xem bàn này có đang có hóa đơn chưa thanh toán không
                bool dangCoKhach = hdDAO.BanDangCoHoaDonChuaThanhToan(maSoBan);

                if (dangCoKhach)
                {
                    // Bàn đang có khách → đổi màu đỏ + disable + thêm chữ "Bận"
                    btn.BackColor = Color.IndianRed;
                    btn.ForeColor = Color.White;
                    btn.Text = tenBan + "\n(Bận)";
                    btn.Enabled = false;
                }
                else
                {
                    // Bàn trống → màu xanh + cho phép chọn
                    btn.BackColor = Color.LightGreen;
                    btn.ForeColor = Color.Black;
                    btn.Text = tenBan + "\n(Trống)";

                    // Gắn sự kiện click
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
        // Gọi từ frmHoaDon sau khi xóa thành công
        public void CapNhatBanTrong(int maBan)
        {
            // Tìm nút bàn tương ứng và đổi màu về "trống" (xanh lá hoặc trắng)
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                if (ctrl is Button btn && btn.Tag != null && (int)btn.Tag == maBan)
                {
                    btn.BackColor = Color.FromArgb(144, 238, 144); // xanh lá nhạt = trống
                    btn.Text = $"Bàn {maBan}\nTrống";
                    btn.Enabled = true;
                    break;
                }
            }
        }

        private void cbbKhuVuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBan();
        }
        public void RefreshBan()
        {
            LoadBan();
        }
    }
}
