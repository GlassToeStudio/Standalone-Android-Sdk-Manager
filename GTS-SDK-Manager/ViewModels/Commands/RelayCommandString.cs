using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GTS_SDK_Manager 
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
