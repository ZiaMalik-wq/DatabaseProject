using FYP_Management.Documents;
using FYP_Management.HelperClasses;
using FYP_Management.Views.Groups;
using Microsoft.Win32;
using QuestPDF.Fluent;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FYP_Management
{
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
                int groupId = Convert.ToInt32(row.Row[0]);
                var editWindow = new UpdateGroup(groupId);
                editWindow.ShowDialog();
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
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBar.Text == "")
            {
                LoadData();
            }
            else if (int.TryParse(SearchBar.Text.ToString(), out int indd))
            {
                Grid.ItemsSource = Group_Helper.SearchGroup(int.Parse(SearchBar.Text.ToString())).DefaultView;
            }
        }

        private void Grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Grid.SelectedItem is not DataRowView row)
            {
                MessageBox.Show("Please Select a value from Table", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {
                    StudentsGrid.ItemsSource = Group_Helper.GetStudentsFromGroup(int.Parse(row.Row[0].ToString())).DefaultView;
                    StudentsGrid.Columns[0].Visibility = Visibility.Hidden;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Getting Data from Database " + ex, "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedItem is not DataRowView selectedRow)
            {
                MessageBox.Show("Please select a group from the list first.", "No Group Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int groupId = Convert.ToInt32(selectedRow.Row[0]);

            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    FileName = $"Group_{groupId}_Grade_Report.pdf",
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    Title = "Save Grade Report"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var reportModel = Report_Helper.GetGroupReportData(groupId);
                    var document = new GroupReportDocument(reportModel);
                    document.GeneratePdf(saveFileDialog.FileName);

                    var result = MessageBox.Show("Report generated successfully. Would you like to open it?", "Success", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo(saveFileDialog.FileName) { UseShellExecute = true });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while generating the report.\n\nDetails: {ex.Message}", "Report Generation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}