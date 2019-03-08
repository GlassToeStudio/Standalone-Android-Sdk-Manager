using System.Linq;
using SdkManager.Core;
using System.Collections.ObjectModel;
using System;

namespace SdkManager.UI
{
    public class SdkToolsTabViewModel : TabBaseViewModel
    {
        #region Private Backing Feilds

        private bool _showItems;

        #endregion

        #region Public Properties

        /// <summary>
        /// Data container for all currently available platform items
        /// </summary>
        public SdkToolsStructure ItemStructure { get; set; }

        /// <summary>
        ///CheckBox, if true, show foldout for low-level tools items, only high-level otherwise.
        /// </summary>
        public bool ShowItems
        {
            get => _showItems;
            set
            {
                if (_showItems != value)
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
        /// View model to hold a list of tools Items.
        /// </summary>
        public SdkToolsTabViewModel(MainWindowViewModel main) : base(main)
        {
            PopulateItems(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get all high-level tool items. If showpackageItems == true, get lower-level tools items also.
        /// </summary>
        /// <param name="showItems"></param>
        public void PopulateItems(bool showItems)
        {
            ShowItems = showItems;
            CheckBoxChanged = null;
            ItemStructure = new SdkToolsStructure();

            var topLevelItems = ItemStructure.Items;
            if (topLevelItems == null)
            {
                return;
            }

            this.PackageItems = new ObservableCollection<SdkItemBaseViewModel>(
                topLevelItems.Select(package => new SdkToolItemViewModel(package, showItems))
                );

            foreach (var item in PackageItems)
            {
                item.CheckBoxChanged += OnCheckBoxChanged;
                if (item.OtherPackages != null)
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
