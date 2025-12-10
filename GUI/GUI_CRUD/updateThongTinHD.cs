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
        private BindingList<khuVucDTO> dsKhuVuc;
        private hoaDonBUS bus = new hoaDonBUS();
        private int maBan = -1, maNV = -1, maKH = -1, maBanCu;

        public updateThongTinHD(hoaDonDTO hd)
        {
            InitializeComponent();
            dsKhuVuc = new khuvucBUS().LayDanhSach();
            this.hd = hd;
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
            loadComBoThanhToan();
            this.maBanCu = hd.MaBan;
            SetComboBoxPlaceholder(cboPhatSinh, "Tiền phát sinh");
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

        private void button1_Click(object sender, EventArgs e)
        {
            using(insertPhieuHuy form = new insertPhieuHuy(hd))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
        }

        private void btnBan_Click(object sender, EventArgs e)
        {
            using (var f = new FormChonBan())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    maBan = f.maBan;
                    banBUS busBan = new banBUS();
                    banDTO banChon = busBan.LayDanhSach().FirstOrDefault(x => x.MaBan == maBan);

                    if (banChon != null)
                    {
                        khuVucDTO khuVuc = dsKhuVuc.FirstOrDefault(x => x.MaKhuVuc == banChon.MaKhuVuc);
                        string tenKhuVuc = (khuVuc != null) ? khuVuc.TenKhuVuc : "Chưa xác định";
                        txtban.Text = $"{banChon.TenBan} - {tenKhuVuc}";
                    }
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

            decimal tienPhatSinh = 0;
            if (!string.IsNullOrWhiteSpace(txtTienPhatSinh.Text))
            {
                decimal tienNhapVao;
                if (!decimal.TryParse(txtTienPhatSinh.Text, out tienNhapVao))
                {
                    MessageBox.Show("Tiền phát sinh phải là số hợp lệ", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTienPhatSinh.Focus();
                    return;
                }
                if (cboPhatSinh.SelectedIndex != -1)
                {
                    string kieuPhatSinh = cboPhatSinh.SelectedItem.ToString();

                    if (kieuPhatSinh == "Cộng tiền")
                    {
                        tienPhatSinh = tienNhapVao;
                    }
                    else if (kieuPhatSinh == "Trừ tiền")
                    {
                        tienPhatSinh = -tienNhapVao;
                    }
                }
            }

            decimal tongTienCu = hd.TongTien;
            decimal tongTienMoi = tongTienCu + tienPhatSinh;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("XÁC NHẬN CẬP NHẬT HÓA ĐƠN?");
            sb.AppendLine("------------------------------------------------");
            sb.AppendLine($"• Nhân viên: {txtTenNhanVien.Text}");
            sb.AppendLine($"• Khách hàng: {txtKhacHang.Text}");
            sb.AppendLine($"• Bàn mới: {txtban.Text}");
            sb.AppendLine($"• Thanh toán: {cboPPThanhToan.Text}");
            sb.AppendLine("");

            if (tienPhatSinh != 0)
            {
                sb.AppendLine("*************************************");
                string dau = tienPhatSinh > 0 ? "+" : "";
                sb.AppendLine($"   PHÁT SINH: {dau}{tienPhatSinh:N0} VNĐ");
                sb.AppendLine("*************************************");
            }
            else
            {
                sb.AppendLine("• Phát sinh: Không có");
            }

            sb.AppendLine("");
            sb.AppendLine($"-> TỔNG TIỀN MỚI: {tongTienMoi:N0} VNĐ");
            sb.AppendLine("(Tiền cũ: " + tongTienCu.ToString("N0") + " VNĐ)");

            DialogResult result = MessageBox.Show(sb.ToString(),
                                                  "Xác nhận thay đổi",
                                                  MessageBoxButtons.OKCancel,
                                                  MessageBoxIcon.Question);

            if (result != DialogResult.OK)
            {
                return;
            }

            hoaDonDTO hdSua = new hoaDonDTO();
            hdSua.MaHD = hd.MaHD;
            hdSua.MaNhanVien = maNV;
            hdSua.MaKhachHang = maKH;
            hdSua.MaBan = maBan;
            hdSua.MaTT = Convert.ToInt32(cboPPThanhToan.SelectedValue);
            hdSua.TongTien = hd.TongTien + tienPhatSinh;

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

        private void SetComboBoxPlaceholder(ComboBox cbo, string placeholder)
        {

            cbo.ForeColor = System.Drawing.Color.Gray;
            cbo.Text = placeholder;

            cbo.GotFocus += (s, e) =>
            {
                if (cbo.Text == placeholder)
                {
                    cbo.Text = "";
                    cbo.ForeColor = System.Drawing.Color.Black;
                }
            };
            cbo.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(cbo.Text))
                {
                    cbo.Text = placeholder;
                    cbo.ForeColor = System.Drawing.Color.Gray;
                }
            };
        }
    }
}
