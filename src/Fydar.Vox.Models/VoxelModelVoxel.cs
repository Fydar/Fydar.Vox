namespace Fydar.Vox.VoxFiles
{
	public struct VoxelModelVoxel
	{
		public static readonly VoxelModelVoxel Empty;

		public byte Index { get; set; }

		public bool IsEmpty => Index == 0;

		public override string ToString()
		{
			return $"({Index})";
		}
	}
}
