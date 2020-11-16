using Fydar.Vox.VoxFiles.Hierarchy;
using System.Collections.Generic;

namespace Fydar.Vox.VoxFiles
{
	public abstract class VoxelModel
	{
		public List<VoxelShape> Parents { get; }
		public VoxelScene VoxelScene { get; }
		public VoxelColourPallette VoxelColourPallette => VoxelScene.Pallette;

		public abstract int Width { get; }
		public abstract int Depth { get; }
		public abstract int Height { get; }

		public abstract VoxelModelVoxel GetWithRangeCheck(int x, int y, int z);

		public VoxelModel(VoxelScene voxelScene)
		{
			Parents = new List<VoxelShape>();

			VoxelScene = voxelScene;
		}
	}
}
