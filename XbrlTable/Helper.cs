﻿namespace XbrlTable
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class Helper
	{
		public static void DumpDatapoints(Table table)
		{
			var datapoints = new List<Tuple<string, string>>();
			var xAxis = table.Axes.Where(a => a.Direction == Direction.X).Single(a => !a.IsOpen);
			var yAxis = table.Axes.Where(a => a.Direction == Direction.Y).SingleOrDefault(a => !a.IsOpen);
			var tableSignature = table.
									  Axes.
									  Where(a => a.IsOpen).
									  SelectMany(a => a.Ordinates).
									  SelectMany(o => o.Signature);

			foreach (var y in yAxis.Ordinates)
			{
				foreach (var x in xAxis.Ordinates)
				{
					var address = $"{table.Code}_{y.Code}_{x.Code}";
					var signature = tableSignature.Concat(y.Signature).Concat(x.Signature).Where(i => !string.IsNullOrEmpty(i.Value)).Select(m => m.ToString()).Join(",");
					datapoints.Add(Tuple.Create(signature, address));
				}
			}

			Console.WriteLine(datapoints.Select(p => $"{p.Item1}->{p.Item2}").Join("\n"));
		}

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
				Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => $"{o.Code} {o.Signature}").Join("\t")); ;
			}

			Console.Write("Y \\ X\t");

			var xAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.X);
			var xOrdinates = xAxis.Ordinates.OrderBy(o => o.Path);
			Console.WriteLine(xOrdinates.Select(o => o.Code.PadRight(10)).Join("\t"));

			var yAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.Y);

			var yOrdinates = (yAxis.Ordinates ?? new OrdinateCollection()).OrderBy(o => o.Path).ToList();

			if (!yOrdinates.Any())
			{
				yOrdinates = new List<Ordinate>() { new Ordinate("999", "0", new Signature()) };

			}

			foreach (var y in yOrdinates)
			{
				Console.Write($"{y.Code}\t");
				Console.Write(xOrdinates.Select(x => ".".PadRight(10)).Join("\t"));
				Console.WriteLine($"{y.Signature}");
			}

			var max = xOrdinates.Select(o => o.Signature.Count).Max();
			for (int i = 0; i < max; i++)
			{
				Console.Write("\t");
				foreach (var x in xOrdinates)
				{
					var e = x.Signature.ElementAtOrDefault(i);
					if (!string.IsNullOrEmpty(e.Key))
					{
						if (e.Key == "met")
						{
							Console.Write($"{e.Value.Split(':').Last()}".PadRight(10));
						}
						else
						{
							Console.Write($"{e.Key.Split(':').Last()}/{e.Value}".PadRight(10));
						}
					}
					else
					{
						Console.Write(new string(' ', 10));
					}

					Console.Write("\t");
				}
				Console.WriteLine();
			}
		}

		public static void DumpHypercubes(IEnumerable<Hypercube> cubes)
		{
			foreach (var cube in cubes)
			{
				Console.WriteLine(cube);
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
				Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => $"{o.Code} {o.Signature["met"].Split(':').Last()}").Join("\t")); ;
			}

			Console.Write("Y \\ X\t");

			var xAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.X);
			var xOrdinates = xAxis.Ordinates.OrderBy(o => o.Path);
			Console.WriteLine(xOrdinates.Select(o => o.Code).Join("\t"));

			var yAxis = table.Axes.Where(a => !a.IsOpen).FirstOrDefault(a => a.Direction == Direction.Y);

			var yOrdinates = (yAxis.Ordinates ?? new OrdinateCollection()).OrderBy(o => o.Path).ToList();

			if (!yOrdinates.Any())
			{
				yOrdinates = new List<Ordinate>() { new Ordinate("999", "0", new Signature()) };
			}

			foreach (var y in yOrdinates)
			{
				Console.Write($"{y.Code}\t");
				Console.WriteLine(xOrdinates.Select(x => $"{x.Signature["met"].Split(':').Last()}{y.Signature["met"].Split(':').Last()}").Join("\t"));
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