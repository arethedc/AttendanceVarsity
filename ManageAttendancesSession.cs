using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AttendanceVarsity
{
    public partial class ManageAttendancesSession : Form
    {
        private readonly int _sessionId;
        private readonly string _sessionTitle;
        private readonly string CONN_STR;

        public ManageAttendancesSession(int sessionId, string sessionTitle, DateTime sessionDate)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            _sessionId = sessionId;
            _sessionTitle = sessionTitle;
            CONN_STR = ConfigurationManager.AppSettings["ConnStr"];

            // Layout events
            this.Load += ManageAttendancesSession_Load;
            cmbStatus.SelectedIndexChanged += (s, e) => LoadAttendance();
            txtSearch.TextChanged += (s, e) => LoadAttendance();
            btnRefresh.Click += (s, e) => LoadAttendance();
            btnExport.Click += (s, e) => ExportAttendance();
            btnPrint.Click += (s, e) => PrintAttendance();

            lblSessionName.Text = _sessionTitle;
        }

        private void ManageAttendancesSession_Load(object sender, EventArgs e)
        {
            // Populate status filter
            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new[] { "All", "Present", "Absent", "Pending" });
            cmbStatus.SelectedIndex = 0;

            LoadAttendance();
        }

        private void LoadAttendance()
        {
            DataTable dt = new DataTable();
            using (var db = new MySqlConnection(CONN_STR))
            {
                db.Open();
                string sql = @"
                    SELECT
                        p.full_name AS 'Player Name',
                        p.student_id AS 'Student ID',
                        IFNULL(a.status, 'Absent') AS 'Status',
                        IFNULL(DATE_FORMAT(a.time_marked, '%H:%i'), '') AS 'Time Marked'
                    FROM tbl_players p
                    LEFT JOIN tbl_attendance a
                        ON p.player_id = a.player_id AND a.session_id = @sessionId
                    WHERE p.sport_id = (
                        SELECT sport_id FROM tbl_sessions WHERE session_id=@sessionId
                    )
                      AND p.level = (SELECT level FROM tbl_sessions WHERE session_id=@sessionId)
                      AND p.gender = (SELECT gender FROM tbl_sessions WHERE session_id=@sessionId)
                ";

                // Status filter
                if (cmbStatus.SelectedIndex > 0)
                {
                    sql += " AND IFNULL(a.status, 'Absent') = @status";
                }

                // Search filter
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    sql += " AND (p.full_name LIKE @search OR p.student_id LIKE @search)";
                }

                sql += " ORDER BY p.full_name";

                var cmd = new MySqlCommand(sql, db);
                cmd.Parameters.AddWithValue("@sessionId", _sessionId);
                if (cmbStatus.SelectedIndex > 0)
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text.Trim() + "%");

                var da = new MySqlDataAdapter(cmd);
                da.Fill(dt);

                dgvSessionAttendance.Columns.Clear();
                dgvSessionAttendance.DataSource = dt;
            }
        }

        private void ExportAttendance()
        {
            // Placeholder for export code
            MessageBox.Show("Export to Excel/CSV not yet implemented.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PrintAttendance()
        {
            // Placeholder for print code
            MessageBox.Show("Print not yet implemented.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close(); // This will return to the hidden ManagePlayers form and show it again
        }

    }
}
