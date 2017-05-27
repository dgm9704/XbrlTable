namespace XbrlTable
{
	public struct Ordinate
	{
		public string Id { get; }
		public string Code { get; }
		public string Path { get; }
		public Ordinate(string id, string code, string path)
		{
			Id = id;
			Code = code;
			Path = path;
		}
	}
}
