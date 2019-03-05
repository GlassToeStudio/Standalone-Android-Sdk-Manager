using System;

namespace GTS_SDK_Manager
{
    public class SdkToolsItem : SdkItemBase, IComparable<SdkToolsItem>
    {
        #region Overrides

        public int CompareTo(SdkToolsItem packageData)
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
