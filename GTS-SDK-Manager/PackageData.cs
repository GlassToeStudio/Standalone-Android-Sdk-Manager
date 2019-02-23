using System;

namespace GTS_SDK_Manager
{
    public class PackageData : IComparable<PackageData>
    {
        public string PlatformName { get; set; }
        public int APILevel { get; set; }
        public string Revision { get; set; }
        public string Status { get; set; }

        public PackageData(string platformName, int apilevel, string revision, string status)
        {
            PlatformName = platformName;
            APILevel = apilevel;
            Revision = revision;
            Status = status;
        }

        public int CompareTo(PackageData packageData)
        {
            // A null value means that this object is greater.
            if (packageData == null)
            {
                return 1;
            }
            else
            {
                return packageData.APILevel.CompareTo(this.APILevel);
            }
        }
    }
}
