using System.Linq;
using SdkManager.Core;
using System.Collections.ObjectModel;

namespace SdkManager.UI
{
    public class SdkToolsTabViewModel : TabBaseViewModel
    {
        #region Private Backing Feilds

        private ObservableCollection<SdkToolItemViewModel> _toolsItems;

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
        public ObservableCollection<SdkToolItemViewModel> ToolsItems
        {
            get => _toolsItems;
            set
            {
                if (_toolsItems != value)
                {
                    _toolsItems = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        ///CheckBox, if true, show foldout for low-level package items, onlu high-level otherwise.
        /// </summary>
        public bool ShowPackageItems
        {
            get => showPackageItems;
            set
            {
                if (showPackageItems != value)
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
        public SdkToolsTabViewModel() : base()
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

            var topLevelItems = PackageItemStructure.ToolsItems;
            this.ToolsItems = new ObservableCollection<SdkToolItemViewModel>(
                topLevelItems.Select(package => new SdkToolItemViewModel(package, showPackageItems))
                );
        }

        #endregion
    }
}
