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
using Emgu.CV.Face;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
namespace AttendanceVarsity
{
    public partial class Login : Form
    {

        private readonly string CONN_STR = System.Configuration.ConfigurationManager.AppSettings["ConnStr"];

        public Login()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Exact client area size


            // Center on primary screen
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public static class AppSession
        {
            public static int UserId { get; set; }
            public static string FullName { get; set; }
            public static string Role { get; set; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private string HashPassword(string input)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text; // user input
            string hashedPassword = HashPassword(password); // hash it!

            using (var db = new MySqlConnection(CONN_STR))
            {
                db.Open();
                string sql = @"SELECT user_id, full_name, role, password_hash FROM tbl_users WHERE username=@u LIMIT 1";
                using (var cmd = new MySqlCommand(sql, db))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            string hashInDb = rdr.GetString("password_hash");
                            if (hashedPassword == hashInDb) // Compare hash to hash!
                            {
                                // Save session
                                AppSession.UserId = rdr.GetInt32("user_id");
                                AppSession.FullName = rdr.IsDBNull(rdr.GetOrdinal("full_name")) ? "" : rdr.GetString("full_name");
                                AppSession.Role = rdr.GetString("role");

                                this.Hide();
                                new MainForm().Show();
                                return;
                            }
                        }
                    }
                }
            }
            MessageBox.Show("Invalid username or password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e) { }

        private void panel1_Paint(object sender, PaintEventArgs e) { }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new MainForm().Show();
        }
    }
}
