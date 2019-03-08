using System.Collections.ObjectModel;

namespace SdkManager.UI
{
    /// <summary>
    /// Base class from which all Tab ViewModels inherit.
    /// </summary>
    public class TabBaseViewModel : BaseViewModel
    {

        /// <summary>
        /// PackageItems backing field.
        /// </summary>
        protected ObservableCollection<SdkItemBaseViewModel> _packageItems;

        /// <summary>
        /// List of all high-level  items and their lower level children.
        /// </summary>
        public ObservableCollection<SdkItemBaseViewModel> PackageItems
        {
            get => _packageItems;
            set
            {
                if (_packageItems != value)
                {
                    _packageItems = value;
                    NotifyPropertyChanged();
                }
            }
        }
        /// <summary>
        /// The Header Name of this tab.
        /// </summary>
        public string TxtTabName { get; set; }

        /// <summary>
        /// Will display text in teh tab view main upper text area.
        /// </summary>
        public string TxtInformation { get; set; }
    }
}
