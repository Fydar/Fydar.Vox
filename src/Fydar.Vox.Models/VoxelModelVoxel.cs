namespace Fydar.Vox.VoxFiles
{
	public struct VoxelModelVoxel
	{
		public byte Index { get; set; }

		public override string ToString()
		{
			return $"({Index})";
		}
	}
}
