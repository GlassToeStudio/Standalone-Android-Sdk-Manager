﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GTS_SDK_Manager
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Private Properties

        private string _pathName;

        #endregion

        #region Public Properties

        public SdkManagerBatViewModel SdkManager { get; set; }

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
                            SdkManager.Reset();
                            PopulatePlatformsTab();
                        }
                    }
                }
            }
        }

        public bool IsValidPath
        {
            get => ValidatePath();
        }

        public ObservableCollection<TabBaseViewModel> TabViewModels { get; set; }
        
        #endregion


        #region Commands

        public ICommand ChangePathCommand { get; set; }

        public ICommand UpdatePackagesCommand { get; set; }

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            ChangePathCommand = new RelayCommandString(ChangePath);
            UpdatePackagesCommand = new RelayCommand(UpdatePackages);

            SdkManager = new SdkManagerBatViewModel();
            PathName = SdkManager.PathName;
            CreateTabViewModels();
        }

        #endregion

        private void CreateTabViewModels()
        {
            TabViewModels = new ObservableCollection<TabBaseViewModel>
            {
                new SdkPlatformsTabViewModel
                {
                    TxtTabName = "SDK Packages",
                    TxtInformation = "Each Android SDK Platform package includes the Android platform and sources pertaining to an API level by default. Once installed, Android Studio will automatically check for updates. Check \"show package details\" to display individual SDK components.",
                    TxtPackageName = "Package Name",
                    TxtAPILevel = "API Level",
                    TxtRevision = "Revision",
                    TxtStatus = "Status"
                },
                new SdkToolsTabViewModel
                {
                    TxtTabName = "SDK Tools",
                },
                new SdkUpdateSitesTabViewModel { TxtTabName = "Package Updates", },
                new CommandLineTabViewModel { TxtTabName = "Command Line", }
            };

            PopulatePlatformsTab();
        }

        private void PopulatePlatformsTab()
        {
            SdkManager.Reset();
            ((SdkPlatformsTabViewModel)TabViewModels[0])?.Populate();
        }

        private void UpdatePackages()
        {
            StringBuilder sbInstall = new StringBuilder();
            StringBuilder sbUninstall = new StringBuilder();
            StringBuilder descriptions = new StringBuilder();

            var packageTabs = (SdkPlatformsTabViewModel)TabViewModels[0];
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
                    if(sbInstall.Length > 0)
                    {
                        Console.WriteLine(sbInstall.ToString());
                        var t = Task.Run( async () => {
                            await SdkManager.InstallOrUpdatePackages(sbInstall.ToString());
                            PopulatePlatformsTab();
                        });
                    }

                    if(sbUninstall.Length > 0)
                    {
                        Console.WriteLine(sbUninstall.ToString());
                        var t = Task.Run(async () => {
                            await SdkManager.UninstallPackages(sbUninstall.ToString());
                            PopulatePlatformsTab();
                        });
                    }
                                       
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

        private bool ValidatePath()
        {
            if (File.Exists(_pathName + @"\tools\bin\sdkmanager.bat"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
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
