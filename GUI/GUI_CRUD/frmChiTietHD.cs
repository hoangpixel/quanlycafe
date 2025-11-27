using DAO;
using DTO;
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
    public partial class frmChiTietHD : Form
    {
        private int maHD;
        private hoaDonDAO hdDAO = new hoaDonDAO();
        BindingList<cthoaDonDTO> dsCT;
        public frmChiTietHD()
        {
            InitializeComponent();
            this.maHD = maHD;
            this.Text = $"Chi Tiết Hóa Đơn - HD{maHD}";
            LoadDuLieu();
        }
        private void LoadDuLieu()
        {

            // 1. Load thông tin hóa đơn (bàn, khách, nhân viên, thời gian)
            var hd = hdDAO.LayThongTinHoaDon(maHD);
            if (hd != null)
            {
                txtBan.Text = $"Bàn {hd.MaBan}";
                txtKhachHang.Text = hd.TenKhachHang ?? "Khách lẻ";
                txtNhanVien.Text = hd.HoTen ?? "Nhân viên";
                txtThgian.Text = hd.ThoiGianTao.ToString("HH:mm dd/MM/yyyy");
                txtTongTien.Text = hd.TongTien.ToString("N0") + " VNĐ";
            }

            // 2. Load danh sách món ăn (chi tiết)
            BindingList<cthoaDonDTO> dsCT = hdDAO.LayChiTietHoaDon(maHD);

            dgvChiTiet.DataSource = dsCT;
            dgvChiTiet.AutoGenerateColumns = false;
            dgvChiTiet.Columns.Clear();

            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenSP", HeaderText = "Tên món" });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuong", HeaderText = "SL" });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DonGia",
                HeaderText = "Đơn giá",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ThanhTien",
                HeaderText = "Thành tiền",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            // Căn giữa, font đẹp
            foreach (DataGridViewColumn col in dgvChiTiet.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
