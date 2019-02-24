using System;

namespace GTS_SDK_Manager
{
    public class PackageData : IComparable<PackageData>
    {
        public string DisplayName { get; set; }
        public int APILevel { get; set; }
        public string Revision { get; set; }
        public string Status { get; set; }
        public string PackageName { get; set;
        }
        public PackageData(string packageName, string displayName, int apilevel, string revision, string status)
        {
            PackageName = packageName;
            DisplayName = displayName;
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
