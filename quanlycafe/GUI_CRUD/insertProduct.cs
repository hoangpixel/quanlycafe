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

            string fileName = Path.GetFileName(imagePath);

            // ✅ Đường dẫn gốc project (chứa quanlycafe.csproj)
            string projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\quanlycafe"));

            // ✅ Thư mục Resources\IMG\SP trong project
            string targetFolder = Path.Combine(projectDir, "Resources", "IMG", "SP");
            string targetPath = Path.Combine(targetFolder, fileName);

            try
            {
                // Nếu thư mục chưa có thì tạo
                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);

                // Copy ảnh vào thư mục project
                File.Copy(imagePath, targetPath, true);

                // ✅ Copy thêm một bản xuống thư mục bin/Debug/IMG/SP để hiển thị ngay
                string binFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IMG", "SP");
                string binPath = Path.Combine(binFolder, fileName);

                if (!Directory.Exists(binFolder))
                    Directory.CreateDirectory(binFolder);

                File.Copy(imagePath, binPath, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ Lưu đường dẫn tương đối — chỉ còn "SP/tên_ảnh" cho gọn (vì khi chạy form sẽ load từ bin)
            sanPhamDTO sp = new sanPhamDTO
            {
                MaLoai = Convert.ToInt32(cbLoai.SelectedValue),
                TenSP = txtTenSP.Text.Trim(),
                Gia = float.Parse(txtGia.Text),
                Hinh = "SP/" + fileName
            };

            sanPhamBUS bus = new sanPhamBUS();
            bus.them(sp);

            MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
