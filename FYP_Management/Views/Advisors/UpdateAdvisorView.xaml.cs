using FYP_Management.HelperClasses;
using FYP_Management.Validations1;
using FYP_Management.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace FYP_Management.Views.Advisors
{
    /// <summary>
    /// Interaction logic for updateAdv.xaml
    /// </summary>
    public partial class UpdateAdvisorView : Window
    {
        private int PId;
        public UpdateAdvisorView(string FirstName, string LastName,string design ,int salary, string Contact, string email, DateTime dateTime, string gender, int PersonID)
        {
            InitializeComponent();
            CmboxGender.ItemsSource = Lookup.getGenders();
            DesignationCmBox.ItemsSource = Lookup.getDesignations();
            this.FirstName.Text = FirstName;
            this.LastName.Text = LastName;
            this.Salarytxtbox.Text = salary.ToString();
            this.ContactNo.Text = Contact;
            this.Email.Text = email;
            DatePicker.SelectedDate = dateTime;
            CmboxGender.SelectedValue = gender;
            DesignationCmBox.SelectedValue = design;
            PId = PersonID;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Donebtn_Click(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
                try
                {
                    Person person = new(PId,FirstName.Text, LastName.Text, ContactNo.Text, Email.Text, DatePicker.SelectedDate.Value, Lookup.getIndexFromValue(CmboxGender.SelectedValue.ToString()));
                    // Update the values of person and advisor in db
                    Person_Helper.UpdatePerson(person);
                    Advisor_Helper.UpdateAdvisor(Lookup.getIndexFromValue(DesignationCmBox.SelectedValue.ToString()), int.Parse(Salarytxtbox.Text), PId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There is an error while updating the record " + ex, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                this.Close();
            }
        }

        private bool validate()
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
            if (!ValidationsHelper.age16plus(DatePicker.SelectedDate.Value))
            {
                MessageBox.Show("Age is Below 16", "Alert", MessageBoxButton.OK, MessageBoxImage.Question);
                return false;
            }
            if (!int.TryParse(Salarytxtbox.Text,out int ans))
            {
                MessageBox.Show("Salary Must be a postive Number.", "Alert", MessageBoxButton.OK, MessageBoxImage.Question);
                return false;
            }
            return true;
        }

        private void Salarytxtbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = ValidationsHelper.NumberInput(e);
        }
    }
}
