using BUS;
using DTO;
using System;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class xoaTk : Form
    {
        private taikhoanBUS bustaikhoan = new taikhoanBUS();
        private taikhoanDTO taikhoanCanXoa;

        public xoaTk(taikhoanDTO tk)
        {
            InitializeComponent();
            this.taikhoanCanXoa = tk;

            // Gán sự kiện
            btnXacNhan.Click += btnXacNhan_Click;
            btnHuy.Click += btnHuy_Click;

            // Cập nhật label với thông tin tài khoản
            label1.Text = $"Bạn có chắc muốn xóa tài khoản\n\"{tk.TENDANGNHAP}\" ({tk.TENNHANVIEN})?";
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                // Xác nhận lần cuối
                DialogResult result = MessageBox.Show(
                    $"⚠️ CẢNH BÁO: Bạn sắp xóa tài khoản!\n\n" +
                    $"Tên đăng nhập: {taikhoanCanXoa.TENDANGNHAP}\n" +
                    $"Nhân viên: {taikhoanCanXoa.TENNHANVIEN}\n\n" +
                    $"Hành động này KHÔNG THỂ HOÀN TÁC!\n\n" +
                    $"Bạn có chắc chắn muốn tiếp tục?",
                    "Xác nhận xóa tài khoản",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    // Gọi BUS để xóa
                    if (bustaikhoan.Xoa(taikhoanCanXoa.MAtaikHOAN))
                    {
                        MessageBox.Show(
                            "✅ Đã xóa tài khoản thành công!",
                            "Thành công",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(
                            "❌ Xóa tài khoản thất bại!",
                            "Lỗi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi: {ex.Message}\n\n{ex.StackTrace}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}