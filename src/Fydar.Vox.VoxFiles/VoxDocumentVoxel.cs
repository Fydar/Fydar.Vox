namespace Fydar.Vox.VoxFiles
{
	public struct VoxDocumentVoxel
	{
		public byte X { get; set; }
		public byte Y { get; set; }
		public byte Z { get; set; }
		public byte Index { get; set; }

		public VoxDocumentVoxel(byte x, byte y, byte z, byte index)
		{
			X = x;
			Y = y;
			Z = z;
			Index = index;
		}

		public override string ToString()
		{
			return $"({X}, {Y}, {Z}, {Index})";
		}
	}
}
