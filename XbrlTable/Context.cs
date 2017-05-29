namespace XbrlTable
{
	using System.Collections.Generic;

	public struct Context
	{
		public string Dimension { get; }
		public string Domain { get; }
		public HashSet<string> Members { get; }

		public Context(string dimension, string domain, IEnumerable<string> members)
		{
			Dimension = dimension;
			Domain = domain;
			Members = new HashSet<string>(members);
		}

		public override string ToString()
		{
			return $"[Context: Dimension={Dimension}, Domain={Domain}, Members={Members.Join(",")}]";
		}
	}
}
