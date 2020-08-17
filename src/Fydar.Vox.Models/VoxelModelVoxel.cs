namespace Fydar.Vox.VoxFiles
{
	public struct VoxelModelVoxel
	{
		public static readonly VoxelModelVoxel Empty;

		public byte Index { get; set; }

		public bool IsEmpty
		{
			get
			{
				return Index == 0;
			}
		}

		public override string ToString()
		{
			return $"({Index})";
		}
	}
}
