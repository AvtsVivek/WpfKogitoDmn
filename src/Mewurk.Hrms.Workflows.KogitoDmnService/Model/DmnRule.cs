﻿namespace Mewurk.Hrms.Workflows.KogitoDmnService.Model
{
    public class DmnRule
    {
        public DmnRule(DmnRuleStatus dmnRuleStatus)
        {
            _dmnRuleStatus = dmnRuleStatus;
        }
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DmnRuleElement DmnRuleEntryName { get; set; } = default!;
        public DmnRuleElement DmnRuleInputEntryValue { get; set; } = default!;
        public DmnRuleElement DmnRuleOutputEntryOne { get; set; } = default!;
        public DmnRuleElement DmnRuleOutputEntryTwo { get; set; } = default!;

        public List<DmnRuleElement> DmnRuleElements { get; set; } = new();

        private DmnRuleStatus _dmnRuleStatus = DmnRuleStatus.Existing;

        public DmnRuleStatus DmnRuleStatus
        {
            get { return _dmnRuleStatus; }
            init { _dmnRuleStatus = DmnRuleStatus.Existing; }
        }

    }
}
