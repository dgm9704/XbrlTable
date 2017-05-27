namespace XbrlTable
{
	using System.Collections.Generic;

	public class Signature : Dictionary<string, string>
	{
		public Signature()
		{

		}

		public Signature(Signature values)
		{
			foreach (var item in values)
			{
				this.Add(item.Key, item.Value);
			}
		}

		public override string ToString()
		{
			return this.Values.Join(" ");
		}
	}
}