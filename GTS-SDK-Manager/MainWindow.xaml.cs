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

        List<PackageData> installedPackages = new List<PackageData>();
        List<PackageData> availablePackages = new List<PackageData>();
        List<PackageData> updateablePackages = new List<PackageData>();
        List<PackageData> finalPackageList = new List<PackageData>();

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
                    pathName = fbd.SelectedPath;
                    FolderPathBox.Text = fbd.SelectedPath;
                    System.Windows.Forms.MessageBox.Show("Folder found: " + fbd.SelectedPath);
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            pathName = FolderPathBox.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //RunSDKManagerListVerbose();

            //CreateInstalledPackages();

            //CreateAvailablePackageRows();

            //CreateUpdatablePackageRows();

            //CreateFinalPackageList();

            //foreach (var p in finalPackageList)
            //{
            //    this.AllPackages.Children.Add(new PackageRowUserControl(p.PlatformName, p.APILevel.ToString(), p.Revision, p.Status));
            //}
        }

        private void Initialize()
        {
            RunSDKManagerListVerbose();

            CreateInstalledPackages();

            CreateAvailablePackageRows();

            CreateUpdatablePackageRows();

            CreateFinalPackageList();

            foreach (var p in finalPackageList)
            {
                bool isChecked = p.Status.Equals("Installed");
                this.AllPackages.Items.Add(new PackageRowUserControl(p.PlatformName, p.APILevel.ToString(), p.Revision, p.Status, this, isChecked));
            }
        }

        public void ChildChecked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(sender.ToString() + " Checked");
        }
        
        public void ChildUnchecked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(sender.ToString() + " Unchecked");
        }

        private void RunSDKManagerListVerbose()
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
            processInfo.Arguments = " --list --verbose";

            using (Process pro = new Process())
            {
                pro.StartInfo = processInfo;
                pro.Start();

                string[] processOutput;
                using (StreamReader std_out = pro.StandardOutput)
                {
                    processOutput = std_out.ReadToEnd().Split('\n');
                    std_out.Close();
                    pro.Close();
                }

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
                        ConsoleFrame.Text += s.ToString() + "\n";
                    }
                    if (s.Contains("Available Packages:"))
                    {
                        packageDataList.Add("Available Packages:");
                        ConsoleFrame.Text += s.ToString() + "\n";
                    }
                    if (s.Contains("Available Updates:"))
                    {
                        packageDataList.Add("Available Updates:");
                        ConsoleFrame.Text += s.ToString() + "\n";
                    }
                    if (s.Contains("platforms;"))
                    {
                        packageDataList.Add(s);
                        ConsoleFrame.Text += s.ToString() + "\n";
                        addOneMore = addTwoMore = addThreeMore = true;
                    }
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
                        var apilevel = (packageDataList[j].Split(';')[1]).Split('-')[1].Trim();
                        j++;
                        // Description:        Android SDK Platform 21
                        var platformName = LookUpTable.CoolNames[(packageDataList[j].Split(':')[1]).Trim()];
                        j++;
                        // Version:            2
                        var revision = (packageDataList[j].Split(':')[1]).Trim();
                        var status = "Installed";

                        installedPackages.Add(new PackageData(platformName, int.Parse(apilevel), revision, status));
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
                        var apilevel = (packageDataList[j].Split(';')[1]).Split('-')[1].Trim();
                        j++;
                        // Description:        Android SDK Platform 21
                        var platformName = LookUpTable.CoolNames[(packageDataList[j].Split(':')[1]).Trim()];
                        j++;
                        // Version:            2
                        var revision = (packageDataList[j].Split(':')[1]).Trim();
                        var status = "Not Installed";

                        availablePackages.Add(new PackageData(platformName, int.Parse(apilevel), revision, status));
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
                        var platformName = (packageDataList[j].Split(';')[1]).Trim();

                        // platforms;android-21
                        var apilevel = platformName.Split('-')[1].Trim();
                        j++;
                        // Version:            2
                        var revision = (packageDataList[j].Split(':')[1]).Trim();
                        var status = "Update Available";

                        platformName = LookUpTable.CoolNames[platformName];
                        platformName = LookUpTable.CoolNames[platformName]; // yes twice.

                        updateablePackages.Add(new PackageData(platformName, int.Parse(apilevel), revision, status));
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
    }
}
