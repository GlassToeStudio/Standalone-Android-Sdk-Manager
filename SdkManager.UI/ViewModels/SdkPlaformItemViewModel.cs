using System.Linq;
using SdkManager.Core;
using System.Collections.ObjectModel;

namespace SdkManager.UI
{
    /// <summary>
    /// Standard data container view model for each high-level sdk platform,
    /// </summary>
    public class SdkPlaformItemViewModel : BaseViewModel
    {
        #region Private Backing Fields

        /// <summary>
        /// OtherPackages backing field.
        /// </summary>
        private ObservableCollection<SdkPlaformItemViewModel> _otherPackages;
        /// <summary>
        /// IsChecked backing field.
        /// </summary>
        private bool _isChecked;
        /// <summary>
        /// IsExpanded backing field.
        /// </summary>
        private bool _isExpanded;
        /// <summary>
        /// CanExpand backing field.
        /// </summary>
        private bool _canExpand;
        /// <summary>
        /// Cached reference to this ViewModel's model.
        /// </summary>
        private SdkItemBase _package;

        #endregion

        #region Public Properties

        /// <summary>
        /// The name of this platform, as read from sdk manager: platforms;android-23
        /// </summary>
        public string Platform { get; private set; }
     
        /// <summary>
        /// The current API Level of this package, as read from sdk manager: 23
        /// </summary>
        public int ApiLevel { get; private set; }
     
        /// <summary>
        /// Android SDK Platform 23 => Android 6.0 (Marshmallow)
        /// </summary>
        public string Description { get; private set; }
     
        /// <summary>
        ///  The current version fo this package, as read from sdk manager: 3
        /// </summary>
        public string Version { get; private set; }
     
        /// <summary>
        /// The installed lcoation of this package.
        /// C:\Users\GlassToe\AppData\Local\Android\Sdk\platforms\android-23
        /// </summary>
        public string InstallLocation { get; private set; }
      
        /// <summary>
        /// Returns true if Install Location is not null, false otherwise.
        /// </summary>
        public bool IsInstalled { get; private set; }
       
        /// <summary>
        /// Status: Installed, Not Installed, or Update Available
        /// </summary>
        public PackageStatus Status { get; private set; }
      
        /// <summary>
        /// True if this is a child of a Package Item, false otherwise.
        /// </summary>
        public bool IsChild { get; private set; }
      
        /// <summary>
        /// List of OtherPackages of this package item, items is this list will have null OtherPackages.
        /// </summary>
        public ObservableCollection<SdkPlaformItemViewModel> OtherPackages
        {
            get => _otherPackages;
            set {
                if(_otherPackages != value)
                {
                    _otherPackages = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region UI Properties

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
                if (_isChecked == value)
                {
                    return;
                }

                _isChecked = value;
                NotifyPropertyChanged();
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

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="package"></param>
        public SdkPlaformItemViewModel(SdkItemBase package, bool canExpand)
        {
            _package = package;
            CanExpand = canExpand;

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
            IsExpanded = true;

            GetOtherPackages();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get low-level child packages if CanExpand is true. Get empty list otherwise.
        /// </summary>
        private void GetOtherPackages()
        {
            if(!CanExpand)
            {
                _otherPackages = new ObservableCollection<SdkPlaformItemViewModel>();
            }
            else
            {
                var children = _package.Children;
                if (_package.IsChild == false)
                {
                    _otherPackages = new ObservableCollection<SdkPlaformItemViewModel>(
                        children.Select(p => new SdkPlaformItemViewModel(p, false))
                        );
                }
            }


        }

        #endregion
    }
}
