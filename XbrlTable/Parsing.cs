namespace XbrlTable
{
	using System;
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
			string rendFilename = $"{code}-rend.xml";
			var rendFilePath = Path.Combine(tableDirectoryPath, rendFilename);

			////var rend = XDocument.Load(rendFilePath);
			////var axes = rend.Descendants().Where(e => e.Name.LocalName == "tableBreakdownArc");
			////Console.WriteLine(string.Join("\n", axes.OrderBy(a => a.Attributes().First(att => att.Name == "order").Value).Select(a => a.Attributes().First(att => att.Name == "axis").Value)));

			var rend = new XmlDocument();
			rend.Load(rendFilePath);
			var ns = new XmlNamespaceManager(rend.NameTable);
			ns.AddNamespace("table", "http://xbrl.org/2014/table");
			ns.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
			ns.AddNamespace("formula", "http://xbrl.org/2008/formula");
			var tableElement = rend.SelectSingleNode(".//table:table", ns);

			var tableId = tableElement.Attributes["id"].Value;
			var tableBreakDownArcs = rend.SelectNodes($".//table:tableBreakdownArc[@xlink:from='{tableId}']", ns);

			var table = new Table(code);

			foreach (XmlElement tableBreakDownArc in tableBreakDownArcs)
			{
				// new axis
				var order = int.Parse(tableBreakDownArc.GetAttribute("order"));
				var direction = (Direction)Enum.Parse(typeof(Direction), tableBreakDownArc.GetAttribute("axis"), true);
				var axis = new Axis(order, direction);

				var breakdownTreeArc = rend.SelectSingleNode($".//table:breakdownTreeArc[@xlink:from='{tableBreakDownArc.GetAttribute("xlink:to")}']", ns);
				var ruleNodeId = breakdownTreeArc.Attributes["xlink:to"].Value;

				//var ruleNode = rend.SelectSingleNode($".//table:ruleNode[@id='{ruleNodeId}']", ns);

				// axis members = ruleNode children?

				var definitionNodeSubtreeArcs = rend.SelectNodes($".//table:definitionNodeSubtreeArc[@xlink:from='{ruleNodeId}']", ns);
				foreach (XmlElement definitionNodeSubtreeArc in definitionNodeSubtreeArcs)
				{
					int o;
					Ordinate ordinate;
					o = int.Parse(definitionNodeSubtreeArc.GetAttribute("order"));
					// New axis ordinate
					var id = definitionNodeSubtreeArc.GetAttribute("xlink:to");
					var ruleNode = (XmlElement)rend.SelectSingleNode($".//table:ruleNode[@id='{id}']", ns);
					//var o = int.Parse(ruleNode.Attributes["order"].Value);
					// ordinate members = axis members + ordinate members?

					if (ruleNode.GetAttribute("abstract") == "true")
					{
						var subs = rend.SelectNodes($".//table:definitionNodeSubtreeArc[@xlink:from='{id}']", ns);
						foreach (XmlElement sub in subs)
						{
							id = sub.GetAttribute("xlink:to");
							o = int.Parse(sub.GetAttribute("order"));
							ordinate = new Ordinate(id, id, o);
							axis.Ordinates.Add(ordinate);
						}
					}
					else
					{
						ordinate = new Ordinate(id, id, o);
						axis.Ordinates.Add(ordinate);
					}
				}


				table.Axes.Add(axis);
			}
			return table;
		}
	}
}
