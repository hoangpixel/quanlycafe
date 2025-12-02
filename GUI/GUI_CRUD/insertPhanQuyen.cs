using BUS;
using DTO;
using FONTS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class insertPhanQuyen : Form
    {
        public insertPhanQuyen()
        {
            InitializeComponent();
        }

        private void insertPhanQuyen_Load(object sender, EventArgs e)
        {
            SetupTableLayoutPanelHeader();
            LoadVaiTroToComboBox();
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
        }

        private void LoadVaiTroToComboBox()
        {
            vaitroBUS vtBUS = new vaitroBUS();
            BindingList<vaitroDTO> dsVaiTro = vtBUS.LayDanhSach();

            cmbVaiTro.DataSource = dsVaiTro;
            cmbVaiTro.DisplayMember = "TenVaiTro";
            cmbVaiTro.ValueMember = "MaVaiTro";

            // Tìm Admin để chọn mặc định
            int indexAdmin = cmbVaiTro.FindStringExact("Admin");
            if (indexAdmin >= 0)
            {
                cmbVaiTro.SelectedIndex = indexAdmin;
            }
            else if (dsVaiTro.Any())
            {
                cmbVaiTro.SelectedIndex = 0;
            }

            // --- THÊM ĐOẠN NÀY ĐỂ ÉP NÓ HIỆN RA NGAY ---
            // Kiểm tra và gọi load quyền thủ công ngay lập tức, không chờ sự kiện
            if (cmbVaiTro.SelectedValue != null)
            {
                // Ép kiểu an toàn, phòng trường hợp SelectedValue chưa kịp nhận là int
                int maVaiTro = 0;
                if (cmbVaiTro.SelectedValue is int)
                {
                    maVaiTro = (int)cmbVaiTro.SelectedValue;
                }
                else if (cmbVaiTro.SelectedItem is vaitroDTO vt)
                {
                    maVaiTro = vt.MaVaiTro;
                }

                if (maVaiTro > 0)
                {
                    LoadPermissionsToUI(maVaiTro);
                }
            }
        }

        private void SetupTableLayoutPanelHeader()
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.RowCount = 1;

            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.BackColor = Color.White;

            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));

            // --- ĐÃ XÓA: Font cứng (Segoe UI 10) ---
            Color headerColor = Color.FromArgb(64, 64, 64);

            // Cột 0: Tên Quyền
            Label lblHeaderQuyen = new Label()
            {
                Text = "Tên Quyền / Chức năng",
                // Không set Font ở đây nữa, để FontManager tự lo
                ForeColor = headerColor,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 0, 0, 0)
            };
            tableLayoutPanel1.Controls.Add(lblHeaderQuyen, 0, 0);

            // Cột 1-4: Tiêu đề CRUD
            string[] crudHeaders = { "CREATE", "READ", "UPDATE", "DELETE" };
            for (int i = 0; i < crudHeaders.Length; i++)
            {
                Label lblHeader = new Label()
                {
                    Text = crudHeaders[i],
                    // Không set Font ở đây nữa
                    ForeColor = headerColor,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                tableLayoutPanel1.Controls.Add(lblHeader, i + 1, 0);
            }
        }

        // Sử dụng thư viện System.Data.DataTable
        private void LoadPermissionsToUI(int maVaiTro)
        {
            // Tạm dừng vẽ để mượt mà
            this.SuspendLayout();
            pnlPhanQuyen.SuspendLayout();
            gbPhanQuyen.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();

            try
            {
                SetupTableLayoutPanelHeader();

                phanquyenBUS pqBUS = new phanquyenBUS();
                BindingList<phanquyenDTO> listPermissions = pqBUS.LayChiTietQuyenTheoVaiTro(maVaiTro);

                int row = 1;
                // --- ĐÃ XÓA: Font rowFont = new Font(...) ---

                foreach (phanquyenDTO item in listPermissions)
                {
                    tableLayoutPanel1.RowCount++;
                    // AutoSize để hàng tự cao theo nội dung text
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                    int maQuyen = item.MaQuyen;

                    Label lblQuyen = new Label();
                    lblQuyen.Text = item.TenQuyen;
                    // Không gán Font cứng, để nó kế thừa hoặc được set bởi FontManager sau đó

                    // Cấu hình để Label tự xuống dòng nếu tên quá dài
                    lblQuyen.AutoSize = true;
                    lblQuyen.Dock = DockStyle.Fill;
                    lblQuyen.TextAlign = ContentAlignment.MiddleLeft;
                    lblQuyen.Padding = new Padding(10, 5, 0, 5);

                    tableLayoutPanel1.Controls.Add(lblQuyen, 0, row);

                    // --- Cột 1-4: Checkbox ---
                    AddPermissionCheckBox(item.CAN_CREATE, "CAN_CREATE", tableLayoutPanel1, 1, row, maQuyen);
                    AddPermissionCheckBox(item.CAN_READ, "CAN_READ", tableLayoutPanel1, 2, row, maQuyen);
                    AddPermissionCheckBox(item.CAN_UPDATE, "CAN_UPDATE", tableLayoutPanel1, 3, row, maQuyen);
                    AddPermissionCheckBox(item.CAN_DELETE, "CAN_DELETE", tableLayoutPanel1, 4, row, maQuyen);

                    row++;
                }
            }
            finally
            {
                // Vẽ lại giao diện 1 lần duy nhất
                tableLayoutPanel1.ResumeLayout(true);
                gbPhanQuyen.ResumeLayout(true);
                pnlPhanQuyen.ResumeLayout(true);

                // --- QUAN TRỌNG NHẤT: Áp dụng lại Font chuẩn cho các control vừa mới sinh ra ---
                // Vì các Label/Checkbox mới sinh ra sẽ mang font mặc định của hệ thống
                // nên cần gọi lại hàm này để đồng bộ với font của FontManager.
                FontManager.ApplyFontToAllControls(this);

                this.ResumeLayout(true);
            }
        }

        // Hàm hỗ trợ tạo CheckBox
        private void AddPermissionCheckBox(int value, string columnName, TableLayoutPanel tlp, int col, int row, int maQuyen)
        {
            CheckBox chk = new CheckBox();
            chk.Checked = value == 1;
            chk.Tag = $"{maQuyen}|{columnName}";

            chk.Anchor = AnchorStyles.None;
            chk.Cursor = Cursors.Hand;
            chk.Size = new Size(18, 18);

            tlp.Controls.Add(chk, col, row);
        }

        private void btnLuuPhanQuyen_Click(object sender, EventArgs e)
        {
            if (cmbVaiTro.SelectedValue == null || !(cmbVaiTro.SelectedValue is int))
            {
                MessageBox.Show("Vui lòng chọn một Vai trò hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maVaiTro = (int)cmbVaiTro.SelectedValue;
            string tenVaiTro = cmbVaiTro.Text;

            // Dictionary gom nhóm dữ liệu
            Dictionary<int, phanquyenDTO> dictQuyenMoi = new Dictionary<int, phanquyenDTO>();

            // Duyệt qua các Checkbox để lấy trạng thái tích/bỏ tích
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is CheckBox chk && chk.Tag != null)
                {
                    string[] tags = chk.Tag.ToString().Split('|');
                    int maQuyen = Convert.ToInt32(tags[0]);
                    string action = tags[1];

                    if (!dictQuyenMoi.ContainsKey(maQuyen))
                    {
                        dictQuyenMoi.Add(maQuyen, new phanquyenDTO { MaVaiTro = maVaiTro, MaQuyen = maQuyen });
                    }

                    int val = chk.Checked ? 1 : 0;
                    switch (action)
                    {
                        case "CAN_CREATE": dictQuyenMoi[maQuyen].CAN_CREATE = val; break;
                        case "CAN_READ": dictQuyenMoi[maQuyen].CAN_READ = val; break;
                        case "CAN_UPDATE": dictQuyenMoi[maQuyen].CAN_UPDATE = val; break;
                        case "CAN_DELETE": dictQuyenMoi[maQuyen].CAN_DELETE = val; break;
                    }
                }
            }

            // Gọi BUS lưu xuống DB
            phanquyenBUS pqBUS = new phanquyenBUS();
            BindingList<phanquyenDTO> dsLuu = new BindingList<phanquyenDTO>(dictQuyenMoi.Values.ToList());

            if (pqBUS.LuuPhanQuyen(maVaiTro, dsLuu))
            {
                MessageBox.Show($"Cập nhật thành công cho vai trò '{tenVaiTro}'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.Close();
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra khi cập nhật.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbVaiTro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVaiTro.SelectedValue != null && cmbVaiTro.SelectedValue is int)
            {
                int maVaiTro = (int)cmbVaiTro.SelectedValue;
                if (maVaiTro > 0)
                {
                    LoadPermissionsToUI(maVaiTro);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
