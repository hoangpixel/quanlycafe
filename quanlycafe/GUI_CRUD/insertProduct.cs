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

            // Lấy tên file ảnh
            string fileName = Path.GetFileName(imagePath);

            // Tạo đường dẫn thư mục IMG/SP
            string targetFolder = Path.Combine(Application.StartupPath, "IMG", "SP");
            string targetPath = Path.Combine(targetFolder, fileName);

            try
            {
                // Nếu thư mục chưa có thì tạo mới
                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);

                // Sao chép file ảnh (nếu chưa tồn tại)
                if (!File.Exists(targetPath))
                    File.Copy(imagePath, targetPath, true);
                MessageBox.Show("Ảnh lưu tại: " + targetPath);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sao chép ảnh: " + ex.Message);
                return;
            }

            // Tạo DTO để thêm sản phẩm
            sanPhamDTO sp = new sanPhamDTO
            {
                MaLoai = Convert.ToInt32(cbLoai.SelectedValue),
                TenSP = txtTenSP.Text,
                Gia = float.Parse(txtGia.Text),
                Hinh = "SP/" + fileName   // 👉 chỉ lưu "SP/tênfile.png" vào DB
            };

            // Gọi BUS để thêm sản phẩm
            sanPhamBUS bus = new sanPhamBUS();
            bus.them(sp);

            MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
