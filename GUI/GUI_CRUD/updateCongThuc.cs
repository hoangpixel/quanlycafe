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
    public partial class updateCongThuc : Form
    {
        private congThucDTO ct;
        private int maDV = -1;
        public congThucDTO ctSUA;

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

            maDV = ct.MaDonViCoSo;
            txtSoLuong.Value = (decimal)ct.SoLuongCoSo;

            BindingList<sanPhamDTO> dsSP = new sanPhamBUS().LayDanhSach();
            BindingList<nguyenLieuDTO> dsNL = new nguyenLieuBUS().LayDanhSach();
            BindingList<donViDTO> dsDV = new donViBUS().LayDanhSach();

            sanPhamDTO sp = dsSP.FirstOrDefault(x => x.MaSP == ct.MaSanPham);
            txtTenSanPham.Text = sp?.TenSP ?? "Không xác định";

            nguyenLieuDTO nl = dsNL.FirstOrDefault(x => x.MaNguyenLieu == ct.MaNguyenLieu);
            txtTenNguyenLieu.Text = nl?.TenNguyenLieu ?? "Không xác định";

            donViDTO dv = dsDV.FirstOrDefault(x => x.MaDonVi == ct.MaDonViCoSo);
            txtTenDV.Text = dv?.TenDonVi ?? "Không xác định";

            txtSoLuong.Select();
        }

        private void btnXacNhanSua_Click(object sender, EventArgs e)
        {
            if(maDV == -1)
            {
                return;
            }
            if(txtSoLuong.Value == 0)
            {
                return;
            }

            DialogResult confirm = MessageBox.Show(
            "Bạn có chắc chắn muốn cập nhật công thức này không?",
            "Xác nhận cập nhật",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            float soLuongMoi = (float)txtSoLuong.Value;

            if (confirm == DialogResult.Yes)
            {
                ct.MaSanPham = ct.MaSanPham;
                ct.MaNguyenLieu = ct.MaNguyenLieu;
                ct.MaDonViCoSo = maDV;
                ct.SoLuongCoSo = soLuongMoi;

                if(ct != null)
                {
                    MessageBox.Show("Cập nhật nguyên liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ctSUA = ct;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }else
                {
                    MessageBox.Show("Lỗi khi cập nhật nguyên liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }

        private void btnChonDV_Click(object sender, EventArgs e)
        {
            using(selectDonVi form = new selectDonVi())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if(form.ShowDialog() == DialogResult.OK)
                {
                    maDV = form.maDonVi;
                    txtTenDV.Text = form.tenDonVi;
                }
            }
        }
    }
}
