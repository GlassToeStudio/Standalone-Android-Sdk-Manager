using System.Linq;
using SdkManager.Core;
using System.Collections.ObjectModel;

namespace SdkManager.UI
{
    /// <summary>
    /// Standard data container view model for each high-level sdk platform,
    /// </summary>
    public class SdkToolItemViewModel : SdkItemBaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Standard data container for a tool tiem.
        /// </summary>
        /// <param name="package"></param>
        public SdkToolItemViewModel(SdkItem package, bool canExpand) : base(package, canExpand)
        {

        }

        #endregion
    }
}
