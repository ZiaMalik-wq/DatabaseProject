using FYP_Management.HelperClasses;
using FYP_Management.Views.Groups;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FYP_Management
{
    /// <summary>
    /// Interaction logic for GroupCRUD.xaml
    /// </summary>
    public partial class GroupCRUD : UserControl
    {
        public GroupCRUD()
        {
            InitializeComponent();
            LoadData();
            Grid.Loaded += Grid_Loaded;
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            AddGrp addGrp = new();
            addGrp.ShowDialog();
            LoadData();
            
        }
        private void UpdateGroup_Click(object sender, RoutedEventArgs e)
        {
            // Ensure a group is selected in the grid
            if (Grid.SelectedItem is not DataRowView row)
            {
                MessageBox.Show(
                    "Please select a group to update.",
                    "Alert",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            try
            {
                // Extract the GroupId (column 0) from the selected row
                int groupId = Convert.ToInt32(row.Row[0]);

                // Open the UpdateGroup window, passing the selected GroupId
                var editWindow = new UpdateGroup(groupId);
                editWindow.ShowDialog();

                // After closing the edit window, refresh the main grid
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error opening Update Group window:\n" + ex.Message,
                    "Alert",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }


        private void LoadData()
        {
            try
            {
                Grid.ItemsSource = Group_Helper.GetGroupDetails().DefaultView;
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
        private void Grid_Loaded(object sender = null, RoutedEventArgs e = null)
        {
            // try { Grid.Columns[0].Visibility = Visibility.Collapsed; } catch { }
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            // data changes as text changes
            if (SearchBar.Text == "")
            {
                LoadData();
            }
            else if (int.TryParse(SearchBar.Text.ToString(),out int indd))
            {
                Grid.ItemsSource = Group_Helper.SearchGroup(int.Parse(SearchBar.Text.ToString())).DefaultView;
            }
        }

        private void Grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row = Grid.SelectedItem as DataRowView;
            if (row == null)
            {
                MessageBox.Show("Please Select a value from Table", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {
                    //  Get the selected group table from database
                    StudentsGrid.ItemsSource = Group_Helper.GetStudentsFromGroup(int.Parse(row.Row[0].ToString())).DefaultView;
                    StudentsGrid.Columns[0].Visibility = Visibility.Hidden;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Getting Data from Database " + ex, "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
