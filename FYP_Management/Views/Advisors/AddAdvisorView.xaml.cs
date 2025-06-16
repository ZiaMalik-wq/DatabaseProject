using FYP_Management.HelperClasses;
using FYP_Management.Validations1;
using System;
using System.Windows;
using System.Windows.Input;
using FYP_Management.Models;

namespace FYP_Management.Views.Advisors;
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

        // Use a single connection and transaction for the entire "all or nothing" operation
        using var conn = Config.GetConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();
        try
        {
            // Create the person object from the form inputs
            Person person = new(
                FirstName.Text,
                LastName.Text,
                ContactNo.Text,
                Email.Text,
                DatePicker.SelectedDate.Value,
                Lookup.getIndexFromValue(CmboxGender.SelectedValue.ToString())
            );
            int newPersonId = Person_Helper.AddPerson(person);

            Advisor_Helper.AddAdvisor(
                newPersonId,
                Lookup.getIndexFromValue(DesignationCmBox.SelectedValue.ToString()),
                int.Parse(Salarytxtbox.Text),
                conn, transaction
            );
            transaction.Commit();

            MessageBox.Show("Advisor created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
        }
        catch (FormatException)
        {
            transaction.Rollback();
            MessageBox.Show("Invalid salary format. Please enter numbers only.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
