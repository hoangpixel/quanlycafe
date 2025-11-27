namespace GUI.GUI_CRUD
{
    partial class updatePhieuNhap
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelBody = new System.Windows.Forms.Panel();
            this.grpChiTiet = new System.Windows.Forms.GroupBox();
            this.dgvChiTiet = new System.Windows.Forms.DataGridView();
            this.grpThongTin = new System.Windows.Forms.GroupBox();
            this.btnChonNV = new System.Windows.Forms.Button();
            this.btnChonNCC = new System.Windows.Forms.Button();
            this.txtTongTien = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNhanVien = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNhaCungCap = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNgayNhap = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMaPN = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnXoaNL = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.grpChiTiet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).BeginInit();
            this.grpThongTin.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();

            this.panel1.Controls.Add(this.panelBody);
            this.panel1.Controls.Add(this.panelFooter);
            this.panel1.Controls.Add(this.panelHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 600);
            this.panel1.TabIndex = 0;

            this.panelBody.BackColor = System.Drawing.Color.White;
            this.panelBody.Controls.Add(this.grpChiTiet);
            this.panelBody.Controls.Add(this.grpThongTin);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 80);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(10);
            this.panelBody.Size = new System.Drawing.Size(1000, 440);
            this.panelBody.TabIndex = 2;

            this.grpChiTiet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpChiTiet.Controls.Add(this.dgvChiTiet);
            this.grpChiTiet.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold);
            this.grpChiTiet.Location = new System.Drawing.Point(450, 10);
            this.grpChiTiet.Name = "grpChiTiet";
            this.grpChiTiet.Size = new System.Drawing.Size(540, 420);
            this.grpChiTiet.TabIndex = 1;
            this.grpChiTiet.TabStop = false;
            this.grpChiTiet.Text = "Danh sách nguyên liệu nhập";

            this.dgvChiTiet.AllowUserToAddRows = false;
            this.dgvChiTiet.AllowUserToDeleteRows = false;
            this.dgvChiTiet.BackgroundColor = System.Drawing.Color.White;
            this.dgvChiTiet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChiTiet.Location = new System.Drawing.Point(3, 24);
            this.dgvChiTiet.MultiSelect = false;
            this.dgvChiTiet.Name = "dgvChiTiet";
            this.dgvChiTiet.ReadOnly = true;
            this.dgvChiTiet.RowHeadersVisible = false;
            this.dgvChiTiet.RowHeadersWidth = 51;
            this.dgvChiTiet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChiTiet.Size = new System.Drawing.Size(534, 393);
            this.dgvChiTiet.TabIndex = 0;

            this.grpThongTin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpThongTin.Controls.Add(this.btnChonNV);
            this.grpThongTin.Controls.Add(this.btnChonNCC);
            this.grpThongTin.Controls.Add(this.txtTongTien);
            this.grpThongTin.Controls.Add(this.label5);
            this.grpThongTin.Controls.Add(this.txtNhanVien);
            this.grpThongTin.Controls.Add(this.label4);
            this.grpThongTin.Controls.Add(this.txtNhaCungCap);
            this.grpThongTin.Controls.Add(this.label3);
            this.grpThongTin.Controls.Add(this.txtNgayNhap);
            this.grpThongTin.Controls.Add(this.label2);
            this.grpThongTin.Controls.Add(this.txtMaPN);
            this.grpThongTin.Controls.Add(this.label1);
            this.grpThongTin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold);
            this.grpThongTin.Location = new System.Drawing.Point(10, 10);
            this.grpThongTin.Name = "grpThongTin";
            this.grpThongTin.Size = new System.Drawing.Size(430, 420);
            this.grpThongTin.TabIndex = 0;
            this.grpThongTin.TabStop = false;
            this.grpThongTin.Text = "Thông tin chung";

            this.btnChonNV.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChonNV.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnChonNV.Location = new System.Drawing.Point(375, 208);
            this.btnChonNV.Name = "btnChonNV";
            this.btnChonNV.Size = new System.Drawing.Size(45, 30);
            this.btnChonNV.TabIndex = 11;
            this.btnChonNV.Text = "...";
            this.btnChonNV.UseVisualStyleBackColor = true;
            this.btnChonNV.Click += new System.EventHandler(this.btnChonNV_Click);

            this.btnChonNCC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChonNCC.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnChonNCC.Location = new System.Drawing.Point(375, 148);
            this.btnChonNCC.Name = "btnChonNCC";
            this.btnChonNCC.Size = new System.Drawing.Size(45, 30);
            this.btnChonNCC.TabIndex = 10;
            this.btnChonNCC.Text = "...";
            this.btnChonNCC.UseVisualStyleBackColor = true;
            this.btnChonNCC.Click += new System.EventHandler(this.btnChonNCC_Click);

            this.txtTongTien.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtTongTien.Enabled = false;
            this.txtTongTien.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.txtTongTien.ForeColor = System.Drawing.Color.Red;
            this.txtTongTien.Location = new System.Drawing.Point(150, 270);
            this.txtTongTien.Name = "txtTongTien";
            this.txtTongTien.Size = new System.Drawing.Size(220, 28);
            this.txtTongTien.TabIndex = 9;
            this.txtTongTien.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;

            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 273);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 22);
            this.label5.TabIndex = 8;
            this.label5.Text = "Tổng tiền:";

            this.txtNhanVien.BackColor = System.Drawing.Color.White;
            this.txtNhanVien.Enabled = false;
            this.txtNhanVien.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtNhanVien.Location = new System.Drawing.Point(150, 210);
            this.txtNhanVien.Name = "txtNhanVien";
            this.txtNhanVien.ReadOnly = true;
            this.txtNhanVien.Size = new System.Drawing.Size(220, 26);
            this.txtNhanVien.TabIndex = 7;

            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 213);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 22);
            this.label4.TabIndex = 6;
            this.label4.Text = "Nhân viên:";

            this.txtNhaCungCap.BackColor = System.Drawing.Color.White;
            this.txtNhaCungCap.Enabled = false;
            this.txtNhaCungCap.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtNhaCungCap.Location = new System.Drawing.Point(150, 150);
            this.txtNhaCungCap.Name = "txtNhaCungCap";
            this.txtNhaCungCap.ReadOnly = true;
            this.txtNhaCungCap.Size = new System.Drawing.Size(220, 26);
            this.txtNhaCungCap.TabIndex = 5;

            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 22);
            this.label3.TabIndex = 4;
            this.label3.Text = "Nhà CC:";

            this.txtNgayNhap.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtNgayNhap.Enabled = false;
            this.txtNgayNhap.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtNgayNhap.Location = new System.Drawing.Point(150, 95);
            this.txtNgayNhap.Name = "txtNgayNhap";
            this.txtNgayNhap.Size = new System.Drawing.Size(220, 26);
            this.txtNgayNhap.TabIndex = 3;

            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ngày nhập:";

            this.txtMaPN.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtMaPN.Enabled = false;
            this.txtMaPN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtMaPN.Location = new System.Drawing.Point(150, 45);
            this.txtMaPN.Name = "txtMaPN";
            this.txtMaPN.Size = new System.Drawing.Size(220, 26);
            this.txtMaPN.TabIndex = 1;

            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã phiếu:";

            this.panelFooter.Controls.Add(this.btnLuu);
            this.panelFooter.Controls.Add(this.btnXoaNL);
            this.panelFooter.Controls.Add(this.btnHuy);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 520);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1000, 80);
            this.panelFooter.TabIndex = 3;

            this.btnLuu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLuu.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnLuu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(690, 20);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(130, 40);
            this.btnLuu.TabIndex = 2;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);

            this.btnXoaNL.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnXoaNL.BackColor = System.Drawing.Color.OrangeRed;
            this.btnXoaNL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoaNL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaNL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnXoaNL.ForeColor = System.Drawing.Color.White;
            this.btnXoaNL.Location = new System.Drawing.Point(34, 20);
            this.btnXoaNL.Name = "btnXoaNL";
            this.btnXoaNL.Size = new System.Drawing.Size(180, 40);
            this.btnXoaNL.TabIndex = 1;
            this.btnXoaNL.Text = "Xóa NL";
            this.btnXoaNL.UseVisualStyleBackColor = false;
            this.btnXoaNL.Click += new System.EventHandler(this.btnXoaNL_Click);

            this.btnHuy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHuy.BackColor = System.Drawing.Color.Gray;
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(840, 20);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(130, 40);
            this.btnHuy.TabIndex = 0;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);

            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panelHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1000, 80);
            this.panelHeader.TabIndex = 0;

            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(998, 78);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "CẬP NHẬT PHIẾU NHẬP";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.panel1);
            this.Name = "updatePhieuNhap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cập Nhật Phiếu Nhập";
            this.Load += new System.EventHandler(this.updatePhieuNhap_Load);
            this.panel1.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.grpChiTiet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).EndInit();
            this.grpThongTin.ResumeLayout(false);
            this.grpThongTin.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.GroupBox grpChiTiet;
        private System.Windows.Forms.GroupBox grpThongTin;
        private System.Windows.Forms.DataGridView dgvChiTiet;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMaPN;
        private System.Windows.Forms.TextBox txtTongTien;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtNhanVien;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNhaCungCap;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNgayNhap;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChonNV;
        private System.Windows.Forms.Button btnChonNCC;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnXoaNL;
    }
}