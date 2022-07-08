using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Command;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Model;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Properties;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Services;

namespace Mewurk.Hrms.Workflows.WpfKogitoDmnApp.ViewModel
{
    public class DmnFileViewModel : ViewModelBase
    {
        private string _selectedDmnFileName = string.Empty;
        private string _selectedDmnFilePath = string.Empty;
        public ObservableCollection<DmnRuleViewModel> Rules { get; } = new();
        private readonly IDmnService _dmnService;

        public DmnFileViewModel()
        {
            AddNewDmnRuleCommand = new DelegateCommand(AddNewDmnRule);
            SaveDmnXmlFileCommand = new DelegateCommand(SaveDmnXmlFile);
            OpenFileCommand = new DelegateCommand(OpenFile);
            _dmnService = new DmnService();
        }

        internal void DmnFileView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRules();
        }

        private void OpenFile(object? paramter)
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
                SelectedDmnFilePath = dialog.FileName;
                LoadRules();
            }

        }

        private void LoadRules()
        {
            if (File.Exists(Settings.Default.DmnFilePath))
            {
                SelectedDmnFilePath = Settings.Default.DmnFilePath;
                SelectedDmnFileName = Path.GetFileName(SelectedDmnFilePath);

                var ruleNodes = _dmnService.GetRules(SelectedDmnFilePath);

                Rules.Clear();

                foreach (var node in ruleNodes)
                    Rules.Add(new DmnRuleViewModel(node));

                //SelectedRule = new DmnRuleViewModel(ruleNodes.ToList().FirstOrDefault()!);
                SelectedRule = Rules.FirstOrDefault()!;
            }
        }

        private void SaveDmnXmlFile(object? parameter)
        {
            var ruleViewModelList = Rules.ToList();

            var ruleList = new List<DmnRule>();

            foreach (var ruleViewModel in ruleViewModelList)
            {
                var rule = new DmnRule(ruleViewModel.DmnRuleStatus)
                {
                    Id = ruleViewModel.Id,
                    Name = ruleViewModel.Name,
                    DmnRuleEntryName = ruleViewModel.DmnRuleEntryName,
                    DmnRuleInputEntryValue = ruleViewModel.DmnRuleInputEntryValue,
                    DmnRuleOutputEntryOne = ruleViewModel.DmnRuleOutputEntryOne,
                    DmnRuleOutputEntryTwo = ruleViewModel.DmnRuleOutputEntryTwo,
                    DmnRuleElements = ruleViewModel.DmnRuleElements,
                };
                ruleList.Add(rule);
            }

            // var r = _viewModel.Rules;
            _dmnService.SaveRules(ruleList, SelectedDmnFilePath);
            LoadRules();
        }

        private void AddNewDmnRule(object? parameter)
        {
            var dmnRuleEntryName = new DmnRuleElement
            {
                Id = new Guid().ToString(),
                Name = "inputEntry",
                Value = "\"The new Rule...\""
            };

            var dmnRuleInputEntryValue = new DmnRuleElement
            {
                Id = new Guid().ToString(),
                Name = "inputEntry",
                Value = "1000"
            };

            var dmnRuleOutputEntryOne = new DmnRuleElement
            {
                Id = new Guid().ToString(),
                Name = "outputEntry",
                Value = "1000"
            };

            var dmnRuleOutputEntryTwo = new DmnRuleElement
            {
                Id = new Guid().ToString(),
                Name = "outputEntry",
                Value = "1000"
            };

            var newRule = new DmnRule(DmnRuleStatus.New) {
                DmnRuleEntryName = dmnRuleEntryName,
                DmnRuleInputEntryValue = dmnRuleInputEntryValue,
                DmnRuleOutputEntryOne = dmnRuleOutputEntryOne,
                DmnRuleOutputEntryTwo = dmnRuleOutputEntryTwo,
            };

            newRule.DmnRuleElements.Add(dmnRuleEntryName);
            newRule.DmnRuleElements.Add(dmnRuleInputEntryValue);
            newRule.DmnRuleElements.Add(dmnRuleOutputEntryOne);
            newRule.DmnRuleElements.Add(dmnRuleOutputEntryTwo);

            var newRuleViewModel = new DmnRuleViewModel(newRule);
            SelectedRule = newRuleViewModel;
            Rules.Add(newRuleViewModel);

        }

        public DelegateCommand OpenFileCommand { get; }
        public DelegateCommand SaveDmnXmlFileCommand { get; }
        public DelegateCommand AddNewDmnRuleCommand { get; }

        private DmnRuleViewModel _selectedRule = default!;

        public DmnRuleViewModel SelectedRule
        {
            get => _selectedRule;
            set
            {
                _selectedRule = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedDmnFileName {
            get => _selectedDmnFileName;
            set
            {
                _selectedDmnFileName = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedDmnFilePath
        {
            get => _selectedDmnFilePath;
            set
            {
                _selectedDmnFilePath = value;
                RaisePropertyChanged();
            }
        }
    }
}
