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
    public partial class Grades : Form
    {
        public string studentID = "";
        string details = "{0, -12}{1, -15}";
        string idDetails = "{0, -15}{1, -15}";
        string nameDetails = "{0, -15}{1, -10}{2, -10}";
        string courseDetails = "{0, -20}{1, -10}{2, -10}{3, -8}";
        string gradesDetails = "{0, -20}{1, -10}{2, -8}{3, -8}";

        public Grades()
        {
            InitializeComponent();
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

        private void Grades_Load(object sender, EventArgs e)
        {
            // Load the student details
            loadStudentDetails();

            listBoxGrades.Items.Add(String.Format(gradesDetails, "Assignment", "Worth(%)", "Grade", "Overall Worth"));


            // Load the course details into the listbox
            string courseQuery = "select papercode, grade from takepaper where studID = " + studentID;
            string course = "", grade = "", letter = "";

            // Run the query
            SQL.selectQuery(courseQuery);

            //clear the listbox previous data
            listBoxCourses.Items.Clear();

            try
            {
                if (SQL.read.HasRows)
                {
                    // Get the information from the database
                    while (SQL.read.Read())
                    {
                        course = SQL.read[0].ToString();
                        grade = SQL.read[1].ToString();
                        letter = letterGrade(grade);
                        listBoxCourses.Items.Add(String.Format(details, course, letter));
                    }
                }
                else
                {
                    listBoxCourses.Items.Add("No course information found.");
                }
            }
            catch
            {
                //If an error happens here, it means error in locating data
                MessageBox.Show("Error in querying database. Please check that the database is connected.");
            }
        }

        /// <summary>
        /// Load in the students details
        /// </summary>
        private void loadStudentDetails() 
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
                        listBoxStudentDetails.Items.Add(String.Format(idDetails, "Student ID:", ID));
                        listBoxStudentDetails.Items.Add(String.Format(nameDetails, "Student Name:", fname, lname));
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
        /// Load the chosen courses assignments to the grades listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] courseArray = new string[2];
            string fullSelect = "";
            string selectedCourse = "";
            fullSelect = listBoxCourses.GetItemText(listBoxCourses.SelectedItem);
            courseArray = fullSelect.Split(' ');
            selectedCourse = courseArray[0];

            string gradesQuery = "select assName, percentageWorth, earntGrade, percentEarnt " +
                "from assignment a join sitAssignment s " +
                "on a.assName = s.assignmentName " +
                "where a.course = '" + selectedCourse + "'";

            string name = "", worth = "", earnt = "", recieved = "";

            // Run the query
            SQL.selectQuery(gradesQuery);

            //clear the listbox previous data
            listBoxGrades.Items.Clear();
            listBoxGrades.Items.Add(String.Format(gradesDetails, "Assignment", "Worth(%)", "Grade", "Overall Worth"));

            try
            {
                if (SQL.read.HasRows)
                {
                    // Get the information from the database
                    while (SQL.read.Read())
                    {
                        name = SQL.read[0].ToString();
                        worth = SQL.read[1].ToString();
                        earnt = SQL.read[2].ToString();
                        recieved = SQL.read[3].ToString();

                        // Get the information from the database
                        listBoxGrades.Items.Add(String.Format(courseDetails, name, worth, earnt, recieved));
                    }
                }
                else
                {
                    listBoxGrades.Items.Add("No information found.");
                }
            }
            catch
            {
                //If an error happens here, it means error in locating data
                MessageBox.Show("Error in querying database. Please check that the database is connected.");
            }

            // Load the course details into the listbox
            string courseQuery = "select grade from takepaper where studID = " + studentID + " and papercode = '"+ selectedCourse +"'";
            string grade = "";

            // Run the query
            SQL.selectQuery(courseQuery);

            try
            {
                if (SQL.read.HasRows)
                {
                    // Get the information from the database
                    while (SQL.read.Read())
                    {
                        grade = SQL.read[0].ToString();
                        listBoxGrades.Items.Add(String.Format(details, "Current Total: ", grade));
                    }
                }
                else
                {
                    listBoxCourses.Items.Add("No course information found.");
                }
            }
            catch
            {
                //If an error happens here, it means error in locating data
                MessageBox.Show("Error in querying database. Please check that the database is connected.");
            }

        }

        /// <summary>
        /// Calculate the letter grade of the passed in grade
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        private string letterGrade(string grade) 
        {
            double mark = double.Parse(grade);
            string letterGrade = "F";

            if (mark < 50) 
            {
                letterGrade = "F";
            }
            else if (mark < 55)
            {
                letterGrade = "C-";
            }
            else if (mark < 60)
            {
                letterGrade = "C";
            }
            else if (mark < 65)
            {
                letterGrade = "C+";
            }
            else if (mark < 70)
            {
                letterGrade = "B-";
            }
            else if (mark < 75)
            {
                letterGrade = "B";
            }
            else if (mark < 80)
            {
                letterGrade = "B+";
            }
            else if (mark < 85)
            {
                letterGrade = "A-";
            }
            else if (mark < 90)
            {
                letterGrade = "A";
            }
            else if (mark <= 100)
            {
                letterGrade = "A+";
            }

            return letterGrade;
        }

        /// <summary>
        /// Return to the home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            HomePage home = new HomePage();
            home.studentID = studentID;
            home.ShowDialog();
            this.Close();
        }

        /// <summary>
        /// Return to the login page and sign out
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

        private void listBoxGrades_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
