using System.Collections.ObjectModel;

namespace GTS_SDK_Manager
{
    public class SdkUpdateSitesTabViewModel : TabBaseViewModel
    {
        public ObservableCollection<SdkPlaformItemViewModel> PackageItems { get; set; }
        public SdkUpdateSitesTabViewModel() : base()
        {

        }
    }
}
