using System.Linq;
using SdkManager.Core;
using System.Collections.ObjectModel;

namespace SdkManager.UI
{
    /// <summary>
    /// Standard data container view model for each high-level sdk platform,
    /// </summary>
    public class SdkPlaformItemViewModels : SdkItemBaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="package"></param>
        public SdkPlaformItemViewModels(SdkItem package, bool canExpand) :base(package, canExpand)
        {
        }

        #endregion
    }
}
