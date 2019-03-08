using System.Linq;
using SdkManager.Core;
using System.Collections.ObjectModel;
using System;

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

        public event Action<string, bool> CheckBoxChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// View model to hold a list of Platform Items.
        /// </summary>
        public SdkPlatformsTabViewModel(MainWindowViewModel main) : base(main)
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
            CheckBoxChanged = null;
            ItemStructure = new SdkPlatformStructure();

            var topLevelItems = ItemStructure.Items;

            if (topLevelItems == null)
            {
                return;
            }

            this.PackageItems = new ObservableCollection<SdkItemBaseViewModel>(
                topLevelItems.Select(package => new SdkPlaformItemViewModels(package, showItems))
                );

            foreach (var item in PackageItems)
            {
                item.CheckBoxChanged += OnCheckBoxChanged;
                if(item.OtherPackages != null)
                {
                    foreach (var child in item.OtherPackages)
                    {
                        child.CheckBoxChanged += OnCheckBoxChanged;
                    }
                }
            }
            _main.Subscribe(this);
        }

        public void OnCheckBoxChanged(string platform, bool isInstalled)
        {
            CheckBoxChanged?.Invoke(platform, isInstalled);
        }
        
        #endregion
    }
}
