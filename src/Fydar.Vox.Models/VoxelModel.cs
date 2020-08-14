using Fydar.Vox.VoxFiles.Hierarchy;

namespace Fydar.Vox.VoxFiles
{
	public class VoxelModel
	{
		public VoxelScene VoxelScene { get; }
		public VoxelShape Shape { get; internal set; }

		public int Width { get; set; }
		public int Depth { get; set; }
		public int Height { get; set; }

		public VoxelModelVoxel?[,,] Voxels { get; set; }

		public VoxelColourPallette VoxelColourPallette => VoxelScene.Pallette;

		public VoxelModel(VoxelScene voxelScene, VoxDocumentDiamentions diamentions, VoxDocumentVoxels voxDocumentVoxels)
		{
			VoxelScene = voxelScene;
			Width = diamentions.X;
			Depth = diamentions.Y;
			Height = diamentions.Z;

			Voxels = new VoxelModelVoxel?[Width, Depth, Height];

			foreach (var voxel in voxDocumentVoxels.Voxels)
			{
				Voxels[voxel.X, voxel.Y, voxel.Z] = new VoxelModelVoxel()
				{
					Index = voxel.Index
				};
			}
		}

		public override string ToString()
		{
			return $"({Width}, {Depth}, {Height})";
		}

		public VoxelModelVoxel? TryGetVoxel(int x, int y, int z)
		{
			if (x < 0 || y < 0 || z < 0
				|| x >= Voxels.GetLength(0)
				|| y >= Voxels.GetLength(1)
				|| z >= Voxels.GetLength(2))
			{
				return null;
			}

			return Voxels[x, y, z];
		}
	}
}
