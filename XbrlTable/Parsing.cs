namespace XbrlTable
{
	using System;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Linq;
	using System.Xml;
	using System.Xml.XPath;

	public static class Parsing
	{

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

				var axisId = tableBreakDownArc.GetAttribute("xlink:to");

				var breakdownTreeArc = (XmlElement)doc.SelectSingleNode($".//table:breakdownTreeArc[@xlink:from='{axisId}']", ns);
				var ruleNodeId = breakdownTreeArc.Attributes["xlink:to"].Value;

				// normal axis ordinates
				var definitionNodeSubtreeArcs = doc.SelectNodes($".//table:definitionNodeSubtreeArc[@xlink:from='{ruleNodeId}']", ns);
				foreach (XmlElement definitionNodeSubtreeArc in definitionNodeSubtreeArcs)
				{
					Ordinate ordinate;
					string member = "";
					var path = int.Parse(definitionNodeSubtreeArc.GetAttribute("order")).ToString("000");

					// New axis ordinate
					var id = definitionNodeSubtreeArc.GetAttribute("xlink:to");
					var ruleNode = (XmlElement)doc.SelectSingleNode($".//table:ruleNode[@id='{id}']", ns);

					if (ruleNode != null)
					{
						var metricNode = ruleNode.SelectSingleNode("formula:concept/formula:qname", ns);
						if (metricNode != null)
						{
							member = metricNode.InnerText;
						}
					}

					var subOrdinates = SubOrdinates(doc, id, ns, labels, path);

					foreach (var subOrdinate in subOrdinates)
					{
						ordinates.Add(subOrdinate);
					}

					var ordinateLabel = labels.Where(l => l.Id == id).FirstOrDefault(l => l.Type == "rc-code");
					var ordinateCode = ordinateLabel.Value;
					if (!string.IsNullOrEmpty(ordinateCode))
					{
						ordinate = new Ordinate(id, ordinateCode, path, member);
						ordinates.Add(ordinate);
					}
				}

				// key values
				var aspectNodes = doc.SelectNodes($".//table:aspectNode[@id='{breakdownTreeArc.GetAttribute("xlink:to")}']", ns);
				var openAxis = aspectNodes.Count > 0;

				foreach (XmlElement aspectNode in aspectNodes)
				{
					var path = breakdownTreeArc.GetAttribute("order");
					var aspectId = aspectNode.GetAttribute("id");
					var labelItem = labels.FirstOrDefault(l => l.Id == axisId);
					var ordinateCode = labelItem.Value + "*";
					var dimensionNode = aspectNode.SelectSingleNode("table:dimensionAspect", ns);
					var member = dimensionNode.InnerText;
					var ordinate = new Ordinate(aspectId, ordinateCode, path, member);

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

		public static OrdinateCollection SubOrdinates(XmlDocument doc, string id, XmlNamespaceManager ns, Collection<Label> labels, string currentPath)
		{
			var result = new OrdinateCollection();
			var items = doc.SelectNodes($".//table:definitionNodeSubtreeArc[@xlink:from='{id}']", ns);
			foreach (XmlElement item in items)
			{
				id = item.GetAttribute("xlink:to");
				var order = int.Parse(item.GetAttribute("order")).ToString("000");
				var path = $"{currentPath}.{order}";
				var ordinateCode = labels.Where(l => l.Id == id).FirstOrDefault(l => l.Type == "rc-code").Value;
				var member = "";
				var ruleNode = (XmlElement)doc.SelectSingleNode($".//table:ruleNode[@id='{id}']", ns);

				if (ruleNode != null)
				{
					var metricNode = ruleNode.SelectSingleNode("formula:concept/formula:qname", ns);
					if (metricNode != null)
					{
						member = metricNode.InnerText;
					}
				}

				if (!string.IsNullOrEmpty(ordinateCode))
				{
					var ordinate = new Ordinate(id, ordinateCode, path, member);
					result.Add(ordinate);
				}


				var subItems = SubOrdinates(doc, id, ns, labels, path);
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
