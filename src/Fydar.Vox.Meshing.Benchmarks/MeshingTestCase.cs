using Fydar.Vox.VoxFiles;

namespace Fydar.Vox.Meshing.Benchmarks
{
	public class MeshingTestCase
	{
		public string Name;
		public VoxelModel Model;
		public GroupedMesh GroupedMesh;

		public MeshingTestCase(string name, VoxelModel model)
		{
			Name = name;
			Model = model;
			var dataDriver = new VoxelModelImporter(model);
			var groupedMesher = new GroupedMesher(dataDriver);

			GroupedMesh = groupedMesher.Voxelize();
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
