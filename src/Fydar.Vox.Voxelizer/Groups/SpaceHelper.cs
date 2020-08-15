namespace Fydar.Vox.Voxelizer
{
	public static class SpaceHelper
	{
		public static void RotatePosition(Vector3SByte position, VoxelNormal normal, out Vector2SByte planePosition, out int depth)
		{
			var rotated = VoxelNormal.RotateAroundOrigin(position, normal);
			planePosition = new Vector2SByte(rotated.x, rotated.y);
			depth = rotated.z;
		}
	}
}
