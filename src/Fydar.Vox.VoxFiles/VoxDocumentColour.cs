namespace Fydar.Vox.VoxFiles
{
	public struct VoxDocumentColour
	{
		public byte R { get; set; }
		public byte G { get; set; }
		public byte B { get; set; }
		public byte A { get; set; }

		public VoxDocumentColour(uint rawData)
		{
			R = (byte)(rawData & 0xff);
			G = (byte)((rawData >> 8) & 0xff);
			B = (byte)((rawData >> 16) & 0xff);
			A = (byte)(rawData >> 24);
		}

		public override string ToString()
		{
			return $"({R}, {G}, {B}, {A})";
		}
	}
}
