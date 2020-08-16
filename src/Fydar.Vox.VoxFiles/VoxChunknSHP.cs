﻿namespace Fydar.Vox.VoxFiles
{
	public struct VoxChunknSHP : IVoxChunk
	{
		public int NodeId { get; set; }
		public VoxStructureDictionary NodeAttributes { get; set; }
		public VoxStructureShapeModelArray Models { get; set; }

		public void Initialise(VoxDocument document, ref int offset)
		{
			NodeId = document.ReadInt32(ref offset);
			NodeAttributes = document.ReadStructure<VoxStructureDictionary>(ref offset);
			Models = document.ReadStructure<VoxStructureShapeModelArray>(ref offset);
		}
	}
}
