namespace XbrlTable.Tests
{
	using NUnit.Framework;
	using System;

	[TestFixture]
	public class Metric
	{
		string MetricPath = "/home/john/xbrl/eiopa.europa.eu/eu/xbrl/s2md/dict/met";

		[Test]
		public void ParseS2Metrics()
		{
			var file = $"{MetricPath}/met.xsd";
			var metrics = Parsing.ParseNames(file);
			Console.WriteLine(metrics.Count);
		}
	}
}