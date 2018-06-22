namespace Diwen.XbrlTable
{
	public struct Label
	{
		public string Id { get; }
		public string Type { get; }
		public string Language { get; }
		public string Value { get; }

		public Label(string id, string type, string language, string value)
		{
			Id = id;
			Type = type;
			Language = language;
			Value = value;
		}
	}
}