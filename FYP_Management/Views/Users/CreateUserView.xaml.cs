using FYP_Management.HelperClasses;
using Microsoft.Data.SqlClient;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FYP_Management.Views.Users
{
    public partial class CreateUserView : UserControl
    {
        public CreateUserView()
        {
            InitializeComponent();
            LoadGenderComboBox();
        }

        private void LoadGenderComboBox()
        {
            // Populate the Gender ComboBox from your Lookup table
            // You should have a helper method for this. For now, we can hardcode.
            GenderComboBox.Items.Add(new ComboBoxItem { Content = "Male", Tag = 1 });
            GenderComboBox.Items.Add(new ComboBoxItem { Content = "Female", Tag = 2 });
            GenderComboBox.SelectedIndex = 0;
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            // --- 1. Validate Input ---
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                GenderComboBox.SelectedItem == null ||
                DobDatePicker.SelectedDate == null)
            {
                ErrorTextBlock.Text = "Please fill all required fields.";
                return;
            }

            using var conn = Config.GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();
            try
            {
                // Step A: Create the Person and get their new ID
                int newPersonId = Person_Helper.AddPersonAndGetId(
                    FirstNameTextBox.Text,
                    LastNameTextBox.Text,
                    ContactTextBox.Text,
                    EmailTextBox.Text,
                    DobDatePicker.SelectedDate.Value,
                    (int)((ComboBoxItem)GenderComboBox.SelectedItem).Tag,
                    conn, transaction
                );

                // Step B: Use the new ID to create the UserAccount
                UserAccount_Helper.CreateAdminUser(
                    newPersonId,
                    UsernameTextBox.Text,
                    PasswordBox.Password,
                    conn, transaction
                );

                // If both succeed, commit the transaction
                transaction.Commit();

                MessageBox.Show("Admin account created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                // If anything fails, roll back everything
                transaction.Rollback();
                MessageBox.Show($"Failed to create account. The operation was cancelled.\n\nError: {ex.Message}", "Transaction Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            FirstNameTextBox.Clear();
            LastNameTextBox.Clear();
            EmailTextBox.Clear();
            ContactTextBox.Clear();
            DobDatePicker.SelectedDate = null;
            GenderComboBox.SelectedIndex = 0;
            UsernameTextBox.Clear();
            PasswordBox.Clear();
            ErrorTextBlock.Text = "";
        }

        private void CreateAccountButton_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}