namespace Fydar.Vox.Voxelizer
{
	public struct GreedySurfaceFace
	{
		public Vector2SByte Position;
		public Vector2SByte Scale;

		public Vector2SByte TopLeft => Position + new Vector2SByte(0, Scale.y);
		public Vector2SByte TopRight => Position + new Vector2SByte(Scale.x, Scale.y);
		public Vector2SByte BottomRight => Position + new Vector2SByte(Scale.x, 0);
		public Vector2SByte BottomLeft => Position;

		public bool ConnectsWithY(GreedySurfaceFace other) =>
			TopLeft == other.BottomLeft && 
			TopRight == other.BottomRight;

		public bool ConnectsWithX(GreedySurfaceFace other) =>
			TopLeft == other.BottomLeft && 
			TopRight == other.BottomRight;

		public bool ConnectsWithY(GroupedSurfaceFace other) => 
			TopLeft == other.BottomLeft && 
			TopRight == other.BottomRight;

		public bool ConnectsWithX(GroupedSurfaceFace other) =>
			TopRight == other.TopLeft &&
			BottomRight == other.BottomLeft;

		public override string ToString()
		{
			return $"(pos: {Position}, scale: {Scale})";
		}
	}
}
