using quanlycafe.BUS;
using quanlycafe.DTO;
using quanlycafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace quanlycafe.GUI
{
    public partial class updateProduct : Form
    {
        private string imagePath = "";
        private sanPhamDTO sp;

        public updateProduct(sanPhamDTO sp)
        {
            InitializeComponent();
            this.sp = sp;
        }

        private void updateProduct_Load(object sender, EventArgs e)
        {
            txtTenSP.Text = sp.TenSP;
            txtGia.Text = sp.Gia.ToString();

            loaiSanPhamBUS loaiBus = new loaiSanPhamBUS();
            cbLoai.DataSource = loaiBus.layDanhSachLoai();
            cbLoai.DisplayMember = "TenLoai";
            cbLoai.ValueMember = "MaLoai";
            cbLoai.SelectedValue = sp.MaLoai;
            string imgPath = Path.Combine(Application.StartupPath, "IMG", sp.Hinh);
            if (File.Exists(imgPath))
            {
                picHinh.Image = Image.FromFile(imgPath);
                picHinh.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                picHinh.Image = null;
                Console.WriteLine("Ảnh không tồn tại: " + imgPath);
            }


        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Ảnh sản phẩm (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

            if (open.ShowDialog() == DialogResult.OK)
            {
                imagePath = open.FileName;
                sp.Hinh = imagePath;
                picHinh.Image = Image.FromFile(imagePath);
                picHinh.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void btnXoaAnh_Click(object sender, EventArgs e)
        {
            picHinh.Image = null;
            imagePath = "";
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNhapSP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenSP.Text) || string.IsNullOrWhiteSpace(txtGia.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn cập nhật sản phẩm này không?",
                "Xác nhận cập nhật",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.No)
                return;

            try
            {
                // Nếu có chọn ảnh mới
                if (!string.IsNullOrEmpty(imagePath))
                {
                    // Lấy tên file ảnh
                    string fileName = Path.GetFileName(imagePath);

                    // Tạo đường dẫn thư mục IMG/SP (trong bin)
                    string targetFolder = Path.Combine(Application.StartupPath, "IMG", "SP");
                    string targetPath = Path.Combine(targetFolder, fileName);

                    // Nếu thư mục chưa có thì tạo mới
                    if (!Directory.Exists(targetFolder))
                        Directory.CreateDirectory(targetFolder);

                    // Sao chép ảnh vào thư mục (ghi đè nếu đã tồn tại)
                    if (!File.Exists(targetPath))
                        File.Copy(imagePath, targetPath, true);

                    // Cập nhật lại đường dẫn hình trong DB
                    sp.Hinh = "SP/" + fileName;
                }

                // Cập nhật thông tin sản phẩm
                sp.TenSP = txtTenSP.Text;
                sp.Gia = float.Parse(txtGia.Text);
                sp.MaLoai = Convert.ToInt32(cbLoai.SelectedValue);

                // Gọi BUS để cập nhật
                sanPhamBUS bus = new sanPhamBUS();
                bus.Sua(sp);

                MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
