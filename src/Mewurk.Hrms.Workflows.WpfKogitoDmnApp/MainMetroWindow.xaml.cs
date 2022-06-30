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
using MahApps.Metro.Controls;

namespace Mewurk.Hrms.Workflows.WpfKogitoDmnApp
{
    /// <summary>
    /// Interaction logic for MainMetroWindow.xaml
    /// </summary>
    public partial class MainMetroWindow : MetroWindow
    {
        public MainMetroWindow()
        {
            InitializeComponent();
        }
        private void LaunchGitHubSite(object sender, RoutedEventArgs e)
        {
            // Launch the GitHub site...
            MessageBox.Show("Launching Git hub...");
        }

        private void DeployCupCakes(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("DeployCupCakes...");
            // deploy some CupCakes...
        }
    }
}
