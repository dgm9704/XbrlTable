﻿namespace XbrlTable.Tests
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

		[Test]
		public void S_01_02_07_03()
		{
			var table = Parsing.ParseTable(Directory, "s.01.02.07.03");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_01_03_01_02()
		{
			var table = Parsing.ParseTable(Directory, "s.01.03.01.02");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_02_01_01_01()
		{
			var table = Parsing.ParseTable(Directory, "s.02.01.01.01");
			Helper.DumpTable(table);
		}

		[Test]
		public void SR_02_01_01_01()
		{
			var table = Parsing.ParseTable(Directory, "sr.02.01.01.01");
			Helper.DumpTable(table);
		}

		[Test]
		public void SE_02_01_16_01()
		{
			var table = Parsing.ParseTable(Directory, "se.02.01.16.01");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_02_02_01_02()
		{
			var table = Parsing.ParseTable(Directory, "s.02.02.01.02");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_04_01_01_01()
		{
			var table = Parsing.ParseTable(Directory, "s.04.01.01.01");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_04_01_01_02()
		{
			var table = Parsing.ParseTable(Directory, "s.04.01.01.02");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_04_01_01_03()
		{
			var table = Parsing.ParseTable(Directory, "s.04.01.01.03");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_04_01_01_04()
		{
			var table = Parsing.ParseTable(Directory, "s.04.01.01.04");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_05_01_01_01()
		{
			var table = Parsing.ParseTable(Directory, "s.05.01.01.01");
			Helper.DumpTable(table);
		}


		[Test]
		public void S_06_02_01_01()
		{
			var table = Parsing.ParseTable(Directory, "s.06.02.01.01");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_06_02_04_01()
		{
			var table = Parsing.ParseTable(Directory, "s.06.02.04.01");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_12_01_01_01()
		{
			var table = Parsing.ParseTable(Directory, "s.12.01.01.01");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_16_01_01_02()
		{
			var table = Parsing.ParseTable(Directory, "s.16.01.01.02");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_22_06_01_04()
		{
			var table = Parsing.ParseTable(Directory, "s.22.06.01.04");
			Helper.DumpTable(table);
		}

		[Test]
		public void S_23_01_13_01()
		{
			var table = Parsing.ParseTable(Directory, "s.23.01.13.01");
			Helper.DumpTable(table);
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
		//	var table = Parsing.ParseTable(Directory, "s.26.07.01.02");
		//	var cubes = Parsing.ParseHypercubes(Directory, "s.26.07.01.02");
		//	Helper.DumpTable(table);
		//	Helper.DumpHypercubes(cubes);
		//}

		[Test]
		public void S_26_07_01_04_cube()
		{
			var metFile = "/home/john/Downloads/EIOPA_SolvencyII_XBRL_Taxonomy_2.1.0/eiopa.europa.eu/eu/xbrl/s2md/dict/met/met.xsd";
			var metrics = Parsing.ParseNames(metFile);

			var dimFile = "/home/john/Downloads/EIOPA_SolvencyII_XBRL_Taxonomy_2.1.0/eiopa.europa.eu/eu/xbrl/s2c/dict/dim/dim.xsd";
			var dimensions = Parsing.ParseNames(dimFile);

			var expFile = "/home/john/Downloads/EIOPA_SolvencyII_XBRL_Taxonomy_2.1.0/eiopa.europa.eu/eu/xbrl/s2c/dict/dom/exp.xsd";
			var typFile = "/home/john/Downloads/EIOPA_SolvencyII_XBRL_Taxonomy_2.1.0/eiopa.europa.eu/eu/xbrl/s2c/dict/dom/typ.xsd";
			var domains = Parsing.ParseNames(expFile);
			var typDomains = Parsing.ParseNames(typFile);
			typDomains.ToList().ForEach(x => domains.Add(x.Key, x.Value));


			var table = Parsing.ParseTable(Directory, "s.26.07.01.04");
			var cubes = Parsing.ParseHypercubes(Directory, "s.26.07.01.04", metrics, dimensions, domains);
			Helper.DumpTable(table);
			Helper.DumpHypercubes(cubes);

		}
	}
}