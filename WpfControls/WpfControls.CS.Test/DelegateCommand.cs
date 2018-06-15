using System;
using System.Windows.Input;

namespace WpfControls.CS.Test
{
    public class DelegateCommand : ICommand
    {

        private readonly Action<object> _executeMethod;

        private readonly Func<object, bool> _canExecuteMethod;
        public DelegateCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            _executeMethod = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod));
            _canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteMethod == null)
            {
                return true;
            }
            return _canExecuteMethod(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _executeMethod(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}