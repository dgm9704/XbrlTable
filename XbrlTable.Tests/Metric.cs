namespace XbrlTable.Tests
{
	using NUnit.Framework;
	using System;

	[TestFixture]
	public class Metric
	{

		[Test]
		public void ParseS2Metrics()
		{
			var file = "/home/john/Downloads/EIOPA_SolvencyII_XBRL_Taxonomy_2.1.0/eiopa.europa.eu/eu/xbrl/s2md/dict/met/met.xsd";
			var metrics = Parsing.ParseNames(file);
			Console.WriteLine(metrics.Count);
		}
	}
}