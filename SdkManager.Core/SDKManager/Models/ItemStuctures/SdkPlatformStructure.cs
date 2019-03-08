namespace SdkManager.Core
{
    /// <summary>
    /// Data container for all platforns currently available.
    /// </summary>
    public class SdkPlatformStructure : SdkItemStructureBase
    {
        /// <summary>
        /// Data container for all Sdk Platform Items
        /// </summary>
        public SdkPlatformStructure() :base()
        {
            CreateItems();
        }

        protected override void CreateItems()
        {
            Items = SdkManagerBat.GetPlatforms();

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
