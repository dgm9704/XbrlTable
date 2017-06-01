namespace XbrlTable
{
	using System.Collections.Generic;
	using System.Linq;

	public class Signature : SortedDictionary<string, string>
	{
		public Signature()
		{
			this["met"] = string.Empty;
		}

		public Signature(Signature values)
		{
			foreach (var item in values)
			{
				Add(item.Key, item.Value);
			}
		}

		public override string ToString()
		{
			return this.
					   Where(s => !string.IsNullOrEmpty(s.Value)).
					   Select(s => s.Key == "met" ? s.Value.Split(':').Last() : $"{s.Key.Split(':').Last()}/{s.Value}").
					   Join(", ");
		}
	}
}