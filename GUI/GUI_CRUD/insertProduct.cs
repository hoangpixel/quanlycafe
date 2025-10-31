using BUS;
using DTO;
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
using FONTS;


namespace GUI.GUI_CRUD
{
    public partial class insertProduct : Form
    {
        private string imagePath = "";
        public sanPhamDTO ct;
        public insertProduct(sanPhamDTO ct)
        {
            InitializeComponent();
            loadComBoBox();
            this.ct = ct;
        }

        public void loadComBoBox()
        {
            loaiSanPhamBUS bus = new loaiSanPhamBUS();
            BindingList<loaiDTO> dsLoai = bus.LayDanhSach();

            cbLoai.DataSource = dsLoai;
            cbLoai.DisplayMember = "TenLoai";
            cbLoai.ValueMember = "MaLoai";
            cbLoai.SelectedIndex = -1;
        }

        private void insertProduct_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void btnChonAnh_Click_1(object sender, EventArgs e)
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

        private void btnXoaAnh_Click_1(object sender, EventArgs e)
        {
            picHinh.Image = null;
            imagePath = "";
        }

        private void btnNhapSP_Click_2(object sender, EventArgs e)
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

            string extension = Path.GetExtension(imagePath);
            string randomName = "sp_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N").Substring(0, 6) + extension;

            string projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\GUI"));
            string targetFolder = Path.Combine(projectDir, "Resources", "IMG", "SP");
            string targetPath = Path.Combine(targetFolder, randomName);

            try
            {
                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);

                File.Copy(imagePath, targetPath, true);

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

            sanPhamBUS bus = new sanPhamBUS();
            sanPhamDTO sp = new sanPhamDTO();
            sp.MaSP = bus.layMaSP();
            sp.MaLoai = Convert.ToInt32(cbLoai.SelectedValue);
            sp.TenSP = txtTenSP.Text.Trim();
            sp.Gia = float.Parse(txtGia.Text);
            sp.Hinh = randomName;

                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ct = sp;
                this.DialogResult = DialogResult.OK;
                this.Close();
        }


        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
