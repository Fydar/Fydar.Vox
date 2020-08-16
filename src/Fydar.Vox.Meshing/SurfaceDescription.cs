using System;

namespace Fydar.Vox.Meshing
{
	public struct SurfaceDescription : IEquatable<SurfaceDescription>
	{
		public VoxelNormal Normal;
		public Colour24 Colour;
		public int Depth;

		public override string ToString()
		{
			return $"nrm: {Normal}, col: {Colour}, dep: {Depth}";
		}

		public override bool Equals(object obj)
		{
			return obj is SurfaceDescription description && Equals(description);
		}

		public bool Equals(SurfaceDescription other)
		{
			return Normal.Equals(other.Normal) &&
				   Colour.Equals(other.Colour) &&
				   Depth == other.Depth;
		}

		public override int GetHashCode()
		{
			int hashCode = 837563585;
			hashCode = hashCode * -1521134295 + Normal.GetHashCode();
			hashCode = hashCode * -1521134295 + Colour.GetHashCode();
			hashCode = hashCode * -1521134295 + Depth.GetHashCode();
			return hashCode;
		}

		public static bool operator ==(SurfaceDescription left, SurfaceDescription right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SurfaceDescription left, SurfaceDescription right)
		{
			return !(left == right);
		}
	}
}
