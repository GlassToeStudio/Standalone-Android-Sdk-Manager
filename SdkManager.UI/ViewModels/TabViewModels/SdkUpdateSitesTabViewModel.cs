using System.Collections.ObjectModel;

namespace SdkManager.UI
{
    public class SdkUpdateSitesTabViewModel : TabBaseViewModel
    {
        public ObservableCollection<SdkPlaformItemViewModel> PackageItems { get; set; }
        public SdkUpdateSitesTabViewModel() : base()
        {

        }
    }
}
