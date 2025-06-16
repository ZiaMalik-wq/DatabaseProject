using FYP_Management.HelperClasses;
using System;
using System.Windows;

namespace FYP_Management
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1) Test DB
            TestDatabaseConnection();

            // 2) Show login as a modal dialog
            var login = new LoginWindow();
            bool? ok = login.ShowDialog();

            if (ok == true)
            {
                // 3a) Now switch shutdown mode so closing MainWindow will exit:
                ShutdownMode = ShutdownMode.OnMainWindowClose;

                // 3b) Create & register the real main window
                var main = new MainWindow();
                Current.MainWindow = main;
                main.Show();
            }
            else
            {
                // 4) Login cancelled or failed → exit now
                Shutdown();
            }
        }

        private void TestDatabaseConnection()
        {
            try
            {
                using var connection = Config.GetConnection();
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"A critical error occurred and the application cannot start.\n\nDetails: {ex.Message}",
                    "Startup Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                Shutdown();
            }
        }
    }
}
