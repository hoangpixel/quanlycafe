using BUS;
using DAO;
using DTO;
using FONTS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class FormChonBan : Form
    {
        public int maBan { get; private set; }
        private banDAO banDAO = new banDAO();
        private hoaDonDAO hdDAO = new hoaDonDAO();
        private Timer timerDemGio;
        private Dictionary<int, DateTime> thoiGianCacBan = new Dictionary<int, DateTime>();
        public FormChonBan()
        {
            InitializeComponent();
            timerDemGio = new Timer();
            timerDemGio.Interval = 1000;
            timerDemGio.Tick += TimerDemGio_Tick;
            timerDemGio.Start();

            LoadKhuVuc();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void TimerDemGio_Tick(object sender, EventArgs e)
        {
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                if (ctrl is Button btn && btn.Tag != null)
                {
                    int maBan = (int)btn.Tag;

                    // Nếu bàn này nằm trong danh sách đang tính giờ
                    if (thoiGianCacBan.ContainsKey(maBan))
                    {
                        DateTime timeVao = thoiGianCacBan[maBan];
                        TimeSpan thoiGianDaNgoi = DateTime.Now - timeVao;

                        // Định dạng hiển thị: 01:30:45
                        string timeString = string.Format("{0:D2}:{1:D2}:{2:D2}",
                            (int)thoiGianDaNgoi.TotalHours,
                            thoiGianDaNgoi.Minutes,
                            thoiGianDaNgoi.Seconds);

                        // Cập nhật text mà không làm mất tên bàn
                        // Format nút: "Bàn 1\n(Bận)\n00:15:30"
                        string tenBan = btn.Name; // Mình sẽ lưu tên gốc vào Name để dễ lấy lại
                        btn.Text = $"{tenBan}\n(Bận)\n{timeString}";
                    }
                }
            }
        }

        private void LoadKhuVuc()
        {
            // Gọi BUS hoặc DAO để lấy danh sách khu vực từ Database
            khuvucBUS kvBus = new khuvucBUS(); // Giả sử bạn có BUS, nếu chưa thì dùng DAO
            BindingList<khuVucDTO> listKV = kvBus.LayDanhSach(); // Hàm này trả về SELECT * FROM khuvuc

            cbbKhuVuc.DataSource = listKV;
            cbbKhuVuc.DisplayMember = "TenKhuVuc"; // Tên cột hiển thị
            cbbKhuVuc.ValueMember = "MaKhuVuc";    // Tên cột giá trị lấy dùng


            // Load bàn cho khu vực đầu tiên
            if (cbbKhuVuc.Items.Count > 0)
            {
                cbbKhuVuc.SelectedIndex = 0;
                LoadBan();
            }
        }

        private void LoadBan()
        {
            flowLayoutPanel1.Controls.Clear();
            thoiGianCacBan.Clear(); // Reset bộ đếm giờ

            if (cbbKhuVuc.SelectedValue == null) return;

            // Ép kiểu về int an toàn
            if (!int.TryParse(cbbKhuVuc.SelectedValue.ToString(), out int maKhuVuc)) return;

            BindingList<banDTO> danhSachBan = banDAO.LayDanhSachTheoKhuVuc(maKhuVuc);

            foreach (banDTO ban in danhSachBan)
            {
                int maSoBan = ban.MaBan;
                string tenBan = ban.TenBan;

                // Logic cũ của bạn: 0 là bận, 1 là trống
                bool dangCoKhach = (ban.DangSuDung == 0);

                Button btn = new Button
                {
                    Name = tenBan,
                    Text = tenBan,
                    Width = 120,
                    Height = 80,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Margin = new Padding(5),
                    Tag = maSoBan,
                    Cursor = Cursors.Hand
                };

                if (dangCoKhach)
                {
                    btn.BackColor = Color.IndianRed;
                    btn.ForeColor = Color.White;
                    btn.Enabled = false; // Bàn bận thì không cho chọn (hoặc tùy logic bạn)

                    // --- LẤY GIỜ VÀO TỪ CSDL ---
                    DateTime gioVao = hdDAO.LayThoiGianTaoCuaBan(maSoBan);

                    if (gioVao != DateTime.MinValue)
                    {
                        thoiGianCacBan.Add(maSoBan, gioVao); // Thêm vào danh sách để Timer chạy

                        // Hiển thị ngay lập tức (để đỡ đợi 1 giây sau mới hiện)
                        TimeSpan span = DateTime.Now - gioVao;
                        btn.Text = $"{tenBan}\n(Bận)\n{span.ToString(@"hh\:mm\:ss")}";
                    }
                    else
                    {
                        btn.Text = $"{tenBan}\n(Bận)\n--:--:--";
                    }
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timerDemGio.Stop();
            timerDemGio.Dispose();
            base.OnFormClosing(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(banVaKhuVuc form = new banVaKhuVuc())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSetkhu_Click(object sender, EventArgs e)
        {
            using (SetKhu formSetKhu = new SetKhu())
            {
                formSetKhu.ShowDialog();
                LoadKhuVuc();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SetBan form = new SetBan())
            {
                form.ShowDialog();
                LoadBan();
            }    
        }
    }
}
