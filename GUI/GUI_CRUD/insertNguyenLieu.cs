using BUS;
using DTO;
using GUI.FONTS;
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
    public partial class insertNguyenLieu : Form
    {
        private List<donViDTO> dsDonVi;
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

            donViBUS busDonVi = new donViBUS();
            dsDonVi = busDonVi.layDanhSachDonVi();

            cbDonVi.DataSource = dsDonVi;
            cbDonVi.DisplayMember = "TenDonVi";   
            cbDonVi.ValueMember = "MaDonVi";     
            cbDonVi.SelectedIndex = -1;
        }

        private void btnNhapNL_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenNL.Text) || cbDonVi.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập tên và chọn đơn vị nguyên liệu!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string tenNL = txtTenNL.Text.Trim();
                int maDonVi = Convert.ToInt32(cbDonVi.SelectedValue);

                nguyenLieuDTO nl = new nguyenLieuDTO
                {
                    TenNguyenLieu = tenNL,
                    MaDonViCoSo = maDonVi,
                    TonKho = 0,
                    TrangThai = 1
                };

                nguyenLieuBUS bus = new nguyenLieuBUS();
                bool kq = bus.themNguyenLieu(nl);

                if (kq)
                {
                    MessageBox.Show("Thêm nguyên liệu mới thành công!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
