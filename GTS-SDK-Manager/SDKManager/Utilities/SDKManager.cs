using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GTS_SDK_Manager
{
    /// <summary>
    /// Simulate the built-in sdkmanger.bat
    /// </summary>
    public static class SDKManagerBat
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

        /// <summary>
        /// The path to sdkmanager.bat
        /// </summary> TODO: Intentional Typeo!!!!!!!!!!!!!!!!!!!!!!
        public static string PathName { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Andyroid\Sdk";
        /// <summary>
        /// A string representation of the output from sdkmanager.bat --list --verbose
        /// </summary>
        public static string VerboseOutput { get; private set; }

        /// <summary>
        /// Listen for this event to get the output from the hidden console window.
        /// </summary>
        public static Action<string> SendOutput { get; set; }

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
                    SendOutput(arg.Data);
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
                    SendOutput(arg.Data);
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
        public static void Reset()
        {
            _body = null;
            _updates = null;
            _googleApis = null;
            _systemImages = null;
            _sources = null;
            _googleGlass = null;
            VerboseOutput = null;
        }

        /// <summary>
        /// Helper method to get setup a new process.
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        private static ProcessStartInfo NoShellProcessInfo(string pathName)
        {
            string sdkManagerPath = $"{pathName}{@"\tools\bin\sdkmanager.bat"}";
            return new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                FileName = sdkManagerPath
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

        /// <summary>
        /// Will create a list of package items based on VerboseOutput.
        /// </summary>
        /// <returns></returns>
        public static List<SDK_PlatformItem> CreatePackageItems()
        {
            if (_body == null)
            {
                _body = Regex.Matches(VerboseOutput, Patterns.test_string, _options);
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

        /// <summary>
        /// Check this SDK_PlatformItem for updates.
        /// </summary>
        /// <param name="packageItem"></param>
        /// <returns></returns>
        public static SDK_PlatformItem CheckForUpdates(this SDK_PlatformItem packageItem)
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
        public static SDK_PlatformItem CreatePackageChildren(this SDK_PlatformItem packageItem)
        {
            if (_systemImages == null)
            {
                _systemImages = Regex.Matches(VerboseOutput, Patterns.SYSTEM_IMAGES_PATTERN, _options);
            }
            packageItem.GetChild(_systemImages);

            if (_googleApis == null)
            {
                _googleApis = Regex.Matches(VerboseOutput, Patterns.GOOGLE_APIS, _options);
            }
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

            return packageItem;
        }
        /// <summary>
        /// Helper method to get specific children of this SDK_PlatformItem. Use by CreatePackageChildren().
        /// </summary>
        /// <param name="packageItem"></param>
        /// <returns></returns>
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
