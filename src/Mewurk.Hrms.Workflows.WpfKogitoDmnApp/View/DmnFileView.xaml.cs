using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Properties;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Services;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.ViewModel;

namespace WiredBrainCoffee.CustomersApp.View
{
    /// <summary>
    /// Interaction logic for CustomersView.xaml
    /// </summary>
    public partial class DmnFileView : UserControl
    {
        private readonly DmnFileViewModel _viewModel;
        private readonly IDmnService _dmnService;

        public DmnFileView()
        {
            InitializeComponent();
            _viewModel = new DmnFileViewModel();
            DataContext = _viewModel;
            _dmnService = new DmnService();
            Loaded += DmnFileView_Loaded;
        }

        private void DmnFileView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRules();
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
                dialog.InitialDirectory = Path.GetDirectoryName(Settings.Default.DmnFilePath);
            else
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var result = dialog.ShowDialog();

            if (result == true)
            {
                Settings.Default.DmnFilePath = dialog.FileName;
                Settings.Default.Save();
                _viewModel.SelectedDmnFilePath = dialog.FileName;
                LoadRules();
            }
        }

        private void LoadRules()
        {
            if (File.Exists(Settings.Default.DmnFilePath))
            {
                _viewModel.SelectedDmnFilePath = Settings.Default.DmnFilePath;
                _viewModel.SelectedDmnFileName = Path.GetFileName(_viewModel.SelectedDmnFilePath);

                var ruleNodes = _dmnService.GetRules(_viewModel.SelectedDmnFilePath);

                _viewModel.Rules.Clear();

                foreach (var node in ruleNodes)
                    _viewModel.Rules.Add(node);

                _viewModel.SelectedRule = ruleNodes.ToList().FirstOrDefault()!;
            }
        }

        private void btnShowInExplorer_Click(object sender, RoutedEventArgs e)
        {
            if(!File.Exists(_viewModel.SelectedDmnFilePath))
                return;

            var directoryPath = new FileInfo(_viewModel.SelectedDmnFilePath).Directory!.FullName;

            if (!Directory.Exists(directoryPath))
                return;

            Process.Start("explorer.exe", directoryPath);
        }

        private void btnOpenInVsCode_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(_viewModel.SelectedDmnFilePath))
                return;

            var csCodeExePath = @"C:\Program Files\Microsoft VS Code\Code.exe";

            if (!File.Exists(csCodeExePath))
            {
                MessageBox.Show($"Vs Code does not exist at the location {csCodeExePath}" + Environment.NewLine + "Possible that vs code is not installed on this machine.", "Cannot open your file in VsCode", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var process = new Process();
            var procInfo = new ProcessStartInfo()
            {
                FileName = csCodeExePath,
                Arguments = _viewModel.SelectedDmnFilePath,
            };
            process.StartInfo = procInfo;
            process.Start();
            if (process != null)
                process.Dispose();
        }

        private void btnOpenInNotePadPulsPlus_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(_viewModel.SelectedDmnFilePath))
                return;

            var notepadPlusPlusExePath = @"C:\Program Files\Notepad++\notepad++.exe";


            if (!File.Exists(notepadPlusPlusExePath))
            {
                MessageBox.Show($"Notepad++ does not exist at the location {notepadPlusPlusExePath}" + Environment.NewLine + "Possible that notepad ++ is not installed on this machine.", "Cannot open your file in Notepad++", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var process = new Process();
            var procInfo = new ProcessStartInfo()
            {
                FileName = notepadPlusPlusExePath,
                Arguments = _viewModel.SelectedDmnFilePath,
            };
            process.StartInfo = procInfo;
            process.Start();
            if (process != null)
                process.Dispose();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
