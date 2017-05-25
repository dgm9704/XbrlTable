namespace XbrlTable
{
	using System;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Linq;
	using System.Xml;

	public static class Parsing
	{

		public static void DumpTable(Table table)
		{
			Console.WriteLine(table.Code);
			Console.Write("\t");
			foreach (var x in table.Axes.Single(a => a.Direction == Direction.X).Ordinates.OrderBy(a => a.Order))
			{
				Console.Write(x.Code + "\t");
			}
			Console.WriteLine();
			foreach (var y in table.Axes.Single(a => a.Direction == Direction.Y).Ordinates.OrderBy(a => a.Order))
			{
				Console.WriteLine(y.Code);
			}
		}

		public static Table ParseTable(string code)
		{
			string tableDirectoryPath = $@"/home/john/Downloads/EBA Taxonomy and supporting documents.2.7.0.0/FullTaxonomy.2.7.0.0/www.eba.europa.eu/eu/fr/xbrl/crr/fws/fp/gl-2014-04/2016-11-15/tab/{code}/";

			string labFileName = $"{code}-lab-codes.xml";

			var labels = new Collection<Label>();
			var lab = new XmlDocument();
			lab.Load(Path.Combine(tableDirectoryPath, labFileName));
			var labNs = new XmlNamespaceManager(lab.NameTable);
			labNs.AddNamespace("link", "http://www.xbrl.org/2003/linkbase");
			labNs.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
			var locators = lab.SelectNodes(".//link:loc", labNs);
			foreach (XmlElement locator in locators)
			{
				//<link:loc xlink:type="locator" xlink:href="p_01.01-rend.xml#eba_tP_01.01" xlink:label="loc_eba_tP_01.01" />
				var href = locator.GetAttribute("xlink:href").Split('#').Last();
				var locatorId = locator.GetAttribute("xlink:label");
				//    <gen:arc xlink:type="arc" xlink:arcrole="http://xbrl.org/arcrole/2008/element-label" xlink:from="loc_eba_tP_01.01" xlink:to="label_eba_tP_01.01" />
				var arcs = lab.SelectNodes($".//node()[@xlink:from='{locatorId}']", labNs);
				foreach (XmlElement arc in arcs)
				{
					var labelId = arc.GetAttribute("xlink:to");
					var labelElement = (XmlElement)lab.SelectSingleNode($".//node()[@xlink:label='{labelId}']", labNs);
					var type = labelElement.GetAttribute("xlink:role").Split('/').Last();
					var value = labelElement.InnerText;
					var language = labelElement.GetAttribute("xml:lang");
					var label = new Label(href, type, language, value);
					//    <label:label xlink:type="resource" xlink:label="label_eba_tP_01.01" xml:lang="en" xlink:role="http://www.eurofiling.info/xbrl/role/rc-code">P 01.01</label:label>
					labels.Add(label);
				}
			}

			string rendFilename = $"{code}-rend.xml";
			var rendFilePath = Path.Combine(tableDirectoryPath, rendFilename);

			var rend = new XmlDocument();
			rend.Load(rendFilePath);
			var rendNs = new XmlNamespaceManager(rend.NameTable);
			rendNs.AddNamespace("table", "http://xbrl.org/2014/table");
			rendNs.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
			rendNs.AddNamespace("formula", "http://xbrl.org/2008/formula");
			var tableElement = rend.SelectSingleNode(".//table:table", rendNs);

			var tableId = tableElement.Attributes["id"].Value;
			var tableBreakDownArcs = rend.SelectNodes($".//table:tableBreakdownArc[@xlink:from='{tableId}']", rendNs);
			var tableCode = labels.Where(l => l.Id == tableId).First(l => l.Type == "rc-code").Value;
			var table = new Table(tableId, tableCode);

			foreach (XmlElement tableBreakDownArc in tableBreakDownArcs)
			{
				// new axis
				var order = int.Parse(tableBreakDownArc.GetAttribute("order"));
				var direction = (Direction)Enum.Parse(typeof(Direction), tableBreakDownArc.GetAttribute("axis"), true);
				var axis = new Axis(order, direction);

				var breakdownTreeArc = rend.SelectSingleNode($".//table:breakdownTreeArc[@xlink:from='{tableBreakDownArc.GetAttribute("xlink:to")}']", rendNs);
				var ruleNodeId = breakdownTreeArc.Attributes["xlink:to"].Value;

				//var ruleNode = rend.SelectSingleNode($".//table:ruleNode[@id='{ruleNodeId}']", ns);

				// axis members = ruleNode children?

				var definitionNodeSubtreeArcs = rend.SelectNodes($".//table:definitionNodeSubtreeArc[@xlink:from='{ruleNodeId}']", rendNs);
				foreach (XmlElement definitionNodeSubtreeArc in definitionNodeSubtreeArcs)
				{
					int o;
					Ordinate ordinate;
					o = int.Parse(definitionNodeSubtreeArc.GetAttribute("order"));
					// New axis ordinate
					var id = definitionNodeSubtreeArc.GetAttribute("xlink:to");
					var ruleNode = (XmlElement)rend.SelectSingleNode($".//table:ruleNode[@id='{id}']", rendNs);
					//var o = int.Parse(ruleNode.Attributes["order"].Value);
					// ordinate members = axis members + ordinate members?

					if (ruleNode.GetAttribute("abstract") == "true")
					{
						var subs = rend.SelectNodes($".//table:definitionNodeSubtreeArc[@xlink:from='{id}']", rendNs);
						foreach (XmlElement sub in subs)
						{
							id = sub.GetAttribute("xlink:to");
							o = int.Parse(sub.GetAttribute("order"));
							var ordinateCode = labels.Where(l => l.Id == id).First(l => l.Type == "rc-code").Value;
							ordinate = new Ordinate(id, ordinateCode, o);
							axis.Ordinates.Add(ordinate);
						}
					}
					else
					{
						var ordinateCode = labels.Where(l => l.Id == id).First(l => l.Type == "rc-code").Value;
						ordinate = new Ordinate(id, ordinateCode, o);
						axis.Ordinates.Add(ordinate);
					}
				}


				table.Axes.Add(axis);
			}
			return table;
		}
	}
}
