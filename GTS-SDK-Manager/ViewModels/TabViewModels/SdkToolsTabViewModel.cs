
using System.Collections.ObjectModel;


namespace SdkManger.UI
{
    class SdkToolsTabViewModel : TabBaseViewModel
    {
        public ObservableCollection<SdkPlaformItemViewModel> Tools { get; set; }
        public SdkToolsTabViewModel() : base()
        {

        }
    }
}
