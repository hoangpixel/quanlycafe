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
                using (var fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                {
                    picHinh.Image = Image.FromStream(fs);
                }
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

                using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    picHinh.Image = Image.FromStream(fs);
                }

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
            this.Close();
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
                if (!string.IsNullOrEmpty(imagePath))
                {
                    string fileName = Path.GetFileName(imagePath);

                    // ✅ Đường dẫn gốc project (chứa .csproj)
                    string projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\quanlycafe"));

                    // ✅ Thư mục Resources/IMG/SP
                    string targetFolderProject = Path.Combine(projectDir, "Resources", "IMG", "SP");
                    string targetPathProject = Path.Combine(targetFolderProject, fileName);

                    if (!Directory.Exists(targetFolderProject))
                        Directory.CreateDirectory(targetFolderProject);

                    // ✅ Chỉ copy nếu ảnh chưa tồn tại
                    if (!File.Exists(targetPathProject))
                    {
                        File.Copy(imagePath, targetPathProject);
                    }

                    // ✅ Copy thêm 1 bản xuống bin/Debug/IMG/SP
                    string targetFolderBin = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IMG", "SP");
                    string targetPathBin = Path.Combine(targetFolderBin, fileName);

                    if (!Directory.Exists(targetFolderBin))
                        Directory.CreateDirectory(targetFolderBin);

                    if (!File.Exists(targetPathBin))
                    {
                        File.Copy(imagePath, targetPathBin);
                    }

                    // ✅ Cập nhật đường dẫn ảnh tương đối
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
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
