
using System.Collections.ObjectModel;


namespace GTS_SDK_Manager
{
    class SdkToolsTabViewModel : TabBaseViewModel
    {
        public ObservableCollection<SdkPlaformItemViewModel> Tools { get; set; }
        public SdkToolsTabViewModel() : base()
        {

        }
    }
}
