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
using BUS;
namespace GUI.GUI_CRUD
{
    public partial class frmChiTietHD : Form
    {
        private hoaDonDTO hd;
        private hoaDonDAO hdDAO = new hoaDonDAO();
        private BindingList<ppThanhToanDTO> dsThanhToan;
        public frmChiTietHD(hoaDonDTO hd)
        {
            InitializeComponent();
            this.hd = hd;
            this.Text = $"Chi Tiết Hóa Đơn - HD{hd.MaHD}";

            dsThanhToan = new ppThanhToanBUS().LayDanhSach();            

            LoadDuLieu();
        }
        private void LoadDuLieu()
        {

                txtBan.Text = $"Bàn {hd.MaBan}";
                txtKhachHang.Text = hd.TenKhachHang ?? "Khách lẻ";
                txtNhanVien.Text = hd.HoTen ?? "Nhân viên";
                txtThgian.Text = hd.ThoiGianTao.ToString("HH:mm dd/MM/yyyy");
                txtTongTien.Text = hd.TongTien.ToString("N0") + " VNĐ";
                ppThanhToanDTO TT = dsThanhToan.FirstOrDefault(x => x.MaTT == hd.MaTT);
                txtThanhToan.Text = TT?.HinhThuc ?? "Khong xac dinh";

            Console.Write(hd.MaTT);

            // 2. Load danh sách món ăn (chi tiết)
            BindingList<cthoaDonDTO> dsCT = hdDAO.LayChiTietHoaDon(hd.MaHD);

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
