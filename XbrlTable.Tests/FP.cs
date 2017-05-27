namespace XbrlTable.Tests
{
	using NUnit.Framework;

	[TestFixture]
	public class FP
	{
		string Directory = "/home/john/Downloads/EBA Taxonomy and supporting documents.2.7.0.0/FullTaxonomy.2.7.0.0/www.eba.europa.eu/eu/fr/xbrl/crr/fws/fp/gl-2014-04/2016-11-15/tab/";

		[Test]
		public void P_00_01()
		{
			var table = Parsing.ParseTable(Directory, "p_00.01");
			Helper.DumpAxes(table);
		}

		[Test]
		public void P_01_01()
		{
			var table = Parsing.ParseTable(Directory, "p_01.01");
			Helper.DumpAxes(table);
		}

		[Test]
		public void P_01_02()
		{
			var table = Parsing.ParseTable(Directory, "p_01.02");
			Helper.DumpAxes(table);
		}

		[Test]
		public void P_01_03()
		{
			var table = Parsing.ParseTable(Directory, "p_01.03");
			Helper.DumpAxes(table);
		}

		[Test]
		public void P_02_01()
		{
			var table = Parsing.ParseTable(Directory, "p_02.01");
			Helper.DumpAxes(table);
		}

		[Test]
		public void P_02_02()
		{
			var table = Parsing.ParseTable(Directory, "p_02.02");
			Helper.DumpAxes(table);
		}

		[Test]
		public void P_02_03()
		{
			var table = Parsing.ParseTable(Directory, "p_02.03");
			Helper.DumpAxes(table);
		}

		[Test]
		public void P_02_04()
		{
			var table = Parsing.ParseTable(Directory, "p_02.04");
			Helper.DumpAxes(table);
		}

		[Test]
		public void P_02_05()
		{
			var table = Parsing.ParseTable(Directory, "p_02.05");
			Helper.DumpAxes(table);
		}

		[Test]
		public void P_02_06()
		{
			var table = Parsing.ParseTable(Directory, "p_02.06");
			Helper.DumpAxes(table);
		}

		[Test]
		public void P_02_07()
		{
			var table = Parsing.ParseTable(Directory, "p_02.07");
			Helper.DumpAxes(table);
		}

		[Test]
		public void P_02_08()
		{
			var table = Parsing.ParseTable(Directory, "p_02.08");
			Helper.DumpAxes(table);
		}

		[Test]
		public void P_03_00()
		{
			var table = Parsing.ParseTable(Directory, "p_03.00");
			Helper.DumpAxes(table);
		}
	}
}
