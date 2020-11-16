using Fydar.Vox.Meshing;
using Fydar.Vox.Meshing.Greedy;
using Fydar.Vox.VoxFiles;

namespace Fydar.Vox.WebDemo
{
	public struct DemoModel
	{
		public VoxelModel Model;
		public GroupedMesh Grouped;
		public GreedyMesh Greedy;

		public DemoModel(DataVoxelizerDriver voxelDriver, VoxelModel model = null)
		{
			var voxelizer = new GroupedMesher(voxelDriver);
			Grouped = voxelizer.Voxelize();
			Greedy = new GreedyMesher().Optimize(Grouped);

			Model = model;
		}
	}
}
