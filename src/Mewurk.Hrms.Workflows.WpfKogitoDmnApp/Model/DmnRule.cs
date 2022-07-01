using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Model
{
    public class DmnRule
    {
        public string Name { get; set; } = string.Empty;
        public string InputEntryName { get; set; } = string.Empty;
        public string InputEntryValue { get; set; } = string.Empty;
        public string OutputEntryOne { get; set; } = string.Empty;
        public string OutputEntryTwo { get; set; } = string.Empty;
    }
}
