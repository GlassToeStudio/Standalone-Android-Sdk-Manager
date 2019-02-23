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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string pathName = @"C:\Users\USER\AppData\Local\Android\Sdk";
        static List<string> InfoData = new List<string>();

        public MainWindow()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            pathName = appdata + @"\Android\Sdk";

            InitializeComponent();
            FolderPathBox.Text = pathName;

            //PopulateList();
        }

        private void PopulateList()
        {
            //this.PackageList.Items.Add(new MyItem { Revision = 1, Name = "Oreo", ApiLevel = "20.1" });
            //this.PackageList.Items.Add(new MyItem { Revision = 1, Name = "Oreo", ApiLevel = "20.1" });
            //this.PackageList.Items.Add(new MyItem { Revision = 1, Name = "Oreo", ApiLevel = "20.1" });
            //this.PackageList.Items.Add(new MyItem { Revision = 1, Name = "Oreo", ApiLevel = "20.1" });
            //this.PackageList.Items.Add(new MyItem { Revision = 1, Name = "Oreo", ApiLevel = "20.1" });
            //this.PackageList.Items.Add(new MyItem { Revision = 1, Name = "Oreo", ApiLevel = "20.1" });
            //this.PackageList.Items.Add(new MyItem { Revision = 1, Name = "Oreo", ApiLevel = "20.1" });
            //this.PackageList.Items.Add(new MyItem { Revision = 1, Name = "Oreo", ApiLevel = "20.1" });
            //for (int i = 0; i < InfoData.Count; i++)
            //{
            //    if(InfoData[i].Contains("Available Packages:") || InfoData[i].Contains("Available Updates:"))
            //    {
            //        break;
            //    }

            //    if (InfoData[i].Contains("Installed packages:"))
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        this.AllPackages.Children.Add(new PackageRowUserControl(InfoData[i], 5, 1));
            //    }
            //}
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
            ConsoleOut();
        }

        private void ConsoleOut()
        {
            string YourApplicationPath = pathName + @"\tools\bin\sdkmanager";
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.UseShellExecute = false;
            processInfo.CreateNoWindow = true;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.FileName = YourApplicationPath + ".bat";
            Console.WriteLine(processInfo.FileName);
            //processInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(YourApplicationPath);
            processInfo.Arguments = " --list --verbose";


            using (Process pro = new Process())
            {
                pro.StartInfo = processInfo;
                pro.Start();

                string[] data;
                using (StreamReader std_out = pro.StandardOutput)
                {
                    data = std_out.ReadToEnd().Split('\n');
                    std_out.Close();
                    pro.Close();
                }

                bool addOneMore = false;
                bool addTwoMore = false;
                bool addThreeMore = false;

                foreach (var s in data)
                {
                    if (addThreeMore)
                    {
                        InfoData.Add(s);
                        addThreeMore = false;
                        continue;
                    }
                    if (addTwoMore)
                    {
                        InfoData.Add(s);
                        addTwoMore = false;
                        continue;
                    }
                    if(addOneMore)
                    {
                        InfoData.Add(s);
                        addOneMore = false;
                        continue;
                    }
                    Console.WriteLine(s);
                    if(s.Contains("Installed packages:"))
                    {
                        InfoData.Add("Installed packages:");
                        ConsoleFrame.Text += s.ToString() + "\n";
                    }
                    if (s.Contains("Available Packages:"))
                    {
                        InfoData.Add("Available Packages:");
                        ConsoleFrame.Text += s.ToString() + "\n";
                    }
                    if (s.Contains("Available Updates:"))
                    {
                        InfoData.Add("Available Updates:");
                        ConsoleFrame.Text += s.ToString() + "\n";
                    }
                    if (s.Contains("platforms;"))
                    {
                        InfoData.Add(s);
                        ConsoleFrame.Text += s.ToString() + "\n";
                        addOneMore = true;
                        addTwoMore = true;
                        addThreeMore = true;
                    }
                }

                for (int i = 0; i < InfoData.Count; i++)
                {
                    if (InfoData[i].Contains("Installed packages:"))
                    {
                        for (int j = i+1; j < InfoData.Count; j++)
                        {
                            if (InfoData[j].Contains("Available Packages:"))
                            {
                                j = InfoData.Count;
                                continue;
                            }
                            /*
                             * platforms;android-21
                                    Description:        Android SDK Platform 21
                                    Version:            2
                                    Installed Location: C:\Users\GlassToe\AppData\Local\Android\Sdk\platforms\android-21
                            */
                            Console.WriteLine(j + " : " + InfoData[j]);
                            var api = (InfoData[j].Split(';')[1]).Split('-')[1].Trim();
                            j++;
                            Console.WriteLine(InfoData[j]);
                            var platform = (InfoData[j].Split(':')[1]).Trim();
                            platform = LookUpTable.CoolNames[platform];
                            j++;
                            Console.WriteLine(InfoData[j]);
                            var revision = (InfoData[j].Split(':')[1]).Trim();
                            var status = "Installed";
                            Console.WriteLine(string.Format("Platform {0}, API {1}, Revision {2}, Status {3}", platform, api, revision, status));
                            this.AllPackages.Children.Add(new PackageRowUserControl(platform, api, revision, status));
                            j++;
                        }
                    }
                }
            }
            //PopulateList();
        }

        [DllImport("Kernel32.dll")]
        private static extern bool AttachConsole(int processId);

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
    }

    public static class LookUpTable
    {
        public static Dictionary<string, string> CoolNames = new Dictionary<string, string>
        {
            { "Android SDK Platform 7", "Android 2.1 (Eclair)"},
            { "Android SDK Platform 8", "Android 2.2 (Froyo)"},
            { "Android SDK Platform 9", "Android 2.3 (Gingerbread)"},
            { "Android SDK Platform 10", "Android 2.3.3 (Gingerbread)"},
            { "Android SDK Platform 11", "Android 3.0 (Honeycomb)"},
            { "Android SDK Platform 12", "Android 3.1 (Honeycomb)"},
            { "Android SDK Platform 13", "Android 3.2 (Honeycomb)"},
            { "Android SDK Platform 14", "Android 4.0 (IceCreamsandwich)"},
            { "Android SDK Platform 15", "Android 4.0.3 (IceCreamsandwich)"},
            { "Android SDK Platform 16", "Android 4.1 (Jelly Bean)"},
            { "Android SDK Platform 17", "Android 4.2 (Jelly Bean)"},
            { "Android SDK Platform 18", "Android 4.3 (Jelly Bean)"},
            { "Android SDK Platform 19", "Android 4.4 (KitKat)"},
            { "Android SDK Platform 20", "Android 4.4W (KitKat Wear)"},
            { "Android SDK Platform 21", "Android 5.0 (Lillipop)"},
            { "Android SDK Platform 22", "Android 5.1 (Lillipop)"},
            { "Android SDK Platform 23", "Android 6.0 (Marshmallow)"},
            { "Android SDK Platform 24", "Android 7.0 (Nougat)"},
            { "Android SDK Platform 25", "Android 7.1.1 (Nougat)"},
            { "Android SDK Platform 26", "Android 8.0 (Oreo)"},
            { "Android SDK Platform 27", "Android 8.1 (Oreo)"},
            { "Android SDK Platform 28", "Android 28 (Pie)"}
        };
    }
}
