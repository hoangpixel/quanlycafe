using BUS;
using DTO;
using FONTS;
using System;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class XOAnv : Form
    {
        public nhanVienDTO nv;
        private nhanVienBUS busNhanVien = new nhanVienBUS();

        public XOAnv(nhanVienDTO nv)
        {
            InitializeComponent();
            this.nv = nv;
        }

        private void XOAnv_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            // Hiển thị thông tin trong label
            label1.Text = $"Bạn có chắc chắn muốn xóa nhân viên\n\n'{nv.HoTen}'\n\nkhông?";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            btnXacNhan.Focus();
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult confirm = MessageBox.Show(
                    $"⚠️ CẢNH BÁO: Hành động này không thể hoàn tác!\n\n" +
                    $"Bạn có chắc chắn muốn xóa nhân viên '{nv.HoTen}' không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirm == DialogResult.No)
                    return;

                if (busNhanVien.XoaNhanVien(nv.MaNhanVien))
                {
                    MessageBox.Show("✅ Xóa nhân viên thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("❌ Xóa nhân viên thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("foreign key") ||
                    ex.Message.Contains("FOREIGN KEY") ||
                    ex.Message.Contains("liên quan") ||
                    ex.Message.Contains("REFERENCE"))
                {
                    MessageBox.Show(
                        "❌ Không thể xóa nhân viên này!\n\n" +
                        "Nhân viên đang có dữ liệu liên quan (hóa đơn, phiếu nhập...).\n" +
                        "Vui lòng xóa các dữ liệu liên quan trước! ",
                        "Lỗi ràng buộc dữ liệu",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
                else
                {
                    MessageBox.Show("Lỗi xóa nhân viên: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}