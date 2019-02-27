using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GTS_SDK_Manager
{
    class PackageStructure
    {
        static TextBox ConsoleFrame;
        public static string pathName = @"C:\Users\USER\AppData\Local\Android\Sdk";

        public async static Task<string[]> RunSDKManagerListVerbose(TextBox consoleFrame, params string[] args)
        {
            ConsoleFrame = consoleFrame;
            string YourApplicationPath = pathName + @"\tools\bin\sdkmanager";
            string[] processOutput;

            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = YourApplicationPath + ".bat"
            };
            processInfo.Arguments = String.Join(" ", args); //" --list --verbose";

            //Process p = new Process();
            //p.StartInfo = processInfo;
            //p.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            //p.Start();
            //p.BeginOutputReadLine();

            using (Process pro = new Process())
            {
                pro.StartInfo = processInfo;
                pro.Start();

                using (StreamReader std_out = pro.StandardOutput)
                {
                    var outstuff = await std_out.ReadToEndAsync();
                    processOutput = outstuff.Split('\n');
                    ConsoleFrame.Text = outstuff;
                    std_out.Close();
                    pro.Close();
                }
            }

            return processOutput;
        }

        public static async Task<List<PackageData>> GetPackageList(TextBox consoleFrame)
        {
            List<string> packageDataList = CreatePackageDataList(await RunSDKManagerListVerbose(consoleFrame, "--list", "--verbose"));

            return CreateFinalPackageList(
                CreateInstalledPackages(packageDataList),
                CreateAvailablePackageRows(packageDataList),
                CreateUpdatablePackageRows(packageDataList)
                );
        }

        private static List<string> CreatePackageDataList(string[] data)
        {
            bool addOneMore = false;
            bool addTwoMore = false;
            bool addThreeMore = false;
            List<string> packageDataList = new List<string>();

            foreach (var d in data)
            {
                if (addThreeMore)
                {
                    packageDataList.Add(d);
                    addThreeMore = false;
                    continue;
                }
                if (addTwoMore)
                {
                    packageDataList.Add(d);
                    addTwoMore = false;
                    continue;
                }
                if (addOneMore)
                {
                    packageDataList.Add(d);
                    addOneMore = false;
                    continue;
                }

                if (d.Contains("Installed packages:"))
                {
                    packageDataList.Add("Installed packages:");
                    //ConsoleFrame.Text += s.ToString() + "\n";
                }
                if (d.Contains("Available Packages:"))
                {
                    packageDataList.Add("Available Packages:");
                    //ConsoleFrame.Text += s.ToString() + "\n";
                }
                if (d.Contains("Available Updates:"))
                {
                    packageDataList.Add("Available Updates:");
                    //ConsoleFrame.Text += s.ToString() + "\n";
                }
                if (d.Contains("platforms;"))
                {
                    packageDataList.Add(d);
                    //ConsoleFrame.Text += s.ToString() + "\n";
                    addOneMore = addTwoMore = addThreeMore = true;
                }
            }

            return packageDataList;
        }

        private static List<PackageData> CreateInstalledPackages(List<string> packageDataList)
        {
            /*
            * platforms;android-21
            *       Description:        Android SDK Platform 21
            *       Version:            2
            *       Installed Location: C:\Users\GlassToe\AppData\Local\Android\Sdk\platforms\android-21     
            */
            List<PackageData> installedPackages = new List<PackageData>();

            for (int i = 0; i < packageDataList.Count; i++)
            {
                if (packageDataList[i].Contains("Installed packages:"))
                {
                    for (int j = i + 1; j < packageDataList.Count; j++)
                    {
                        if (packageDataList[j].Contains("Available Packages:"))
                        {
                            return installedPackages;
                        }
                        // platforms;android-21
                        var packageName = packageDataList[j];
                        var apilevel = (packageName.Split(';')[1]).Split('-')[1].Trim();
                        // Description:        Android SDK Platform 21
                        var displayName = LookUpTable.CoolNames[(packageDataList[j + 1].Split(':')[1]).Trim()];
                        // Version:            2
                        var revision = (packageDataList[j + 2].Split(':')[1]).Trim();
                        var status = "Installed";

                        installedPackages.Add(new PackageData(packageName, displayName, int.Parse(apilevel), revision, status));
                        Console.WriteLine(string.Format("Package {0}, Display {1}, API {2}, Revision {3}, Status {4}", packageName, displayName, apilevel, revision, status));
                        j += 3;
                    }
                }
            }
            return installedPackages;
        }

        private static List<PackageData> CreateAvailablePackageRows(List<string> packageDataList)
        {
            List<PackageData> availablePackages = new List<PackageData>();

            for (int i = 0; i < packageDataList.Count; i++)
            {
                if (packageDataList[i].Contains("Available Packages:"))
                {
                    for (int j = i + 1; j < packageDataList.Count; j++)
                    {
                        if (packageDataList[j].Contains("Available Updates:"))
                        {
                            return availablePackages;
                        }
                        /*
                         * platforms;android-10
                         *       Description:        Android SDK Platform 10
                         *       Version:            2
                        */

                        // platforms;android-21
                        var packageName = packageDataList[j];
                        var apilevel = (packageName.Split(';')[1]).Split('-')[1].Trim();
                        // Description:        Android SDK Platform 21
                        var displayName = LookUpTable.CoolNames[(packageDataList[j + 1].Split(':')[1]).Trim()];
                        // Version:            2
                        var revision = (packageDataList[j + 2].Split(':')[1]).Trim();
                        var status = "Not Installed";

                        availablePackages.Add(new PackageData(packageName, displayName, int.Parse(apilevel), revision, status));
                        Console.WriteLine(string.Format("Package {0}, Display {1}, API {2}, Revision {3}, Status {4}", packageName, displayName, apilevel, revision, status));
                        j += 3;
                    }
                }
            }
            return availablePackages;
        }

        private static List<PackageData> CreateUpdatablePackageRows(List<string> packageDataList)
        {
            List<PackageData> updateablePackages = new List<PackageData>();
            for (int i = 0; i < packageDataList.Count; i++)
            {
                if (packageDataList[i].Contains("Available Updates:"))
                {
                    for (int j = i + 1; j < packageDataList.Count; j++)
                    {
                        if (packageDataList[j].Contains("Available Updates:"))
                        {
                            return updateablePackages;
                        }
                        /*
                            platforms;android-28
                                Installed Version: 4
                                Available Version: 6
                        */

                        // platforms;android-28
                        var packageName = packageDataList[j];
                        var displayName = (packageName.Split(';')[1]).Trim();

                        // platforms;android-21
                        var apilevel = displayName.Split('-')[1].Trim();
                        // Version:            2
                        var revision = (packageDataList[j + 1].Split(':')[1]).Trim();
                        var status = "Update Available";

                        displayName = LookUpTable.CoolNames[displayName];
                        displayName = LookUpTable.CoolNames[displayName]; // yes twice.

                        updateablePackages.Add(new PackageData(packageName, displayName, int.Parse(apilevel), revision, status));
                        Console.WriteLine(string.Format("Package {0}, Display {1}, API {2}, Revision {3}, Status {4}", packageName, displayName, apilevel, revision, status));
                        j += 3;
                    }
                }
            }
            return updateablePackages;
        }

        private static List<PackageData> CreateFinalPackageList(List<PackageData> installedPackages, List<PackageData> availablePackages, List<PackageData> updateablePackages)
        {
            List<PackageData> finalPackageList = new List<PackageData>();

            for (int i = 0; i < installedPackages.Count; i++)
            {
                Console.WriteLine(installedPackages[i].Status);
            }
            for (int i = 0; i < availablePackages.Count; i++)
            {
                Console.WriteLine(availablePackages[i].APILevel);
            }
            for (int i = 0; i < updateablePackages.Count; i++)
            {
                Console.WriteLine(updateablePackages[i].Status);
            }

            availablePackages.Sort();
            for (int i = 0; i < availablePackages.Count; i++)
            {
                var p = IsInstalled(i, installedPackages, availablePackages, updateablePackages);
                if (p != null)
                {
                    finalPackageList.Add(p);
                }
                else
                {
                    finalPackageList.Add(availablePackages[i]);
                }

                Console.WriteLine(finalPackageList[i].Status);
            }
            return finalPackageList;
        }

        private static PackageData IsInstalled(int i, List<PackageData> installedPackages, List<PackageData> availablePackages, List<PackageData> updateablePackages)
        {
            foreach (var p in installedPackages)
            {
                if (availablePackages[i].APILevel == p.APILevel)
                {
                    var u = IsUpdatable(p, updateablePackages);
                    if (u != null)
                    {
                        return u;
                    }
                    else
                    {
                        return p;
                    }
                }
            }
            return null;
        }

        private static PackageData IsUpdatable(PackageData p, List<PackageData> updateablePackages)
        {
            foreach (var u in updateablePackages)
            {
                if (p.APILevel == u.APILevel)
                {
                    return u;
                }
            }
            return null;
        }

        public static void RunSDKManagerInstall(TextBox consoleFrame, params string[] args)
        {
            ConsoleFrame = consoleFrame;
            string YourApplicationPath = pathName + @"\tools\bin\sdkmanager";

            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                FileName = YourApplicationPath + ".bat"
            };
            processInfo.Arguments = String.Join(" ", args); //" --list --verbose";

            Process pro = new Process
            {
                StartInfo = processInfo
            };

            pro.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            pro.Start();
            pro.BeginOutputReadLine();
            var answer = System.Windows.Forms.MessageBox.Show("Accept License Agreement?", "Accept?", System.Windows.Forms.MessageBoxButtons.YesNoCancel);
            if (answer == System.Windows.Forms.DialogResult.Yes)
            {
                pro.StandardInput.WriteLine('y');
            }
            else
            {
                pro.StandardInput.WriteLine('n');
            }
        }

        static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            ConsoleFrame.Dispatcher.Invoke(() => 
            {
                ConsoleFrame.Text += outLine.Data + "\n";
            });
        }
    }
}
