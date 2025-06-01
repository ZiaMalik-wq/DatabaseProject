using FYP_Management.HelperClasses;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using FYP_Management.Views.Advisors;

namespace FYP_Management
{
    /// <summary>
    /// Interaction logic for AdvisorMenu.xaml
    /// </summary>
    public partial class AdvisorMenu : UserControl
    {
        public AdvisorMenu()
        {
            InitializeComponent();
            LoadAdvisors();
            Grid.Loaded += Grid_Loaded;
        }
        private void LoadAdvisors()
        {
            try
            {
                // changes as text changes in the textbox
                Grid.ItemsSource = Advisor_Helper.Search(SearchBar.Text).DefaultView;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading data from Database " + e, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Grid_Loaded(object sender = null, RoutedEventArgs e = null)
        {
            Grid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void AddAvisorBtn(object sender, RoutedEventArgs e)
        {
            var dlg = new AddAdvisorView
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                LoadAdvisors();

            }
        }

        private void UpdateAdvBtnClick(object sender, RoutedEventArgs e)
        {
            // Edit Advisor Menu with values given from the selected column
            if (Grid.SelectedItem is not DataRowView row)
            {
                MessageBox.Show("Please Select a Row From the Table", "Alert",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                // Passing value to the next form to update them
                try
                {
                    UpdateAdvisorView ustu = new(row.Row.ItemArray[1].ToString(), row.Row.ItemArray[2].ToString(), row.Row.ItemArray[3].ToString(), int.Parse(row.Row.ItemArray[4].ToString()), row.Row.ItemArray[5].ToString(), (row.Row.ItemArray[6].ToString()), (DateTime)row.Row.ItemArray[7], (row.Row.ItemArray[8].ToString()), (int)row.Row.ItemArray[0]);
                    ustu.ShowDialog();
                    LoadAdvisors();
                    Grid_Loaded();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("There is an error while getting value from table. "+ex, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            // data changes as text changes
            LoadAdvisors();
            Grid_Loaded();
        }

        private void ClearText_Click(object sender, RoutedEventArgs e)
        {
            SearchBar.Text = "";
        }

        private void DeleteAdvBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem is not DataRowView row)
            {
                MessageBox.Show("Please Select a Row From the Table", "Alert",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int id = (int)row.Row.ItemArray[0];

            var confirm = MessageBox.Show(
                "Are you sure you want to delete this advisor?\n",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            if (confirm != MessageBoxResult.Yes) return;
            try
            {
                Advisor_Helper.DeleteAdvisor(id);
                LoadAdvisors();
                Grid_Loaded();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting advisor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
