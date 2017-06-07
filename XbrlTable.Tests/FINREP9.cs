namespace XbrlTable.Tests
{
	using Xunit;
	using XbrlTable;

	public class FINREP9
	{
		//string TaxonomyPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/fws/finrep/cir-680-2014-ind/2017-04-04";
		string TaxonomyPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/fws/finrep/cp-2015-23+feedback/2016-11-15";
		string MetricPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/dict/met";
		string DimensionPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/dict/dim";
		string DomainPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/dict/dom";

		[Fact]
		public void F_00_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "f_00.01");
		}

		[Fact]
		public void F_04_02_01()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "f_04.02.1");
		}
	}
}
