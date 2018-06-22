namespace Diwen.XbrlTable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class AxisCollection : Collection<Axis> //KeyedCollection<Direction, Axis>
    {
        public AxisCollection(IEnumerable<Axis> axes)
        {
            axes.ToList().ForEach(this.Add);
        }
    }
}
