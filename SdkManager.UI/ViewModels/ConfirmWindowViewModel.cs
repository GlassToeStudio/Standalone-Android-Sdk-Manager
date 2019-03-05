using System;
using System.Windows;
using System.Windows.Input;

namespace SdkManager.UI
{
    /// <summary>
    /// A window to diplay when updating, intalling, uninstalling packages.
    /// <para>Will return true if user accpets, or false if user cancles.</para>
    /// <para>Use Window.ShowDialog()</para>
    /// </summary>
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
