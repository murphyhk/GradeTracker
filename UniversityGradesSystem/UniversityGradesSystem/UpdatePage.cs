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
    public partial class UpdatePage : Form
    {
        public string studentID = "";
        string idDetails = "{0, -15}{1, -15}";
        string nameDetails = "{0, -15}{1, -10}{2, -10}";
        public UpdatePage()
        {
            InitializeComponent();
        }

        private void UpdatePage_Load(object sender, EventArgs e)
        {
            // Load the student details
            loadStudentDetails();
            initiliseTextboxes();

            // Load the course details into the listbox
            string courseQuery = "select papercode from takepaper where studID = " + studentID;
            string course = "";

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
                        listBoxCourses.Items.Add(course);
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

        /// <summary>
        /// Close down the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void initiliseTextboxes() 
        {
            //goes through and clears all of the textboxes
            foreach (Control c in this.Controls)
            {
                //if the it is a textbox
                if (c is TextBox)
                {
                    //clear the text box
                    (c as TextBox).Clear();
                }
            }
            //focus on first text box
            textBoxCourseName.Focus();
        }

        

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedAssignment = "";
            selectedAssignment = listBoxAssignment.GetItemText(listBoxAssignment.SelectedItem);

            string selectedCourse = "";
            selectedCourse = listBoxCourses.GetItemText(listBoxCourses.SelectedItem);

            string assignmentQuery = "select assName, percentageWorth, earntGrade, percentEarnt " +
                "from assignment a join sitAssignment s " +
                "on a.assName = s.assignmentName " +
                "where a.course = '" + selectedCourse + "' and a.assName = '" + selectedAssignment + "'";

            string worth = "", mark = "", percentage = "", name = "";

            // Run the query
            SQL.selectQuery(assignmentQuery);

            try
            {
                if (SQL.read.HasRows)
                {
                    // Get the information from the database
                    while (SQL.read.Read())
                    {
                        name = SQL.read[0].ToString();
                        worth = SQL.read[1].ToString();
                        mark = SQL.read[2].ToString();
                        percentage = SQL.read[3].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("No information found.");
                }
            }
            catch
            {
                //If an error happens here, it means error in locating data
                MessageBox.Show("Error in querying database. Please check that the database is connected.");
            }

            // display in textboxes
            textBoxAssName.Text = name;
            textBoxWorth.Text = worth;
            textBoxAssMark.Text = mark;
            textBoxPercentage.Text = percentage;
            textBoxAssGrade.Text = LetterGrade(mark);
        }

        private void listBoxCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            initiliseTextboxes();

            refreshCourse();

        }

        public void refreshCourse()
        {
            initiliseTextboxes();

            string selectedCourse = "";
            selectedCourse = listBoxCourses.GetItemText(listBoxCourses.SelectedItem);

            string courseQuery = "select papercode, name, grade from takePaper t join course c " +
                "on t.papercode = c.courseCode where studID = '" + studentID + "' and c.courseCode = '" + selectedCourse + "'";

            string name = "", code = "", grade = "", mark = "", assignment = "";

            // Run the query
            SQL.selectQuery(courseQuery);

            try
            {
                if (SQL.read.HasRows)
                {
                    // Get the information from the database
                    while (SQL.read.Read())
                    {
                        code = SQL.read[0].ToString();
                        name = SQL.read[1].ToString();
                        mark = SQL.read[2].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("No information found.");
                }
            }
            catch
            {
                //If an error happens here, it means error in locating data
                MessageBox.Show("Error in querying database. Please check that the database is connected.");
            }

            // display in textboxes
            textBoxCourseName.Text = name;
            textBoxCourseCode.Text = code;
            textBoxGrade.Text = LetterGrade(mark);
            textBoxMark.Text = mark;

            assignmentRefresh(code);
        }


        public void assignmentRefresh(string code)
        {
            string assignment = "";

            // Display the courses assignments
            string assignmentQuery = "select assName from assignment where course = '" + code + "'";

            // Run the query
            SQL.selectQuery(assignmentQuery);

            //clear the listbox previous data
            listBoxAssignment.Items.Clear();

            try
            {
                if (SQL.read.HasRows)
                {
                    // Get the information from the database
                    while (SQL.read.Read())
                    {
                        assignment = SQL.read[0].ToString();
                        listBoxAssignment.Items.Add(assignment);
                    }
                }
                else
                {
                    listBoxAssignment.Items.Add("No information found.");
                }
            }
            catch
            {
                // If an error happens here, it means error in locating data
                MessageBox.Show("Error in querying database. Please check that the database is connected.");
            }
        }

        /// <summary>
        /// Calculate the letter grade of the passed in grade
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        private string LetterGrade(string grade)
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
        /// Update all the information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            string name = "", code = "", mark = "", grade = "";
            string worth = "", assMark = "", percentage = "", assGrade = "", assName = "";
            // Get the data from the textboxes and store into variables created above, good to put in a try catch with error message
            try
            {
                // Course
                name = textBoxCourseName.Text.Trim();
                code = textBoxCourseCode.Text.Trim();
                mark = textBoxMark.Text.Trim();
                grade = textBoxGrade.Text.Trim();

                // Assignment
                if (listBoxAssignment.SelectedItem != null)
                {
                    assName = textBoxAssName.Text.Trim();
                    worth = textBoxWorth.Text.Trim();
                    assMark = textBoxAssMark.Text.Trim();
                    percentage = textBoxPercentage.Text.Trim();
                    assGrade = textBoxAssGrade.Text.Trim();
                }
            }
            catch
            {
                //Error message, more useful when you are storing numbers etc. into the database.
                MessageBox.Show("Please make sure your text is in correct format.");
                return;
            }

            try
            {
                // Replace all values in the database relevant to the class
                if (listBoxAssignment.SelectedItem != null)
                {
                    SQL.executeQuery("UPDATE sitAssignment SET earntGrade = " + assMark + " WHERE studID = " + studentID + " and assignmentName = '" + assName + "'");
                    
                    string updatePercent = "update sitAssignment set percentEarnt = (select round(earntGrade * (percentageWorth / 100), 2) as GradeWorth " +
                        "from sitAssignment s join assignment a on s.assignmentName = a.assName " +
                        "where s.studID = " + studentID + " and s.assignmentName = '" + assName + "') " +
                        "where studID = " + studentID + " and assignmentName = '" + assName + "'";
                    SQL.executeQuery(updatePercent);

                    assGrade = LetterGrade(assMark);

                    textBoxAssName.Text = name;
                    textBoxAssMark.Text = assMark;
                    textBoxAssGrade.Text = assGrade;
                    textBoxWorth.Text = worth;
                    textBoxPercentage.Text = percentage;
                }

                // Update the overall course grades
                string courseGradeQuery = "update takePaper set grade = (select sum(percentEarnt) from sitAssignment " +
                    "where studID = " + studentID + " and course = '" + code + "') " +
                    "where studID = " + studentID + " and papercode = '" + code + "'";

                SQL.executeQuery(courseGradeQuery);
                Console.WriteLine(courseGradeQuery);

                // display in textboxes
                textBoxCourseName.Text = name;
                textBoxCourseCode.Text = code;
                textBoxGrade.Text = LetterGrade(mark);
                textBoxMark.Text = mark;

                grade = LetterGrade(mark);

                refreshCourse();

                // Success message for the user to know it worked
                MessageBox.Show("Successfully updated");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (listBoxAssignment.SelectedItem == null)
            {
                string assignmentName = "", course = "", worth = "", assMark = "", percentage = "";

                assignmentName = textBoxAssName.Text.Trim();
                course = textBoxCourseCode.Text.Trim();
                worth = textBoxWorth.Text.Trim();
                assMark = textBoxAssMark.Text.Trim();
                percentage = "0";

                string addAssignmentQuery = "insert into assignment values('"+ assignmentName + "', "+ worth +", '"+ course +"')";
                string addSitAssignmentQuery = "insert into sitAssignment values(" + studentID + ", '" 
                    + assignmentName + "', '"+ course + "', "+ assMark + ", " + percentage +")";
                SQL.executeQuery(addAssignmentQuery);
                SQL.executeQuery(addSitAssignmentQuery);


                string updatePercent = "update sitAssignment set percentEarnt = (select round(earntGrade * (percentageWorth / 100), 2) as GradeWorth " +
                        "from sitAssignment s join assignment a on s.assignmentName = a.assName " +
                        "where s.studID = " + studentID + " and s.assignmentName = '" + assignmentName + "') " +
                        "where studID = " + studentID + " and assignmentName = '" + assignmentName + "'";
                SQL.executeQuery(updatePercent);

                // Refresh the page
                assignmentRefresh(course);
                textBoxAssGrade.Clear();
                textBoxAssMark.Clear();
                textBoxAssName.Clear();
                textBoxPercentage.Clear();
                textBoxWorth.Clear();
                listBoxAssignment.SelectedIndex = -1;
            }
            else 
            {
                MessageBox.Show("You cannot add an assignment which already exists"); 
                listBoxAssignment.SelectedIndex = -1;
                return;
            }
        }
    }
}
