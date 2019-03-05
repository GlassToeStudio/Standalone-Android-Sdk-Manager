using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;

namespace SdkManager.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainWindowViewModel();

            InitializeComponent();

            this.MainTabControl.SelectedIndex = 0;
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
                        //PackageStructure.pathName = fbd.SelectedPath;
                        FolderPathBox.Text = fbd.SelectedPath;
                    }
                    else
                    {
                        FolderPathBox.Text = "";
                        System.Windows.Forms.MessageBox.Show("File Not found: " + fbd.SelectedPath + @"\tools\bin\sdkmanager.bat");
                    }
                }
            }
        }

        private void ArgList_LostFocus(object sender, RoutedEventArgs e)
        {
            //var argListText = ArgList.Text;
            //var argArray = argListText.Split();
            //for (int i = 0; i < argArray.Length; i++)
            //{
            //    if (argArray[i].Contains("--") == false)
            //    {
            //        argArray[i].Trim('-');
            //        argArray[i] = "--" + argArray[i];
            //    }
            //}
            //argListText = String.Join(" ", argArray);
            //ArgList.Text = argListText;
        }

        private void FolderPathBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = ((System.Windows.Controls.TextBox)sender).GetBindingExpression(System.Windows.Controls.TextBox.TextProperty);
            binding.UpdateSource();
        }
    }
}
