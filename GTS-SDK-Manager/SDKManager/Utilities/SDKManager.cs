using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GTS_SDK_Manager
{
    public static class SDKManagerBat
    {
        private static readonly RegexOptions _options = RegexOptions.Multiline;
        public static string PathName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Andyroid\Sdk";
 
        private static MatchCollection _body;
        private static MatchCollection _updates;
        private static MatchCollection _googleApis;
        private static MatchCollection _systemImages;
        private static MatchCollection _sources;
        private static MatchCollection _googleGlass;

        public static string AllData { get; private set; }

        public async static Task<string> GetListVerboseOutputAsync(params string[] args)
        {
            Console.WriteLine("SDK Manager Path Name: " + PathName);
            if(string.IsNullOrEmpty(PathName))
            {
                PathName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Android\Sdk";
            }

            if (File.Exists(PathName + @"\tools\bin\sdkmanager.bat") == false)
            {
                return null;
            }

            if (AllData == null)
            {
                string YourApplicationPath = PathName + @"\tools\bin\sdkmanager";
                string processOutput;

                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = YourApplicationPath + ".bat"
                };
                processInfo.Arguments = String.Join(" ", args); //" --list --verbose";

                Process pro = new Process
                {
                    StartInfo = processInfo
                };
                pro.Start();

                var r = await Task.Run(async () =>
                {
                    using (StreamReader std_out = pro.StandardOutput)
                    {
                        var outstuff = await std_out.ReadToEndAsync();
                        processOutput = outstuff;
                        std_out.Close();
                        pro.Close();
                        return processOutput;
                    }
                });

                AllData = r;
            }
            
            return AllData;
        }

        public static void Reset()
        {
             _body = null;
             _updates = null;
             _googleApis = null;
             _systemImages = null;
             _sources = null;
             _googleGlass = null;
             AllData = null;
        }

        public static List<SDK_PlatformItem> CreatePackageItems()
        {
            if(_body == null)
            {
                _body = Regex.Matches(AllData, Patterns.test_string, _options);
            }

            List<SDK_PlatformItem> packageItems = new List<SDK_PlatformItem>();

            for (int i = 0; i < _body.Count; i++)
            {
                var platform = _body[i].Groups["Platform"].ToString().Trim();
                var apilevel = _body[i].Groups["APILevel"].ToString().Trim();
                var description = LookUpTable.CoolNames[_body[i].Groups["Description"].ToString().Trim()];
                var version = _body[i].Groups["Version"].ToString().Trim();
                var installLocation = _body[i].Groups["Installed_Location"].ToString().Trim();

                if (packageItems.Any(x => x.Platform == platform) == false)
                {
                    packageItems.Add(
                        new SDK_PlatformItem
                        {
                            Platform = platform,
                            ApiLevel = int.Parse(apilevel = apilevel.Substring(0, apilevel.IndexOf('.') > -1 ? apilevel.IndexOf('.') : apilevel.Length)),
                            Description = description,
                            Version = version,
                            InstallLocation = installLocation,
                            IsInstalled = string.IsNullOrEmpty(installLocation) == false,
                            Status = string.IsNullOrEmpty(installLocation) ? PackageStatus.NOT_INSTALLED : PackageStatus.INSTALLED,
                            IsChild = false
                        });
                }
            }
            packageItems.Sort();
            return packageItems;
        }

        #region SDK_PlatformItem Extension Methods

        public static SDK_PlatformItem CheckForUpdates(this SDK_PlatformItem packageItem)
        {
            if(_updates == null)
            {
                _updates = Regex.Matches(AllData, Patterns.UPDATEABLE_PACKAGES_PATTERN, _options);
            }
       
            for (int i = 0; i < _updates.Count; i++)
            {
                var platform = _updates[i].Groups["Platform"].ToString().Trim();
                var installedVersion = _updates[i].Groups["Installed_Version"].ToString().Trim();
                var availableVersion = _updates[i].Groups["Available_Version"].ToString().Trim();

                if (packageItem.Platform == platform)
                {
                    packageItem.Status = PackageStatus.UPDATE_AVAILABLE;
                    return packageItem;
                }
            }
            return packageItem;
        }

        public static SDK_PlatformItem CreatePackageChildren(this SDK_PlatformItem packageItem)
        {
            if(_systemImages == null)
            {
                _systemImages = Regex.Matches(AllData, Patterns.SYSTEM_IMAGES_PATTERN, _options);
            }
            packageItem.GetChild(_systemImages);

            if(_googleApis == null)
            {
                _googleApis = Regex.Matches(AllData, Patterns.GOOGLE_APIS, _options);
            }
            if(_sources == null)
            {
                _sources = Regex.Matches(AllData, Patterns.SOURCES_PATTERN, _options);
            }
            packageItem.GetChild(_sources);

            if(_googleGlass == null)
            {
                _googleGlass = Regex.Matches(AllData, Patterns.GOOGLE_GLASS_PATTERN, _options);
            }

            packageItem.GetChild(_googleGlass);

            return packageItem;
        }

        private static SDK_PlatformItem GetChild(this SDK_PlatformItem packageItem, MatchCollection collection)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                var platform = collection[i].Groups["Platform"].ToString().Trim();
                var apilevel = collection[i].Groups["APILevel"].ToString().Trim();
                var description = collection[i].Groups["Description"].ToString().Trim();
                var version = collection[i].Groups["Version"].ToString().Trim();
                var installLocation = collection[i].Groups["Installed_Location"].ToString().Trim();
                int intapilevel = int.Parse(apilevel = apilevel.Substring(0, apilevel.IndexOf('.') > -1 ? apilevel.IndexOf('.') : apilevel.Length));

                if (packageItem.Children.Any(x => x.Platform == platform) == false)
                {
                    if (packageItem.ApiLevel == int.Parse(apilevel))
                    {
                        packageItem.Children.Add(new SDK_PlatformItem
                        {
                            Platform = platform,
                            ApiLevel = intapilevel,
                            Description = description,
                            Version = version,
                            InstallLocation = installLocation,
                            IsInstalled = string.IsNullOrEmpty(installLocation) == false,
                            Status = string.IsNullOrEmpty(installLocation) ? PackageStatus.NOT_INSTALLED : PackageStatus.INSTALLED,
                            IsChild = true
                        });
                    }
                }
            }
            return packageItem;
        }
        
        #endregion
    }
}
