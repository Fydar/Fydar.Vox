namespace Fydar.Vox.VoxFiles
{
	public struct VoxelTranslation
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }

		public VoxelTranslation(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static VoxelTranslation operator +(VoxelTranslation rhs, VoxelTranslation lhs)
		{
			return new VoxelTranslation(rhs.X + lhs.X, rhs.Y + lhs.Y, rhs.Z + lhs.Z);
		}

		public static VoxelTranslation operator -(VoxelTranslation rhs, VoxelTranslation lhs)
		{
			return new VoxelTranslation(rhs.X - lhs.X, rhs.Y - lhs.Y, rhs.Z - lhs.Z);
		}
	}
}
