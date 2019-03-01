using System.Collections.ObjectModel;
using System.Linq;

namespace GTS_SDK_Manager
{
    public class SDK_PlatformsTabViewModel : TabBaseViewModel
    {
        private SDK_PlatformStructure _packageItemStructure;
        public ObservableCollection<SDK_PlaformItemViewModel> PackageItems { get; set; }
        public SDK_PlatformsTabViewModel() : base()
        {
            _packageItemStructure = new SDK_PlatformStructure();

            var children = _packageItemStructure.PackageItems;
            this.PackageItems = new ObservableCollection<SDK_PlaformItemViewModel>(
                children.Select(package => new SDK_PlaformItemViewModel(package))
                );
        }
    }
}
