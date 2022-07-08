using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Model;

namespace Mewurk.Hrms.Workflows.WpfKogitoDmnApp.ViewModel
{
    public class DmnRuleViewModel : ViewModelBase
    {
        private readonly DmnRule _dmnRule;
        public DmnRuleViewModel(DmnRule dmnRule)
        {
            _dmnRule = dmnRule;
        }

        public string Id => _dmnRule.Id;

        public string Name => _dmnRule.Name;

        public DmnRuleElement DmnRuleEntryName
        {
            get => _dmnRule.DmnRuleEntryName;
            set {
                _dmnRule.DmnRuleEntryName = value;
                RaisePropertyChanged();
            }
        }

        public DmnRuleStatus DmnRuleStatus {
            get => _dmnRule.DmnRuleStatus;
        }

        public DmnRuleElement DmnRuleInputEntryValue
        {
            get => _dmnRule.DmnRuleInputEntryValue;
            set
            {
                _dmnRule.DmnRuleInputEntryValue = value;
                RaisePropertyChanged();
            }
        }

        public DmnRuleElement DmnRuleOutputEntryOne
        {
            get => _dmnRule.DmnRuleOutputEntryOne;
            set
            {
                _dmnRule.DmnRuleOutputEntryOne = value;
                RaisePropertyChanged();
            }
        }

        public DmnRuleElement DmnRuleOutputEntryTwo
        {
            get => _dmnRule.DmnRuleOutputEntryTwo;
            set
            {
                _dmnRule.DmnRuleOutputEntryTwo = value;
                RaisePropertyChanged();
            }
        }

    }
}
