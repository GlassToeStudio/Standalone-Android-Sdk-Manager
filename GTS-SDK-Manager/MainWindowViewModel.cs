using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS_SDK_Manager
{
    public class MainWindowViewModel : BaseViewModel
    {
        public string LabelLocation { get { return "Android SDK Location"; } }

        public string TxtSDKPath { get; set; }

        public string TxtBroweFolder { get; set; }

        public ObservableCollection<TabViewModel> TabViewModels { get; set; }
    }

    public class TabViewModel : BaseViewModel
    {
        private string m_description1;
        public string Description1 { get { return m_description1; } set { m_description1 = value; } }

        public string TxtInformation { get { return "Each Android SDK Platform package includes the Android platform and sources pertaining to an API level by default. Once installed, Android Studio will automatically check for updates. Check \"show package details\" to display individual SDK components."; } }

        public string TxtPackageName { get { return "Package Name"; } }
        public string TxtAPILevel { get { return "API Level"; } }
        public string TxtRevision { get { return "Revision"; } }
        public string TxtStatus { get { return "Status"; } }
        public ObservableCollection<TabViewModel> PackageItems { get; set; }
    }

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
