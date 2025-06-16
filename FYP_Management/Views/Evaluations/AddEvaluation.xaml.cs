using FYP_Management.HelperClasses;
using FYP_Management.Validations1;
using System;
using System.Windows;
using System.Windows.Input;


namespace FYP_Management
{
    /// <summary>
    /// Interaction logic for AddEvaluation.xaml
    /// </summary>
    public partial class AddEvaluation : Window
    {
        public AddEvaluation()
        {
            InitializeComponent();
        }

        private void TotalMarks_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = ValidationsHelper.NumberInput(e);
        }

        private void WeightAgetxtbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = ValidationsHelper.NumberInput(e);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Donebtn_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                try
                {
                    Evaluation_Helper.insertEvaluation(EvlName.Text.ToString(), int.Parse(TotalMarks.Text.ToString()), int.Parse(WeightAgetxtbox.Text.ToString()));
                }
                catch(Exception ex)
                {
                    MessageBox.Show("There is an error while updating the record " + ex, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                this.Close();
            }
        }
        private bool Validate()
        {
            // ――― basic field checks ―――
            if (!ValidationsHelper.name(EvlName.Text))
            {
                MessageBox.Show("Name should be at least 3 characters.",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Question);
                return false;
            }

            if (!ValidationsHelper.GreaterThanZero(int.Parse(TotalMarks.Text)))
            {
                MessageBox.Show("Marks must be a positive number.",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Question);
                return false;
            }

            // ――― weightage guard ―――
            if (!int.TryParse(WeightAgetxtbox.Text, out int newWeightage) || newWeightage <= 0)
            {
                MessageBox.Show("Weightage must be a positive number.",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Question);
                return false;
            }

            int currentSum = Evaluation_Helper.GetAssigedPercentageEvaluations();
            int prospectiveTotal = currentSum + newWeightage;                

            if (prospectiveTotal > 100)
            {
                MessageBox.Show($"Total weightage cannot exceed 100 %. " +
                                $"Adding {newWeightage}% would bring the total to {prospectiveTotal}%.",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

    }
}
