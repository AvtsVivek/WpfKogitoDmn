using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        void SaveRules(List<DmnRule> rules, string filePath);
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
                            //rule.InputEntryName = element!.Value.Trim('\"');
                            rule.InputEntryName = element!.Value;
                            rule.DmnRuleEntryName = new DmnRuleElement
                            {
                                //Value = element!.Value.Trim('\"'),
                                Value = element!.Value,
                                Id = element.LastAttribute!.Value
                            };
                            continue;
                        }

                        if (element.Name.LocalName == "inputEntry" && element.PreviousNode != null)
                        {
                            rule.InputEntryValue = element!.Value;
                            // DmnRuleInputEntryValue
                            rule.DmnRuleInputEntryValue = new DmnRuleElement
                            {
                                //Value = element!.Value.Trim('\"'),
                                Value = element!.Value,
                                Id = element.LastAttribute!.Value
                            };
                            continue;
                        }

                        if (element.Name.LocalName == "outputEntry" && element.PreviousNode != null && ((XElement)element.PreviousNode).Name.LocalName == "inputEntry")
                        {
                            rule.OutputEntryOne = element!.Value;
                            rule.DmnRuleOutputEntryOne = new DmnRuleElement
                            {
                                Value = element!.Value,
                                Id = element.LastAttribute!.Value
                            };
                            continue;
                        }

                        if (element.Name.LocalName == "outputEntry" && element.PreviousNode != null && ((XElement)element.PreviousNode).Name.LocalName == "outputEntry")
                        {
                            rule.OutputEntryTwo = element!.Value;
                            rule.DmnRuleOutputEntryTwo = new DmnRuleElement
                            {
                                Value = element!.Value,
                                Id = element.LastAttribute!.Value
                            };
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

        public void SaveRules(List<DmnRule> rules, string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("File does not exist!!");
            }

            var dmnRootElement = XElement.Load(filePath);

            SaveRules(dmnRootElement, rules);

            dmnRootElement.Save(filePath);
        }

        private void SaveRules(XElement dmnElement, List<DmnRule> rules)
        {
            var dmnRuleElementList = new List<DmnRuleElement>();

            foreach (var rule in rules)
            {
                dmnRuleElementList.Add(rule.DmnRuleOutputEntryOne);
                dmnRuleElementList.Add(rule.DmnRuleOutputEntryTwo);
                dmnRuleElementList.Add(rule.DmnRuleEntryName);
                dmnRuleElementList.Add(rule.DmnRuleInputEntryValue);
            }

            var allNodes = dmnElement.DescendantNodesAndSelf();

            var allElements = new List<XElement>();

            foreach (var node in allNodes)
            {
                var element = node as XElement;

                if(element != null)
                    allElements.Add(element);
            }

            foreach (var dmnRuleElement in dmnRuleElementList)
            {
                foreach (var element in allElements)
                {
                    if (element.LastAttribute == null)
                        continue;

                    if (string.IsNullOrWhiteSpace(element.LastAttribute.Value))
                        continue;

                    if (dmnRuleElement.Id != element.LastAttribute.Value)
                        continue;

                    if (dmnRuleElement.Value == element.Value)
                        continue;

                    var textElement = GetTextElement(element);

                    if (textElement == null)
                        continue;

                    textElement.SetValue(dmnRuleElement.Value);
                }
            }
        }

        public XElement? GetTextElement(XElement element)
        {
            var childNodes = element.DescendantNodes();
            foreach (var childNode in childNodes)
            {
                var textElement = childNode as XElement;

                if(textElement == null)
                    continue ;

                if(textElement.Name.LocalName == "text")
                    return textElement;
            }
            return null;
        }
    }
}
