using System;
using System.Windows.Input;

namespace GTS_SDK_Manager
{
    class RelayCommand : ICommand
    {
        private readonly Action _action;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _action?.Invoke();
        }
    }
}
