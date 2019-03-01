using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GTS_SDK_Manager
{
    public class SDK_PlaformItemViewModel : BaseViewModel
    {
        public SDK_PlaformItemViewModel(SDK_PlatformItem package)
        {
            Platform = package.Platform;
            ApiLevel = package.ApiLevel;
            Description = package.Description;
            Version = package.Version;
            InstallLocation = package.InstallLocation;
            IsInstalled = package.IsInstalled;
            Status = package.Status;
            IsChild = package.IsChild;
            InitialState = string.IsNullOrEmpty(InstallLocation) ? false : true;
            IsChecked = InitialState;

            var children = package.Children;
            if (package.IsChild == false)
            {
                _otherPackages = new ObservableCollection<SDK_PlaformItemViewModel>(
                    children.Select(p => new SDK_PlaformItemViewModel(p))
                    );
            }
 
        }
        /// <summary>
        /// The name of this platform, as read from sdk manager: platforms;android-23
        /// </summary>
        public string Platform { get; set; }
        /// <summary>
        /// The current API Level of this package, as read from sdk manager: 23
        /// </summary>
        public int ApiLevel { get; set; }
        /// <summary>
        /// Android SDK Platform 23 => Android 6.0 (Marshmallow)
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///  The current version fo this package, as read from sdk manager: 3
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// The installed lcoation of this package.
        /// C:\Users\GlassToe\AppData\Local\Android\Sdk\platforms\android-23
        /// </summary>
        public string InstallLocation { get; set; }
        /// <summary>
        /// Returns true if Install Location is not null, false otherwise.
        /// </summary>
        public bool IsInstalled { get; set; }
        /// <summary>
        /// Status: Installed, Not Installed, or Update Available
        /// </summary>
        public PackageStatus Status { get; set; }
        /// <summary>
        /// True if this is a child of a Package Item, false otherwise.
        /// </summary>
        public bool IsChild { get; set; }
        /// <summary>
        /// OtherPackages backing field.
        /// </summary>
        private ObservableCollection<SDK_PlaformItemViewModel> _otherPackages;
        /// <summary>
        /// List of OtherPackages of this package item, items is this list will have null OtherPackages.
        /// </summary>
        public ObservableCollection<SDK_PlaformItemViewModel> OtherPackages
        {
            get { return IsChild ? null : _otherPackages; }
            set { _otherPackages = value; }
        }

        public bool InitialState { get; set; }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value)
                {
                    return;
                }

                _isChecked = value;
                NotifyPropertyChanged();
            }
        }
    }
}
