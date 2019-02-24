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

namespace GTS_SDK_Manager
{
    public partial class PackageRowUserControl : UserControl
    {
        public static readonly DependencyProperty PackageNameProperty =
            DependencyProperty.Register("DisplayName", typeof(string), typeof(PackageRowUserControl), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty APILevelProperty =
            DependencyProperty.Register("APILevel", typeof(string), typeof(PackageRowUserControl), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty RevisionProperty =
            DependencyProperty.Register("Revision", typeof(string), typeof(PackageRowUserControl), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(PackageRowUserControl), new PropertyMetadata(default(string)));

        public string PackageName { get; set; }

        public string DisplayName
        {
            get { return (string)GetValue(PackageNameProperty); }
            set { SetValue(PackageNameProperty, value); }
        }

        public string APILevel
        {
            get { return (string)GetValue(APILevelProperty); }
            set { SetValue(APILevelProperty, value); }
        }

        public string Revision
        {
            get { return (string)GetValue(RevisionProperty); }
            set { SetValue(RevisionProperty, value); }
        }

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public bool IsChecked { get; set; }

        public bool InitialState { get; private set; }


        public MainWindow Main { get; set; }

        public PackageRowUserControl(string packagename, string displayName, string apilevel, string revision, string status, MainWindow main, bool isChecked)
        {
            InitializeComponent();
            this.PackageName = packagename;
            this.DisplayName = displayName;
            this.APILevel = apilevel;
            this.Revision = revision;
            this.Status = status;
            this.Main = main;
            this.IsChecked = isChecked;
            this.InitialState = isChecked;
            this.DataContext = this;
        }

        private void PackageNameHeader_Checked(object sender, RoutedEventArgs e)
        {
            Main.ChildChecked(this, e);
        }

        private void PackageNameHeader_Unchecked(object sender, RoutedEventArgs e)
        {
            Main.ChildUnchecked(this, e);
        }
    }
}