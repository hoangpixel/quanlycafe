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
    public partial class detailTaiKhoan : Form
    {
        private taikhoanDTO tk;
        private BindingList<nhanVienDTO> dsNV = new nhanVienBUS().LayDanhSach();
        private BindingList<vaitroDTO> dsVT = new vaitroBUS().LayDanhSach();
        public detailTaiKhoan(taikhoanDTO tk)
        {
            InitializeComponent();
            this.tk = tk;
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void detailTaiKhoan_Load(object sender, EventArgs e)
        {
            bool isAdmin = (Session.TaiKhoanHienTai.MAVAITRO == 1);
            nhanVienDTO nv = dsNV.FirstOrDefault(x => x.MaNhanVien == tk.MANHANVIEN);
            vaitroDTO vt = dsVT.FirstOrDefault(x => x.MaVaiTro == tk.MAVAITRO);

            txtTenNV.Text = nv?.HoTen ?? "Không xác định";
            txtTenVT.Text = vt?.TenVaiTro ?? "Không xác định";
            if(isAdmin)
            {
                txtTenTK.Text = tk.TENDANGNHAP;
            }else
            {
                txtTenTK.Text = "Không có quyền xem tài khoản";
            }
            if (isAdmin)
            {
                txtMatKhau.Text = tk.MATKHAU;
            }
            else
            {
                txtMatKhau.Text = "Không có quyền xem mật khẩu";
            }

                txtNgayTao.Text = tk.NGAYTAO.ToString();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
