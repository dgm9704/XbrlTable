namespace Diwen.XbrlTable
{
	public struct Ordinate
	{
		public string Code { get; }
		public string Path { get; }
		public Signature Signature { get; }

		public Ordinate(string code, string path, Signature signature)
		{
			Code = code;
			Path = path;
			Signature = signature;
		}
	}
}
