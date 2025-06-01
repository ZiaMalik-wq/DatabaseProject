using FYP_Management.HelperClasses;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FYP_Management
{
    public partial class AddAssignProject : Window
    {
        private DataView _mainView = null!;
        private DataView _coView = null!;
        private DataView _indView = null!;

        private int? _mainId, _coId, _indId;

        public AddAssignProject()
        {
            InitializeComponent();
            Datepicker.SelectedDate = DateTime.Now;

            foreach (var dg in new[]
            {
                GroupGrid, ProjectGrid, MainAdvGrid, coAdvGrid, IndAdvGrid,
                GroupMembersGrid, SelectedProjectGrid, SelectedMainAdvGrid,
                SelectedCoAdvGrid,  SelectedIndAdvGrid
            })
                dg.Loaded += (_, __) => HideFirstColumn((DataGrid)_);

            LoadGroups();
            LoadProjects();
            LoadMainAdvisors();
            LoadCoAdvisors();
            LoadIndustryAdvisors();
        }

        /* ---------------- data loading ---------------- */
        private void LoadGroups()
        {
            try { GroupGrid.ItemsSource = Group_Helper.GetGroupDetailsWithMembers().DefaultView; }
            catch (Exception ex) { Alert("groups", ex); }
        }
        private void LoadProjects()
        {
            try { ProjectGrid.ItemsSource = Project_Helper.GetProjectNotAssigned(SearchBar.Text).DefaultView; }
            catch (Exception ex) { Alert("projects", ex); }
        }
        private void LoadMainAdvisors()
        {
            try
            {
                _mainView = Advisor_Helper.GetAdvisors(searchMainAdvbox.Text).DefaultView;
                MainAdvGrid.ItemsSource = _mainView;
                ApplyAdvisorFilters();
            }
            catch (Exception ex) { Alert("main advisors", ex); }
        }
        private void LoadCoAdvisors()
        {
            try
            {
                _coView = Advisor_Helper.GetAdvisors(searchCoAdvbox.Text).DefaultView;
                coAdvGrid.ItemsSource = _coView;
                ApplyAdvisorFilters();
            }
            catch (Exception ex) { Alert("co-advisors", ex); }
        }
        private void LoadIndustryAdvisors()
        {
            try
            {
                _indView = Advisor_Helper.GetIndustryAdvisors(IndustryAdvSearchbox.Text).DefaultView;
                IndAdvGrid.ItemsSource = _indView;
                ApplyAdvisorFilters();
            }
            catch (Exception ex) { Alert("industry advisors", ex); }
        }

        /* ---------------- helpers ---------------- */
        private static void Alert(string what, Exception ex) =>
            MessageBox.Show($"Error loading {what}: {ex.Message}", "Alert",
                            MessageBoxButton.OK, MessageBoxImage.Warning);

        private static void HideFirstColumn(DataGrid dg)
        {
            if (dg.Columns.Count > 0) dg.Columns[0].Visibility = Visibility.Collapsed;
        }

        /* filters out IDs already selected for other roles */
        private void ApplyAdvisorFilters()
        {
            Filter(_mainView, _coId, _indId);
            Filter(_coView, _mainId, _indId);
            Filter(_indView, _mainId, _coId);

            static void Filter(DataView view, params int?[] exceptIds)
            {
                if (view == null) return;
                string key = view.Table.Columns[0].ColumnName;           // first-column name
                var ids = exceptIds.Where(id => id.HasValue).Select(id => id.Value);
                view.RowFilter = ids.Any() ? string.Join(" AND ", ids.Select(id => $"{key} <> {id}"))
                                           : string.Empty;
            }
        }

        /* ---------------- search & clear ---------------- */
        private void SearchBar_TextChanged(object s, TextChangedEventArgs e) => LoadProjects();
        private void searchMainAdvbox_TextChanged(object s, TextChangedEventArgs e) => LoadMainAdvisors();
        private void searchCoAdvbox_TextChanged(object s, TextChangedEventArgs e) => LoadCoAdvisors();
        private void IndustryAdvSearchbox_TextChanged(object s, TextChangedEventArgs e) => LoadIndustryAdvisors();

        private void Button_Click(object s, RoutedEventArgs e) => SearchBar.Text = "";
        private void Button_Click_1(object s, RoutedEventArgs e) => searchMainAdvbox.Text = "";
        private void Button_Click_2(object s, RoutedEventArgs e) => searchCoAdvbox.Text = "";
        private void Button_Click_3(object s, RoutedEventArgs e) => IndustryAdvSearchbox.Text = "";

        private void CloseBtn_Click(object s, RoutedEventArgs e) => Close();

        /* ---------------- DataGrid selections ---------------- */
        private void GroupGrid_SelectionChanged(object _, SelectionChangedEventArgs e)
        {
            if (GroupGrid.SelectedItem is not DataRowView row) { GroupMembersGrid.ItemsSource = null; return; }
            try { GroupMembersGrid.ItemsSource = Group_Helper.GetStudentsFromGroup(IntKey(row)).DefaultView; }
            catch (Exception ex) { Alert("group members", ex); }
        }

        private void ProjectGrid_SelectionChanged(object s, SelectionChangedEventArgs e) => ShowOne(ProjectGrid, SelectedProjectGrid);
        private void MainAdvGrid_SelectionChanged(object s, SelectionChangedEventArgs e) { _mainId = ShowOne(MainAdvGrid, SelectedMainAdvGrid); ApplyAdvisorFilters(); }
        private void CoAdvGrid_SelectionChanged(object s, SelectionChangedEventArgs e) { _coId = ShowOne(coAdvGrid, SelectedCoAdvGrid); ApplyAdvisorFilters(); }
        private void IndustryAdvGrid_SelectionChanged(object s, SelectionChangedEventArgs e) { _indId = ShowOne(IndAdvGrid, SelectedIndAdvGrid); ApplyAdvisorFilters(); }

        private static int? ShowOne(DataGrid source, DataGrid target)
        {
            if (source.SelectedItem is not DataRowView drv) { target.ItemsSource = null; return null; }
            DataTable t = drv.Row.Table.Clone();
            t.ImportRow(drv.Row);
            target.ItemsSource = t.DefaultView;
            HideFirstColumn(target);
            return IntKey(drv);
        }

        private static int IntKey(DataRowView drv) => Convert.ToInt32(drv.Row[0]);

        /* ---------------- assign button ---------------- */
        private void AssignBtn_Click(object s, RoutedEventArgs e)
        {
            if (!ValidateSelections()) return;
            try
            {
                int groupId = IntKey((DataRowView)GroupGrid.SelectedItem!);
                int projectId = IntKey((DataRowView)ProjectGrid.SelectedItem!);

                Group_Helper.AssignProjectToGroup(groupId, projectId, Datepicker.SelectedDate!.Value);
                Advisor_Helper.AssignAdvisor(_mainId!.Value, projectId, 11, DateTime.Now);
                Advisor_Helper.AssignAdvisor(_coId!.Value, projectId, 12, DateTime.Now);
                Advisor_Helper.AssignAdvisor(_indId!.Value, projectId, 14, DateTime.Now);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during assignment:\n{ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateSelections()
        {
            if (ProjectGrid.SelectedItem == null) return Warn("a Project");
            if (GroupGrid.SelectedItem == null) return Warn("a Group");
            if (_mainId == null) return Warn("a Main Advisor");
            if (_coId == null) return Warn("a Co-Advisor");
            if (_indId == null) return Warn("an Industry Advisor");
            return true;

            static bool Warn(string what)
            {
                MessageBox.Show($"Please select {what}.", "Alert",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }
    }
}
