using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using Mewurk.Hrms.Workflows.KogitoDmnService.Model;
using Mewurk.Hrms.Workflows.KogitoDmnService.Service;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Command;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Properties;

namespace Mewurk.Hrms.Workflows.WpfKogitoDmnApp.ViewModel
{
    public class DmnFileViewModel : ViewModelBase
    {
        private bool _modelChanged = false;
        private string _selectedDmnFileName = string.Empty;
        private string _selectedDmnFilePath = string.Empty;
        public ObservableCollection<DmnRuleViewModel> Rules { get; } = new();
        private readonly IDmnService _dmnService;

        public DmnFileViewModel()
        {
            AddNewDmnRuleCommand = new DelegateCommand(AddNewDmnRule);
            SaveDmnXmlFileCommand = new DelegateCommand(SaveDmnXmlFile, CanSave);
            OpenFileCommand = new DelegateCommand(OpenFile);
            DeleteDmnRuleCommand = new DelegateCommand(DeleteDmnRule);
            _dmnService = new DmnService();
            Rules.CollectionChanged += Rules_CollectionChanged;
        }

        private void Rules_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _modelChanged = true;
        }

        private bool CanSave(object? parameter)
        {
            // This is still work in progress.

            // return _modelChanged;
            return true;
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

                SelectedRule = Rules.FirstOrDefault()!;
            }

            _modelChanged = false;
            SaveDmnXmlFileCommand.RaiseCanExecuteChanged();
        }

        private void SaveDmnXmlFile(object? parameter)
        {
            //var ruleViewModelList = Rules.ToList();

            //var ruleList = new List<DmnRule>();

            //foreach (var ruleViewModel in ruleViewModelList)
            //{
            //    var rule = GetRuleFromViewModel(ruleViewModel);
            //    ruleList.Add(rule);
            //}

            var ruleList = GetRulesFromRuleViewModelList();

            _dmnService.SaveRules(ruleList, SelectedDmnFilePath);

            LoadRules();
        }

        private List<DmnRule> GetRulesFromRuleViewModelList()
        {
            var ruleViewModelList = Rules.ToList();

            var ruleList = new List<DmnRule>();

            foreach (var ruleViewModel in ruleViewModelList)
            {
                var rule = GetRuleFromViewModel(ruleViewModel);
                ruleList.Add(rule);
            }

            return ruleList;
        }

        private DmnRule GetRuleFromViewModel(DmnRuleViewModel ruleViewModel)
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
            return rule;
        }

        private void DeleteDmnRule(object? parameter)
        {
            if (SelectedRule == null)
            {
                MessageBox.Show("No Rule selected. Please select a rule from the list view and then click Delete", "No Rule Selected", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var ruleList = GetRulesFromRuleViewModelList();
            var ruleToBeDeleted = GetRuleFromViewModel(SelectedRule);
            _dmnService.DeleteRule(ruleToBeDeleted, ruleList, SelectedDmnFilePath);
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
            _modelChanged = true;
            SaveDmnXmlFileCommand.RaiseCanExecuteChanged();
        }

        public DelegateCommand OpenFileCommand { get; }
        public DelegateCommand SaveDmnXmlFileCommand { get; }
        public DelegateCommand AddNewDmnRuleCommand { get; }
        public DelegateCommand DeleteDmnRuleCommand { get; }

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
