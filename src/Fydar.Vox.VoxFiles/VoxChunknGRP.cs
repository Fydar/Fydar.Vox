namespace Fydar.Vox.VoxFiles
{
	/// <summary>
	/// Represents chunks of data for .vox file group nodes.
	/// </summary>
	public struct VoxChunknGRP : IVoxChunk
	{
		public int NodeId { get; set; }
		public VoxStructureDictionary NodeAttributes { get; set; }
		public VoxStructureIntArray ChildrenIds { get; set; }

		public void Initialise(VoxDocument document, ref int offset)
		{
			NodeId = document.ReadInt32(ref offset);
			NodeAttributes = document.ReadStructure<VoxStructureDictionary>(ref offset);
			ChildrenIds = document.ReadStructure<VoxStructureIntArray>(ref offset);
		}
	}
}
