namespace XbrlTable.Tests
{
	using System.Linq;
	using NUnit.Framework;

	[TestFixture]
	public class SBP
	{
		string TaxonomyPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/fws/sbp/its-2015-01+2018exercise/2016-11-15"; // "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/fws/sbp/its-2016-svbxx/2016-02-01";
		string MetricPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/dict/met";
		string DimensionPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/dict/dim";
		string DomainPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/dict/dom";

		[Test]
		public void C_101_00()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "c_101.00");

		}

		[Test]
		public void C_101_00_hypercubes()
		{
			var tableCode = "c_101.00";

			var metFile = $"{MetricPath}/met.xsd";
			var metrics = Parsing.ParseNames(metFile);

			var dimFile = $"{DimensionPath}/dim.xsd";
			var dimensions = Parsing.ParseNames(dimFile);

			var expFile = $"{DomainPath}/exp.xsd";
			var typFile = $"{DomainPath}/typ.xsd";
			var domains = Parsing.ParseNames(expFile);
			var typDomains = Parsing.ParseNames(typFile);

			var typedDimensions = Parsing.ParseTypedDimensions(dimFile);

			typDomains.ToList().ForEach(x => domains[x.Key] = x.Value);
			var cubes = Parsing.ParseHypercubes(TaxonomyPath, tableCode, metrics, dimensions, domains, typedDimensions);

			Helper.DumpHypercubes(cubes);
		}

		[Test]
		public void C_102_00()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "c_102.00");

		}

		[Test]
		public void C_103_00()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "c_103.00");

		}

		[Test]
		public void C_105_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "c_105.01");

		}

		[Test]
		public void C_105_03()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "c_105.03");

		}

		[Test]
		public void C_106_00()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "c_106.00");

		}

		[Test]
		public void C_107_01_a()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "c_107.01.a");

		}

		[Test]
		public void C_109_01_a()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "c_109.01.a");

		}

		[Test]
		public void C_109_01_b()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "c_109.01.b");

		}

		[Test]
		public void C_109_02()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "c_109.02");

		}
	}
}
