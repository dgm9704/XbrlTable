namespace Diwen.XbrlTable
{
	using System.Collections.Generic;

	public struct Dimension
	{
		public string DimensionCode { get; }
		public string DomainCode { get; }
		public HashSet<string> Members { get; }

		public Dimension(string dimensionCode, string domainCode, IEnumerable<string> members)
		{
			DimensionCode = dimensionCode;
			DomainCode = domainCode;
			Members = new HashSet<string>(members);
		}

		public override string ToString()
		{
			return $"[{DimensionCode}, {DomainCode}, {Members.Join(",")}]";
		}
	}
}
