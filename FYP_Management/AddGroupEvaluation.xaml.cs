using FYP_Management.HelperClasses;
using FYP_Management.Validations1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for AddGroupEvaluation.xaml
    /// </summary>
    public partial class AddGroupEvaluation : Window
    {
        private int GroupId = 0;
        private int EvlId = 0;
        public AddGroupEvaluation(int groupId, int evlId, string evlName)
        {
            InitializeComponent();
            EvlDatepicker.SelectedDate = DateTime.Now;
            EvlName.Text = evlName;
            GroupId = groupId;
            EvlId = evlId;
            TotalMarks.Text = Evaluation_Helper.GetTotalMarksofEvaluation(evlId);
        }

        private void ObtainedMarks_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = ValidationsHelper.NumberInput(e);
        }

        private void TotalMarks_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = ValidationsHelper.NumberInput(e);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void donebtn_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;   // early-out if the form isn’t valid

            try
            {
                // safer numeric parse
                if (!int.TryParse(ObtainedMarks.Text, out var marks))
                {
                    MessageBox.Show("Obtained Marks must be a whole number.",
                                    "Validation", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                Evaluation_Helper.AddGroupEvaluation(
                    GroupId,
                    EvlId,
                    marks,
                    EvlDatepicker.SelectedDate!.Value);
                this.Close();
            }
            catch (Exception ex)
            {
                // concise, user-friendly message
                MessageBox.Show("Error saving data: " + ex.Message,
                                "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool Validate()
        {
            if (!int.TryParse(ObtainedMarks.Text.ToString(), out int u))
            {
                MessageBox.Show("Marks must be integar.", "Alert", MessageBoxButton.OK, MessageBoxImage.Question);
                return false;
            }
            //if (int.Parse(ObtainedMarks.Text.ToString()) > int.Parse(TotalMarks.Text.ToString()))
            //{
            //    MessageBox.Show("Obtained Marks cannot be more than Total Marks.", "Alert", MessageBoxButton.OK, MessageBoxImage.Question);
            //    return false;
            //}
            return true;
        }
    }
}
