using FYP_MS.ViewModels.Base;

namespace FYP_MS.Services
{
    public interface IDialogService
    {
        /// <summary>
        /// Shows a dialog window for the specified View and ViewModel types.
        /// </summary>
        /// <typeparam name="TView">The Window to show.</typeparam>
        /// <typeparam name="TViewModel">The ViewModel to use as DataContext.</typeparam>
        void ShowDialog<TView, TViewModel>()
            where TView : System.Windows.Window
            where TViewModel : ViewModelBase;

        /// <summary>
        /// Closes the dialog associated with the given ViewModel.
        /// </summary>
        /// <param name="viewModel">The ViewModel associated with the open Window.</param>
        void Close(object viewModel);

        /// <summary>
        /// Shows a simple error message box.
        /// </summary>
        /// <param name="msg">The error message.</param>
        /// <param name="title">Optional title for the dialog.</param>
        void ShowError(string msg, string? title = null);
    }
}
