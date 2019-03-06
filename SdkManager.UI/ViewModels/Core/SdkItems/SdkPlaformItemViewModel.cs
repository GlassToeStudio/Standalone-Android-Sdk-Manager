using System.Linq;
using SdkManager.Core;
using System.Collections.ObjectModel;

namespace SdkManager.UI
{
    /// <summary>
    /// Standard data container view model for each high-level sdk platform,
    /// </summary>
    public class SdkPlaformItemViewModel : SdkItemBaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="package"></param>
        public SdkPlaformItemViewModel(SdkItemBase package, bool canExpand) :base(package, canExpand)
        {
        }

        #endregion
    }
}
