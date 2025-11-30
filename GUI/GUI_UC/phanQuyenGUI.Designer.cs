namespace GUI.GUI_UC
{
    partial class phanQuyenGUI
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.topPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chucNangPanel = new System.Windows.Forms.Panel();
            this.panelTable = new System.Windows.Forms.Panel();
            this.tbPhanQuyen = new System.Windows.Forms.DataGridView();
            this.roundedChucNang = new GUI.COMPONENTS.RoundedPanel();
            this.layoutChucNang = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.layoutTimKiemNangCao = new System.Windows.Forms.TableLayoutPanel();
            this.txtTenDV = new System.Windows.Forms.TextBox();
            this.txtTenSP = new System.Windows.Forms.TextBox();
            this.txtTenNL = new System.Windows.Forms.TextBox();
            this.txtSoLuongMax = new System.Windows.Forms.TextBox();
            this.txtSoLuongMin = new System.Windows.Forms.TextBox();
            this.rdoTimNangCao = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.layoutTimKiemCoban = new System.Windows.Forms.TableLayoutPanel();
            this.cboTimKiemPQ = new System.Windows.Forms.ComboBox();
            this.txtTimKiemPQ = new System.Windows.Forms.TextBox();
            this.rdoTimCoBan = new System.Windows.Forms.RadioButton();
            this.btnThucHienTimKiem = new System.Windows.Forms.Button();
            this.xulycongthuc = new System.Windows.Forms.GroupBox();
            this.tableLayoutChoChucNang = new System.Windows.Forms.TableLayoutPanel();
            this.btnExcelPQ = new System.Windows.Forms.Button();
            this.btnReFreshPQ = new System.Windows.Forms.Button();
            this.btnChiTietPQ = new System.Windows.Forms.Button();
            this.btnVaiTro = new System.Windows.Forms.Button();
            this.btnGanQuyen = new System.Windows.Forms.Button();
            this.btnCRUDPQ = new System.Windows.Forms.Button();
            this.chucNangPanel.SuspendLayout();
            this.panelTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbPhanQuyen)).BeginInit();
            this.roundedChucNang.SuspendLayout();
            this.layoutChucNang.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.layoutTimKiemNangCao.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.layoutTimKiemCoban.SuspendLayout();
            this.xulycongthuc.SuspendLayout();
            this.tableLayoutChoChucNang.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.White;
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(10);
            this.topPanel.Size = new System.Drawing.Size(1550, 60);
            this.topPanel.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 660);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1550, 60);
            this.panel2.TabIndex = 7;
            // 
            // chucNangPanel
            // 
            this.chucNangPanel.BackColor = System.Drawing.Color.White;
            this.chucNangPanel.Controls.Add(this.roundedChucNang);
            this.chucNangPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.chucNangPanel.Location = new System.Drawing.Point(0, 60);
            this.chucNangPanel.Name = "chucNangPanel";
            this.chucNangPanel.Padding = new System.Windows.Forms.Padding(10);
            this.chucNangPanel.Size = new System.Drawing.Size(1550, 215);
            this.chucNangPanel.TabIndex = 8;
            // 
            // panelTable
            // 
            this.panelTable.BackColor = System.Drawing.Color.White;
            this.panelTable.Controls.Add(this.tbPhanQuyen);
            this.panelTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTable.Location = new System.Drawing.Point(0, 275);
            this.panelTable.Name = "panelTable";
            this.panelTable.Padding = new System.Windows.Forms.Padding(10);
            this.panelTable.Size = new System.Drawing.Size(1550, 385);
            this.panelTable.TabIndex = 9;
            // 
            // tbPhanQuyen
            // 
            this.tbPhanQuyen.AllowUserToAddRows = false;
            this.tbPhanQuyen.AllowUserToDeleteRows = false;
            this.tbPhanQuyen.AllowUserToResizeColumns = false;
            this.tbPhanQuyen.AllowUserToResizeRows = false;
            this.tbPhanQuyen.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tbPhanQuyen.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tbPhanQuyen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tbPhanQuyen.DefaultCellStyle = dataGridViewCellStyle2;
            this.tbPhanQuyen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPhanQuyen.EnableHeadersVisualStyles = false;
            this.tbPhanQuyen.Location = new System.Drawing.Point(10, 10);
            this.tbPhanQuyen.MultiSelect = false;
            this.tbPhanQuyen.Name = "tbPhanQuyen";
            this.tbPhanQuyen.RowHeadersVisible = false;
            this.tbPhanQuyen.RowHeadersWidth = 51;
            this.tbPhanQuyen.RowTemplate.Height = 24;
            this.tbPhanQuyen.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tbPhanQuyen.Size = new System.Drawing.Size(1530, 365);
            this.tbPhanQuyen.TabIndex = 5;
            this.tbPhanQuyen.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.tbPhanQuyen_CellFormatting);
            this.tbPhanQuyen.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.tbPhanQuyen_DataBindingComplete);
            // 
            // roundedChucNang
            // 
            this.roundedChucNang.BackColor = System.Drawing.Color.White;
            this.roundedChucNang.BorderColor = System.Drawing.Color.Silver;
            this.roundedChucNang.BorderRadius = 15;
            this.roundedChucNang.BorderSize = 2;
            this.roundedChucNang.Controls.Add(this.layoutChucNang);
            this.roundedChucNang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.roundedChucNang.Location = new System.Drawing.Point(10, 10);
            this.roundedChucNang.Name = "roundedChucNang";
            this.roundedChucNang.Padding = new System.Windows.Forms.Padding(10);
            this.roundedChucNang.Size = new System.Drawing.Size(1530, 195);
            this.roundedChucNang.TabIndex = 0;
            // 
            // layoutChucNang
            // 
            this.layoutChucNang.ColumnCount = 2;
            this.layoutChucNang.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.9F));
            this.layoutChucNang.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.1F));
            this.layoutChucNang.Controls.Add(this.tableLayoutPanel6, 1, 0);
            this.layoutChucNang.Controls.Add(this.xulycongthuc, 0, 0);
            this.layoutChucNang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutChucNang.Location = new System.Drawing.Point(10, 10);
            this.layoutChucNang.Name = "layoutChucNang";
            this.layoutChucNang.RowCount = 1;
            this.layoutChucNang.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutChucNang.Size = new System.Drawing.Size(1510, 175);
            this.layoutChucNang.TabIndex = 0;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.groupBox8, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.groupBox4, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(862, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(645, 169);
            this.tableLayoutPanel6.TabIndex = 7;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.layoutTimKiemNangCao);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox8.Location = new System.Drawing.Point(3, 87);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(639, 79);
            this.groupBox8.TabIndex = 5;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Tìm kiếm nâng cao";
            // 
            // layoutTimKiemNangCao
            // 
            this.layoutTimKiemNangCao.ColumnCount = 6;
            this.layoutTimKiemNangCao.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.09496F));
            this.layoutTimKiemNangCao.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.09496F));
            this.layoutTimKiemNangCao.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.09496F));
            this.layoutTimKiemNangCao.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.09496F));
            this.layoutTimKiemNangCao.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.09496F));
            this.layoutTimKiemNangCao.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.525194F));
            this.layoutTimKiemNangCao.Controls.Add(this.txtTenDV, 2, 0);
            this.layoutTimKiemNangCao.Controls.Add(this.txtTenSP, 0, 0);
            this.layoutTimKiemNangCao.Controls.Add(this.txtTenNL, 1, 0);
            this.layoutTimKiemNangCao.Controls.Add(this.txtSoLuongMax, 4, 0);
            this.layoutTimKiemNangCao.Controls.Add(this.txtSoLuongMin, 3, 0);
            this.layoutTimKiemNangCao.Controls.Add(this.rdoTimNangCao, 5, 0);
            this.layoutTimKiemNangCao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutTimKiemNangCao.Location = new System.Drawing.Point(3, 23);
            this.layoutTimKiemNangCao.Name = "layoutTimKiemNangCao";
            this.layoutTimKiemNangCao.RowCount = 1;
            this.layoutTimKiemNangCao.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTimKiemNangCao.Size = new System.Drawing.Size(633, 53);
            this.layoutTimKiemNangCao.TabIndex = 1;
            // 
            // txtTenDV
            // 
            this.txtTenDV.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtTenDV.Location = new System.Drawing.Point(245, 13);
            this.txtTenDV.Name = "txtTenDV";
            this.txtTenDV.Size = new System.Drawing.Size(109, 27);
            this.txtTenDV.TabIndex = 8;
            // 
            // txtTenSP
            // 
            this.txtTenSP.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtTenSP.Location = new System.Drawing.Point(5, 13);
            this.txtTenSP.Name = "txtTenSP";
            this.txtTenSP.Size = new System.Drawing.Size(109, 27);
            this.txtTenSP.TabIndex = 7;
            // 
            // txtTenNL
            // 
            this.txtTenNL.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtTenNL.Location = new System.Drawing.Point(125, 13);
            this.txtTenNL.Name = "txtTenNL";
            this.txtTenNL.Size = new System.Drawing.Size(109, 27);
            this.txtTenNL.TabIndex = 5;
            // 
            // txtSoLuongMax
            // 
            this.txtSoLuongMax.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtSoLuongMax.Location = new System.Drawing.Point(485, 13);
            this.txtSoLuongMax.Name = "txtSoLuongMax";
            this.txtSoLuongMax.Size = new System.Drawing.Size(109, 27);
            this.txtSoLuongMax.TabIndex = 3;
            // 
            // txtSoLuongMin
            // 
            this.txtSoLuongMin.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtSoLuongMin.Location = new System.Drawing.Point(365, 13);
            this.txtSoLuongMin.Name = "txtSoLuongMin";
            this.txtSoLuongMin.Size = new System.Drawing.Size(109, 27);
            this.txtSoLuongMin.TabIndex = 2;
            // 
            // rdoTimNangCao
            // 
            this.rdoTimNangCao.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rdoTimNangCao.AutoSize = true;
            this.rdoTimNangCao.Location = new System.Drawing.Point(605, 18);
            this.rdoTimNangCao.Margin = new System.Windows.Forms.Padding(3, 3, 9, 3);
            this.rdoTimNangCao.Name = "rdoTimNangCao";
            this.rdoTimNangCao.Size = new System.Drawing.Size(17, 16);
            this.rdoTimNangCao.TabIndex = 4;
            this.rdoTimNangCao.TabStop = true;
            this.rdoTimNangCao.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.layoutTimKiemCoban);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(639, 78);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Tìm kiếm cơ bản";
            // 
            // layoutTimKiemCoban
            // 
            this.layoutTimKiemCoban.ColumnCount = 4;
            this.layoutTimKiemCoban.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.layoutTimKiemCoban.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.layoutTimKiemCoban.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.layoutTimKiemCoban.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.layoutTimKiemCoban.Controls.Add(this.cboTimKiemPQ, 0, 0);
            this.layoutTimKiemCoban.Controls.Add(this.txtTimKiemPQ, 1, 0);
            this.layoutTimKiemCoban.Controls.Add(this.rdoTimCoBan, 2, 0);
            this.layoutTimKiemCoban.Controls.Add(this.btnThucHienTimKiem, 3, 0);
            this.layoutTimKiemCoban.Location = new System.Drawing.Point(3, 23);
            this.layoutTimKiemCoban.Name = "layoutTimKiemCoban";
            this.layoutTimKiemCoban.RowCount = 1;
            this.layoutTimKiemCoban.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTimKiemCoban.Size = new System.Drawing.Size(633, 45);
            this.layoutTimKiemCoban.TabIndex = 1;
            // 
            // cboTimKiemPQ
            // 
            this.cboTimKiemPQ.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cboTimKiemPQ.FormattingEnabled = true;
            this.cboTimKiemPQ.Items.AddRange(new object[] {
            "Mã SP",
            "Tên SP",
            "Mã NL",
            "Tên NL",
            "Tên ĐV",
            "SL min",
            "SL max"});
            this.cboTimKiemPQ.Location = new System.Drawing.Point(10, 10);
            this.cboTimKiemPQ.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.cboTimKiemPQ.Name = "cboTimKiemPQ";
            this.cboTimKiemPQ.Size = new System.Drawing.Size(176, 28);
            this.cboTimKiemPQ.TabIndex = 0;
            // 
            // txtTimKiemPQ
            // 
            this.txtTimKiemPQ.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtTimKiemPQ.Location = new System.Drawing.Point(200, 9);
            this.txtTimKiemPQ.Name = "txtTimKiemPQ";
            this.txtTimKiemPQ.Size = new System.Drawing.Size(243, 27);
            this.txtTimKiemPQ.TabIndex = 1;
            // 
            // rdoTimCoBan
            // 
            this.rdoTimCoBan.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rdoTimCoBan.AutoSize = true;
            this.rdoTimCoBan.Location = new System.Drawing.Point(464, 14);
            this.rdoTimCoBan.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.rdoTimCoBan.Name = "rdoTimCoBan";
            this.rdoTimCoBan.Size = new System.Drawing.Size(17, 16);
            this.rdoTimCoBan.TabIndex = 2;
            this.rdoTimCoBan.TabStop = true;
            this.rdoTimCoBan.UseVisualStyleBackColor = true;
            // 
            // btnThucHienTimKiem
            // 
            this.btnThucHienTimKiem.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnThucHienTimKiem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThucHienTimKiem.Location = new System.Drawing.Point(520, 3);
            this.btnThucHienTimKiem.Name = "btnThucHienTimKiem";
            this.btnThucHienTimKiem.Size = new System.Drawing.Size(109, 39);
            this.btnThucHienTimKiem.TabIndex = 3;
            this.btnThucHienTimKiem.Text = "Tìm kiếm";
            this.btnThucHienTimKiem.UseVisualStyleBackColor = true;
            // 
            // xulycongthuc
            // 
            this.xulycongthuc.Controls.Add(this.tableLayoutChoChucNang);
            this.xulycongthuc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xulycongthuc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xulycongthuc.Location = new System.Drawing.Point(3, 3);
            this.xulycongthuc.Name = "xulycongthuc";
            this.xulycongthuc.Size = new System.Drawing.Size(853, 169);
            this.xulycongthuc.TabIndex = 0;
            this.xulycongthuc.TabStop = false;
            this.xulycongthuc.Text = "Xử lý phân quyền";
            // 
            // tableLayoutChoChucNang
            // 
            this.tableLayoutChoChucNang.ColumnCount = 6;
            this.tableLayoutChoChucNang.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutChoChucNang.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutChoChucNang.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutChoChucNang.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutChoChucNang.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutChoChucNang.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutChoChucNang.Controls.Add(this.btnExcelPQ, 5, 0);
            this.tableLayoutChoChucNang.Controls.Add(this.btnReFreshPQ, 4, 0);
            this.tableLayoutChoChucNang.Controls.Add(this.btnChiTietPQ, 3, 0);
            this.tableLayoutChoChucNang.Controls.Add(this.btnVaiTro, 2, 0);
            this.tableLayoutChoChucNang.Controls.Add(this.btnGanQuyen, 1, 0);
            this.tableLayoutChoChucNang.Controls.Add(this.btnCRUDPQ, 0, 0);
            this.tableLayoutChoChucNang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutChoChucNang.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutChoChucNang.Name = "tableLayoutChoChucNang";
            this.tableLayoutChoChucNang.RowCount = 1;
            this.tableLayoutChoChucNang.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutChoChucNang.Size = new System.Drawing.Size(847, 143);
            this.tableLayoutChoChucNang.TabIndex = 0;
            // 
            // btnExcelPQ
            // 
            this.btnExcelPQ.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExcelPQ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExcelPQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExcelPQ.Image = global::GUI.Properties.Resources.xls;
            this.btnExcelPQ.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExcelPQ.Location = new System.Drawing.Point(727, 10);
            this.btnExcelPQ.Margin = new System.Windows.Forms.Padding(22, 10, 22, 10);
            this.btnExcelPQ.Name = "btnExcelPQ";
            this.btnExcelPQ.Size = new System.Drawing.Size(98, 123);
            this.btnExcelPQ.TabIndex = 7;
            this.btnExcelPQ.Text = "Excel";
            this.btnExcelPQ.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExcelPQ.UseVisualStyleBackColor = true;
            // 
            // btnReFreshPQ
            // 
            this.btnReFreshPQ.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReFreshPQ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReFreshPQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReFreshPQ.Image = global::GUI.Properties.Resources.reload;
            this.btnReFreshPQ.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnReFreshPQ.Location = new System.Drawing.Point(586, 10);
            this.btnReFreshPQ.Margin = new System.Windows.Forms.Padding(22, 10, 22, 10);
            this.btnReFreshPQ.Name = "btnReFreshPQ";
            this.btnReFreshPQ.Size = new System.Drawing.Size(97, 123);
            this.btnReFreshPQ.TabIndex = 4;
            this.btnReFreshPQ.Text = "Làm mới";
            this.btnReFreshPQ.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReFreshPQ.UseVisualStyleBackColor = true;
            // 
            // btnChiTietPQ
            // 
            this.btnChiTietPQ.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChiTietPQ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChiTietPQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChiTietPQ.Image = global::GUI.Properties.Resources.info;
            this.btnChiTietPQ.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnChiTietPQ.Location = new System.Drawing.Point(445, 10);
            this.btnChiTietPQ.Margin = new System.Windows.Forms.Padding(22, 10, 22, 10);
            this.btnChiTietPQ.Name = "btnChiTietPQ";
            this.btnChiTietPQ.Size = new System.Drawing.Size(97, 123);
            this.btnChiTietPQ.TabIndex = 3;
            this.btnChiTietPQ.Text = "Chi tiết";
            this.btnChiTietPQ.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnChiTietPQ.UseVisualStyleBackColor = true;
            // 
            // btnVaiTro
            // 
            this.btnVaiTro.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVaiTro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVaiTro.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVaiTro.Image = global::GUI.Properties.Resources.id_card__1_;
            this.btnVaiTro.Location = new System.Drawing.Point(304, 10);
            this.btnVaiTro.Margin = new System.Windows.Forms.Padding(22, 10, 22, 10);
            this.btnVaiTro.Name = "btnVaiTro";
            this.btnVaiTro.Size = new System.Drawing.Size(97, 123);
            this.btnVaiTro.TabIndex = 2;
            this.btnVaiTro.Text = "Vai trò";
            this.btnVaiTro.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnVaiTro.UseVisualStyleBackColor = true;
            // 
            // btnGanQuyen
            // 
            this.btnGanQuyen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGanQuyen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGanQuyen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGanQuyen.Image = global::GUI.Properties.Resources.project_management__3_;
            this.btnGanQuyen.Location = new System.Drawing.Point(163, 10);
            this.btnGanQuyen.Margin = new System.Windows.Forms.Padding(22, 10, 22, 10);
            this.btnGanQuyen.Name = "btnGanQuyen";
            this.btnGanQuyen.Size = new System.Drawing.Size(97, 123);
            this.btnGanQuyen.TabIndex = 1;
            this.btnGanQuyen.Text = "Gán";
            this.btnGanQuyen.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnGanQuyen.UseVisualStyleBackColor = true;
            // 
            // btnCRUDPQ
            // 
            this.btnCRUDPQ.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCRUDPQ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCRUDPQ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCRUDPQ.Image = global::GUI.Properties.Resources.add;
            this.btnCRUDPQ.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCRUDPQ.Location = new System.Drawing.Point(22, 10);
            this.btnCRUDPQ.Margin = new System.Windows.Forms.Padding(22, 10, 22, 10);
            this.btnCRUDPQ.Name = "btnCRUDPQ";
            this.btnCRUDPQ.Size = new System.Drawing.Size(97, 123);
            this.btnCRUDPQ.TabIndex = 0;
            this.btnCRUDPQ.Text = "T/S/X";
            this.btnCRUDPQ.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCRUDPQ.UseVisualStyleBackColor = true;
            this.btnCRUDPQ.Click += new System.EventHandler(this.btnCRUDPQ_Click);
            // 
            // phanQuyenGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelTable);
            this.Controls.Add(this.chucNangPanel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.topPanel);
            this.Name = "phanQuyenGUI";
            this.Size = new System.Drawing.Size(1550, 720);
            this.Load += new System.EventHandler(this.phanQuyenGUI_Load);
            this.chucNangPanel.ResumeLayout(false);
            this.panelTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbPhanQuyen)).EndInit();
            this.roundedChucNang.ResumeLayout(false);
            this.layoutChucNang.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.layoutTimKiemNangCao.ResumeLayout(false);
            this.layoutTimKiemNangCao.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.layoutTimKiemCoban.ResumeLayout(false);
            this.layoutTimKiemCoban.PerformLayout();
            this.xulycongthuc.ResumeLayout(false);
            this.tableLayoutChoChucNang.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel chucNangPanel;
        private COMPONENTS.RoundedPanel roundedChucNang;
        private System.Windows.Forms.TableLayoutPanel layoutChucNang;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TableLayoutPanel layoutTimKiemNangCao;
        private System.Windows.Forms.TextBox txtTenDV;
        private System.Windows.Forms.TextBox txtTenSP;
        private System.Windows.Forms.TextBox txtTenNL;
        private System.Windows.Forms.TextBox txtSoLuongMax;
        private System.Windows.Forms.TextBox txtSoLuongMin;
        private System.Windows.Forms.RadioButton rdoTimNangCao;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel layoutTimKiemCoban;
        private System.Windows.Forms.ComboBox cboTimKiemPQ;
        private System.Windows.Forms.TextBox txtTimKiemPQ;
        private System.Windows.Forms.RadioButton rdoTimCoBan;
        private System.Windows.Forms.Button btnThucHienTimKiem;
        private System.Windows.Forms.GroupBox xulycongthuc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutChoChucNang;
        private System.Windows.Forms.Button btnExcelPQ;
        private System.Windows.Forms.Button btnReFreshPQ;
        private System.Windows.Forms.Button btnChiTietPQ;
        private System.Windows.Forms.Button btnVaiTro;
        private System.Windows.Forms.Button btnGanQuyen;
        private System.Windows.Forms.Button btnCRUDPQ;
        private System.Windows.Forms.Panel panelTable;
        private System.Windows.Forms.DataGridView tbPhanQuyen;
    }
}
