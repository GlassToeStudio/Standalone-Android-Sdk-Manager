﻿using System.Collections.ObjectModel;

namespace SdkManager.UI
{
    class SdkToolsTabViewModel : TabBaseViewModel
    {
        public ObservableCollection<SdkPlaformItemViewModel> Tools { get; set; }
        public SdkToolsTabViewModel() : base()
        {

        }
    }
}
