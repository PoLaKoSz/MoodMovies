using System;
using System.Windows.Input;

namespace MoodMovies.Resources
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _executeMethod;
        private readonly Func<object, bool> _canExecuteMethod;

        public RelayCommand(Action<object> ExecuteMethod, Func<object, bool> CanExecuteMethod)
        {
            _executeMethod = ExecuteMethod;
            _canExecuteMethod = CanExecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteMethod != null)
            {
                return _canExecuteMethod(parameter);
            }
            else
            {
                return false;
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _executeMethod(parameter);
        }
    }
}