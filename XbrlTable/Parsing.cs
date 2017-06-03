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
		public static Dictionary<string, string> ParseNames(string path)
		{
			var metrics = new Dictionary<string, string>();
			var doc = new XmlDocument();
			doc.Load(path);
			var root = doc.DocumentElement;
			var ns = CreateNameSpaceManager(doc);
			var metricNodes = root.SelectNodes("xs:element", ns);
			var metricNs = root.GetAttribute("targetNamespace");
			var metricPrefix = ns.LookupPrefix(metricNs);
			foreach (XmlElement metricNode in metricNodes)
			{
				var metricName = metricNode.GetAttribute("name");
				metrics.Add(metricNode.GetAttribute("id"), $"{metricPrefix}:{metricName}");
			}
			return metrics;
		}

		public static List<Hypercube> ParseHypercubes(string directory, string code,
													  Dictionary<string, string> metricNames,
													  Dictionary<string, string> dimensionNames,
													  Dictionary<string, string> domainNames)
		{
			var result = new List<Hypercube>();

			string tableDirectoryPath = $@"{directory}{code}/";

			string defFileName = $"{code}-def.xml";
			var defFilePath = Path.Combine(tableDirectoryPath, defFileName);

			var doc = new XmlDocument();
			doc.Load(defFilePath);
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

								// actually needs to be looked up from the location specified in href!!!
								var domainId = domainNode.GetAttribute("xlink:href").Split('#').Last();
								domainName = domainNames[domainId];

								var domainMemberNodes = currentDefinitionLink.SelectNodes($"link:definitionArc[@xlink:from='{domainNodeId}']", ns);
								foreach (XmlElement domainMemberNode in domainMemberNodes)
								{
									var memberNodeId = domainMemberNode.GetAttribute("xlink:to");
									var memberNode = (XmlElement)currentDefinitionLink.SelectSingleNode($"link:loc[@xlink:label='{memberNodeId}']", ns);
									// actually needs to be looked up from the location specified in href!!!
									var member = memberNode.GetAttribute("xlink:href").Split('#').Last();
									members.Add(member);
								}
							}

							var context = new Dimension(dimensionName, domainName, members);
							contexts.Add(context);
						}

					}

					//var role = definitionLink.GetAttribute("xlink:role").Split('/').Last();
					result.Add(new Hypercube(metrics, contexts));
				}
			}

			return result;
		}

		public static Table ParseTable(string directory, string code)
		{
			string tableDirectoryPath = $@"{directory}{code}/";

			var labels = ParseLabels(directory, code);

			string rendFilename = $"{code}-rend.xml";
			var rendFilePath = Path.Combine(tableDirectoryPath, rendFilename);

			var doc = new XmlDocument();
			doc.Load(rendFilePath);
			var ns = CreateNameSpaceManager(doc);


			var tableElement = doc.SelectSingleNode(".//table:table", ns);

			var tableId = tableElement.Attributes["id"].Value;
			var tableBreakDownArcs = doc.SelectNodes($".//table:tableBreakdownArc[@xlink:from='{tableId}']", ns);
			var tableCode = labels.Where(l => l.Id == tableId).First(l => l.Type == "rc-code").Value;
			var table = new Table(tableId, tableCode);

			foreach (XmlElement tableBreakDownArc in tableBreakDownArcs)
			{
				// new axis
				var order = int.Parse(tableBreakDownArc.GetAttribute("order"));
				var direction = (Direction)Enum.Parse(typeof(Direction), tableBreakDownArc.GetAttribute("axis"), true);

				var ordinates = new OrdinateCollection();
				var axisSignature = new Signature();
				var axisId = tableBreakDownArc.GetAttribute("xlink:to");

				var breakdownTreeArc = (XmlElement)doc.SelectSingleNode($".//table:breakdownTreeArc[@xlink:from='{axisId}']", ns);
				var ruleNodeId = breakdownTreeArc.Attributes["xlink:to"].Value;
				var ruleNode = (XmlElement)doc.SelectSingleNode($".//table:ruleNode[@id='{ruleNodeId}']", ns);
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
				var definitionNodeSubtreeArcs = doc.SelectNodes($".//table:definitionNodeSubtreeArc[@xlink:from='{ruleNodeId}']", ns);
				foreach (XmlElement definitionNodeSubtreeArc in definitionNodeSubtreeArcs)
				{
					Ordinate ordinate;
					string metric = "";
					var signature = new Signature(axisSignature);

					var path = int.Parse(definitionNodeSubtreeArc.GetAttribute("order")).ToString("000");

					// New axis ordinate
					var id = definitionNodeSubtreeArc.GetAttribute("xlink:to");
					ruleNode = (XmlElement)doc.SelectSingleNode($".//table:ruleNode[@id='{id}']", ns);

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

					var subOrdinates = SubOrdinates(doc, id, ns, labels, path, signature);

					foreach (var subOrdinate in subOrdinates)
					{
						ordinates.Add(subOrdinate);
					}

					var ordinateLabel = labels.Where(l => l.Id == id).FirstOrDefault(l => l.Type == "rc-code");
					var ordinateCode = ordinateLabel.Value;

					if (!string.IsNullOrEmpty(ordinateCode))
					{
						ordinate = new Ordinate(ordinateCode, path, signature);
						ordinates.Add(ordinate);
					}
				}

				// key values
				var aspectNodes = doc.SelectNodes($".//table:aspectNode[@id='{breakdownTreeArc.GetAttribute("xlink:to")}']", ns);
				var openAxis = aspectNodes.Count > 0;

				foreach (XmlElement aspectNode in aspectNodes)
				{
					var path = breakdownTreeArc.GetAttribute("order");
					//var aspectId = aspectNode.GetAttribute("id");
					var labelItem = labels.FirstOrDefault(l => l.Id == axisId);
					var ordinateCode = labelItem.Value;
					var dimensionNode = aspectNode.SelectSingleNode("table:dimensionAspect", ns);
					var dimension = dimensionNode.InnerText;
					var member = "*";
					var signature = new Signature();
					signature.Add(dimension, member);
					var ordinate = new Ordinate(ordinateCode, path, signature);

					ordinates.Add(ordinate);
				}
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

		public static OrdinateCollection SubOrdinates(XmlDocument doc, string id, XmlNamespaceManager ns, Collection<Label> labels, string currentPath, Signature currentSignature)
		{
			var result = new OrdinateCollection();
			var items = doc.SelectNodes($".//table:definitionNodeSubtreeArc[@xlink:from='{id}']", ns);
			foreach (XmlElement item in items)
			{
				id = item.GetAttribute("xlink:to");
				var order = int.Parse(item.GetAttribute("order")).ToString("000");
				var path = $"{currentPath}.{order}";
				var ordinateCode = labels.Where(l => l.Id == id).FirstOrDefault(l => l.Type == "rc-code").Value;
				var metric = "";
				var signature = new Signature(currentSignature);
				var ruleNode = (XmlElement)doc.SelectSingleNode($".//table:ruleNode[@id='{id}']", ns);

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

				var subItems = SubOrdinates(doc, id, ns, labels, path, signature);
				foreach (var subItem in subItems)
				{
					result.Add(subItem);
				}
			}
			return result;
		}

		public static Collection<Label> ParseLabels(string directory, string code)
		{
			string tableDirectoryPath = $@"{directory}{code}/";
			string labFileName = $"{code}-lab-codes.xml";
			var labelFilePath = Path.Combine(tableDirectoryPath, labFileName);

			var labels = new Collection<Label>();
			var doc = new XmlDocument();
			doc.Load(labelFilePath);
			var ns = CreateNameSpaceManager(doc);

			var locators = doc.SelectNodes(".//link:loc", ns);
			foreach (XmlElement locator in locators)
			{
				//<link:loc xlink:type="locator" xlink:href="p_01.01-rend.xml#eba_tP_01.01" xlink:label="loc_eba_tP_01.01" />
				var href = locator.GetAttribute("xlink:href").Split('#').Last();
				var locatorId = locator.GetAttribute("xlink:label");
				//    <gen:arc xlink:type="arc" xlink:arcrole="http://xbrl.org/arcrole/2008/element-label" xlink:from="loc_eba_tP_01.01" xlink:to="label_eba_tP_01.01" />
				var arcs = doc.SelectNodes($".//node()[@xlink:from='{locatorId}']", ns);
				foreach (XmlElement arc in arcs)
				{
					var labelId = arc.GetAttribute("xlink:to");
					var labelElement = (XmlElement)doc.SelectSingleNode($".//node()[@xlink:label='{labelId}']", ns);
					var type = labelElement.GetAttribute("xlink:role").Split('/').Last();
					var value = labelElement.InnerText;
					var language = labelElement.GetAttribute("xml:lang");
					var label = new Label(href, type, language, value);
					//    <label:label xlink:type="resource" xlink:label="label_eba_tP_01.01" xml:lang="en" xlink:role="http://www.eurofiling.info/xbrl/role/rc-code">P 01.01</label:label>
					labels.Add(label);
				}
			}
			return labels;
		}
	}
}
