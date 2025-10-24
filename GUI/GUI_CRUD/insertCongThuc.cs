using BUS;
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
using GUI.GUI_SELECT;

namespace GUI.GUI_CRUD
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
            donViBUS dvBUS = new donViBUS();
            var dsDonVi = dvBUS.layDanhSachDonVi();
            cboDonVi.DisplayMember = "TenDonVi";
            cboDonVi.ValueMember = "MaDonVi";
            cboDonVi.DataSource = dsDonVi;
            cboDonVi.SelectedIndex = -1;
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

                    nguyenLieuBUS nlBUS = new nguyenLieuBUS();
                    var dsNL = nlBUS.docDSNguyenLieu();
                    var nl = dsNL.FirstOrDefault(x => x.MaNguyenLieu == maNL);

                    if (nl != null)
                    {
                        cboDonVi.SelectedValue = nl.MaDonViCoSo;
                    }
                    else
                    {
                        cboDonVi.SelectedIndex = -1;
                    }

                    txtSoLuong.Focus();
                }
            }
        }

        private void btnNhapCT_Click(object sender, EventArgs e)
        {
            if (maSP == -1 || maNL == -1 || txtSoLuong.Value <= 0 || cboDonVi.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn đủ sản phẩm, nguyên liệu, đơn vị và nhập số lượng hợp lệ!",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDonVi = Convert.ToInt32(cboDonVi.SelectedValue);
            string tenDonVi = cboDonVi.Text;

            var tonTai = dsTam.FirstOrDefault(x => x.MaNguyenLieu == maNL);

            if (tonTai != null)
            {
                DialogResult confirm = MessageBox.Show(
                    $"Nguyên liệu '{txtTenNguyenLieu.Text}' đã tồn tại trong danh sách.\n" +
                    $"Bạn có muốn cộng dồn thêm {txtSoLuong.Value} {tenDonVi} không?",
                    "Xác nhận cộng dồn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    tonTai.SoLuongCoSo += (float)txtSoLuong.Value;
                    tonTai.MaDonViCoSo = maDonVi;
                    tonTai.TenDonViCoSo = tenDonVi;

                    MessageBox.Show(
                        $"Đã cộng dồn nguyên liệu '{txtTenNguyenLieu.Text}'.\n" +
                        $"Tổng mới: {tonTai.SoLuongCoSo} {tenDonVi}",
                        "Cập nhật công thức tạm",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    return;
                }
            }
            else
            {
                congThucDTO ct = new congThucDTO
                {
                    MaSanPham = maSP,
                    MaNguyenLieu = maNL,
                    TenNguyenLieu = txtTenNguyenLieu.Text,
                    SoLuongCoSo = (float)txtSoLuong.Value,
                    MaDonViCoSo = maDonVi,
                    TenDonViCoSo = tenDonVi,
                    TrangThai = 1
                };

                dsTam.Add(ct);
            }

            tableCongThuc.DataSource = null;
            tableCongThuc.DataSource = dsTam.Select(x => new
            {
                MaNL = x.MaNguyenLieu,
                TenNL = x.TenNguyenLieu,
                SoLuong = x.SoLuongCoSo,
                DonVi = x.TenDonViCoSo
            }).ToList();

            tableCongThuc.Columns["MaNL"].HeaderText = "Mã NL";
            tableCongThuc.Columns["TenNL"].HeaderText = "Tên NL";
            tableCongThuc.Columns["SoLuong"].HeaderText = "Số lượng";
            tableCongThuc.Columns["DonVi"].HeaderText = "Đơn vị";

            // 🧹 Dọn input
            btnChonSP.Enabled = false;
            txtTenNguyenLieu.Clear();
            cboDonVi.SelectedIndex = -1;
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
                bool kq = bus.themCongThuc(ct);
                Console.WriteLine($"Lưu CT cho SP {ct.MaSanPham}, NL {ct.MaNguyenLieu}: {kq}");
            }

            btnChonSP.Enabled = true;
            txtTenSanPham.Clear();
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
                    MaNL = x.MaNguyenLieu,
                    TenNL = x.TenNguyenLieu,
                    SoLuong = x.SoLuongCoSo,
                    DonVi = x.TenDonViCoSo
                }).ToList();

                tableCongThuc.Columns["MaNL"].HeaderText = "Mã NL";
                tableCongThuc.Columns["TenNL"].HeaderText = "Tên NL";
                tableCongThuc.Columns["SoLuong"].HeaderText = "Số lượng";
                tableCongThuc.Columns["DonVi"].HeaderText = "Đơn vị";

                if (dsTam.Count == 0)
                {
                    btnChonSP.Enabled = true;
                    txtTenSanPham.Clear();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
