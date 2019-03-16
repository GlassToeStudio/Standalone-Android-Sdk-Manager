using System;
using System.Windows;
using System.Windows.Input;

namespace SdkManager.UI
{
    /// <summary>
    /// </summary>
    public class ConsoleOutputWindowViewModel : BaseViewModel
    {

        private Window _win {get;set;}
        private SdkManagerBatViewModel sdkManagerbatVM;

        public SdkManagerBatViewModel SdkManagerBatVM
        {
            get => sdkManagerbatVM;
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
