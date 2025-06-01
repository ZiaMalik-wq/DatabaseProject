using FYP_Management.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FYP_Management.Views.Groups
{
    /// <summary>
    /// Interaction logic for UpdateGroup.xaml
    /// </summary>
    public partial class UpdateGroup : Window
    {
        private readonly int _groupId;

        // In‐memory lists for managing students
        private List<Stu> stuList;    // Students currently in this group
        private List<Stu> allStuds;   // Available students: not in any group, plus those in this one

        // Keep original student IDs to diff on Save
        private List<int> originalStudentIds;

        public UpdateGroup(int groupId)
        {
            InitializeComponent();
            _groupId = groupId;

            // 1) Load group’s existing creation date
            var dtGroup = Group_Helper.SearchGroup(_groupId);
            if (dtGroup.Rows.Count > 0)
            {
                Datepicker.SelectedDate = Convert.ToDateTime(dtGroup.Rows[0]["Creation Date"]);
            }
            else
            {
                Datepicker.SelectedDate = DateTime.Now;
            }

            // 2) Load students already assigned to this group
            var dtInGroup = Group_Helper.GetStudentsFromGroup(_groupId);
            stuList = dtInGroup.AsEnumerable()
                               .Select(r => new Stu(
                                   Convert.ToInt32(r["id"]),
                                   r["FirstName"].ToString(),
                                   r["LastName"].ToString(),
                                   r["RegistrationNo"].ToString()
                               ))
                               .ToList();

            // Store original IDs for comparison on Save
            originalStudentIds = stuList.Select(s => s.id).ToList();

            SelectedStudents.ItemsSource = stuList;

            // 3) Load students not in any group
            var dtNotInAny = Group_Helper.GetStudentsNotInAnyGroup("%");
            allStuds = dtNotInAny.AsEnumerable()
                                 .Select(r => new Stu(
                                     Convert.ToInt32(r["id"]),
                                     r["FirstName"].ToString(),
                                     r["LastName"].ToString(),
                                     r["RegistrationNo"].ToString()
                                 ))
                                 .ToList();

            // Add back those already in this group so they appear on left as well
            foreach (var s in stuList)
            {
                if (!allStuds.Exists(x => x.id == s.id))
                    allStuds.Add(s);
            }

            AllStudents.ItemsSource = allStuds;
        }

        /// <summary>
        /// Reload “AllStudents” whenever search text changes.
        /// </summary>
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadAvailableStudents();
        }

        /// <summary>
        /// Clear the search text.
        /// </summary>
        private void ClearTxt_Click(object sender, RoutedEventArgs e)
        {
            SearchBar.Text = "";
        }

        /// <summary>
        /// Reloads available students, including group members.
        /// </summary>
        private void LoadAvailableStudents()
        {
            try
            {
                string pattern = $"%{SearchBar.Text}%";
                var dtNotInAny = Group_Helper.GetStudentsNotInAnyGroup(pattern);
                allStuds = dtNotInAny.AsEnumerable()
                                     .Select(r => new Stu(
                                         Convert.ToInt32(r["id"]),
                                         r["FirstName"].ToString(),
                                         r["LastName"].ToString(),
                                         r["RegistrationNo"].ToString()
                                     ))
                                     .ToList();

                // Ensure current group members are also listed
                foreach (var s in stuList)
                {
                    if (!allStuds.Exists(x => x.id == s.id))
                        allStuds.Add(s);
                }

                AllStudents.ItemsSource = allStuds;
                AllStudents.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading available students: " + ex.Message,
                    "Alert",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// “>” button or double‐click on AllStudents: add selected student to stuList.
        /// </summary>
        private void UpdateStudentBtnClick(object sender, RoutedEventArgs e)
        {
            #nullable enable
            DataRowView? drv = AllStudents.SelectedItem as DataRowView;
            Stu? stuObj = AllStudents.SelectedItem as Stu;
            #nullable disable

            if (drv == null && stuObj == null)
            {
                MessageBox.Show(
                    "Please select a student to add.",
                    "Alert",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            int studentId;
            string firstName, lastName, regNo;

            if (drv != null)
            {
                studentId = Convert.ToInt32(drv.Row["id"]);
                firstName = drv.Row["FirstName"].ToString();
                lastName = drv.Row["LastName"].ToString();
                regNo = drv.Row["RegistrationNo"].ToString();
            }
            else // stuObj not null
            {
                studentId = stuObj.id;
                firstName = stuObj.FirstName;
                lastName = stuObj.LastName;
                regNo = stuObj.RegistrationNo;
            }

            // Prevent duplicates
            if (stuList.Exists(s => s.id == studentId))
            {
                MessageBox.Show(
                    "This student is already in the group.",
                    "Alert",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            stuList.Add(new Stu(studentId, firstName, lastName, regNo));
            SelectedStudents.Items.Refresh();
        }

        /// <summary>
        /// “<” button or double‐click on SelectedStudents: remove selected Stu from stuList.
        /// </summary>
        private void RemoveStu(object sender, RoutedEventArgs e)
        {
            if (SelectedStudents.SelectedItem is not Stu row)
            {
                MessageBox.Show(
                    "Please select a student to remove.",
                    "Alert",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            stuList.Remove(row);
            SelectedStudents.Items.Refresh();
        }

        /// <summary>
        /// Save button: update group date and synchronize student membership.
        /// </summary>
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1) Update group’s creation date
                Group_Helper.UpdateGroup(_groupId, Datepicker.SelectedDate.Value);

                // 2) Determine which students to delete vs. add
                var newStudentIds = stuList.Select(s => s.id).ToList();

                // a) Delete: in original ∖ new
                var toDelete = originalStudentIds.Except(newStudentIds);
                foreach (var sid in toDelete)
                {
                    Group_Helper.DeleteStudentFromGroup(_groupId, sid);
                }

                // b) Add: in new ∖ original
                var toAdd = newStudentIds.Except(originalStudentIds);
                foreach (var sid in toAdd)
                {
                    Group_Helper.AddStudentToGroup(_groupId, sid, true, DateTime.Now);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error saving group changes:\n" + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Cancel button: close without saving.
        /// </summary>
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    /// <summary>
    /// Simple DTO for binding selected students.
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
