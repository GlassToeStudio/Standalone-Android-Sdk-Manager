using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;

namespace GTS_SDK_Manager
{
    public partial class MainWindow : Window
    {
        List<PackageRowUserControl> packagesToInstallorUpdate = new List<PackageRowUserControl>();
        List<PackageRowUserControl> packagesToUninstall = new List<PackageRowUserControl>();

        public MainWindow()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            PackageStructure.pathName = appdata + @"\Android\Sdk";

            InitializeComponent();

            FolderPathBox.Text = PackageStructure.pathName;

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
                        PackageStructure.pathName = fbd.SelectedPath;
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
            if ( ! File.Exists(PackageStructure.pathName + @"\tools\bin\sdkmanager.bat"))
            {
                System.Windows.Forms.MessageBox.Show("File Not found: " + PackageStructure.pathName + @"\tools\bin\sdkmanager.bat");
                return;
            }

            List<PackageData> finalPackageList = PackageStructure.GetPackageList(ConsoleFrame);

            foreach (var p in finalPackageList)
            {
                bool isChecked = p.Status.Equals("Installed");
                this.AllPackages.Items.Add(new PackageRowUserControl(p.PackageName, p.DisplayName, p.APILevel.ToString(), p.Revision, p.Status, this, isChecked));
            }
        }

        private void Refresh()
        {
            packagesToInstallorUpdate.Clear();
            packagesToUninstall.Clear();
            this.AllPackages.Items.Clear();
            Initialize();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PackageStructure.pathName = FolderPathBox.Text;
        }

        public void ChildChecked(object sender, RoutedEventArgs e)
        {
            var packageData = (PackageRowUserControl)sender;

            // We are an installed package, now we
            if (packageData.InitialState == true)
            {
                if(packagesToUninstall.Contains(packageData))
                {
                    packagesToUninstall.Remove(packageData);
                    return;
                }
            }
            else
            {
                if ( ! packagesToInstallorUpdate.Contains(packageData))
                {
                    packagesToInstallorUpdate.Add(packageData);
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
                if (packagesToInstallorUpdate.Contains(packageData))
                {
                    packagesToInstallorUpdate.Remove(packageData);
                    return;
                }
            }
            else
            {
                if ( ! packagesToUninstall.Contains(packageData))
                {
                    packagesToUninstall.Add(packageData);
                    return;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var p in packagesToInstallorUpdate)
            {
                ConsoleFrame.Text += "Install: " + p.PackageName + "\n";
                Console.WriteLine("Install: " + p.PackageName);
            }
            foreach (var p in packagesToUninstall)
            {
                ConsoleFrame.Text += "Uninstall: " + p.PackageName + "\n";
                Console.WriteLine("Uninstall: " + p.PackageName);
            }
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(ArgList.Text);
            PackageStructure.RunSDKManagerListVerbose(ConsoleFrame, ArgList.Text);
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
