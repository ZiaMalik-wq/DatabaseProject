using FYP_Management.HelperClasses;
using System.Windows;

namespace FYP_Management
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text.Trim();
            var password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorTextBlock.Text = "Username and password cannot be empty.";
                return;  // stay on the dialog
            }

            if (UserAccount_Helper.VerifyLogin(username, password))
            {
                // Success → set DialogResult to true
                // (this will automatically close the window when ShowDialog() returns)
                this.DialogResult = true;
            }
            else
            {
                // Failure → show an error and DO NOT set DialogResult or Close()
                ErrorTextBlock.Text = "Invalid username or password.";
                UsernameTextBox.Focus();
            }
        }

    }
}