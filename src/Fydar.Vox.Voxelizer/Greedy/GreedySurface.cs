using System.Collections.Generic;

namespace Fydar.Vox.Voxelizer
{
	public struct GreedySurface
	{
		public SurfaceDescription Description;
		public GreedySurfaceFace[] Faces;

		public override string ToString()
		{
			return $"({Faces?.Length} faces with {Description})";
		}

		public IEnumerable<GroupedSurfaceTransformedFace> TransformedFaces
		{
			get
			{
				for (int i = 0; i < Faces.Length; i++)
				{
					var face = Faces[i];

					sbyte depth = (sbyte)Description.Depth;

					Vector3SByte v1;
					Vector3SByte v2;
					Vector3SByte v3;
					Vector3SByte v4;

					if (Description.Normal == VoxelNormal.Forward)
					{
						v1 = new Vector3SByte(face.Position.x, face.Position.y, depth + 1);
						v2 = new Vector3SByte(face.Position.x + 1, face.Position.y, depth + 1);
						v3 = new Vector3SByte(face.Position.x + 1, face.Position.y + 1, depth + 1);
						v4 = new Vector3SByte(face.Position.x, face.Position.y + 1, depth + 1);
					}
					else if (Description.Normal == VoxelNormal.Back)
					{
						v1 = new Vector3SByte(face.Position.x, face.Position.y, depth);
						v2 = new Vector3SByte(face.Position.x - 1, face.Position.y, depth);
						v3 = new Vector3SByte(face.Position.x - 1, face.Position.y - 1, depth);
						v4 = new Vector3SByte(face.Position.x, face.Position.y - 1, depth);
					}
					else if (Description.Normal == VoxelNormal.Left)
					{
						v1 = new Vector3SByte(face.Position.x, face.Position.y, depth);
						v2 = new Vector3SByte(face.Position.x - 1, face.Position.y, depth);
						v3 = new Vector3SByte(face.Position.x - 1, face.Position.y + 1, depth);
						v4 = new Vector3SByte(face.Position.x, face.Position.y + 1, depth);
					}
					else if (Description.Normal == VoxelNormal.Right)
					{
						v1 = new Vector3SByte(face.Position.x, face.Position.y, depth - 1);
						v2 = new Vector3SByte(face.Position.x - 1, face.Position.y, depth - 1);
						v3 = new Vector3SByte(face.Position.x - 1, face.Position.y - 1, depth - 1);
						v4 = new Vector3SByte(face.Position.x, face.Position.y - 1, depth - 1);
					}
					else if (Description.Normal == VoxelNormal.Up)
					{
						v1 = new Vector3SByte(face.Position.x + 1, face.Position.y, depth);
						v2 = new Vector3SByte(face.Position.x + 1, face.Position.y, depth + 1);
						v3 = new Vector3SByte(face.Position.x + 1, face.Position.y + 1, depth + 1);
						v4 = new Vector3SByte(face.Position.x + 1, face.Position.y + 1, depth);
					}
					else // if (Description.Normal == Vector3SByte.Down)
					{
						v1 = new Vector3SByte(face.Position.x, face.Position.y, depth);
						v2 = new Vector3SByte(face.Position.x, face.Position.y, depth - 1);
						v3 = new Vector3SByte(face.Position.x, face.Position.y + 1, depth - 1);
						v4 = new Vector3SByte(face.Position.x, face.Position.y + 1, depth);
					}

					yield return new GroupedSurfaceTransformedFace()
					{
						BottomLeft = VoxelNormal.RotateAroundOrigin(v1, Description.Normal),
						TopLeft = VoxelNormal.RotateAroundOrigin(v2, Description.Normal),
						TopRight = VoxelNormal.RotateAroundOrigin(v3, Description.Normal),
						BottomRight = VoxelNormal.RotateAroundOrigin(v4, Description.Normal)
					};
				}
			}
		}
	}
}
