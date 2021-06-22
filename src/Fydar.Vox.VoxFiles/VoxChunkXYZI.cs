namespace Fydar.Vox.VoxFiles
{
	/// <summary>
	/// Represents chunks of data for .vox file voxel data.
	/// </summary>
	public struct VoxChunkXYZI : IVoxChunk
	{
		public VoxStructureVoxelArray Voxels;

		public void Initialise(VoxDocument document, ref int offset)
		{
			Voxels = document.ReadStructure<VoxStructureVoxelArray>(ref offset);
		}
	}
}
