//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Xml.Linq;
//using Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Model;

//namespace Mewurk.Hrms.Workflows.WpfKogitoDmnApp.Services
//{
//    public interface IDmnService
//    {
//        public List<DmnRule> GetRules(string filePath);
//        void SaveRules(List<DmnRule> rules, string filePath);

//        void DeleteRule(DmnRule ruleToBeDeleted, List<DmnRule> ruleList, string filePath);
//    }
//    public class DmnService : IDmnService
//    {
//        public DmnService()
//        {

//        }



//        public List<DmnRule> GetRules(string filePath)
//        {
//            if (!File.Exists(filePath))
//            {
//                throw new Exception("File does not exist!!");
//            }

//            var dmnElement = XElement.Load(filePath);

//            var ruleNodes = GetRuleNodes(dmnElement.DescendantNodesAndSelf());

//            var rules = new List<DmnRule>();

//            foreach (var ruleNode in ruleNodes)
//            {
//                var ruleElement = ruleNode as XElement;
//                if (ruleElement != null)
//                {
//                    var rule = new DmnRule(DmnRuleStatus.Existing);

//                    SetIdOnRule(rule, ruleElement);

//                    var nodesInsideRules = GetNodesInsideRules(ruleElement.DescendantNodesAndSelf());
//                    foreach (var node in nodesInsideRules)
//                    {
//                        var element = node as XElement;

//                        if (element == null)
//                            continue;

//                        if (element.Name.LocalName == "rule")
//                            continue;

//                        if (element.Name.LocalName == "inputEntry" && element.PreviousNode == null)
//                        {
//                            var dmnRuleElement = new DmnRuleElement
//                            {
//                                //Value = element!.Value.Trim('\"'),
//                                Value = element!.Value,
//                                Id = element.LastAttribute!.Value,
//                                Name = element.Name.LocalName,
//                            };
//                            rule.DmnRuleEntryName = dmnRuleElement;
//                            rule.DmnRuleElements.Add(dmnRuleElement);
//                            continue;
//                        }

//                        if (element.Name.LocalName == "inputEntry" && element.PreviousNode != null)
//                        {
//                            var dmnRuleElement = new DmnRuleElement
//                            {
//                                //Value = element!.Value.Trim('\"'),
//                                Value = element!.Value,
//                                Id = element.LastAttribute!.Value,
//                                Name = element.Name.LocalName,
//                            };
//                            rule.DmnRuleInputEntryValue = dmnRuleElement;
//                            rule.DmnRuleElements.Add(dmnRuleElement);
//                            continue;
//                        }

//                        if (element.Name.LocalName == "outputEntry" && element.PreviousNode != null && ((XElement)element.PreviousNode).Name.LocalName == "inputEntry")
//                        {
//                            //rule.OutputEntryOne = element!.Value;
//                            //rule.DmnRuleOutputEntryOne = new DmnRuleElement
//                            //{
//                            //    Value = element!.Value,
//                            //    Id = element.LastAttribute!.Value
//                            //};

//                            var dmnRuleElement = new DmnRuleElement
//                            {
//                                Value = element!.Value,
//                                Id = element.LastAttribute!.Value,
//                                Name = element.Name.LocalName,
//                            };

//                            rule.DmnRuleOutputEntryOne = dmnRuleElement;
//                            rule.DmnRuleElements.Add(dmnRuleElement);
//                            continue;
//                        }

//                        if (element.Name.LocalName == "outputEntry" && element.PreviousNode != null && ((XElement)element.PreviousNode).Name.LocalName == "outputEntry")
//                        {
//                            //rule.OutputEntryTwo = element!.Value;
//                            //rule.DmnRuleOutputEntryTwo = new DmnRuleElement
//                            //{
//                            //    Value = element!.Value,
//                            //    Id = element.LastAttribute!.Value
//                            //};

//                            var dmnRuleElement = new DmnRuleElement
//                            {
//                                Value = element!.Value,
//                                Id = element.LastAttribute!.Value,
//                                Name = element.Name.LocalName,
//                            };

//                            rule.DmnRuleOutputEntryTwo = dmnRuleElement;
//                            rule.DmnRuleElements.Add(dmnRuleElement);
//                            continue;
//                        }
//                    }

//                    rules.Add(rule);
//                }
//            }

//            return rules; //ruleNodes;
//        }

//        private void SetIdOnRule(DmnRule rule, XElement ruleElement)
//        {
//            if (ruleElement.FirstAttribute != null)
//                if (ruleElement.FirstAttribute.Name != null)
//                    if (!string.IsNullOrWhiteSpace(ruleElement.FirstAttribute.Name.LocalName))
//                        if (ruleElement.FirstAttribute.Name.LocalName == "id")
//                            if (!string.IsNullOrWhiteSpace(ruleElement.FirstAttribute.Value))
//                                rule.Id = ruleElement.FirstAttribute.Value;
//        }

//        private IEnumerable<XNode> GetRuleNodes(IEnumerable<XNode> nodes)
//        {
//            var selectedNodes = new List<XNode>();

//            var nodeList = nodes.ToList();

//            foreach (var node in nodeList)
//            {
//                var element = node as XElement;

//                if (element == null)
//                    continue;

//                if (element.Name.LocalName == "rule")
//                    selectedNodes.Add(element);
//            }

//            return selectedNodes;
//        }

//        private IEnumerable<XNode> GetNodesInsideRules(IEnumerable<XNode> nodes)
//        {
//            var selectedNodes = new List<XNode>();

//            var nodeList = nodes.ToList();

//            foreach (var node in nodeList)
//            {
//                var element = node as XElement;

