using System;
using System.ComponentModel;
using System.Windows.Forms;
using BUS;
using DTO;
using FONTS;

namespace GUI.GUI_CRUD
{
    public partial class excelTK : Form
    {
        public excelTK()
        {
            InitializeComponent();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            // ✅ THÊM ĐĂNG KÝ SỰ KIỆN CHO CÁC NÚT
            this.btnXuatExcel.Click += btnXuatExcel_Click;
            this.btnNhapExcel.Click += btnNhapExcel_Click;
            this.btnThoat.Click += btnThoat_Click;
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel Files|*.xlsx";
            save.FileName = "DanhSachTaiKhoan.xlsx";
            save.Title = "Lưu file Excel Tài khoản";

            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 1. Lấy dữ liệu từ DB lên
                    taikhoanBUS bus = new taikhoanBUS();
                    BindingList<taikhoanDTO> dsDB = bus.LayDanhSach();

                    // 2. Gọi hàm Export (Phải gọi rõ GUI.EXCEL)
                    GUI.EXCEL.excelTaiKhoan.Export(dsDB, save.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnNhapExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Excel Files|*.xlsx;*.xls";
            open.Title = "Chọn file Excel Tài khoản";

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 1. Đọc file Excel (Phải gọi rõ GUI.EXCEL để không nhầm với tên Form)
                    BindingList<taikhoanDTO> dsExcel = GUI.EXCEL.excelTaiKhoan.Import(open.FileName);

                    // 2. Gọi BUS Tài Khoản
                    taikhoanBUS bus = new taikhoanBUS();

                    // 3. Nhập từng tài khoản và đếm kết quả
                    int thanhCong = 0;
                    int thatBai = 0;
                    string chiTietLoi = "";

                    foreach (var tk in dsExcel)
                    {
                        try
                        {
                            if (bus.Them(tk))
                                thanhCong++;
                            else
                            {
                                thatBai++;
                                chiTietLoi += $"- Tài khoản '{tk.TENDANGNHAP}': Không thể thêm\n";
                            }
                        }
                        catch (Exception ex)
                        {
                            thatBai++;
                            chiTietLoi += $"- Tài khoản '{tk.TENDANGNHAP}': {ex.Message}\n";
                        }
                    }

                    // 4. Hiển thị kết quả
                    string ketQua = $"Nhập Excel hoàn tất!\n\n";
                    ketQua += $"✅ Thành công: {thanhCong} tài khoản\n";
                    ketQua += $"❌ Thất bại: {thatBai} tài khoản";

                    if (thatBai > 0)
                    {
                        ketQua += "\n\nChi tiết lỗi:\n" + chiTietLoi;
                    }

                    MessageBox.Show(ketQua,
                        "Kết quả nhập liệu",
                        MessageBoxButtons.OK,
                        thatBai > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi nhập Excel: " + ex.Message,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}