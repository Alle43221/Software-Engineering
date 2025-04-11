using BugTracker.Service;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTracker
{
    public partial class BugTrackerOpen : Form
    {
        public BugTrackerOpen(MyService service)
        {
            this.service = service;
            InitializeComponent();
            loadBugs();
        }
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        MyService service;

        private void loadBugs()
        {
            dataGridView1.DataSource = service.GetAllBugs().ToList();
            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns["Title"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView1.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
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

                loadBugs();

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

        private void button2_Click(object sender, EventArgs e)
        {
            service.LogOut();
            this.Hide();
            using (LogIn loginForm = new LogIn(service))
            {
                loginForm.ShowDialog();
            }
            this.Close();
        }
    }
}
