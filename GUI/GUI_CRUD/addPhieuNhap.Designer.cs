namespace GUI.GUI_CRUD
{
    partial class addPhieuNhap
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();

            this.panel1 = new System.Windows.Forms.Panel();
            this.panelBody = new System.Windows.Forms.Panel();

            // Các GroupBox
            this.grpThongTin = new System.Windows.Forms.GroupBox();
            this.grpNhapLieu = new System.Windows.Forms.GroupBox();
            this.grpChiTiet = new System.Windows.Forms.GroupBox();

            // Controls Thông tin
            this.label2 = new System.Windows.Forms.Label(); this.txtNgayNhap = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label(); this.txtNhaCungCap = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label(); this.txtNhanVien = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label(); this.txtTongTien = new System.Windows.Forms.TextBox();

            // Controls Nhập liệu
            this.label8 = new System.Windows.Forms.Label(); this.cboHang = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label(); this.cboDonVi = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label(); this.numSL = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label(); this.txtDonGia = new System.Windows.Forms.TextBox();
            this.btnThem = new System.Windows.Forms.Button();

            // Grid
            this.dgvChiTiet = new System.Windows.Forms.DataGridView();

            // Footer & Header
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();

            // Nút ẩn
            this.btnChonNCC = new System.Windows.Forms.Button();
            this.btnChonNV = new System.Windows.Forms.Button();

            this.panel1.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.grpThongTin.SuspendLayout();
            this.grpNhapLieu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSL)).BeginInit();
            this.grpChiTiet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).BeginInit();
            this.panelFooter.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();

            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelBody);
            this.panel1.Controls.Add(this.panelFooter);
            this.panel1.Controls.Add(this.panelHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Size = new System.Drawing.Size(1100, 650);

            // 
            // panelBody
            // 
            this.panelBody.BackColor = System.Drawing.Color.White;
            this.panelBody.Controls.Add(this.grpThongTin);
            this.panelBody.Controls.Add(this.grpNhapLieu);
            this.panelBody.Controls.Add(this.grpChiTiet);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 60);
            this.panelBody.Padding = new System.Windows.Forms.Padding(10);
            this.panelBody.Size = new System.Drawing.Size(1100, 530);

            // 
            // GROUP THÔNG TIN (TRÁI)
            // 
            this.grpThongTin.Location = new System.Drawing.Point(15, 15);
            this.grpThongTin.Size = new System.Drawing.Size(350, 500);
            this.grpThongTin.Text = "Thông tin chung";
            this.grpThongTin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpThongTin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));

            this.label2.Location = new System.Drawing.Point(15, 40); this.label2.Text = "Ngày nhập:"; this.label2.AutoSize = true;
            this.txtNgayNhap.Location = new System.Drawing.Point(15, 65); this.txtNgayNhap.Size = new System.Drawing.Size(315, 25); this.txtNgayNhap.Enabled = false;

            this.label3.Location = new System.Drawing.Point(15, 110); this.label3.Text = "Nhà cung cấp:"; this.label3.AutoSize = true;
            this.txtNhaCungCap.Location = new System.Drawing.Point(15, 135); this.txtNhaCungCap.Size = new System.Drawing.Size(315, 25); this.txtNhaCungCap.Enabled = false;

            this.label4.Location = new System.Drawing.Point(15, 180); this.label4.Text = "Nhân viên:"; this.label4.AutoSize = true;
            this.txtNhanVien.Location = new System.Drawing.Point(15, 205); this.txtNhanVien.Size = new System.Drawing.Size(315, 25); this.txtNhanVien.Enabled = false;

            this.label5.Location = new System.Drawing.Point(15, 280); this.label5.Text = "Tổng tiền:"; this.label5.AutoSize = true;
            this.txtTongTien.Location = new System.Drawing.Point(15, 305); this.txtTongTien.Size = new System.Drawing.Size(315, 35);
            this.txtTongTien.Enabled = false; this.txtTongTien.ForeColor = System.Drawing.Color.Red;
            this.txtTongTien.TextAlign = System.Windows.Forms.HorizontalAlignment.Right; this.txtTongTien.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);

            this.grpThongTin.Controls.Add(this.label2); this.grpThongTin.Controls.Add(this.txtNgayNhap);
            this.grpThongTin.Controls.Add(this.label3); this.grpThongTin.Controls.Add(this.txtNhaCungCap);
            this.grpThongTin.Controls.Add(this.label4); this.grpThongTin.Controls.Add(this.txtNhanVien);
            this.grpThongTin.Controls.Add(this.label5); this.grpThongTin.Controls.Add(this.txtTongTien);

            // 
            // GROUP NHẬP LIỆU (PHẢI TRÊN)
            // 
            this.grpNhapLieu.Location = new System.Drawing.Point(380, 15);
            this.grpNhapLieu.Size = new System.Drawing.Size(700, 130);
            this.grpNhapLieu.Text = "Chọn nguyên liệu";
            this.grpNhapLieu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpNhapLieu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));

            // Row 1: Tên Hàng & Đơn Vị
            this.label8.Location = new System.Drawing.Point(20, 35); this.label8.Text = "Tên NL:"; this.label8.AutoSize = true;
            this.cboHang.Location = new System.Drawing.Point(90, 32); this.cboHang.Size = new System.Drawing.Size(250, 30);
            this.cboHang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHang.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.label9.Location = new System.Drawing.Point(360, 35); this.label9.Text = "ĐVT:"; this.label9.AutoSize = true;
            this.cboDonVi.Location = new System.Drawing.Point(410, 32); this.cboDonVi.Size = new System.Drawing.Size(120, 30);
            this.cboDonVi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDonVi.Font = new System.Drawing.Font("Segoe UI", 10F);

            // Row 2: Số Lượng & Đơn Giá
            this.label7.Location = new System.Drawing.Point(20, 85); this.label7.Text = "Số lượng:"; this.label7.AutoSize = true;

            this.numSL.Location = new System.Drawing.Point(90, 82);
            this.numSL.Size = new System.Drawing.Size(80, 30);
            this.numSL.Value = 1;
            this.numSL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numSL.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold); // Số to rõ

            this.label6.Location = new System.Drawing.Point(200, 85); this.label6.Text = "Đơn giá:"; this.label6.AutoSize = true;

            // --- CHỈNH SỬA Ô ĐƠN GIÁ ---
            this.txtDonGia.Location = new System.Drawing.Point(270, 80);
            this.txtDonGia.Size = new System.Drawing.Size(260, 32);
            this.txtDonGia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDonGia.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.txtDonGia.BackColor = System.Drawing.Color.WhiteSmoke;
            // -----------------------------------

            // Button Thêm
            this.btnThem.Location = new System.Drawing.Point(550, 25);
            this.btnThem.Size = new System.Drawing.Size(130, 90); // Nút to hơn
            this.btnThem.Text = "THÊM";
            this.btnThem.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);

            this.grpNhapLieu.Controls.Add(this.label8); this.grpNhapLieu.Controls.Add(this.cboHang);
            this.grpNhapLieu.Controls.Add(this.label9); this.grpNhapLieu.Controls.Add(this.cboDonVi);
            this.grpNhapLieu.Controls.Add(this.btnThem);
            this.grpNhapLieu.Controls.Add(this.label7); this.grpNhapLieu.Controls.Add(this.numSL);
            this.grpNhapLieu.Controls.Add(this.label6); this.grpNhapLieu.Controls.Add(this.txtDonGia);

            // 
            // GROUP CHI TIẾT (PHẢI DƯỚI)
            // 
            this.grpChiTiet.Location = new System.Drawing.Point(380, 155);
            this.grpChiTiet.Size = new System.Drawing.Size(700, 360);
            this.grpChiTiet.Text = "Danh sách hàng đã chọn";
            this.grpChiTiet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpChiTiet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));

            this.dgvChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChiTiet.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvChiTiet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChiTiet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChiTiet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChiTiet.AllowUserToAddRows = false;
            this.dgvChiTiet.ReadOnly = true;
            this.dgvChiTiet.RowHeadersVisible = false;

            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(235, 245, 255);
            this.dgvChiTiet.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvChiTiet.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
            this.dgvChiTiet.EnableHeadersVisualStyles = false;

            this.grpChiTiet.Controls.Add(this.dgvChiTiet);

            // FOOTER
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom; this.panelFooter.Size = new System.Drawing.Size(1100, 60);
            this.panelFooter.BackColor = System.Drawing.Color.WhiteSmoke;

            this.btnLuu.Location = new System.Drawing.Point(800, 10); this.btnLuu.Size = new System.Drawing.Size(130, 40);
            this.btnLuu.Text = "LƯU PHIẾU"; this.btnLuu.BackColor = System.Drawing.Color.ForestGreen; this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold); this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);

            this.btnHuy.Location = new System.Drawing.Point(950, 10); this.btnHuy.Size = new System.Drawing.Size(130, 40);
            this.btnHuy.Text = "THOÁT"; this.btnHuy.BackColor = System.Drawing.Color.IndianRed; this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold); this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);

            this.panelFooter.Controls.Add(this.btnLuu); this.panelFooter.Controls.Add(this.btnHuy);

            // HEADER
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top; this.panelHeader.Size = new System.Drawing.Size(1100, 60);
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(192, 192, 255);

            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Text = "BỔ SUNG NGUYÊN LIỆU";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);

            this.panelHeader.Controls.Add(this.lblTitle);

            // FORM SETUP
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.Controls.Add(this.panel1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bổ Sung Nguyên Liệu";
            this.Load += new System.EventHandler(this.addPhieuNhap_Load);

            this.panel1.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.grpThongTin.ResumeLayout(false);
            this.grpThongTin.PerformLayout();
            this.grpNhapLieu.ResumeLayout(false);
            this.grpNhapLieu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSL)).EndInit();
            this.grpChiTiet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).EndInit();
            this.panelFooter.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion 

        private System.Windows.Forms.Panel panel1, panelHeader, panelBody, panelFooter;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpThongTin, grpNhapLieu, grpChiTiet;
        private System.Windows.Forms.DataGridView dgvChiTiet;
        private System.Windows.Forms.Button btnLuu, btnHuy, btnThem;
        private System.Windows.Forms.Label label2, label3, label4, label5, label6, label7, label8, label9;
        private System.Windows.Forms.TextBox txtNgayNhap, txtNhaCungCap, txtNhanVien, txtTongTien, txtDonGia;
        private System.Windows.Forms.ComboBox cboHang, cboDonVi;
        private System.Windows.Forms.NumericUpDown numSL;
        private System.Windows.Forms.Button btnChonNCC, btnChonNV;
    }
}