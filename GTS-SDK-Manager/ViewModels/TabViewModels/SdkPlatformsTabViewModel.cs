using System.Linq;
using System.Collections.ObjectModel;
using SdkManger.Core;

namespace SdkManger.UI
{
    public class SdkPlatformsTabViewModel : TabBaseViewModel
    {
        #region Private Backing Feilds

        private ObservableCollection<SdkPlaformItemViewModel> _packageItems;

        private bool showPackageItems;

        #endregion

        #region Public Properties

        /// <summary>
        /// Data container for all currently available platform items
        /// </summary>
        public SdkPlatformStructure PackageItemStructure { get; set; }

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

        /// <summary>
        ///CheckBox, if true, show foldout for low-level package items, onlu high-level otherwise.
        /// </summary>
        public bool ShowPackageItems { get => showPackageItems;
            set
            {
                if(showPackageItems != value)
                {
                    showPackageItems = value;
                    PopulatePackageItemStructure(showPackageItems);
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// View model to hold a list of Platform Items.
        /// </summary>
        public SdkPlatformsTabViewModel() : base()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get all high-level packages. If showpackageItems == true, get lower-level packages items also.
        /// </summary>
        /// <param name="showPackageItems"></param>
        public void PopulatePackageItemStructure(bool showPackageItems)
        {
            PackageItemStructure = new SdkPlatformStructure();

            var topLevelItems = PackageItemStructure.PlatformItems;
            this.PackageItems = new ObservableCollection<SdkPlaformItemViewModel>(
                topLevelItems.Select(package => new SdkPlaformItemViewModel(package, showPackageItems))
                );
        }
        
        #endregion
    }
}
