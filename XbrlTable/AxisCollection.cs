namespace XbrlTable
{
	using System.Collections.ObjectModel;

	public class AxisCollection : KeyedCollection<Direction, Axis>
	{
		protected override Direction GetKeyForItem(Axis item)
		{
			return item.Direction;
		}
	}
}
