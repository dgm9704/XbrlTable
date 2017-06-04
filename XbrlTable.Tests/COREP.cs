namespace XbrlTable.Tests
{
	using NUnit.Framework;

	[TestFixture]
	public class COREP
	{
		string TaxonomyPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/fws/corep/its-2016-repxx/2016-02-01/";
		string MetricPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/dict/met";
		string DimensionPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/dict/dim";
		string DomainPath = "/home/john/xbrl/www.eba.europa.eu/eu/fr/xbrl/crr/dict/dom";

		[Test]
		public void C_07_00_a()
		{
			Helper.DumpAll(TaxonomyPath, MetricPath, DimensionPath, DomainPath, "c_07.00.a");
		}
	}
}
