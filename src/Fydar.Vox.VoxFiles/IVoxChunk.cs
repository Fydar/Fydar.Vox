namespace Fydar.Vox.VoxFiles
{
	public interface IVoxChunk
	{
		void Initialise(VoxDocument document, ref int offset);
	}
}
