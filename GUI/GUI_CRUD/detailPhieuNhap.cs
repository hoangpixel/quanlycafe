using BUS;
using DTO;
using FONTS;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
namespace GUI.GUI_CRUD
{
    public partial class detailPhieuNhap : Form
    {
        private int _maPN;
        private phieuNhapBUS pnBus = new phieuNhapBUS();
        private nhaCungCapBUS nccBus = new nhaCungCapBUS();
        private nhanVienBUS nvBus = new nhanVienBUS();


        public detailPhieuNhap(int maPN)
        {
            InitializeComponent();
            _maPN = maPN;
            btnClose.Click += (s, e) => this.Close();
        }

        private void detailPhieuNhap_Load(object sender, EventArgs e)
        {
            try { FontManager.LoadFont(); FontManager.ApplyFontToAllControls(this); } catch { }
            LoadHeader();
            LoadDetails();
        }

        private void LoadHeader()
        {
            // Lấy thông tin phiếu nhập từ list
            var pn = pnBus.LayDanhSach().FirstOrDefault(x => x.MaPN == _maPN);

            if (pn != null)
            {
                txtMaPN.Text = pn.MaPN.ToString();
                txtNgayNhap.Text = pn.ThoiGian.ToString("dd/MM/yyyy HH:mm");
                txtTongTien.Text = pn.TongTien.ToString("N0") + " VNĐ";


                var ncc = nccBus.LayDanhSach().FirstOrDefault(x => x.MaNCC == pn.MaNCC);
                txtNhaCungCap.Text = ncc != null ? ncc.TenNCC : "Không xác định";


                var nv = nvBus.LayDanhSach().FirstOrDefault(x => x.MaNhanVien == pn.MaNhanVien);
                txtNhanVien.Text = nv != null ? nv.HoTen : "Không xác định";
            }
        }

        private void LoadDetails()
        {
            // Lấy danh sách chi tiết từ BUS
            var listDetails = pnBus.LayChiTiet(_maPN);

            dgvChiTiet.DataSource = listDetails;

            dgvChiTiet.Columns.Clear();
            dgvChiTiet.AutoGenerateColumns = false;

            
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Mã NL",
                DataPropertyName = "MaNguyenLieu",
            });

           
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tên Nguyên Liệu",
                DataPropertyName = "TenNguyenLieu",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

           
           dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ĐVT",
                DataPropertyName = "TenDonVi",
            });

            // Cột 4: Số Lượng
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "SL Nhập",
                DataPropertyName = "SoLuong",
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });


            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Đơn Giá",
                DataPropertyName = "DonGia",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Thành Tiền",
                DataPropertyName = "ThanhTien",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight, Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold) }
            });
            loadFontChuVaSizeTableNguyenLieu();
        }
        private void loadFontChuVaSizeTableNguyenLieu()
        {
            foreach (DataGridViewColumn col in dgvChiTiet.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            dgvChiTiet.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvChiTiet.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            dgvChiTiet.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChiTiet.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvChiTiet.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgvChiTiet.Refresh();
        }
    }
}