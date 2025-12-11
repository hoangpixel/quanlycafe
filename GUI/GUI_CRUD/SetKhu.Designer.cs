namespace GUI.GUI_CRUD
{
    partial class SetKhu
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvKV = new System.Windows.Forms.DataGridView();
            this.btnThemKV = new System.Windows.Forms.Button();
            this.btnSuaKV = new System.Windows.Forms.Button();
            this.btnXoaKV = new System.Windows.Forms.Button();
            this.txtKV = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKV)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(482, 100);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(37, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(407, 51);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chỉnh sửa Khu Vực";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvKV);
            this.groupBox1.Location = new System.Drawing.Point(0, 106);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(482, 260);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Danh Sách Khu Vực";
            // 
            // dgvKV
            // 
            this.dgvKV.AllowUserToAddRows = false;
            this.dgvKV.AllowUserToDeleteRows = false;
            this.dgvKV.AllowUserToResizeColumns = false;
            this.dgvKV.AllowUserToResizeRows = false;
            this.dgvKV.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgvKV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKV.Location = new System.Drawing.Point(6, 32);
            this.dgvKV.MultiSelect = false;
            this.dgvKV.Name = "dgvKV";
            this.dgvKV.ReadOnly = true;
            this.dgvKV.RowHeadersWidth = 51;
            this.dgvKV.RowTemplate.Height = 24;
            this.dgvKV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvKV.Size = new System.Drawing.Size(470, 222);
            this.dgvKV.TabIndex = 0;
            this.dgvKV.SelectionChanged += new System.EventHandler(this.dgvKV_SelectionChanged);
            // 
            // btnThemKV
            // 
            this.btnThemKV.Location = new System.Drawing.Point(12, 422);
            this.btnThemKV.Name = "btnThemKV";
            this.btnThemKV.Size = new System.Drawing.Size(108, 48);
            this.btnThemKV.TabIndex = 2;
            this.btnThemKV.Text = "Thêm";
            this.btnThemKV.UseVisualStyleBackColor = true;
            this.btnThemKV.Click += new System.EventHandler(this.btnThemKV_Click);
            // 
            // btnSuaKV
            // 
            this.btnSuaKV.Location = new System.Drawing.Point(170, 422);
            this.btnSuaKV.Name = "btnSuaKV";
            this.btnSuaKV.Size = new System.Drawing.Size(108, 48);
            this.btnSuaKV.TabIndex = 3;
            this.btnSuaKV.Text = "Sửa";
            this.btnSuaKV.UseVisualStyleBackColor = true;
            this.btnSuaKV.Click += new System.EventHandler(this.btnSuaKV_Click);
            // 
            // btnXoaKV
            // 
            this.btnXoaKV.Location = new System.Drawing.Point(340, 422);
            this.btnXoaKV.Name = "btnXoaKV";
            this.btnXoaKV.Size = new System.Drawing.Size(108, 48);
            this.btnXoaKV.TabIndex = 4;
            this.btnXoaKV.Text = "Xóa";
            this.btnXoaKV.UseVisualStyleBackColor = true;
            this.btnXoaKV.Click += new System.EventHandler(this.btnXoaKV_Click);
            // 
            // txtKV
            // 
            this.txtKV.Location = new System.Drawing.Point(12, 382);
            this.txtKV.Name = "txtKV";
            this.txtKV.Size = new System.Drawing.Size(217, 22);
            this.txtKV.TabIndex = 5;
            // 
            // SetKhu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 490);
            this.Controls.Add(this.txtKV);
            this.Controls.Add(this.btnXoaKV);
            this.Controls.Add(this.btnSuaKV);
            this.Controls.Add(this.btnThemKV);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "SetKhu";
            this.Text = "SetKhu";
            this.Load += new System.EventHandler(this.SetKhu_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvKV;
        private System.Windows.Forms.Button btnThemKV;
        private System.Windows.Forms.Button btnSuaKV;
        private System.Windows.Forms.Button btnXoaKV;
        private System.Windows.Forms.TextBox txtKV;
    }
}