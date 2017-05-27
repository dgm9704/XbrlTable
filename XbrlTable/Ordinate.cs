namespace XbrlTable
{
	using System.Collections.Generic;

	public struct Ordinate
	{
		public string Id { get; }
		public string Code { get; }
		public string Path { get; }
		public string Member { get; }

		public Ordinate(string id, string code, string path, string member)
		{
			Id = id;
			Code = code;
			Path = path;
			Member = member;
		}
	}
}
