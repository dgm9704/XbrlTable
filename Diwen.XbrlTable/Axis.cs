namespace Diwen.XbrlTable
{
    public struct Axis
    {
        public int Order { get; }
        public Direction Direction { get; }
        public bool IsOpen { get; }
        public OrdinateCollection Ordinates { get; }

        public static Axis DefaultYAxis
            => new Axis(0, Direction.Y, false, new OrdinateCollection { new Ordinate("999", "0", new Signature()) });

        public Axis(int order, Direction direction, bool open, OrdinateCollection ordinates)
        {
            Order = order;
            Direction = direction;
            IsOpen = open;
            Ordinates = ordinates;
        }
    }
}
