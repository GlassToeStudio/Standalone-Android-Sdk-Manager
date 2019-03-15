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
    public class ConsoleOutputWindowViewModel : BaseViewModel
    {
        private Window _win {get;set;}

        private SdkManagerBatViewModel sdkManagerbatVM;
        public SdkManagerBatViewModel SdkManagerBatVM { get => sdkManagerbatVM;
            set
            {
                if(sdkManagerbatVM != value)
                {
                    sdkManagerbatVM = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand CancelCommand { get; set; }

        public ConsoleOutputWindowViewModel(Window win, SdkManagerBatViewModel information)
        {
            _win = win;
            SdkManagerBatVM = information;

            CancelCommand = new RelayCommand(Cancel);
        }

        private void Cancel()
        {
            _win.DialogResult = false;
            _win.Close();
        }
    }
}
