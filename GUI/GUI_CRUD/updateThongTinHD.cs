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
using System.Windows.Media;

namespace GUI.GUI_CRUD
{
    public partial class updateThongTinHD : Form
    {
        public hoaDonDTO hd;
        private hoaDonBUS bus = new hoaDonBUS();
        private int maBan = -1, maNV = -1, maKH = -1, maBanCu;

        public updateThongTinHD(hoaDonDTO hd)
        {
            InitializeComponent();
            this.hd = hd;
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
            loadComBoThanhToan();
            this.maBanCu = hd.MaBan;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadComBoThanhToan()
        {
            BindingList<ppThanhToanDTO> dsThanhThoan = new ppThanhToanBUS().LayDanhSach();
            cboPPThanhToan.DataSource = dsThanhThoan;
            cboPPThanhToan.DisplayMember = "HinhThuc";
            cboPPThanhToan.ValueMember = "MaTT";
        }

        private void btnChonKH_Click(object sender, EventArgs e)
        {
            using (var f = new FormchonKH())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    txtKhacHang.Text = f.TenKHChon;
                    maKH = f.MaKHChon;
                }
            }
        }

        private void btnBan_Click(object sender, EventArgs e)
        {
            using (var f = new FormChonBan())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    txtban.Text = f.maBan.ToString();
                    maBan = f.maBan;
                }
            }
        }

        private void updateThongTinHD_Load(object sender, EventArgs e)
        {
            var dsKh = new khachHangBUS().LayDanhSach();
            var kh = dsKh.FirstOrDefault(x => x.MaKhachHang == hd.MaKhachHang);

            txtKhacHang.Text = kh?.TenKhachHang ?? "Khách lẻ";
            maKH = hd.MaKhachHang ?? 0;

            BindingList<banDTO> dsBan = new banBUS().LayDanhSach();
            BindingList<khuVucDTO> dsKhuVuc = new khuvucBUS().LayDanhSach();
            banDTO ban = dsBan.FirstOrDefault(x => x.MaBan == hd.MaBan);
            khuVucDTO khuVuc = dsKhuVuc.FirstOrDefault(x => x.MaKhuVuc == ban.MaKhuVuc);

            string tenBanKhuVuc = ban?.TenBan + " - " + khuVuc?.TenKhuVuc;

            txtban.Text = tenBanKhuVuc;

            maBan = hd.MaBan;
            maKH = hd.MaKhachHang ?? 0;

            cboPPThanhToan.SelectedValue = hd.MaTT;


            if (DTO.Session.NhanVienHienTai != null)
            {
                txtTenNhanVien.Text = DTO.Session.NhanVienHienTai.HoTen;
                maNV = DTO.Session.NhanVienHienTai.MaNhanVien;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(maNV == -1)
            {
                MessageBox.Show("Không tìm thấy nhân viên", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (maKH == -1)
            {
                MessageBox.Show("Không được để trống khách hàng", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (maBan == -1)
            {
                MessageBox.Show("Không được để trống bàn", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(cboPPThanhToan.SelectedIndex == -1)
            {
                MessageBox.Show("Không được để trống phương thức thanh toán", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboPPThanhToan.Focus();
                return;
            }

            hoaDonDTO hdSua = new hoaDonDTO();
            hdSua.MaHD = hd.MaHD;
            hdSua.MaNhanVien = maNV;
            hdSua.MaKhachHang = maKH;
            hdSua.MaBan = maBan;
            hdSua.MaTT = Convert.ToInt32(cboPPThanhToan.SelectedValue);

            if (maBan != maBanCu)
            {
                banBUS busBan = new banBUS();
                busBan.DoiTrangThai(maBan);
                bool conNguoiKhac = bus.KiemTraBanCoHoaDonMo(maBanCu, hd.MaHD);

                if (conNguoiKhac == false)
                {
                    bus.doiTrangThaiBanSauKhiXoaHD(maBanCu);

                    foreach (Form f in Application.OpenForms)
                    {
                        if (f is FormChonBan chonBan)
                            chonBan.CapNhatBanTrong(maBanCu);
                    }
                }
            }

            hd = hdSua;
            this.DialogResult = DialogResult.OK;
            MessageBox.Show("Sửa thông tin hóa đơn thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
