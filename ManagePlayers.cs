using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Globalization;
using Emgu.CV.CvEnum;

namespace AttendanceVarsity
{
    public partial class ManagePlayers : Form
    {
        private int _editingRowIdx = -1;
        private readonly string CONN_STR;
        private readonly string FACE_ROOT;
        private readonly string TOOLS_FOLDER;
        private DataTable sportsTable;

        public ManagePlayers()
        {
            InitializeComponent();
            dgvPlayers.EditMode = DataGridViewEditMode.EditOnEnter;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            CONN_STR = ConfigurationManager.AppSettings["ConnStr"];
            FACE_ROOT = ConfigurationManager.AppSettings["FaceRoot"];
            TOOLS_FOLDER = ConfigurationManager.AppSettings["ToolsFolder"];

            Load += ManagePlayers_Load;
            txtSearch.TextChanged += SomeFilterChanged;
            btnRefresh.Click += SomeFilterChanged;
            cmbSport.SelectedIndexChanged += SomeFilterChanged;
            cmbGender.SelectedIndexChanged += SomeFilterChanged;
            cmbLevel.SelectedIndexChanged += SomeFilterChanged;
            dgvPlayers.CellContentClick += dgvPlayers_CellContentClick;
        }

        private void ManagePlayers_Load(object sender, EventArgs e)
        {
            LoadFilterOptions();
            LoadPlayers();
        }

        private void LoadFilterOptions()
        {
            using var db = new MySqlConnection(CONN_STR);
            db.Open();
            var da = new MySqlDataAdapter("SELECT sport_id, sport_name FROM tbl_sports ORDER BY sport_name", db);
            sportsTable = new DataTable();
            da.Fill(sportsTable);

            var all = sportsTable.NewRow();
            all["sport_id"] = DBNull.Value;
            all["sport_name"] = "All Sports";
            sportsTable.Rows.InsertAt(all, 0);

            cmbSport.DataSource = sportsTable;
            cmbSport.DisplayMember = "sport_name";
            cmbSport.ValueMember = "sport_id";
            cmbSport.SelectedIndex = 0;

            cmbGender.Items.Clear();
            cmbGender.Items.AddRange(new[] { "All Genders", "Male", "Female" });
            cmbGender.SelectedIndex = 0;

            cmbLevel.Items.Clear();
            cmbLevel.Items.AddRange(new[] { "All Levels", "High School", "College" });
            cmbLevel.SelectedIndex = 0;
        }

        private void LoadPlayers()
        {
            var dt = new DataTable();
            using var db = new MySqlConnection(CONN_STR);
            db.Open();

            const string sql = @"
SELECT p.player_id, p.student_id, p.full_name, p.gender, p.level, s.sport_name
FROM tbl_players p
LEFT JOIN tbl_sports s ON p.sport_id = s.sport_id
WHERE (p.student_id LIKE @f OR p.full_name LIKE @f)
  AND (@sport IS NULL OR p.sport_id = @sport)
  AND (@gender = '' OR p.gender = @gender)
  AND (@level = '' OR p.level = @level)";

            using var cmd = new MySqlCommand(sql, db);
            cmd.Parameters.AddWithValue("@f", "%" + txtSearch.Text.Trim() + "%");
            cmd.Parameters.AddWithValue("@sport", cmbSport.SelectedIndex > 0 ? cmbSport.SelectedValue : DBNull.Value);
            cmd.Parameters.AddWithValue("@gender", cmbGender.SelectedIndex > 0 ? cmbGender.Text : "");
            cmd.Parameters.AddWithValue("@level", cmbLevel.SelectedIndex > 0 ? cmbLevel.Text : "");

            using var da = new MySqlDataAdapter(cmd);
            da.Fill(dt);

            dgvPlayers.Columns.Clear();
            dgvPlayers.DataSource = dt;
            // ...existing code for setting up dgvPlayers...

            // Insert "View Attendance" button column if not present
            if (!dgvPlayers.Columns.Contains("btnViewAttendance"))
            {
                var btnViewAtt = new DataGridViewButtonColumn
                {
                    Name = "btnViewAttendance",
                    HeaderText = "Attendance",
                    Text = "View",
                    UseColumnTextForButtonValue = true
                };
                dgvPlayers.Columns.Add(btnViewAtt);
            }

            dgvPlayers.Columns["player_id"].HeaderText = "ID";
            dgvPlayers.Columns["student_id"].HeaderText = "Student ID";
            dgvPlayers.Columns["full_name"].HeaderText = "Full Name";
            dgvPlayers.Columns["gender"].HeaderText = "Gender";
            dgvPlayers.Columns["level"].HeaderText = "Level";
            dgvPlayers.Columns["sport_name"].HeaderText = "Sport";

            InsertButtonColumn("btnEdit", "Edit");
            InsertButtonColumn("btnDelete", "Delete");
            InsertButtonColumn("btnUpdateFace", "Update Face");

            foreach (DataGridViewRow r in dgvPlayers.Rows)
            {
                ((DataGridViewButtonCell)r.Cells["btnEdit"]).Value = "Edit";
                ((DataGridViewButtonCell)r.Cells["btnDelete"]).Value = "Delete";
                ((DataGridViewButtonCell)r.Cells["btnUpdateFace"]).Value = "Update Face";
            }

            foreach (DataGridViewColumn col in dgvPlayers.Columns)
                if (!col.Name.StartsWith("btn"))
                    col.ReadOnly = true;
        }

