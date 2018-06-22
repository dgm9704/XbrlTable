namespace Diwen.XbrlTable
{
    using System.Collections.Generic;

    public struct Table
    {
        public string Id { get; }
        public string Code { get; }
        public AxisCollection Axes { get; }

        public Table(string id, string code, IEnumerable<Axis> axes)
        {
            Id = id;
            Code = code;
            Axes = new AxisCollection(axes);
        }
    }
}
