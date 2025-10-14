using quanlycafe.BUS;
using quanlycafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlycafe.GUI_CRUD
{
    public partial class updateCongThuc : Form
    {
        private congThucDTO ct;
        public updateCongThuc(congThucDTO ct)
        {
            InitializeComponent();
            this.ct = ct;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void updateCongThuc_Load(object sender, EventArgs e)
        {
            if(ct!=null)
            {
                txtTenNguyenLieu.Text = ct.TenNguyenLieu;
                txtTenSanPham.Text = ct.TenSanPham;
                txtSoLuong.Value = (decimal)ct.SoLuongCoSo;
            }
        }

        private void btnXacNhanSua_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn cập nhật số lượng cho công thức này không?",
                    "Xác nhận cập nhật",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                congThucBUS bus = new congThucBUS();
                float soLuongMoi = (float)txtSoLuong.Value;

                congThucDTO moi = new congThucDTO
                {
                    MaSanPham = ct.MaSanPham,
                    MaNguyenLieu = ct.MaNguyenLieu,
                    SoLuongCoSo = soLuongMoi,
                    TrangThai = 1 
                };

                bool kq = bus.suaCongThuc(moi);

                if (kq)
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
