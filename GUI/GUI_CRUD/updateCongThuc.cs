using BUS;
using DTO;
using FONTS;
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
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
            try
            {
                donViBUS dvBUS = new donViBUS();
                BindingList<donViDTO> dsDonVi = dvBUS.LayDanhSach();

                cboDonVi.DisplayMember = "TenDonVi";
                cboDonVi.ValueMember = "MaDonVi";
                cboDonVi.DataSource = dsDonVi;

                if (ct != null)
                {
                    txtTenSanPham.Text = ct.TenSanPham;
                    txtTenNguyenLieu.Text = ct.TenNguyenLieu;
                    txtSoLuong.Value = (decimal)ct.SoLuongCoSo;

                    if (ct.MaDonViCoSo > 0)
                    {
                        cboDonVi.SelectedValue = ct.MaDonViCoSo;
                    }
                    else
                    {
                        cboDonVi.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load đơn vị: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXacNhanSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboDonVi.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn đơn vị!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn cập nhật công thức này không?",
                    "Xác nhận cập nhật",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.No) return;

                int maDonViMoi = Convert.ToInt32(cboDonVi.SelectedValue);
                string tenDonViMoi = cboDonVi.Text;
                float soLuongMoi = (float)txtSoLuong.Value;

                congThucDTO moi = new congThucDTO
                {
                    MaSanPham = ct.MaSanPham,
                    MaNguyenLieu = ct.MaNguyenLieu,
                    SoLuongCoSo = soLuongMoi,
                    MaDonViCoSo = maDonViMoi,
                    TenDonViCoSo = tenDonViMoi,
                    TrangThai = 1
                };

                congThucBUS bus = new congThucBUS();
                bool kq = bus.suaCongThuc(moi);

                if (kq)
                    MessageBox.Show("Cập nhật công thức thành công!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Cập nhật thất bại!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
