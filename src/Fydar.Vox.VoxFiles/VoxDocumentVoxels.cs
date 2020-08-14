namespace Fydar.Vox.VoxFiles
{
	public struct VoxDocumentVoxels
	{
		public VoxDocumentVoxel[] Voxels { get; set; }

		public override string ToString()
		{
			return $"Count = {Voxels.Length}";
		}
	}
}