//                if (element == null)
//                    continue;

//                selectedNodes.Add(element);
//            }

//            return selectedNodes;
//        }

//        public void SaveRules(List<DmnRule> rules, string filePath)
//        {
//            if (!File.Exists(filePath))
//            {
//                throw new Exception("File does not exist!!");
//            }

//            var dmnRootElement = XDocument.Load(filePath);
//            XNamespace dmnNamespace = "http://www.omg.org/spec/DMN/20180521/MODEL/";

//            UpdateExistingRules(dmnRootElement, rules);
//            AddNewRules(dmnRootElement, rules, dmnNamespace);
//            dmnRootElement.Save(filePath);
            
//        }

//        private void AddNewRules(XDocument dmnRootElement, List<DmnRule> dmnRules, XNamespace dmnNamespace) {

//            var decisionTableElement = dmnRootElement.Descendants(dmnNamespace + "decisionTable").FirstOrDefault()!;

//            // Look for the once that are changed or modified.
//            foreach (var dmnRule in dmnRules)
//            {
//                // Skip those ones that are existing. Pick only newly added ones.
//                if (dmnRule.DmnRuleStatus == DmnRuleStatus.Existing)
//                    continue;

//                var newRuleElements = new List<XElement>();
                

//                foreach (var dmnRuleElement in dmnRule.DmnRuleElements)
//                {
//                    var newRuleElement = new XElement(dmnNamespace + dmnRuleElement.Name,
//                        new XAttribute("id", "_" + Guid.NewGuid().ToString().ToUpper()),
//                            new XElement(dmnNamespace + "text",
//                            dmnRuleElement.Value
//                        ));

//                    newRuleElements.Add(newRuleElement);
//                }

//                var annotationEntryElement = new XElement(dmnNamespace + "annotationEntry",
//                    new XElement(dmnNamespace + "text"));

//                newRuleElements.Add(annotationEntryElement);

//                var newRule = new XElement(dmnNamespace + "rule",
//                    new XAttribute("id", "_" + Guid.NewGuid().ToString().ToUpper()),
//                    newRuleElements);

//                decisionTableElement.Add(newRule);


//            }

            

//            // Look for the once that are changed or modified.
//            //foreach (var dmnRule in dmnRules)
//            //{
//            //    // Skip those ones that are existing. Pick only newly added ones.
//            //    if (dmnRule.DmnRuleStatus == DmnRuleStatus.Existing)
//            //    {

//            //    }
//            //}
//        }

//        private void UpdateExistingRules(XDocument dmnRootElement, List<DmnRule> rules)
//        {
//            var updatedElementCount = 0;
//            var dmnUpdateExistingRuleElementList = new List<DmnRuleElement>();

//            // Look for the once that are changed or modified.
//            foreach (var rule in rules)
//            {
//                // Skip those ones that are newly added.
//                // Consider only those that are existing and not new.
//                if (rule.DmnRuleStatus == DmnRuleStatus.Existing)
//                {
//                    dmnUpdateExistingRuleElementList.Add(rule.DmnRuleOutputEntryOne);
//                    dmnUpdateExistingRuleElementList.Add(rule.DmnRuleOutputEntryTwo);
//                    dmnUpdateExistingRuleElementList.Add(rule.DmnRuleEntryName);
//                    dmnUpdateExistingRuleElementList.Add(rule.DmnRuleInputEntryValue);
//                }
//            }

//            var allNodes = dmnRootElement.DescendantNodes();

//            var allElements = new List<XElement>();

//            foreach (var node in allNodes)
//            {
//                var element = node as XElement;

//                if(element != null)
//                    allElements.Add(element);
//            }

//            foreach (var dmnRuleElement in dmnUpdateExistingRuleElementList)
//            {
//                foreach (var element in allElements)
//                {
//                    if (element.LastAttribute == null)
//                        continue;

//                    if (string.IsNullOrWhiteSpace(element.LastAttribute.Value))
//                        continue;

//                    if (dmnRuleElement.Id != element.LastAttribute.Value)
//                        continue;

//                    if (dmnRuleElement.Value == element.Value)
//                        continue;

//                    var textElement = GetTextElement(element);

//                    if (textElement == null)
//                        continue;

//                    updatedElementCount++; // Just a count to know how many...

//                    textElement.SetValue(dmnRuleElement.Value);
//                }
//            }
//        }

//        public void DeleteRule(DmnRule ruleToBeDeleted, List<DmnRule> ruleList, string filePath)
//        {
//            var dmnRootElement = XDocument.Load(filePath);

//            var allNodes = dmnRootElement.DescendantNodes();

//            var allElements = new List<XElement>();

//            foreach (var node in allNodes)
//            {
//                var element = node as XElement;

//                if (element != null)
//                    allElements.Add(element);
//            }

//            foreach (var element in allElements)
//            {
//                if (element.LastAttribute == null)
//                    continue;

//                if (string.IsNullOrWhiteSpace(element.LastAttribute.Value))
//                    continue;

//                if ("id" != element.LastAttribute.Name.LocalName)
//                    continue;

//                if (ruleToBeDeleted.Id != element.LastAttribute.Value)
//                    continue;

//                element.Remove();
//            }

//            dmnRootElement.Save(filePath);
//        }

//        public XElement? GetTextElement(XElement element)
//        {
//            var childNodes = element.DescendantNodes();
//            foreach (var childNode in childNodes)
//            {
//                var textElement = childNode as XElement;

//                if(textElement == null)
//                    continue ;

//                if(textElement.Name.LocalName == "text")
//                    return textElement;
//            }
//            return null;
//        }
//    }
//}
