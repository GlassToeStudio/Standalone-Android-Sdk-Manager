using System.Collections.Generic;

using System.Threading.Tasks;

namespace GTS_SDK_Manager
{
    public class SDK_PlatformStructure
    {
        public List<SDK_PlatformItem> PackageItems { get; set; } = new List<SDK_PlatformItem>();

        public SDK_PlatformStructure()
        {
            if (SDKManagerBat.AllData == null)
            {
                var t = Task.Run(() => SDKManagerBat.GetListVerboseOutputAsync("--list --verbose"));
                t.Wait();
            }

            if (SDKManagerBat.AllData == null)
            {
                return;
            }

            PackageItems = SDKManagerBat.CreatePackageItems();

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
