namespace XbrlTable
{
	public struct Ordinate
	{
		public string Code { get; }
		public string Path { get; }
		public string Concept { get; }
		public Signature Signature { get; }

		public Ordinate(string code, string path, string member, Signature signature)
		{
			Code = code;
			Path = path;
			Concept = member;
			Signature = signature;
		}
	}
}
