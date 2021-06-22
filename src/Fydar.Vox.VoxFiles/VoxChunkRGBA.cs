namespace Fydar.Vox.VoxFiles
{
	/// <summary>
	/// Represents chunks of data for .vox file colour palletes.
	/// </summary>
	public struct VoxChunkRGBA : IVoxChunk
	{
		public VoxStructureColorPaletteArray Colours;

		public void Initialise(VoxDocument document, ref int offset)
		{
			Colours = document.ReadStructure<VoxStructureColorPaletteArray>(ref offset);
		}
	}
}
