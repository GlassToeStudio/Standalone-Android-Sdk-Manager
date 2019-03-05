using System;

namespace SdkManger.Core
{
    public class SdkPlatformItem : SdkItemBase, IComparable<SdkPlatformItem>
    {

        #region Overrides

        public int CompareTo(SdkPlatformItem packageData)
        {
            // A null value means that this object is greater.
            if (packageData == null)
            {
                return 1;
            }
            else
            {
                return packageData.ApiLevel.CompareTo(this.ApiLevel);
            }
        }

        public override string ToString()
        {
            return $"Platform: {Platform}, API Level: {ApiLevel}, Description: {Description}, Version: {Version}, Installed = {IsInstalled}, Status: {Status} Location: {InstallLocation}.";
        }
        #endregion
    }
}