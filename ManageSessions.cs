using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AttendanceVarsity
{
    public partial class ManageSessions : Form
    {
        private readonly string CONN_STR;
        private DataTable sportsTable;

        public ManageSessions()
        {
            InitializeComponent();
            rbAll.Checked = true;
            dtpSessionDateFrom.Enabled = false;
            dtpSessionDateTo.Enabled = false;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            CONN_STR = ConfigurationManager.AppSettings["ConnStr"];
            dgvSessions.EditMode = DataGridViewEditMode.EditProgrammatically;

            Load += ManageSessions_Load;
            btnRefresh.Click += (s, e) => LoadSessions();
            txtSearch.TextChanged += (s, e) => LoadSessions();

            cmbSport.SelectedIndexChanged += (s, e) => LoadSessions();
            cmbLevel.SelectedIndexChanged += (s, e) => LoadSessions();
            cmbGender.SelectedIndexChanged += (s, e) => LoadSessions();

            rbAll.CheckedChanged += (s, e) => ToggleDatePickers();
            rbUpcoming.CheckedChanged += (s, e) => ToggleDatePickers();
            rbByDate.CheckedChanged += (s, e) => ToggleDatePickers();
            chkViewAttendance.CheckedChanged += (s, e) => LoadSessions();

            dtpSessionDateFrom.ValueChanged += (s, e) =>
            {
                if (dtpSessionDateTo.Value < dtpSessionDateFrom.Value)
                    dtpSessionDateTo.Value = dtpSessionDateFrom.Value;
                if (!dtpSessionDateTo.Focused)
                    dtpSessionDateTo.Value = dtpSessionDateFrom.Value;
                if (rbByDate.Checked)
                    LoadSessions();
            };

            dtpSessionDateTo.ValueChanged += (s, e) =>
            {
                if (dtpSessionDateTo.Value < dtpSessionDateFrom.Value)
                    dtpSessionDateTo.Value = dtpSessionDateFrom.Value;
                if (rbByDate.Checked)
                    LoadSessions();
            };

            dgvSessions.CellContentClick += dgvSessions_CellContentClick;
        }

        // Enable/disable date pickers based on filter
        private void ToggleDatePickers()
        {
            bool enable = rbByDate.Checked;
            dtpSessionDateFrom.Enabled = enable;
            dtpSessionDateTo.Enabled = enable;
            if (enable)
            {
                dtpSessionDateFrom.Value = DateTime.Today;
                dtpSessionDateTo.Value = DateTime.Today;
            }
            LoadSessions();
        }

        private void ManageSessions_Load(object sender, EventArgs e)
        {
            LoadFilterOptions();
            LoadSessions();
            rbAll.Checked = true;
        }

        private void LoadFilterOptions()
        {
            using var db = new MySqlConnection(CONN_STR);
            db.Open();
            var da = new MySqlDataAdapter("SELECT sport_id, sport_name FROM tbl_sports ORDER BY sport_name", db);
            sportsTable = new DataTable();
            da.Fill(sportsTable);

            // Sport filter
            var all = sportsTable.NewRow();
            all["sport_id"] = DBNull.Value;
            all["sport_name"] = "All Sports";
            sportsTable.Rows.InsertAt(all, 0);

            cmbSport.DataSource = sportsTable;
            cmbSport.DisplayMember = "sport_name";
            cmbSport.ValueMember = "sport_id";
            cmbSport.SelectedIndex = 0;

            // Gender & Level
            cmbGender.Items.Clear();
            cmbGender.Items.AddRange(new[] { "All Genders", "Male", "Female", "Both" });
            cmbGender.SelectedIndex = 0;

            cmbLevel.Items.Clear();
            cmbLevel.Items.AddRange(new[] { "All Levels", "High School", "College" });
            cmbLevel.SelectedIndex = 0;

            dtpSessionDateFrom.Value = DateTime.Today;
            dtpSessionDateTo.Value = DateTime.Today;
        }

        private void LoadSessions()
        {
            DataTable dt = new DataTable();
            using (var db = new MySqlConnection(CONN_STR))
            {
                db.Open();

                string sql = @"SELECT s.session_id, s.session_title, s.level, sp.sport_name, s.gender, 
                              s.session_date, s.start_time, s.end_time
                       FROM tbl_sessions s
                       LEFT JOIN tbl_sports sp ON s.sport_id = sp.sport_id
                       WHERE (@sport IS NULL OR s.sport_id = @sport)
                         AND (@gender = '' OR s.gender = @gender)
                         AND (@level = '' OR s.level = @level)
                         AND (s.session_title LIKE @title)
                         {0}
                       ORDER BY s.session_date DESC, s.start_time DESC";

                string extraFilter = "";

                if (rbUpcoming.Checked)
                {
                    extraFilter = "AND s.session_date >= CURDATE()";
                }
                else if (rbByDate.Checked)
                {
                    extraFilter = "AND s.session_date BETWEEN @dateFrom AND @dateTo";
                }

                // Filter for attendance only
                if (chkViewAttendance.Checked)
                {
                    extraFilter += " AND EXISTS (SELECT 1 FROM tbl_attendance a WHERE a.session_id = s.session_id)";
                }

                sql = string.Format(sql, extraFilter);

                var cmd = new MySqlCommand(sql, db);

                cmd.Parameters.AddWithValue("@sport", cmbSport.SelectedIndex > 0 ? cmbSport.SelectedValue : DBNull.Value);
                cmd.Parameters.AddWithValue("@gender", cmbGender.SelectedIndex > 0 ? cmbGender.Text : "");
                cmd.Parameters.AddWithValue("@level", cmbLevel.SelectedIndex > 0 ? cmbLevel.Text : "");
                string searchText = txtSearch.Text.Trim();
                cmd.Parameters.AddWithValue("@title", "%" + searchText + "%");

                if (rbByDate.Checked)
                {
                    cmd.Parameters.AddWithValue("@dateFrom", dtpSessionDateFrom.Value.Date);
                    cmd.Parameters.AddWithValue("@dateTo", dtpSessionDateTo.Value.Date);
                }

                var da = new MySqlDataAdapter(cmd);
                da.Fill(dt);

                dgvSessions.Columns.Clear();
                dgvSessions.DataSource = dt;

                dgvSessions.Columns["session_id"].HeaderText = "Session ID";
                dgvSessions.Columns["session_title"].HeaderText = "Title";
                dgvSessions.Columns["sport_name"].HeaderText = "Sport";
                dgvSessions.Columns["level"].HeaderText = "Level";
                dgvSessions.Columns["gender"].HeaderText = "Gender";
                dgvSessions.Columns["session_date"].HeaderText = "Date";
                dgvSessions.Columns["start_time"].HeaderText = "Start";
                dgvSessions.Columns["end_time"].HeaderText = "End";

                // Add/Delete View Attendance button as needed
                if (chkViewAttendance.Checked)
                {
                    if (!dgvSessions.Columns.Contains("btnViewAttendance"))
                    {
                        var btnView = new DataGridViewButtonColumn
                        {
                            Name = "btnViewAttendance",
                            HeaderText = "View Attendance",
                            Text = "View",
                            UseColumnTextForButtonValue = true
                        };
                        dgvSessions.Columns.Add(btnView);
                    }
                }
                else
                {
                    if (dgvSessions.Columns.Contains("btnViewAttendance"))
                        dgvSessions.Columns.Remove("btnViewAttendance");
                }

                // Add Delete button column if not exists
                if (!dgvSessions.Columns.Contains("btnDelete"))
                {
                    var btnDel = new DataGridViewButtonColumn
                    {
                        Name = "btnDelete",
                        HeaderText = "Delete",
                        Text = "Delete",
                        UseColumnTextForButtonValue = true
                    };
                    dgvSessions.Columns.Add(btnDel);
                }
            }
        }

        private void dgvSessions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Handle View Attendance
            if (dgvSessions.Columns[e.ColumnIndex].Name == "btnViewAttendance")
            {
                int sessionId = Convert.ToInt32(dgvSessions.Rows[e.RowIndex].Cells["session_id"].Value);
                string title = dgvSessions.Rows[e.RowIndex].Cells["session_title"].Value.ToString();
                DateTime sessionDate = Convert.ToDateTime(dgvSessions.Rows[e.RowIndex].Cells["session_date"].Value);

                this.Hide();
                var attendanceForm = new ManageAttendancesSession(sessionId, title, sessionDate);
                attendanceForm.ShowDialog();
                this.Show(); // This will reshow your current form after the dialog closes

            }

            // Handle Delete
            if (dgvSessions.Columns[e.ColumnIndex].Name == "btnDelete")
            {
                int sessionId = Convert.ToInt32(dgvSessions.Rows[e.RowIndex].Cells["session_id"].Value);
                string title = dgvSessions.Rows[e.RowIndex].Cells["session_title"].Value.ToString();
                DateTime sessionDate = Convert.ToDateTime(dgvSessions.Rows[e.RowIndex].Cells["session_date"].Value);

                // 1. Block deleting past sessions
                if (sessionDate < DateTime.Today)
                {
                    MessageBox.Show(
                        $"Session '{title}' is in the past and cannot be deleted.",
                        "Delete Not Allowed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // 2. Block deleting sessions that have attendance
                if (SessionHasAttendance(sessionId))
                {
                    MessageBox.Show(
                        $"Session '{title}' has attendance records and cannot be deleted.",
                        "Delete Not Allowed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // 3. Safe to delete (future session, no attendance)
                var result = MessageBox.Show(
                    $"Are you sure you want to delete session '{title}'?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DeleteSession(sessionId);
                    LoadSessions();
                }
            }
        }

        private bool SessionHasAttendance(int sessionId)
        {
            using var db = new MySqlConnection(CONN_STR);
            db.Open();
            using var cmd = new MySqlCommand("SELECT COUNT(*) FROM tbl_attendance WHERE session_id=@id", db);
            cmd.Parameters.AddWithValue("@id", sessionId);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }

        private void DeleteSession(int sessionId)
        {
            using var db = new MySqlConnection(CONN_STR);
            db.Open();
            using var cmd = new MySqlCommand("DELETE FROM tbl_sessions WHERE session_id=@id", db);
            cmd.Parameters.AddWithValue("@id", sessionId);
            cmd.ExecuteNonQuery();

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close(); // Closes the current RegisterPlayer form and returns control to MainForm

        }
    }
}


