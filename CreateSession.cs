using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AttendanceVarsity
{
    public partial class CreateSession : Form
    {
        // ═══════════ CONSTANTS ═══════════
        private static readonly TimeSpan OPEN = TimeSpan.FromHours(6);    // 06:00
        private static readonly TimeSpan CLOSE = TimeSpan.FromHours(22);  // 22:00
        private static readonly TimeSpan GAP = TimeSpan.FromMinutes(10);

        private readonly string CONN_STR;

        // ═══════════ CONSTRUCTOR ═══════════
        public CreateSession()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            txtSessionTitle.ReadOnly = true;
            txtSessionTitle.TabStop = false;

            CONN_STR = ConfigurationManager.AppSettings["ConnStr"];

            LoadGender();
            LoadLevel();
            LoadSports();

            InitDefaults();
            WireEvents();
            ValidateForm(); // initial validation
        }

        // ═══════════ INIT HELPERS ═══════════
        private void InitDefaults()
        {
            DateTime now = RoundUpToNextMinute(DateTime.Now);
            dtpSessionDate.Value = now.Date;
            dtpStartTime.Value = ClampToWindow(now);
            dtpEndTime.Value = dtpStartTime.Value.Add(GAP);

            txtSessionTitle.Text = BuildTitleIfReady();
            ValidateDate();
            ValidateTimes();
            ClearStatus();
        }

        private static DateTime RoundUpToNextMinute(DateTime dt) =>
            new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0).AddMinutes(1);

        private DateTime ClampToWindow(DateTime dt) =>
            dt.TimeOfDay < OPEN ? dt.Date + OPEN :
            dt.TimeOfDay > CLOSE ? dt.Date + CLOSE :
                                   dt;

        // ═══════════ EVENT WIRING ═══════════
        private void WireEvents()
        {
            cmbSport.SelectedIndexChanged += (_, __) => ValidateSport();
            cmbLevel.SelectedIndexChanged += (_, __) => ValidateLevel();
            cmbGender.SelectedIndexChanged += (_, __) => ValidateGender();

            dtpSessionDate.ValueChanged += (_, __) => { ResetTimes(); ValidateDate(); ValidateTimes(); };
            dtpSessionDate.CloseUp += (_, __) => ValidateDate();

            dtpStartTime.ValueChanged += (_, __) =>
            {
                dtpStartTime.Value = ClampToWindow(dtpStartTime.Value);
                dtpEndTime.Value = dtpStartTime.Value.Add(GAP);
                ValidateTimes();
            };
            dtpStartTime.CloseUp += (_, __) => ValidateTimes();

            dtpEndTime.ValueChanged += (_, __) => ValidateTimes();
            dtpEndTime.CloseUp += (_, __) => ValidateTimes();
        }

        private void ResetTimes()
        {
            DateTime baseTime = RoundUpToNextMinute(DateTime.Now);
            if (dtpSessionDate.Value.Date > DateTime.Today)
                baseTime = dtpSessionDate.Value.Date + OPEN;

            dtpStartTime.Value = ClampToWindow(baseTime);
            dtpEndTime.Value = dtpStartTime.Value.Add(GAP);
        }

        // ═══════════ LABEL HELPERS ═══════════
        private void ClearStatus()
        {
            foreach (var lbl in new[]
                     { lblSport, lblLevel, lblGender, lblSessionDate, lblStartTime, lblEndTime, lblSessionTitle })
                lbl.Text = "";
            lblStatus.Text = "";
        }

        private void Set(Label lbl, bool good, string err)
        {
            lbl.Text = good ? "✓" : err;
            lbl.ForeColor = good ? Color.Green : Color.Red;
        }

        // ═══════════ FIELD VALIDATORS ═══════════
        private void ValidateSport() { Set(lblSport, cmbSport.SelectedIndex > 0, "Pick sport"); ValidateForm(); }
        private void ValidateLevel() { Set(lblLevel, cmbLevel.SelectedIndex > 0, "Pick level"); ValidateForm(); }
        private void ValidateGender() { Set(lblGender, cmbGender.SelectedIndex > 0, "Pick gender"); ValidateForm(); }
        private void ValidateDate()
        {
            Set(lblSessionDate, dtpSessionDate.Value.Date >= DateTime.Today, "Past date");
            ValidateForm();
        }

        private void ValidateTimes()
        {
            bool inWindow = dtpStartTime.Value.TimeOfDay >= OPEN &&
                            dtpEndTime.Value.TimeOfDay <= CLOSE;

            bool chrono = dtpEndTime.Value > dtpStartTime.Value;
            bool gapOK = chrono && (dtpEndTime.Value - dtpStartTime.Value) >= GAP;
            bool startFuture = !(dtpSessionDate.Value.Date == DateTime.Today &&
                                 dtpStartTime.Value < DateTime.Now);

            bool clash = false;
            if (inWindow && chrono && gapOK && startFuture &&
                cmbSport.SelectedIndex > 0 &&
                cmbLevel.SelectedIndex > 0 &&
                cmbGender.SelectedIndex > 0)
                clash = TimeOverlapExists();

            string startErr = "", endErr = "";
            if (!inWindow) { startErr = "6AM–10PM only"; endErr = "6AM–10PM"; }
            else if (!startFuture) { startErr = "Past time"; }
            else if (!chrono) { startErr = "Before end"; endErr = "After start"; }
            else if (!gapOK) { endErr = "≥10 min gap"; }
            else if (clash) { endErr = "Slot taken"; }

            Set(lblStartTime, startErr == "", startErr);
            Set(lblEndTime, endErr == "", endErr);
            ValidateForm();
        }

        // ═══════════ FORM VALIDATOR ═══════════
        private void ValidateForm()
        {
            txtSessionTitle.Text = BuildTitleIfReady();

            bool dup = txtSessionTitle.Text != "" && SessionExists();
            if (dup) Set(lblSessionTitle, false, "Session already exists");
            else if (txtSessionTitle.Text != "")
                Set(lblSessionTitle, true, "✓");
            else lblSessionTitle.Text = "";

            bool allGreen =
                lblSport.Text == "✓" &&
                lblLevel.Text == "✓" &&
                lblGender.Text == "✓" &&
                lblSessionDate.Text == "✓" &&
                lblStartTime.Text == "✓" &&
                lblEndTime.Text == "✓" &&
                lblSessionTitle.Text == "✓";

            btnCreate.Enabled = true;
            lblStatus.Text = allGreen ? "" : "";
        }

        // ═══════════ DUPLICATE CHECK ═══════════
        private bool SessionExists()
        {
            const string SQL = @"
                SELECT 1 FROM tbl_sessions
                WHERE sport_id     = @sport
                  AND level        = @level
                  AND gender       = @gender
                  AND session_date = @date
                  AND start_time   = @start
                LIMIT 1";

            using var db = new MySqlConnection(CONN_STR);
            db.Open();
            using var cmd = new MySqlCommand(SQL, db);
            cmd.Parameters.AddWithValue("@sport", cmbSport.SelectedValue);
            cmd.Parameters.AddWithValue("@level", cmbLevel.Text);
            cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
            cmd.Parameters.AddWithValue("@date", dtpSessionDate.Value.Date);
            cmd.Parameters.AddWithValue("@start", dtpStartTime.Value.TimeOfDay);
            return cmd.ExecuteScalar() != null;
        }

        // ═══════════ SAVE BUTTON ═══════════
        private async void btnCreate_Click(object sender, EventArgs e)
        {
            if (!FormIsValid(out string msg))
            {
                lblStatus.Text = msg;
                return;
            }

            const string INSERT = @"
              INSERT INTO tbl_sessions
                (session_title, level, sport_id, gender,
                 session_date, start_time, end_time)
              VALUES
                (@title, @level, @sport, @gender,
                 @date, @start, @end)";

            try
            {
                await using var db = new MySqlConnection(CONN_STR);
                await db.OpenAsync();
                await using var cmd = new MySqlCommand(INSERT, db);
                cmd.Parameters.AddWithValue("@title", BuildTitle());
                cmd.Parameters.AddWithValue("@level", cmbLevel.Text);
                cmd.Parameters.AddWithValue("@sport", cmbSport.SelectedValue);
                cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
                cmd.Parameters.AddWithValue("@date", dtpSessionDate.Value.Date);
                cmd.Parameters.AddWithValue("@start", dtpStartTime.Value.TimeOfDay);
                cmd.Parameters.AddWithValue("@end", dtpEndTime.Value.TimeOfDay);
                await cmd.ExecuteNonQueryAsync();

                MessageBox.Show("Session saved!");
                ResetForm();
                ClearStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DB error: {ex.Message}");
            }
        }

        // ═══════════ FINAL BLOCKING VALIDATION ═══════════
        private bool FormIsValid(out string error)
        {
            error = "";

            if (cmbSport.SelectedIndex == 0 ||
                cmbLevel.SelectedIndex == 0 ||
                cmbGender.SelectedIndex == 0)
            { error = "Select sport, level, and gender."; return false; }

            if (dtpSessionDate.Value.Date < DateTime.Today)
            { error = "Date cannot be in the past."; return false; }

            if (dtpSessionDate.Value.Date == DateTime.Today &&
                dtpStartTime.Value < DateTime.Now)
            { error = "Start time must be in the future."; return false; }

            if (dtpStartTime.Value.TimeOfDay < OPEN ||
                dtpEndTime.Value.TimeOfDay > CLOSE)
            { error = "Time must be between 6 AM and 10 PM."; return false; }

            if (dtpEndTime.Value <= dtpStartTime.Value ||
               (dtpEndTime.Value - dtpStartTime.Value) < GAP)
            { error = "End time must be at least 10 minutes after start."; return false; }

            if (TimeOverlapExists())
            { error = "Another session overlaps this time."; return false; }

            if (SessionExists())
            { error = "A session with these details already exists."; return false; }

            return true;
        }

        // ═══════════ TITLE BUILDERS ═══════════
        private string BuildTitle() =>
            $"{cmbSport.Text} - {cmbLevel.Text} - {cmbGender.Text} - " +
            $"{dtpSessionDate.Value:yyyy-MM-dd HHmm}-{dtpEndTime.Value:HHmm}";

        private string BuildTitleIfReady() =>
            (cmbSport.SelectedIndex > 0 &&
             cmbLevel.SelectedIndex > 0 &&
             cmbGender.SelectedIndex > 0) ? BuildTitle() : "";

        // ═══════════ COMBOBOX LOADERS ═══════════
        private void LoadGender()
        {
            cmbGender.Items.Clear();
            cmbGender.Items.Add("-- Select Gender --");
            cmbGender.Items.AddRange(new[] { "Male", "Female", "Both" });
            cmbGender.SelectedIndex = 0;
        }

        private void LoadLevel()
        {
            cmbLevel.Items.Clear();
            cmbLevel.Items.Add("-- Select Level --");
            cmbLevel.Items.AddRange(new[] { "High School", "College" });
            cmbLevel.SelectedIndex = 0;
        }

        private void LoadSports()
        {
            using var db = new MySqlConnection(CONN_STR);
            db.Open();
            var da = new MySqlDataAdapter(
                "SELECT sport_id, sport_name FROM tbl_sports ORDER BY sport_name", db);
            var dt = new DataTable();
            da.Fill(dt);
            var dummy = dt.NewRow();
            dummy["sport_id"] = DBNull.Value;
            dummy["sport_name"] = "-- Select Sport --";
            dt.Rows.InsertAt(dummy, 0);

            cmbSport.DataSource = dt;
            cmbSport.DisplayMember = "sport_name";
            cmbSport.ValueMember = "sport_id";
            cmbSport.SelectedIndex = 0;
        }

        /// <summary>
        /// Returns true if another session of the same sport & level AND a
        /// compatible gender overlaps the requested time window on the same date.
        /// </summary>
        private bool TimeOverlapExists()
        {
            const string SQL = @"
        SELECT 1
        FROM   tbl_sessions
        WHERE  sport_id = @sport
          AND  level    = @level
          AND  ( gender = @gender
                 OR gender = 'Both'
                 OR @gender = 'Both' )
          AND  session_date = @date
          AND  @newStart < end_time
          AND  @newEnd   > start_time
        LIMIT 1";

            using var db = new MySqlConnection(CONN_STR);
            db.Open();
            using var cmd = new MySqlCommand(SQL, db);
            cmd.Parameters.AddWithValue("@sport", cmbSport.SelectedValue);
            cmd.Parameters.AddWithValue("@level", cmbLevel.Text);
            cmd.Parameters.AddWithValue("@gender", cmbGender.Text);
            cmd.Parameters.AddWithValue("@date", dtpSessionDate.Value.Date);
            cmd.Parameters.AddWithValue("@newStart", dtpStartTime.Value.TimeOfDay);
            cmd.Parameters.AddWithValue("@newEnd", dtpEndTime.Value.TimeOfDay);
            return cmd.ExecuteScalar() != null;
        }

        // ═══════════ RESET & NAVIGATION ═══════════
        private void ResetForm()
        {
            cmbSport.SelectedIndex = 0;
            cmbLevel.SelectedIndex = 0;
            cmbGender.SelectedIndex = 0;
            InitDefaults();
        }

        private void btnClear_Click(object sender, EventArgs e) => ResetForm();
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close(); // Closes the current RegisterPlayer form and returns control to MainForm
        }
        private void cmbSport_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e) { }
        private void CreateSession_Load(object sender, EventArgs e) { }
        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}
