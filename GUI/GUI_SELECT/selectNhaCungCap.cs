using BUS;
using DTO;
using System;
using System.Collections.Generic;
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

           
            dgvNCC.CellDoubleClick += (s, e) => XulyChon();
        }

        private void selectNhaCungCap_Load(object sender, EventArgs e)
        {
            if (cboLoaiTimKiem.Items.Count > 0)
                cboLoaiTimKiem.SelectedIndex = 0;

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var dataTuBus = bus.LayDanhSach();
                listGoc = dataTuBus.ToList();

                dgvNCC.DataSource = listGoc;
                dgvNCC.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
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
                else
                {
                    ketQua = listGoc.Where(x => x.TenNCC.ToLower().Contains(keyword)).ToList();
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
            XulyTimKiem();
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
    }
}