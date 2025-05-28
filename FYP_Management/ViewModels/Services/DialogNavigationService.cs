using FYP_MS.Services;
using FYP_MS.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;

public class DialogNavigationService : IDialogService
{
    private readonly IServiceProvider _provider;

    public DialogNavigationService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void ShowDialog<TView, TViewModel>()
        where TView : Window
        where TViewModel : ViewModelBase
    {
        var viewModel = ActivatorUtilities.CreateInstance<TViewModel>(_provider);
        var window = ActivatorUtilities.CreateInstance<TView>(_provider);

        window.DataContext = viewModel;
        window.ShowDialog();
    }

    public void Close(object viewModel)
    {
        foreach (Window win in Application.Current.Windows)
        {
            if (win.DataContext == viewModel)
            {
                win.Close();
                break;
            }
        }
    }

    public void ShowError(string msg, string? title = null) =>
        MessageBox.Show(msg, title ?? "Error", MessageBoxButton.OK, MessageBoxImage.Error);
}
