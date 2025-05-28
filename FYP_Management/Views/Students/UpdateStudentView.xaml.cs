using FYP_MS.HelperClasses;
using FYP_MS.Validations1;
using System;
using System.Windows;
using FYP_MS.Models;

namespace FYP_MS.Views.Students
{
    /// <summary>
    /// Interaction logic for UpdateStudent.xaml
    /// </summary>
    public partial class UpdateStudentView : Window
    {
        private int PId;
        public UpdateStudentView(string FirstName, string LastName, string regno, string Contact, string email, DateTime dateTime, string gender, int PersonID)
        {
            InitializeComponent();
            // Assign values to the input fields
            this.FirstName.Text = FirstName;
            this.LastName.Text = LastName;
            this.RegNo.Text = regno;
            this.ContactNo.Text = Contact;
            this.Email.Text = email;
            Datepicker.SelectedDate = dateTime;
            CmboxGender.ItemsSource = Lookup.getGenders();
            CmboxGender.SelectedValue = gender;
            PId = PersonID;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Donebtn_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                try
                {
                    Person person = new(PId, FirstName.Text, LastName.Text, ContactNo.Text, Email.Text, Datepicker.SelectedDate.Value, Lookup.getIndexFromValue(CmboxGender.SelectedValue.ToString()));
                    // update person and student in db
                    Person_Helper.UpdatePerson(person);
                    Stu_Helper.UpdateStudent(RegNo.Text, PId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There is an error while updating the record " + ex, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                this.Close();
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
