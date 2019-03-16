using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SdkManager.Core
{
    /// <summary>
    /// Simulate the built-in sdkmanger.bat
    /// </summary>
    public static class SdkManagerBat
    {
        #region Private fields

        private static readonly RegexOptions _options = RegexOptions.Multiline;

        private static MatchCollection _platformBody;
        private static MatchCollection _updates;
        private static MatchCollection _googleApis;
        private static MatchCollection _systemImages;
        private static MatchCollection _sources;
        private static MatchCollection _googleGlass;
        private static string _pathName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Android\Sdk";

        #endregion

        #region Public Properties

        /// <summary>
        /// A string representation of the output from sdkmanager.bat --list --verbose
        /// </summary>
        public static string VerboseOutput { get; private set; }

        /// <summary>
        /// The path to sdkmanager.bat
        /// </summary>
        public static string PathName { get => _pathName;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _pathName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Android\Sdk";
                }
                else
                {
                    _pathName = value;
                }
            }
        }
        #endregion

        #region Events/Actions

        /// <summary>
        /// Listen for this event to get the output from the hidden console window as it it received.
        /// </summary>
        public static event Action<string> CommandLineOutputReceived;
        
        /// <summary>
        /// Listen for this event to get the full output from the hidden console window.
        /// </summary>
        public static event Action<string> CommandLineOutputComplete;

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the output VerboseOutput from running sdkmanager.bat --list --verbose
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async static Task<string> FetchVerboseOutputAsync()
        {
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
        public static List<SdkItem> GetPlatforms()
        {
            if (VerboseOutput == null)
            {
                return null;
            }

            if (_platformBody == null)
            {
                _platformBody = Regex.Matches(VerboseOutput, Patterns.TestString, _options);
            }

            List<SdkItem> packageItems = new List<SdkItem>();

            for (int i = 0; i < _platformBody.Count; i++)
            {
                var platform = _platformBody[i].Groups["Platform"].ToString().Trim();
                var apilevel = _platformBody[i].Groups["APILevel"].ToString().Trim();
                var description = LookUpTable.GetDescription(_platformBody[i].Groups["Description"].ToString().Trim());
                var plaindescription = _platformBody[i].Groups["Description"].ToString().Trim();
                var version = _platformBody[i].Groups["Version"].ToString().Trim();
                var installLocation = _platformBody[i].Groups["Installed_Location"].ToString().Trim();

                if (packageItems.Any(x => x.Platform == platform) == false)
                {
                    packageItems.Add(
                        new SdkPlatformItems
                        {
                            Platform = platform,
                            ApiLevel = ConvertPlatformAPILevelToInt(apilevel),
                            Description = description,
                            PlainDescription = plaindescription,
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
        /// Will create a list of tools items based on VerboseOutput.
        /// </summary>
        /// <returns></returns>
        public static List<SdkItem> GetTools()
        {
            if (VerboseOutput == null)
            {
                return null;
            }

            List<SdkItem> toolsItems = new List<SdkItem>
            {
                CreateGenericSDKTools(Patterns.BUILD_TOOLS_PATTERN),
                CreateGenericSDKTools(Patterns.GPU_DEBUGGING_TOOLS_PATTERN),
                CreateGenericSDKTools(Patterns.CMAKE_PATTERN),
                CreateGenericSDKTools(Patterns.LLDB_PATTERN),
                CreateGenericSDKTools(Patterns.ANDROID_AUTO_API_SIM_PATTERN),
                CreateGenericSDKTools(Patterns.ANDROID_AUTO_EMULATOR_PATTERN),
                CreateGenericSDKTools(Patterns.EMULATOR_PATTERN),
                CreateGenericSDKTools(Patterns.PLATFORM_TOOLS_PATTERN),
                CreateGenericSDKTools(Patterns.SDK_TOOLS_PATTERN),
                CreateGenericSDKTools(Patterns.DOCS_PATTERN),
                CreateGenericSDKTools(Patterns.GOOGLE_APK_EXT_LIBRARY_PATTERN),
                CreateGenericSDKTools(Patterns.GOOLGE_PLAY_INSTANT_SDK_PATTERN),
                CreateGenericSDKTools(Patterns.GOOGLE_PLAY_LICENSE_LIBRARY_PATTERN),
                CreateGenericSDKTools(Patterns.GOOGLE_PLAY_SERVICES_PATTERN),
                CreateGenericSDKTools(Patterns.GOOGLE_USB_DRIVER_PATTERN),
                CreateGenericSDKTools(Patterns.GOOGLE_WEB_DRIVER_PATTERN),
                CreateGenericSDKTools(Patterns.INTEL_86_EMULATOR_PATTERN),
                CreateGenericSDKTools(Patterns.NDK_PATTERN),

                //TODO: First, these really belong to their own subgroup.
                //      Second, we need to get a find a better way to display
                //      the information on screen when expaned, using version,
                //      like in the other packages does not works, since
                //      the version of all of these are '1'. 
                //      Additionally, we are stripping the -betaX from the
                //      API level, we don't want this.
                //
                // CreateGenericSDKTools(Patterns.CONSTRAINT_LAYOUT_PATTERN),
                // CreateGenericSDKTools(Patterns.SOLVER_CONSTRAINT_LAYOUT_PATTERN),
                CreateGenericSDKTools(Patterns.ANDROID_SUPPORT_PATTERN),
                CreateGenericSDKTools(Patterns.GOOGLE_REPOSITORY_PATTERN)
            };

            for (int i = 0; i < toolsItems.Count; i++)
            {
                if (toolsItems[i] == null)
                {
                    toolsItems.RemoveAt(i);
                    i--;
                }
            }

            return toolsItems;
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
        /// Runs sdkmanager.bat args
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async static Task<string> RunCommandAsync(params string[] args)
        {
            ProcessStartInfo processInfo = NoShellProcessInfo(PathName);

            processInfo.Arguments = $"{String.Join(" ", args)}";

            var output = new StringBuilder();
            Process pro = new Process
            {
                StartInfo = processInfo
            };

            var t = await Task.Run(() =>
            {
                pro.OutputDataReceived += (sender, arg) =>
                {
                    output.Append(arg.Data + "\n");
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
                    CommandLineOutputComplete(output.ToString());
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
            CommandLineOutputReceived(output.ToString());
            return t;
        }

        /// <summary>
        /// Reset the static members to prepare to gather new data. Called when changes to installed sdks are made.
        /// </summary>
        public static void ClearCache()
        {
            VerboseOutput = null;

            // SDK Platforms
            _platformBody = null;
            _updates = null;
            _googleApis = null;
            _systemImages = null;
            _sources = null;
            _googleGlass = null;
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
        /// Will create a list of any platform items matching the passed in pattern, based on VerboseOutput.
        /// </summary>
        /// <returns></returns>
        private static SdkToolsItems CreateGenericSDKTools(string pattern)
        {
            MatchCollection collection = Regex.Matches(VerboseOutput, pattern, _options);

            List<SdkToolsItems> items = new List<SdkToolsItems>();

            for (int i = 0; i < collection.Count; i++)
            {
                var platform = collection[i].Groups["Platform"].ToString().Trim();
                var apilevel = collection[i].Groups["APILevel"].ToString().Trim();
                var description = collection[i].Groups["Description"].ToString().Trim();
                var plaindescription = collection[i].Groups["Description"].ToString().Trim();
                var version = collection[i].Groups["Version"].ToString().Trim();
                var installLocation = collection[i].Groups["Installed_Location"].ToString().Trim();

                if (items.Any(x => x.Platform == platform) == false)
                {
                    items.Add(
                        new SdkToolsItems
                        {
                            Platform = platform,
                            ApiLevel = ConvertToolsAPILevelToInt(apilevel),
                            Description = description,
                            PlainDescription = plaindescription,
                            Version = version,
                            InstallLocation = installLocation,
                            IsInstalled = string.IsNullOrEmpty(installLocation) == false,
                            Status = string.IsNullOrEmpty(installLocation) ? PackageStatus.NOT_INSTALLED : PackageStatus.INSTALLED,
                            IsChild = true
                        });
                }

            }
            if (items.Count > 0)
            {
                items.Sort();
                var mainItem = items[0];
                mainItem.IsChild = false;

                items.Remove(mainItem);
                foreach (var c in items)
                {
                    mainItem.Children.Add(c);
                }
                return mainItem;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// TODO: Needs work
        /// </summary>
        /// <param name="apilevel"></param>
        /// <returns></returns>
        private static long ConvertPlatformAPILevelToInt(string apilevel)
        {
            if (string.IsNullOrEmpty(apilevel))
                return 0;

            return long.Parse(apilevel = apilevel.Substring(0, apilevel.IndexOf('.') > -1 ? apilevel.IndexOf('.') : apilevel.Length));
        }
        /// <summary>
        /// TODO:  needs work
        /// </summary>
        /// <param name="apilevel"></param>
        /// <returns></returns>
        private static long ConvertToolsAPILevelToInt(string apilevel)
        {
            if (string.IsNullOrEmpty(apilevel))
                return 0;

            return long.Parse(apilevel.Split('-')[0].Replace(".", "").Trim());
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
        public static SdkItem CheckForUpdates(this SdkItem packageItem)
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
        public static SdkItem CreatePackageChildren(this SdkItem packageItem)
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
        private static SdkItem GetChild(this SdkItem packageItem, MatchCollection collection)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                var platform = collection[i].Groups["Platform"].ToString().Trim();
                var apilevel = collection[i].Groups["APILevel"].ToString().Trim();
                var description = collection[i].Groups["Description"].ToString().Trim();
                var plaindescription = collection[i].Groups["Description"].ToString().Trim();
                var version = collection[i].Groups["Version"].ToString().Trim();
                var installLocation = collection[i].Groups["Installed_Location"].ToString().Trim();
                long intapilevel = ConvertToolsAPILevelToInt(apilevel);

                if (packageItem.Children.Any(x => x.Platform == platform) == false)
                {
                    if (packageItem.ApiLevel == intapilevel)
                    {
                        packageItem.Children.Add(
                            new SdkItem
                            {
                                Platform = platform,
                                ApiLevel = intapilevel,
                                Description = description,
                                PlainDescription = plaindescription,
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
