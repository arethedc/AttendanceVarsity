using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Face;

namespace AttendanceVarsity
{
    public partial class ScanAttendance : Form
    {
        // --- Runtime fields (unchanged) ---
        private System.Windows.Forms.Timer scanTimer;
        private System.Windows.Forms.Timer sessionEndTimer;
        private VideoCapture _cam;
        private CascadeClassifier _faceDet;
        private LBPHFaceRecognizer _recognizer;

        private string CONN_STR, CASCADE_FACE, TOOLS_FOLDER, FACE_ROOT;
        private DataTable _activeSessions;

        // Feedback throttle
        private DateTime _lastFeedback = DateTime.MinValue;
        private int? _lastLabel = null;
        private string _lastStatus = "";

        private bool _scanProcessing = false; // prevent timer reentrancy

        public ScanAttendance()
        {
            InitializeComponent();
            this.MinimumSize = new Size(820, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            pictureBoxCam.SizeMode = PictureBoxSizeMode.StretchImage;

            // Load config
            CONN_STR = ConfigurationManager.AppSettings["ConnStr"];
            CASCADE_FACE = ConfigurationManager.AppSettings["CascadeFace"];
            TOOLS_FOLDER = ConfigurationManager.AppSettings["ToolsFolder"];
            FACE_ROOT = ConfigurationManager.AppSettings["FaceRoot"];

            // Set up camera, model, timers
            SetupCamera();
            LoadRecognizer();
            LoadActiveSessions();

            scanTimer = new System.Windows.Forms.Timer();
            scanTimer.Interval = 1400;
            scanTimer.Tick += ScanTimer_Tick;

            sessionEndTimer = new System.Windows.Forms.Timer();
            sessionEndTimer.Interval = 30000;
            sessionEndTimer.Tick += SessionEndTimer_Tick;

            if (HasActiveSession())
            {
                scanTimer.Start();
                sessionEndTimer.Start();
            }
            else
            {
                SetScanUIEnabled(false);
            }
        }

        // --- Camera/model setup ---
        private void SetupCamera()
        {
            _faceDet = new CascadeClassifier(CASCADE_FACE);
            _cam = new VideoCapture(0, VideoCapture.API.DShow);
        }
        private void LoadRecognizer()
        {
            string modelPath = Path.Combine(TOOLS_FOLDER, "trained_model.yml");
            if (!File.Exists(modelPath))
            {
                lblStatus.Text = "Trained model not found!";
                SetScanUIEnabled(false);
                return;
            }
            _recognizer = new LBPHFaceRecognizer();
            _recognizer.Read(modelPath);
        }

        // --- Session state ---
        private void LoadActiveSessions()
        {
            using var db = new MySqlConnection(CONN_STR);
            db.Open();
            string sql = @"
                SELECT * FROM tbl_sessions
                WHERE session_date = CURDATE()
                AND start_time <= CURTIME()
                AND end_time > CURTIME()
                AND (force_ended IS NULL OR force_ended = 0)";
            var da = new MySqlDataAdapter(sql, db);
            _activeSessions = new DataTable();
            da.Fill(_activeSessions);

            if (_activeSessions.Rows.Count == 0)
            {
                lblStatus.Text = "No active session right now.";
                lblSessionInfo.Text = "";
                lblBigStatus.Text = "";
                lblStudentInfo.Text = "";
                panelFeedback.BackColor = Color.White;
                pictureBoxCam.Image = null;
                progressBar.Value = 0;
                SetScanUIEnabled(false);
                scanTimer?.Stop();
                sessionEndTimer?.Stop();
            }
            else
            {
                DataRow session = _activeSessions.Rows[0];
                lblSessionInfo.Text = $"Session: {session["session_title"]}";
                lblStatus.Text = "Ready for attendance.";
                progressBar.Value = 0;
                SetScanUIEnabled(true);
                scanTimer?.Start();
                sessionEndTimer?.Start();
            }
        }

        private bool HasActiveSession() => _activeSessions != null && _activeSessions.Rows.Count > 0;

        // --- UI toggling ---
        private void SetScanUIEnabled(bool enabled)
        {
            pictureBoxCam.Enabled = enabled;
            panelFeedback.Enabled = enabled;
            btnForceEndSession.Enabled = enabled && HasActiveSession();
            lblBigStatus.Visible = enabled;
            lblStudentInfo.Visible = enabled;
            progressBar.Visible = enabled;
        }

        // --- Main scan timer tick (async, non-blocking) ---
        private async void ScanTimer_Tick(object sender, EventArgs e)
        {
            if (!HasActiveSession()) return;
            if (_scanProcessing) return; // prevent reentry while still working
            if ((DateTime.Now - _lastFeedback).TotalSeconds < 1.3) return;

            _scanProcessing = true;
            try
            {
                using Mat m = new();
                _cam.Read(m);
                if (m.IsEmpty)
                {
                    DisplayFeedback("No camera feed.", Color.Red, "", "", Color.White);
                    _lastLabel = null; _lastStatus = "";
                    _scanProcessing = false;
                    return;
                }

                var img = m.ToImage<Bgr, byte>();
                var gray = img.Convert<Gray, byte>();
                var faces = _faceDet.DetectMultiScale(gray, 1.1, 5);

                if (faces.Length == 0)
                {
                    DisplayFeedback("Ready for attendance.", Color.Black, "", "", Color.White);
                    _lastLabel = null; _lastStatus = "";
                }
                else if (faces.Length > 1)
                {
                    DisplayFeedback("More than one face detected.", Color.DarkOrange,
                        "MULTIPLE FACES!", "Ensure only one person is visible.", Color.Yellow, "multiple_faces");
                }
                else
                {
                    var face = faces[0];
                    img.Draw(face, new Bgr(Color.Lime), 2);
                    int centerX = img.Width / 2, centerY = img.Height / 2;
                    int faceCenterX = face.X + face.Width / 2;
                    int faceCenterY = face.Y + face.Height / 2;
                    if (Math.Abs(faceCenterX - centerX) > img.Width * 0.22 || Math.Abs(faceCenterY - centerY) > img.Height * 0.22)
                    {
                        DisplayFeedback("Face not centered.", Color.DarkOrange,
                            "ALIGN FACE", "Center your face in the camera.", Color.Orange, "not_centered");
                        pictureBoxCam.Image = img.ToBitmap();
                        _scanProcessing = false;
                        return;
                    }

                    var roi = gray.Copy(face).Resize(200, 200, Emgu.CV.CvEnum.Inter.Cubic);
                    var result = _recognizer.Predict(roi);

                    if (result.Label == -1 || result.Distance >= 60)
                    {
                        DisplayFeedback("Face not recognized.", Color.Red,
                            "NOT ALLOWED", "No Information Detected", Color.Red, "not_allowed");
                        _lastLabel = null;
                    }
                    else
                    {
                        string studentId = GetStudentIdFromLabel(result.Label);
                        await Task.Run(() => HandleRecognition(img, result.Label, studentId));
                    }
                }
                pictureBoxCam.Image = img.ToBitmap();
            }
            finally
            {
                _scanProcessing = false;
            }
        }

        // --- Display helper: only update UI if status or label changes ---
        private void DisplayFeedback(string status, Color statusColor, string bigStatus, string studentInfo, Color panelColor, string state = null)
        {
            if (_lastStatus == state && lblStatus.Text == status && lblStudentInfo.Text == studentInfo)
                return;

            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    lblStatus.Text = status;
                    lblStatus.ForeColor = statusColor;
                    lblBigStatus.Text = bigStatus;
                    lblBigStatus.BackColor = panelColor;
                    lblStudentInfo.Text = studentInfo;
                    panelFeedback.BackColor = panelColor;
                    progressBar.Value = (state == "now_present" || state == "already_present") ? 100 : 0;
                }));
            }
            else
            {
                lblStatus.Text = status;
                lblStatus.ForeColor = statusColor;
                lblBigStatus.Text = bigStatus;
                lblBigStatus.BackColor = panelColor;
                lblStudentInfo.Text = studentInfo;
                panelFeedback.BackColor = panelColor;
                progressBar.Value = (state == "now_present" || state == "already_present") ? 100 : 0;
            }

            _lastStatus = state ?? status;
            _lastFeedback = DateTime.Now;
        }

        // --- Recognition and attendance logic (runs in Task) ---
        private void HandleRecognition(Image<Bgr, byte> img, int label, string studentId)
        {
            if (_lastLabel == label && (_lastStatus == "already_present" || _lastStatus == "now_present")) return;

            try
            {
                using var db = new MySqlConnection(CONN_STR);
                db.Open();
                var cmd = new MySqlCommand("SELECT * FROM tbl_players WHERE student_id=@id", db);
                cmd.Parameters.AddWithValue("@id", studentId);
                var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    DisplayFeedback("Face not recognized.", Color.Red, "NOT ALLOWED", "No Information Detected", Color.Red, "not_found");
                    _lastLabel = null;
                    return;
                }
                int playerId = reader.GetInt32("player_id");
                string gender = reader.GetString("gender");
                string level = reader.GetString("level");
                int sportId = reader.GetInt32("sport_id");
                reader.Close();

                bool eligible = false;
                int eligibleSessionId = -1;
                foreach (DataRow session in _activeSessions.Rows)
                {
                    if ((session["gender"].ToString() == gender || session["gender"].ToString() == "Both")
                        && session["level"].ToString() == level
                        && (int)session["sport_id"] == sportId)
                    {
                        eligible = true;
                        eligibleSessionId = (int)session["session_id"];
                        break;
                    }
                }

                if (!eligible)
                {
                    DisplayFeedback("You are not allowed for this session.", Color.Red, "NOT ALLOWED",
                        $"Student ID: {studentId}", Color.Red, "not_eligible");
                    _lastLabel = label;
                    return;
                }

                var cmd2 = new MySqlCommand("SELECT status FROM tbl_attendance WHERE player_id=@pid AND session_id=@sid", db);
                cmd2.Parameters.AddWithValue("@pid", playerId);
                cmd2.Parameters.AddWithValue("@sid", eligibleSessionId);
                var attendanceStatus = cmd2.ExecuteScalar();

                if (attendanceStatus != null && attendanceStatus.ToString() == "Present")
                {
                    DisplayFeedback("You have already been marked present for this session.", Color.Green,
                        "PRESENT", $"Student ID: {studentId}", Color.Green, "already_present");
                }
                else
                {
                    // Mark attendance, play beep ONCE
                    string attDir = Path.Combine(FACE_ROOT, studentId, "Attendance");
                    Directory.CreateDirectory(attDir);
                    string imgPath = Path.Combine(attDir, $"{DateTime.Now:yyyyMMdd_HHmmss}.jpg");
                    img.ToBitmap().Save(imgPath);

                    var cmd3 = new MySqlCommand(
                        @"INSERT INTO tbl_attendance (player_id, session_id, status, image_path)
                          VALUES (@pid, @sid, 'Present', @img)", db);
                    cmd3.Parameters.AddWithValue("@pid", playerId);
                    cmd3.Parameters.AddWithValue("@sid", eligibleSessionId);
                    cmd3.Parameters.AddWithValue("@img", imgPath);
                    cmd3.ExecuteNonQuery();

                    DisplayFeedback("Attendance marked.", Color.Green, "PRESENT",
                        $"Student ID: {studentId}", Color.Green, "now_present");
                    System.Media.SystemSounds.Beep.Play();
                }
                _lastLabel = label;
            }
            catch (Exception ex)
            {
                DisplayFeedback("Database error: " + ex.Message, Color.Red, "ERROR", "", Color.Red, "db_error");
                _lastLabel = null;
            }
        }

        // --- Mark absentees at session end ---
        private void MarkAbsenteesForSession(int sessionId)
        {
            using var db = new MySqlConnection(CONN_STR);
            db.Open();

            var cmdSession = new MySqlCommand("SELECT * FROM tbl_sessions WHERE session_id=@sid", db);
            cmdSession.Parameters.AddWithValue("@sid", sessionId);
            using var reader = cmdSession.ExecuteReader();
            if (!reader.Read()) return;
            string sessionGender = reader.GetString("gender");
            string sessionLevel = reader.GetString("level");
            int sessionSportId = reader.GetInt32("sport_id");
            reader.Close();

            string genderCond = (sessionGender == "Both")
                ? "(gender='Male' OR gender='Female')"
                : "gender=@gender";
            string sql = $"SELECT player_id FROM tbl_players WHERE {genderCond} AND level=@level AND sport_id=@sport";
            var cmdPlayers = new MySqlCommand(sql, db);
            if (sessionGender != "Both") cmdPlayers.Parameters.AddWithValue("@gender", sessionGender);
            cmdPlayers.Parameters.AddWithValue("@level", sessionLevel);
            cmdPlayers.Parameters.AddWithValue("@sport", sessionSportId);

            var absentPlayers = new List<int>();
            using (var rdr = cmdPlayers.ExecuteReader())
                while (rdr.Read())
                    absentPlayers.Add(rdr.GetInt32("player_id"));

            foreach (int pid in absentPlayers)
            {
                var cmdCheck = new MySqlCommand(
                    "SELECT 1 FROM tbl_attendance WHERE player_id=@pid AND session_id=@sid", db);
                cmdCheck.Parameters.AddWithValue("@pid", pid);
                cmdCheck.Parameters.AddWithValue("@sid", sessionId);
                var result = cmdCheck.ExecuteScalar();
                if (result == null)
                {
                    var cmdInsert = new MySqlCommand(
                        "INSERT INTO tbl_attendance (player_id, session_id, status) VALUES (@pid, @sid, 'Absent')", db);
                    cmdInsert.Parameters.AddWithValue("@pid", pid);
                    cmdInsert.Parameters.AddWithValue("@sid", sessionId);
                    cmdInsert.ExecuteNonQuery();
                }
            }
        }

        // --- Map face label to student ID ---
        private string GetStudentIdFromLabel(int label)
        {
            string mapPath = Path.Combine(TOOLS_FOLDER, "labels.csv");
            if (!File.Exists(mapPath)) return "";
            foreach (var line in File.ReadAllLines(mapPath))
            {
                var parts = line.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[0], out int lbl) && lbl == label)
                    return parts[1];
            }
            return "";
        }

        // --- Session end logic ---
        private void SessionEndTimer_Tick(object sender, EventArgs e)
        {
            if (!HasActiveSession())
                return;

            DataRow session = _activeSessions.Rows[0];
            TimeSpan endTimeSpan = (TimeSpan)session["end_time"];
            DateTime endTime = DateTime.Today.Add(endTimeSpan);
            if (DateTime.Now >= endTime)
            {
                int sessionId = Convert.ToInt32(session["session_id"]);
                MarkAbsenteesForSession(sessionId);
                lblStatus.Text = "Session ended. Absentees marked.";
                scanTimer?.Stop();
                sessionEndTimer?.Stop();
                btnForceEndSession.Enabled = false;
                btnBack.Enabled = true;
                pictureBoxCam.Image = null;
                lblBigStatus.Text = "SESSION ENDED";
                lblBigStatus.BackColor = Color.DarkGray;
                lblBigStatus.ForeColor = Color.White;
                panelFeedback.BackColor = Color.DarkGray;
                using (var db = new MySqlConnection(CONN_STR))
                {
                    db.Open();
                    var cmd = new MySqlCommand(
                        "UPDATE tbl_sessions SET force_ended=1 WHERE session_id=@sid", db);
                    cmd.Parameters.AddWithValue("@sid", sessionId);
                    cmd.ExecuteNonQuery();
                }
                SetScanUIEnabled(false);
            }
        }

        // --- Form events ---
        private void ScanAttendance_FormClosing(object sender, FormClosingEventArgs e)
        {
            scanTimer?.Stop();
            sessionEndTimer?.Stop();
            _cam?.Dispose();
            _faceDet?.Dispose();
            _recognizer?.Dispose();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnForceEndSession_Click(object sender, EventArgs e)
        {
            if (!HasActiveSession())
                return;

            int sessionId = Convert.ToInt32(_activeSessions.Rows[0]["session_id"]);
            MarkAbsenteesForSession(sessionId);

            using (var db = new MySqlConnection(CONN_STR))
            {
                db.Open();
                var cmd = new MySqlCommand(
                    "UPDATE tbl_sessions SET force_ended=1, end_time=CURTIME() WHERE session_id=@sid", db);
                cmd.Parameters.AddWithValue("@sid", sessionId);
                cmd.ExecuteNonQuery();
            }

            lblStatus.Text = "Session force-ended. Absentees marked.";
            scanTimer?.Stop();
            sessionEndTimer?.Stop();
            btnForceEndSession.Enabled = false;
            btnBack.Enabled = true;
            pictureBoxCam.Image = null;
            lblBigStatus.Text = "SESSION ENDED";
            lblBigStatus.BackColor = Color.DarkGray;
            lblBigStatus.ForeColor = Color.White;
            panelFeedback.BackColor = Color.DarkGray;
            SetScanUIEnabled(false);
        }
    }
}
