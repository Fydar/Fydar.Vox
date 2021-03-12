using Fydar.Vox.Export.ToHtml;
using Fydar.Vox.Meshing;
using Fydar.Vox.Meshing.Greedy;
using Fydar.Vox.VoxFiles;

namespace Fydar.Vox.WebDemo
{
	public class DemoModel
	{
		public string Name;
		public VoxelModel Model;
		public GroupedMesh Grouped;
		public GreedyMesh Greedy;

		public LayeredModel LayeredModel;
		public string SpriteAtlasImage;

		public DemoModel(string name, DataVoxelizerDriver voxelDriver, VoxelModel model = null)
		{
			Name = name;
			var voxelizer = new GroupedMesher(voxelDriver);
			Grouped = voxelizer.Voxelize();
			Greedy = new GreedyMesher().Optimize(Grouped);

			Model = model;

			LayeredModel = LayeredModel.Create(model);
			SpriteAtlasImage = LayeredModel.RenderImageString();
		}
	}
}
