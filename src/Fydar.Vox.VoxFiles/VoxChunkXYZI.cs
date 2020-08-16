namespace Fydar.Vox.VoxFiles
{
	public struct VoxChunkXYZI : IVoxChunk
	{
		public VoxStructureVoxelArray Voxels;

		public void Initialise(VoxDocument document, ref int offset)
		{
			Voxels = document.ReadStructure<VoxStructureVoxelArray>(ref offset);
		}
	}
}
