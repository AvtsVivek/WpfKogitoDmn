using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Model;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Services;

namespace Mewurk.Hrms.Workflows.WpfKogitoDmnApp.ViewModel
{
    public class DmnFileViewModel : ViewModelBase
    {
        private string _selectedDmnFileName = string.Empty;
        private string _selectedDmnFilePath = string.Empty;
        public ObservableCollection<DmnRule> Rules { get; } = new();



        public DmnFileViewModel()
        {

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
