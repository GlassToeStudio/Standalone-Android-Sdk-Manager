using System.Collections.ObjectModel;
using System.Linq;

namespace GTS_SDK_Manager
{
    public class SdkPlatformsTabViewModel : TabBaseViewModel
    {
        /// <summary>
        /// Data container for all currently available platform items
        /// </summary>
        private SDK_PlatformStructure _packageItemStructure;
        private ObservableCollection<SdkPlaformItemViewModel> _packageItems;
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

        public SdkPlatformsTabViewModel() : base()
        {

        }

        public void Populate()
        {
            _packageItemStructure = new SDK_PlatformStructure();

            var children = _packageItemStructure.PackageItems;
            this.PackageItems = new ObservableCollection<SdkPlaformItemViewModel>(
                children.Select(package => new SdkPlaformItemViewModel(package))
                );
        }
    }
}
