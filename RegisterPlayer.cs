using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using Emgu.CV.Face;
using System.IO;
using System.Configuration;

namespace AttendanceVarsity
{
    public partial class RegisterPlayer : Form
    {
        // ───────── CONFIG CONSTANTS ─────────
        private const int MAX_SAMPLES = 20;
        private const double BLUR_THRESHOLD = 300;

        // ───────── Runtime fields ───────────
        private VideoCapture _cam;
        private CascadeClassifier _faceDet, _eyeDet;
        private System.Windows.Forms.Timer _timer;

        private int _samples = 0;
        private readonly List<Image<Gray, byte>> _sampleImgs = new();
        private bool _cameraReady = false;
        private readonly string CONN_STR;
        private readonly string CASCADE_FACE;
        private readonly string CASCADE_EYE;
        private readonly string FACE_ROOT;
        private readonly string TOOLS_FOLDER;
        // For guided pose capture
        private readonly string[] _poseSteps = { "Look Straight", "Turn Head LEFT", "Turn Head RIGHT", "Look UP", "Look DOWN" };
        private int _currentPoseStep = 0;
        private int _samplesPerPose => MAX_SAMPLES / _poseSteps.Length; // E.g., 20/5 = 4
        private Image<Gray, byte> _lastSample = null; // For duplicate avoidance

