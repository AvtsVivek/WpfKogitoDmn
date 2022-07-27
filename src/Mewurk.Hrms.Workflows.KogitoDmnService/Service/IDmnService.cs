using Mewurk.Hrms.Workflows.KogitoDmnService.Model;

namespace Mewurk.Hrms.Workflows.KogitoDmnService.Service
{
    public interface IDmnService
    {
        public List<DmnRule> GetRules(string filePath);
        void SaveRules(List<DmnRule> rules, string filePath);

        void DeleteRule(DmnRule ruleToBeDeleted, List<DmnRule> ruleList, string filePath);
    }
}