        private void InsertButtonColumn(string name, string text)
        {
            var btn = new DataGridViewButtonColumn
            {
                Name = name,
                HeaderText = text,
                UseColumnTextForButtonValue = false
            };
            dgvPlayers.Columns.Add(btn);
        }

        private void dgvPlayers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var col = dgvPlayers.Columns[e.ColumnIndex].Name;
            var row = dgvPlayers.Rows[e.RowIndex];
            int id = Convert.ToInt32(row.Cells["player_id"].Value);
            string sid = row.Cells["student_id"].Value.ToString();


            // If another edit is in progress and user is trying to edit a different row
            if (col == "btnEdit" && _editingRowIdx >= 0 && _editingRowIdx != e.RowIndex)
            {
                if (!PromptToSaveIfEditing()) return;
                // _editingRowIdx and grid will reload inside PromptToSaveIfEditing
                // so find the row again after reload
                row = dgvPlayers.Rows[e.RowIndex];
            }
            switch (col)
            {
                case "btnEdit": ToggleEditSave(row, e.RowIndex); break;
                case "btnDelete":
                    if (MessageBox.Show($"Delete player {sid}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        DeletePlayer(id, sid);
                        LoadPlayers();
                    }
                    break;
                case "btnUpdateFace": TrainAndSaveRecognizer(); break;

                case "btnViewAttendance":
                    // Pass player_id, full_name, and/or student_id as needed

                    var playerName = row.Cells["full_name"].Value.ToString();
                    this.Hide();
                    var attendanceForm = new ManageAttendancesPlayer(id, playerName, sid);// Adjust constructor as needed
                    attendanceForm.ShowDialog();
                    this.Show();
                    break;
            }
        }

