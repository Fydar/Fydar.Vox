namespace Fydar.Vox.VoxFiles.Hierarchy
{
	public class VoxDocumentTransformNodeChunk : VoxDocumentNodeChunk
	{
		public int ChildNodeId { get; set; }
		public int ReservedId { get; set; }
		public int LayerId { get; set; }
		public int NumberOfFrames { get; set; }
	}
}
