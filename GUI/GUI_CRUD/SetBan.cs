using BUS;
using DAO;
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
    public partial class SetBan : Form
    {
        private banBUS busBan = new banBUS();
        private khuvucBUS busKV = new khuvucBUS();
        private banDTO banDangChon =null;
        private BindingList<banDTO> dsBan;
        public SetBan()
        {
            InitializeComponent();
        }

        private void SetBan_Load(object sender, EventArgs e)
        {
            LoadKhuVuc();
            // Sự kiện thay đổi khu vực → load lại bàn
            cbbKhuVuc.SelectedIndexChanged += cbbKhuVuc_SelectedIndexChanged;

            // Ban đầu ẩn nút Sửa và Xóa
            btnSuaBan.Visible = false;
            btnXoaBan.Visible = false;
        }
        private void LoadKhuVuc()
        {
            var listKV = busKV.LayDanhSach(); 

            cbbKhuVuc.DataSource = listKV;
            cbbKhuVuc.DisplayMember = "TenKhuVuc"; 
            cbbKhuVuc.ValueMember = "MaKhuVuc"; 


            // Load bàn cho khu vực đầu tiên
            if (cbbKhuVuc.Items.Count > 0)
            {
                cbbKhuVuc.SelectedIndex = 0;
            }
        }
        private void ReloadDuLieu(int maBan)
        {
            var state = this.WindowState;
            if (state == FormWindowState.Normal)
            {
                var size = this.Size;
                var loc = this.Location;

                LoadDanhSachBan(maBan);

                this.Size = size;
                this.Location = loc;
            }
            else
            {
                LoadDanhSachBan(maBan);
            }
            this.WindowState = state;
        }
        private void cbbKhuVuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbKhuVuc.SelectedValue == null) return;

            if (int.TryParse(cbbKhuVuc.SelectedValue.ToString(), out int maKV))
            {
                ReloadDuLieu(maKV);
            }
        }
        private void LoadDanhSachBan(int maKhuVuc)
        {
            dsBan = busBan.LayDanhSachTheoKhuVuc(maKhuVuc); // VIẾT HÀM NÀY TRONG BUS

            dgvBan.DataSource = null;
            dgvBan.DataSource = dsBan;

            // Cấu hình cột (chỉ làm 1 lần cũng được, nhưng làm lại cho chắc)
            dgvBan.Columns.Clear();
            dgvBan.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaBan",
                HeaderText = "Mã Bàn",
                Width = 100
            });
            dgvBan.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenBan",
                HeaderText = "Tên Bàn",
                Width = 150
            });

            // Ẩn cột MaKhuVuc nếu có
            if (dgvBan.Columns.Contains("MaKhuVuc"))
                dgvBan.Columns["MaKhuVuc"].Visible = false;

            dgvBan.ClearSelection();

            // Ẩn nút sửa xóa khi đổi khu vực
            btnSuaBan.Visible = false;
            btnXoaBan.Visible = false;
            txtBan.Clear();
        }

        private void dgvBan_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBan.SelectedRows.Count > 0)
            {
                banDangChon = (banDTO)dgvBan.SelectedRows[0].DataBoundItem;
                txtBan.Text = banDangChon.TenBan;
                btnSuaBan.Visible = true;
                btnXoaBan.Visible = true;
            }
            else
            {
                banDangChon = null;
                txtBan.Clear();
                btnSuaBan.Visible = false;
                btnXoaBan.Visible = false;
            }
        }

        private void btnThemBan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBan.Text))
            {
                MessageBox.Show("Vui lòng nhập tên bàn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbbKhuVuc.SelectedValue == null) return;

            int maKV = Convert.ToInt32(cbbKhuVuc.SelectedValue);

            var banMoi = new banDTO
            {
                TenBan = txtBan.Text.Trim(),
                MaKhuVuc = maKV
            };

            if (busBan.ThemBan(banMoi))
            {
                MessageBox.Show("Thêm bàn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReloadDuLieu(maKV); // reload lại
                txtBan.Clear();
            }
            else
            {
                MessageBox.Show("Thêm thất bại! Tên bàn có thể đã tồn tại trong khu vực này.", "Lỗi");
            }
        }

        private void btnSuaBan_Click(object sender, EventArgs e)
        {
            if (banDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần sửa!", "Thông báo");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtBan.Text))
            {
                MessageBox.Show("Tên bàn không được để trống!", "Cảnh báo");
                return;
            }

            // Cập nhật lại tên mới
            banDangChon.TenBan = txtBan.Text.Trim();

            if (busBan.SuaBan(banDangChon))
            {
                MessageBox.Show("Sửa bàn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                int maKV = Convert.ToInt32(cbbKhuVuc.SelectedValue);
                ReloadDuLieu(maKV);
                txtBan.Focus();
            }
            else
            {
                MessageBox.Show("Sửa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaBan_Click(object sender, EventArgs e)
        {
            if (banDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần xóa!", "Thông báo");
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa bàn \"{banDangChon.TenBan}\"?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                if (busBan.XoaBan(banDangChon.MaBan))
                {
                    MessageBox.Show("Xóa bàn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    int maKV = Convert.ToInt32(cbbKhuVuc.SelectedValue);
                    ReloadDuLieu(maKV);
                    txtBan.Clear();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại! Có thể bàn đang được sử dụng ở bảng khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
