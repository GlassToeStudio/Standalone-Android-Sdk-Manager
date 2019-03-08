using System;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace SdkManager.UI
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Private Properties

        private string _pathName;

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

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            UpdatePackagesCommand = new RelayCommand(UpdatePackages);
            CancelCommand = new RelayCommand(ResetAll);

            SdkManager = new SdkManagerBatViewModel();
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
                //new CommandLineTabViewModel { TxtTabName = "Command Line", }
            };
        }

        private void PopulatePlatformsTab()
        {
            SdkManager.ClearCache();

            ((SdkPlatformsTabViewModel)TabViewModels[0])?.PopulateItems(false);
            ((SdkToolsTabViewModel)TabViewModels[1])?.PopulateItems(false);
        }

        private void UpdatePackages()
        {
            StringBuilder sbInstall = new StringBuilder();
            StringBuilder sbUninstall = new StringBuilder();
            StringBuilder descriptions = new StringBuilder();

            foreach (var item in ((SdkPlatformsTabViewModel)TabViewModels[0]).PackageItems)
            {
                CheckStatus(sbInstall, sbUninstall, item);

                foreach (var child in item.OtherPackages)
                {
                    CheckStatus(sbInstall, sbUninstall, child);
                }
            }

            foreach (var item in ((SdkToolsTabViewModel)TabViewModels[1]).PackageItems)
            {
                CheckStatus(sbInstall, sbUninstall, item);

                foreach (var child in item.OtherPackages)
                {
                    CheckStatus(sbInstall, sbUninstall, child);
                }
            }

            if (sbInstall.Length == 0 && sbUninstall.Length == 0)
            {
                return;
            }

            descriptions.Append("Install: \n");
            for (int i = 0; i < sbInstall.Length; i++)
            {
                if(char.IsWhiteSpace(sbInstall[i]))
                {
                    descriptions.Append("\n");
                }
                descriptions.Append(sbInstall[i]);
            }
            descriptions.Append("\nUninstall: \n");
            for (int i = 0; i < sbUninstall.Length; i++)
            {
                if (char.IsWhiteSpace(sbUninstall[i]))
                {
                    descriptions.Append("\n");
                }
                descriptions.Append(sbUninstall[i]);
            }

            Console.WriteLine(sbInstall.ToString());

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

                foreach (var child in item.OtherPackages)
                {
                    ResetItem(item);
                }
            }

            foreach (var item in ((SdkToolsTabViewModel)TabViewModels[1]).PackageItems)
            {
                ResetItem(item);

                foreach (var child in item.OtherPackages)
                {
                    ResetItem(item);
                }
            }
        }

        private void CheckStatus(StringBuilder sbInstall, StringBuilder sbUninstall, SdkItemBaseViewModel item)
        {
            if (item.InitialState != item.IsChecked)
            {
                if (item.InitialState == false)
                {
                    sbInstall.Append($"{item.Platform} ");
                }
                else
                {
                    sbUninstall.Append($"{item.Platform} ");
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
                Console.WriteLine(sbInstall.ToString());
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
                Console.WriteLine(sbUninstall.ToString());
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

    #region Validate Class

    public class FileExistsValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (File.Exists((string)value + @"\tools\bin\sdkmanager.bat"))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Path is not a valid path to the android sdk.");
            }
        }
    }
    
    #endregion
}
