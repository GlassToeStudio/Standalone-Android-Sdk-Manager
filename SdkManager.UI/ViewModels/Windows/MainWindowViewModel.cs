using System;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace SdkManager.UI
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Private Properties

        private string _pathName;
        private bool _enableApplyButton;

        private List<string> installList = new List<string>();
        private List<string> uninstallList = new List<string>();
        private bool enableApplyButton;
        #endregion

        #region Public Properties

        public SdkManagerBatViewModel SdkManager { get; set; }

        /// <summary>
        /// The path to sdkmanager.bat
        /// </summary>
        public string PathName
        {
            get => _pathName;
            set
            {
                if (_pathName != value)
                {
                    _pathName = value;
                    SdkManager.PathName = value;

                    if (IsValidPath)
                    {
                        NotifyPropertyChanged();
                        Properties.Settings.Default.sdkpath = value;
                        Properties.Settings.Default.Save();

                        if (TabViewModels != null)
                        {
                            SdkManager.ClearCache();
                            PopulatePlatformsTab();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if sdkmanager.bat is found, false otherwise.
        /// </summary>
        public bool IsValidPath
        {
            get => ValidatePath();
        }


        /// <summary>
        /// Container for all Tab View Modles, a view created for each..
        /// </summary>
        public ObservableCollection<TabBaseViewModel> TabViewModels { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Called when user clicks button to update packages.
        /// </summary>
        public ICommand UpdatePackagesCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public bool EnableApplyButton
        {
            get => (installList.Count != 0 || uninstallList.Count != 0);
            set
            {
                if(_enableApplyButton != value)
                {
                    _enableApplyButton = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            UpdatePackagesCommand = new RelayCommand(UpdatePackages);
            CancelCommand = new RelayCommand(ResetAll);

            SdkManager = new SdkManagerBatViewModel("");
            PathName = SdkManager.PathName;

            CreateTabViewModels();
        }

        #endregion

        #region Private Methods

        private void CreateTabViewModels()
        {
            TabViewModels = new ObservableCollection<TabBaseViewModel>
            {
                new SdkPlatformsTabViewModel
                {
                    TxtTabName = "SDK Packages",
                    TxtInformation = "Each Android SDK Platform package includes the Android platform and sources pertaining to an API level by default. Once installed, Android Studio will automatically check for updates. Check \"show package details\" to display individual SDK components.",

                },
                new SdkToolsTabViewModel
                {
                    TxtTabName = "SDK Tools",
                    TxtInformation = "Below are the available SDK developer tools. Once installed, Standalone SDK Manager will automatically check for updates. Check \"show package details\" to display available versions of an SDK Tool.",

                },
                //new SdkUpdateSitesTabViewModel { TxtTabName = "Package Updates", },
                new CommandLineTabViewModel { TxtTabName = "Command Line", }
            };
        }

        private void PopulatePlatformsTab()
        {
            SdkManager.ClearCache();

            ((SdkPlatformsTabViewModel)TabViewModels[0])?.PopulateItems(false);
            ((SdkToolsTabViewModel)TabViewModels[1])?.PopulateItems(false);

            foreach (var item in TabViewModels)
            {
                Subscribe(item);
            }
        }

        private void Subscribe(TabBaseViewModel tabViewModel)
        {
            if(tabViewModel.PackageItems == null)
            {
                return;
            }

            foreach (var item in tabViewModel.PackageItems)
            {
                item.CheckBoxChanged += OnCheckBoxChanged;
                if (item.OtherPackages != null)
                {
                    foreach (var child in item.OtherPackages)
                    {
                        child.CheckBoxChanged += OnCheckBoxChanged;
                    }
                }
            }
        }

        private void OnCheckBoxChanged(string platform, bool isInstalled)
        {
            Console.WriteLine(platform + " Changed, and it was installed = " + isInstalled);
            if(isInstalled)
            {
                if(uninstallList.Contains(platform))
                {
                    uninstallList.Remove(platform);
                }
                else
                {
                    uninstallList.Add(platform);
                }
            }
            else
            {
                if (installList.Contains(platform))
                {
                    installList.Remove(platform);
                }
                else
                {
                    installList.Add(platform);
                }
            }

            EnableApplyButton = (installList.Count != 0 || uninstallList.Count != 0);

            foreach (var s in installList)
            {
                Console.WriteLine("Install: " + s);
            }
            foreach (var s in uninstallList)
            {
                Console.WriteLine("Uninstall: " + s);
            }
        }

        private void UpdatePackages()
        {
            if(installList.Count == 0 && uninstallList.Count == 0)
            {
                return;
            }

            StringBuilder sbInstall = new StringBuilder();
            StringBuilder sbUninstall = new StringBuilder();
            StringBuilder descriptions = new StringBuilder();

            sbInstall.Append(string.Join(" ", installList));
            sbUninstall.Append(string.Join(" ", uninstallList));

            ConfirmChangeWindow win = new ConfirmChangeWindow(descriptions.ToString());
            bool? result = win.ShowDialog();
            switch (result)
            {
                case true:
                    Install(sbInstall);
                    Uninstall(sbUninstall);
                    break;
                case false:
                    Console.WriteLine("User Canceled");
                    break;
                default:
                    break;
            }
        }

        private void ResetAll()
        {
            foreach (var item in ((SdkPlatformsTabViewModel)TabViewModels[0]).PackageItems)
            {
                ResetItem(item);
                if (item.OtherPackages != null)
                {
                    foreach (var child in item.OtherPackages)
                    {
                        ResetItem(item);
                    }
                }
            }

            foreach (var item in ((SdkToolsTabViewModel)TabViewModels[1]).PackageItems)
            {
                ResetItem(item);
                if (item.OtherPackages != null)
                {
                    foreach (var child in item.OtherPackages)
                    {
                        ResetItem(item);
                    }
                }
            }
        }

        private void ResetItem(SdkItemBaseViewModel item)
        {
            item.IsChecked = item.InitialState;
        }

        private void Install(StringBuilder sbInstall)
        {
            if (sbInstall.Length > 0)
            {
                Console.WriteLine("Installing: " + sbInstall.ToString());
                var t = Task.Run(async () =>
                {
                    await SdkManager.InstallOrUpdatePackages(sbInstall.ToString());
                    PopulatePlatformsTab();
                });
            }
        }

        private void Uninstall(StringBuilder sbUninstall)
        {
            if (sbUninstall.Length > 0)
            {
                Console.WriteLine("Uninstalling: " + sbUninstall.ToString());
                var t = Task.Run(async () =>
                {
                    await SdkManager.UninstallPackages(sbUninstall.ToString());
                    PopulatePlatformsTab();
                });
            }
        }

        private bool ValidatePath()
        {
            return File.Exists(_pathName + @"\tools\bin\sdkmanager.bat");
        }

        #endregion
    }
}
