namespace GUI.GUI_CRUD
{
    partial class insertPhanQuyen
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
            this.pnlPhanQuyen = new System.Windows.Forms.Panel();
            this.gbPhanQuyen = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnThoat = new System.Windows.Forms.Button();
            this.cmbVaiTro = new System.Windows.Forms.ComboBox();
            this.btnLuuPhanQuyen = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bigLabel1 = new ReaLTaiizor.Controls.BigLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pnlPhanQuyen.SuspendLayout();
            this.gbPhanQuyen.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPhanQuyen
            // 
            this.pnlPhanQuyen.AutoScroll = true;
            this.pnlPhanQuyen.Controls.Add(this.panel2);
            this.pnlPhanQuyen.Controls.Add(this.gbPhanQuyen);
            this.pnlPhanQuyen.Controls.Add(this.panel1);
            this.pnlPhanQuyen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPhanQuyen.Location = new System.Drawing.Point(0, 0);
            this.pnlPhanQuyen.Name = "pnlPhanQuyen";
            this.pnlPhanQuyen.Padding = new System.Windows.Forms.Padding(10);
            this.pnlPhanQuyen.Size = new System.Drawing.Size(759, 477);
            this.pnlPhanQuyen.TabIndex = 0;
            // 
            // gbPhanQuyen
            // 
            this.gbPhanQuyen.AutoSize = true;
            this.gbPhanQuyen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbPhanQuyen.Controls.Add(this.tableLayoutPanel1);
            this.gbPhanQuyen.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbPhanQuyen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPhanQuyen.Location = new System.Drawing.Point(10, 110);
            this.gbPhanQuyen.Name = "gbPhanQuyen";
            this.gbPhanQuyen.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.gbPhanQuyen.Size = new System.Drawing.Size(739, 73);
            this.gbPhanQuyen.TabIndex = 1;
            this.gbPhanQuyen.TabStop = false;
            this.gbPhanQuyen.Text = "Chọn phân quyền";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 30);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(733, 40);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.Controls.Add(this.cmbVaiTro);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 94);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chọn vai trò";
            // 
            // btnThoat
            // 
            this.btnThoat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThoat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnThoat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThoat.Location = new System.Drawing.Point(194, 32);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(131, 45);
            this.btnThoat.TabIndex = 3;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = false;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // cmbVaiTro
            // 
            this.cmbVaiTro.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmbVaiTro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVaiTro.FormattingEnabled = true;
            this.cmbVaiTro.Location = new System.Drawing.Point(84, 41);
            this.cmbVaiTro.Name = "cmbVaiTro";
            this.cmbVaiTro.Size = new System.Drawing.Size(173, 28);
            this.cmbVaiTro.TabIndex = 3;
            this.cmbVaiTro.SelectedIndexChanged += new System.EventHandler(this.cmbVaiTro_SelectedIndexChanged);
            // 
            // btnLuuPhanQuyen
            // 
            this.btnLuuPhanQuyen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLuuPhanQuyen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLuuPhanQuyen.Location = new System.Drawing.Point(45, 32);
            this.btnLuuPhanQuyen.Name = "btnLuuPhanQuyen";
            this.btnLuuPhanQuyen.Size = new System.Drawing.Size(131, 45);
            this.btnLuuPhanQuyen.TabIndex = 2;
            this.btnLuuPhanQuyen.Text = "Lưu";
            this.btnLuuPhanQuyen.UseVisualStyleBackColor = true;
            this.btnLuuPhanQuyen.Click += new System.EventHandler(this.btnLuuPhanQuyen_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.bigLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(739, 100);
            this.panel1.TabIndex = 2;
            // 
            // bigLabel1
            // 
            this.bigLabel1.AutoSize = true;
            this.bigLabel1.BackColor = System.Drawing.Color.Transparent;
            this.bigLabel1.Font = new System.Drawing.Font("Segoe UI", 25.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bigLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.bigLabel1.Location = new System.Drawing.Point(163, 16);
            this.bigLabel1.Name = "bigLabel1";
            this.bigLabel1.Size = new System.Drawing.Size(373, 57);
            this.bigLabel1.TabIndex = 0;
            this.bigLabel1.Text = "Xử lý phân quyền";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(10, 183);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(739, 100);
            this.panel2.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(739, 100);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.btnThoat);
            this.groupBox2.Controls.Add(this.btnLuuPhanQuyen);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(372, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(364, 94);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Xử lý chức năng";
            // 
            // insertPhanQuyen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 477);
            this.Controls.Add(this.pnlPhanQuyen);
            this.Name = "insertPhanQuyen";
            this.Text = "Thêm phân quyền";
            this.Load += new System.EventHandler(this.insertPhanQuyen_Load);
            this.pnlPhanQuyen.ResumeLayout(false);
            this.pnlPhanQuyen.PerformLayout();
            this.gbPhanQuyen.ResumeLayout(false);
            this.gbPhanQuyen.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlPhanQuyen;
        private System.Windows.Forms.GroupBox gbPhanQuyen;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnLuuPhanQuyen;
        private System.Windows.Forms.ComboBox cmbVaiTro;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.Panel panel1;
        private ReaLTaiizor.Controls.BigLabel bigLabel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}