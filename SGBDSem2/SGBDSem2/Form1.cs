using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;


namespace SGBDSem2
{
    public partial class Form1 : Form
    {

        DataSet ds = new DataSet();
        SqlDataAdapter daParent = new SqlDataAdapter();
        SqlDataAdapter daChild = new SqlDataAdapter();
        string connectionString = @"Server=DESKTOP-VJN0NT8\SQLEXPRESS;Database=SGBDSeminar2;Integrated Security=true;Trust Server Certificate=true";
        BindingSource bsParent = new BindingSource();
        BindingSource bsChild = new BindingSource();

        public Form1()

        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                using(SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    daChild.SelectCommand = new SqlCommand("SELECT * FROM Telefoane", con);
                    daParent.SelectCommand = new SqlCommand("SELECT * FROM Producatori", con);
                    daParent.Fill(ds, "Producatori");
                    daChild.Fill(ds, "Telefoane");

                    DataColumn pkcolumn = ds.Tables["Producatori"].Columns["cod_p"];
                    DataColumn fkcolumn = ds.Tables["Telefoane"].Columns["cod_p"];
                    DataRelation dr = new DataRelation("FK_Producatori_Telefoane", pkcolumn, fkcolumn, true);
                    ds.Relations.Add(dr);
                    bsParent.DataSource = ds.Tables["Producatori"];
                    dataGridViewParent.DataSource = bsParent;
                    textBox1.DataBindings.Add("Text", bsParent, "nume", true);

                    

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