        private void ToggleEditSave(DataGridViewRow row, int rowIndex)
        {
            var btn = (DataGridViewButtonCell)row.Cells["btnEdit"];
            bool isSave = (_editingRowIdx == rowIndex);

            if (!isSave)
            {
                _editingRowIdx = rowIndex;

                // Swap text cells with combo boxes
                SwapToComboBox(row, "gender", new[] { "Male", "Female" });
                SwapToComboBox(row, "level", new[] { "High School", "College" });
                SwapToComboBox(row, "sport_name", sportsTable, "sport_name", "sport_name");

                // Unlock row
                foreach (DataGridViewCell c in row.Cells)
                    if (!(c is DataGridViewButtonCell)) c.ReadOnly = false;

                row.DefaultCellStyle.BackColor = Color.LightYellow;
                btn.Value = "Save";
            }
            else
            {
                if (MessageBox.Show("Save changes?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    SaveRowChanges(row);

                _editingRowIdx = -1;
                LoadPlayers();
            }
        }

        private void SwapToComboBox(DataGridViewRow row, string columnName, IEnumerable<string> items)
        {
            var cell = new DataGridViewComboBoxCell
            {
                DataSource = items.ToList(),
                Value = row.Cells[columnName].Value
            };
            row.Cells[columnName] = cell;
        }

        private void SwapToComboBox(DataGridViewRow row, string columnName, DataTable table, string display, string value)
        {
            var cell = new DataGridViewComboBoxCell
            {
                DataSource = table.Copy(),
                DisplayMember = display,
                ValueMember = value,
                Value = row.Cells[columnName].Value
            };
            row.Cells[columnName] = cell;
        }


        private void SaveRowChanges(DataGridViewRow row)
        {
            int id = Convert.ToInt32(row.Cells["player_id"].Value);
            string sid = row.Cells["student_id"].Value.ToString();
            string name = row.Cells["full_name"].Value.ToString();
            string gender = row.Cells["gender"].EditedFormattedValue.ToString();
            string level = row.Cells["level"].EditedFormattedValue.ToString();
            string sport = row.Cells["sport_name"].EditedFormattedValue.ToString();

            using var db = new MySqlConnection(CONN_STR);
            db.Open();
            var spCmd = new MySqlCommand("SELECT sport_id FROM tbl_sports WHERE sport_name=@n", db);
            spCmd.Parameters.AddWithValue("@n", sport);
            int sp = Convert.ToInt32(spCmd.ExecuteScalar());

            var cmd = new MySqlCommand(
                "UPDATE tbl_players SET student_id=@sid, full_name=@nm, gender=@g, level=@l, sport_id=@sp WHERE player_id=@id", db);
            cmd.Parameters.AddWithValue("@sid", sid);
            cmd.Parameters.AddWithValue("@nm", name);
            cmd.Parameters.AddWithValue("@g", gender);
            cmd.Parameters.AddWithValue("@l", level);
            cmd.Parameters.AddWithValue("@sp", sp);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        private void DeletePlayer(int playerId, string studentId)
        {
            using var db = new MySqlConnection(CONN_STR);
            db.Open();
            var del = new MySqlCommand("DELETE FROM tbl_players WHERE player_id=@id", db);
            del.Parameters.AddWithValue("@id", playerId);
            del.ExecuteNonQuery();
            TrainAndSaveRecognizer();
        }

        private void TrainAndSaveRecognizer()
        {
            var images = new List<Mat>();
            var labels = new List<int>();
            var map = new Dictionary<int, string>();
            int label = 0;

            foreach (var dir in Directory.GetDirectories(FACE_ROOT))
            {
                var sid = Path.GetFileName(dir);
                var reg = Path.Combine(dir, "Registration");
                if (!Directory.Exists(reg)) continue;

                foreach (var f in Directory.GetFiles(reg, "*.jpg"))
                {
                    images.Add(new Image<Gray, byte>(f).Resize(200, 200, Inter.Cubic).Mat);
                    labels.Add(label);
                }
                map[label++] = sid;
            }

            if (images.Any())
            {
                var rec = new LBPHFaceRecognizer(1, 8, 8, 8, 100);
                rec.Train(images.ToArray(), labels.ToArray());
                Directory.CreateDirectory(TOOLS_FOLDER);
                rec.Write(Path.Combine(TOOLS_FOLDER, "trained_model.yml"));
                using var sw = new StreamWriter(Path.Combine(TOOLS_FOLDER, "labels.csv"));
                foreach (var kv in map)
                    sw.WriteLine($"{kv.Key},{kv.Value}");
            }
        }
        private bool PromptToSaveIfEditing()
        {
            if (_editingRowIdx >= 0)
            {
                var row = dgvPlayers.Rows[_editingRowIdx];
                var result = MessageBox.Show("You have unsaved changes. Save now?", "Unsaved Edit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    SaveRowChanges(row);
                    _editingRowIdx = -1;
                    LoadPlayers();
                    return true;
                }
                else if (result == DialogResult.No)
                {
                    _editingRowIdx = -1;
                    LoadPlayers();
                    return true;
                }
                else // Cancel
                {
                    // Don't change anything, cancel the user's new action.
                    return false;
                }
            }
            return true;
        }
        private void SomeFilterChanged(object sender, EventArgs e)
        {
            if (!PromptToSaveIfEditing()) return;
            LoadPlayers();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close(); // Closes the current RegisterPlayer form and returns control to MainForm
        }
    }
}
