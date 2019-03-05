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

namespace SdkManger.UI
{
    /// <summary>
    /// Interaction logic for CommandLineView.xaml
    /// </summary>
    public partial class CommandLineView : UserControl
    {
        public CommandLineView()
        {
            InitializeComponent();
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(ArgList.Text);
            //PackageStructure.RunSDKManagerInstall(null, ArgList.Text);
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

    }
}
