namespace XbrlTable
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class Helper
	{
		public static void DumpAxes(Table table)
		{
			Console.WriteLine(table.Code);

			foreach (var axis in table.Axes.OrderBy(a => a.Order))
			{
				Console.Write($"{axis.Direction}\t");
				Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => o.Code).Join("\t"));
			}
		}

		public static void DumpTable(Table table)
		{

			foreach (var axis in table.Axes.Where(a => a.IsOpen))
			{
				Console.Write($"{axis.Direction}\t");
				Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => $"{o.Code} {o.Member.Split(':').Last()}").Join("\t")); ;
			}

			Console.Write("Y \\ X\t");

			var xAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.X);
			var xOrdinates = xAxis.Ordinates.OrderBy(o => o.Path);
			Console.WriteLine(xOrdinates.Select(o => o.Code).Join("\t"));

			var yAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.Y);

			var yOrdinates = (yAxis.Ordinates ?? new OrdinateCollection()).OrderBy(o => o.Path).ToList();

			if (!yOrdinates.Any())
			{
				yOrdinates = new List<Ordinate>() { new Ordinate("", "999", "0", "") };

			}
			foreach (var y in yOrdinates)
			{
				Console.Write($"{y.Code}\t");
				Console.WriteLine(xOrdinates.Select(x => $"{x.Member.Split(':').Last()}{y.Member.Split(':').Last()}").Join("\t"));
			}

			//Console.WriteLine(table.Axes.First(a => a.Direction == Direction.X).Ordinates.OrderBy(o => o.Path).Select(o => o.Member).Join("\t"));

			//Console.WriteLine(table.Axes.First(a => a.Direction == Direction.Y).Ordinates.OrderBy(o => o.Path).Select(o => o.Code).Join("\n"));
		}

		public static string Join(this IEnumerable<string> values, string separator) => string.Join(separator, values);

	}
}