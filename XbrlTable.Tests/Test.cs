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
	}
}
