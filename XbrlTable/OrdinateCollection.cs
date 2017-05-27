namespace XbrlTable
{
	using System.Collections.ObjectModel;

	public class OrdinateCollection : KeyedCollection<string, Ordinate>
	{
		protected override string GetKeyForItem(Ordinate item)
		{
			return item.Code;
		}
	}
}