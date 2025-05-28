using FYP_MS.HelperClasses;
using System.Windows;
using System.Windows.Controls;
using FYP_MS.Views.Students;

namespace FYP_MS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StuBtn_Click(object sender, RoutedEventArgs e)
        {
            var studentView = new StudentMainView();
            mainField.Children.Clear();
            mainField.Children.Add(studentView);
        }

        private void AdvBtn_Click(object sender, RoutedEventArgs e)
        {
            var advMenu = new AdvisorMenu();
            mainField.Children.Clear();
            mainField.Children.Add(advMenu);
        }

        private void Projectbtn_Click(object sender, RoutedEventArgs e)
        {
            var projectCrud = new ProjectCRUD();
            mainField.Children.Clear();
            mainField.Children.Add(projectCrud);
        }

        private void GroupManagebtn_Click(object sender, RoutedEventArgs e)
        {
            var groupCrud = new GroupCRUD();
            mainField.Children.Clear();
            mainField.Children.Add(groupCrud);
        }

        private void AssignProjectbtn_Click(object sender, RoutedEventArgs e)
        {
            var assignProject = new AssignProjectCRUD();
            mainField.Children.Clear();
            mainField.Children.Add(assignProject);
        }

        private void EvalutionBtn_Click(object sender, RoutedEventArgs e)
        {
            var evalCrud = new EvaluationCRUD();
            mainField.Children.Clear();
            mainField.Children.Add(evalCrud);
        }

        private void GroupEvaluation_Click(object sender, RoutedEventArgs e)
        {
            var groupEvl = new GroupEvaluation();
            mainField.Children.Clear();
            mainField.Children.Add(groupEvl);
        }
    }
}
