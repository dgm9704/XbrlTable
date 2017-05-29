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

		public static List<Hypercube> ParseHypercubes(string directory, string code)
		{
			var result = new List<Hypercube>();

			string tableDirectoryPath = $@"{directory}{code}/";

			string defFileName = $"{code}-def.xml";
			var defFilePath = Path.Combine(tableDirectoryPath, defFileName);

			var doc = new XmlDocument();
			doc.Load(defFilePath);
			var ns = CreateNameSpaceManager(doc);

			var definitionLinks = doc.SelectNodes(".//link:definitionLink", ns);
			foreach (XmlElement definitionLink in definitionLinks)
			{
				var metrics = new List<string>();
				var metricNodes = definitionLink.SelectNodes("link:loc[contains(@xlink:href, 'met.xsd')]", ns);
				foreach (XmlElement metricNode in metricNodes)
				{
					var metric = metricNode.GetAttribute("xlink:href").Split('#').Last();
					metrics.Add(metric);
				}


				var contexts = new List<Context>();

				var foos = definitionLink.SelectNodes("link:definitionArc[@xlink:arcrole = 'http://xbrl.org/int/dim/arcrole/all']", ns);
				foreach (XmlElement foo in foos)
				{
					var hypId = foo.GetAttribute("xlink:to");
					// context
					var hyp = definitionLink.SelectSingleNode($"link:loc[@xlink:label='{hypId}']", ns);

					// dimensions
					var hypercubeDimensions = definitionLink.SelectNodes($"link:definitionArc[@xlink:from='{hypId}']", ns);
					foreach (XmlElement hypercubeDimension in hypercubeDimensions)
					{
						var dimensionNodeId = hypercubeDimension.GetAttribute("xlink:to");
						var dimensionNode = (XmlElement)definitionLink.SelectSingleNode($"link:loc[@xlink:label='{dimensionNodeId}']", ns);

						// actually needs to be looked up from the location specified in href!!!
						var dimension = dimensionNode.GetAttribute("xlink:href").Split('#').Last();

						var dimensionDomainNode = (XmlElement)definitionLink.SelectSingleNode($"link:definitionArc[@xlink:from='{dimensionNodeId}']", ns);
						var domainNodeId = dimensionDomainNode.GetAttribute("xlink:to");
						var domainNode = (XmlElement)definitionLink.SelectSingleNode($"link:loc[@xlink:label='{domainNodeId}']", ns);

						// actually needs to be looked up from the location specified in href!!!
						var domain = domainNode.GetAttribute("xlink:href").Split('#').Last();

						var members = new List<string>();
						var domainMemberNodes = definitionLink.SelectNodes($"link:definitionArc[@xlink:from='{domainNodeId}']", ns);
						foreach (XmlElement domainMemberNode in domainMemberNodes)
						{
							var memberNodeId = domainMemberNode.GetAttribute("xlink:to");
							var memberNode = (XmlElement)definitionLink.SelectSingleNode($"link:loc[@xlink:label='{memberNodeId}']", ns);
							// actually needs to be looked up from the location specified in href!!!
							var member = memberNode.GetAttribute("xlink:href").Split('#').Last();
							members.Add(member);
						}

						var context = new Context(dimension, domain, members);
						contexts.Add(context);
					}

				}

				var role = definitionLink.GetAttribute("xlink:role").Split('/').Last();
				result.Add(new Hypercube(metrics, contexts));
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
						ordinate = new Ordinate(ordinateCode, path, metric, signature);
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
					var ordinateCode = labelItem.Value + "*";
					var dimensionNode = aspectNode.SelectSingleNode("table:dimensionAspect", ns);
					var member = dimensionNode.InnerText;
					var signature = new Signature();
					var ordinate = new Ordinate(ordinateCode, path, member, signature);

					ordinates.Add(ordinate);
				}
				if (ordinates.Any())
				{
					var axis = new Axis(order, direction, openAxis, ordinates);
					table.Axes.Add(axis);
				}
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
					var ordinate = new Ordinate(ordinateCode, path, metric, signature);
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
