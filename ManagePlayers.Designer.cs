namespace AttendanceVarsity
{
    partial class ManagePlayers
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            panel3 = new Panel();
            label13 = new Label();
            panel1 = new Panel();
            btnBack = new Guna.UI2.WinForms.Guna2Button();
            panel2 = new Panel();
            dgvPlayers = new Guna.UI2.WinForms.Guna2DataGridView();
            btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            cmbGender = new Guna.UI2.WinForms.Guna2ComboBox();
            cmbLevel = new Guna.UI2.WinForms.Guna2ComboBox();
            cmbSport = new Guna.UI2.WinForms.Guna2ComboBox();
            label1 = new Label();
            label2 = new Label();
            panel3.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPlayers).BeginInit();
            SuspendLayout();
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(21, 95, 40);
            panel3.Controls.Add(label13);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(50, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(834, 50);
            panel3.TabIndex = 4;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.BackColor = Color.Transparent;
            label13.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label13.ForeColor = Color.White;
            label13.Location = new Point(16, 13);
            label13.Name = "label13";
            label13.Size = new Size(159, 24);
            label13.TabIndex = 30;
            label13.Text = "Manage Players";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(166, 200, 142);
            panel1.Controls.Add(btnBack);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(50, 611);
            panel1.TabIndex = 3;
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
            btnBack.TabIndex = 13;
            btnBack.Click += btnBack_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(dgvPlayers);
            panel2.Controls.Add(btnRefresh);
            panel2.Controls.Add(txtSearch);
            panel2.Controls.Add(cmbGender);
            panel2.Controls.Add(cmbLevel);
            panel2.Controls.Add(cmbSport);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label2);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(50, 50);
            panel2.Name = "panel2";
            panel2.Size = new Size(834, 561);
            panel2.TabIndex = 31;
            // 
            // dgvPlayers
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvPlayers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvPlayers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvPlayers.ColumnHeadersHeight = 4;
            dgvPlayers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvPlayers.DefaultCellStyle = dataGridViewCellStyle3;
            dgvPlayers.GridColor = Color.FromArgb(231, 229, 255);
            dgvPlayers.Location = new Point(27, 63);
            dgvPlayers.Name = "dgvPlayers";
            dgvPlayers.RowHeadersVisible = false;
            dgvPlayers.Size = new Size(778, 475);
            dgvPlayers.TabIndex = 94;
            dgvPlayers.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvPlayers.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvPlayers.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvPlayers.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvPlayers.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvPlayers.ThemeStyle.BackColor = Color.White;
            dgvPlayers.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvPlayers.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvPlayers.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvPlayers.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dgvPlayers.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvPlayers.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvPlayers.ThemeStyle.HeaderStyle.Height = 4;
            dgvPlayers.ThemeStyle.ReadOnly = false;
            dgvPlayers.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvPlayers.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPlayers.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvPlayers.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvPlayers.ThemeStyle.RowsStyle.Height = 25;
            dgvPlayers.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvPlayers.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // btnRefresh
            // 
            btnRefresh.CustomBorderColor = Color.White;
            btnRefresh.CustomizableEdges = customizableEdges3;
            btnRefresh.DisabledState.BorderColor = Color.DarkGray;
            btnRefresh.DisabledState.CustomBorderColor = Color.DarkGray;
            btnRefresh.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnRefresh.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnRefresh.FillColor = Color.FromArgb(166, 200, 142);
            btnRefresh.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.HoverState.FillColor = Color.FromArgb(21, 95, 40);
            btnRefresh.HoverState.ForeColor = Color.White;
            btnRefresh.Location = new Point(761, 21);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnRefresh.Size = new Size(44, 36);
            btnRefresh.TabIndex = 93;
            // 
            // txtSearch
            // 
            txtSearch.CustomizableEdges = customizableEdges5;
            txtSearch.DefaultText = "";
            txtSearch.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSearch.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSearch.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.Font = new Font("Segoe UI", 9F);
            txtSearch.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.Location = new Point(97, 21);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "";
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtSearch.Size = new Size(200, 36);
            txtSearch.TabIndex = 92;
            // 
            // cmbGender
            // 
            cmbGender.BackColor = Color.Transparent;
            cmbGender.CustomizableEdges = customizableEdges7;
            cmbGender.DrawMode = DrawMode.OwnerDrawFixed;
            cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGender.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbGender.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbGender.Font = new Font("Segoe UI", 10F);
            cmbGender.ForeColor = Color.FromArgb(68, 88, 112);
            cmbGender.ItemHeight = 30;
            cmbGender.Location = new Point(629, 21);
            cmbGender.Name = "cmbGender";
            cmbGender.ShadowDecoration.CustomizableEdges = customizableEdges8;
            cmbGender.Size = new Size(126, 36);
            cmbGender.TabIndex = 90;
            // 
            // cmbLevel
            // 
            cmbLevel.BackColor = Color.Transparent;
            cmbLevel.CustomizableEdges = customizableEdges9;
            cmbLevel.DrawMode = DrawMode.OwnerDrawFixed;
            cmbLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLevel.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbLevel.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbLevel.Font = new Font("Segoe UI", 10F);
            cmbLevel.ForeColor = Color.FromArgb(68, 88, 112);
            cmbLevel.ItemHeight = 30;
            cmbLevel.Location = new Point(497, 21);
            cmbLevel.Name = "cmbLevel";
            cmbLevel.ShadowDecoration.CustomizableEdges = customizableEdges10;
            cmbLevel.Size = new Size(126, 36);
            cmbLevel.TabIndex = 89;
            // 
            // cmbSport
            // 
            cmbSport.BackColor = Color.Transparent;
            cmbSport.CustomizableEdges = customizableEdges11;
            cmbSport.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSport.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSport.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbSport.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbSport.Font = new Font("Segoe UI", 10F);
            cmbSport.ForeColor = Color.FromArgb(68, 88, 112);
            cmbSport.ItemHeight = 30;
            cmbSport.Location = new Point(365, 21);
            cmbSport.Name = "cmbSport";
            cmbSport.ShadowDecoration.CustomizableEdges = customizableEdges12;
            cmbSport.Size = new Size(126, 36);
            cmbSport.TabIndex = 88;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Microsoft Sans Serif", 12F);
            label1.ForeColor = Color.FromArgb(21, 95, 40);
            label1.Location = new Point(311, 29);
            label1.Name = "label1";
            label1.Size = new Size(48, 20);
            label1.TabIndex = 83;
            label1.Text = "Filter:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Microsoft Sans Serif", 12F);
            label2.ForeColor = Color.FromArgb(21, 95, 40);
            label2.Location = new Point(27, 29);
            label2.Name = "label2";
            label2.Size = new Size(64, 20);
            label2.TabIndex = 81;
            label2.Text = "Search:";
            // 
            // ManagePlayers
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 611);
            Controls.Add(panel2);
            Controls.Add(panel3);
            Controls.Add(panel1);
            Name = "ManagePlayers";
            Text = "ManagePlayers";
            Load += ManagePlayers_Load;
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPlayers).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel3;
        private Label label13;
        private Panel panel1;
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private Panel panel2;
        private Label label1;
        private Label label2;
        private Guna.UI2.WinForms.Guna2ComboBox cmb2;
        private Guna.UI2.WinForms.Guna2ComboBox cmb1;
        private Guna.UI2.WinForms.Guna2ComboBox cmb3;
        private Guna.UI2.WinForms.Guna2ComboBox cmbSport;
        private Guna.UI2.WinForms.Guna2ComboBox cmbGender;
        private Guna.UI2.WinForms.Guna2ComboBox cmbLevel;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private Guna.UI2.WinForms.Guna2DataGridView dgvPlayers;
    }
}