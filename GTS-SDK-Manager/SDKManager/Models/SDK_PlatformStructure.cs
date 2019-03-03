using System.Threading.Tasks;
using System.Collections.Generic;

namespace GTS_SDK_Manager
{
    /// <summary>
    /// Data container for all platforns currently available.
    /// </summary>
    public class SDK_PlatformStructure
    {
        /// <summary>
        /// List of all high-level Platform items, with their lower-level packages.
        /// </summary>
        public List<SDK_PlatformItem> PackageItems { get; set; } = new List<SDK_PlatformItem>();

        /// <summary>
        /// Default constructor:
        /// <para>Will parse all sdkmanager --list --verbose output and create a list of platform items.</para>
        /// </summary>
        public SDK_PlatformStructure()
        {
            if (SdkManagerBat.VerboseOutput == null)
            {
                var t = Task.Run(() => SdkManagerBat.FetchVerboseOutputAsync());
                t.Wait();
            }

            if (SdkManagerBat.VerboseOutput == null)
            {
                return;
            }

            PackageItems = SdkManagerBat.CreatePackageItems();

            foreach (var p in PackageItems)
            {
                p.CheckForUpdates();
                p.CreatePackageChildren();

                foreach (var c in p.Children)
                {
                    c.CheckForUpdates();
                }
            } 
        }
    }
}
