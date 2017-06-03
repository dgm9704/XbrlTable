namespace XbrlTable.Tests
{
	using System.Linq;
	using NUnit.Framework;

	[TestFixture]
	public class S2
	{
		string TaxonomyPath = "/home/john/xbrl/eiopa.europa.eu/eu/xbrl/s2md/fws/solvency/solvency2/2016-07-15";
		string MetricPath = "/home/john/xbrl/eiopa.europa.eu/eu/xbrl/s2md/dict/met";
		string DimensionPath = "/home/john/xbrl/eiopa.europa.eu/eu/xbrl/s2c/dict/dim";
		string DomainPath = "/home/john/xbrl/eiopa.europa.eu/eu/xbrl/s2c/dict/dom";

		[Test]
		public void E_01_01_16_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "e.01.01.16.01");
		}

		[Test]
		public void E_02_01_16_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "e.02.01.16.01");
		}

		[Test]
		public void E_03_01_16_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "e.03.01.16.01");

		}

		[Test]
		public void E_03_01_16_02() // needs work with open Y
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "e.03.01.16.02");

		}

		[Test]
		public void S_01_01_01_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.01.01.01.01");

		}

		[Test]
		public void S_01_01_02_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.01.01.02.01");

		}

		[Test]
		public void SR_01_01_01_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "sr.01.01.01.01");

		}

		[Test]
		public void S_01_02_07_03()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.01.02.07.03");

		}

		[Test]
		public void S_01_03_01_02()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.01.03.01.02");

		}

		[Test]
		public void S_02_01_01_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.02.01.01.01");

		}

		[Test]
		public void SR_02_01_01_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "sr.02.01.01.01");

		}

		[Test]
		public void SE_02_01_16_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "se.02.01.16.01");

		}

		[Test]
		public void S_02_02_01_02()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.02.02.01.02");

		}

		[Test]
		public void S_04_01_01_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.04.01.01.01");

		}

		[Test]
		public void S_04_01_01_02()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.04.01.01.02");
		}

		[Test]
		public void S_04_01_01_03()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.04.01.01.03");

		}

		[Test]
		public void S_04_01_01_04()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.04.01.01.04");

		}

		[Test]
		public void S_05_01_01_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.05.01.01.01");

		}


		[Test]
		public void S_06_02_01_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.06.02.01.01");

		}

		[Test]
		public void S_06_02_04_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.06.02.04.01");

		}

		[Test]
		public void S_12_01_01_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.12.01.01.01");

		}

		[Test]
		public void S_16_01_01_02()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.16.01.01.02");

		}

		[Test]
		public void S_22_06_01_04()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.22.06.01.04");

		}

		[Test]
		public void S_23_01_13_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.23.01.13.01");

		}

		//[Test]
		//public void S_23_01_13_01_cube()
		//{
		//	var cubes = Parsing.ParseHypercubes(Taxonomy, Dictionary, "s.23.01.13.01");
		//	Helper.DumpHypercubes(cubes);
		//}

		//[Test]
		//public void S_26_03_01_02_cube()
		//{
		//	var cubes = Parsing.ParseHypercubes(Taxonomy, Dictionary, "s.26.03.01.02");
		//	Helper.DumpHypercubes(cubes);
		//}

		//[Test]
		//public void S_26_07_01_02_cube()
		//{
		//	Helper.DumpAll(Taxonomy, Dictionary, "s.26.07.01.02");
		//	var cubes = Parsing.ParseHypercubes(Taxonomy, Dictionary, "s.26.07.01.02");
		//	
		//	Helper.DumpHypercubes(cubes);
		//}

		[Test]
		public void S_26_07_01_03()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.26.07.01.03");
		}

		[Test]
		public void S_26_07_01_04()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "s.26.07.01.04");
		}
	}
}