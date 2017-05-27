namespace XbrlTable
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class Helper
	{
		public static void DumpTable(Table table)
		{
			Console.WriteLine(table.Code);

			foreach (var axis in table.Axes.Where(a => a.Direction == Direction.Z).Where(a => !a.IsOpen))
			{
				Console.Write($"{axis.Direction}\t");
				Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => o.Code).Join("\t"));
			}

			foreach (var axis in table.Axes.Where(a => a.IsOpen))
			{
				Console.Write($"{axis.Direction}\t");
				Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => $"{o.Code} {o.Concept.Split(':').Last()}").Join("\t")); ;
			}

			Console.Write("Y \\ X\t");

			var xAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.X);
			var xOrdinates = xAxis.Ordinates.OrderBy(o => o.Path);
			Console.WriteLine(xOrdinates.Select(o => o.Code).Join("\t"));

			var yAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.Y);

			var yOrdinates = (yAxis.Ordinates ?? new OrdinateCollection()).OrderBy(o => o.Path).ToList();

			if (!yOrdinates.Any())
			{
				yOrdinates = new List<Ordinate>() { new Ordinate("999", "0", "", new Signature()) };

			}

			foreach (var y in yOrdinates)
			{
				Console.Write($"{y.Code}\t");
				Console.Write(xOrdinates.Select(x => $"{x.Concept.Split(':').Last()}{y.Concept.Split(':').Last()}").Join("\t"));
				Console.WriteLine($"\t{y.Signature}");
			}

			var max = xOrdinates.Select(o => o.Signature.Count).Max();
			for (int i = 0; i < max; i++)
			{
				Console.Write("\t");
				foreach (var x in xOrdinates)
				{
					if (x.Signature.Count > i)
					{
						Console.Write(x.Signature.Values.ToList()[i]);
					}
					Console.Write("\t");
				}
				Console.WriteLine();
			}
		}

		public static void DumpAxesAndMetrics(Table table)
		{
			Console.WriteLine(table.Code);

			foreach (var axis in table.Axes.Where(a => a.Direction == Direction.Z).Where(a => !a.IsOpen))
			{
				Console.Write($"{axis.Direction}\t");
				Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => o.Code).Join("\t"));
			}

			foreach (var axis in table.Axes.Where(a => a.IsOpen))
			{
				Console.Write($"{axis.Direction}\t");
				Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => $"{o.Code} {o.Concept.Split(':').Last()}").Join("\t")); ;
			}

			Console.Write("Y \\ X\t");

			var xAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.X);
			var xOrdinates = xAxis.Ordinates.OrderBy(o => o.Path);
			Console.WriteLine(xOrdinates.Select(o => o.Code).Join("\t"));

			var yAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.Y);

			var yOrdinates = (yAxis.Ordinates ?? new OrdinateCollection()).OrderBy(o => o.Path).ToList();

			if (!yOrdinates.Any())
			{
				yOrdinates = new List<Ordinate>() { new Ordinate("999", "0", "", new Signature()) };
			}

			foreach (var y in yOrdinates)
			{
				Console.Write($"{y.Code}\t");
				Console.WriteLine(xOrdinates.Select(x => $"{x.Concept.Split(':').Last()}{y.Concept.Split(':').Last()}").Join("\t"));
			}
		}

		public static void DumpAxes(Table table)
		{
			Console.WriteLine(table.Code);

			foreach (var axis in table.Axes.OrderBy(a => a.Order))
			{
				Console.Write($"{axis.Direction}\t");
				Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => o.Code).Join("\t"));
			}
		}

		public static string Join(this IEnumerable<string> values, string separator) => string.Join(separator, values);
	}
}