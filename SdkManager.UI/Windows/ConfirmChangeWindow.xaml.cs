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
using System.Windows.Shapes;
using System.Windows.Forms;

namespace SdkManager.UI
{
    public partial class ConfirmChangeWindow : Window
    {
        public ConfirmChangeWindow(string information)
        {
            this.DataContext = new ConfirmWindowViewModel(this, information);
            InitializeComponent();
        }
    }
}
