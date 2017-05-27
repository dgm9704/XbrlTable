namespace XbrlTable.Tests
{
	using NUnit.Framework;

	[TestFixture]
	public class SBP
	{
		string Directory = "/home/john/Downloads/EBA Taxonomy and supporting documents.2.6.0.0/FullTaxonomy.2.6.0.0/www.eba.europa.eu/eu/fr/xbrl/crr/fws/sbp/its-2016-svbxx/2016-02-01/tab/";

		[Test]
		public void C_101_00()
		{
			var table = Parsing.ParseTable(Directory, "c_101.00");
			Helper.DumpAxes(table);
		}

		[Test]
		public void C_102_00()
		{
			var table = Parsing.ParseTable(Directory, "c_102.00");
			Helper.DumpAxes(table);
		}

		[Test]
		public void C_103_00()
		{
			var table = Parsing.ParseTable(Directory, "c_103.00");
			Helper.DumpAxes(table);
		}

		[Test]
		public void C_105_01()
		{
			var table = Parsing.ParseTable(Directory, "c_105.01");
			Helper.DumpAxes(table);
		}

		[Test]
		public void C_105_03()
		{
			var table = Parsing.ParseTable(Directory, "c_105.03");
			Helper.DumpAxes(table);
		}

		[Test]
		public void C_106_00()
		{
			var table = Parsing.ParseTable(Directory, "c_106.00");
			Helper.DumpAxes(table);
		}

		[Test]
		public void C_107_01_a()
		{
			var table = Parsing.ParseTable(Directory, "c_107.01.a");
			Helper.DumpAxes(table);
		}

		[Test]
		public void C_109_01_a()
		{
			var table = Parsing.ParseTable(Directory, "c_109.01.a");
			Helper.DumpAxes(table);
		}

		[Test]
		public void C_109_01_b()
		{
			var table = Parsing.ParseTable(Directory, "c_109.01.b");
			Helper.DumpAxes(table);
		}

		[Test]
		public void C_109_02()
		{
			var table = Parsing.ParseTable(Directory, "c_109.02");
			Helper.DumpAxes(table);
		}
	}
}
