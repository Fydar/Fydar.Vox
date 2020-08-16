namespace Fydar.Vox.VoxFiles
{
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
