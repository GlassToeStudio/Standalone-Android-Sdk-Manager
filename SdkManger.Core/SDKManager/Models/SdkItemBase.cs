using System.Collections.Generic;

namespace SdkManager.Core
{
    /// <summary>
    /// Standard data container for each high-level sdk platform,
    /// </summary>
    public class SdkItemBase
    {
        /// <summary>
        /// The name of this platform, as read from sdk manager: platforms;android-23
        /// </summary>
        public string Platform { get; set; }
        /// <summary>
        /// The current API Level of this package, as read from sdk manager: 23
        /// </summary>
        public int ApiLevel { get; set; }
        /// <summary>
        /// The descrion of the plafrom as read from sdk manager: Android SDK Platform 23 => Android 6.0 (Marshmallow)
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///  The current version fo this package, as read from sdk manager: 3
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// The installed lcoation of this package.
        /// C:\Users\USER\AppData\Local\Android\Sdk\platforms\android-23 ( for example)
        /// </summary>
        public string InstallLocation { get; set; }
        /// <summary>
        /// Returns true if Install Location is not null, false otherwise.
        /// </summary>
        public bool IsInstalled { get; set; }
        /// <summary>
        /// Status: Installed, Not Installed, or Update Available
        /// </summary>
        public PackageStatus Status { get; set; }
        /// <summary>
        /// True if this is a child of a Package Item, false otherwise.
        /// </summary>
        public bool IsChild { get; set; }
        /// <summary>
        /// Children backing field.
        /// </summary>
        private List<SdkPlatformItem> _children = new List<SdkPlatformItem>();
        /// <summary>
        /// List of Children of this package item, items is this list will have null children.
        /// </summary>
        public List<SdkPlatformItem> Children
        {
            get { return IsChild ? null : _children; }
            set { _children = value; }
        }
    }
}
