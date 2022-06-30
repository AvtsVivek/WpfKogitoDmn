using System;
using System.Collections.Generic;
using System.IO;
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
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Properties;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.ViewModel;

namespace WiredBrainCoffee.CustomersApp.View
{
    /// <summary>
    /// Interaction logic for CustomersView.xaml
    /// </summary>
    public partial class DmnFileView : UserControl
    {
        private readonly DmnFileViewModel _viewModel;
        public DmnFileView()
        {
            InitializeComponent();
            _viewModel = new DmnFileViewModel();
            DataContext = _viewModel;
            Loaded += DmnFileView_Loaded;
        }

        private void DmnFileView_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Settings.Default.DmnFilePath))
            {
                _viewModel.SelectedDmnFilePath = Settings.Default.DmnFilePath;
                _viewModel.SelectedDmnFileName = System.IO.Path.GetFileName(_viewModel.SelectedDmnFilePath);
            }
        }

        private void ButtonMoveNavigation_Click(object sender, RoutedEventArgs e)
        {
            //var column = (int)customerListGrid.GetValue(Grid.ColumnProperty);

            //var newColumn = column == 0 ? 2 : 0;
            //customerListGrid.SetValue(Grid.ColumnProperty, newColumn);

            var column = Grid.GetColumn(customerListGrid);

            var newColumn = column == 0 ? 2 : 0;
            Grid.SetColumn(customerListGrid, newColumn);
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.InitialDirectory = @"c:\temp\";
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".dmn"; // Default file extension
            dialog.Filter = "Dmn files (*.dmn)|*.dmn|Xml files (*.dml)|*.xml|Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (File.Exists(Settings.Default.DmnFilePath))
                dialog.InitialDirectory = System.IO.Path.GetDirectoryName(Settings.Default.DmnFilePath);
            else
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                Settings.Default.DmnFilePath = dialog.FileName;
                Settings.Default.Save();
                _viewModel.SelectedDmnFilePath = dialog.FileName;
            }
        }
    }
}
