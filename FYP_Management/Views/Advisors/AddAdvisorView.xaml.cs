using FYP_MS.HelperClasses;
using FYP_MS.Validations1;
using System;
using System.Windows;
using System.Windows.Input;
using FYP_MS.Models;

namespace FYP_MS.Views.Advisors;
public partial class AddAdvisorView : Window
{
    public AddAdvisorView()
    {
        InitializeComponent();
        CmboxGender.ItemsSource = Lookup.getGenders();
        DesignationCmBox.ItemsSource = Lookup.getDesignations();
        CmboxGender.SelectedIndex = 0;
        DesignationCmBox.SelectedIndex = 0;
        DatePicker.SelectedDate = DateTime.Now;
    }

    private void donebtn_Click(object sender, RoutedEventArgs e)
    {
        if (!validate())
            return;

        try
        {
            // Create Person object using form inputs
            Person person = new Person(
                FirstName.Text,
                LastName.Text,
                ContactNo.Text,
                Email.Text,
                DatePicker.SelectedDate.Value,
                Lookup.getIndexFromValue(CmboxGender.SelectedValue.ToString())
            );

            // Add person to database
            Person_Helper.AddPerson(person);

            // Add advisor using the inserted person's ID
            Advisor_Helper.AddAdvisor(
                Person_Helper.getLastPersonId(),
                Lookup.getIndexFromValue(DesignationCmBox.SelectedValue.ToString()),
                int.Parse(Salarytxtbox.Text)
            );

            // Close only this dialog
            this.DialogResult = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "There is an error while saving the record:\n" + ex.Message,
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
    }


    private void Close_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = false;
    }

    private bool validate()
    {
        if (!ValidationsHelper.name(FirstName.Text) ||
            !ValidationsHelper.name(LastName.Text))
        {
            MessageBox.Show("Name should be at least 3 characters", "Alert",
                            MessageBoxButton.OK, MessageBoxImage.Question);
            return false;
        }
        if (!ValidationsHelper.contact(ContactNo.Text))
        {
            MessageBox.Show("Contact number must be 11 digits", "Alert",
                            MessageBoxButton.OK, MessageBoxImage.Question);
            return false;
        }
        if (!ValidationsHelper.email(Email.Text))
        {
            MessageBox.Show("Invalid email address", "Alert",
                            MessageBoxButton.OK, MessageBoxImage.Question);
            return false;
        }
        if (!ValidationsHelper.age16plus(DatePicker.SelectedDate.Value))
        {
            MessageBox.Show("Age must be 16 or above", "Alert",
                            MessageBoxButton.OK, MessageBoxImage.Question);
            return false;
        }
        if (!int.TryParse(Salarytxtbox.Text, out _))
        {
            MessageBox.Show("Salary must be a positive number", "Alert",
                            MessageBoxButton.OK, MessageBoxImage.Question);
            return false;
        }
        return true;
    }

    private void Salarytxtbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = ValidationsHelper.NumberInput(e);
    }

    private void FirstName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {

    }
}
