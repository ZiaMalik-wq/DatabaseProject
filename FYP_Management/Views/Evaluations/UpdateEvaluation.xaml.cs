using FYP_Management.HelperClasses;
using FYP_Management.Validations1;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace FYP_Management
{
    /// <summary>
    /// Interaction logic for UpdateEvaluation.xaml
    /// </summary>
    public partial class UpdateEvaluation : Window
    {
        private int EvaluationId;
        private readonly int _originalWeightage;

        public UpdateEvaluation(string name,int totalMarks,int weightAge,int evlId)
        {
            InitializeComponent();
            EvlName.Text = name;
            TotalMarks.Text = totalMarks.ToString();
            WeightAgetxtbox.Text = weightAge.ToString();
            EvaluationId = evlId;
            _originalWeightage = weightAge;
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

        private void donebtn_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                try
                {
                    Evaluation_Helper.UpdateEvaluation(EvlName.Text.ToString(), int.Parse(TotalMarks.Text.ToString()), int.Parse(WeightAgetxtbox.Text.ToString()),EvaluationId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There is an error while updating the record " + ex, "Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                this.Close();
            }
        }
        private bool Validate()
        {
            if (!ValidationsHelper.name(EvlName.Text))
            {
                MessageBox.Show("Name should be at least 3 characters",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Question);
                return false;
            }

            if (!ValidationsHelper.GreaterThanZero(int.Parse(TotalMarks.Text)))
            {
                MessageBox.Show("Marks must be a positive number.",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Question);
                return false;
            }

            int newWeightage = int.Parse(WeightAgetxtbox.Text);
            int currentSum = Evaluation_Helper.GetAssigedPercentageEvaluations();
            int prospectiveTotal = currentSum - _originalWeightage + newWeightage;

            if (prospectiveTotal > 100)
            {
                MessageBox.Show($"Total weightage cannot exceed 100%. " +
                                $"Current sum would become {prospectiveTotal}.",
                                "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

    }
}
