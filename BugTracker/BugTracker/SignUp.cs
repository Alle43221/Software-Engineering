using BugTracker.Domain;
using BugTracker.Service;
using log4net;
using System;
using System.Windows.Forms;

namespace BugTracker
{
    public partial class SignUp : Form
    {
        public SignUp(MyService service)
        {
            this.service = service;
            InitializeComponent();
            this.comboBoxRole.Items.AddRange(new object[] {
                        "Programmer",
                        "QualityAssuranceEngineer",
                        });
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        MyService service;

        private void OpenLogInWindow(object sender, EventArgs e)
        {
            this.Hide();
            using (LogIn loginForm = new LogIn(service))
            {
                loginForm.ShowDialog();
            }
            this.Close();
        }

        private void SignUpClick(object sender, EventArgs e)
        {
            log.Info("Trying to sign up new user");

            // Validate inputs
            if (string.IsNullOrWhiteSpace(textBoxName.Text) ||
                string.IsNullOrWhiteSpace(textBoxUsername.Text) ||
                string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                MessageBox.Show("All fields are required.");
                log.Info("Sign-up failed: Missing input fields.");
                return;
            }

            if (!Enum.TryParse(comboBoxRole.Text, out Role selectedRole))
            {
                MessageBox.Show("Invalid role selected.");
                log.Info("Sign-up failed: Invalid role.");
                return;
            }

            try
            {
                // Check if the username already exists
                var existingUser = service.GetUserByUsername(textBoxUsername.Text);
                if (existingUser != null)
                {
                    MessageBox.Show("Username already taken. Please choose another.");
                    log.Info("Sign-up failed: Username already exists.");
                    return;
                }

                // Create new user
                Role t = (Role)Enum.Parse(typeof(Role), comboBoxRole.Text);
                var result = service.CreateNewUser(textBoxName.Text, textBoxUsername.Text, textBoxPassword.Text, t);
                if (result == null)
                {
                    MessageBox.Show("Error during sign-up. Please try again.");
                    log.Info("Sign-up failed: Error saving user.");
                }
                else
                {
                    log.Info("User signed up successfully.");
                    MessageBox.Show("Sign-up successful! You can now log in.");

                    // Redirect to login form
                    this.Hide();
                    using (LogIn loginForm = new LogIn(service))
                    {
                        loginForm.ShowDialog();
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error during sign-up", ex);
                MessageBox.Show("An error occurred. Please try again.");
            }
        
        }
    }
}
