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
    public partial class Login : Form
    {
        string studentID = "";
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clear the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Clear all textboxes
            textBoxID.Clear();

            // Refocus to ID textbox
            textBoxID.Focus();
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
        /// Go to the main page of the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Go_Click(object sender, EventArgs e)
        {
            studentID = textBoxID.Text.ToString();
            // Check that the login is successfull
            string IDQuery = "select studentID from student where studentID = " + studentID;

            // Run the query
            SQL.selectQuery(IDQuery);
            try
            {
                if (SQL.read.HasRows)
                {
                    // If yes close login and open home
                    Hide();
                    HomePage home = new HomePage();
                    home.studentID = studentID;
                    home.ShowDialog();
                    this.Close(); 
                    // Console.WriteLine("Has rows");
                }
                else
                {
                    // If no reset login and display error message
                    MessageBox.Show(studentID + " is not found in student database");
                    textBoxID.Clear();
                }
            }
            catch
            {
                //If an error happens here, it means error in locating data
                MessageBox.Show("Error in querying database. Please check that the database is connected.");
            }  
        }
    }
}
