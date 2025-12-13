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
using System.Reflection;
using System.Windows.Forms;

namespace GUI.GUI_CRUD
{
    public partial class insertPhanQuyen : Form
    {
        private bool isLoading = false;
        private BindingList<phanquyenDTO> _cachedList;
        public insertPhanQuyen()
        {
            InitializeComponent();
        }

        private void insertPhanQuyen_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.DoubleBuffered(true);
            pnlPhanQuyen.DoubleBuffered(true);

            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);

            LoadVaiTroToComboBox();
        }

        private void LoadVaiTroToComboBox()
        {
            isLoading = true;
            vaitroBUS vtBUS = new vaitroBUS();
            BindingList<vaitroDTO> dsVaiTro = vtBUS.LayDanhSach();

            cmbVaiTro.DataSource = dsVaiTro;
            cmbVaiTro.DisplayMember = "TenVaiTro";
            cmbVaiTro.ValueMember = "MaVaiTro";

            int indexAdmin = cmbVaiTro.FindStringExact("Admin");
            cmbVaiTro.SelectedIndex = indexAdmin >= 0 ? indexAdmin : 0;
            isLoading = false;

            if (cmbVaiTro.SelectedValue is int maVaiTro)
            {
                LoadPermissionsToUI(maVaiTro);
            }
        }

        private void SetupTableLayoutPanelHeader()
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.ColumnCount = 6;

            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 13F));

            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.BackColor = Color.White;

            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            Color headerColor = Color.FromArgb(64, 64, 64);
            Font headerFont = new Font("Segoe UI", 10F, FontStyle.Bold);

            tableLayoutPanel1.Controls.Add(new Label()
            {
                Text = "Tên Quyền / Chức năng",
                ForeColor = headerColor,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 0, 0, 0),
                Font = headerFont
            }, 0, 0);

            string[] crudHeaders = { "CREATE", "READ", "UPDATE", "DELETE" };
            for (int i = 0; i < crudHeaders.Length; i++)
            {
                tableLayoutPanel1.Controls.Add(new Label()
                {
                    Text = crudHeaders[i],
                    ForeColor = headerColor,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = headerFont
                }, i + 1, 0);
            }

            tableLayoutPanel1.Controls.Add(new Label()
            {
                Text = "TẤT CẢ",
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = headerFont
            }, 5, 0);
        }

        private void LoadPermissionsToUI(int maVaiTro)
        {
            this.SuspendLayout();
            pnlPhanQuyen.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();

            try
            {
                SetupTableLayoutPanelHeader();

                phanquyenBUS pqBUS = new phanquyenBUS();
                _cachedList = pqBUS.LayChiTietQuyenTheoVaiTro(maVaiTro);

                int row = 1;

                foreach (phanquyenDTO item in _cachedList)
                {
                    if (maVaiTro != 1 && item.TenQuyen.Trim().Equals("Quản lý phân quyền", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    tableLayoutPanel1.RowCount++;
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

                    int maQuyen = item.MaQuyen;

                    Label lblQuyen = new Label();
                    lblQuyen.Text = item.TenQuyen;
                    lblQuyen.AutoSize = true;
                    lblQuyen.Dock = DockStyle.Fill;
                    lblQuyen.TextAlign = ContentAlignment.MiddleLeft;
                    lblQuyen.Padding = new Padding(10, 0, 0, 0);
                    tableLayoutPanel1.Controls.Add(lblQuyen, 0, row);

                    AddPermissionCheckBox(item.CAN_CREATE, "CAN_CREATE", tableLayoutPanel1, 1, row, maQuyen);
                    AddPermissionCheckBox(item.CAN_READ, "CAN_READ", tableLayoutPanel1, 2, row, maQuyen);
                    AddPermissionCheckBox(item.CAN_UPDATE, "CAN_UPDATE", tableLayoutPanel1, 3, row, maQuyen);
                    AddPermissionCheckBox(item.CAN_DELETE, "CAN_DELETE", tableLayoutPanel1, 4, row, maQuyen);

                    CheckBox chkAllRow = new CheckBox();
                    chkAllRow.Anchor = AnchorStyles.None;
                    chkAllRow.Cursor = Cursors.Hand;
                    chkAllRow.Size = new Size(18, 18);
                    chkAllRow.Tag = $"CMD_ALL_ROW|{row}";
                    chkAllRow.Click += ChkAllRow_Click;

                    bool isAllChecked = (item.CAN_CREATE == 1 && item.CAN_READ == 1 && item.CAN_UPDATE == 1 && item.CAN_DELETE == 1);
                    chkAllRow.Checked = isAllChecked;

                    tableLayoutPanel1.Controls.Add(chkAllRow, 5, row);

                    row++;
                }

                AddFooterControls(row);
            }
            finally
            {
                tableLayoutPanel1.ResumeLayout(true);
                pnlPhanQuyen.ResumeLayout(true);
                this.ResumeLayout(true);
            }
        }

        private void AddFooterControls(int lastRow)
        {
            tableLayoutPanel1.RowCount++;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));

            Label lblAllCols = new Label()
            {
                Text = "CHỌN TẤT CẢ CỘT",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                ForeColor = Color.Blue,
            };
            tableLayoutPanel1.Controls.Add(lblAllCols, 0, lastRow);

            string[] colActions = { "CAN_CREATE", "CAN_READ", "CAN_UPDATE", "CAN_DELETE" };
            for (int i = 0; i < 4; i++)
            {
                CheckBox chkAllCol = new CheckBox();
                chkAllCol.Anchor = AnchorStyles.None;
                chkAllCol.Cursor = Cursors.Hand;
                chkAllCol.Size = new Size(18, 18);
                chkAllCol.Tag = $"CMD_ALL_COL|{i + 1}";
                chkAllCol.Click += ChkAllCol_Click;
                tableLayoutPanel1.Controls.Add(chkAllCol, i + 1, lastRow);
            }

            CheckBox chkMaster = new CheckBox();
            chkMaster.Anchor = AnchorStyles.None;
            chkMaster.Size = new Size(18, 18);
            chkMaster.Tag = "CMD_MASTER_ALL";
            chkMaster.Click += (s, e) => { ToggleAllInGrid(((CheckBox)s).Checked); };
            tableLayoutPanel1.Controls.Add(chkMaster, 5, lastRow);
        }

        private void ChkAllRow_Click(object sender, EventArgs e)
        {
            CheckBox chkAll = sender as CheckBox;
            if (chkAll == null) return;
            string[] parts = chkAll.Tag.ToString().Split('|');
            int targetRow = int.Parse(parts[1]);
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                if (c is CheckBox chkItem && tableLayoutPanel1.GetRow(c) == targetRow && c != chkAll)
                    chkItem.Checked = chkAll.Checked;
            }
        }

        private void ChkAllCol_Click(object sender, EventArgs e)
        {
            CheckBox chkAll = sender as CheckBox;
            if (chkAll == null) return;
            string[] parts = chkAll.Tag.ToString().Split('|');
            int targetCol = int.Parse(parts[1]);
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                int r = tableLayoutPanel1.GetRow(c);
                if (c is CheckBox chkItem && tableLayoutPanel1.GetColumn(c) == targetCol && r > 0 && r < tableLayoutPanel1.RowCount - 1)
                    chkItem.Checked = chkAll.Checked;
            }
        }

        private void ToggleAllInGrid(bool isChecked)
        {
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                if (c is CheckBox chk) chk.Checked = isChecked;
            }
        }

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
                MessageBox.Show("Vui lòng chọn một Vai trò hợp lệ.");
                return;
            }
            if (_cachedList == null) return;

            int maVaiTro = (int)cmbVaiTro.SelectedValue;

            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is CheckBox chk && chk.Tag != null)
                {
                    string tagStr = chk.Tag.ToString();
                    if (tagStr.StartsWith("CMD_")) continue;

                    string[] tags = tagStr.Split('|');
                    if (tags.Length < 2) continue;
                    if (!int.TryParse(tags[0], out int maQuyen)) continue;

                    string action = tags[1];
                    int val = chk.Checked ? 1 : 0;

                    var item = _cachedList.FirstOrDefault(x => x.MaQuyen == maQuyen);
                    if (item != null)
                    {
                        switch (action)
                        {
                            case "CAN_CREATE": item.CAN_CREATE = val; break;
                            case "CAN_READ": item.CAN_READ = val; break;
                            case "CAN_UPDATE": item.CAN_UPDATE = val; break;
                            case "CAN_DELETE": item.CAN_DELETE = val; break;
                        }
                    }
                }
            }

            phanquyenBUS pqBUS = new phanquyenBUS();
            if (pqBUS.LuuPhanQuyen(maVaiTro, _cachedList))
            {
                MessageBox.Show("Cập nhật thành công!");
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra.");
            }
        }

        private void cmbVaiTro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            if (cmbVaiTro.SelectedValue is int maVaiTro && maVaiTro > 0)
                LoadPermissionsToUI(maVaiTro);
        }


        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    public static class ControlExtensions
    {
        public static void DoubleBuffered(this Control control, bool enable)
        {
            var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            doubleBufferPropertyInfo?.SetValue(control, enable, null);
        }
    }
}
