using System.Collections.ObjectModel;
using System.Linq;

namespace GTS_SDK_Manager
{
    public class SDK_PlatformsTabViewModel : TabBaseViewModel
    {
        /// <summary>
        /// Data container for all currently available platform items
        /// </summary>
        private SDK_PlatformStructure _packageItemStructure;
        private ObservableCollection<SDK_PlaformItemViewModel> _packageItems;
        /// <summary>
        /// List of all high-level platform items and their lower level packages.
        /// </summary>
        public ObservableCollection<SDK_PlaformItemViewModel> PackageItems { get => _packageItems;
            set
            {
                if(_packageItems != value)
                {
                    _packageItems = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public SDK_PlatformsTabViewModel() : base()
        {

        }

        public void Populate()
        {
            _packageItemStructure = new SDK_PlatformStructure();

            var children = _packageItemStructure.PackageItems;
            this.PackageItems = new ObservableCollection<SDK_PlaformItemViewModel>(
                children.Select(package => new SDK_PlaformItemViewModel(package))
                );
        }
    }
}
