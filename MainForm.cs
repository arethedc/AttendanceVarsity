using System;
using System.Windows.Forms;

namespace AttendanceVarsity
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Form style
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // or Sizable
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Any initialization logic if needed
        }

        private void btnRegisterPlayer_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var regForm = new RegisterPlayer())
            {
                regForm.ShowDialog();
            }
            this.Show(); // Reshow MainForm after RegisterPlayer is closed
        }

        private void btnCreateSession_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var createSessForm = new CreateSession())
            {
                createSessForm.ShowDialog();
            }
            this.Show();
        }

        private void btnScanAttendance_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var scanForm = new ScanAttendance())
            {
                scanForm.ShowDialog();
            }
            this.Show();
        }

        private void btnManagePlayers_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var manageForm = new ManagePlayers())
            {
                manageForm.ShowDialog();
            }
            this.Show();
        }

        private void btnManageSessions_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var manageSessForm = new ManageSessions())
            {
                manageSessForm.ShowDialog();
            }
            this.Show();
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            // Exit button (or change to your own quit button event name)
            Environment.Exit(0);
        }

        // If you use designer events, you can keep these empty or remove if not used
        private void pa_Paint(object sender, PaintEventArgs e) { }
    }
}
