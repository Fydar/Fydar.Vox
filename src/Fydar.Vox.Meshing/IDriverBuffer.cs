namespace Fydar.Vox.Meshing
{
	public interface IDriverBuffer
	{
		public GroupedMesh Build();

		void AddVoxelFace(Vector3SByte position, VoxelNormal normal, Colour24 colour, FaceEdgeFlags edges);
	}
}
