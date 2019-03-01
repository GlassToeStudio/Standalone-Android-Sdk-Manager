
using System.Collections.ObjectModel;


namespace GTS_SDK_Manager
{
    class SDK_ToolsTabViewModel : TabBaseViewModel
    {
        public ObservableCollection<SDK_PlaformItemViewModel> Tools { get; set; }
        public SDK_ToolsTabViewModel() : base()
        {

        }
    }
}
