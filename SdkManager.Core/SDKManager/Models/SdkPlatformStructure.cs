using System.Threading.Tasks;
using System.Collections.Generic;

namespace SdkManager.Core
{
    /// <summary>
    /// Data container for all platforns currently available.
    /// </summary>
    public class SdkPlatformStructure
    {
        /// <summary>
        /// List of all high-level Platform items, with their lower-level packages.
        /// </summary>
        public List<SdkItem> PlatformItems { get; set; } = new List<SdkItem>();
        /// <summary>
        /// List of all high-level Tools items, with their lower-level packages.
        /// </summary>
        public List<SdkItem> ToolsItems { get; set; } = new List<SdkItem>();

        /// <summary>
        /// Default constructor:
        /// <para>Will parse all sdkmanager --list --verbose output and create a list of platform items.</para>
        /// </summary>
        public SdkPlatformStructure()
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

            CreatePackageItems();
            CreateToolItems();
        }

        private void CreatePackageItems()
        {
            PlatformItems = SdkManagerBat.GetPlatforms();

            foreach (var p in PlatformItems)
            {
                p.CheckForUpdates();
                p.CreatePackageChildren();

                foreach (SdkItem c in p.Children)
                {
                    c.CheckForUpdates();
                }
            }
        }

        // TODO: Fix This
        private void CreateToolItems()
        {
            ToolsItems = SdkManagerBat.GetTools();

            foreach (var p in ToolsItems)
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
