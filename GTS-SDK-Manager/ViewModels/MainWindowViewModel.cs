using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace GTS_SDK_Manager
{
    public class MainWindowViewModel : BaseViewModel
    {
        private SDKManagerViewModel sdkManager;

        public string LabelLocation = "Android SDK Location";

        public string TxtSDKPath { get; set; }

        public string TxtBroweFolder { get; set; }

        private string _pathName;

        public string PathName
        {
            get => _pathName;
            set
            {
                if (_pathName != value)
                {
                    _pathName = value;
                    sdkManager.PathName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand ChangePathCommand { get; set; }

        public ICommand UpdatePackagesCommand { get; set; }

        public ObservableCollection<TabBaseViewModel> TabViewModels { get; set; }

        public MainWindowViewModel()
        {
            ChangePathCommand = new RelayCommandString(ChangePath);
            UpdatePackagesCommand = new RelayCommand(UpdatePackages);

            sdkManager = new SDKManagerViewModel();
            PathName = sdkManager.PathName;
            TabViewModels = new ObservableCollection<TabBaseViewModel>
            {
                new SDK_PlatformsTabViewModel
                {
                    TxtTabName = "SDK Packages",
                    TxtInformation = "Each Android SDK Platform package includes the Android platform and sources pertaining to an API level by default. Once installed, Android Studio will automatically check for updates. Check \"show package details\" to display individual SDK components.",
                    TxtPackageName = "Package Name",
                    TxtAPILevel = "API Level",
                    TxtRevision = "Revision",
                    TxtStatus = "Status"
                },
                new SDK_ToolsTabViewModel
                {
                    TxtTabName = "SDK Tools",
                    TxtInformation = "Each Android SDK Platform package includes the Android platform and sources pertaining to an API level by default. Once installed, Android Studio will automatically check for updates. Check \"show package details\" to display individual SDK components.",
                    TxtPackageName = "Package Name",
                    TxtAPILevel = "API Level",
                    TxtRevision = "Revision",
                    TxtStatus = "Status"
                },
                new SDK_UpdateSitesTabViewModel { TxtTabName = "Package Updates", },
                new CommandLineTabViewModel { TxtTabName = "Command Line", }
            };
        }

        private void UpdatePackages()
        {
            StringBuilder sbInstall = new StringBuilder("--install ");
            StringBuilder sbUninstall = new StringBuilder("--uninstall ");
            StringBuilder descriptions = new StringBuilder();
            var packageTabs = (SDK_PlatformsTabViewModel)TabViewModels[0];
            foreach (var item in packageTabs.PackageItems)
            {
                if(item.InitialState != item.IsChecked)
                {
                    if(item.InitialState == false)
                    {
                        descriptions.Append($"{item.Description}\n");
                        sbInstall.Append($"{item.Platform} ");
                    }
                    else
                    {
                        sbUninstall.Append($"{item.Platform} ");
                    }
                }
            }

            ConfirmChangeWindow win = new ConfirmChangeWindow(descriptions.ToString());
            bool? result = win.ShowDialog();
            switch (result)
            {
                case true:
                    Console.WriteLine(sbInstall.ToString());
                    Console.WriteLine(sbUninstall.ToString());
                    break;
                case false:
                    Console.WriteLine("User Canceled");
                    break;
                default:
                    break;
            }
        }

        private void ChangePath(string obj)
        {
            Console.WriteLine("This is obj: " + obj);
        }
    }
}
