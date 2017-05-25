namespace XbrlTable
{
	public struct Table
	{
		public string Code { get; }
		public AxisCollection Axes { get; }

		public Table(string code)
		{
			Code = code;
			Axes = new AxisCollection();
		}
	}
}
