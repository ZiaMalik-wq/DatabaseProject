
using FYP_Management.HelperClasses;
using FYP_Management.Validations1;
using System;
using System.Windows;
using FYP_Management.Models;

namespace FYP_Management.Views.Students
{
    /// <summary>
    /// Interaction logic for AddStu.xaml
    /// </summary>
    /// 

    public partial class AddStudentView : Window
    {
        public AddStudentView()
        {
            InitializeComponent();
            Datepicker.SelectedDate = DateTime.Now;
            // Assign values to combox
            CmboxGender.ItemsSource = Lookup.getGenders();
            CmboxGender.SelectedIndex = 0;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void donebtn_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;

            // Ensure date is selected
            if (Datepicker.SelectedDate == null)
            {
                MessageBox.Show("Please select a valid date of birth.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Use a single connection and transaction for the entire operation
            using var conn = Config.GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();
            try
            {
                // Create the person object from the form
                Person person = new(
                    FirstName.Text,
                    LastName.Text,
                    ContactNo.Text,
                    Email.Text,
                    Datepicker.SelectedDate.Value,
                    Lookup.getIndexFromValue(CmboxGender.SelectedValue.ToString())
                );

                int newPersonId = Person_Helper.AddPerson(person);
                Stu_Helper.AddStudent(newPersonId, RegNo.Text, conn, transaction);
                transaction.Commit();

                MessageBox.Show("Student created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                MessageBox.Show(
                    "The operation failed and all changes have been undone.\n\nError: " + ex.Message,
                    "Transaction Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private bool Validate()
        {
            if (!ValidationsHelper.name(FirstName.Text) || !ValidationsHelper.name(LastName.Text))
            {
                MessageBox.Show("Name should atleast be 3 characters", "Alert", MessageBoxButton.OK, MessageBoxImage.Question);
                return false;
            }
            if (!ValidationsHelper.contact(ContactNo.Text))
            {
                MessageBox.Show("Contact Number length Must be 11 and should not Contain characters", "Alert", MessageBoxButton.OK, MessageBoxImage.Question);
                return false;
            }
            if (!ValidationsHelper.email(Email.Text))
            {
                MessageBox.Show("InValid Email Address", "Alert", MessageBoxButton.OK, MessageBoxImage.Question);
                return false;
            }
            if (!ValidationsHelper.age16plus(Datepicker.SelectedDate.Value))
            {
                MessageBox.Show("Age is Below 16", "Alert", MessageBoxButton.OK, MessageBoxImage.Question);
                return false;
            }
            return true;
        }

    }
}