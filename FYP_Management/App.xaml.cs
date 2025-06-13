#nullable enable
using FYP_Management.HelperClasses;
using Microsoft.Data.SqlClient;
using System;
using System.IO;
using System.Windows;

namespace FYP_Management
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            TestDatabaseConnection();
            base.OnStartup(e);
        }

        private void TestDatabaseConnection()
        {
            try
            {
                using var connection = Config.GetConnection();
                connection.Open(); // Will auto-attach the .mdf file
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"Database file not found.\n{ex.Message}", "Missing Database",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                Shutdown();
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database connection error:\n{ex.Message}", "Critical Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error:\n{ex.Message}", "Unexpected Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
    }
}
