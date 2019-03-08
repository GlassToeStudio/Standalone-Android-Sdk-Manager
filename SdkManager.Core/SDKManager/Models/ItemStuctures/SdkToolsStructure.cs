using System.Threading.Tasks;
using System.Collections.Generic;

namespace SdkManager.Core
{
    /// <summary>
    /// Data container for all sdk tools currently available.
    /// </summary>
    public class SdkToolsStructure : SdkItemStructureBase
    {

        /// <summary>
        /// Data container for all Sdk Tools Items
        /// </summary>
        public SdkToolsStructure() : base()
        {
            CreateItems();
        }


        // TODO: Fix This
        protected override void CreateItems()
        {
            Items = SdkManagerBat.GetTools();

            if (Items == null)
            {
                return;
            }
            foreach (var p in Items)
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
