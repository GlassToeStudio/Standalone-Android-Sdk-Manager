using System.Collections.ObjectModel;

namespace SdkManger.UI
{
    public class SdkUpdateSitesTabViewModel : TabBaseViewModel
    {
        public ObservableCollection<SdkPlaformItemViewModel> PackageItems { get; set; }
        public SdkUpdateSitesTabViewModel() : base()
        {

        }
    }
}
