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
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class insertNguyenLieu : Form
    {
        private int maDonVi = -1;
        private string tenDonVi = "";
        public nguyenLieuDTO ct;
        public insertNguyenLieu()
        {
            InitializeComponent();
            this.Shown += insertNguyenLieu_Shown;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbDonVi_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void insertNguyenLieu_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void btnNhapNL_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenNL.Text) || maDonVi == -1)
            {
                MessageBox.Show("Vui lòng nhập tên và chọn đơn vị nguyên liệu!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string tenNL = txtTenNL.Text.Trim();
                nguyenLieuBUS bus = new nguyenLieuBUS();
                int maNL = bus.layMa();

                nguyenLieuDTO nl = new nguyenLieuDTO();
                nl.MaNguyenLieu = maNL;
                nl.TenNguyenLieu = tenNL;
                nl.MaDonViCoSo = maDonVi;
                nl.TonKho = 0;
                nl.TrangThai = 1;

                ct = nl;
                if (nl != null)
                {
                    MessageBox.Show("Thêm nguyên liệu mới thành công",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi khi thêm nguyên liệu!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm nguyên liệu: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void insertNguyenLieu_Shown(object sender, EventArgs e)
        {
            txtTenNL.Focus();

        }

        private void btnDonVi_Click(object sender, EventArgs e)
        {
            using(selectDonVi form = new selectDonVi())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if(form.ShowDialog() == DialogResult.OK)
                {
                    maDonVi = form.maDonVi;
                    tenDonVi = form.tenDonVi;
                    txtTenDonVi.Text = form.tenDonVi;
                }
            }
            donViBUS bus = new donViBUS();
            bus.LayDanhSach();
        }
    }
}
