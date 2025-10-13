﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using quanlycafe.DTO;
using quanlycafe.BUS;

namespace quanlycafe.GUI_CRUD
{
    public partial class updateNguyenLieu : Form
    {
        private nguyenLieuDTO ct;
        public updateNguyenLieu()
        {
            InitializeComponent();
        }
        public updateNguyenLieu(nguyenLieuDTO ct)
        {
            InitializeComponent();
            this.ct = ct;
            this.Shown += updateNguyenLieu_Shown;
        }

        private void btnSuaNL_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenNL.Text) || cbDonVi.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập tên và chọn đơn vị nguyên liệu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ct.TenNguyenLieu = txtTenNL.Text.Trim();
                ct.DonViCoSo = cbDonVi.Text.Trim();
                ct.TrangThai = 1; 

                nguyenLieuBUS bus = new nguyenLieuBUS();
                bool kq = bus.suaNguyenLieu(ct);

                if (kq)
                {
                    MessageBox.Show("Cập nhật nguyên liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); 
                }
                else
                {
                    MessageBox.Show("Lỗi khi cập nhật nguyên liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void updateNguyenLieu_Load(object sender, EventArgs e)
        {
            if (ct != null)
            {
                txtTenNL.Text = ct.TenNguyenLieu;
                cbDonVi.Text = ct.DonViCoSo;
                txtTenNL.Focus();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void updateNguyenLieu_Shown(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }
    }
}
