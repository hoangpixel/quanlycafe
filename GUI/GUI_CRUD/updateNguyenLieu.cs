using BUS;
using DTO;
using FONTS;
using GUI.GUI_SELECT;
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
    public partial class updateNguyenLieu : Form
    {
        private nguyenLieuDTO ct;
        private int maDonVi = -1;
        private string tenDonVi = "";
        public updateNguyenLieu()
        {
            InitializeComponent();
        }
        public updateNguyenLieu(nguyenLieuDTO ct)
        {
            InitializeComponent();
            this.ct = ct;
            this.Shown += updateNguyenLieu_Shown;
        }

        private void updateNguyenLieu_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            if (ct != null)
            {
                txtTenNL.Text = ct.TenNguyenLieu;
                maDonVi = ct.MaDonViCoSo;
                txtTenDonVi.Text = ct.TenDonViCoSo;
                txtTenNL.Focus();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void updateNguyenLieu_Shown(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void btnSuaNL_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenNL.Text) || maDonVi == -1)
            {
                MessageBox.Show("Vui lòng nhập tên và chọn đơn vị nguyên liệu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ct.TenNguyenLieu = txtTenNL.Text.Trim();
                ct.MaDonViCoSo = maDonVi;
                ct.TrangThai = 1;

                nguyenLieuBUS bus = new nguyenLieuBUS();
                bool kq = bus.suaNguyenLieu(ct);

                if (kq)
                {
                    MessageBox.Show("Cập nhật nguyên liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi khi cập nhật nguyên liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDonVi_Click(object sender, EventArgs e)
        {
            using (selectDonVi form = new selectDonVi())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    maDonVi = form.maDonVi;
                    tenDonVi = form.tenDonVi;
                    txtTenDonVi.Text = form.tenDonVi;
                }
            }
        }
    }
}
