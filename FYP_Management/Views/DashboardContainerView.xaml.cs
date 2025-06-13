using FYP_Management.Views.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FYP_Management.Views
{
    /// <summary>
    /// Interaction logic for DashboardContainerView.xaml
    /// </summary>
    public partial class DashboardContainerView : UserControl
    {
        public DashboardContainerView()
        {
            InitializeComponent();
        }

        private void AdvisorWorkloadBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadAdvisorWorkloadChart();
        }

        private void LoadAdvisorWorkloadChart()
        {
            chartDisplayArea.Children.Clear();
            chartDisplayArea.Children.Add(new AdvisorWorkloadView());
        }

        private void StudentGenderBtn_Click(object sender, RoutedEventArgs e)
        {
            chartDisplayArea.Children.Clear();
            chartDisplayArea.Children.Add(new StudentGenderView());
        }
    }
}
