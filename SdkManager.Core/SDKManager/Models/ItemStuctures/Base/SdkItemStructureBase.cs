using System.Threading.Tasks;
using System.Collections.Generic;

namespace SdkManager.Core
{
    /// <summary>
    /// Base data container for all skd items currently available.
    /// </summary>
    public class SdkItemStructureBase
    {
        /// <summary>
        /// List of all sdk items, with their lower-level packages.
        /// </summary>
        public List<SdkItem> Items { get; set; } = new List<SdkItem>();

        /// <summary>
        /// Default constructor:
        /// <para>Will parse all sdkmanager --list --verbose output and create a list of platform items.</para>
        /// </summary>
        public SdkItemStructureBase()
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
        }

        protected virtual void CreateItems()
        {
            throw new System.Exception("This method is meant to be overriden by the derived class.");
        }
    }
}
