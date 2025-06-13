using FYP_Management.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace FYP_Management
{
    public partial class AddGrp : Window
    {
        // In‐memory list for “selected” students
        private List<Stu> stuList;

        public AddGrp()
        {
            InitializeComponent();

            // 1) Create the in‐memory list, bind it to the SelectedStudents grid
            stuList = new List<Stu>();
            SelectedStudents.ItemsSource = stuList;

            // 2) Default the date picker to today
            Datepicker.SelectedDate = DateTime.Now;

            // 3) Load all students (no search filter yet)
            LoadData();
        }

        /// <summary>
        /// Closes the window (Cancel/Close button).
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Loads (or refreshes) the “AllStudents” grid using the current SearchBar text.
        /// We wrap the text in %...% so that SQL’s LIKE '%term%' matches correctly.
        /// </summary>
        private void LoadData()
        {
            try
            {
                // Wrap in %...% for the stored procedure’s LIKE clause
                string pattern = $"%{SearchBar.Text}%";
                DataTable dt = Group_Helper.GetStudentsNotInAnyGroup(pattern);

                // Bind to AllStudents DataGrid
                AllStudents.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading data from database:\n" + ex.Message,
                    "Alert",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        /// <summary>
        /// Called whenever the SearchBar text changes. Refresh the AllStudents grid.
        /// </summary>
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// “Clear” button: empty the search box, which in turn refreshes AllStudents to show everyone.
        /// </summary>
        private void ClearTxt_Click(object sender, RoutedEventArgs e)
        {
            SearchBar.Text = "";
        }

        /// <summary>
        /// The “>” button: When the user clicks this, we take the selected row from AllStudents,
        /// pull out Id/FirstName/LastName/RegistrationNo, and add it to our in‐memory stuList.
        /// </summary>
        private void UpdateStudentBtnClick(object sender, RoutedEventArgs e)
        {
            if (AllStudents.SelectedItem is not DataRowView row)
            {
                MessageBox.Show(
                    "Please select a student from the left table.",
                    "Alert",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            try
            {
                // Column 0 = Id, 1 = FirstName, 2 = LastName, 3 = RegistrationNo
                int studentId = Convert.ToInt32(row.Row[0]);
                string firstName = row.Row[1].ToString();
                string lastName = row.Row[2].ToString();
                string regNo = row.Row[3].ToString();

                // Prevent adding the same student twice
                bool alreadyAdded = stuList.Exists(s => s.id == studentId);
                if (alreadyAdded)
                {
                    MessageBox.Show(
                        "This student is already selected.",
                        "Alert",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                // Add to the in‐memory list and refresh the SelectedStudents grid
                stuList.Add(new Stu(studentId, firstName, lastName, regNo));
                SelectedStudents.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error selecting student:\n" + ex.Message,
                    "Alert",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        /// <summary>
        /// The “<” button: remove the selected student from stuList.
        /// </summary>
        private void RemoveStu(object sender, RoutedEventArgs e)
        {
            if (SelectedStudents.SelectedItem is not Stu row)
            {
                MessageBox.Show(
                    "Please select a student from the right table.",
                    "Alert",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            try
            {
                stuList.Remove(row);
                SelectedStudents.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error removing student:\n" + ex.Message,
                    "Alert",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }

        /// <summary>
        /// Save button: create the group with Datepicker’s date, then assign each selected student.
        /// </summary>
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (stuList.Count == 0)
                {
                    MessageBox.Show(
                        "Select at least one student before saving.",
                        "Alert",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                // 1) Create the group (pass the date only)
                Group_Helper.AddGroup(Datepicker.SelectedDate.Value);

                // 2) Retrieve the newly‐created GroupId
                int newGroupId = Group_Helper.GetLastGroupId();

                // 3) Assign each selected student to that new group
                foreach (var s in stuList)
                {
                    Group_Helper.AddStudentToGroup(
                        newGroupId,
                        s.id,
                        3,              
                        DateTime.Now      
                    );
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "There was an error while adding students to the group:\n" + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
    }

    /// <summary>
    /// Simple DTO for each selected student.
    /// </summary>
    public class Stu
    {
        public int id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string RegistrationNo { get; }

        public Stu(int id, string fname, string lname, string regno)
        {
            this.id = id;
            FirstName = fname;
            LastName = lname;
            RegistrationNo = regno;
        }
    }
}
