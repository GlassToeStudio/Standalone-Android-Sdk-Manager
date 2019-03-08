using System.Linq;
using SdkManager.Core;
using System.Collections.ObjectModel;

namespace SdkManager.UI
{
    public class SdkPlatformsTabViewModel : TabBaseViewModel
    {
        #region Private Backing Feilds

        private bool _showItems;

        #endregion

        #region Public Properties

        /// <summary>
        /// Data container for all currently available platform items
        /// </summary>
        public SdkPlatformStructure ItemStructure { get; set; }

        /// <summary>
        /// List of all high-level platform items and their lower level packages.
        /// </summary>

        /// <summary>
        ///CheckBox, if true, show foldout for low-level package items, onlu high-level otherwise.
        /// </summary>
        public bool ShowItems { get => _showItems;
            set
            {
                if(_showItems != value)
                {
                    _showItems = value;
                    PopulateItems(_showItems);
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
            PopulateItems(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get all high-level packages. If showpackageItems == true, get lower-level packages items also.
        /// </summary>
        /// <param name="showItems"></param>
        public void PopulateItems(bool showItems)
        {
            ItemStructure = new SdkPlatformStructure();

            var topLevelItems = ItemStructure.Items;

            if (topLevelItems == null)
            {
                return;
            }

            this.PackageItems = new ObservableCollection<SdkItemBaseViewModel>(
                topLevelItems.Select(package => new SdkItemBaseViewModel(package, showItems))
                );
        }
        
        #endregion
    }
}
