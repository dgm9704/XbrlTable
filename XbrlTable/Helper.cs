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

			foreach (var axis in table.Axes.Where(a => a.Ordinates.Any()).OrderBy(a => a.Order))
			{
				Console.Write($"{axis.Direction}\t");
				Console.WriteLine(axis.Ordinates.OrderBy(o => o.Path).Select(o => o.Code).Join("\t"));
			}
		}

		public static string Join(this IEnumerable<string> values, string separator) => string.Join(separator, values);

	}
}