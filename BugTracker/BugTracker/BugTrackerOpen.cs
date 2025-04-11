using BugTracker.Domain;
using BugTracker.Service;
using log4net;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BugTracker
{
    public partial class BugTrackerOpen : Form, IBugObserver
    {
        public BugTrackerOpen(MyService service)
        {
            this.service = service;
            service.AddObserver(this);
            InitializeComponent();
            loadBugs(service.GetAllBugs());
        }
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        MyService service;

        private void loadBugs(IEnumerable<Bug> bugs)
        {
            dataGridView1.DataSource = bugs;
            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns["Title"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.ReadOnly = true;
        }

        private void CreateNewBugClick(object sender, EventArgs e)
        {
            string title = textBoxTitle.Text.Trim();
            string description = textBoxDescription.Text.Trim();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Title and/or Description cannot be empty.", "Input Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var bug = service.CreateNewBug(title, description);

                textBoxTitle.Clear();
                textBoxDescription.Clear();

                log.Info($"Bug '{title}' added successfully.");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the bug: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Error("Error adding bug.", ex);
            }
        }

        private void LogoutClick(object sender, EventArgs e)
        {
            service.LogOut();
            this.Hide();
            using (LogIn loginForm = new LogIn(service))
            {
                loginForm.ShowDialog();
            }
            this.Close();
        }

        void IBugObserver.OnBugListChanged(IEnumerable<Bug> bugs)
        {
            loadBugs(bugs);
        }
    }
}
