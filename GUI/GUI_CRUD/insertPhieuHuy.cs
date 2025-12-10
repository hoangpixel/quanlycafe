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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class insertPhieuHuy : Form
    {
        private int maNL = -1, maNV = -1, maHD = -1, maDV = -1, indexMaNL = -1;
        public BindingList<phieuHuyDTO> dsTam = new BindingList<phieuHuyDTO>();
        private BindingList<donViDTO> dsDonVi;
        private BindingList<nguyenLieuDTO> dsNguyenLieu;
        private int lastSelectNguyenLieu = -1;
        private hoaDonDTO hd;

        public insertPhieuHuy(hoaDonDTO hd)
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
            this.hd = hd;
        }

        private void insertPhieuHuy_Load(object sender, EventArgs e)
        {
            dsDonVi = new donViBUS().LayDanhSach();
            dsNguyenLieu = new nguyenLieuBUS().LayDanhSach();

            btnSuaCT.Enabled = false;
            btnXoaCT.Enabled = false;

            maHD = hd.MaHD;
            txtMaHD.Text = hd.MaHD.ToString();
            tuDongLoadTenNhanVien();
                setupBangNguyenLieu();
    tableNguyenLieu.DataSource = dsTam;
        }

        private void tuDongLoadTenNhanVien()
        {
            if (DTO.Session.NhanVienHienTai != null)
            {
                txtTenNhanVien.Text = DTO.Session.NhanVienHienTai.HoTen;
                maNV = DTO.Session.NhanVienHienTai.MaNhanVien;
            }
        }

        private void loadFontChuVaSizeTableCongThuc()
        {
            foreach (DataGridViewColumn col in tableNguyenLieu.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            tableNguyenLieu.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tableNguyenLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tableNguyenLieu.DefaultCellStyle.Font = FontManager.GetLightFont(10);
            tableNguyenLieu.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);
            tableNguyenLieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableNguyenLieu.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            tableNguyenLieu.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            tableNguyenLieu.Refresh();
        }

        private void btnChonNL_Click(object sender, EventArgs e)
        {
            using (selectNguyenLieu form = new selectNguyenLieu())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    maNL = form.MaNL;
                    txtTenNguyenLieu.Text = form.TenNL;
                }
            }
        }

        private void btnChonDonVi_Click(object sender, EventArgs e)
        {
            if (maNL == -1)
            {
                MessageBox.Show("Bạn chưa có nhập nguyên liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                using (selectDonVi form = new selectDonVi())
                {
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.chiLayHeSo = true;
                    form.maNguyenLieu = maNL;
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        maDV = form.maDonVi;
                        txtDonVi.Text = form.tenDonVi;
                    }
                }
            }
        }

        private void btnNhapCT_Click(object sender, EventArgs e)
        {
            if(maHD == -1)
            {
                MessageBox.Show("Lỗi không tìm thấy hóa đơn", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(maNV == -1)
            {
                MessageBox.Show("Lỗi không tìm thấy nhân viên", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(maNL == -1)
            {
                MessageBox.Show("Không được để trống nguyên liệu", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (maDV == -1)
            {
                MessageBox.Show("Không được để trống đơn vị", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtSoLuong.Value < 1)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0", "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSoLuong.Focus();
                return;
            }

            nguyenLieuDTO nl = dsNguyenLieu.FirstOrDefault(x => x.MaNguyenLieu == maNL);

            if (nl == null) return;

            phieuHuyBUS bus = new phieuHuyBUS();
            double heSo = bus.LayHeSo(maNL, maDV);

            decimal soLuongQuyDoi = txtSoLuong.Value * (decimal)heSo;

            if (soLuongQuyDoi > nl.TonKho)
            {
                MessageBox.Show($"Số lượng nhập ({txtSoLuong.Value} {txtDonVi.Text}) tương đương {soLuongQuyDoi:N0} (đơn vị gốc).\nLớn hơn tồn kho hiện tại là {nl.TonKho:N0}!",
                                "Cảnh cáo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }

            phieuHuyDTO tonTai = dsTam.FirstOrDefault(x => x.MaNguyenLieu == maNL && x.MaDonVi == maDV);
            if (tonTai != null)
            {
                tonTai.SoLuong += txtSoLuong.Value;
                tableNguyenLieu.Refresh();
            }
            else
            {
                phieuHuyDTO ph = new phieuHuyDTO();
                ph.MaHoaDon = maHD;
                ph.MaNhanVien = maNV;
                ph.MaNguyenLieu = maNL;
                ph.MaDonVi = maDV;
                ph.SoLuong = txtSoLuong.Value;

                dsTam.Add(ph);
            }
            resetInput();
        }

        private void btnSuaCT_Click(object sender, EventArgs e)
        {
            if (indexMaNL < 0 || indexMaNL >= dsTam.Count) return;

            // Cập nhật dữ liệu từ textbox vào dòng đang chọn trong dsTam
            dsTam[indexMaNL].MaDonVi = maDV;
            dsTam[indexMaNL].SoLuong = txtSoLuong.Value;
            // dsTam[indexMaNL].LyDo = ... (nếu có)

            tableNguyenLieu.Refresh(); // Làm mới hiển thị
            resetInput();
            btnSuaCT.Enabled = false;
            btnXoaCT.Enabled = false;
            btnNhapCT.Enabled = true;
            btnChonNL.Enabled = true;
        }

        private void tableNguyenLieu_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dsTam.Count) return;

            // SỬA LỖI: Ép kiểu về phieuHuyDTO
            phieuHuyDTO ph = dsTam[e.RowIndex];

            if (tableNguyenLieu.Columns[e.ColumnIndex].HeaderText == "Đơn vị")
            {
                // Tìm tên đơn vị dựa trên MaDonVi trong phieuHuyDTO
                donViDTO dv = dsDonVi.FirstOrDefault(x => x.MaDonVi == ph.MaDonVi);
                e.Value = dv?.TenDonVi ?? "Không xác định";
                e.FormattingApplied = true;
            }
            if (tableNguyenLieu.Columns[e.ColumnIndex].HeaderText == "Tên NL")
            {
                nguyenLieuDTO nl = dsNguyenLieu.FirstOrDefault(x => x.MaNguyenLieu == ph.MaNguyenLieu);
                e.Value = nl?.TenNguyenLieu ?? "Không xác định";
                e.FormattingApplied = true;
            }
        }

        private void btnLuuCT_Click(object sender, EventArgs e)
        {
            if (dsTam.Count == 0)
            {
                MessageBox.Show("Danh sách hủy đang trống!", "Thông báo");
                return;
            }
            phieuHuyBUS bus = new phieuHuyBUS();
            int countSuccess = 0;
            foreach (var ph in dsTam)
            {
                ph.LyDo = "Hủy món sai - Hóa đơn " + maHD;

                bool ketQua = bus.them(ph, ph.MaDonVi, ph.SoLuong);

                if (ketQua) countSuccess++;
            }
            if (countSuccess == dsTam.Count)
            {
                MessageBox.Show("Đã cập nhật phiếu hủy và trừ kho thành công!", "Thông báo");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show($"Hoàn tất {countSuccess}/{dsTam.Count} mục. Có một số mục bị lỗi (có thể do hết hàng hoặc lỗi mạng).", "Thông báo");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnXoaCT_Click(object sender, EventArgs e)
        {
            if (indexMaNL < 0 || indexMaNL >= dsTam.Count) return;

            if (MessageBox.Show("Bạn chắc chắn muốn xóa dòng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dsTam.RemoveAt(indexMaNL);
                resetInput();
                btnSuaCT.Enabled = false;
                btnXoaCT.Enabled = false;
                btnNhapCT.Enabled = true;
                btnChonNL.Enabled = true;
            }
        }

        private void tableNguyenLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.RowIndex == lastSelectNguyenLieu)
            {
                btnNhapCT.Enabled = true;
                btnSuaCT.Enabled = false;
                btnXoaCT.Enabled = false;
                btnChonNL.Enabled = true;
                resetInput();
                return;
            }

            DataGridViewRow row = tableNguyenLieu.Rows[e.RowIndex];
            phieuHuyDTO ph = dsTam[e.RowIndex];

            this.maNL = ph.MaNguyenLieu;
            this.maDV = ph.MaDonVi;
            this.indexMaNL = e.RowIndex;

            nguyenLieuDTO nl = dsNguyenLieu.FirstOrDefault(x => x.MaNguyenLieu == ph.MaNguyenLieu);
            donViDTO dv = dsDonVi.FirstOrDefault(x => x.MaDonVi == ph.MaDonVi);

            txtTenNguyenLieu.Text = nl?.TenNguyenLieu ?? "Không xác định";
            txtSoLuong.Value = ph.SoLuong;
            txtDonVi.Text = dv?.TenDonVi ?? "Không xác định";

            lastSelectNguyenLieu = e.RowIndex;

            btnNhapCT.Enabled = false;
            btnSuaCT.Enabled = true;
            btnXoaCT.Enabled = true;

            btnChonDonVi.Enabled = true;
            btnChonNL.Enabled = false;
        }

        private void resetInput()
        {
            tableNguyenLieu.ClearSelection();
            txtTenNguyenLieu.Clear();
            txtSoLuong.Value = 0;
            txtDonVi.Clear();
            maNL = -1;
            maDV = -1;
            txtDonVi.Clear();
            lastSelectNguyenLieu = -1;
        }

        //private void loadBangNguyenLieu()
        //{
        //    tableNguyenLieu.AutoGenerateColumns = false;
        //    tableNguyenLieu.Columns.Clear();

        //    tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Mã NL", Visible = false });
        //    tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNguyenLieu", HeaderText = "Tên NL" });
        //    tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoLuong", HeaderText = "Số lượng" });
        //    tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaDonVi", HeaderText = "Đơn vị" });

        //    tableNguyenLieu.DataSource = dsTam;

        //    btnSuaCT.Enabled = false;
        //    btnXoaCT.Enabled = false;
        //    tableNguyenLieu.ClearSelection();
        //    loadFontChuVaSizeTableCongThuc();
        //}
        private void setupBangNguyenLieu()
        {
            tableNguyenLieu.AutoGenerateColumns = false;

            tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaNguyenLieu",
                HeaderText = "Mã NL"
            });

            tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaNguyenLieu",
                HeaderText = "Tên NL"
            });

            tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoLuong",
                HeaderText = "Số lượng"
            });

            tableNguyenLieu.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaDonVi",
                HeaderText = "Đơn vị"
            });
            loadFontChuVaSizeTableCongThuc();
        }

    }
}
