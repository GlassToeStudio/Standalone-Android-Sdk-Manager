using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GTS_SDK_Manager
{
    /// <summary>
    /// A base ViewModel from which all ViewModels inherit.
    /// <para>Implements INotifyPropertyChanged interface.</para>
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
