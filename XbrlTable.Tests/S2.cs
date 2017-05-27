namespace XbrlTable.Tests
{
	using NUnit.Framework;

	[TestFixture]
	public class S2
	{
		string Directory = "/home/john/Downloads/EIOPA_SolvencyII_XBRL_Taxonomy_2.1.0/eiopa.europa.eu/eu/xbrl/s2md/fws/solvency/solvency2/2016-07-15/tab/";

		[Test]
		public void E_01_01_16_01()
		{
			var table = Parsing.ParseTable(Directory, "e.01.01.16.01");
			Helper.DumpTable(table);
		}

		[Test]
		public void E_02_01_16_01()
		{
			var table = Parsing.ParseTable(Directory, "e.02.01.16.01");
			Helper.DumpTable(table);
		}

		[Test]
		public void E_03_01_16_01()
		{
			var table = Parsing.ParseTable(Directory, "e.03.01.16.01");
			Helper.DumpTable(table);
		}

		[Test]
		public void E_03_01_16_02() // needs work with open Y
		{
			var table = Parsing.ParseTable(Directory, "e.03.01.16.02");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_01_01_01_01()
		{
			var table = Parsing.ParseTable(Directory, "s.01.01.01.01");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_01_01_02_01()
		{
			var table = Parsing.ParseTable(Directory, "s.01.01.02.01");
			Helper.DumpTable(table);
		}

		[Test]
		public void SR_01_01_01_01()
		{
			var table = Parsing.ParseTable(Directory, "sr.01.01.01.01");
			Helper.DumpTable(table);
		}
	}
}