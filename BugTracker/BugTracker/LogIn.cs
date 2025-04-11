using log4net;
using System;
using System.Windows.Forms;
using BugTracker.Service;

namespace BugTracker
{
    public partial class LogIn : Form
    {
        public LogIn(MyService service)
        {
            this.service = service;
            InitializeComponent();
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        MyService service;

        private void label3_Click(object sender, EventArgs e)
        {

            this.Hide();
            using (SignUp loginForm = new SignUp(service))
            {
                loginForm.ShowDialog();
            }
            this.Close();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            log.Info("Trying to authenticate user");
            var result = service.AuthenticateUser(textBoxUsername.Text, textBoxPassword.Text);
            if (result == null)
            {
                MessageBox.Show("Invalid username or password");
                log.Info("Invalid username or password");
            }
            else
            {
                log.Info("User authenticated");
                this.Hide();
                if (result.Role ==Domain.Role.Programmer)
                {
                    using (BugTrackerClose app = new BugTrackerClose(service))
                    {
                        app.ShowDialog();
                    }

                }
                else
                {
                    using (BugTrackerOpen app = new BugTrackerOpen(service))
                    {
                        app.ShowDialog();
                    }

                }
                this.Close();
            }
        }
    }
    }
