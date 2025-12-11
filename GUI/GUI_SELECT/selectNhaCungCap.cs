using BUS;
using DTO;
using FONTS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace GUI.GUI_SELECT
{
    public partial class selectNhaCungCap : Form
    {
        public int MaNCC { get; private set; }
        public string TenNCC { get; private set; }

        private nhaCungCapBUS bus = new nhaCungCapBUS();
        private List<nhaCungCapDTO> listGoc;

        public selectNhaCungCap()
        {
            InitializeComponent();

           
            dgvNCC.CellClick += (s, e) => XulyChon();
        }

        private void selectNhaCungCap_Load(object sender, EventArgs e)
        {
            FontManager.LoadFont();
            FontManager.ApplyFontToAllControls(this);
            if (cboLoaiTimKiem.Items.Count > 0)
                cboLoaiTimKiem.SelectedIndex = 0;

            LoadData();
            loadFontChuVaSizeTableDonVi();
        }
        private void loadFontChuVaSizeTableDonVi()
        {
            foreach (DataGridViewColumn col in dgvNCC.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            dgvNCC.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvNCC.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvNCC.DefaultCellStyle.Font = FontManager.GetLightFont(10);

            dgvNCC.ColumnHeadersDefaultCellStyle.Font = FontManager.GetBoldFont(10);

            dgvNCC.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvNCC.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvNCC.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgvNCC.Refresh();
        }
        private void LoadData()
        {
            BindingList<nhaCungCapDTO> ds = new nhaCungCapBUS().LayDanhSach();
            listGoc = ds.ToList();
            dgvNCC.AutoGenerateColumns = false;
            dgvNCC.Columns.Clear();

            dgvNCC.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaNCC", HeaderText = "Mã NCC" });
            dgvNCC.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TenNCC", HeaderText = "Tên NCC" });
            dgvNCC.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "SoDienThoai", HeaderText = "Số điện thoại" });
            dgvNCC.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Email", HeaderText = "Email" });
            dgvNCC.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DiaChi", HeaderText = "Địa chỉ" });

            dgvNCC.DataSource = ds;
            dgvNCC.ReadOnly = true;
            dgvNCC.ClearSelection();
        }

        private void XulyTimKiem()
        {
            if (listGoc == null) return;

            string keyword = txtTim.Text.ToLower().Trim();
            string tieuChi = cboLoaiTimKiem.Text;

            List<nhaCungCapDTO> ketQua = new List<nhaCungCapDTO>();

            if (string.IsNullOrEmpty(keyword))
            {
                ketQua = listGoc;
            }
            else
            {
                if (tieuChi == "Mã NCC")
                {
                    ketQua = listGoc.Where(x => x.MaNCC.ToString().Contains(keyword)).ToList();
                }
                else if(tieuChi == "Tên NCC")
                {
                    ketQua = listGoc.Where(x => x.TenNCC.ToLower().Contains(keyword)).ToList();
                }
                else if(tieuChi == "SĐT")
                {
                    ketQua = listGoc.Where(x => x.SoDienThoai.ToLower().Contains(keyword)).ToList();
                }
                else if (tieuChi == "Email")
                {
                    ketQua = listGoc.Where(x => x.Email.ToLower().Contains(keyword)).ToList();
                }
                else
                {
                    ketQua = listGoc.Where(x => x.DiaChi.ToLower().Contains(keyword)).ToList();
                }
            }

            dgvNCC.DataSource = ketQua;
        }


        private void XulyChon()
        {
            if (dgvNCC.CurrentRow != null)
            {
                var item = dgvNCC.CurrentRow.DataBoundItem as nhaCungCapDTO;
                if (item != null)
                {
                
                    if (item.ConHoatDong == 0)
                    {
                        MessageBox.Show(
                            $"Nhà cung cấp '{item.TenNCC}' đã ngừng hoạt động.\nVui lòng chọn nhà cung cấp khác!",
                            "Cảnh báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }

                
                    MaNCC = item.MaNCC;
                    TenNCC = item.TenNCC;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp!");
            }
        }

       

        private void txtTim_TextChanged(object sender, EventArgs e)
        {
            //XulyTimKiem();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            XulyTimKiem();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtTim.Text = "";
            LoadData();
        }

        private void btnChon_Click(object sender, EventArgs e)
        {
            XulyChon();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvNCC_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvNCC.ClearSelection();
        }
    }
}