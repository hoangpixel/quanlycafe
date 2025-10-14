namespace quanlycafe.GUI
{
    partial class loaiSPGUI
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtLoaiSp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnXoaLoaiSP = new System.Windows.Forms.Button();
            this.btnThemLoaiSP = new System.Windows.Forms.Button();
            this.btnSuaLoaiSp = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLoaiSP = new System.Windows.Forms.DataGridView();
            this.bigLabel1 = new ReaLTaiizor.Controls.BigLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tableLoaiSP)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtLoaiSp);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(32, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(322, 128);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin loại sản phẩm";
            // 
            // txtLoaiSp
            // 
            this.txtLoaiSp.Location = new System.Drawing.Point(123, 59);
            this.txtLoaiSp.Name = "txtLoaiSp";
            this.txtLoaiSp.Size = new System.Drawing.Size(177, 24);
            this.txtLoaiSp.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên loại SP:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnXoaLoaiSP);
            this.groupBox2.Controls.Add(this.btnThemLoaiSP);
            this.groupBox2.Controls.Add(this.btnSuaLoaiSp);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(32, 245);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(322, 95);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Xử lý loại sản phẩm";
            // 
            // btnXoaLoaiSP
            // 
            this.btnXoaLoaiSP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoaLoaiSP.Location = new System.Drawing.Point(221, 43);
            this.btnXoaLoaiSP.Name = "btnXoaLoaiSP";
            this.btnXoaLoaiSP.Size = new System.Drawing.Size(79, 31);
            this.btnXoaLoaiSP.TabIndex = 6;
            this.btnXoaLoaiSP.Text = "Xóa";
            this.btnXoaLoaiSP.UseVisualStyleBackColor = true;
            this.btnXoaLoaiSP.Click += new System.EventHandler(this.btnXoaLoaiSP_Click);
            // 
            // btnThemLoaiSP
            // 
            this.btnThemLoaiSP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThemLoaiSP.Location = new System.Drawing.Point(20, 42);
            this.btnThemLoaiSP.Name = "btnThemLoaiSP";
            this.btnThemLoaiSP.Size = new System.Drawing.Size(79, 31);
            this.btnThemLoaiSP.TabIndex = 4;
            this.btnThemLoaiSP.Text = "Thêm";
            this.btnThemLoaiSP.UseVisualStyleBackColor = true;
            this.btnThemLoaiSP.Click += new System.EventHandler(this.btnThemLoaiSP_Click);
            // 
            // btnSuaLoaiSp
            // 
            this.btnSuaLoaiSp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSuaLoaiSp.Location = new System.Drawing.Point(123, 43);
            this.btnSuaLoaiSp.Name = "btnSuaLoaiSp";
            this.btnSuaLoaiSp.Size = new System.Drawing.Size(79, 31);
            this.btnSuaLoaiSp.TabIndex = 5;
            this.btnSuaLoaiSp.Text = "Sửa";
            this.btnSuaLoaiSp.UseVisualStyleBackColor = true;
            this.btnSuaLoaiSp.Click += new System.EventHandler(this.btnSuaLoaiSp_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLoaiSP);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(397, 84);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(273, 257);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Dữ liệu loại sản phẩm";
            // 
            // tableLoaiSP
            // 
            this.tableLoaiSP.AllowUserToAddRows = false;
            this.tableLoaiSP.AllowUserToDeleteRows = false;
            this.tableLoaiSP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLoaiSP.Location = new System.Drawing.Point(6, 21);
            this.tableLoaiSP.MultiSelect = false;
            this.tableLoaiSP.Name = "tableLoaiSP";
            this.tableLoaiSP.ReadOnly = true;
            this.tableLoaiSP.RowHeadersVisible = false;
            this.tableLoaiSP.RowHeadersWidth = 51;
            this.tableLoaiSP.RowTemplate.Height = 24;
            this.tableLoaiSP.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tableLoaiSP.Size = new System.Drawing.Size(260, 230);
            this.tableLoaiSP.TabIndex = 0;
            this.tableLoaiSP.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tableLoaiSP_CellClick);
            // 
            // bigLabel1
            // 
            this.bigLabel1.AutoSize = true;
            this.bigLabel1.BackColor = System.Drawing.Color.Transparent;
            this.bigLabel1.Font = new System.Drawing.Font("Segoe UI", 25F);
            this.bigLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.bigLabel1.Location = new System.Drawing.Point(145, 9);
            this.bigLabel1.Name = "bigLabel1";
            this.bigLabel1.Size = new System.Drawing.Size(393, 57);
            this.bigLabel1.TabIndex = 3;
            this.bigLabel1.Text = "Xử lý dữ liệu loại SP";
            // 
            // loaiSPGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 377);
            this.Controls.Add(this.bigLabel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "loaiSPGUI";
            this.Text = "Loại sản phẩm";
            this.Load += new System.EventHandler(this.loaiSPGUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tableLoaiSP)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnXoaLoaiSP;
        private System.Windows.Forms.Button btnThemLoaiSP;
        private System.Windows.Forms.Button btnSuaLoaiSp;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView tableLoaiSP;
        private System.Windows.Forms.TextBox txtLoaiSp;
        private System.Windows.Forms.Label label1;
        private ReaLTaiizor.Controls.BigLabel bigLabel1;
    }
}