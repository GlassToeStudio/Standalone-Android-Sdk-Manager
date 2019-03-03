using System.Linq;
using System.Collections.ObjectModel;

namespace GTS_SDK_Manager
{
    public class SdkPlatformsTabViewModel : TabBaseViewModel
    {
        #region Private Backing Feilds

        private ObservableCollection<SdkPlaformItemViewModel> _packageItems;
       
        #endregion

        #region Public Properties

        /// <summary>
        /// Data container for all currently available platform items
        /// </summary>
        public SDK_PlatformStructure PackageItemStructure { get; set; }

        /// <summary>
        /// List of all high-level platform items and their lower level packages.
        /// </summary>
        public ObservableCollection<SdkPlaformItemViewModel> PackageItems { get => _packageItems;
            set
            {
                if(_packageItems != value)
                {
                    _packageItems = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// View model to hold a list of Platform Items.
        /// </summary>
        public SdkPlatformsTabViewModel() : base() { }

        #endregion

        #region Public Methods

        public void PopulatePackageItemStructure()
        {
            PackageItemStructure = new SDK_PlatformStructure();

            var children = PackageItemStructure.PackageItems;
            this.PackageItems = new ObservableCollection<SdkPlaformItemViewModel>(
                children.Select(package => new SdkPlaformItemViewModel(package))
                );
        }
        
        #endregion
    }
}
