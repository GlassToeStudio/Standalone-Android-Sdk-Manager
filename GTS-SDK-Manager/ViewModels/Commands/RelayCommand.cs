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
            Console.WriteLine("Im created " + action.Method.Name);
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("Im Executin");
            _action.Invoke();
        }
    }
}
