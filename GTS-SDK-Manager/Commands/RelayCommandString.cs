using System;
using System.Windows.Input;

namespace SdkManger.UI 
{
   public class RelayCommandString : ICommand
    {
        private Action<string> _action;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayCommandString(Action<string> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _action((string)parameter);
        }
    }
}
