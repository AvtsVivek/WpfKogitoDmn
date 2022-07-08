using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Model
{
    public class DmnRule
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DmnRuleElement DmnRuleEntryName { get; set; } = default!;
        public DmnRuleElement DmnRuleInputEntryValue { get; set; } = default!;
        public DmnRuleElement DmnRuleOutputEntryOne { get; set; } = default!;
        public DmnRuleElement DmnRuleOutputEntryTwo { get; set; } = default!;
    }

    public class DmnRuleElement {
        public string Id { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }    
}
