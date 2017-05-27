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

			var rend = new XmlDocument();
			rend.Load(rendFilePath);
			var ns = CreateNameSpaceManager(rend);


			var tableElement = rend.SelectSingleNode(".//table:table", ns);

			var tableId = tableElement.Attributes["id"].Value;
			var tableBreakDownArcs = rend.SelectNodes($".//table:tableBreakdownArc[@xlink:from='{tableId}']", ns);
			var tableCode = labels.Where(l => l.Id == tableId).First(l => l.Type == "rc-code").Value;
			var table = new Table(tableId, tableCode);

			foreach (XmlElement tableBreakDownArc in tableBreakDownArcs)
			{
				// new axis
				var order = int.Parse(tableBreakDownArc.GetAttribute("order"));
				var direction = (Direction)Enum.Parse(typeof(Direction), tableBreakDownArc.GetAttribute("axis"), true);
				var axis = new Axis(order, direction);
				var axisId = tableBreakDownArc.GetAttribute("xlink:to");

				var breakdownTreeArc = (XmlElement)rend.SelectSingleNode($".//table:breakdownTreeArc[@xlink:from='{axisId}']", ns);
				var ruleNodeId = breakdownTreeArc.Attributes["xlink:to"].Value;

				// normal axis ordinates
				var definitionNodeSubtreeArcs = rend.SelectNodes($".//table:definitionNodeSubtreeArc[@xlink:from='{ruleNodeId}']", ns);
				foreach (XmlElement definitionNodeSubtreeArc in definitionNodeSubtreeArcs)
				{
					int o;
					Ordinate ordinate;
					o = int.Parse(definitionNodeSubtreeArc.GetAttribute("order"));
					// New axis ordinate
					var id = definitionNodeSubtreeArc.GetAttribute("xlink:to");
					//var ruleNode = (XmlElement)rend.SelectSingleNode($".//table:ruleNode[@id='{id}']", ns);

					var subOrdinates = SubOrdinates(rend, id, ns, labels);

					foreach (var subOrdinate in subOrdinates)
					{
						axis.Ordinates.Add(subOrdinate);
					}

					var ordinateLabel = labels.Where(l => l.Id == id).FirstOrDefault(l => l.Type == "rc-code");
					var ordinateCode = ordinateLabel.Value;
					if (!string.IsNullOrEmpty(ordinateCode))
					{
						ordinate = new Ordinate(id, ordinateCode, o);
						axis.Ordinates.Add(ordinate);
					}
					else
					{
						Console.WriteLine($"abstract? {id}");
					}

				}

				// key values
				var aspectNodes = rend.SelectNodes($".//table:aspectNode[@id='{breakdownTreeArc.GetAttribute("xlink:to")}']", ns);
				foreach (XmlElement aspectNode in aspectNodes)
				{
					var o = int.Parse(breakdownTreeArc.GetAttribute("order"));
					var aspectId = aspectNode.GetAttribute("id");
					var labelItem = labels.FirstOrDefault(l => l.Id == axisId);
					var ordinateLabel = labelItem.Value + "*";

					var ordinate = new Ordinate(aspectId, ordinateLabel, o);
					axis.Ordinates.Add(ordinate);
				}

				table.Axes.Add(axis);
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
			////rendNs.AddNamespace("table", "http://xbrl.org/2014/table");
			////rendNs.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
			////rendNs.AddNamespace("formula", "http://xbrl.org/2008/formula");

			return ns;
		}

		public static OrdinateCollection SubOrdinates(XmlDocument doc, string id, XmlNamespaceManager ns, Collection<Label> labels)
		{
			var result = new OrdinateCollection();
			var items = doc.SelectNodes($".//table:definitionNodeSubtreeArc[@xlink:from='{id}']", ns);
			foreach (XmlElement item in items)
			{
				id = item.GetAttribute("xlink:to");
				var order = int.Parse(item.GetAttribute("order"));
				var ordinateCode = labels.Where(l => l.Id == id).First(l => l.Type == "rc-code").Value;
				var ordinate = new Ordinate(id, ordinateCode, order);
				result.Add(ordinate);
				var subItems = SubOrdinates(doc, id, ns, labels);
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
