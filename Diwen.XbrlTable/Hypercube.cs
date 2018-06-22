namespace Diwen.XbrlTable
{
	using System.Collections.Generic;
	using System.Linq;

	public struct Hypercube
	{
		public List<string> Metrics { get; }
		public List<Dimension> Dimensions { get; }

		public Hypercube(List<string> metrics, List<Dimension> dimensions)
		{
			Metrics = metrics;
			Dimensions = dimensions;
		}

		public override string ToString()
		{
			return $"[{Metrics.Join(",")}];{Dimensions.Select(d => d.ToString()).Join(";")}]";
		}
	}
}
