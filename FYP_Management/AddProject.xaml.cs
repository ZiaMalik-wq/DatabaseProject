using FYP_Management.Validations1;
using System;
using System.Windows;

namespace FYP_Management
{
    /// <summary>
    /// Interaction logic for AddProject.xaml
    /// </summary>
    public partial class AddProject : Window
    {
        public AddProject()
        {
            InitializeComponent();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Donebtn_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                try
                {
                    HelperClasses.Project_Helper.AddProject(TopicTxtBox.Text, DescriptionTextBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There is an error while Adding Value to DataBase" + ex, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                this.Close();
            }
        }
        private bool Validate()
        {
            if (!ValidationsHelper.name(TopicTxtBox.Text))
            {
                MessageBox.Show("Title should atleast be 3 characters.", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if(!ValidationsHelper.description(DescriptionTextBox.Text))
            {
                MessageBox.Show("Description Should be more than 50 characters.", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}
