using System.Collections.Generic;
using System.Linq;

namespace XbrlTable
{
	public struct Hypercube
	{
		public List<string> Metrics { get; }
		public List<Context> Contexts { get; }

		public Hypercube(List<string> metrics, List<Context> contexts)
		{
			Metrics = metrics;
			Contexts = contexts;
		}

		public override string ToString()
		{
			return $"[Hypercube: Metrics={Metrics.Join(",")}, Contexts={Contexts.Select(c => c.ToString()).Join(";")}]";
		}
	}
}