        public RegisterPlayer()
        {
            InitializeComponent();

            // Config
            CONN_STR = ConfigurationManager.AppSettings["ConnStr"];
            CASCADE_FACE = ConfigurationManager.AppSettings["CascadeFace"];
            CASCADE_EYE = ConfigurationManager.AppSettings["CascadeEye"];
            FACE_ROOT = ConfigurationManager.AppSettings["FaceRoot"];
            TOOLS_FOLDER = ConfigurationManager.AppSettings["ToolsFolder"];

            LoadSports();
            LoadLevel();
            LoadGender();

            WireLiveValidation();
            SetupCamera();
            InitTimer();
            ClearStatus();
            ttSave.SetToolTip(btnSave, "Please complete all fields to enable saving.");

            progressSamples.Maximum = MAX_SAMPLES;
            this.MinimumSize = new Size(820, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            pictureBoxCam.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        // ───── 2. ASYNC DB INSERT ─────
        private async Task<bool> InsertPlayerAsync()
        {
            string studentId = txtStudentID.Text.Trim();
            string regDir = Path.Combine(FACE_ROOT, studentId, "Registration");
            Directory.CreateDirectory(regDir);
            for (int i = 0; i < _sampleImgs.Count; i++)
                _sampleImgs[i].Save(Path.Combine(regDir, $"{i + 1}.jpg"));

            try
            {
                await using var db = new MySqlConnection(CONN_STR);
                await db.OpenAsync();

                string sql = @"INSERT INTO tbl_players
                   (student_id, full_name, gender, level, sport_id, face_folder_path)
                   VALUES (@id, @n, @g, @lvl, @sp, @path)";

                await using var cmd = new MySqlCommand(sql, db);
                cmd.Parameters.AddWithValue("@id", studentId);
                cmd.Parameters.AddWithValue("@n", txtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@g", cmbGender.Text);
                cmd.Parameters.AddWithValue("@lvl", cmbLevel.Text);
                cmd.Parameters.AddWithValue("@sp", cmbSport.SelectedValue);
                cmd.Parameters.AddWithValue("@path", Path.Combine(FACE_ROOT, studentId));

                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DB error:\n{ex.Message}", "Database",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ───── CAMERA ─────
        private double CompareImagesMSE(Image<Gray, byte> img1, Image<Gray, byte> img2)
        {
            var diff = img1.AbsDiff(img2);
            MCvScalar sumSq = CvInvoke.Sum(diff.Mul(diff));
            double mse = sumSq.V0 / (img1.Width * img1.Height);
            return mse;
        }

        private void SetupCamera()
        {
            try
            {
                _faceDet = new CascadeClassifier(CASCADE_FACE);
                _eyeDet = new CascadeClassifier(CASCADE_EYE);

                _cam = new VideoCapture(0, VideoCapture.API.DShow);
                _cam.ImageGrabbed += (_, _) => { try { ShowLive(); } catch { } };
                _cam.Start();
                _cameraReady = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Camera init error:\n{ex.Message}", "Camera", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowLive()
        {
            if (!_cameraReady) return;

            using Mat m = new();
            _cam.Retrieve(m);

            var img = m.ToImage<Bgr, byte>();
            var gray = img.Convert<Gray, byte>();

            Rectangle[] faces = _faceDet.DetectMultiScale(gray, 1.1, 5);
            if (faces.Length > 0)
                img.Draw(faces[0], new Bgr(Color.Lime), 2);

            pictureBoxCam.Image = img.ToBitmap();
        }

        private void btnStartCapture_Click(object sender, EventArgs e)
        {
            _samples = 0; _sampleImgs.Clear();
            lblCapture.Text = "Starting…";
            _timer.Start();
        }

        private bool HasTwoEyes(Image<Gray, byte> faceImg) =>
            _eyeDet.DetectMultiScale(faceImg, 1.1, 5).Length >= 2;

        private void RegisterPlayerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _timer.Stop();
            _cam?.Dispose();
            _faceDet?.Dispose();
            _eyeDet?.Dispose();
        }

        private double CalcBlur(Image<Gray, byte> img)
        {
            using var lap = new Mat();
            CvInvoke.Laplacian(img, lap, DepthType.Cv64F);
            var mean = new MCvScalar();
            var std = new MCvScalar();
            CvInvoke.MeanStdDev(lap, ref mean, ref std);
            return std.V0 * std.V0;
        }

        // ======= Timer for auto‑capture =======
        private void InitTimer()
        {
            _timer = new System.Windows.Forms.Timer { Interval = 1000 }; // Or 1000
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_samples >= MAX_SAMPLES)
            {
                _timer.Stop();
                lblPoseInstruction.Text = "All poses captured!";
                return;
            }

            // --- Pose instruction logic
            _currentPoseStep = _samples / _samplesPerPose;
            lblPoseInstruction.Text = $"Pose {_currentPoseStep + 1}/{_poseSteps.Length}: {_poseSteps[_currentPoseStep]}";

            using Mat m = new();
            _cam.Retrieve(m);
            var rgb = m.ToImage<Bgr, byte>();
            var gray = rgb.Convert<Gray, byte>();
            var faces = _faceDet.DetectMultiScale(gray, 1.1, 5);

            if (faces.Length == 0)
            {
                lblCapture.Text = "No face detected";
                return;
            }

            var face = faces[0];

            // Face size guidance (forgiving)
            double faceArea = face.Width * face.Height;
            double imgArea = rgb.Width * rgb.Height;
            double ratio = faceArea / imgArea;
            if (ratio < 0.12)
            {
                lblCapture.Text = "Move closer to the camera";
                return;
            }
            if (ratio > 0.85)
            {
                lblCapture.Text = "Move back from the camera";
                return;
            }

            var roi = gray.Copy(face).Resize(200, 200, Inter.Cubic);

            // Relaxed blur check
            if (CalcBlur(roi) < 80)
            {
                lblCapture.Text = "Image too blurry, try to hold still";
                return;
            }

            // Eye detection: OPTIONAL. Only warn, don't block.
            if (_eyeDet.DetectMultiScale(roi, 1.1, 5).Length == 0)
            {
                lblCapture.Text = "Try to open eyes wider (not blocking capture)";
                // Do NOT return; just warn user!
            }

            // Duplicate check with a relaxed threshold (MSE)
            if (_lastSample != null)
            {
                double mse = CompareImagesMSE(_lastSample, roi);
                if (mse < 1500) // Higher = less strict. Adjust as needed.
                {
                    lblCapture.Text = "Change your pose, try a different angle!";
                    return;
                }
            }
            //_lastSample = roi.Clone();

            // Save sample
            _sampleImgs.Add(roi);
            _samples++;
            progressSamples.Value = _samples;
            lblCapture.Text = $"Captured {_samples}/{MAX_SAMPLES}";
            lblCapture.ForeColor = Color.Green;

           
            if (_samples >= MAX_SAMPLES)
            {
                lblStatus.Text = "✔ Samples complete";
                lblStatus.ForeColor = Color.Green;
                _timer.Stop();
                lblPoseInstruction.Text = "All poses captured!";
            }
        }


        // SAVING DATA
        private void TrainAndSaveRecognizer()
        {
            var images = new List<Mat>();
            var labels = new List<int>();
            var map = new Dictionary<int, string>();
            int label = 0;
            foreach (string dir in Directory.GetDirectories(FACE_ROOT))
            {
                string sid = Path.GetFileName(dir);
                if (!Regex.IsMatch(sid, @"^\d{3}-\d{4}$")) continue;
                foreach (string file in Directory.GetFiles(dir, "*.jpg", SearchOption.AllDirectories))
                {
                    images.Add(new Image<Gray, byte>(file).Resize(200, 200, Inter.Cubic).Mat);
                    labels.Add(label);
                }
                map[label] = sid; label++;
            }
            if (images.Count == 0) { MessageBox.Show("No training data."); return; }
            var lbph = new LBPHFaceRecognizer(1, 8, 8, 8, 100);
            lbph.Train(images.ToArray(), labels.ToArray());
            Directory.CreateDirectory(TOOLS_FOLDER);
            lbph.Write(Path.Combine(TOOLS_FOLDER, "trained_model.yml"));
            using var sw = new StreamWriter(Path.Combine(TOOLS_FOLDER, "labels.csv"));
            foreach (var kv in map) sw.WriteLine($"{kv.Key},{kv.Value}");
        }

        //VALIDATION
        private void ClearStatus()
        {
            foreach (var lbl in new[] { lblFullName, lblStudentID, lblGender, lblLevel, lblSport, lblCapture, lblStatus })
                lbl.Text = "";
        }

        private async void txtStudentID_TextChanged(object sender, EventArgs e)
        {
            string digits = Regex.Replace(txtStudentID.Text, "[^0-9]", "");
            if (digits.Length > 3) digits = digits.Insert(3, "-");

            if (!txtStudentID.Text.Equals(digits))
            {
                txtStudentID.Text = digits;
                txtStudentID.SelectionStart = digits.Length;
                return;
            }

            await Task.Delay(200);
            ValidateStudentID();
        }

        private void ValidateStudentID()
        {
            bool idFmt = Regex.IsMatch(txtStudentID.Text.Trim(), @"^\d{3}-\d{4}$");
            bool idUniq = idFmt && IsIdUnique(txtStudentID.Text.Trim());
            Set(lblStudentID, idFmt && idUniq, idFmt ? "Already ID" : "Bad ID");
        }

        private void Set(Label lbl, bool good, string err)
        {
            lbl.Text = good ? "✓" : err;
            lbl.ForeColor = good ? Color.Green : Color.Red;
        }

        private bool IsIdUnique(string id)
        {
            using var db = new MySqlConnection(CONN_STR);
            db.Open();
            using var cmd = new MySqlCommand("SELECT COUNT(*) FROM tbl_players WHERE student_id=@id", db);
            cmd.Parameters.AddWithValue("@id", id);
            return Convert.ToInt32(cmd.ExecuteScalar()) == 0;
        }

        private void WireLiveValidation()
        {
            txtFullName.KeyUp += (_, _) => ValidateFullName();
            txtStudentID.TextChanged += txtStudentID_TextChanged;
            cmbGender.SelectedIndexChanged += (_, _) => ValidateGender();
            cmbLevel.SelectedIndexChanged += (_, _) => ValidateLevel();
            cmbSport.SelectedIndexChanged += (_, _) => ValidateSport();
        }

        private void ValidateFullName()
        {
            bool nameOk = Regex.IsMatch(txtFullName.Text.Trim(), @"^[A-Za-z][A-Za-z\s.'-]{1,}$");
            Set(lblFullName, nameOk, "Invalid name");
        }

        private void ValidateGender()
        {
            bool gOk = cmbGender.SelectedIndex > 0;
            Set(lblGender, gOk, "Pick gender");
        }

        private void ValidateLevel()
        {
            bool lOk = cmbLevel.SelectedIndex > 0;
            Set(lblLevel, lOk, "Pick level");
        }

        private void ValidateSport()
        {
            bool sOk = cmbSport.SelectedIndex > 0;
            Set(lblSport, sOk, "Pick sport");
        }

        private bool ValidateAll()
        {
            bool ok = true;
            bool nameOk = Regex.IsMatch(txtFullName.Text.Trim(), @"^[A-Za-z][A-Za-z\s.'-]{1,}$");
            Set(lblFullName, nameOk, "Invalid name"); ok &= nameOk;

            bool idFmt = Regex.IsMatch(txtStudentID.Text.Trim(), @"^\d{3}-\d{4}$");
            bool idUniq = idFmt && IsIdUnique(txtStudentID.Text.Trim());
            Set(lblStudentID, idFmt && idUniq, idFmt ? "Already ID" : "Bad ID"); ok &= idFmt && idUniq;

            bool gOk = cmbGender.SelectedIndex > 0; Set(lblGender, gOk, "Pick gender"); ok &= gOk;
            bool lOk = cmbLevel.SelectedIndex > 0; Set(lblLevel, lOk, "Pick level"); ok &= lOk;
            bool sOk = cmbSport.SelectedIndex > 0; Set(lblSport, sOk, "Pick sport"); ok &= sOk;
            bool fOk = _samples >= MAX_SAMPLES; Set(lblCapture, fOk, "Need samples"); ok &= fOk;

            ttSave.SetToolTip(btnSave, ok ? "Click to save player" : "Fill all fields to enable Save");
            return ok;
        }

        // Private reset helper
        private void ResetForm()
        {
            txtFullName.Clear();
            txtStudentID.Clear();
            cmbGender.SelectedIndex = 0;
            cmbLevel.SelectedIndex = 0;
            cmbSport.SelectedIndex = 0;

            _sampleImgs.Clear();
            _samples = 0;
            pictureBoxCam.Image = null;
            progressSamples.Value = 0;

            ClearStatus();
        }

        // COMBOBOX LOADERS
        private void LoadSports()
        {
            using var db = new MySqlConnection(CONN_STR);
            db.Open();
            var da = new MySqlDataAdapter("SELECT sport_id, sport_name FROM tbl_sports", db);
            var dt = new DataTable();
            da.Fill(dt);
            var row = dt.NewRow();
            row["sport_id"] = DBNull.Value;
            row["sport_name"] = "-- Select Sport --";
            dt.Rows.InsertAt(row, 0);
            cmbSport.DataSource = dt;
            cmbSport.DisplayMember = "sport_name";
            cmbSport.ValueMember = "sport_id";
            cmbSport.SelectedIndex = 0;
            cmbSport.ForeColor = Color.Gray;
        }

        private void LoadGender()
        {
            cmbGender.Items.Clear();
            cmbGender.Items.Add("-- Select Gender --");
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");

            cmbGender.SelectedIndex = 0;
            cmbGender.ForeColor = Color.Gray;

            cmbGender.SelectedIndexChanged += (_, _) =>
                cmbGender.ForeColor = cmbGender.SelectedIndex == 0 ? Color.Gray : Color.Black;
        }

        private void LoadLevel()
        {
            cmbLevel.Items.Clear();
            cmbLevel.Items.Add("-- Select Level --");
            cmbLevel.Items.Add("High School");
            cmbLevel.Items.Add("College");

            cmbLevel.SelectedIndex = 0;
            cmbLevel.ForeColor = Color.Gray;

            cmbLevel.SelectedIndexChanged += (_, _) =>
                cmbLevel.ForeColor = cmbLevel.SelectedIndex == 0 ? Color.Gray : Color.Black;
        }

        // SAVE/CLEAR/EXIT BUTTONS
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateAll()) return;
            if (!await InsertPlayerAsync()) return;
            await Task.Run(TrainAndSaveRecognizer);
            MessageBox.Show("Player saved & model updated!", "Done");
            ResetForm();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e) { }
        private void RegisterPlayer_Load(object sender, EventArgs e) { }
        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label12_Click(object sender, EventArgs e) { }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close(); // Closes the current RegisterPlayer form and returns control to MainForm
        }
    }
}
