using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniversityGradesSystem
{
    public partial class HomePage : Form
    {
        string idDetails = "{0, -15}{1, -15}";
        string nameDetails = "{0, -15}{1, -10}{2, -10}";
        public string studentID = "";
        public HomePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Go to view grades page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            Grades gradePage = new Grades();
            gradePage.studentID = studentID;
            gradePage.ShowDialog();
            this.Close();
        }

        private void HomePage_Load(object sender, EventArgs e)
        {
            // Load the students details into the listbox
            string studentQuery = "select * from student where studentID = " + studentID;
            Console.WriteLine(studentQuery);
            string ID = "", fname = "", lname = "";

            // Run the query
            SQL.selectQuery(studentQuery);

            //clear the listbox previous data
            listBoxStudentDetails.Items.Clear();

            try
            {
                if (SQL.read.HasRows)
                {
                    // Get the information from the database
                    while (SQL.read.Read())
                    {
                        ID = SQL.read[0].ToString();
                        fname = SQL.read[1].ToString();
                        lname = SQL.read[2].ToString();

                        // Get the information from the database
                        listBoxStudentDetails.Items.Add(String.Format(idDetails,"Student ID:", ID));
                        listBoxStudentDetails.Items.Add(String.Format(nameDetails,"Student Name:", fname, lname));
                    }
                }
                else
                {
                    listBoxStudentDetails.Items.Add("No information found.");
                }
            }
            catch
            {
                //If an error happens here, it means error in locating data
                MessageBox.Show("Error in querying database. Please check that the database is connected.");
            }
        }

        /// <summary>
        /// Close down the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Open the update courses page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            Hide();
            UpdatePage update = new UpdatePage();
            update.studentID = studentID;
            update.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// Sign out and return to the login page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            Login login = new Login();
            login.ShowDialog();
            this.Close();

        }
    }
}
