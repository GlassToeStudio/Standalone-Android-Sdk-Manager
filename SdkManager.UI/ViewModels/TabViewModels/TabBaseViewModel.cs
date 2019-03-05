namespace SdkManger.UI
{
    /// <summary>
    /// Base class from which all Tab ViewModels inherit.
    /// </summary>
    public class TabBaseViewModel : BaseViewModel
    {
        public string Description1 { get; set; }
        /// <summary>
        /// The Header Name of this tab.
        /// </summary>
        public string TxtTabName { get; set; }

        // These do not belong in the base class
        // TODO: Move to apprpriate class.
        public string TxtInformation { get; set; }

        public string TxtPackageName { get; set; }
        public string TxtAPILevel { get; set; }
        public string TxtRevision { get; set; }
        public string TxtStatus { get; set; }
    }
}
