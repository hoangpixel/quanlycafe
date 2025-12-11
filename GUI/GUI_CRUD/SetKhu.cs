using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DTO;
namespace GUI.GUI_CRUD
{
    public partial class SetKhu : Form
    {
        private BindingList<khuVucDTO> dskv;
        khuvucBUS kvBUS = new khuvucBUS();
        private khuVucDTO kvDangChon = null;
        public SetKhu()
        {
            InitializeComponent();
        }

        private void SetKhu_Load(object sender, EventArgs e)
        {
            ReloadDuLieu();
        }
        public void LoadDanhSachKV()
        {
            dskv = kvBUS.LayDanhSach();
            dgvKV.AutoGenerateColumns = false;
            dgvKV.DataSource = null;
            dgvKV.DataSource = dskv;
            dgvKV.Columns.Clear();

            dgvKV.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MAKHUVUC", HeaderText = "Mã Khu Vực", Width=120 });
            dgvKV.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TENKHUVUC", HeaderText = "Tên Khu Vực", Width = 120 });

            dgvKV.ReadOnly = true;
            dgvKV.ClearSelection();
        }
        private void ReloadDuLieu()
        {
                LoadDanhSachKV();
                dgvKV.ClearSelection();
                
        }
        private void btnThemKV_Click(object sender, EventArgs e)
        {
            var ds = new khuVucDTO
            {
                TenKhuVuc = txtKV.Text
            };
            bool maKV = kvBUS.ThemKV(ds);
            if(maKV)
            {
                MessageBox.Show("Thêm khu vực thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReloadDuLieu();
                txtKV.Clear();
            }
            else
            {
                MessageBox.Show("Thêm khu vực thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvKV_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvKV.SelectedRows.Count > 0)
            {
                kvDangChon = (khuVucDTO)dgvKV.SelectedRows[0].DataBoundItem;
                txtKV.Text = kvDangChon.TenKhuVuc;

                // Hiện nút Sửa và Xóa
                btnSuaKV.Visible = true;
                btnXoaKV.Visible = true;
            }
            else
            {
                kvDangChon = null;
                txtKV.Clear();

                btnSuaKV.Visible = false;
                btnXoaKV.Visible = false;
            }
        }

        private void btnSuaKV_Click(object sender, EventArgs e)
        {
            if (kvDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn khu vực cần sửa!", "Thông báo");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtKV.Text))
            {
                MessageBox.Show("Tên khu vực không được để trống!", "Cảnh báo");
                return;
            }

            // Cập nhật lại tên mới
            kvDangChon.TenKhuVuc = txtKV.Text.Trim();

            if (kvBUS.SuaKV(kvDangChon))
            {
                MessageBox.Show("Sửa khu vực thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReloadDuLieu();
                txtKV.Focus();
            }
            else
            {
                MessageBox.Show("Sửa thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaKV_Click(object sender, EventArgs e)
        {
            if (kvDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn khu vực cần xóa!", "Thông báo");
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa khu vực \"{kvDangChon.TenKhuVuc}\"?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                if (kvBUS.XoaKV(kvDangChon.MaKhuVuc))
                {
                    MessageBox.Show("Xóa khu vực thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ReloadDuLieu();
                    txtKV.Clear();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại! Có thể khu vực đang được sử dụng ở bảng khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
