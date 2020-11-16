using System.Collections.Generic;

namespace Fydar.Vox.Meshing
{
	public struct GroupedSurface : ISurface
	{
		public SurfaceDescription Description { get; set; }
		public GroupedSurfaceFace[] Faces;

		public IEnumerable<GroupedSurfaceTransformedFace> TransformedFaces
		{
			get
			{
				for (int i = 0; i < Faces.Length; i++)
				{
					var face = Faces[i];

					sbyte depth = (sbyte)Description.Depth;

					Vector3SByte tl;
					Vector3SByte tr;
					Vector3SByte bl;
					Vector3SByte br;

					if (Description.Normal == VoxelNormal.Forward)
					{
						tl = new Vector3SByte(face.Position.x + 1, face.Position.y + 1, depth);
						tr = new Vector3SByte(face.Position.x, face.Position.y + 1, depth);
						bl = new Vector3SByte(face.Position.x + 1, face.Position.y, depth);
						br = new Vector3SByte(face.Position.x, face.Position.y, depth);
					}
					else if (Description.Normal == VoxelNormal.Back)
					{
						tl = new Vector3SByte(face.Position.x, face.Position.y + 1, -depth);
						tr = new Vector3SByte(face.Position.x + 1, face.Position.y + 1, -depth);
						bl = new Vector3SByte(face.Position.x, face.Position.y, -depth);
						br = new Vector3SByte(face.Position.x + 1, face.Position.y, -depth);
					}
					else if (Description.Normal == VoxelNormal.Left)
					{
						tl = new Vector3SByte(-depth, face.Position.y + 1, face.Position.x + 1);
						tr = new Vector3SByte(-depth, face.Position.y + 1, face.Position.x);
						bl = new Vector3SByte(-depth, face.Position.y, face.Position.x + 1);
						br = new Vector3SByte(-depth, face.Position.y, face.Position.x);
					}
					else if (Description.Normal == VoxelNormal.Right)
					{
						tl = new Vector3SByte(depth, face.Position.y + 1, face.Position.x);
						tr = new Vector3SByte(depth, face.Position.y + 1, face.Position.x + 1);
						bl = new Vector3SByte(depth, face.Position.y, face.Position.x);
						br = new Vector3SByte(depth, face.Position.y, face.Position.x + 1);
					}
					else if (Description.Normal == VoxelNormal.Up)
					{
						tl = new Vector3SByte(face.Position.x, depth, face.Position.y + 1);
						tr = new Vector3SByte(face.Position.x + 1, depth, face.Position.y + 1);
						bl = new Vector3SByte(face.Position.x, depth, face.Position.y);
						br = new Vector3SByte(face.Position.x + 1, depth, face.Position.y);
					}
					else // if (Description.Normal == Vector3SByte.Down)
					{
						tl = new Vector3SByte(face.Position.x, -depth, face.Position.y);
						tr = new Vector3SByte(face.Position.x + 1, -depth, face.Position.y);
						bl = new Vector3SByte(face.Position.x, -depth, face.Position.y + 1);
						br = new Vector3SByte(face.Position.x + 1, -depth, face.Position.y + 1);
					}

					yield return new GroupedSurfaceTransformedFace()
					{
						TopLeft = tl,
						TopRight = tr,
						BottomLeft = bl,
						BottomRight = br
					};
				}
			}
		}

		public override string ToString()
		{
			return $"({Faces?.Length} faces with {Description})";
		}
	}
}
