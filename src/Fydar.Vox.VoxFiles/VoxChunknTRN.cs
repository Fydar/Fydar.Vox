namespace Fydar.Vox.VoxFiles
{
	/// <summary>
	/// Represents chunks of data for .vox file transform nodes.
	/// </summary>
	public struct VoxChunknTRN : IVoxChunk
	{
		public int NodeId { get; set; }
		public VoxStructureDictionary NodeAttributes { get; set; }
		public int ChildNodeId { get; set; }
		public int ReservedId { get; set; }
		public int LayerId { get; set; }
		public int NumberOfFrames { get; set; }

		public void Initialise(VoxDocument document, ref int offset)
		{
			NodeId = document.ReadInt32(ref offset);
			NodeAttributes = document.ReadStructure<VoxStructureDictionary>(ref offset);
			ChildNodeId = document.ReadInt32(ref offset);
			ReservedId = document.ReadInt32(ref offset);
			LayerId = document.ReadInt32(ref offset);
			NumberOfFrames = document.ReadInt32(ref offset);
		}
	}
}
