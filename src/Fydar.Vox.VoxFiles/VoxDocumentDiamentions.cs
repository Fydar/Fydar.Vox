namespace Fydar.Vox.VoxFiles
{
	public struct VoxDocumentDiamentions
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }

		public override string ToString()
		{
			return $"({X}, {Y}, {Z})";
		}
	}
}
