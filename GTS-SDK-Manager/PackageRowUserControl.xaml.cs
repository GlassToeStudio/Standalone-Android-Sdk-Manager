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

    /// <summary>
    /// Interaction logic for PackageRowUserControl.xaml
    /// </summary>
    public partial class PackageRowUserControl : UserControl
    {

        public string APILevel
        {
            get { return (string)GetValue(APILevelProperty); }
            set { SetValue(APILevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for APILevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty APILevelProperty =
            DependencyProperty.Register("APILevel", typeof(string), typeof(PackageRowUserControl), new PropertyMetadata(default(string)));


        public string Revision
        {
            get { return (string)GetValue(RevisionProperty); }
            set { SetValue(RevisionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Revision.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RevisionProperty =
            DependencyProperty.Register("Revision", typeof(string), typeof(PackageRowUserControl), new PropertyMetadata(default(string)));


        public string PackageName
        {
            get { return (string)GetValue(PackageNameProperty); }
            set { SetValue(PackageNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PackageName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PackageNameProperty =
            DependencyProperty.Register("PackageName", typeof(string), typeof(PackageRowUserControl), new PropertyMetadata(default(string)));



        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register("Status", typeof(string), typeof(PackageRowUserControl), new PropertyMetadata(default(string)));



        public PackageRowUserControl(string name, string apilevel, string revision, string status)
        {
            InitializeComponent();
            this.PackageName = name;
            this.APILevel = apilevel;
            this.Revision = revision;
            this.Status = status;
            this.DataContext = this;
        }
    }
}
