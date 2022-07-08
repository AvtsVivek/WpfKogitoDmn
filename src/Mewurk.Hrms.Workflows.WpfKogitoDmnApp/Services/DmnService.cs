using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Model;

namespace Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Services
{
    public interface IDmnService
    {
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
                    var rule = new DmnRule(DmnRuleStatus.Existing);
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
                            //rule.OutputEntryOne = element!.Value;
                            rule.DmnRuleOutputEntryOne = new DmnRuleElement
                            {
                                Value = element!.Value,
                                Id = element.LastAttribute!.Value
                            };
                            continue;
                        }

                        if (element.Name.LocalName == "outputEntry" && element.PreviousNode != null && ((XElement)element.PreviousNode).Name.LocalName == "outputEntry")
                        {
                            //rule.OutputEntryTwo = element!.Value;
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

            UpdateExistingRules(dmnRootElement, rules);
            dmnRootElement.Save(filePath);

            AddNewRules(dmnRootElement, rules);
            dmnRootElement.Save(filePath);
        }

        private void AddNewRules(XElement dmnElement, List<DmnRule> rules) {
            var dmnRuleElementList = new List<DmnRuleElement>();

            // Look for the once that are changed or modified.
            foreach (var rule in rules)
            {
                // Skip those ones that are existing. Pick only newly added ones.
                if (rule.DmnRuleStatus == DmnRuleStatus.Existing)
                    continue;

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

                if (element != null)
                    allElements.Add(element);
            }

            foreach (var dmnRuleElement in dmnRuleElementList)
            {
                foreach (var element in allElements)
                {
                    //if (element.LastAttribute == null)
                    //    continue;

                    //if (string.IsNullOrWhiteSpace(element.LastAttribute.Value))
                    //    continue;

                    //if (dmnRuleElement.Id != element.LastAttribute.Value)
                    //    continue;

                    //if (dmnRuleElement.Value == element.Value)
                    //    continue;

                    //var textElement = GetTextElement(element);

                    //if (textElement == null)
                    //    continue;

                    // updatedElementCount++; // Just a count to know how many...

                    // textElement.SetValue(dmnRuleElement.Value);
                }
            }

        }

        private void UpdateExistingRules(XElement dmnElement, List<DmnRule> rules)
        {
            var updatedElementCount = 0;
            var dmnUpdateExistingRuleElementList = new List<DmnRuleElement>();
            var dmnNewRuleElementList = new List<DmnRuleElement>();

            // Look for the once that are changed or modified.
            foreach (var rule in rules)
            {
                // Skip those ones that are newly added.
                if (rule.DmnRuleStatus == DmnRuleStatus.Existing)
                {
                    dmnUpdateExistingRuleElementList.Add(rule.DmnRuleOutputEntryOne);
                    dmnUpdateExistingRuleElementList.Add(rule.DmnRuleOutputEntryTwo);
                    dmnUpdateExistingRuleElementList.Add(rule.DmnRuleEntryName);
                    dmnUpdateExistingRuleElementList.Add(rule.DmnRuleInputEntryValue);
                }
            }

            foreach (var rule in rules)
            {
                // Skip those ones that are newly added.
                if (rule.DmnRuleStatus == DmnRuleStatus.New)
                {
                    dmnNewRuleElementList.Add(rule.DmnRuleOutputEntryOne);
                    dmnNewRuleElementList.Add(rule.DmnRuleOutputEntryTwo);
                    dmnNewRuleElementList.Add(rule.DmnRuleEntryName);
                    dmnNewRuleElementList.Add(rule.DmnRuleInputEntryValue);
                }
            }

            var allNodes = dmnElement.DescendantNodesAndSelf();

            var allElements = new List<XElement>();

            foreach (var node in allNodes)
            {
                var element = node as XElement;

                if(element != null)
                    allElements.Add(element);
            }

            foreach (var dmnRuleElement in dmnUpdateExistingRuleElementList)
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

                    updatedElementCount++; // Just a count to know how many...

                    textElement.SetValue(dmnRuleElement.Value);
                }
            }

            foreach (var dmnRuleElement in dmnNewRuleElementList)
            {
                //foreach (var element in allElements)
                //{

                //}
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
