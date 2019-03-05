using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SdkManger.Core
{
    /// <summary>
    /// Simulate the built-in sdkmanger.bat
    /// </summary>
    public static class SdkManagerBat
    {
        #region Private fields

        private static readonly RegexOptions _options = RegexOptions.Multiline;

        private static MatchCollection _body;
        private static MatchCollection _updates;
        private static MatchCollection _googleApis;
        private static MatchCollection _systemImages;
        private static MatchCollection _sources;
        private static MatchCollection _googleGlass;

        #endregion

        #region Public Properties
     
        /// <summary>
        /// A string representation of the output from sdkmanager.bat --list --verbose
        /// </summary>
        public static string VerboseOutput { get; private set; }

        /// <summary>
        /// The path to sdkmanager.bat
        /// </summary> TODO: Intentional Typeo!!!!!!!!!!!!!!!!!!!!!!
        public static string PathName { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Andyroid\Sdk";

        #endregion

        #region Events/Actions

        /// <summary>
        /// Listen for this event to get the output from the hidden console window.
        /// </summary>
        public static event Action<string> CommandLineOutputReceived;

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the output VerboseOutput from running sdkmanager.bat --list --verbose
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async static Task<string> FetchVerboseOutputAsync()
        {
            if (string.IsNullOrEmpty(PathName))
            {
                PathName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Android\Sdk";
            }

            if (File.Exists(PathName + @"\tools\bin\sdkmanager.bat") == false)
            {
                return null;
            }

            if (VerboseOutput == null)
            {
                ProcessStartInfo processInfo = NoShellProcessInfo(PathName);
                processInfo.Arguments = "--list --verbose";

                Process cmdProcess = new Process { StartInfo = processInfo };
                cmdProcess.Start();

                VerboseOutput = await Task.Run(async () =>
                {
                    using (StreamReader stdOut = cmdProcess.StandardOutput)
                    {
                        var result = await stdOut.ReadToEndAsync();
                        stdOut.Close();
                        cmdProcess.Close();
                        return result;
                    }
                });
            }
            return VerboseOutput;
        }

        /// <summary>
        /// Will create a list of package items based on VerboseOutput.
        /// </summary>
        /// <returns></returns>
        public static List<SdkPlatformItem> CreatePackageItems()
        {
            if (_body == null)
            {
                _body = Regex.Matches(VerboseOutput, Patterns.TestString, _options);
            }

            List<SdkPlatformItem> packageItems = new List<SdkPlatformItem>();

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
                        new SdkPlatformItem
                        {
                            Platform = platform,
                            ApiLevel = ConvertPlatformAPILevelToInt(apilevel),
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
      
        /// <summary>
        /// Will create a list of package items based on VerboseOutput.
        /// </summary>
        /// <returns></returns>
        public static List<SdkToolsItem> CreateToolsItems()
        {
            if (_body == null)
            {
                _body = Regex.Matches(VerboseOutput, Patterns.BUILD_TOOLS_PATTERN, _options);
            }

            List<SdkToolsItem> toolsItems = new List<SdkToolsItem>();

            for (int i = 0; i < _body.Count; i++)
            {
                var platform = _body[i].Groups["Platform"].ToString().Trim();
                var apilevel = _body[i].Groups["APILevel"].ToString().Trim();
                var description = LookUpTable.CoolNames[_body[i].Groups["Description"].ToString().Trim()];
                var version = _body[i].Groups["Version"].ToString().Trim();
                var installLocation = _body[i].Groups["Installed_Location"].ToString().Trim();

                if (toolsItems.Any(x => x.Platform == platform) == false)
                {
                    toolsItems.Add(
                        new SdkToolsItem
                        {
                            Platform = platform,
                            // Need to accept 28.0.1 and convert
                            ApiLevel = ConvertToolsAPILevelToInt(apilevel),
                            Description = description,
                            // Need to accept 28.0.1 or 28.0.0-rc1
                            Version = version,
                            InstallLocation = installLocation,
                            IsInstalled = string.IsNullOrEmpty(installLocation) == false,
                            Status = string.IsNullOrEmpty(installLocation) ? PackageStatus.NOT_INSTALLED : PackageStatus.INSTALLED,
                            IsChild = false
                        });
                }

            }
            toolsItems.Sort();
            return toolsItems;
        }

        private static int ConvertPlatformAPILevelToInt(string apilevel)
        {
            return int.Parse(apilevel = apilevel.Substring(0, apilevel.IndexOf('.') > -1 ? apilevel.IndexOf('.') : apilevel.Length));
        }

        private static int ConvertToolsAPILevelToInt(string apilevel)
        {
            return int.Parse(apilevel.Remove('.').Trim());
        }

        /// <summary>
        /// Runs sdkmanager.bat --install [package;name]
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async static Task<string> InstallPackagesAsync(params string[] args)
        {
            ProcessStartInfo processInfo = NoShellProcessInfo(PathName);

            processInfo.Arguments = $"{"--install "}{String.Join(" ", args)}";

            Process pro = new Process
            {
                StartInfo = processInfo
            };

            var t = await Task.Run(() =>
            {
                pro.OutputDataReceived += (sender, arg) =>
                {
                    CommandLineOutputReceived(arg.Data);
                }; 

                string stdError = null;
                try
                {
                    pro.Start();
                    pro.BeginOutputReadLine();
                    pro.StandardInput.WriteLine("y");
                    stdError = pro.StandardError.ReadToEnd();
                    pro.WaitForExit();
                }
                catch (Exception e)
                {
                    throw new Exception("OS error while executing " + Format("sdkmanager", String.Join(" ", args)) + ": " + e.Message, e);
                }

                if (pro.ExitCode == 0)
                {
                    return string.Empty;
                }
                else
                {
                    var message = new StringBuilder();

                    if (!string.IsNullOrEmpty(stdError))
                    {
                        message.AppendLine(stdError);
                    }

                    throw new Exception(Format("sdkmanager", String.Join(" ", args)) + " finished with exit code = " + pro.ExitCode + ": " + message);
                }
            });

            return t;
        }
        
        /// <summary>
        /// Runs sdkmanager.bat --uninstall [package;name]
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async static Task<string> UninstallPackagesAsync(params string[] args)
        {
            ProcessStartInfo processInfo = NoShellProcessInfo(PathName);

            processInfo.Arguments = $"{"--uninstall "}{String.Join(" ", args)}";

            Process pro = new Process
            {
                StartInfo = processInfo
            };

            var t = await Task.Run(() =>
            {
                pro.OutputDataReceived += (sender, arg) =>
                {
                    CommandLineOutputReceived(arg.Data);
                }; 

                string stdError = null;
                try
                {
                    pro.Start();
                    pro.BeginOutputReadLine();
                    //pro.StandardInput.WriteLine("y");
                    stdError = pro.StandardError.ReadToEnd();
                    pro.WaitForExit();
                }
                catch (Exception e)
                {
                    throw new Exception("OS error while executing " + Format("sdkmanager", String.Join(" ", args)) + ": " + e.Message, e);
                }

                if (pro.ExitCode == 0)
                {
                    return string.Empty;
                }
                else
                {
                    var message = new StringBuilder();

                    if (!string.IsNullOrEmpty(stdError))
                    {
                        message.AppendLine(stdError);
                    }

                    throw new Exception(Format("sdkmanager", String.Join(" ", args)) + " finished with exit code = " + pro.ExitCode + ": " + message);
                }
            });

            return t;
        }

        /// <summary>
        /// Reset the static members to prepare to gather new data. Called when changes to installed sdks are made.
        /// </summary>
        public static void ClearCache()
        {
            _body = null;
            _updates = null;
            _googleApis = null;
            _systemImages = null;
            _sources = null;
            _googleGlass = null;

            VerboseOutput = null;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper method to get setup a new process.
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        private static ProcessStartInfo NoShellProcessInfo(string pathName)
        {
            return new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                FileName = $"{pathName}{@"\tools\bin\sdkmanager.bat"}"
            };
        }

        /// <summary>
        /// Used for error handling of console output.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private static string Format(string filename, string arguments)
        {
            return "'" + filename +
                ((string.IsNullOrEmpty(arguments)) ? string.Empty : " " + arguments) +
                "'";
        }

        #endregion

        #region SDK_PlatformItem Extension Methods (only here for use of the MatchCollection)

        /// <summary>
        /// Check this SDK_PlatformItem for updates.
        /// </summary>
        /// <param name="packageItem"></param>
        /// <returns></returns>
        public static SdkPlatformItem CheckForUpdates(this SdkPlatformItem packageItem)
        {
            if (_updates == null)
            {
                _updates = Regex.Matches(VerboseOutput, Patterns.UPDATEABLE_PACKAGES_PATTERN, _options);
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
        
        /// <summary>
        /// Get all low-level packages for this SDK_PlatformItem.
        /// </summary>
        /// <param name="packageItem"></param>
        /// <returns></returns>
        public static SdkPlatformItem CreatePackageChildren(this SdkPlatformItem packageItem)
        {

            if (_googleApis == null)
            {
                _googleApis = Regex.Matches(VerboseOutput, Patterns.GOOGLE_APIS, _options);
            }
            packageItem.GetChild(_googleApis);

            if (_sources == null)
            {
                _sources = Regex.Matches(VerboseOutput, Patterns.SOURCES_PATTERN, _options);
            }
            packageItem.GetChild(_sources);

            if (_googleGlass == null)
            {
                _googleGlass = Regex.Matches(VerboseOutput, Patterns.GOOGLE_GLASS_PATTERN, _options);
            }
            packageItem.GetChild(_googleGlass);

            if (_systemImages == null)
            {
                _systemImages = Regex.Matches(VerboseOutput, Patterns.SYSTEM_IMAGES_PATTERN, _options);
            }
            packageItem.GetChild(_systemImages);

            return packageItem;
        }
       
        /// <summary>
        /// Helper method to get specific children of this SDK_PlatformItem. Use by CreatePackageChildren().
        /// </summary>
        /// <param name="packageItem"></param>
        /// <returns></returns>
        private static SdkPlatformItem GetChild(this SdkPlatformItem packageItem, MatchCollection collection)
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
                        packageItem.Children.Add(new SdkPlatformItem
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
