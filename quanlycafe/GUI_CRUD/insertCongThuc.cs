using quanlycafe.BUS;
using quanlycafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quanlycafe.GUI_CRUD
{
    public partial class insertCongThuc : Form
    {
        private int maSP = -1;
        private int maNL = -1;
        private List<congThucDTO> dsTam = new List<congThucDTO>();
        public insertCongThuc()
        {
            InitializeComponent();
        }

        private void btnChonSP_Click(object sender, EventArgs e)
        {
            using (selectSanPham form = new selectSanPham())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    maSP = form.MaSP;
                    txtTenSanPham.Text = form.TenSP;
                    txtTenNguyenLieu.Focus();
                }
            }
        }

        private void insertCongThuc_Load(object sender, EventArgs e)
        {

        }

        private void btnChonNL_Click(object sender, EventArgs e)
        {
            if (maSP == -1)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm trước khi chọn nguyên liệu!",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (selectNguyenLieu form = new selectNguyenLieu())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    maNL = form.MaNL;
                    txtTenNguyenLieu.Text = form.TenNL;
                    txtSoLuong.Focus();
                }
            }
        }

        private void btnNhapCT_Click(object sender, EventArgs e)
        {
            // ✅ ép control cập nhật giá trị đang nhập trước khi đọc
            txtSoLuong.Focus();
            txtSoLuong.Select(0, txtSoLuong.Text.Length);
            txtSoLuong.Validate();

            if (maSP == -1 || maNL == -1 || txtSoLuong.Value <= 0)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ sản phẩm, nguyên liệu và nhập số lượng hợp lệ!",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra xem nguyên liệu này đã có trong danh sách tạm chưa
            var tonTai = dsTam.FirstOrDefault(x => x.MaNguyenLieu == maNL);

            if (tonTai != null)
            {
                // 🔁 Nếu đã có => cộng dồn số lượng
                tonTai.SoLuongCoSo += (float)txtSoLuong.Value;

                MessageBox.Show(
                    $"Nguyên liệu '{txtTenNguyenLieu.Text}' đã có, hệ thống tự cộng dồn.\n" +
                    $"→ Tổng mới: {tonTai.SoLuongCoSo}",
                    "Cập nhật công thức tạm",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                // 🆕 Nếu chưa có => thêm mới
                congThucDTO ct = new congThucDTO
                {
                    MaSanPham = maSP,
                    MaNguyenLieu = maNL,
                    TenNguyenLieu = txtTenNguyenLieu.Text,
                    SoLuongCoSo = (float)txtSoLuong.Value,
                    TrangThai = 1
                };

                dsTam.Add(ct);

            }

            // 🔄 Cập nhật lại DataGridView
            tableCongThuc.DataSource = null;
            tableCongThuc.DataSource = dsTam.Select(x => new
            {
                MaNL = x.MaNguyenLieu,
                TenNL = x.TenNguyenLieu,
                SoLuong = x.SoLuongCoSo
            }).ToList();
            tableCongThuc.Columns["MaNL"].HeaderText = "Mã NL";
            tableCongThuc.Columns["TenNL"].HeaderText = "Tên NL";
            tableCongThuc.Columns["SoLuong"].HeaderText = "Số lượng";
            // 🧹 Reset input
            btnChonSP.Enabled = false;
            txtTenNguyenLieu.Clear();
            txtSoLuong.Value = 0;
            maNL = -1;
        }

        private void btnLuuCT_Click(object sender, EventArgs e)
        {
            if (dsTam.Count == 0)
            {
                MessageBox.Show("Chưa có công thức nào để lưu!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            congThucBUS bus = new congThucBUS();
            foreach (var ct in dsTam)
            {
                //string debug = $"[Lưu CT xuống DB]\n" +
                //       $"→ Mã SP: {ct.MaSanPham}\n" +
                //       $"→ Mã NL: {ct.MaNguyenLieu}\n" +
                //       $"→ Tên NL: {ct.TenNguyenLieu}\n" +
                //       $"→ Số lượng: {ct.SoLuongCoSo}";
                //MessageBox.Show(debug, "DEBUG - Gửi xuống DB", MessageBoxButtons.OK, MessageBoxIcon.Information);

                bool kq = bus.themCongThuc(ct);
                Console.WriteLine($"Lưu CT cho SP {ct.MaSanPham}, NL {ct.MaNguyenLieu}: {kq}");
            }

            btnChonSP.Enabled = true;
            txtTenSanPham.Text = "";
            dsTam.Clear();
            tableCongThuc.DataSource = null;

            MessageBox.Show("Đã lưu công thức thành công!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoaCT_Click(object sender, EventArgs e)
        {
            if (tableCongThuc.SelectedRows.Count > 0)
            {
                int index = tableCongThuc.SelectedRows[0].Index;
                dsTam.RemoveAt(index);

                tableCongThuc.DataSource = null;
                tableCongThuc.DataSource = dsTam.Select(x => new
                {
                    Mã_NL = x.MaNguyenLieu,
                    Tên_NL = x.TenNguyenLieu,
                    Số_lượng = x.SoLuongCoSo
                }).ToList();

                if (dsTam.Count == 0)
                    btnChonSP.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
