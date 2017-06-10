namespace XbrlTable
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.XPath;

    public static class Parsing
    {
        static Dictionary<string, Dictionary<string, string>> preloaded = new Dictionary<string, Dictionary<string, string>>();
        static Dictionary<string, Dictionary<string, string>> loadedTypedDimensions = new Dictionary<string, Dictionary<string, string>>();

        public static Dictionary<string, string> ParseNames(string path)
        {
            Dictionary<string, string> result;
            if (preloaded.ContainsKey(path))
            {
                result = preloaded[path];
            }
            else
            {
                result = new Dictionary<string, string>();
                var doc = LoadDocument(path);
                var root = doc.DocumentElement;
                var ns = CreateNameSpaceManager(doc);
                var metricNodes = root.SelectNodes("xs:element", ns);
                var metricNs = root.GetAttribute("targetNamespace");
                var metricPrefix = ns.LookupPrefix(metricNs);
                foreach (XmlElement metricNode in metricNodes)
                {
                    var metricName = metricNode.GetAttribute("name");
                    result.Add(metricNode.GetAttribute("id"), $"{metricPrefix}:{metricName}");
                }
                preloaded[path] = result;
            }

            return result;
        }

        public static List<Table> ParseTables(string modulePath)
        {
            var tables = new List<Table>();

            var doc = LoadDocument(modulePath);
            var ns = CreateNameSpaceManager(doc);
            var root = (XmlElement)doc.DocumentElement;

            var moduleElement = (XmlElement)root.SelectSingleNode("xs:element", ns);
            var moduleId = moduleElement.GetAttribute("id");

            var tableElements = root.SelectNodes("xs:import[contains(@namespace, '/tab/')]", ns);

            foreach (XmlElement tableElement in tableElements)
            {
                var tableRelativePath = tableElement.GetAttribute("schemaLocation");
                var tablePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(modulePath), tableRelativePath));
                var table = ParseTable(tablePath);
                tables.Add(table);
            }

            return tables;
        }

        private static Table ParseTable(string tablePath)
        {
            tablePath = Path.GetDirectoryName(tablePath);
            var tableCode = tablePath.Split('/').Last(); ;

            var tableParentPath = tablePath.Replace($"/tab/{tableCode}", "");
            return ParseTable(tableParentPath, tableCode);
        }

        private static XmlDocument LoadDocument(string path)
        {
            var doc = new XmlDocument();
            doc.Load(new FileStream(path, FileMode.Open, FileAccess.Read));
            return doc;
        }

        public static Dictionary<string, string> ParseTypedDimensions(string path)
        {
            Dictionary<string, string> result;
            if (loadedTypedDimensions.ContainsKey(path))
            {
                result = loadedTypedDimensions[path];
            }
            else
            {
                result = new Dictionary<string, string>();
                var doc = LoadDocument(path);
                var root = doc.DocumentElement;
                var ns = CreateNameSpaceManager(doc);
                var dimensionNodes = root.SelectNodes("xs:element[@xbrldt:typedDomainRef]", ns);
                foreach (XmlElement dimensionNode in dimensionNodes)
                {
                    var dimensionId = dimensionNode.GetAttribute("id");
                    var typedDomainRef = dimensionNode.GetAttribute("xbrldt:typedDomainRef");
                    var typedDomain = typedDomainRef.Split('#').Last();
                    result[dimensionId] = typedDomain;
                }
                loadedTypedDimensions[path] = result;
            }

            return result;
        }


        public static List<Hypercube> ParseHypercubes(string taxonomyPath, string tableCode,
                                                      Dictionary<string, string> metricNames,
                                                      Dictionary<string, string> dimensionNames,
                                                      Dictionary<string, string> domainNames,
                                                     Dictionary<string, string> typedDimensions)
        {
            var result = new List<Hypercube>();

            string tableDirectoryPath = $@"{taxonomyPath}/tab/{tableCode}/";

            string defFileName = $"{tableCode}-def.xml";
            var defFilePath = Path.Combine(tableDirectoryPath, defFileName);

            var doc = LoadDocument(defFilePath);
            var ns = CreateNameSpaceManager(doc);
            var root = doc.DocumentElement;

            var definitionLinks = root.SelectNodes("link:definitionLink", ns);
            foreach (XmlElement definitionLink in definitionLinks)
            {
                var metrics = new List<string>();
                var metricNodes = definitionLink.SelectNodes("link:loc[contains(@xlink:href, 'met.xsd')]", ns);


                foreach (XmlElement metricNode in metricNodes)
                {
                    var metricId = metricNode.GetAttribute("xlink:href").Split('#').Last();
                    var metricName = metricNames[metricId];
                    metrics.Add(metricName);
                }

                if (metrics.Any())
                {
                    var contexts = new List<Dimension>();

                    var foos = definitionLink.SelectNodes("link:definitionArc[@xlink:arcrole = 'http://xbrl.org/int/dim/arcrole/all']", ns);
                    foreach (XmlElement foo in foos)
                    {

                        var hypId = foo.GetAttribute("xlink:to");

                        // dimensions
                        var hypercubeDimensions = definitionLink.SelectNodes($"link:definitionArc[@xlink:from='{hypId}']", ns);
                        foreach (XmlElement hypercubeDimension in hypercubeDimensions)
                        {
                            var currentDefinitionLink = definitionLink;
                            var dimensionNodeId = hypercubeDimension.GetAttribute("xlink:to");

                            var targetRole = hypercubeDimension.GetAttribute("xbrldt:targetRole");
                            var dimensionNode = (XmlElement)currentDefinitionLink.SelectSingleNode($"link:loc[@xlink:label='{dimensionNodeId}']", ns);
                            var dimensionHref = dimensionNode.GetAttribute("xlink:href");

                            if (!string.IsNullOrEmpty(targetRole))
                            {
                                currentDefinitionLink = (XmlElement)root.SelectSingleNode($"link:definitionLink[@xlink:role='{targetRole}']", ns);
                                dimensionNode = (XmlElement)currentDefinitionLink.SelectSingleNode($"link:loc[@xlink:href='{dimensionHref}']", ns);
                                dimensionNodeId = dimensionNode.GetAttribute("xlink:label");
                            }

                            var dimensionId = dimensionHref.Split('#').Last();
                            var dimensionName = dimensionNames[dimensionId];

                            var members = new List<string>();
                            string domainName = "";
                            var dimensionDomainNode = (XmlElement)currentDefinitionLink.SelectSingleNode($"link:definitionArc[@xlink:from='{dimensionNodeId}']", ns);
                            if (dimensionDomainNode != null)
                            {
                                var domainNodeId = dimensionDomainNode.GetAttribute("xlink:to");
                                var domainNode = (XmlElement)currentDefinitionLink.SelectSingleNode($"link:loc[@xlink:label='{domainNodeId}']", ns);

                                var domainId = domainNode.GetAttribute("xlink:href").Split('#').Last();
                                domainName = domainNames[domainId];

                                var domainMemberNodes = currentDefinitionLink.SelectNodes($"link:definitionArc[@xlink:from='{domainNodeId}']", ns);
                                foreach (XmlElement domainMemberNode in domainMemberNodes)
                                {
                                    if (domainMemberNode.GetAttribute("xbrldt:usable") == "false")
                                    {
                                        Console.WriteLine("whoa, disallowed member");
                                    }
                                    else
                                    {
                                        var memberNodeId = domainMemberNode.GetAttribute("xlink:to");
                                        var memberNode = (XmlElement)currentDefinitionLink.SelectSingleNode($"link:loc[@xlink:label='{memberNodeId}']", ns);
                                        // actually needs to be looked up from the location specified in href!!!
                                        var member = memberNode.GetAttribute("xlink:href").Split('#').Last();
                                        members.Add(member);
                                    }
                                }
                            }
                            else
                            {
                                var domainId = typedDimensions[dimensionId];
                                domainName = domainNames[domainId];
                                members.Add("*");
                            }

                            var context = new Dimension(dimensionName, domainName, members);
                            contexts.Add(context);
                        }

                    }

                    result.Add(new Hypercube(metrics, contexts));
                }
            }

            return result;
        }

        public static Table ParseTable(string taxonomyPath, string code)
        {
            string tableDirectoryPath = $@"{taxonomyPath}/tab/{code}/";

            var labels = ParseLabels(taxonomyPath, code);

            string rendFilename = $"{code}-rend.xml";
            var rendFilePath = Path.Combine(tableDirectoryPath, rendFilename);

            var doc = LoadDocument(rendFilePath);
            var ns = CreateNameSpaceManager(doc);
            var root = (XmlElement)doc.DocumentElement.SelectSingleNode("gen:link", ns);

            var tableElement = root.SelectSingleNode("table:table", ns);

            var tableId = tableElement.Attributes["id"].Value;
            var tableBreakDownArcs = root.SelectNodes($"table:tableBreakdownArc[@xlink:from='{tableId}']", ns);
            var tableCode = labels.Where(l => l.Id == tableId).First(l => l.Type == "rc-code").Value;
            var table = new Table(tableId, tableCode);

            foreach (XmlElement tableBreakDownArc in tableBreakDownArcs)
            {
                // new axis
                var order = int.Parse(tableBreakDownArc.GetAttribute("order"));
                var direction = (Direction)Enum.Parse(typeof(Direction), tableBreakDownArc.GetAttribute("axis"), true);

                var axisSignature = new Signature();
                var axisId = tableBreakDownArc.GetAttribute("xlink:to");

                var breakdownTreeArc = (XmlElement)root.SelectSingleNode($"table:breakdownTreeArc[@xlink:from='{axisId}']", ns);
                var ruleNodeId = breakdownTreeArc.Attributes["xlink:to"].Value;
                var ruleNode = (XmlElement)root.SelectSingleNode($"table:ruleNode[@id='{ruleNodeId}']", ns);
                if (ruleNode != null)
                {
                    var sigNodes = ruleNode.SelectNodes("formula:explicitDimension", ns);
                    foreach (XmlElement sigNode in sigNodes)
                    {
                        var dimension = sigNode.GetAttribute("dimension");
                        var member = sigNode.SelectSingleNode("formula:member/formula:qname", ns).InnerText;
                        axisSignature.Add(dimension, member);
                    }
                }

                // normal axis ordinates
                var ordinates = ParseOrdinates(root, ruleNodeId, ns, labels, "", new Signature(axisSignature));

                var aspectId = breakdownTreeArc.GetAttribute("xlink:to");
                var aspectOrder = breakdownTreeArc.GetAttribute("order");

                // key values
                var keyOrdinates = ParseKeyOrdinates(root, ns, aspectId, aspectOrder, axisId, labels);
                var openAxis = keyOrdinates.Count > 0;

                ordinates.AddRange(keyOrdinates);

                if (ordinates.Any())
                {
                    var axis = new Axis(order, direction, openAxis, ordinates);
                    table.Axes.Add(axis);
                }
            }

            if (!table.Axes.Any(a => a.Direction == Direction.Y && !a.IsOpen))
            {
                table.Axes.Add(new Axis(1, Direction.Y, false, new OrdinateCollection() { new Ordinate("999", "0", new Signature()) }));
            }

            var yAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.Y);

            var yOrdinates = (yAxis.Ordinates ?? new OrdinateCollection()).OrderBy(o => o.Path).ToList();

            if (!yOrdinates.Any())
            {


            }

            return table;
        }

        static XmlNamespaceManager CreateNameSpaceManager(XmlDocument doc)
        {
            var ns = new XmlNamespaceManager(doc.NameTable);
            var nav = doc.CreateNavigator();
            nav.MoveToFollowing(XPathNodeType.Element);
            var namespaces = nav.GetNamespacesInScope(XmlNamespaceScope.All);

            foreach (var n in namespaces)
            {
                ns.AddNamespace(n.Key, n.Value);
            }
            return ns;
        }

        public static OrdinateCollection ParseKeyOrdinates(XmlElement root, XmlNamespaceManager ns, string aspectId, string order, string axisId, Collection<Label> labels)
        {
            var ordinates = new OrdinateCollection();

            var aspectNodes = root.SelectNodes($"table:aspectNode[@id='{aspectId}']", ns);


            foreach (XmlElement aspectNode in aspectNodes)
            {
                var path = order;
                var labelItem = labels.FirstOrDefault(l => l.Id == axisId);
                var ordinateCode = labelItem.Value;
                var dimensionNode = aspectNode.SelectSingleNode("table:dimensionAspect", ns);
                var dimension = dimensionNode.InnerText;

                //<table:aspectNodeFilterArc xlink:type="arc" xlink:arcrole="http://xbrl.org/arcrole/2014/aspect-node-filter" xlink:from="s2md_a6.root" xlink:to="s2md_a6.root.filter" complement="false" />

                //	< df:explicitDimension xlink:type = "resource" xlink: label = "s2md_a6.root.filter" id = "s2md_a6.root.filter" > 
                //	< df:dimension >
                //	   < df:qname > s2c_dim:BL </ df:qname >
                //		  </ df:dimension >
                //		   < df:member >
                //			  < df:qname > s2c_LB:x0 </ df:qname >
                //				   < df:linkrole > http://eiopa.europa.eu/xbrl/s2c/role/dict/dom/LB/33</df:linkrole>
                //< df:arcrole > http://xbrl.org/int/dim/arcrole/domain-member</df:arcrole>
                //< df:axis > descendant </ df:axis >
                //   </ df:member >
                //  </ df:explicitDimension >

                var member = "*";
                var filterArc = (XmlElement)root.SelectSingleNode($"table:aspectNodeFilterArc[@xlink:from='{aspectId}']", ns);
                if (filterArc != null)
                {
                    var explicitDimensionId = filterArc.GetAttribute("xlink:to");
                    var explicitDimension = (XmlElement)root.SelectSingleNode($"df:explicitDimension[@id='{explicitDimensionId}']", ns);
                    var linkroleNode = (XmlElement)explicitDimension.SelectSingleNode("df:member/df:linkrole", ns);
                    var role = linkroleNode.InnerText;
                    var r = role.Split('/');
                    member = $"{r[r.Length - 2]}/{r[r.Length - 1]}";
                }

                var signature = new Signature();
                signature.Add(dimension, member);
                var ordinate = new Ordinate(ordinateCode, path, signature);

                ordinates.Add(ordinate);
            }
            return ordinates;
        }

        public static OrdinateCollection ParseOrdinates(XmlElement root, string id, XmlNamespaceManager ns, Collection<Label> labels, string currentPath, Signature currentSignature)
        {
            var result = new OrdinateCollection();
            var items = root.SelectNodes($"table:definitionNodeSubtreeArc[@xlink:from='{id}']", ns);
            foreach (XmlElement item in items)
            {
                id = item.GetAttribute("xlink:to");
                var order = int.Parse(item.GetAttribute("order")).ToString("000");
                var path = string.IsNullOrEmpty(currentPath) ? order : $"{currentPath}.{order}";
                var ordinateCode = labels.Where(l => l.Id == id).FirstOrDefault(l => l.Type == "rc-code").Value;
                var metric = "";
                var signature = new Signature(currentSignature);
                var ruleNode = (XmlElement)root.SelectSingleNode($"table:ruleNode[@id='{id}']", ns);

                if (ruleNode != null)
                {
                    var metricNode = ruleNode.SelectSingleNode("formula:concept/formula:qname", ns);
                    if (metricNode != null)
                    {
                        metric = metricNode.InnerText;
                        signature["met"] = metric;
                    }

                    var sigNodes = ruleNode.SelectNodes("formula:explicitDimension", ns);
                    foreach (XmlElement sigNode in sigNodes)
                    {
                        var dimension = sigNode.GetAttribute("dimension");
                        var member = sigNode.SelectSingleNode("formula:member/formula:qname", ns).InnerText;

                        signature[dimension] = member;
                    }
                }

                if (!string.IsNullOrEmpty(ordinateCode))
                {
                    var ordinate = new Ordinate(ordinateCode, path, signature);
                    result.Add(ordinate);
                }

                var subItems = ParseOrdinates(root, id, ns, labels, path, signature);
                foreach (var subItem in subItems)
                {
                    result.Add(subItem);
                }
            }
            return result;
        }

        public static Collection<Label> ParseLabels(string taxonomyPath, string code)
        {
            string tableDirectoryPath = $@"{taxonomyPath}/tab/{code}/";
            string labFileName = $"{code}-lab-codes.xml";
            var labelFilePath = Path.Combine(tableDirectoryPath, labFileName);

            var labels = new Collection<Label>();
            var doc = LoadDocument(labelFilePath);
            var ns = CreateNameSpaceManager(doc);
            var root = doc.DocumentElement.SelectSingleNode("gen:link", ns);

            var locators = root.SelectNodes("link:loc", ns);
            foreach (XmlElement locator in locators)
            {
                var href = locator.GetAttribute("xlink:href").Split('#').Last();
                var locatorId = locator.GetAttribute("xlink:label");
                var arcs = root.SelectNodes($"node()[@xlink:from='{locatorId}']", ns);
                foreach (XmlElement arc in arcs)
                {
                    var labelId = arc.GetAttribute("xlink:to");
                    var labelElement = (XmlElement)root.SelectSingleNode($"node()[@xlink:label='{labelId}']", ns);
                    var type = labelElement.GetAttribute("xlink:role").Split('/').Last();
                    var value = labelElement.InnerText;
                    var language = labelElement.GetAttribute("xml:lang");
                    var label = new Label(href, type, language, value);
                    labels.Add(label);
                }
            }
            return labels;
        }

        public static List<Tuple<Signature, Address>> ParseDatapoints(Table table)
        {
            var datapoints = new List<Tuple<Signature, Address>>();
            var xAxis = table.Axes.Where(a => a.Direction == Direction.X).Single(a => !a.IsOpen);
            var yAxis = table.Axes.Where(a => a.Direction == Direction.Y).SingleOrDefault(a => !a.IsOpen);
            var tableSignature = table.
                                      Axes.
                                      Where(a => a.IsOpen).
                                      SelectMany(a => a.Ordinates).
                                      SelectMany(o => o.Signature);

            var sheets = table.
                              Axes.
                              Where(a => a.Direction == Direction.Z).
                              Where(a => !a.IsOpen).
                              SelectMany(a => a.Ordinates);

            if (!sheets.Any())
            {
                sheets = new Ordinate[] { new Ordinate("", "", new Signature()) };
            }

            foreach (var sheet in sheets)
            {
                foreach (var y in yAxis.Ordinates)
                {
                    foreach (var x in xAxis.Ordinates)
                    {
                        var address = new Address(table.Code, y.Code, x.Code, sheet.Code);


                        var combinedOrdinates = sheet.Signature.
                                             Concat(tableSignature).
                                             Concat(y.Signature).
                                             Concat(x.Signature).
                                             Where(i => !string.IsNullOrEmpty(i.Value)).
                                             ToList();

                        var combinedSignature = new Signature();
                        combinedOrdinates.ForEach(o => combinedSignature[o.Key] = o.Value);

                        datapoints.Add(Tuple.Create(combinedSignature, address));
                    }
                }
            }
            return datapoints;
        }
    }
}
