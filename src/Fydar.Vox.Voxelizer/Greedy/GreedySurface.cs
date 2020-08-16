using System.Collections.Generic;

namespace Fydar.Vox.Voxelizer
{
	public struct GreedySurface : ISurface
	{
		public SurfaceDescription Description { get; set; }
		public GreedySurfaceFace[] Faces;

		public IEnumerable<GroupedSurfaceTransformedFace> TransformedFaces
		{
			get
			{
				sbyte depth = (sbyte)Description.Depth;

				for (int i = 0; i < Faces.Length; i++)
				{
					var face = Faces[i];

					Vector3SByte tl;
					Vector3SByte tr;
					Vector3SByte bl;
					Vector3SByte br;

					if (Description.Normal == VoxelNormal.Forward)
					{
						tl = new Vector3SByte(face.Position.x + face.Scale.x, face.Position.y + face.Scale.y, depth);
						tr = new Vector3SByte(face.Position.x, face.Position.y + face.Scale.y, depth);
						bl = new Vector3SByte(face.Position.x + face.Scale.x, face.Position.y, depth);
						br = new Vector3SByte(face.Position.x, face.Position.y, depth);
					}
					else if (Description.Normal == VoxelNormal.Back)
					{
						tl = new Vector3SByte(face.Position.x, face.Position.y + face.Scale.y, -depth);
						tr = new Vector3SByte(face.Position.x + face.Scale.x, face.Position.y + face.Scale.y, -depth);
						bl = new Vector3SByte(face.Position.x, face.Position.y, -depth);
						br = new Vector3SByte(face.Position.x + face.Scale.x, face.Position.y, -depth);
					}
					else if (Description.Normal == VoxelNormal.Left)
					{
						tl = new Vector3SByte(-depth, face.Position.y + face.Scale.y, face.Position.x + face.Scale.x);
						tr = new Vector3SByte(-depth, face.Position.y + face.Scale.y, face.Position.x);
						bl = new Vector3SByte(-depth, face.Position.y, face.Position.x + face.Scale.x);
						br = new Vector3SByte(-depth, face.Position.y, face.Position.x);
					}
					else if (Description.Normal == VoxelNormal.Right)
					{
						tl = new Vector3SByte(depth, face.Position.y + face.Scale.y, face.Position.x);
						tr = new Vector3SByte(depth, face.Position.y + face.Scale.y, face.Position.x + face.Scale.x);
						bl = new Vector3SByte(depth, face.Position.y, face.Position.x);
						br = new Vector3SByte(depth, face.Position.y, face.Position.x + face.Scale.x);
					}
					else if (Description.Normal == VoxelNormal.Up)
					{
						tl = new Vector3SByte(face.Position.x, depth, face.Position.y + face.Scale.y);
						tr = new Vector3SByte(face.Position.x + face.Scale.x, depth, face.Position.y + face.Scale.y);
						bl = new Vector3SByte(face.Position.x, depth, face.Position.y);
						br = new Vector3SByte(face.Position.x + face.Scale.x, depth, face.Position.y);
					}
					else // if (Description.Normal == Vector3SByte.Down)
					{
						tl = new Vector3SByte(face.Position.x, -depth, face.Position.y);
						tr = new Vector3SByte(face.Position.x + face.Scale.x, -depth, face.Position.y);
						bl = new Vector3SByte(face.Position.x, -depth, face.Position.y + face.Scale.y);
						br = new Vector3SByte(face.Position.x + face.Scale.x, -depth, face.Position.y + face.Scale.y);
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
