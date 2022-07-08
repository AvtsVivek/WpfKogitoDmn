using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Command;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Model;
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
            _dmnService = new DmnService();
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
                };
                ruleList.Add(rule);
            }

            // var r = _viewModel.Rules;
            _dmnService.SaveRules(ruleList, SelectedDmnFilePath);
        }

        private void AddNewDmnRule(object? parameter)
        {
            var newRule = new DmnRule(DmnRuleStatus.New) {
                DmnRuleEntryName = new DmnRuleElement {
                    Id = new Guid().ToString(),
                    Value = "\"The new Rule...\""
                }
            };

            var newRuleViewModel = new DmnRuleViewModel(newRule);
            SelectedRule = newRuleViewModel;
            Rules.Add(newRuleViewModel);

        }

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
