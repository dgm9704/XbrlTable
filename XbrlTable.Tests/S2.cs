namespace XbrlTable.Tests
{
	using System.Linq;
	using NUnit.Framework;

	[TestFixture]
	public class S2
	{
		string Directory = "/home/john/Downloads/EIOPA_SolvencyII_XBRL_Taxonomy_2.1.0/eiopa.europa.eu/eu/xbrl/s2md/fws/solvency/solvency2/2016-07-15/tab/";

		[Test]
		public void E_01_01_16_01()
		{
			Helper.DumpAll(Directory, "e.01.01.16.01");
		}

		[Test]
		public void E_02_01_16_01()
		{
			Helper.DumpAll(Directory, "e.02.01.16.01");
		}

		[Test]
		public void E_03_01_16_01()
		{
			Helper.DumpAll(Directory, "e.03.01.16.01");

		}

		[Test]
		public void E_03_01_16_02() // needs work with open Y
		{
			Helper.DumpAll(Directory, "e.03.01.16.02");

		}

		[Test]
		public void S_01_01_01_01()
		{
			Helper.DumpAll(Directory, "s.01.01.01.01");

		}

		[Test]
		public void S_01_01_02_01()
		{
			Helper.DumpAll(Directory, "s.01.01.02.01");

		}

		[Test]
		public void SR_01_01_01_01()
		{
			Helper.DumpAll(Directory, "sr.01.01.01.01");

		}

		[Test]
		public void S_01_02_07_03()
		{
			Helper.DumpAll(Directory, "s.01.02.07.03");

		}

		[Test]
		public void S_01_03_01_02()
		{
			Helper.DumpAll(Directory, "s.01.03.01.02");

		}

		[Test]
		public void S_02_01_01_01()
		{
			Helper.DumpAll(Directory, "s.02.01.01.01");

		}

		[Test]
		public void SR_02_01_01_01()
		{
			Helper.DumpAll(Directory, "sr.02.01.01.01");

		}

		[Test]
		public void SE_02_01_16_01()
		{
			Helper.DumpAll(Directory, "se.02.01.16.01");

		}

		[Test]
		public void S_02_02_01_02()
		{
			Helper.DumpAll(Directory, "s.02.02.01.02");

		}

		[Test]
		public void S_04_01_01_01()
		{
			Helper.DumpAll(Directory, "s.04.01.01.01");

		}

		[Test]
		public void S_04_01_01_02()
		{
			Helper.DumpAll(Directory, "s.04.01.01.02");
		}

		[Test]
		public void S_04_01_01_03()
		{
			Helper.DumpAll(Directory, "s.04.01.01.03");

		}

		[Test]
		public void S_04_01_01_04()
		{
			Helper.DumpAll(Directory, "s.04.01.01.04");

		}

		[Test]
		public void S_05_01_01_01()
		{
			Helper.DumpAll(Directory, "s.05.01.01.01");

		}


		[Test]
		public void S_06_02_01_01()
		{
			Helper.DumpAll(Directory, "s.06.02.01.01");

		}

		[Test]
		public void S_06_02_04_01()
		{
			Helper.DumpAll(Directory, "s.06.02.04.01");

		}

		[Test]
		public void S_12_01_01_01()
		{
			Helper.DumpAll(Directory, "s.12.01.01.01");

		}

		[Test]
		public void S_16_01_01_02()
		{
			Helper.DumpAll(Directory, "s.16.01.01.02");

		}

		[Test]
		public void S_22_06_01_04()
		{
			Helper.DumpAll(Directory, "s.22.06.01.04");

		}

		[Test]
		public void S_23_01_13_01()
		{
			Helper.DumpAll(Directory, "s.23.01.13.01");

		}

		//[Test]
		//public void S_23_01_13_01_cube()
		//{
		//	var cubes = Parsing.ParseHypercubes(Directory, "s.23.01.13.01");
		//	Helper.DumpHypercubes(cubes);
		//}

		//[Test]
		//public void S_26_03_01_02_cube()
		//{
		//	var cubes = Parsing.ParseHypercubes(Directory, "s.26.03.01.02");
		//	Helper.DumpHypercubes(cubes);
		//}

		//[Test]
		//public void S_26_07_01_02_cube()
		//{
		//	Helper.DumpAll(Directory, "s.26.07.01.02");
		//	var cubes = Parsing.ParseHypercubes(Directory, "s.26.07.01.02");
		//	
		//	Helper.DumpHypercubes(cubes);
		//}

		[Test]
		public void S_26_07_01_03()
		{
			Helper.DumpAll(Directory, "s.26.07.01.03");
		}

		[Test]
		public void S_26_07_01_04()
		{
			Helper.DumpAll(Directory, "s.26.07.01.04");
		}
	}
}