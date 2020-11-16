using BenchmarkDotNet.Running;

namespace Fydar.Vox.Meshing.Benchmarks
{
	public class Program
	{
		public static int Main(string[] args)
		{
			var summary = BenchmarkRunner.Run<MeshingBenchmarks>();

			return summary.HasCriticalValidationErrors ? 1 : 0;
		}
	}
}
