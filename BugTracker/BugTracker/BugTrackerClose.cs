using BugTracker.Domain;
using BugTracker.Service;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BugTracker
{
    public partial class BugTrackerClose : Form, IBugObserver
    {
        public BugTrackerClose(MyService service)
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

        private void CloseBugClick(object sender, EventArgs e)
        {
            // Ensure that a row is selected in the DataGridView
            if (dataGridView1.SelectedRows.Count !=1 )
            {
                MessageBox.Show("Please select a single bug to close.", "Bug Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected bug's ID from the DataGridView
            int selectedBugId = (int)dataGridView1.SelectedRows[0].Cells["Id"].Value;
            BugStatus status = (BugStatus)dataGridView1.SelectedRows[0].Cells["Status"].Value;

            if (status == BugStatus.Closed)
            {
                MessageBox.Show("Bug already closed.", "Bug Closed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            try
            {

                // Call the service to update the bug's status to "Closed"
                var closedBug = service.CloseBug(selectedBugId);

                // Show success message
                MessageBox.Show($"Bug '{closedBug.Title}' has been closed.", "Bug Closed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Optionally, log the action
                log.Info($"Bug with ID {selectedBugId} was closed.");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur while closing the bug
                MessageBox.Show($"Error while closing the bug: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log.Error("Error closing bug", ex);
            }
        }
        // Event handler when the selection changes in the DataGridView
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the selected bug's data from the selected row
                int selectedBugId = (int)dataGridView1.SelectedRows[0].Cells["Id"].Value;
                var selectedBug = service.GetAllBugs().FirstOrDefault(b => b.Id == selectedBugId);

                if (selectedBug != null)
                {
                    // Populate the TextBox controls with the selected bug's data
                    textBoxTitle.Text = selectedBug.Title;
                    textBoxDescription.Text = selectedBug.Description;
                }
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
