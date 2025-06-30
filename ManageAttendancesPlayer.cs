using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AttendanceVarsity
{
    public partial class ManageAttendancesPlayer : Form
    {
        private readonly int _playerId;
        private readonly string _playerName;
        private readonly string _studentId;
        private readonly string CONN_STR;

        public ManageAttendancesPlayer(int playerId, string playerName, string studentId)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            _playerId = playerId;
            _playerName = playerName;
            _studentId = studentId;
            CONN_STR = ConfigurationManager.AppSettings["ConnStr"];

            // Setup controls

            btnRefresh.Click += (s, e) => LoadAttendance();
            btnExport.Click += (s, e) => ExportAttendance();
            btnPrint.Click += (s, e) => PrintAttendance();
            cmbStatus.SelectedIndexChanged += (s, e) => LoadAttendance();
        }



        private void LoadStatusFilter()
        {
            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new[] { "All", "Present", "Absent" });
            cmbStatus.SelectedIndex = 0;
        }

        private void LoadAttendance()
        {
            DataTable dt = new DataTable();
            using (var db = new MySqlConnection(CONN_STR))
            {
                db.Open();
                string sql = @"
SELECT
    a.attendance_id,
    s.session_title AS 'Session',
    s.session_date AS 'Date',
    s.start_time AS 'Start',
    s.end_time AS 'End',
    a.status AS 'Status',
    a.time_marked AS 'Time Marked',
    u.full_name AS 'Checked By'
FROM tbl_attendance a
LEFT JOIN tbl_sessions s ON a.session_id = s.session_id
LEFT JOIN tbl_users u ON a.checked_by = u.user_id
WHERE a.player_id = @playerId
";

                if (cmbStatus.SelectedIndex > 0)
                {
                    sql += " AND a.status = @status";
                }

                sql += " ORDER BY s.session_date DESC, s.start_time DESC";

                using var cmd = new MySqlCommand(sql, db);
                cmd.Parameters.AddWithValue("@playerId", _playerId);
                if (cmbStatus.SelectedIndex > 0)
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text);

                using var da = new MySqlDataAdapter(cmd);
                da.Fill(dt);

                dgvPlayerAttendance.Columns.Clear();
                dgvPlayerAttendance.DataSource = dt;

                // Optionally set column headers/format
                dgvPlayerAttendance.Columns["attendance_id"].Visible = false;
                if (dgvPlayerAttendance.Columns.Contains("Date"))
                    dgvPlayerAttendance.Columns["Date"].DefaultCellStyle.Format = "yyyy-MM-dd";
                if (dgvPlayerAttendance.Columns.Contains("Start"))
                    dgvPlayerAttendance.Columns["Start"].DefaultCellStyle.Format = @"hh\:mm";
                if (dgvPlayerAttendance.Columns.Contains("End"))
                    dgvPlayerAttendance.Columns["End"].DefaultCellStyle.Format = @"hh\:mm";
            }
        }

        private void ExportAttendance()
        {
            // Dummy code for export, replace with your export logic
            MessageBox.Show("Export feature not implemented yet.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PrintAttendance()
        {
            // Dummy code for print, replace with your print logic
            MessageBox.Show("Print feature not implemented yet.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ManageAttendancesPlayer_Load(object sender, EventArgs e)
        {
            lblPlayerName.Text = $"Name:{_playerName} Student ID:{_studentId}";
            LoadStatusFilter();
            LoadAttendance();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close(); // This will return to the hidden ManagePlayers form and show it again
        }

    }
}
