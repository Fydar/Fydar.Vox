namespace Fydar.Vox.VoxFiles
{
	public interface IVoxDocumentStructure
	{
		VoxDocument Document { get; set; }
		int StartIndex { get; set; }
		int Length { get; }
	}
}
