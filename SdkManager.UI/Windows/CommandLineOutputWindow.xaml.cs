﻿using System;
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

namespace SdkManager.UI
{
    /// <summary>
    /// Interaction logic for CommandLineOutputWindow.xaml
    /// </summary>
    public partial class CommandLineOutputWindow : Window
    {
        public CommandLineOutputWindow(SdkManagerBatViewModel dataContext)
        {
            this.DataContext = new ConsoleOutputWindowViewModel(this, dataContext);
            InitializeComponent();
        }
    }
}
