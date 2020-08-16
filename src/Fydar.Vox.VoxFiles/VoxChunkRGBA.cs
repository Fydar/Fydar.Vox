namespace Fydar.Vox.VoxFiles
{
	public struct VoxChunkRGBA : IVoxChunk
	{
		public VoxStructureColorPaletteArray Colours;

		public void Initialise(VoxDocument document, ref int offset)
		{
			Colours = document.ReadStructure<VoxStructureColorPaletteArray>(ref offset);
		}
	}
}
