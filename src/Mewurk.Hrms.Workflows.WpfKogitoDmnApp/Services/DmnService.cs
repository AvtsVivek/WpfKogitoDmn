using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Model;

namespace Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Services
{
    public interface IDmnService
    {
        //public IEnumerable<XNode> GetRules(string filePath);
        public List<DmnRule> GetRules(string filePath);
    }
    public class DmnService : IDmnService
    {
        public DmnService()
        {

        }

        public List<DmnRule> GetRules(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("File does not exist!!");
            }

            var dmnElement = XElement.Load(filePath);

            var ruleNodes = GetRuleNodes(dmnElement.DescendantNodesAndSelf());

            var rules = new List<DmnRule>();

            foreach (var ruleNode in ruleNodes)
            {
                var ruleElement = ruleNode as XElement;
                if (ruleElement != null)
                {
                    var rule = new DmnRule();
                    var nodesInsideRules = GetNodesInsideRules(ruleElement.DescendantNodesAndSelf());
                    foreach (var node in nodesInsideRules)
                    {
                        var element = node as XElement;

                        if (element == null)
                            continue;

                        if (element.Name.LocalName == "rule")
                            continue;

                        if (element.Name.LocalName == "inputEntry" && element.PreviousNode == null)
                        { 
                            rule.InputEntryName = element!.Value.Trim('\"');
                            continue;
                        }

                        if (element.Name.LocalName == "inputEntry" && element.PreviousNode != null)
                        {
                            rule.InputEntryValue = element!.Value;
                            continue;
                        }

                        if (element.Name.LocalName == "outputEntry" && element.PreviousNode != null && ((XElement)element.PreviousNode).Name.LocalName == "inputEntry")
                        {
                            rule.OutputEntryOne = element!.Value;
                            continue;
                        }

                        if (element.Name.LocalName == "outputEntry" && element.PreviousNode != null && ((XElement)element.PreviousNode).Name.LocalName == "outputEntry")
                        {
                            rule.OutputEntryTwo = element!.Value;
                            continue;
                        }
                    }

                    rules.Add(rule);
                }
            }

            return rules; //ruleNodes;
        }



        private IEnumerable<XNode> GetRuleNodes(IEnumerable<XNode> nodes)
        {
            var selectedNodes = new List<XNode>();

            var nodeList = nodes.ToList();

            foreach (var node in nodeList)
            {
                var element = node as XElement;

                if (element == null)
                    continue;

                if (element.Name.LocalName == "rule")
                    selectedNodes.Add(element);
            }

            return selectedNodes;
        }

        private IEnumerable<XNode> GetNodesInsideRules(IEnumerable<XNode> nodes)
        {
            var selectedNodes = new List<XNode>();

            var nodeList = nodes.ToList();

            foreach (var node in nodeList)
            {
                var element = node as XElement;

                if (element == null)
                    continue;

                selectedNodes.Add(element);
            }

            return selectedNodes;
        }
    }
}
