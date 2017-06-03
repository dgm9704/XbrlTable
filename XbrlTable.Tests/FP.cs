namespace XbrlTable.Tests
{
	using NUnit.Framework;

	[TestFixture]
	public class FP
	{
		string TaxonomyPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/fws/fp/gl-2014-04/2016-11-15";
		string MetricPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/dict/met";
		string DimensionPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/dict/dim";
		string DomainPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/dict/dom";

		[Test]
		public void P_00_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "p_00.01");
		}

		[Test]
		public void P_01_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "p_01.01");

		}

		[Test]
		public void P_01_02()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "p_01.02");

		}

		[Test]
		public void P_01_03()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "p_01.03");

		}

		[Test]
		public void P_02_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "p_02.01");

		}

		[Test]
		public void P_02_02()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "p_02.02");

		}

		[Test]
		public void P_02_03()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "p_02.03");

		}

		[Test]
		public void P_02_04()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "p_02.04");

		}

		[Test]
		public void P_02_05()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "p_02.05");

		}

		[Test]
		public void P_02_06()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "p_02.06");

		}

		[Test]
		public void P_02_07()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "p_02.07");

		}

		[Test]
		public void P_02_08()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "p_02.08");

		}

		[Test]
		public void P_03_00()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "p_03.00");

		}
	}
}
