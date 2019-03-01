using System.Collections.ObjectModel;

namespace GTS_SDK_Manager
{
    public class SDK_UpdateSitesTabViewModel : TabBaseViewModel
    {
        public ObservableCollection<SDK_PlaformItemViewModel> PackageItems { get; set; }
        public SDK_UpdateSitesTabViewModel() : base()
        {

        }
    }
}
