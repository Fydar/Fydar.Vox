namespace Fydar.Vox.Meshing
{
	public struct GroupedSurfaceFace
	{
		public Vector2SByte Position;
		public FaceCornerFlags Neighbours;

		public Vector2SByte TopLeft => Position + new Vector2SByte(0, 1);
		public Vector2SByte TopRight => Position + new Vector2SByte(1, 1);
		public Vector2SByte BottomRight => Position + new Vector2SByte(1, 0);
		public Vector2SByte BottomLeft => Position;

		public override string ToString()
		{
			return $"(xyz: {Position}, n[{Neighbours}])";
		}
	}
}
