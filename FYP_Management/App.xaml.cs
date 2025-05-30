#nullable enable
using System.Windows;
using Microsoft.Extensions.Configuration;

namespace FYP_Management
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IConfiguration? Config { get; private set; }

        public App()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
