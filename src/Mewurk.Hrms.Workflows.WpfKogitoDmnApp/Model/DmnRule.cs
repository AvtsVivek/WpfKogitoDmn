using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Model
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

        private DmnRuleStatus _dmnRuleStatus = DmnRuleStatus.Existing;

        public DmnRuleStatus DmnRuleStatus
        {
            get { return _dmnRuleStatus; }
            init { _dmnRuleStatus = DmnRuleStatus.Existing; }
        }

    }

    public class DmnRuleElement {
        public string Id { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public enum DmnRuleStatus
    {
        Existing,
        New,
        MarkedForDeletion
    }
}
