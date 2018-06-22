namespace Diwen.XbrlTable
{
	public struct Address
	{
		public string Table { get; }
		public string Row { get; }
		public string Column { get; }
		public string Sheet { get; }

		public Address(string table, string row, string column, string sheet)
		{
			Table = table;
			Row = row;
			Column = column;
			Sheet = sheet;
		}

		public override string ToString()
		{
			var result = $"{Table}_{Row}_{Column}";
			if (!string.IsNullOrEmpty(Sheet))
			{
				result = $"{result}/{Sheet}";
			}
			return result.Replace(" ", "");
		}
	}
}