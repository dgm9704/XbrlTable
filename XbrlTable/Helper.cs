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
			if (table.Axes.Any(a => a.Direction == Direction.Z))
			{
				foreach (var axis in table.Axes.Where(a => a.IsOpen))
				{
					Console.Write($"{axis.Direction}\t");
					Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => $"{o.Code} {o.Member}").Join("\t")); ;
				}

				Console.Write("Y \\ X\t");

				Console.WriteLine(table.Axes.First(a => a.Direction == Direction.X).Ordinates.OrderBy(o => o.Path).Select(o => o.Code).Join("\t"));

				Console.WriteLine(table.Axes.First(a => a.Direction == Direction.Y).Ordinates.OrderBy(o => o.Path).Select(o => o.Code).Join("\n"));
			}
		}

		public static string Join(this IEnumerable<string> values, string separator) => string.Join(separator, values);

	}
}