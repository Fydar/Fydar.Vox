namespace Fydar.Vox.VoxFiles
{
	/// <summary>
	/// Represents chunks of data for .vox file layer configuration.
	/// </summary>
	public struct VoxChunkLAYR : IVoxChunk
	{
		public int LayerId;
		public VoxStructureDictionary VoxDictionary;
		public int ReservedId;

		public void Initialise(VoxDocument document, ref int offset)
		{
			LayerId = document.ReadInt32(ref offset);
			VoxDictionary = document.ReadStructure<VoxStructureDictionary>(ref offset);
			ReservedId = document.ReadInt32(ref offset);
		}
	}
}
