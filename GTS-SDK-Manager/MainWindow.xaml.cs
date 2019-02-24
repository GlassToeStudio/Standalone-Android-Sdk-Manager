using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GTS_SDK_Manager
{
    public partial class MainWindow : Window
    {
        string pathName = @"C:\Users\USER\AppData\Local\Android\Sdk";
        List<string> packageDataList = new List<string>();
        string[] processOutput;

        List<PackageData> installedPackages = new List<PackageData>();
        List<PackageData> availablePackages = new List<PackageData>();
        List<PackageData> updateablePackages = new List<PackageData>();
        List<PackageData> finalPackageList = new List<PackageData>();

        List<PackageRowUserControl> PackagesToInstallorUpdate = new List<PackageRowUserControl>();
        List<PackageRowUserControl> PackagesToUninstall = new List<PackageRowUserControl>();


        public MainWindow()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            pathName = appdata + @"\Android\Sdk";

            InitializeComponent();

            FolderPathBox.Text = pathName;

            Initialize();
        }

        private void OpenFolder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = FolderPathBox.Text;
                DialogResult result = fbd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    if(File.Exists(fbd.SelectedPath + @"\tools\bin\sdkmanager.bat"))
                    {
                        pathName = fbd.SelectedPath;
                        FolderPathBox.Text = fbd.SelectedPath;
                        Refresh();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("File Not found: " + fbd.SelectedPath + @"\tools\bin\sdkmanager.bat");
                    }
                }
            }
        }

        private void Initialize()
        {
            if ( ! File.Exists(pathName + @"\tools\bin\sdkmanager.bat"))
            {
                System.Windows.Forms.MessageBox.Show("File Not found: " + pathName + @"\tools\bin\sdkmanager.bat");
                return;
            }

            RunSDKManagerListVerbose("--list", "--verbose");

            CreatePackageDataList();

            CreateInstalledPackages();

            CreateAvailablePackageRows();

            CreateUpdatablePackageRows();

            CreateFinalPackageList();

            foreach (var p in finalPackageList)
            {
                bool isChecked = p.Status.Equals("Installed");
                this.AllPackages.Items.Add(new PackageRowUserControl(p.PackageName, p.DisplayName, p.APILevel.ToString(), p.Revision, p.Status, this, isChecked));
            }
        }

        private void Refresh()
        {
            packageDataList = new List<string>();
            installedPackages.Clear();
            availablePackages.Clear();
            updateablePackages.Clear();
            finalPackageList.Clear();
            PackagesToInstallorUpdate.Clear();
            PackagesToUninstall.Clear();
            this.AllPackages.Items.Clear();
            Initialize();
        }

        private void RunSDKManagerListVerbose(params string[] args)
        {
            string YourApplicationPath = pathName + @"\tools\bin\sdkmanager";

            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = YourApplicationPath + ".bat"
            };
            processInfo.Arguments = String.Join(" ", args); //" --list --verbose";

            using (Process pro = new Process())
            {
                pro.StartInfo = processInfo;
                pro.Start();

                using (StreamReader std_out = pro.StandardOutput)
                {
                    processOutput = std_out.ReadToEnd().Split('\n');
                    std_out.Close();
                    pro.Close();
                }

                foreach (var s in processOutput)
                {
                    ConsoleFrame.Text += s + "\n";
                }
            }
        }

        private void CreatePackageDataList()
        {
            bool addOneMore = false;
            bool addTwoMore = false;
            bool addThreeMore = false;

            foreach (var s in processOutput)
            {
                if (addThreeMore)
                {
                    packageDataList.Add(s);
                    addThreeMore = false;
                    continue;
                }
                if (addTwoMore)
                {
                    packageDataList.Add(s);
                    addTwoMore = false;
                    continue;
                }
                if (addOneMore)
                {
                    packageDataList.Add(s);
                    addOneMore = false;
                    continue;
                }

                if (s.Contains("Installed packages:"))
                {
                    packageDataList.Add("Installed packages:");
                    //ConsoleFrame.Text += s.ToString() + "\n";
                }
                if (s.Contains("Available Packages:"))
                {
                    packageDataList.Add("Available Packages:");
                    //ConsoleFrame.Text += s.ToString() + "\n";
                }
                if (s.Contains("Available Updates:"))
                {
                    packageDataList.Add("Available Updates:");
                    //ConsoleFrame.Text += s.ToString() + "\n";
                }
                if (s.Contains("platforms;"))
                {
                    packageDataList.Add(s);
                    //ConsoleFrame.Text += s.ToString() + "\n";
                    addOneMore = addTwoMore = addThreeMore = true;
                }
            }
        }

        private void CreateInstalledPackages()
        {
            /*
            * platforms;android-21
            *       Description:        Android SDK Platform 21
            *       Version:            2
            *       Installed Location: C:\Users\GlassToe\AppData\Local\Android\Sdk\platforms\android-21     
            */
            for (int i = 0; i < packageDataList.Count; i++)
            {
                if (packageDataList[i].Contains("Installed packages:"))
                {
                    for (int j = i + 1; j < packageDataList.Count; j++)
                    {
                        if (packageDataList[j].Contains("Available Packages:"))
                        {
                            return;
                        }
                        // platforms;android-21
                        var packageName = packageDataList[j];
                        var apilevel = (packageDataList[j].Split(';')[1]).Split('-')[1].Trim();
                        j++;
                        // Description:        Android SDK Platform 21
                        var platformName = LookUpTable.CoolNames[(packageDataList[j].Split(':')[1]).Trim()];
                        j++;
                        // Version:            2
                        var revision = (packageDataList[j].Split(':')[1]).Trim();
                        var status = "Installed";

                        installedPackages.Add(new PackageData(packageName, platformName, int.Parse(apilevel), revision, status));
                        //Console.WriteLine(string.Format("Platform {0}, API {1}, Revision {2}, Status {3}", platformName, apilevel, revision, status));
                        //this.AllPackages.Children.Add(new PackageRowUserControl(platformName, apilevel, revision, status));
                        j++;
                    }
                }
            }
        }

        private void CreateAvailablePackageRows()
        {
            for (int i = 0; i < packageDataList.Count; i++)
            {
                if (packageDataList[i].Contains("Available Packages:"))
                {
                    for (int j = i + 1; j < packageDataList.Count; j++)
                    {
                        if (packageDataList[j].Contains("Available Updates:"))
                        {
                            return;
                        }
                        /*
                         * platforms;android-10
                         *       Description:        Android SDK Platform 10
                         *       Version:            2
                        */

                        // platforms;android-21
                        var packageName = packageDataList[j];
                        var apilevel = (packageDataList[j].Split(';')[1]).Split('-')[1].Trim();
                        j++;
                        // Description:        Android SDK Platform 21
                        var platformName = LookUpTable.CoolNames[(packageDataList[j].Split(':')[1]).Trim()];
                        j++;
                        // Version:            2
                        var revision = (packageDataList[j].Split(':')[1]).Trim();
                        var status = "Not Installed";

                        availablePackages.Add(new PackageData(packageName, platformName, int.Parse(apilevel), revision, status));
                        //Console.WriteLine(string.Format("Platform {0}, API {1}, Revision {2}, Status {3}", platformName, apilevel, revision, status));
                        //this.AllPackages.Children.Add(new PackageRowUserControl(platformName, apilevel, revision, status));
                        j++;
                    }
                }
            }
        }

        private void CreateUpdatablePackageRows()
        {
            for (int i = 0; i < packageDataList.Count; i++)
            {
                if (packageDataList[i].Contains("Available Updates:"))
                {
                    for (int j = i + 1; j < packageDataList.Count; j++)
                    {
                        if (packageDataList[j].Contains("Available Updates:"))
                        {
                            return;
                        }
                        /*
                            platforms;android-28
                                Installed Version: 4
                                Available Version: 6
                        */

                        // platforms;android-28
                        var packageName = packageDataList[j];
                        var platformName = (packageDataList[j].Split(';')[1]).Trim();

                        // platforms;android-21
                        var apilevel = platformName.Split('-')[1].Trim();
                        j++;
                        // Version:            2
                        var revision = (packageDataList[j].Split(':')[1]).Trim();
                        var status = "Update Available";

                        platformName = LookUpTable.CoolNames[platformName];
                        platformName = LookUpTable.CoolNames[platformName]; // yes twice.

                        updateablePackages.Add(new PackageData(packageName, platformName, int.Parse(apilevel), revision, status));
                        //Console.WriteLine(string.Format("Platform {0}, API {1}, Revision {2}, Status {3}", platformName, apilevel, revision, status));
                        //this.AllPackages.Children.Add(new PackageRowUserControl(platformName, apilevel, revision, status));
                        j+=2;
                    }
                }
            }
        }

        private void CreateFinalPackageList()
        {
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
                var p = IsInstalled(i);
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
        }

        private PackageData IsInstalled(int i)
        {
            foreach (var p in installedPackages)
            {
                if (availablePackages[i].APILevel == p.APILevel)
                {
                    var u = IsUpdatable(p);
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

        private PackageData IsUpdatable(PackageData p)
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            pathName = FolderPathBox.Text;
        }

        public void ChildChecked(object sender, RoutedEventArgs e)
        {
            var packageData = (PackageRowUserControl)sender;

            // We are an installed package, now we
            if (packageData.InitialState == true)
            {
                if(PackagesToUninstall.Contains(packageData))
                {
                    PackagesToUninstall.Remove(packageData);
                    return;
                }
            }
            else
            {
                if ( ! PackagesToInstallorUpdate.Contains(packageData))
                {
                    PackagesToInstallorUpdate.Add(packageData);
                    return;
                }
            }
        }

        public void ChildUnchecked(object sender, RoutedEventArgs e)
        {
            var packageData = (PackageRowUserControl)sender;

            // We are an installed package, now we
            if (packageData.InitialState == false)
            {
                if (PackagesToInstallorUpdate.Contains(packageData))
                {
                    PackagesToInstallorUpdate.Remove(packageData);
                    return;
                }
            }
            else
            {
                if ( ! PackagesToUninstall.Contains(packageData))
                {
                    PackagesToUninstall.Add(packageData);
                    return;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var p in PackagesToInstallorUpdate)
            {
                ConsoleFrame.Text += "Install: " + p.PackageName + "\n";
                Console.WriteLine("Install: " + p.PackageName);
            }
            foreach (var p in PackagesToUninstall)
            {
                ConsoleFrame.Text += "Uninstall: " + p.PackageName + "\n";
                Console.WriteLine("Uninstall: " + p.PackageName);
            }
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(ArgList.Text);
            RunSDKManagerListVerbose(ArgList.Text);
        }

        private void ArgList_LostFocus(object sender, RoutedEventArgs e)
        {
            var argListText = ArgList.Text;
            var argArray = argListText.Split();
            for (int i = 0; i < argArray.Length; i++)
            {
                if (argArray[i].Contains("--") == false)
                {
                    argArray[i].Trim('-');
                    argArray[i] = "--" + argArray[i];
                }
            }
            argListText = String.Join(" ", argArray);
            ArgList.Text = argListText;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ConsoleFrame.Text = "";
        }
    }

    /*
    * Usage:
      sdkmanager [--uninstall] [<common args>] [--package_file=<file>] [<packages>...]
      sdkmanager --update [<common args>]
      sdkmanager --list [<common args>]
      sdkmanager --licenses [<common args>]
      sdkmanager --version

    With --install (optional), installs or updates packages.
        By default, the listed packages are installed or (if already installed)
        updated to the latest version.
    With --uninstall, uninstall the listed packages.

        <package> is a sdk-style path (e.g. "build-tools;23.0.0" or
                 "platforms;android-23").
        <package-file> is a text file where each line is a sdk-style path
                       of a package to install or uninstall.
        Multiple --package_file arguments may be specified in combination
        with explicit paths.

    With --update, all installed packages are updated to the latest version.

    With --list, all installed and available packages are printed out.

    With --licenses, show and offer the option to accept licenses for all
         available packages that have not already been accepted.

    With --version, prints the current version of sdkmanager.

    Common Arguments:
        --sdk_root=<sdkRootPath>: Use the specified SDK root instead of the SDK
                                  containing this tool

        --channel=<channelId>: Include packages in channels up to <channelId>.
                               Common channels are:
                               0 (Stable), 1 (Beta), 2 (Dev), and 3 (Canary).

        --include_obsolete: With --list, show obsolete packages in the
                            package listing. With --update, update obsolete
                            packages as well as non-obsolete.

        --no_https: Force all connections to use http rather than https.

        --proxy=<http | socks>: Connect via a proxy of the given type.

        --proxy_host=<IP or DNS address>: IP or DNS address of the proxy to use.

        --proxy_port=<port #>: Proxy port to connect to.

        --verbose: Enable verbose output.
        */
}
