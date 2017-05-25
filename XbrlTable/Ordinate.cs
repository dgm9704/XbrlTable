namespace XbrlTable
{
	public struct Ordinate
	{
		public string Id { get; }
		public string Code { get; }
		public int Order { get; }
		public Ordinate(string id, string code, int order)
		{
			Id = id;
			Code = code;
			Order = order;
		}
	}
}
