namespace XbrlTable.Tests
{
	using NUnit.Framework;

	[TestFixture]
	public class Test
	{
		[Test]
		public void ParseTest_00_01()
		{
			var table = Parsing.ParseTable("p_00.01");
			Parsing.DumpTable(table);
		}

		[Test]
		public void ParseTest_01_01()
		{
			var table = Parsing.ParseTable("p_01.01");
			Parsing.DumpTable(table);
		}

		[Test]
		public void ParseTest_01_02()
		{
			var table = Parsing.ParseTable("p_01.02");
			Parsing.DumpTable(table);
		}

		[Test]
		public void ParseTest_01_03()
		{
			var table = Parsing.ParseTable("p_01.03");
			Parsing.DumpTable(table);
		}

		[Test]
		public void ParseTest_02_01()
		{
			var table = Parsing.ParseTable("p_02.01");
			Parsing.DumpTable(table);
		}

		[Test]
		public void ParseTest_02_02()
		{
			var table = Parsing.ParseTable("p_02.02");
			Parsing.DumpTable(table);
		}

		[Test]
		public void ParseTest_02_03()
		{
			var table = Parsing.ParseTable("p_02.03");
			Parsing.DumpTable(table);
		}

		[Test]
		public void ParseTest_02_04()
		{
			var table = Parsing.ParseTable("p_02.04");
			Parsing.DumpTable(table);
		}

		[Test]
		public void ParseTest_02_05()
		{
			var table = Parsing.ParseTable("p_02.05");
			Parsing.DumpTable(table);
		}

		[Test]
		public void ParseTest_02_06()
		{
			var table = Parsing.ParseTable("p_02.06");
			Parsing.DumpTable(table);
		}

		[Test]
		public void ParseTest_02_07()
		{
			var table = Parsing.ParseTable("p_02.07");
			Parsing.DumpTable(table);
		}

		[Test]
		public void ParseTest_02_08()
		{
			var table = Parsing.ParseTable("p_02.08");
			Parsing.DumpTable(table);
		}

		[Test]
		public void ParseTest_03_00()
		{
			var table = Parsing.ParseTable("p_03.00");
			Parsing.DumpTable(table);
		}
	}
}
