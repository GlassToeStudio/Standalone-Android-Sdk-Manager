using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GTS_SDK_Manager
{
    public class ConfirmWindowViewModel : BaseViewModel
    {
        private Window _win {get;set;}

        private string _information;
        public string Information { get => _information;
            set
            {
                if(_information != value)
                {
                    _information = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand AcceptCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ConfirmWindowViewModel(Window win, string information)
        {
            _win = win;
            Information = information;

            AcceptCommand = new RelayCommand(Accept);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Accept()
        {
            _win.DialogResult = true;
            Console.WriteLine("Accepted");
            
        }
        private void Cancel()
        {
            _win.DialogResult = false;
            _win.Close();
        }
    }
}
