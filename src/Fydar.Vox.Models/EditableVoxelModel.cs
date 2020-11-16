namespace Fydar.Vox.VoxFiles
{
	public class EditableVoxelModel : VoxelModel
	{
		public override int Width { get; }
		public override int Depth { get; }
		public override int Height { get; }

		public VoxelModelVoxel[,,] Voxels { get; set; }

		public override VoxelModelVoxel GetWithRangeCheck(int x, int y, int z)
		{
			return x < 0 || y < 0 || z < 0 || x >= Width || y >= Depth || z >= Height
				? VoxelModelVoxel.Empty
				: Voxels[x, y, z];
		}

		public EditableVoxelModel(VoxelScene voxelScene, int width, int height, int depth)
			: base(voxelScene)
		{
			Width = width;
			Height = height;
			Depth = depth;

			Voxels = new VoxelModelVoxel[Width, Depth, Height];
		}

		public EditableVoxelModel(VoxelScene voxelScene, VoxDocumentDiamentions diamentions, VoxStructureVoxelArray voxDocumentVoxels)
			: base(voxelScene)
		{
			Width = diamentions.X;
			Depth = diamentions.Y;
			Height = diamentions.Z;

			Voxels = new VoxelModelVoxel[Width, Depth, Height];

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
	}
}
