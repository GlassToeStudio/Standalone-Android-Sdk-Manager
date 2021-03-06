﻿using System.Linq;
using SdkManager.Core;
using System.Collections.ObjectModel;
using System;

namespace SdkManager.UI
{
    /// <summary>
    /// Standard data container view model for each high-level sdk platform,
    /// </summary>
    public class SdkItemBaseViewModel : BaseViewModel
    {
        #region Private Backing Fields

        /// <summary>
        /// OtherPackages backing field.
        /// </summary>
        protected ObservableCollection<SdkItemBaseViewModel> _otherPackages;
        /// <summary>
        /// IsChecked backing field.
        /// </summary>
        protected bool _isChecked;
        /// <summary>
        /// IsExpanded backing field.
        /// </summary>
        protected bool _isExpanded;
        /// <summary>
        /// CanExpand backing field.
        /// </summary>
        protected bool _canExpand;
        /// <summary>
        /// Cached reference to this ViewModel's model.
        /// </summary>
        protected SdkItem _package;
        /// <summary>
        /// 
        /// </summary>
        protected bool isEnabled = true;

        private StatusImageType _statusImage;
        #endregion

        #region Public Properties

        /// <summary>
        /// The name of this platform, as read from sdk manager: platforms;android-23
        /// </summary>
        public string Platform { get; protected set; }

        /// <summary>
        /// The current API Level of this package, as read from sdk manager: 23
        /// </summary>
        public long? ApiLevel { get; protected set; }

        /// <summary>
        /// Android SDK Platform 23 => Android 6.0 (Marshmallow)
        /// </summary>
        public string Description { get; protected set; }
        /// <summary>
        /// Android SDK Platform 23 => Android 6.0 (Marshmallow)
        /// </summary>
        public string PlainDescription { get; protected set; }

        /// <summary>
        ///  The current version fo this package, as read from sdk manager: 3
        /// </summary>
        public string Version { get; protected set; }

        /// <summary>
        /// The installed lcoation of this package.
        /// C:\Users\GlassToe\AppData\Local\Android\Sdk\platforms\android-23
        /// </summary>
        public string InstallLocation { get; protected set; }

        /// <summary>
        /// Returns true if Install Location is not null, false otherwise.
        /// </summary>
        public bool IsInstalled { get; protected set; }

        /// <summary>
        /// Status: Installed, Not Installed, or Update Available
        /// </summary>
        public PackageStatus? Status { get; protected set; }

        /// <summary>
        /// True if this is a child of a Package Item, false otherwise.
        /// </summary>
        public bool IsChild { get; protected set; }

        /// <summary>
        /// List of OtherPackages of this package item, items is this list will have null OtherPackages.
        /// </summary>
        public ObservableCollection<SdkItemBaseViewModel> OtherPackages
        {
            get => _otherPackages;
            set
            {
                if (_otherPackages != value)
                {
                    _otherPackages = value;
                    NotifyPropertyChanged();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (value != isEnabled)
                {

                    isEnabled = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region UI Properties


        public bool IsMultiLevel;
        /// <summary>
        /// True if this package was initially installed, false otherwise.
        /// <para>Used to determine how to handle this package if its Checkbox is toggled.</para>
        /// </summary>
        public bool InitialState { get; private set; }

        /// <summary>
        /// True if Checked, false otherwise.
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;

                    UpdateStatus();
                    CheckBoxChanged?.Invoke(Platform, InitialState);
                    NotifyPropertyChanged();
                    return;
                }

            }
        }

        private void UpdateStatus()
        {
            if(InitialState == true)
            {
                if (IsChecked == false)
                {
                    StatusImage = StatusImageType.Uninstall;
                    return;
                }
                StatusImage = StatusImageType.Donothing;
            }
            else
            {
                if(IsChecked == true)
                {
                    StatusImage = StatusImageType.Install;
                    return;
                }
                StatusImage = StatusImageType.Donothing;
            }
        }

        /// <summary>
        /// Indicates if the current item is expanded or not
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// IF true, show foldout arrow to view children, otherwise no arrow.
        /// </summary>
        public bool CanExpand
        {
            get => _canExpand;
            set
            {
                if (_canExpand != value)
                {
                    _canExpand = value;
                    GetOtherPackages();
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Status Image will determine what image is displayed to the left of this item. To 
        /// make it apparenet if this item is being installed, or uninstalled.
        /// </summary>
        public StatusImageType StatusImage
        {
            get => _statusImage;
            set
            {
                if (_statusImage != value)
                {
                    _statusImage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        public event Action<string, bool> CheckBoxChanged;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="package"></param>
        public SdkItemBaseViewModel(SdkItem package, bool canExpand)
        {
            _package = package;
            CanExpand = canExpand;

            Platform = package.Platform;
            ApiLevel = package.ApiLevel;
            Description = package.Description;
            PlainDescription = package.PlainDescription;
            Version = package.Version;
            InstallLocation = package.InstallLocation;
            IsInstalled = package.IsInstalled;
            Status = package.Status;
            IsChild = package.IsChild;
            InitialState = string.IsNullOrEmpty(InstallLocation) ? false : true;
            IsChecked = InitialState;
            IsExpanded = true;

            GetOtherPackages();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get low-level child packages if CanExpand is true. Get empty list otherwise.
        /// </summary>
        protected virtual void GetOtherPackages()
        {
            if (!CanExpand)
            {
                CheckBoxChanged = null;
                IsEnabled = true;
                this.ApiLevel = _package.ApiLevel;
                this.Version = _package.Version;
                this.Status = _package.Status;
                Description = _package.Description;
                _otherPackages = new ObservableCollection<SdkItemBaseViewModel>();
            }
            else
            {
                var children = _package.Children;
                CheckBoxChanged = null;
                if (children?.Count <= 0 || children == null)
                {
                    return;
                }

                IsEnabled = false; // TODO: Alter look of checkbox when disabled.
                this.ApiLevel = null;
                this.Version = null;
                this.Status = null;

                _otherPackages = new ObservableCollection<SdkItemBaseViewModel>(
                    children.Select(p => new SdkItemBaseViewModel(p, false))
                    );

                var package = new SdkItemBaseViewModel(_package, false)
                {
                    Description = _package.PlainDescription
                };
                _otherPackages.Insert(0, package);
            }
        }

        #endregion
    }

    // TODO: Move this
    public enum StatusImageType
    {
        Donothing,
        Install,
        Uninstall
    }
}
