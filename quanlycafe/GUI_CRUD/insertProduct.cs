using quanlycafe.BUS;
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

namespace quanlycafe.GUI_CRUD
{
    public partial class insertProduct : Form
    {
        private string imagePath = "";
        public insertProduct()
        {
            InitializeComponent();
            loadComBoBox();
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
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        public void loadComBoBox()
        {
            loaiSanPhamBUS bus = new loaiSanPhamBUS();
            List<loaiDTO> dsLoai = bus.layDanhSachLoai();

            cbLoai.DataSource = dsLoai;
            cbLoai.DisplayMember = "TenLoai";
            cbLoai.ValueMember = "MaLoai";

        }

        private void insertProduct_Load(object sender, EventArgs e)
        {

        }

        private void btnNhapSP_Click(object sender, EventArgs e)
        {
        }

        private void btnNhapSP_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenSP.Text) || string.IsNullOrWhiteSpace(txtGia.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(imagePath))
            {
                MessageBox.Show("Vui lòng chọn ảnh sản phẩm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //string fileName = Path.GetFileName(imagePath);

            //// ✅ Đường dẫn gốc project (chứa quanlycafe.csproj)
            //string projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\quanlycafe"));

            //// ✅ Thư mục Resources\IMG\SP trong project
            //string targetFolder = Path.Combine(projectDir, "Resources", "IMG", "SP");
            //string targetPath = Path.Combine(targetFolder, fileName);

            //try
            //{
            //    // Nếu thư mục chưa có thì tạo
            //    if (!Directory.Exists(targetFolder))
            //        Directory.CreateDirectory(targetFolder);

            //    // Nếu file ảnh chưa tồn tại thì mới copy
            //    if (!File.Exists(targetPath))
            //    {
            //        File.Copy(imagePath, targetPath);
            //    }

            //    // ✅ Copy thêm 1 bản xuống bin/Debug/IMG/SP để hiển thị ngay
            //    string binFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IMG", "SP");
            //    string binPath = Path.Combine(binFolder, fileName);

            //    if (!Directory.Exists(binFolder))
            //        Directory.CreateDirectory(binFolder);

            //    if (!File.Exists(binPath))
            //    {
            //        File.Copy(imagePath, binPath);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Lỗi khi lưu ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}


            //// ✅ Lưu đường dẫn tương đối — chỉ còn "SP/tên_ảnh" cho gọn (vì khi chạy form sẽ load từ bin)
            //sanPhamDTO sp = new sanPhamDTO
            //{
            //    MaLoai = Convert.ToInt32(cbLoai.SelectedValue),
            //    TenSP = txtTenSP.Text.Trim(),
            //    Gia = float.Parse(txtGia.Text),
            //    Hinh = "SP/" + fileName
            //};

            //sanPhamBUS bus = new sanPhamBUS();
            //bus.them(sp);

            //MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //this.Close();

            // ✅ Tạo tên file ngẫu nhiên
            string extension = Path.GetExtension(imagePath);
            string randomName = "sp_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N").Substring(0, 6) + extension;

            // ✅ Đường dẫn gốc project (chứa quanlycafe.csproj)
            string projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\quanlycafe"));
            string targetFolder = Path.Combine(projectDir, "Resources", "IMG", "SP");
            string targetPath = Path.Combine(targetFolder, randomName);

            try
            {
                // Nếu thư mục chưa có thì tạo
                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);

                // Copy file ảnh vào thư mục đích (với tên mới)
                File.Copy(imagePath, targetPath, true);

                // ✅ Copy thêm 1 bản xuống bin/Debug/IMG/SP để hiển thị ngay
                string binFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IMG", "SP");
                string binPath = Path.Combine(binFolder, randomName);

                if (!Directory.Exists(binFolder))
                    Directory.CreateDirectory(binFolder);

                File.Copy(imagePath, binPath, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ Lưu vào database chỉ TÊN FILE (vd: "sp_20251019_xxx.png")
            sanPhamDTO sp = new sanPhamDTO
            {
                MaLoai = Convert.ToInt32(cbLoai.SelectedValue),
                TenSP = txtTenSP.Text.Trim(),
                Gia = float.Parse(txtGia.Text),
                Hinh = randomName
            };

            sanPhamBUS bus = new sanPhamBUS();
            bus.them(sp);

            MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();

        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
