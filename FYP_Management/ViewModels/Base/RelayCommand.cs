// ViewModels/Base/RelayCommand.cs
using System;
using System.Windows.Input;

namespace FYP_MS.ViewModels.Base;
public sealed class RelayCommand : ICommand
{
    readonly Action<object> exec; readonly Predicate<object> can;
    public RelayCommand(Action<object> e, Predicate<object> c = null) { exec = e; can = c; }
    public bool CanExecute(object p) => can?.Invoke(p) ?? true;
    public void Execute(object p) => exec(p);
    public event EventHandler CanExecuteChanged
    { add => CommandManager.RequerySuggested += value; remove => CommandManager.RequerySuggested -= value; }
}
