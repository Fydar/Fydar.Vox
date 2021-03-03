using Fydar.Vox.Meshing;
using Fydar.Vox.Meshing.Greedy;
using Fydar.Vox.VoxFiles;
using Fydar.Vox.WebDemo.Shared;
using System.Collections.Generic;

namespace Fydar.Vox.WebDemo
{
	public class DemoModel
	{
		public string Name;
		public VoxelModel Model;
		public GroupedMesh Grouped;
		public GreedyMesh Greedy;

		public int OutputWidth { get; set; }
		public int OutputHeight { get; set; }
		public string ImageSource { get; set; }
		public List<SurfaceSprite> Sprites { get; set; }

		public DemoModel(string name, DataVoxelizerDriver voxelDriver, VoxelModel model = null)
		{
			Name = name;
			var voxelizer = new GroupedMesher(voxelDriver);
			Grouped = voxelizer.Voxelize();
			Greedy = new GreedyMesher().Optimize(Grouped);

			Model = model;
		}
	}
}
