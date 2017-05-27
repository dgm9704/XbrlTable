namespace XbrlTable
{
	public struct Axis
	{
		public int Order { get; }
		public Direction Direction { get; }
		public bool IsOpen { get; }
		public OrdinateCollection Ordinates { get; }

		public Axis(int order, Direction direction, bool open, OrdinateCollection ordinates)
		{
			Order = order;
			Direction = direction;
			IsOpen = open;
			Ordinates = ordinates;
		}
	}
}
