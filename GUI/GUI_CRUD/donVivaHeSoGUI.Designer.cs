namespace GUI.GUI_CRUD
{
    partial class donVivaHeSoGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dungeonLabel1 = new ReaLTaiizor.Controls.DungeonLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableDonVi = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnXoaDonVi = new System.Windows.Forms.Button();
            this.btnSuaDonVi = new System.Windows.Forms.Button();
            this.btnThemDonVI = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDonVi = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dungeonLabel2 = new ReaLTaiizor.Controls.DungeonLabel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableHeSo = new System.Windows.Forms.DataGridView();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnXoaHeSO = new System.Windows.Forms.Button();
            this.btnSuaHS = new System.Windows.Forms.Button();
            this.btnThemHs = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnSanPham = new System.Windows.Forms.Button();
            this.txtHeSo = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.cboDonVi = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTenNL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableDonVi)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeSo)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeSo)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(714, 413);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(706, 384);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Đơn vị";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.dungeonLabel1);
            this.panel2.Location = new System.Drawing.Point(6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(694, 97);
            this.panel2.TabIndex = 10;
            // 
            // dungeonLabel1
            // 
            this.dungeonLabel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dungeonLabel1.AutoSize = true;
            this.dungeonLabel1.BackColor = System.Drawing.Color.Transparent;
            this.dungeonLabel1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dungeonLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(77)))));
            this.dungeonLabel1.Location = new System.Drawing.Point(276, 31);
            this.dungeonLabel1.Name = "dungeonLabel1";
            this.dungeonLabel1.Size = new System.Drawing.Size(164, 38);
            this.dungeonLabel1.TabIndex = 0;
            this.dungeonLabel1.Text = "Xử lý đơn vị";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(694, 368);
            this.panel1.TabIndex = 9;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableDonVi);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(333, 102);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(341, 246);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Danh sách đơn vị";
            // 
            // tableDonVi
            // 
            this.tableDonVi.AllowUserToAddRows = false;
            this.tableDonVi.AllowUserToDeleteRows = false;
            this.tableDonVi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableDonVi.Location = new System.Drawing.Point(6, 23);
            this.tableDonVi.MultiSelect = false;
            this.tableDonVi.Name = "tableDonVi";
            this.tableDonVi.ReadOnly = true;
            this.tableDonVi.RowHeadersVisible = false;
            this.tableDonVi.RowHeadersWidth = 51;
            this.tableDonVi.RowTemplate.Height = 24;
            this.tableDonVi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tableDonVi.Size = new System.Drawing.Size(329, 217);
            this.tableDonVi.TabIndex = 0;
            this.tableDonVi.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tableDonVi_CellClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnXoaDonVi);
            this.groupBox2.Controls.Add(this.btnSuaDonVi);
            this.groupBox2.Controls.Add(this.btnThemDonVI);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(18, 248);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 100);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Xử lý chức năng";
            // 
            // btnXoaDonVi
            // 
            this.btnXoaDonVi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoaDonVi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoaDonVi.Location = new System.Drawing.Point(208, 41);
            this.btnXoaDonVi.Name = "btnXoaDonVi";
            this.btnXoaDonVi.Size = new System.Drawing.Size(84, 34);
            this.btnXoaDonVi.TabIndex = 2;
            this.btnXoaDonVi.Text = "Xóa";
            this.btnXoaDonVi.UseVisualStyleBackColor = true;
            this.btnXoaDonVi.Click += new System.EventHandler(this.btnXoaDonVi_Click);
            // 
            // btnSuaDonVi
            // 
            this.btnSuaDonVi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSuaDonVi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSuaDonVi.Location = new System.Drawing.Point(108, 41);
            this.btnSuaDonVi.Name = "btnSuaDonVi";
            this.btnSuaDonVi.Size = new System.Drawing.Size(84, 34);
            this.btnSuaDonVi.TabIndex = 1;
            this.btnSuaDonVi.Text = "Sửa";
            this.btnSuaDonVi.UseVisualStyleBackColor = true;
            this.btnSuaDonVi.Click += new System.EventHandler(this.btnSuaDonVi_Click);
            // 
            // btnThemDonVI
            // 
            this.btnThemDonVI.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemDonVI.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemDonVI.Location = new System.Drawing.Point(6, 41);
            this.btnThemDonVI.Name = "btnThemDonVI";
            this.btnThemDonVI.Size = new System.Drawing.Size(84, 34);
            this.btnThemDonVI.TabIndex = 0;
            this.btnThemDonVI.Text = "Thêm";
            this.btnThemDonVI.UseVisualStyleBackColor = true;
            this.btnThemDonVI.Click += new System.EventHandler(this.btnThemDonVI_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtDonVi);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(18, 102);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 113);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin đơn vị";
            // 
            // txtDonVi
            // 
            this.txtDonVi.Location = new System.Drawing.Point(108, 51);
            this.txtDonVi.Name = "txtDonVi";
            this.txtDonVi.Size = new System.Drawing.Size(166, 24);
            this.txtDonVi.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên đơn vị:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel3);
            this.tabPage2.Controls.Add(this.panel4);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(706, 384);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Hệ số";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.dungeonLabel2);
            this.panel3.Location = new System.Drawing.Point(6, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(694, 97);
            this.panel3.TabIndex = 14;
            // 
            // dungeonLabel2
            // 
            this.dungeonLabel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dungeonLabel2.AutoSize = true;
            this.dungeonLabel2.BackColor = System.Drawing.Color.Transparent;
            this.dungeonLabel2.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dungeonLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(77)))));
            this.dungeonLabel2.Location = new System.Drawing.Point(246, 31);
            this.dungeonLabel2.Name = "dungeonLabel2";
            this.dungeonLabel2.Size = new System.Drawing.Size(239, 38);
            this.dungeonLabel2.TabIndex = 0;
            this.dungeonLabel2.Text = "Xử lý hệ số đơn vị";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.groupBox4);
            this.panel4.Controls.Add(this.groupBox5);
            this.panel4.Controls.Add(this.groupBox6);
            this.panel4.Location = new System.Drawing.Point(6, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(694, 368);
            this.panel4.TabIndex = 13;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tableHeSo);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(333, 102);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(341, 246);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Danh sách hệ số đơn vị";
            // 
            // tableHeSo
            // 
            this.tableHeSo.AllowUserToAddRows = false;
            this.tableHeSo.AllowUserToDeleteRows = false;
            this.tableHeSo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableHeSo.Location = new System.Drawing.Point(6, 23);
            this.tableHeSo.MultiSelect = false;
            this.tableHeSo.Name = "tableHeSo";
            this.tableHeSo.ReadOnly = true;
            this.tableHeSo.RowHeadersVisible = false;
            this.tableHeSo.RowHeadersWidth = 51;
            this.tableHeSo.RowTemplate.Height = 24;
            this.tableHeSo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tableHeSo.Size = new System.Drawing.Size(329, 217);
            this.tableHeSo.TabIndex = 0;
            this.tableHeSo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tableHeSo_CellClick);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnXoaHeSO);
            this.groupBox5.Controls.Add(this.btnSuaHS);
            this.groupBox5.Controls.Add(this.btnThemHs);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(18, 248);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(298, 100);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Xử lý chức năng";
            // 
            // btnXoaHeSO
            // 
            this.btnXoaHeSO.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoaHeSO.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoaHeSO.Location = new System.Drawing.Point(208, 41);
            this.btnXoaHeSO.Name = "btnXoaHeSO";
            this.btnXoaHeSO.Size = new System.Drawing.Size(84, 34);
            this.btnXoaHeSO.TabIndex = 2;
            this.btnXoaHeSO.Text = "Xóa";
            this.btnXoaHeSO.UseVisualStyleBackColor = true;
            this.btnXoaHeSO.Click += new System.EventHandler(this.btnXoaHeSO_Click);
            // 
            // btnSuaHS
            // 
            this.btnSuaHS.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSuaHS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSuaHS.Location = new System.Drawing.Point(108, 41);
            this.btnSuaHS.Name = "btnSuaHS";
            this.btnSuaHS.Size = new System.Drawing.Size(84, 34);
            this.btnSuaHS.TabIndex = 1;
            this.btnSuaHS.Text = "Sửa";
            this.btnSuaHS.UseVisualStyleBackColor = true;
            this.btnSuaHS.Click += new System.EventHandler(this.btnSuaHS_Click);
            // 
            // btnThemHs
            // 
            this.btnThemHs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemHs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThemHs.Location = new System.Drawing.Point(6, 41);
            this.btnThemHs.Name = "btnThemHs";
            this.btnThemHs.Size = new System.Drawing.Size(84, 34);
            this.btnThemHs.TabIndex = 0;
            this.btnThemHs.Text = "Thêm";
            this.btnThemHs.UseVisualStyleBackColor = true;
            this.btnThemHs.Click += new System.EventHandler(this.btnThemHs_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnSanPham);
            this.groupBox6.Controls.Add(this.txtHeSo);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.cboDonVi);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.txtTenNL);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(18, 102);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(298, 140);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Thông tin hệ số đơn vị";
            // 
            // btnSanPham
            // 
            this.btnSanPham.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSanPham.Location = new System.Drawing.Point(252, 32);
            this.btnSanPham.Name = "btnSanPham";
            this.btnSanPham.Size = new System.Drawing.Size(39, 28);
            this.btnSanPham.TabIndex = 6;
            this.btnSanPham.Text = "...";
            this.btnSanPham.UseVisualStyleBackColor = true;
            this.btnSanPham.Click += new System.EventHandler(this.btnSanPham_Click);
            // 
            // txtHeSo
            // 
            this.txtHeSo.DecimalPlaces = 3;
            this.txtHeSo.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.txtHeSo.Location = new System.Drawing.Point(117, 102);
            this.txtHeSo.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.txtHeSo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.txtHeSo.Name = "txtHeSo";
            this.txtHeSo.Size = new System.Drawing.Size(129, 24);
            this.txtHeSo.TabIndex = 5;
            this.txtHeSo.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(19, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 18);
            this.label4.TabIndex = 4;
            this.label4.Text = "Hệ số đv:";
            // 
            // cboDonVi
            // 
            this.cboDonVi.FormattingEnabled = true;
            this.cboDonVi.Location = new System.Drawing.Point(117, 69);
            this.cboDonVi.Name = "cboDonVi";
            this.cboDonVi.Size = new System.Drawing.Size(129, 26);
            this.cboDonVi.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(19, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Tên đơn vị:";
            // 
            // txtTenNL
            // 
            this.txtTenNL.Enabled = false;
            this.txtTenNL.Location = new System.Drawing.Point(117, 32);
            this.txtTenNL.Name = "txtTenNL";
            this.txtTenNL.Size = new System.Drawing.Size(129, 24);
            this.txtTenNL.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(19, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Tên NL:";
            // 
            // donVivaHeSoGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 432);
            this.Controls.Add(this.tabControl1);
            this.Name = "donVivaHeSoGUI";
            this.Text = "Đơn vị và hệ số";
            this.Load += new System.EventHandler(this.donVivaHeSoGUI_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tableDonVi)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tableHeSo)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHeSo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel2;
        private ReaLTaiizor.Controls.DungeonLabel dungeonLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView tableDonVi;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnXoaDonVi;
        private System.Windows.Forms.Button btnSuaDonVi;
        private System.Windows.Forms.Button btnThemDonVI;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtDonVi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel3;
        private ReaLTaiizor.Controls.DungeonLabel dungeonLabel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView tableHeSo;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnXoaHeSO;
        private System.Windows.Forms.Button btnSuaHS;
        private System.Windows.Forms.Button btnThemHs;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnSanPham;
        private System.Windows.Forms.NumericUpDown txtHeSo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboDonVi;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTenNL;
        private System.Windows.Forms.Label label2;
    }
}