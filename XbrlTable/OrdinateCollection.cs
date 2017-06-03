namespace XbrlTable
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;

	public class OrdinateCollection : KeyedCollection<string, Ordinate>
	{
		protected override string GetKeyForItem(Ordinate item) => item.Code;

		internal void AddRange(IEnumerable<Ordinate> values) => values.ToList().ForEach(v => Add(v));
	}
}