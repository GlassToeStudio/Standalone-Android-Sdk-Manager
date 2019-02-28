using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS_SDK_Manager.SDKTools
{
    public class PakcageItemStructure
    {
        public async Task<string[]> GetAllData()
        {
            return await Task<string[]>.Run(async () => {
                return await PackageStructure.RunSDKManagerListVerbose(null);
            });

        } // Get AllData from another class.

        public List<PackageItem> PackageItems { get; set; } = new List<PackageItem>();
    }
}
