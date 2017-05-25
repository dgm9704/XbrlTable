namespace XbrlTable
{
	public struct Axis
	{
		public int Order { get; }
		public Direction Direction { get; }
		public OrdinateCollection Ordinates { get; }

		public Axis(int order, Direction direction)
		{
			Order = order;
			Direction = direction;
			Ordinates = new OrdinateCollection();
		}
	}
}
