namespace XbrlTable
{
	public struct Table
	{
		public string Id { get; }
		public string Code { get; }

		public AxisCollection Axes { get; }

		public Table(string id, string code)
		{
			Id = id;
			Code = code;
			Axes = new AxisCollection();
		}
	}
}
