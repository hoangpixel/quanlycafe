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
                    string fileName = Path.GetFileName(imagePath);

                    // ✅ Lấy đường dẫn gốc project (chứa .csproj)
                    string projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\quanlycafe"));

                    // ✅ Copy ảnh vào thư mục Resources/IMG/SP trong project (để commit Git)
                    string targetFolderProject = Path.Combine(projectDir, "Resources", "IMG", "SP");
                    string targetPathProject = Path.Combine(targetFolderProject, fileName);

                    if (!Directory.Exists(targetFolderProject))
                        Directory.CreateDirectory(targetFolderProject);

                    File.Copy(imagePath, targetPathProject, true);

                    // ✅ Copy thêm ảnh vào thư mục bin/Debug/IMG/SP (để hiển thị ngay)
                    string targetFolderBin = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IMG", "SP");
                    string targetPathBin = Path.Combine(targetFolderBin, fileName);

                    if (!Directory.Exists(targetFolderBin))
                        Directory.CreateDirectory(targetFolderBin);

                    File.Copy(imagePath, targetPathBin, true);

                    // ✅ Cập nhật đường dẫn ảnh trong DB
                    sp.Hinh = "SP/" + fileName;
                }

                // ✅ Cập nhật thông tin sản phẩm
                sp.TenSP = txtTenSP.Text.Trim();
                sp.Gia = float.Parse(txtGia.Text);
                sp.MaLoai = Convert.ToInt32(cbLoai.SelectedValue);

                // ✅ Gọi BUS để cập nhật
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
