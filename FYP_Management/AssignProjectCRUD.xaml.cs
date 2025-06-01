using FYP_Management.HelperClasses;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FYP_Management
{
    /// <summary>
    /// Interaction logic for AssignProjectCRUD.xaml
    /// </summary>
    public partial class AssignProjectCRUD : UserControl
    {
        public AssignProjectCRUD()
        {
            InitializeComponent();
            LoadData();

        }
        private void LoadData()
        {
            try
            {
                Grid.ItemsSource = Project_Helper.GetAssignedProjectDetails(SearchBar.Text).DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data from Database " + ex, "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void ClearTxt_Click(object sender, RoutedEventArgs e)
        {
            SearchBar.Text = "";
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            // data changes as text changes
            LoadData();
        }

        private void UpdateProjectDetail(object sender, RoutedEventArgs e)
        {
            // Update Group Project Details
        }
        private void AssignProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            AddAssignProject assignProject = new AddAssignProject();
            assignProject.ShowDialog();
            LoadData();
        }
    }
}
