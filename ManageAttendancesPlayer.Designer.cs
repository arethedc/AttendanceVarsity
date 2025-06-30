namespace AttendanceVarsity
{
    partial class ManageAttendancesPlayer
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            panel1 = new Panel();
            btnBack = new Guna.UI2.WinForms.Guna2Button();
            panel3 = new Panel();
            lblPlayerName = new Label();
            panel2 = new Panel();
            btnPrint = new Guna.UI2.WinForms.Guna2Button();
            btnExport = new Guna.UI2.WinForms.Guna2Button();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            label3 = new Label();
            dgvPlayerAttendance = new Guna.UI2.WinForms.Guna2DataGridView();
            cmbStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            label1 = new Label();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPlayerAttendance).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(166, 200, 142);
            panel1.Controls.Add(btnBack);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(50, 611);
            panel1.TabIndex = 5;
            // 
            // btnBack
            // 
            btnBack.CustomBorderColor = Color.White;
            btnBack.CustomizableEdges = customizableEdges1;
            btnBack.DisabledState.BorderColor = Color.DarkGray;
            btnBack.DisabledState.CustomBorderColor = Color.DarkGray;
            btnBack.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnBack.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnBack.FillColor = Color.FromArgb(166, 200, 142);
            btnBack.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBack.ForeColor = Color.White;
            btnBack.HoverState.FillColor = Color.FromArgb(21, 95, 40);
            btnBack.HoverState.ForeColor = Color.White;
            btnBack.Location = new Point(0, 0);
            btnBack.Name = "btnBack";
            btnBack.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnBack.Size = new Size(50, 50);
            btnBack.TabIndex = 14;
            btnBack.Click += btnBack_Click;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(21, 95, 40);
            panel3.Controls.Add(lblPlayerName);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(50, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(834, 50);
            panel3.TabIndex = 6;
            // 
            // lblPlayerName
            // 
            lblPlayerName.AutoSize = true;
            lblPlayerName.BackColor = Color.Transparent;
            lblPlayerName.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPlayerName.ForeColor = Color.White;
            lblPlayerName.Location = new Point(16, 13);
            lblPlayerName.Name = "lblPlayerName";
            lblPlayerName.Size = new Size(135, 24);
            lblPlayerName.TabIndex = 30;
            lblPlayerName.Text = "Player: Name";
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(btnPrint);
            panel2.Controls.Add(btnExport);
            panel2.Controls.Add(txtSearch);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(dgvPlayerAttendance);
            panel2.Controls.Add(cmbStatus);
            panel2.Controls.Add(btnRefresh);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(50, 50);
            panel2.Name = "panel2";
            panel2.Size = new Size(834, 561);
            panel2.TabIndex = 34;
            // 
            // btnPrint
            // 
            btnPrint.BorderColor = Color.FromArgb(21, 95, 40);
            btnPrint.BorderRadius = 5;
            btnPrint.BorderThickness = 1;
            btnPrint.CustomBorderColor = Color.White;
            btnPrint.CustomizableEdges = customizableEdges3;
            btnPrint.DisabledState.BorderColor = Color.DarkGray;
            btnPrint.DisabledState.CustomBorderColor = Color.DarkGray;
            btnPrint.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnPrint.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnPrint.FillColor = Color.White;
            btnPrint.Font = new Font("Segoe UI", 9F);
            btnPrint.ForeColor = Color.FromArgb(21, 95, 40);
            btnPrint.HoverState.FillColor = Color.FromArgb(21, 95, 40);
            btnPrint.HoverState.ForeColor = Color.White;
            btnPrint.Location = new Point(737, 21);
            btnPrint.Name = "btnPrint";
            btnPrint.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnPrint.Size = new Size(68, 36);
            btnPrint.TabIndex = 107;
            btnPrint.Text = "Print";
            // 
            // btnExport
            // 
            btnExport.BorderColor = Color.FromArgb(21, 95, 40);
            btnExport.BorderRadius = 5;
            btnExport.BorderThickness = 1;
            btnExport.CustomBorderColor = Color.White;
            btnExport.CustomizableEdges = customizableEdges5;
            btnExport.DisabledState.BorderColor = Color.DarkGray;
            btnExport.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExport.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExport.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExport.FillColor = Color.White;
            btnExport.Font = new Font("Segoe UI", 9F);
            btnExport.ForeColor = Color.FromArgb(21, 95, 40);
            btnExport.HoverState.FillColor = Color.FromArgb(21, 95, 40);
            btnExport.HoverState.ForeColor = Color.White;
            btnExport.Location = new Point(663, 21);
            btnExport.Name = "btnExport";
            btnExport.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnExport.Size = new Size(68, 36);
            btnExport.TabIndex = 106;
            btnExport.Text = "Export";
            // 
            // txtSearch
            // 
            txtSearch.CustomizableEdges = customizableEdges7;
            txtSearch.DefaultText = "";
            txtSearch.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSearch.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSearch.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.Font = new Font("Segoe UI", 9F);
            txtSearch.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.Location = new Point(379, 21);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Name - Student ID";
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtSearch.Size = new Size(223, 36);
            txtSearch.TabIndex = 103;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Microsoft Sans Serif", 12F);
            label3.ForeColor = Color.FromArgb(21, 95, 40);
            label3.Location = new Point(248, 29);
            label3.Name = "label3";
            label3.Size = new Size(125, 20);
            label3.TabIndex = 102;
            label3.Text = "Search Session:";
            // 
            // dgvPlayerAttendance
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvPlayerAttendance.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvPlayerAttendance.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvPlayerAttendance.ColumnHeadersHeight = 4;
            dgvPlayerAttendance.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvPlayerAttendance.DefaultCellStyle = dataGridViewCellStyle3;
            dgvPlayerAttendance.GridColor = Color.FromArgb(231, 229, 255);
            dgvPlayerAttendance.Location = new Point(27, 63);
            dgvPlayerAttendance.Name = "dgvPlayerAttendance";
            dgvPlayerAttendance.RowHeadersVisible = false;
            dgvPlayerAttendance.Size = new Size(778, 486);
            dgvPlayerAttendance.TabIndex = 96;
            dgvPlayerAttendance.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvPlayerAttendance.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvPlayerAttendance.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvPlayerAttendance.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvPlayerAttendance.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvPlayerAttendance.ThemeStyle.BackColor = Color.White;
            dgvPlayerAttendance.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvPlayerAttendance.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvPlayerAttendance.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvPlayerAttendance.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dgvPlayerAttendance.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvPlayerAttendance.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvPlayerAttendance.ThemeStyle.HeaderStyle.Height = 4;
            dgvPlayerAttendance.ThemeStyle.ReadOnly = false;
            dgvPlayerAttendance.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvPlayerAttendance.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPlayerAttendance.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvPlayerAttendance.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvPlayerAttendance.ThemeStyle.RowsStyle.Height = 25;
            dgvPlayerAttendance.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvPlayerAttendance.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // cmbStatus
            // 
            cmbStatus.BackColor = Color.Transparent;
            cmbStatus.CustomizableEdges = customizableEdges9;
            cmbStatus.DrawMode = DrawMode.OwnerDrawFixed;
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbStatus.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbStatus.Font = new Font("Segoe UI", 10F);
            cmbStatus.ForeColor = Color.FromArgb(68, 88, 112);
            cmbStatus.ItemHeight = 30;
            cmbStatus.Location = new Point(93, 21);
            cmbStatus.Name = "cmbStatus";
            cmbStatus.ShadowDecoration.CustomizableEdges = customizableEdges10;
            cmbStatus.Size = new Size(144, 36);
            cmbStatus.TabIndex = 101;
            // 
            // btnRefresh
            // 
            btnRefresh.BorderRadius = 18;
            btnRefresh.CustomBorderColor = Color.White;
            btnRefresh.CustomizableEdges = customizableEdges11;
            btnRefresh.DisabledState.BorderColor = Color.DarkGray;
            btnRefresh.DisabledState.CustomBorderColor = Color.DarkGray;
            btnRefresh.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnRefresh.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnRefresh.FillColor = Color.FromArgb(166, 200, 142);
            btnRefresh.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.HoverState.FillColor = Color.FromArgb(21, 95, 40);
            btnRefresh.HoverState.ForeColor = Color.White;
            btnRefresh.Location = new Point(615, 21);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnRefresh.Size = new Size(36, 36);
            btnRefresh.TabIndex = 93;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Microsoft Sans Serif", 12F);
            label1.ForeColor = Color.FromArgb(21, 95, 40);
            label1.Location = new Point(27, 29);
            label1.Name = "label1";
            label1.Size = new Size(60, 20);
            label1.TabIndex = 83;
            label1.Text = "Status:";
            // 
            // ManageAttendancesPlayer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 611);
            Controls.Add(panel2);
            Controls.Add(panel3);
            Controls.Add(panel1);
            Name = "ManageAttendancesPlayer";
            Text = "ManageAttendancesPlayer";
            Load += ManageAttendancesPlayer_Load;
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPlayerAttendance).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private Panel panel3;
        private Label lblPlayerName;
        private Panel panel2;
        private Guna.UI2.WinForms.Guna2Button btnPrint;
        private Guna.UI2.WinForms.Guna2Button btnExport;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Label label3;
        private Guna.UI2.WinForms.Guna2DataGridView dgvPlayerAttendance;
        private Guna.UI2.WinForms.Guna2ComboBox cmbStatus;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private Label label1;
    }
}