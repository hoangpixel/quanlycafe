using DTO;
using BUS;
using DAO;
using System;
using FONTS;
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
    public partial class detailHoaDon : Form
    {
        private hoaDonDTO hd;
        private BindingList<banDTO> dsBan;
        private BindingList<khachHangDTO> dsKH;
        private BindingList<nhanVienDTO> dsNV;
        private BindingList<khuVucDTO> dsKhuVuc;
        private BindingList<ppThanhToanDTO> dsPPThanhToan;
        private BindingList<nguyenLieuDTO> dsNguyenLieu;
        private BindingList<donViDTO> dsDonVi;

        private hoaDonDAO hdDAO = new hoaDonDAO();
        public detailHoaDon(hoaDonDTO hd)
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
            this.hd = hd;

            dsNguyenLieu = new nguyenLieuBUS().LayDanhSach();
            dsDonVi = new donViBUS().LayDanhSach();
            dsBan = new banBUS().LayDanhSach();
            dsKH = new khachHangBUS().LayDanhSach();
            dsNV = new nhanVienBUS().LayDanhSach();
            dsKhuVuc = new khuvucBUS().LayDanhSach();
            dsPPThanhToan = new ppThanhToanBUS().LayDanhSach();

            loadDanhSachChiTietHD();
            loadFontChuVaSizeTableDonVi();

            loadDanhSachPhieuHuy();
            loadFontChuVaSizeTable();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void detailHoaDon_Load(object sender, EventArgs e)
        {



            txtMaHD.Text = hd.MaHD.ToString();
            banDTO ban = dsBan.FirstOrDefault(x => x.MaBan == hd.MaBan);
            khuVucDTO khuVuc = dsKhuVuc.FirstOrDefault(x => x.MaKhuVuc == ban.MaKhuVuc);
            txtBan.Text = ban?.TenBan + " - " + khuVuc?.TenKhuVuc;

            khachHangDTO kh = dsKH.FirstOrDefault(x => x.MaKhachHang == hd.MaKhachHang);
            if (kh == null)
            {
                if (hd.MaKhachHang == 0)
                {
                    txtKhachHang.Text = "Khách lẻ";
                }
                else
                {
                    txtKhachHang.Text = "Khách hàng không tồn tại (Đã xóa)";
                }
            }
            else
            {
                if (kh.MaKhachHang == 0)
                {
                    txtKhachHang.Text = "Khách lẻ";
                }
                else
                {
                    txtKhachHang.Text = kh.TenKhachHang;
                }
            }

            nhanVienDTO nv = dsNV.FirstOrDefault(x => x.MaNhanVien == hd.MaNhanVien);
            txtNhanVien.Text = nv?.HoTen ?? "Không xác định";

            ppThanhToanDTO thanhToan = dsPPThanhToan.FirstOrDefault(x => x.MaTT == hd.MaTT);
            txtThanhToan.Text = thanhToan?.HinhThuc ?? "Không xác định";

            txtThoiGian.Text = hd.ThoiGianTao.ToString("dd/MM/yyyy");

            txtTongTien.Text = hd.TongTien.ToString("N0");
        }

        private void loadDanhSachChiTietHD()
        {
            BindingList<cthoaDonDTO> dsCT = hdDAO.LayChiTietHoaDon(hd.MaHD);

            tbChiTietHoaDon.DataSource = dsCT;
            tbChiTietHoaDon.AutoGenerateColumns = false;
            tbChiTietHoaDon.Columns.Clear();

            tbChiTietHoaDon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenSP", HeaderText = "Tên món" });
            tbChiTietHoaDon.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuong", HeaderText = "SL" });
            tbChiTietHoaDon.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DonGia",
                HeaderText = "Đơn giá",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
            tbChiTietHoaDon.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ThanhTien",
                HeaderText = "Thành tiền",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
        }

        private void loadDanhSachPhieuHuy()
        {
            BindingList<phieuHuyDTO> dsPhieuHuy = new phieuHuyBUS().LayDanhSach(hd.MaHD);

            tbPhieuHuy.DataSource = dsPhieuHuy;
            tbPhieuHuy.AutoGenerateColumns = false;
            tbPhieuHuy.Columns.Clear();

            tbPhieuHuy.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNhanVien", HeaderText = "Nhân viên" });
            tbPhieuHuy.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Nguyên liệu" });
            tbPhieuHuy.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonVi", HeaderText = "Đơn vị" });
            tbPhieuHuy.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuong", HeaderText = "SL" });
            tbPhieuHuy.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "LyDo", HeaderText = "Lý do" });
        }

        private void loadFontChuVaSizeTableDonVi()
        {
            foreach (DataGridViewColumn col in tbChiTietHoaDon.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tbChiTietHoaDon.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tbChiTietHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tbChiTietHoaDon.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tbChiTietHoaDon.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            tbChiTietHoaDon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbChiTietHoaDon.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbChiTietHoaDon.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tbChiTietHoaDon.Refresh();
        }

        private void tbPhieuHuy_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            phieuHuyDTO ph = tbPhieuHuy.Rows[e.RowIndex].DataBoundItem as phieuHuyDTO;
            if (tbPhieuHuy.Columns[e.ColumnIndex].HeaderText == "Nhân viên")
            {
                nhanVienDTO nl = dsNV.FirstOrDefault(x => x.MaNhanVien == ph.MaNhanVien);
                e.Value = nl?.HoTen ?? "Không xác định";
            }
            if (tbPhieuHuy.Columns[e.ColumnIndex].HeaderText == "Nguyên liệu")
            {
                nguyenLieuDTO nl = dsNguyenLieu.FirstOrDefault(x => x.MaNguyenLieu == ph.MaNguyenLieu);
                e.Value = nl?.TenNguyenLieu ?? "Không xác định";
            }
            if (tbPhieuHuy.Columns[e.ColumnIndex].HeaderText == "Đơn vị")
            {
                // Cách cũ của bạn (Sai vì ph.MaDonVi = 0):
                // donViDTO nl = dsDonVi.FirstOrDefault(x => x.MaDonVi == ph.MaDonVi);

                // --- CÁCH SỬA ---
                // Bước 1: Tìm Nguyên liệu đó trước
                nguyenLieuDTO nguyenLieu = dsNguyenLieu.FirstOrDefault(x => x.MaNguyenLieu == ph.MaNguyenLieu);

                if (nguyenLieu != null)
                {
                    // Bước 2: Lấy đơn vị cơ sở từ Nguyên liệu tìm được
                    donViDTO donVi = dsDonVi.FirstOrDefault(x => x.MaDonVi == nguyenLieu.MaDonViCoSo);

                    // Bước 3: Hiển thị
                    e.Value = donVi?.TenDonVi ?? "Không xác định";
                }
                else
                {
                    e.Value = "Không xác định";
                }
            }
            if (tbPhieuHuy.Columns[e.ColumnIndex].HeaderText == "SL" && e.Value != null)
            {
                if (double.TryParse(e.Value.ToString(), out double tonKho))
                {
                    if (tonKho % 1 == 0)
                    {
                        e.Value = tonKho.ToString("N0");
                    }
                    else
                    {
                        e.Value = tonKho.ToString("0.000");
                    }
                    e.FormattingApplied = true;
                }
            }
        }

        private void loadFontChuVaSizeTable()
        {
            foreach (DataGridViewColumn col in tbPhieuHuy.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            tbPhieuHuy.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tbPhieuHuy.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            tbPhieuHuy.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            tbPhieuHuy.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            tbPhieuHuy.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tbPhieuHuy.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tbPhieuHuy.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            tbPhieuHuy.Refresh();
        }
    }
}
